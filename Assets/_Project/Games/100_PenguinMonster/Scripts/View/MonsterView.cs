using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 台座の上に立つペンモンの絵と、その演出（出てくる・もどる・攻撃・点滅・ひんし）。
    /// 親に RectMask2D を付けて台座の高さで切り、ひんしで「下に沈んで消える」ように見せる
    /// </summary>
    public class MonsterView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [Tooltip("攻撃で踏み込む向き（相手のいる方）")]
        [SerializeField] private Vector2 _lungeDirection = new Vector2(1f, 0.5f);
        [SerializeField] private float _lungeDistance = 60f;
        [SerializeField] private float _lungeSeconds = 0.18f;
        [SerializeField] private float _appearSeconds = 0.3f;
        [SerializeField] private float _withdrawSeconds = 0.25f;
        [SerializeField] private float _blinkInterval = 0.07f;
        [SerializeField] private int _blinkCount = 3;
        [SerializeField] private float _faintSeconds = 0.45f;

        // 出てくるときに少し大きくなってから戻る「ぽよん」の強さ
        private const float AppearOvershoot = 1.7f;

        private RectTransform Rect => _image.rectTransform;

        private void Awake()
        {
            _image.enabled = false;
        }

        public IEnumerator Appear(Sprite sprite)
        {
            _image.sprite = sprite;
            Rect.anchoredPosition = Vector2.zero;
            _image.enabled = true;
            yield return Tween(_appearSeconds, t => Rect.localScale = Vector3.one * EaseOutBack(t));
            Rect.localScale = Vector3.one;
        }

        public IEnumerator Withdraw()
        {
            yield return Tween(_withdrawSeconds, t => Rect.localScale = Vector3.one * (1f - t));
            Hide();
        }

        /// <summary>相手の方へ踏み込んで戻る</summary>
        public IEnumerator Lunge()
        {
            var offset = _lungeDirection.normalized * _lungeDistance;
            yield return Tween(_lungeSeconds, t => Rect.anchoredPosition = offset * Mathf.Sin(t * Mathf.PI));
            Rect.anchoredPosition = Vector2.zero;
        }

        public IEnumerator Blink()
        {
            for (int i = 0; i < _blinkCount * 2; i++)
            {
                _image.enabled = !_image.enabled;
                yield return new WaitForSeconds(_blinkInterval);
            }
            _image.enabled = true;
        }

        /// <summary>絵の高さぶん下へ沈める。台座より下はマスクで見えないので、地面に消えていくように見える</summary>
        public IEnumerator Faint()
        {
            float height = Rect.rect.height;
            yield return Tween(_faintSeconds, t => Rect.anchoredPosition = new Vector2(0f, -height * t * t));
            Hide();
        }

        private void Hide()
        {
            _image.enabled = false;
            Rect.anchoredPosition = Vector2.zero;
            Rect.localScale = Vector3.one;
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

        private static float EaseOutBack(float t)
        {
            float p = t - 1f;
            return 1f + p * p * ((AppearOvershoot + 1f) * p + AppearOvershoot);
        }
    }
}
