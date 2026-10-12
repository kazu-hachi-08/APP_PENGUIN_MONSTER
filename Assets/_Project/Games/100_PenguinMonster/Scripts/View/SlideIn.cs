using System.Collections;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>画面の外（元の位置 + _from）から元の位置へ滑り込ませる。勢いよく入って、ゆっくり止まる</summary>
    public class SlideIn : MonoBehaviour
    {
        [SerializeField] private Vector2 _from;
        [SerializeField] private float _seconds = 0.35f;

        private RectTransform _rect;
        private Vector2 _home;

        private void Awake()
        {
            _rect = (RectTransform)transform;
            // 途中で止まっても位置がずれていかないよう、元の位置は最初に1回だけ覚える
            _home = _rect.anchoredPosition;
        }

        public void Play(float delay = 0f)
        {
            StopAllCoroutines();
            _rect.anchoredPosition = _home + _from;
            StartCoroutine(Animate(delay));
        }

        private IEnumerator Animate(float delay)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            for (float elapsed = 0f; elapsed < _seconds; elapsed += Time.deltaTime)
            {
                float rest = 1f - elapsed / _seconds;
                _rect.anchoredPosition = _home + _from * (rest * rest * rest);
                yield return null;
            }
            _rect.anchoredPosition = _home;
        }

        private void OnDisable()
        {
            if (_rect != null) _rect.anchoredPosition = _home;
        }
    }
}
