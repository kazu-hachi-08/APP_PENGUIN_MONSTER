using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 「たたかう / ペンモン」のコマンド。対戦では逃げるは無い。
    /// ストーリーの窓だけ「バッグ / 逃げる」も持つ
    /// </summary>
    public class CommandPanel : MonoBehaviour
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _partyButton;
        [Tooltip("ストーリーの窓だけ。対戦の窓では空のまま")]
        [SerializeField] private Button _bagButton;
        [SerializeField] private Button _runButton;

        private Action _onFight;
        private Action _onParty;
        private Action _onRun;
        private Action _onBag;

        private void Awake()
        {
            _fightButton.onClick.AddListener(() => Choose(_onFight));
            _partyButton.onClick.AddListener(() => Choose(_onParty));
            if (_runButton != null)
            {
                _runButton.onClick.AddListener(() => Choose(_onRun));
                _bagButton.onClick.AddListener(() => Choose(_onBag));
            }
        }

        /// <param name="reserved">ダブルスでもう1体が交代先に選んだ控え（交代できる子に数えない）</param>
        /// <param name="onRun">逃げる。null なら押せない（トレーナー戦）</param>
        /// <param name="onBag">バッグ。ストーリーの窓だけ</param>
        public void Show(BattleSide side, ICollection<int> reserved, Action onFight, Action onParty, Action onRun = null,
            Action onBag = null)
        {
            _onFight = onFight;
            _onParty = onParty;
            _onRun = onRun;
            _onBag = onBag;
            _partyButton.interactable = CanSwitchAny(side, reserved);
            if (_runButton != null)
            {
                _runButton.interactable = onRun != null;
                _bagButton.interactable = onBag != null;
            }
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
