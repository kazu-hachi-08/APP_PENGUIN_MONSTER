using System;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 交代先を選ぶ画面。自分で交代するとき（もどれる）と、ひんしで出し直すとき（もどれない）に使う
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
        public void Show(BattleSide side, Action<int> onSelect, Action onBack)
        {
            _onSelect = onSelect;
            _onBack = onBack;

            for (int i = 0; i < _slotButtons.Length; i++)
            {
                var monster = side.Party[i];
                string state = monster.IsFainted ? "瀕死" : side.IsActive(i) ? "戦闘中" : $"HP {monster.CurrentHp}/{monster.MaxHp}";
                string status = monster.IsFainted ? "" : monster.Status.ToJapanese();
                _slotLabels[i].text = $"{monster.Name}   {state}  {status}";
                _slotIcons[i].sprite = MonsterSprites.Front(monster.Definition);
                _slotButtons[i].interactable = side.CanSwitchTo(i);
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
