using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// メッセージ窓。文字を1つずつ送って表示する。
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
        [Tooltip("文字が出るたびに小さな音を鳴らす。フィールドの会話窓だけ（バトルは効果音が多いので鳴らさない）")]
        [SerializeField] private bool _typingSound;
        [Tooltip("文字の音を鳴らす間隔（文字数）。1文字ごとだと速すぎてうるさい")]
        [SerializeField] private int _charsPerTypingSound = 2;

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

        /// <summary>フィールドの A ボタンなど、タップ以外で送る</summary>
        public void RequestAdvance()
        {
            _advanceRequested = true;
        }

        /// <summary>送り表示だけして待たない。「はい / いいえ」のように、続けて選択肢を出す文に使う</summary>
        public IEnumerator Type(string message)
        {
            _nextIndicator.SetActive(false);
            _advanceRequested = false;
            float shown = 0f;
            // 設定画面で変えた速さをすぐ反映するよう、1文ごとに読む
            float charsPerSecond = _charsPerSecond * GameSettings.SpeedRate;
            int nextSoundAt = 0;
            while (shown < message.Length && !_advanceRequested)
            {
                shown += charsPerSecond * Time.deltaTime;
                int count = Mathf.Min(message.Length, Mathf.FloorToInt(shown));
                _text.text = message.Substring(0, count);
                if (_typingSound && count > nextSoundAt)
                {
                    PlayTypingSound(message[count - 1]);
                    nextSoundAt = count + _charsPerTypingSound - 1;
                }
                yield return null;
            }
            _text.text = message;
        }

        private static void PlayTypingSound(char shownChar)
        {
            // 文の区切りの空白・改行は声に出さないので鳴らさない
            if (!char.IsWhiteSpace(shownChar))
            {
                BattleSounds.Play(BattleSound.Talk);
            }
        }

        /// <summary>文はそのまま、タップされるまで待つ（レベルアップの窓を次の段へ送るときなど）</summary>
        public IEnumerator WaitForTap()
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
