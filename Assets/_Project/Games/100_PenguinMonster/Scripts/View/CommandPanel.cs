using System;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技4つ＋「交代」ボタン
    /// </summary>
    public class CommandPanel : MonoBehaviour
    {
        [SerializeField] private Button[] _moveButtons;
        [SerializeField] private Text[] _moveLabels;
        [SerializeField] private Button _switchButton;

        private Action<int> _onMove;
        private Action _onSwitch;

        private void Awake()
        {
            for (int i = 0; i < _moveButtons.Length; i++)
            {
                int index = i;
                _moveButtons[i].onClick.AddListener(() => _onMove?.Invoke(index));
            }
            _switchButton.onClick.AddListener(() => _onSwitch?.Invoke());
        }

        public void Show(BattleSide side, Action<int> onMove, Action onSwitch)
        {
            _onMove = onMove;
            _onSwitch = onSwitch;

            var moves = side.Active.Definition.Moves;
            for (int i = 0; i < _moveButtons.Length; i++)
            {
                var move = moves[i];
                _moveLabels[i].text = $"{move.Name}\n<size=22>{move.Type.ToJapanese()}  威力{move.Power}</size>";
                _moveButtons[i].image.color = TypeColors.Get(move.Type);
            }
            _switchButton.interactable = CanSwitchAny(side);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
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
