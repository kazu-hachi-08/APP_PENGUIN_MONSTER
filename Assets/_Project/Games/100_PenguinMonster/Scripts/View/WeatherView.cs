using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 場の天候の見た目。場全体に色を薄く重ね、吹雪は雪の粒・大雨は雨の筋を降らせる。
    /// 日差しは粒を出さず、色をゆっくり明滅させて照りつける感じを出す。粒は左下基準の RectTransform
    /// </summary>
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private Image _tint;
        [SerializeField] private Image[] _particles;

        [Header("吹雪")]
        [SerializeField] private Color _snowTint = new Color(0.85f, 0.92f, 1f, 0.25f);
        [SerializeField] private Color _snowColor = new Color(1f, 1f, 1f, 0.9f);
        [SerializeField] private Vector2 _snowSize = new Vector2(12f, 12f);
        [SerializeField] private Vector2 _snowFallSpeed = new Vector2(200f, 380f);
        [SerializeField] private float _snowDrift = -160f;

        [Header("大雨")]
        [SerializeField] private Color _rainTint = new Color(0.15f, 0.2f, 0.4f, 0.3f);
        [SerializeField] private Color _rainColor = new Color(0.75f, 0.85f, 1f, 0.7f);
        [SerializeField] private Vector2 _rainSize = new Vector2(4f, 44f);
        [SerializeField] private Vector2 _rainFallSpeed = new Vector2(1100f, 1500f);
        [SerializeField] private float _rainDrift = -120f;

        [Header("日差し")]
        [SerializeField] private Color _sunTint = new Color(1f, 0.8f, 0.35f, 0.2f);
        [Tooltip("日差しの色の明滅の幅（不透明度）と速さ")]
        [SerializeField] private float _sunPulseAlpha = 0.08f;
        [SerializeField] private float _sunPulseSpeed = 1.5f;

        private RectTransform _area;
        private float[] _speeds;
        private Weather _weather;
        private float _drift;

        private void Awake()
        {
            _area = (RectTransform)transform;
            _speeds = new float[_particles.Length];
            Hide();
        }

        public void Show(Weather weather)
        {
            if (weather == Weather.None)
            {
                Hide();
                return;
            }
            _weather = weather;
            _tint.enabled = true;
            _tint.color = TintOf(weather);
            bool falls = weather != Weather.Sun;
            _drift = weather == Weather.Snow ? _snowDrift : _rainDrift;
            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i].enabled = falls;
                if (falls)
                {
                    SetUpParticle(i);
                    // 始まった瞬間から画面じゅうに降っているよう、高さもばらばらに置く
                    Respawn(i, Random.Range(0f, _area.rect.height));
                }
            }
        }

        public void Hide()
        {
            _weather = Weather.None;
            _tint.enabled = false;
            foreach (var particle in _particles)
            {
                particle.enabled = false;
            }
        }

        private void Update()
        {
            if (_weather == Weather.Sun)
            {
                var color = _sunTint;
                color.a += Mathf.Sin(Time.time * _sunPulseSpeed) * _sunPulseAlpha;
                _tint.color = color;
            }
            else if (_weather != Weather.None)
            {
                MoveParticles();
            }
        }

        private void MoveParticles()
        {
            float width = _area.rect.width;
            for (int i = 0; i < _particles.Length; i++)
            {
                var rect = _particles[i].rectTransform;
                var position = rect.anchoredPosition;
                position.y -= _speeds[i] * Time.deltaTime;
                // 横に流れて端から出たら反対の端へ回す（斜めに吹きつける感じ）
                position.x = Mathf.Repeat(position.x + _drift * Time.deltaTime, width);
                rect.anchoredPosition = position;
                if (position.y < -rect.rect.height)
                {
                    Respawn(i, _area.rect.height);
                }
            }
        }

        private void SetUpParticle(int index)
        {
            bool snow = _weather == Weather.Snow;
            _particles[index].color = snow ? _snowColor : _rainColor;
            var rect = _particles[index].rectTransform;
            rect.sizeDelta = snow ? _snowSize : _rainSize;
            // 雨の筋は流れる向きに少し傾ける（左へ流れるなら上端が右に来るよう、時計回り）
            float tilt = snow ? 0f : Mathf.Atan2(_rainDrift, _rainFallSpeed.x) * Mathf.Rad2Deg;
            rect.localRotation = Quaternion.Euler(0f, 0f, tilt);
        }

        private void Respawn(int index, float y)
        {
            var range = _weather == Weather.Snow ? _snowFallSpeed : _rainFallSpeed;
            _speeds[index] = Random.Range(range.x, range.y);
            _particles[index].rectTransform.anchoredPosition = new Vector2(Random.Range(0f, _area.rect.width), y);
        }

        private Color TintOf(Weather weather)
        {
            switch (weather)
            {
                case Weather.Snow: return _snowTint;
                case Weather.Rain: return _rainTint;
                default: return _sunTint;
            }
        }
    }
}
