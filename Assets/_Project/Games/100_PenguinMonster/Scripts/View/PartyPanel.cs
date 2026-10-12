using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 交代先を選ぶ画面。自分で交代するとき（もどれる）と、ひんしで出し直すとき（もどれない）に使う。
    /// 枠はダブルスの4体ぶん作ってあり、シングルでは余った枠を隠す
    /// </summary>
    public class PartyPanel : MonoBehaviour
    {
        [SerializeField] private Button[] _slotButtons;
        [SerializeField] private Text[] _slotLabels;
        [SerializeField] private Image[] _slotIcons;
        [SerializeField] private Button _backButton;

        private Action<int> _onSelect;
        private Action _onBack;

        private void Awake()
        {
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                int index = i;
                _slotButtons[i].onClick.AddListener(() => Select(index));
            }
            _backButton.onClick.AddListener(Back);
        }

        /// <param name="onBack">null ならもどれない（ひんしの入れ替え）</param>
        /// <param name="reserved">ダブルスで同じターンにもう1体が選んだ子（選べなくする）</param>
        public void Show(BattleSide side, Action<int> onSelect, Action onBack, ICollection<int> reserved = null)
        {
            _onSelect = onSelect;
            _onBack = onBack;

            for (int i = 0; i < _slotButtons.Length; i++)
            {
                bool exists = i < side.Party.Count;
                _slotButtons[i].gameObject.SetActive(exists);
                if (!exists)
                {
                    continue;
                }
                var monster = side.Party[i];
                bool isReserved = reserved != null && reserved.Contains(i);
                string state = monster.IsFainted ? "瀕死" : side.IsActive(i) ? "戦闘中" : isReserved ? "交代先" : $"HP {monster.CurrentHp}/{monster.MaxHp}";
                string status = monster.IsFainted ? "" : monster.Status.ToJapanese();
                _slotLabels[i].text = $"{monster.Name}   {state}  {status}";
                _slotIcons[i].sprite = MonsterSprites.Front(monster.Definition);
                _slotButtons[i].interactable = side.CanSwitchTo(i) && !isReserved;
            }
            _backButton.gameObject.SetActive(onBack != null);
            gameObject.SetActive(true);
        }

        private void Select(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            _onSelect?.Invoke(index);
        }

        private void Back()
        {
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            _onBack?.Invoke();
        }
    }
}
