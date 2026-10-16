using System;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// バッグの一覧（PLAN_STORY_TOWN01.md フェーズ6）。道具の種類ごとに枠を1つ持ち、持っている道具だけ並べる。
    /// 押せるかどうかは呼ぶ側が決める（バトル中・フィールドで使える道具が違うため）。バトルとフィールドのメニューで使い回す
    /// </summary>
    public class BagPanel : MonoBehaviour
    {
        private const int DescriptionSize = 28;

        [Tooltip("StoryItem の順に1つずつ")]
        [SerializeField] private Button[] _slotButtons;
        [SerializeField] private Text[] _slotLabels;
        [SerializeField] private Button _backButton;
        [Tooltip("1つも持っていないときに出す文")]
        [SerializeField] private GameObject _emptyLabel;

        private Action<StoryItem> _onSelect;
        private Action _onBack;

        private void Awake()
        {
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                var item = (StoryItem)i;
                _slotButtons[i].onClick.AddListener(() => Choose(() => _onSelect?.Invoke(item)));
            }
            _backButton.onClick.AddListener(() => Choose(_onBack));
        }

        /// <param name="canUse">押せる道具。null なら全部押せる</param>
        public void Show(Bag bag, Func<StoryItem, bool> canUse, Action<StoryItem> onSelect, Action onBack)
        {
            _onSelect = onSelect;
            _onBack = onBack;
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                var item = (StoryItem)i;
                bool has = bag.Has(item);
                _slotButtons[i].gameObject.SetActive(has);
                if (!has)
                {
                    continue;
                }
                var definition = StoryItems.Get(item);
                _slotLabels[i].SetFitted($"{definition.Name}   ×{bag.CountOf(item)}\n<size={DescriptionSize}>{definition.Description}</size>");
                _slotButtons[i].interactable = canUse == null || canUse(item);
            }
            _emptyLabel.SetActive(bag.Items().Count == 0);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Choose(Action action)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            action?.Invoke();
        }
    }
}
