using System;
using PenguinMonster.Common.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 記録があるときに「ストーリー」で出す「続きから / 最初から」（PLAN_STORY_TOWN01.md フェーズ7）。
    /// 最初からは確かめる。前の記録は、次に記録したときに上書きされるまで残す（本家と同じ。ここでは消さない）
    /// </summary>
    public class StoryStartPanel : MonoBehaviour
    {
        private const string NewGameTitle = "最初から 始めますか？";
        private const string NewGameMessage = "前の 記録は\n次に 記録したときに 消えます";
        private const string NewGameConfirm = "最初から";
        private const string NewGameCancel = "やめる";

        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private CommonDialog _dialog;

        private Action<bool> _onChosen;
        private Action _onBack;

        private void Awake()
        {
            _continueButton.onClick.AddListener(() => Choose(true));
            _newGameButton.onClick.AddListener(ConfirmNewGame);
            _backButton.onClick.AddListener(Back);
        }

        /// <param name="onChosen">続きからなら true、最初からなら false</param>
        public void Show(Action<bool> onChosen, Action onBack)
        {
            _onChosen = onChosen;
            _onBack = onBack;
            gameObject.SetActive(true);
        }

        private void ConfirmNewGame()
        {
            BattleSounds.Play(BattleSound.Cursor);
            _dialog.Show(NewGameTitle, NewGameMessage, onConfirm: () => Choose(false), confirmText: NewGameConfirm, cancelText: NewGameCancel);
        }

        private void Choose(bool continueSaved)
        {
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            _onChosen?.Invoke(continueSaved);
        }

        private void Back()
        {
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            _onBack?.Invoke();
        }
    }
}
