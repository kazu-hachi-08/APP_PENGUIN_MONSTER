using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 特性が働いたときの帯（「〇〇の / 強面」。仕様書 §7）。陣営ごとに1つ置き、自分側は左から・相手側は右から滑り込む。
    /// 読める間だけ再生を止め、その後は出したまま続きの演出（能力が下がる など）を流して、しばらくして引っ込める。
    /// どの特性が何をしたかを、続く演出と並べて見せるため
    /// </summary>
    public class AbilityBanner : MonoBehaviour
    {
        [SerializeField] private Text _ownerText;
        [SerializeField] private Text _abilityText;
        [Tooltip("滑り込んでくる元の位置（元の位置からのずれ）")]
        [SerializeField] private Vector2 _slideFrom = new Vector2(-300f, 0f);
        [SerializeField] private float _slideSeconds = 0.18f;
        [Tooltip("出てから続きの演出へ進むまでの秒数。特性の名前を読む間")]
        [SerializeField] private float _readSeconds = 0.45f;
        [Tooltip("出てから引っ込み始めるまでの秒数。続きの演出の間も出しておく")]
        [SerializeField] private float _holdSeconds = 1.6f;

        private RectTransform _rect;
        private CanvasGroup _group;
        private Vector2 _home;

        private void Awake()
        {
            _rect = (RectTransform)transform;
            _group = GetComponent<CanvasGroup>();
            // 途中で止めても位置がずれていかないよう、元の位置は最初に1回だけ覚える
            _home = _rect.anchoredPosition;
            Hide();
        }

        /// <summary>帯を出し、読める間だけ待つ。引っ込めるのは帯が自分で行う</summary>
        public IEnumerator Show(string owner, string ability)
        {
            // 続けて働いたとき（強面 → ひねくれ者 など）は、前の帯を引っ込めずに書き換える
            StopAllCoroutines();
            _ownerText.text = owner;
            _abilityText.text = ability;
            BattleSounds.Play(BattleSound.Ability);
            StartCoroutine(Animate());
            yield return new WaitForSeconds(_slideSeconds + _readSeconds);
        }

        public void Hide()
        {
            StopAllCoroutines();
            _group.alpha = 0f;
            _rect.anchoredPosition = _home;
        }

        private IEnumerator Animate()
        {
            yield return Slide(1f, 0f);
            yield return new WaitForSeconds(_holdSeconds - _slideSeconds);
            yield return Slide(0f, 1f);
            _group.alpha = 0f;
        }

        /// <summary>from → to へ滑らせる（0 が元の位置で見えている、1 が _slideFrom にずれて消えている）</summary>
        private IEnumerator Slide(float from, float to)
        {
            for (float elapsed = 0f; elapsed < _slideSeconds; elapsed += Time.deltaTime)
            {
                Place(Mathf.Lerp(from, to, elapsed / _slideSeconds));
                yield return null;
            }
            Place(to);
        }

        private void Place(float away)
        {
            _rect.anchoredPosition = _home + _slideFrom * away;
            _group.alpha = 1f - away;
        }
    }
}
