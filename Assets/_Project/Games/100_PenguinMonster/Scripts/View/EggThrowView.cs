using System;
using System.Collections;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 繰り出すときにタマゴを投げる演出（仕様書 §7）。画面の外から台座へ山なりに飛び、着いたら光って割れる。続きの「ぽよん」は MonsterView。
    /// ペンモンの絵は台座のマスクの中にあって外から飛んでくる絵が切れるので、場全体に重ねたこの層に出す
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
            yield return Burst(style.Flash, to, dot);
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
            _egg.enabled = false;
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
