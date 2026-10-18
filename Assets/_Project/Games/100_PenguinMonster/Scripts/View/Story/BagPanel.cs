using System;
using System.Collections.Generic;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>バッグのタブ。並びは画面のタブの並び</summary>
    public enum BagPocket
    {
        Eggs,
        Medicine,
    }

    /// <summary>
    /// バッグの一覧。上の「タマゴ / 薬」のタブで分け、持っている道具だけを1ページずつ並べる（道具が増えても枠に入るように）。
    /// 押せるかどうかは呼ぶ側が決める（バトル中・フィールドで使える道具が違うため）。バトルとフィールドのメニューで使い回す
    /// </summary>
    public class BagPanel : MonoBehaviour
    {
        private const int DescriptionSize = 28;
        private static readonly string[] EmptyMessages = { "タマゴを 持っていない", "薬を 持っていない" };

        [Tooltip("BagPocket の順に1つずつ")]
        [SerializeField] private Button[] _pocketTabs;
        [Tooltip("1ページぶんの枠")]
        [SerializeField] private Button[] _slotButtons;
        [SerializeField] private Text[] _slotLabels;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _backButton;
        [Tooltip("今のタブに1つも持っていないときに出す文")]
        [SerializeField] private Text _emptyLabel;

        private Bag _bag;
        private Func<StoryItem, bool> _canUse;
        private Action<StoryItem> _onSelect;
        private Action _onBack;
        private BagPocket _pocket;
        private int _page;
        /// <summary>今のタブで持っている道具（StoryItem の順）</summary>
        private readonly List<StoryItem> _shown = new List<StoryItem>();

        private int PageSize => _slotButtons.Length;
        private int PageCount => PagedList.PageCount(_shown.Count, PageSize);

        private void Awake()
        {
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                int index = i;
                _slotButtons[i].onClick.AddListener(() =>
                {
                    var item = _shown[_page * PageSize + index];
                    Choose(() => _onSelect?.Invoke(item));
                });
            }
            for (int i = 0; i < _pocketTabs.Length; i++)
            {
                var pocket = (BagPocket)i;
                _pocketTabs[i].onClick.AddListener(() =>
                {
                    BattleSounds.Play(BattleSound.Cursor);
                    Open(pocket);
                });
            }
            _prevButton.onClick.AddListener(() => TurnPage(-1));
            _nextButton.onClick.AddListener(() => TurnPage(1));
            _backButton.onClick.AddListener(() => Choose(_onBack));
        }

        private void Update()
        {
            int delta = PagedList.KeyDelta();
            if (delta != 0)
            {
                TurnPage(delta);
            }
        }

        /// <param name="pocket">最初に開くタブ（バトルはタマゴ、フィールドは薬）</param>
        /// <param name="canUse">押せる道具。null なら全部押せる</param>
        public void Show(Bag bag, BagPocket pocket, Func<StoryItem, bool> canUse, Action<StoryItem> onSelect, Action onBack)
        {
            _bag = bag;
            _canUse = canUse;
            _onSelect = onSelect;
            _onBack = onBack;
            Open(pocket);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private static BagPocket PocketOf(StoryItem item)
        {
            return StoryItems.Get(item).IsCaptureEgg ? BagPocket.Eggs : BagPocket.Medicine;
        }

        private void Open(BagPocket pocket)
        {
            _pocket = pocket;
            _page = 0;
            _shown.Clear();
            foreach (var item in _bag.Items())
            {
                if (PocketOf(item) == pocket)
                {
                    _shown.Add(item);
                }
            }
            Refresh();
        }

        private void TurnPage(int delta)
        {
            int page = Mathf.Clamp(_page + delta, 0, PageCount - 1);
            if (page == _page)
            {
                return;
            }
            BattleSounds.Play(BattleSound.Cursor);
            _page = page;
            Refresh();
        }

        private void Refresh()
        {
            // 今のタブは押せなくして、どちらを開いているか見せる（よろず屋の「買う / 売る」と同じ）
            for (int i = 0; i < _pocketTabs.Length; i++)
            {
                _pocketTabs[i].interactable = i != (int)_pocket;
            }
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                int index = _page * PageSize + i;
                bool exists = index < _shown.Count;
                _slotButtons[i].gameObject.SetActive(exists);
                if (exists)
                {
                    ShowSlot(i, _shown[index]);
                }
            }
            PagedList.ShowPageButtons(_prevButton, _nextButton, _page, PageCount);
            _emptyLabel.text = EmptyMessages[(int)_pocket];
            _emptyLabel.gameObject.SetActive(_shown.Count == 0);
        }

        private void ShowSlot(int slot, StoryItem item)
        {
            var definition = StoryItems.Get(item);
            _slotLabels[slot].SetFitted($"{definition.Name}   ×{_bag.CountOf(item)}\n<size={DescriptionSize}>{definition.Description}</size>");
            _slotButtons[slot].interactable = _canUse == null || _canUse(item);
        }

        private void Choose(Action action)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            action?.Invoke();
        }
    }
}
