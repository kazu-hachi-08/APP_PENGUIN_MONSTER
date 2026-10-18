using System;
using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 最初の3体を選ぶ画面。ドラフト画面と同じ形で、左に3つのタマゴのカード、
    /// 押すと右に中身の詳細（DraftDetailView）、「この子に 決めた！」で選ぶ。確かめる「はい / いいえ」は FieldEventPlayer が出す。
    /// PC は上下でタマゴを選び、A（Z）で決める
    /// </summary>
    public class StarterPanel : MonoBehaviour
    {
        private const string EggLabelFormat = "{0}の タマゴ";

        [SerializeField] private Button[] _cardButtons;
        [SerializeField] private Image[] _cardIcons;
        [SerializeField] private Text[] _cardNames;
        [SerializeField] private Outline[] _cardOutlines;
        [Tooltip("1枚につき2つ。タマゴは1タイプなので2つ目は隠す")]
        [SerializeField] private Image[] _typeBadges;
        [SerializeField] private Text[] _typeLabels;
        [SerializeField] private DraftDetailView _detail;
        [SerializeField] private Button _decideButton;
        [SerializeField] private FieldInput _input;

        private IReadOnlyList<StorySpecies> _choices;
        private int _selected;
        private bool _decided;

        private void Awake()
        {
            ButtonHighlight.AddIndexedClick(_cardButtons, OnCardClicked);
            _decideButton.onClick.AddListener(Decide);
        }

        public void Open(IReadOnlyList<StorySpecies> choices)
        {
            _choices = choices;
            gameObject.SetActive(true);
            for (int i = 0; i < _cardButtons.Length; i++)
            {
                ShowEgg(i, choices[i].Definition);
            }
            Select(0);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        /// <summary>「この子に 決めた！」を押すまで待つ。確かめで いいえ のときは、選んでいたタマゴのまま また呼ばれる</summary>
        public IEnumerator Pick(Action<StorySpecies> onPicked)
        {
            _decided = false;
            SetInteractable(true);
            // 前の窓を送った A がそのまま決定にならないよう、出したフレームは入力を見ない
            yield return null;
            Direction? lastHeld = _input.HeldDirection;
            while (!_decided)
            {
                lastHeld = ReadKeys(lastHeld);
                yield return null;
            }
            // 確かめの間に押し直せないよう止める
            SetInteractable(false);
            onPicked(_choices[_selected]);
        }

        private Direction? ReadKeys(Direction? lastHeld)
        {
            var held = _input.HeldDirection;
            int step = FieldInput.VerticalStep(held, lastHeld);
            if (step != 0)
            {
                OnCardClicked((_selected + step + _choices.Count) % _choices.Count);
            }
            if (_input.IsADown)
            {
                Decide();
            }
            return held;
        }

        private void ShowEgg(int index, MonsterDefinition monster)
        {
            var type = monster.Types[0];
            _cardIcons[index].sprite = EggSprites.Get(EggKind.Normal);
            _cardNames[index].SetFitted(string.Format(EggLabelFormat, type.ToJapanese()));
            int badge = index * TypeColors.BadgesPerMonster;
            TypeColors.PaintBadge(_typeBadges[badge], _typeLabels[badge], type);
            _typeBadges[badge + 1].gameObject.SetActive(false);
        }

        private void OnCardClicked(int index)
        {
            if (_decided || index == _selected) return;
            BattleSounds.Play(BattleSound.Cursor);
            Select(index);
        }

        private void Select(int index)
        {
            _selected = index;
            for (int i = 0; i < _cardOutlines.Length; i++)
            {
                _cardOutlines[i].enabled = i == index;
            }
            _detail.Show(_choices[index].Definition);
        }

        private void Decide()
        {
            if (_decided) return;
            BattleSounds.Play(BattleSound.Cursor);
            _decided = true;
        }

        private void SetInteractable(bool interactable)
        {
            foreach (var button in _cardButtons)
            {
                button.interactable = interactable;
            }
            _decideButton.interactable = interactable;
        }
    }
}
