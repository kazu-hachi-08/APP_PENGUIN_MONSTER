using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// タイトルのロゴ。開くたびに1文字ずつ「ぽよん」と出て、そのあとは文字ごとにずらしてゆっくり上下に揺れる
    /// </summary>
    public class TitleLogo : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _letters;
        [SerializeField] private float _popSeconds = 0.35f;
        [SerializeField] private float _popInterval = 0.12f;
        [SerializeField] private float _bobHeight = 10f;
        [SerializeField] private float _bobSpeed = 2f;
        [Tooltip("隣の文字と揺れをずらす量（ラジアン）。波打って見える")]
        [SerializeField] private float _bobPhase = 0.8f;

        // 少し大きくなってから戻る強さ。MonsterView の登場と同じくらい
        private const float PopOvershoot = 1.7f;

        private Vector2[] _basePositions;
        private float _shownAt;

        private void Awake()
        {
            _basePositions = new Vector2[_letters.Length];
            for (int i = 0; i < _letters.Length; i++)
            {
                _basePositions[i] = _letters[i].anchoredPosition;
            }
        }

        private void OnEnable()
        {
            _shownAt = Time.time;
            Update();
        }

        private void Update()
        {
            float elapsed = Time.time - _shownAt;
            for (int i = 0; i < _letters.Length; i++)
            {
                float pop = Mathf.Clamp01((elapsed - i * _popInterval) / _popSeconds);
                _letters[i].localScale = Vector3.one * (pop <= 0f ? 0f : EaseOutBack(pop));
                float bob = Mathf.Sin(Time.time * _bobSpeed + i * _bobPhase) * _bobHeight;
                _letters[i].anchoredPosition = _basePositions[i] + Vector2.up * bob;
            }
        }

        private static float EaseOutBack(float t)
        {
            float p = t - 1f;
            return 1f + p * p * ((PopOvershoot + 1f) * p + PopOvershoot);
        }
    }
}
