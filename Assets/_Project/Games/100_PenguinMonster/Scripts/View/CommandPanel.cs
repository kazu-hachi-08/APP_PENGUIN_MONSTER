using System;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 「たたかう / ペンモン」のコマンド。にげるは無い（対戦ゲームなので）
    /// </summary>
    public class CommandPanel : MonoBehaviour
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _partyButton;

        private Action _onFight;
        private Action _onParty;

        private void Awake()
        {
            _fightButton.onClick.AddListener(() => Choose(_onFight));
            _partyButton.onClick.AddListener(() => Choose(_onParty));
        }

        public void Show(BattleSide side, Action onFight, Action onParty)
        {
            _onFight = onFight;
            _onParty = onParty;
            _partyButton.interactable = CanSwitchAny(side);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Choose(Action action)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            action?.Invoke();
        }

        private static bool CanSwitchAny(BattleSide side)
        {
            for (int i = 0; i < side.Party.Count; i++)
            {
                if (side.CanSwitchTo(i))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
