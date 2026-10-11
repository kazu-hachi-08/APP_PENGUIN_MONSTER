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
        [Tooltip("特性と持ち物（名前と説明を1行ずつ）")]
        [SerializeField] private Text[] _cardTraits;
        /// <summary>1枚につき2つ（2タイプ目が無ければ隠す）。カード i のタイプ k は [i * 2 + k]</summary>
        [SerializeField] private Image[] _typeBadges;
        [SerializeField] private Text[] _typeLabels;
        [SerializeField] private Image[] _pickedIcons;
        [Tooltip("オンラインの1回ごとの制限時間。オフラインでは隠れたまま")]
        [SerializeField] private CountdownTimer _timer;

        private const int BadgesPerCard = TypeColors.BadgesPerMonster;
        private const int TraitDescriptionSize = 20;
        private const string TitleFormat = "ドラフト {0}/{1}　ほしい ペンモンを えらんでください";

        private Draft _draft;
        private Action<IReadOnlyList<MonsterDefinition>> _onDone;
        private float _secondsPerPick;

        private void Awake()
        {
            for (int i = 0; i < _cardButtons.Length; i++)
            {
                int index = i;
                _cardButtons[i].onClick.AddListener(() => Pick(index));
            }
        }

        /// <param name="secondsPerPick">1回の制限時間。0 なら制限なし（オフライン）</param>
        public void Show(Draft draft, Action<IReadOnlyList<MonsterDefinition>> onDone, float secondsPerPick = 0f)
        {
            _draft = draft;
            _onDone = onDone;
            _secondsPerPick = secondsPerPick;
            Refresh();
            gameObject.SetActive(true);
        }

        private void Pick(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            _draft.Pick(index);
            if (_draft.IsDone)
            {
                Hide();
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
            _timer.Begin(_secondsPerPick, PickRandom);
        }

        /// <summary>時間切れは3体からランダム（仕様書 §2.1）</summary>
        private void PickRandom()
        {
            Pick(UnityEngine.Random.Range(0, _draft.Offer.Count));
        }

        /// <summary>途中で相手が抜けたときなど、外から閉じる</summary>
        public void Hide()
        {
            _timer.Stop();
            gameObject.SetActive(false);
        }

        private void ShowCard(int card, MonsterDefinition monster)
        {
            _cardIcons[card].sprite = MonsterSprites.Front(monster);
            _cardNames[card].text = monster.Name;
            _cardStats[card].text = MonsterTexts.StatValues(monster.BaseStats);
            _cardTraits[card].text = MonsterTexts.Traits(monster, TraitDescriptionSize);
            TypeColors.PaintBadges(monster, _typeBadges, _typeLabels, card * BadgesPerCard);
        }
    }
}
