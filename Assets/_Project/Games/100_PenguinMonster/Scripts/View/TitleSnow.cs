using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// タイトルに降る雪。粒（左下基準の RectTransform）を落として、下に抜けたら上へ戻す
    /// </summary>
    public class TitleSnow : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _flakes;
        [SerializeField] private float _minFallSpeed = 40f;
        [SerializeField] private float _maxFallSpeed = 110f;
        [SerializeField] private float _swayWidth = 20f;
        [SerializeField] private float _swaySpeed = 1.5f;

        private RectTransform _area;
        private float[] _speeds;
        private float[] _baseX;
        private float[] _phases;

        private void Awake()
        {
            _area = (RectTransform)transform;
            _speeds = new float[_flakes.Length];
            _baseX = new float[_flakes.Length];
            _phases = new float[_flakes.Length];
        }

        private void OnEnable()
        {
            // 開いた瞬間から画面じゅうに散らばっているように、高さもばらばらに置く
            for (int i = 0; i < _flakes.Length; i++)
            {
                Respawn(i, Random.Range(0f, _area.rect.height));
            }
        }

        private void Update()
        {
            float time = Time.time;
            for (int i = 0; i < _flakes.Length; i++)
            {
                var position = _flakes[i].anchoredPosition;
                position.y -= _speeds[i] * Time.deltaTime;
                position.x = _baseX[i] + Mathf.Sin(time * _swaySpeed + _phases[i]) * _swayWidth;
                _flakes[i].anchoredPosition = position;
                if (position.y < -_flakes[i].rect.height)
                {
                    Respawn(i, _area.rect.height);
                }
            }
        }

        private void Respawn(int index, float y)
        {
            _speeds[index] = Random.Range(_minFallSpeed, _maxFallSpeed);
            _baseX[index] = Random.Range(0f, _area.rect.width);
            _phases[index] = Random.Range(0f, Mathf.PI * 2f);
            _flakes[index].anchoredPosition = new Vector2(_baseX[index], y);
        }
    }
}
