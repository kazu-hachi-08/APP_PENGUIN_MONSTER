using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ずかん（Phase 8）。上の帯でタイプをしぼりこみ、左の一覧で押したペンモンを右に詳しく出す。
    /// 一覧のマスは定義表の体数ぶんシーンに作ってあり、しぼりこんだ数だけ前から使う（GridLayoutGroup が詰めて並べる）
    /// </summary>
    public class DexPanel : SubMenuPanel
    {
        [Tooltip("先頭が「すべて」、続けて MonsterType の並び順")]
        [SerializeField] private Button[] _filterButtons;
        [SerializeField] private Button[] _listButtons;
        [SerializeField] private Image[] _listIcons;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Image _icon;
        [SerializeField] private Text _nameText;
        [SerializeField] private Image[] _typeBadges;
        [SerializeField] private Text[] _typeLabels;
        [SerializeField] private Text _statValues;
        [SerializeField] private Text _movesText;
        [SerializeField] private Text _traitsText;
        [SerializeField] private Color _selectedCellColor = new Color(1f, 0.85f, 0.4f);
        [SerializeField] private Color _cellColor = new Color(0.1f, 0.14f, 0.22f);

        private const int TraitDescriptionSize = 24;
        private const int MoveDetailSize = 22;
        /// <summary>名前を目立たせるため No は小さめ</summary>
        private const int NoSize = 30;

        private IReadOnlyList<MonsterDefinition> _shown = MonsterTable.All;
        private Color[] _filterColors;

        protected override void Awake()
        {
            base.Awake();
            _filterColors = ButtonHighlight.CaptureColors(_filterButtons);
            for (int i = 0; i < _filterButtons.Length; i++)
            {
                int index = i;
                _filterButtons[i].onClick.AddListener(() => OnFilter(index));
            }
            for (int i = 0; i < _listButtons.Length; i++)
            {
                int index = i;
                _listButtons[i].onClick.AddListener(() => OnCell(index));
            }
        }

        protected override void OnShow()
        {
            ApplyFilter(0);
        }

        private void OnFilter(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            ApplyFilter(index);
        }

        private void OnCell(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Select(_shown[index]);
        }

        /// <param name="index">0 は「すべて」、1 以降は MonsterType + 1</param>
        private void ApplyFilter(int index)
        {
            _shown = MonsterTable.WithType(index == 0 ? (MonsterType?)null : (MonsterType)(index - 1));
            ButtonHighlight.Paint(_filterButtons, _filterColors, index);
            for (int i = 0; i < _listButtons.Length; i++)
            {
                bool used = i < _shown.Count;
                _listButtons[i].gameObject.SetActive(used);
                if (used)
                {
                    _listIcons[i].sprite = MonsterSprites.Front(_shown[i]);
                }
            }
            ResetScroll();
            // しぼりこんだ先頭を出しておけば、右側が空になることはない（どのタイプも1体以上いる）
            Select(_shown[0]);
        }

        private void ResetScroll()
        {
            if (_scrollRect != null)
            {
                _scrollRect.velocity = Vector2.zero;
                if (_scrollRect.content != null)
                {
                    _scrollRect.content.anchoredPosition = new Vector2(_scrollRect.content.anchoredPosition.x, 0f);
                }
                _scrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private void Select(MonsterDefinition monster)
        {
            for (int i = 0; i < _shown.Count; i++)
            {
                _listButtons[i].image.color = _shown[i] == monster ? _selectedCellColor : _cellColor;
            }
            _icon.sprite = MonsterSprites.Front(monster);
            _nameText.text = $"<size={NoSize}>No.{monster.No}</size>  {monster.Name}";
            TypeColors.PaintBadges(monster, _typeBadges, _typeLabels);
            _statValues.text = MonsterTexts.StatValues(monster.BaseStats);
            _movesText.SetFitted(Moves(monster));
            _traitsText.SetFitted(MonsterTexts.Traits(monster, TraitDescriptionSize));
        }

        /// <summary>技1つにつき2行（名前とタイプ / 数字と効果）。タイプは札の代わりに色付きの文字にする</summary>
        private static string Moves(MonsterDefinition monster)
        {
            var lines = new List<string>();
            foreach (var move in monster.Moves)
            {
                string color = ColorUtility.ToHtmlStringRGB(TypeColors.Get(move.Type));
                string effects = move.Effects();
                string detail = effects.Length > 0 ? $"{move.Numbers()}　{effects}" : move.Numbers();
                lines.Add($"<b>{move.Name}</b>  <color=#{color}>{move.Type.ToJapanese()}</color>\n<size={MoveDetailSize}>{detail}</size>");
            }
            return string.Join("\n", lines);
        }
    }
}
