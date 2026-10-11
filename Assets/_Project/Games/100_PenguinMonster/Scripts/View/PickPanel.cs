using System;
using System.Collections.Generic;
using System.Linq;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 仮の選択画面（Phase 3）。全ペンモンから3体を選び、選んだ順が選出の順（1番目が先頭）になる。
    /// もう一度押すと外せる。Phase 4 でドラフト・選出の画面に置き換える
    /// </summary>
    public class PickPanel : MonoBehaviour
    {
        [SerializeField] private Text _titleText;
        [SerializeField] private Button[] _cellButtons;
        [SerializeField] private Image[] _cellIcons;
        [SerializeField] private Text[] _cellLabels;
        [SerializeField] private Text[] _cellOrders;
        [SerializeField] private Button _randomButton;
        [SerializeField] private Button _decideButton;
        [SerializeField] private Color _cellColor = new Color(0.3f, 0.5f, 0.8f);
        [SerializeField] private Color _pickedColor = new Color(0.95f, 0.6f, 0.2f);

        private readonly List<int> _picked = new List<int>();
        private IReadOnlyList<MonsterDefinition> _monsters;
        private string _title;
        private Action<List<MonsterDefinition>> _onDecide;

        private void Awake()
        {
            for (int i = 0; i < _cellButtons.Length; i++)
            {
                int index = i;
                _cellButtons[i].onClick.AddListener(() => Toggle(index));
            }
            _randomButton.onClick.AddListener(PickRandom);
            _decideButton.onClick.AddListener(Decide);
        }

        public void Show(IReadOnlyList<MonsterDefinition> monsters, string title, Action<List<MonsterDefinition>> onDecide)
        {
            _monsters = monsters;
            _title = title;
            _onDecide = onDecide;
            _picked.Clear();

            for (int i = 0; i < _cellButtons.Length; i++)
            {
                bool exists = i < monsters.Count;
                _cellButtons[i].gameObject.SetActive(exists);
                if (exists)
                {
                    _cellIcons[i].sprite = MonsterSprites.Front(monsters[i]);
                    _cellLabels[i].text = $"{monsters[i].Name}\n<size=22>{TypeLabel(monsters[i])}</size>";
                }
            }
            Refresh();
            gameObject.SetActive(true);
        }

        private void Toggle(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            if (!_picked.Remove(index) && _picked.Count < BattleBalance.PartySize)
            {
                _picked.Add(index);
            }
            Refresh();
        }

        /// <summary>足りないぶんをランダムで埋める。CPU のチームを決めるときに使う想定</summary>
        private void PickRandom()
        {
            BattleSounds.Play(BattleSound.Cursor);
            var rest = Enumerable.Range(0, _monsters.Count).Except(_picked).OrderBy(_ => UnityEngine.Random.value).ToList();
            _picked.AddRange(rest.Take(BattleBalance.PartySize - _picked.Count));
            Refresh();
        }

        private void Decide()
        {
            if (_picked.Count != BattleBalance.PartySize)
            {
                return;
            }
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            _onDecide?.Invoke(_picked.Select(i => _monsters[i]).ToList());
        }

        private void Refresh()
        {
            _titleText.text = $"{_title}（{_picked.Count}/{BattleBalance.PartySize}）";
            for (int i = 0; i < _cellButtons.Length; i++)
            {
                int order = _picked.IndexOf(i);
                _cellButtons[i].image.color = order >= 0 ? _pickedColor : _cellColor;
                _cellOrders[i].text = order >= 0 ? (order + 1).ToString() : "";
            }
            _decideButton.interactable = _picked.Count == BattleBalance.PartySize;
        }

        private static string TypeLabel(MonsterDefinition monster)
        {
            return string.Join("/", monster.Types.Select(t => t.ToJapanese()));
        }
    }
}
