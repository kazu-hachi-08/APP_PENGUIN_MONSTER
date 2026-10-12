using System;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ダブルスで、1体だけに当たる技の狙う相手を選ぶ（仕様書 §4.9）。相手の場の左右に1つずつボタンを出し、その相手への相性も添える
    /// </summary>
    public class TargetPanel : MonoBehaviour
    {
        [Tooltip("相手の場の順（0 が左）")]
        [SerializeField] private Button[] _targetButtons;
        [SerializeField] private Text[] _targetLabels;
        [SerializeField] private Button _backButton;

        private Action<int> _onSelect;
        private Action _onBack;

        private void Awake()
        {
            for (int i = 0; i < _targetButtons.Length; i++)
            {
                int slot = i;
                _targetButtons[i].onClick.AddListener(() => Choose(() => _onSelect?.Invoke(slot)));
            }
            _backButton.onClick.AddListener(() => Choose(_onBack));
        }

        public void Show(BattleSide foe, MoveDefinition move, Action<int> onSelect, Action onBack)
        {
            _onSelect = onSelect;
            _onBack = onBack;
            for (int slot = 0; slot < _targetButtons.Length; slot++)
            {
                var monster = foe.ActiveAt(slot);
                string tag = MovePanel.EffectivenessTag(move.Effectiveness(monster.Definition.Types));
                _targetLabels[slot].text = $"相手の {monster.Name}\n<size=30>{tag.Trim()}</size>";
                _targetButtons[slot].interactable = !monster.IsFainted;
            }
            gameObject.SetActive(true);
        }

        private void Choose(Action action)
        {
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            action?.Invoke();
        }
    }
}
