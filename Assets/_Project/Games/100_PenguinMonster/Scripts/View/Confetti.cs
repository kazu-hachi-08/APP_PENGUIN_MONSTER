using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 勝ったときの紙吹雪。粒（左下基準の RectTransform）を回しながら落とし、下に抜けたら上へ戻す。
    /// 動きは TitleSnow と同じ作りで、色と回転を足している
    /// </summary>
    public class Confetti : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _pieces;
        [SerializeField] private float _minFallSpeed = 220f;
        [SerializeField] private float _maxFallSpeed = 420f;
        [SerializeField] private float _maxSpinSpeed = 360f;
        [SerializeField] private float _swayWidth = 30f;
        [SerializeField] private float _swaySpeed = 3f;

        // ペンモンのロゴと同じ黄・水色・オレンジに、赤と緑を足して賑やかにする
        private static readonly Color[] Colors =
        {
            new Color(1f, 0.85f, 0.3f),
            new Color(0.55f, 0.85f, 1f),
            new Color(1f, 0.6f, 0.25f),
            new Color(1f, 0.45f, 0.5f),
            new Color(0.5f, 0.9f, 0.55f),
        };

        private RectTransform _area;
        private float[] _speeds;
        private float[] _spins;
        private float[] _baseX;
        private float[] _phases;

        private void Awake()
        {
            _area = (RectTransform)transform;
            _speeds = new float[_pieces.Length];
            _spins = new float[_pieces.Length];
            _baseX = new float[_pieces.Length];
            _phases = new float[_pieces.Length];
            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i].GetComponent<Image>().color = Colors[i % Colors.Length];
            }
        }

        private void OnEnable()
        {
            // 画面の上の外にばらまいて、開いた直後に一斉に降ってくるように見せる
            for (int i = 0; i < _pieces.Length; i++)
            {
                Respawn(i, _area.rect.height + Random.Range(0f, _area.rect.height));
            }
        }

        private void Update()
        {
            float time = Time.time;
            for (int i = 0; i < _pieces.Length; i++)
            {
                var position = _pieces[i].anchoredPosition;
                position.y -= _speeds[i] * Time.deltaTime;
                position.x = _baseX[i] + Mathf.Sin(time * _swaySpeed + _phases[i]) * _swayWidth;
                _pieces[i].anchoredPosition = position;
                _pieces[i].Rotate(0f, 0f, _spins[i] * Time.deltaTime);
                if (position.y < -_pieces[i].rect.height)
                {
                    Respawn(i, _area.rect.height);
                }
            }
        }

        private void Respawn(int index, float y)
        {
            _speeds[index] = Random.Range(_minFallSpeed, _maxFallSpeed);
            _spins[index] = Random.Range(-_maxSpinSpeed, _maxSpinSpeed);
            _baseX[index] = Random.Range(0f, _area.rect.width);
            _phases[index] = Random.Range(0f, Mathf.PI * 2f);
            _pieces[index].anchoredPosition = new Vector2(_baseX[index], y);
        }
    }
}
