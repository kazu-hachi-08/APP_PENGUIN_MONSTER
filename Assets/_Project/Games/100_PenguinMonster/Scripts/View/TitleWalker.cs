using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// タイトルの雪原を横切るペンモン。ぴょこぴょこ跳ねて左右に揺れながら歩き、
    /// 画面の外へ出たら、少し待ってから別のペンモンで左右どちらかから入り直す
    /// </summary>
    public class TitleWalker : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _minSpeed = 120f;
        [SerializeField] private float _maxSpeed = 220f;
        [SerializeField] private float _hopHeight = 24f;
        [SerializeField] private float _hopsPerSecond = 2.5f;
        [SerializeField] private float _waddleAngle = 8f;
        [SerializeField] private float _maxWait = 3f;

        private RectTransform _rect;
        private float _groundY;
        private float _direction;
        private float _speed;
        private float _wait;
        private float _walkTime;

        private void Awake()
        {
            _rect = _image.rectTransform;
            _groundY = _rect.anchoredPosition.y;
        }

        private void OnEnable() => Restart();

        private void Update()
        {
            if (_wait > 0f)
            {
                _wait -= Time.deltaTime;
                return;
            }

            _walkTime += Time.deltaTime;
            float hop = Mathf.Abs(Mathf.Sin(_walkTime * _hopsPerSecond * Mathf.PI));
            float x = _rect.anchoredPosition.x + _direction * _speed * Time.deltaTime;
            _rect.anchoredPosition = new Vector2(x, _groundY + hop * _hopHeight);
            // 跳ねるたびに左右へ傾けて、ペンギンのよちよち歩きに見せる
            _rect.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(_walkTime * _hopsPerSecond * Mathf.PI) * _waddleAngle);

            if (Mathf.Abs(x) > EdgeX)
            {
                Restart();
            }
        }

        /// <summary>画面の端より絵1枚ぶん外。ここを越えたら見えなくなっている</summary>
        private float EdgeX => ((RectTransform)_rect.parent).rect.width / 2f + _rect.rect.width;

        private void Restart()
        {
            var all = MonsterTable.All;
            _image.sprite = MonsterSprites.Front(all[Random.Range(0, all.Count)]);
            _direction = Random.value < 0.5f ? 1f : -1f;
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _wait = Random.Range(0f, _maxWait);
            _walkTime = 0f;
            _rect.anchoredPosition = new Vector2(-_direction * EdgeX, _groundY);
            _rect.localEulerAngles = Vector3.zero;
        }
    }
}
