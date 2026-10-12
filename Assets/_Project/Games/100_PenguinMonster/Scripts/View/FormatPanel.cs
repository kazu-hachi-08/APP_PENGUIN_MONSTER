using System;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// シングルかダブルスかを選ぶ画面（仕様書 §2 / §4.9）。勝ち抜き戦はドラフトの前、オンラインはホストだけが接続の後に選ぶ
    /// </summary>
    public class FormatPanel : MonoBehaviour
    {
        [SerializeField] private Button _singleButton;
        [SerializeField] private Button _doubleButton;
        [SerializeField] private Button _backButton;

        private Action<BattleFormat> _onChosen;
        private Action _onBack;

        private void Awake()
        {
            _singleButton.onClick.AddListener(() => Choose(BattleFormat.Single));
            _doubleButton.onClick.AddListener(() => Choose(BattleFormat.Double));
            _backButton.onClick.AddListener(Back);
        }

        public void Show(Action<BattleFormat> onChosen, Action onBack)
        {
            _onChosen = onChosen;
            _onBack = onBack;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Choose(BattleFormat format)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onChosen?.Invoke(format);
        }

        private void Back()
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onBack?.Invoke();
        }
    }
}
