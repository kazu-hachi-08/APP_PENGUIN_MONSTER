using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// メッセージ窓。1文ずつ表示し、一定時間たつかタップで次へ進む
    /// </summary>
    public class MessageWindow : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Button _advanceButton;
        [SerializeField] private float _autoAdvanceSeconds = 1.2f;

        private bool _advanceRequested;

        private void Awake()
        {
            _advanceButton.onClick.AddListener(() => _advanceRequested = true);
        }

        /// <summary>入力待ちの案内など、送らずに出しておく文</summary>
        public void SetText(string message)
        {
            _text.text = message;
        }

        public IEnumerator Play(string message)
        {
            _text.text = message;
            _advanceRequested = false;
            float elapsed = 0f;
            while (!_advanceRequested && elapsed < _autoAdvanceSeconds)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
