using System;
using System.Collections;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 繰り出すときにタマゴを投げる演出。画面の外から台座へ山なりに飛び、着いたら光って割れる。続きの「ぽよん」は MonsterView。
    /// ペンモンの絵は台座のマスクの中にあって外から飛んでくる絵が切れるので、場全体に重ねたこの層に出す。
    /// ストーリーの捕獲タマゴも同じ絵で、投げたタマゴを相手の所に残して揺らす
    /// </summary>
    public class EggThrowView : MonoBehaviour
    {
        [SerializeField] private Image _egg;
        [Tooltip("割れたときの光")]
        [SerializeField] private Image _burst;
        [Tooltip("自分側のタマゴが飛び出す所（この層の座標）。手前の左下、画面の外")]
        [SerializeField] private Vector2 _playerFrom = new Vector2(-1100f, -350f);
        [Tooltip("相手側のタマゴが飛び出す所。奥の右上、画面の外")]
        [SerializeField] private Vector2 _enemyFrom = new Vector2(1100f, 450f);
        [SerializeField] private float _flightSeconds = 0.5f;
        [SerializeField] private float _arcHeight = 280f;
        [Tooltip("飛んでいる間に回る回数。ドット絵なのでなめらかに回さず 90 度ずつ切り替える")]
        [SerializeField] private int _spinSteps = 6;
        [SerializeField] private float _burstSeconds = 0.25f;
        [SerializeField] private float _burstScale = 5f;
        [Header("捕獲タマゴ")]
        [Tooltip("1回の揺れで傾ける角度。ドット絵なので 2段で切り替える")]
        [SerializeField] private float _shakeAngle = 20f;
        [SerializeField] private float _shakeStepSeconds = 0.12f;
        [Tooltip("揺れと揺れの間")]
        [SerializeField] private float _shakePauseSeconds = 0.5f;

        // ペンモンの絵は 64 ドット。タマゴも同じドットの大きさで描き、絵と並べても浮かないようにする
        private const float MonsterPixels = 64f;
        private const float SpinAngle = 90f;

        private RectTransform Layer => (RectTransform)transform;

        private void Awake()
        {
            _egg.enabled = false;
            _burst.enabled = false;
        }

        /// <param name="target">出てくるペンモンの絵。大きさ（シングル / ダブルス、手前 / 奥）に合わせてタマゴも大きくする</param>
        public IEnumerator Throw(EggKind kind, RectTransform target, bool fromPlayer)
        {
            var style = EggStyleTable.Get(kind);
            float dot = target.rect.height / MonsterPixels;
            Vector2 to = Layer.InverseTransformPoint(target.TransformPoint(target.rect.center));
            yield return Fly(kind, fromPlayer ? _playerFrom : _enemyFrom, to, dot, fromPlayer ? -1f : 1f);
            _egg.enabled = false;
            yield return Burst(style.Flash, to, dot);
        }

        /// <summary>
        /// 野生へ捕獲タマゴを投げる。着いたら光り、タマゴはその場に残る
        /// </summary>
        /// <param name="onLanded">着いた瞬間に呼ぶ（光っている間に野生の絵を消して、吸い込まれたように見せる）</param>
        public IEnumerator ThrowToCapture(EggKind kind, RectTransform target, Action onLanded)
        {
            float dot = target.rect.height / MonsterPixels;
            Vector2 to = Layer.InverseTransformPoint(target.TransformPoint(target.rect.center));
            yield return Fly(kind, _playerFrom, to, dot, -1f);
            _egg.rectTransform.localRotation = Quaternion.identity;
            onLanded();
            yield return Burst(EggStyleTable.Get(kind).Flash, to, dot);
        }

        /// <summary>残したタマゴを左右に1回揺らし、少し待つ</summary>
        public IEnumerator Shake()
        {
            var egg = _egg.rectTransform;
            foreach (float angle in new[] { -_shakeAngle, _shakeAngle, 0f })
            {
                egg.localRotation = Quaternion.Euler(0f, 0f, angle);
                yield return new WaitForSeconds(_shakeStepSeconds);
            }
            yield return new WaitForSeconds(_shakePauseSeconds);
        }

        /// <summary>外れた: タマゴが光って割れ、中の子が出てくる（絵を戻すのは呼ぶ側）</summary>
        public IEnumerator BreakOut(EggKind kind)
        {
            _egg.enabled = false;
            float dot = _egg.rectTransform.sizeDelta.y / EggSprites.Size;
            yield return Burst(EggStyleTable.Get(kind).Flash, _egg.rectTransform.localPosition, dot);
        }

        /// <summary>捕まえた後、残したタマゴを片付ける</summary>
        public void HideEgg()
        {
            _egg.enabled = false;
        }

        private IEnumerator Fly(EggKind kind, Vector2 from, Vector2 to, float dot, float spinSign)
        {
            _egg.sprite = EggSprites.Get(kind);
            _egg.rectTransform.sizeDelta = Vector2.one * EggSprites.Size * dot;
            _egg.enabled = true;
            yield return Tween(_flightSeconds, t =>
            {
                float lift = _arcHeight * 4f * t * (1f - t);
                _egg.rectTransform.localPosition = Vector2.Lerp(from, to, t) + Vector2.up * lift;
                _egg.rectTransform.localRotation = Quaternion.Euler(0f, 0f, spinSign * SpinAngle * Mathf.Floor(t * _spinSteps));
            });
        }

        /// <summary>タマゴの色の光が広がりながら消える。光の絵はオーロラの粒（白の十字）を使い回す</summary>
        private IEnumerator Burst(Color flash, Vector2 at, float dot)
        {
            _burst.sprite = MoveEffectSprites.Get(MonsterType.Aurora);
            _burst.rectTransform.sizeDelta = Vector2.one * MoveEffectSprites.Size * dot;
            _burst.rectTransform.localPosition = at;
            _burst.enabled = true;
            BattleSounds.Play(BattleSound.SendOut);
            yield return Tween(_burstSeconds, t =>
            {
                _burst.rectTransform.localScale = Vector3.one * Mathf.Lerp(1f, _burstScale, t);
                _burst.color = new Color(flash.r, flash.g, flash.b, 1f - t);
            });
            _burst.enabled = false;
        }

        private static IEnumerator Tween(float seconds, Action<float> step)
        {
            for (float elapsed = 0f; elapsed < seconds; elapsed += Time.deltaTime)
            {
                step(elapsed / seconds);
                yield return null;
            }
            step(1f);
        }
    }
}
