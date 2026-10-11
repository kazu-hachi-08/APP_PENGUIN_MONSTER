using System;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技4つ（タイプの色付き）と「もどる」
    /// </summary>
    public class MovePanel : MonoBehaviour
    {
        [SerializeField] private Button[] _moveButtons;
        [SerializeField] private Text[] _moveLabels;
        [SerializeField] private Button _backButton;

        private Action<int> _onMove;
        private Action _onBack;

        private void Awake()
        {
            for (int i = 0; i < _moveButtons.Length; i++)
            {
                int index = i;
                _moveButtons[i].onClick.AddListener(() => Choose(() => _onMove?.Invoke(index)));
            }
            _backButton.onClick.AddListener(() => Choose(_onBack));
        }

        public void Show(BattleMonster monster, Action<int> onMove, Action onBack)
        {
            _onMove = onMove;
            _onBack = onBack;

            var moves = monster.Definition.Moves;
            for (int i = 0; i < _moveButtons.Length; i++)
            {
                var move = moves[i];
                _moveLabels[i].text = $"{move.Name}\n<size=26>{move.Type.ToJapanese()}  {Detail(move)}</size>";
                _moveButtons[i].image.color = TypeColors.Get(move.Type);
            }
            gameObject.SetActive(true);
        }

        /// <summary>変化技は威力が無いので「へんか」と出す</summary>
        private static string Detail(MoveDefinition move)
        {
            return move.IsStatus ? "へんか" : $"いりょく {move.Power}";
        }

        private void Choose(Action action)
        {
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            action?.Invoke();
        }
    }
}
