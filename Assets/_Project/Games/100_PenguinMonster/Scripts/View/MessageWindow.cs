using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// メッセージ窓。文字を1つずつ送って表示する（仕様書 §7）。
    /// 送り中にタップすると全文を出し、読み終わったら▼を点滅させてタップを待つ
    /// </summary>
    public class MessageWindow : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [Tooltip("タップで送れる範囲。窓のほか、画面上の空いたところもタップで送れるようにする")]
        [SerializeField] private Button[] _advanceButtons;
        [SerializeField] private GameObject _nextIndicator;
        [SerializeField] private float _charsPerSecond = 40f;
        [Tooltip("タップを待たない文（「〇〇の △△！」など）を、読み終わってから残しておく秒数")]
        [SerializeField] private float _holdSeconds = 0.5f;
        [SerializeField] private float _indicatorBlinkSeconds = 0.4f;

        private bool _advanceRequested;

        private void Awake()
        {
            foreach (var button in _advanceButtons)
            {
                button.onClick.AddListener(() => _advanceRequested = true);
            }
            _nextIndicator.SetActive(false);
        }

        /// <summary>入力待ちの案内など、送らずにすぐ出しておく文</summary>
        public void SetText(string message)
        {
            _text.text = message;
            _nextIndicator.SetActive(false);
        }

        /// <summary>送り表示して、タップされるまで待つ</summary>
        public IEnumerator Play(string message)
        {
            yield return Type(message);
            yield return WaitForTap();
        }

        /// <summary>送り表示して、少しだけ残したら先へ進む。続けて演出を見せたい文に使う</summary>
        public IEnumerator Show(string message)
        {
            yield return Type(message);
            _advanceRequested = false;
            for (float elapsed = 0f; elapsed < _holdSeconds && !_advanceRequested; elapsed += Time.deltaTime)
            {
                yield return null;
            }
        }

        private IEnumerator Type(string message)
        {
            _nextIndicator.SetActive(false);
            _advanceRequested = false;
            float shown = 0f;
            while (shown < message.Length && !_advanceRequested)
            {
                shown += _charsPerSecond * Time.deltaTime;
                _text.text = message.Substring(0, Mathf.Min(message.Length, Mathf.FloorToInt(shown)));
                yield return null;
            }
            _text.text = message;
        }

        private IEnumerator WaitForTap()
        {
            _advanceRequested = false;
            float elapsed = 0f;
            while (!_advanceRequested)
            {
                elapsed += Time.deltaTime;
                _nextIndicator.SetActive(Mathf.FloorToInt(elapsed / _indicatorBlinkSeconds) % 2 == 0);
                yield return null;
            }
            _nextIndicator.SetActive(false);
        }
    }
}
