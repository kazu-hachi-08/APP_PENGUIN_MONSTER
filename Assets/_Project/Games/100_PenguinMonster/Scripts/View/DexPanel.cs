using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
using PenguinMonster.View.Story;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ずかん（Phase 8）。上の帯でタイプをしぼりこみ、左の一覧で押したペンモンを右に詳しく出す。
    /// 一覧のマスは定義表の体数ぶんシーンに作ってあり、しぼりこんだ数だけ前から使う（GridLayoutGroup が詰めて並べる）。
    /// ストーリーの子（No.151〜）は記録で見た子だけ見せ、見ていない子は「？」のマスにする。捕まえた子にはタマゴの印を付ける
    /// （PLAN_STORY_TOWN01.md フェーズ8）。タイトルから開いたときは保存した記録を、フィールドから開いたときは今の進み具合を使う
    /// </summary>
    public class DexPanel : SubMenuPanel
    {
        [Tooltip("先頭が「すべて」、続けて MonsterType の並び順")]
        [SerializeField] private Button[] _filterButtons;
        [SerializeField] private Button[] _listButtons;
        [SerializeField] private Image[] _listIcons;
        [SerializeField] private Image[] _listCaughtMarks;
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
        private const string UnknownLabel = "？";

        private IReadOnlyList<MonsterDefinition> _shown = MonsterTable.Dex;
        private Color[] _filterColors;
        private Text[] _listLabels;
        /// <summary>フィールドから開いたときの進み具合。タイトルから開いたときは null（保存した記録を読む）</summary>
        private StoryState _storyState;
        private ICollection<int> _seen = new HashSet<int>();
        private ICollection<int> _caught = new HashSet<int>();

        protected override void Awake()
        {
            base.Awake();
            _filterColors = ButtonHighlight.CaptureColors(_filterButtons);
            for (int i = 0; i < _filterButtons.Length; i++)
            {
                int index = i;
                _filterButtons[i].onClick.AddListener(() => OnFilter(index));
            }
            _listLabels = new Text[_listButtons.Length];
            for (int i = 0; i < _listButtons.Length; i++)
            {
                int index = i;
                _listButtons[i].onClick.AddListener(() => OnCell(index));
                _listLabels[i] = _listButtons[i].GetComponentInChildren<Text>(true);
            }
        }

        /// <summary>フィールドのメニューから開く。今の進み具合の 見た / 捕まえた で見せる（まだ記録していなくても）</summary>
        public void ShowStory(StoryState state, System.Action onClose)
        {
            _storyState = state;
            Show(onClose);
        }

        protected override void OnShow()
        {
            var state = _storyState ?? StorySaveStore.Load()?.State;
            _seen = state?.SeenNos ?? new HashSet<int>();
            _caught = state?.CaughtNos ?? new HashSet<int>();
            ApplyFilter(0);
        }

        protected override void OnHide()
        {
            _storyState = null;
        }

        private void OnFilter(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            ApplyFilter(index);
        }

        private void OnCell(int index)
        {
            if (!StoryDex.IsRevealed(_shown[index], _seen))
            {
                return;
            }
            BattleSounds.Play(BattleSound.Cursor);
            Select(_shown[index]);
        }

        /// <param name="index">0 は「すべて」、1 以降は MonsterType + 1</param>
        private void ApplyFilter(int index)
        {
            _shown = StoryDex.Filter(index == 0 ? (MonsterType?)null : (MonsterType)(index - 1), _seen);
            ButtonHighlight.Paint(_filterButtons, _filterColors, index);
            for (int i = 0; i < _listButtons.Length; i++)
            {
                bool used = i < _shown.Count;
                _listButtons[i].gameObject.SetActive(used);
                if (used)
                {
                    PaintCell(i, _shown[i]);
                }
            }
            ResetScroll();
            // しぼりこんだ先頭を出しておけば、右側が空になることはない（どのタイプも1体以上いる）
            Select(_shown[0]);
        }

        /// <summary>見ていない子は絵の代わりに「？」。捕まえた子は右上にタマゴの印</summary>
        private void PaintCell(int index, MonsterDefinition monster)
        {
            bool revealed = StoryDex.IsRevealed(monster, _seen);
            _listIcons[index].enabled = revealed;
            if (revealed)
            {
                _listIcons[index].sprite = MonsterSprites.Front(monster);
            }
            _listLabels[index].text = revealed ? string.Empty : UnknownLabel;
            bool caught = _caught.Contains(monster.No);
            _listCaughtMarks[index].enabled = caught;
            if (caught)
            {
                _listCaughtMarks[index].sprite = EggSprites.Get(EggKind.Normal);
            }
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
