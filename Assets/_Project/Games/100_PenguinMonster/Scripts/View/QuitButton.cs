using System;
using PenguinMonster.Common.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ドラフト〜バトルの間、右上に出す「やめる」。押したら確認を出し、「やめる」でその回を終わらせる。
    /// にげるは無いので、遊ぶのをやめたいときの出口はここだけ
    /// </summary>
    public class QuitButton : MonoBehaviour
    {
        private const string ConfirmTitle = "戦いを やめますか？";
        private const string ConfirmText = "やめる";
        private const string CancelText = "続ける";

        [SerializeField] private Button _button;
        [SerializeField] private CommonDialog _dialog;

        private string _message;
        private Action _onQuit;

        private void Awake()
        {
            _button.onClick.AddListener(Confirm);
        }

        public void Show(string message, Action onQuit)
        {
            _message = message;
            _onQuit = onQuit;
            gameObject.SetActive(true);
        }

        /// <summary>確認を開いたまま決着することもあるので、確認も一緒に閉じる</summary>
        public void Hide()
        {
            _dialog.Hide();
            gameObject.SetActive(false);
        }

        private void Confirm()
        {
            BattleSounds.Play(BattleSound.Cursor);
            _dialog.Show(ConfirmTitle, _message, onConfirm: _onQuit, confirmText: ConfirmText, cancelText: CancelText);
        }
    }
}
