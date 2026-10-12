using System;
using System.Collections.Generic;
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

        /// <param name="reserved">ダブルスでもう1体が交代先に選んだ控え（交代できる子に数えない）</param>
        public void Show(BattleSide side, ICollection<int> reserved, Action onFight, Action onParty)
        {
            _onFight = onFight;
            _onParty = onParty;
            _partyButton.interactable = CanSwitchAny(side, reserved);
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

        private static bool CanSwitchAny(BattleSide side, ICollection<int> reserved)
        {
            for (int i = 0; i < side.Party.Count; i++)
            {
                if (side.CanSwitchTo(i) && !reserved.Contains(i))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
