using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ドラフト画面（仕様書 §2.1）。提示された3体のカードから1体を押して取る × 6回。
    /// 抽選は Draft に任せ、ここは表示と入力だけ
    /// </summary>
    public class DraftPanel : MonoBehaviour
    {
        [SerializeField] private Text _titleText;
        [SerializeField] private Button[] _cardButtons;
        [SerializeField] private Image[] _cardIcons;
        [SerializeField] private Text[] _cardNames;
        [SerializeField] private Text[] _cardStats;
        /// <summary>1枚につき2つ（2タイプ目が無ければ隠す）。カード i のタイプ k は [i * 2 + k]</summary>
        [SerializeField] private Image[] _typeBadges;
        [SerializeField] private Text[] _typeLabels;
        [SerializeField] private Image[] _pickedIcons;
        [SerializeField] private Color _darkLabelColor = new Color(0.2f, 0.2f, 0.25f);
        [SerializeField] private Color _lightLabelColor = Color.white;

        private const int BadgesPerCard = 2;
        /// <summary>タイプの色がこれより明るければ、文字を暗い色にして読めるようにする</summary>
        private const float BrightBadgeThreshold = 0.6f;
        private const string TitleFormat = "ドラフト {0}/{1}　ほしい ペンモンを えらんでください";

        private Draft _draft;
        private Action<IReadOnlyList<MonsterDefinition>> _onDone;

        private void Awake()
        {
            for (int i = 0; i < _cardButtons.Length; i++)
            {
                int index = i;
                _cardButtons[i].onClick.AddListener(() => Pick(index));
            }
        }

        public void Show(Draft draft, Action<IReadOnlyList<MonsterDefinition>> onDone)
        {
            _draft = draft;
            _onDone = onDone;
            Refresh();
            gameObject.SetActive(true);
        }

        private void Pick(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            _draft.Pick(index);
            if (_draft.IsDone)
            {
                gameObject.SetActive(false);
                _onDone?.Invoke(_draft.Picked);
                return;
            }
            Refresh();
        }

        private void Refresh()
        {
            _titleText.text = string.Format(TitleFormat, _draft.Picked.Count + 1, BattleBalance.DraftRounds);
            for (int i = 0; i < _cardButtons.Length; i++)
            {
                ShowCard(i, _draft.Offer[i]);
            }
            for (int i = 0; i < _pickedIcons.Length; i++)
            {
                bool picked = i < _draft.Picked.Count;
                _pickedIcons[i].enabled = picked;
                _pickedIcons[i].sprite = picked ? MonsterSprites.Front(_draft.Picked[i]) : null;
            }
        }

        private void ShowCard(int card, MonsterDefinition monster)
        {
            _cardIcons[card].sprite = MonsterSprites.Front(monster);
            _cardNames[card].text = monster.Name;
            _cardStats[card].text = StatValues(monster.BaseStats);
            for (int k = 0; k < BadgesPerCard; k++)
            {
                int badge = card * BadgesPerCard + k;
                bool exists = k < monster.Types.Count;
                _typeBadges[badge].gameObject.SetActive(exists);
                if (exists)
                {
                    var color = TypeColors.Get(monster.Types[k]);
                    _typeBadges[badge].color = color;
                    _typeLabels[badge].color = color.grayscale > BrightBadgeThreshold ? _darkLabelColor : _lightLabelColor;
                    _typeLabels[badge].text = monster.Types[k].ToJapanese();
                }
            }
        }

        /// <summary>種族値を1行ずつ。見出し（HP・こうげき…）はシーン側の別の Text に並べてある</summary>
        private static string StatValues(Stats s)
        {
            return $"{s.Hp}\n{s.Attack}\n{s.Defense}\n{s.SpAttack}\n{s.SpDefense}\n{s.Speed}\n{s.Total}";
        }
    }
}
