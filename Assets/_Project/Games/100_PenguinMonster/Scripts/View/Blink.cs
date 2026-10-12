using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>「タップで 次へ」のような案内をゆっくり点滅させ、押せる所だと気づかせる</summary>
    [RequireComponent(typeof(Graphic))]
    public class Blink : MonoBehaviour
    {
        [SerializeField] private float _cyclesPerSecond = 0.8f;
        [SerializeField] private float _minAlpha = 0.25f;

        private Graphic _graphic;

        private void Awake() => _graphic = GetComponent<Graphic>();

        private void Update()
        {
            float wave = (Mathf.Sin(Time.time * _cyclesPerSecond * Mathf.PI * 2f) + 1f) / 2f;
            var color = _graphic.color;
            color.a = Mathf.Lerp(_minAlpha, 1f, wave);
            _graphic.color = color;
        }
    }
}
