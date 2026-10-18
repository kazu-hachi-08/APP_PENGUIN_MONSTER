using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// よろず屋の画面。「買う / 売る」を切り替え、道具を押すたびに1個ずつ売り買いする。
    /// 品が枠に入りきらないので1ページずつ並べる（← → で送る）。結果の文は会話窓に出す（画面の下は会話窓のために空けてある）。「やめる」で閉じる
    /// </summary>
    public class ShopPanel : MonoBehaviour
    {
        private const int SubLineSize = 30;
        private const string BuyPrompt = "何を 買いますか？";
        private const string SellPrompt = "どれを 売りますか？";
        private const string NothingToSell = "売れる 道具を 持っていない";
        private const string BoughtFormat = "{0}を 買った！";
        private const string SoldFormat = "{0}を {1}円で 売った！";
        private const string NoMoney = "お金が 足りないよ";

        [SerializeField] private Button _buyTab;
        [SerializeField] private Button _sellTab;
        [SerializeField] private Button _closeButton;
        [Tooltip("1ページぶんの枠")]
        [SerializeField] private Button[] _slotButtons;
        [SerializeField] private Text[] _slotLabels;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Text _moneyText;
        [SerializeField] private MessageWindow _window;

        private StoryState _state;
        private IReadOnlyList<StoryItem> _goods;
        /// <summary>今の枠に並んでいる道具（買う: 店の品 / 売る: バッグの中身）</summary>
        private List<StoryItem> _shown = new List<StoryItem>();
        private bool _selling;
        private bool _closed;
        private int _page;

        private int PageSize => _slotButtons.Length;
        private int PageCount => PagedList.PageCount(_shown.Count, PageSize);

        private void Awake()
        {
            ButtonHighlight.AddIndexedClick(_slotButtons, Trade);
            _prevButton.onClick.AddListener(() => TurnPage(-1));
            _nextButton.onClick.AddListener(() => TurnPage(1));
            _buyTab.onClick.AddListener(() => SetMode(false));
            _sellTab.onClick.AddListener(() => SetMode(true));
            _closeButton.onClick.AddListener(() =>
            {
                BattleSounds.Play(BattleSound.Cursor);
                _closed = true;
            });
        }

        private void Update()
        {
            int delta = PagedList.KeyDelta();
            if (delta != 0)
            {
                TurnPage(delta);
            }
        }

        /// <summary>「やめる」が押されるまで開いておく。売り買いは StoryState に直接行う</summary>
        public IEnumerator Open(StoryState state, IReadOnlyList<StoryItem> goods)
        {
            _state = state;
            _goods = goods;
            _closed = false;
            gameObject.SetActive(true);
            SetMode(false);
            yield return new WaitUntil(() => _closed);
            gameObject.SetActive(false);
        }

        private void SetMode(bool selling)
        {
            _selling = selling;
            _page = 0;
            // 今の側のタブは押せなくして、どちらを開いているか見せる
            _buyTab.interactable = selling;
            _sellTab.interactable = !selling;
            Refresh();
            _window.SetText(!selling ? BuyPrompt : _shown.Count > 0 ? SellPrompt : NothingToSell);
        }

        private void Refresh()
        {
            _shown = _selling ? _state.Bag.Items() : new List<StoryItem>(_goods);
            // 売って道具が無くなると最後のページが消えることがあるので、ページを詰める
            _page = Mathf.Min(_page, PageCount - 1);
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                int index = _page * PageSize + i;
                bool exists = index < _shown.Count;
                _slotButtons[i].gameObject.SetActive(exists);
                if (exists)
                {
                    _slotLabels[i].SetFitted(Label(_shown[index]));
                }
            }
            PagedList.ShowPageButtons(_prevButton, _nextButton, _page, PageCount);
            _moneyText.text = $"お金\n{_state.Money}円";
        }

        private string Label(StoryItem item)
        {
            var definition = StoryItems.Get(item);
            int price = _selling ? definition.SellPrice : definition.Price;
            return $"{definition.Name}   {price}円\n<size={SubLineSize}>持っている数 {_state.Bag.CountOf(item)}</size>";
        }

        private void TurnPage(int delta)
        {
            if (PagedList.Turn(ref _page, delta, PageCount))
            {
                BattleSounds.Play(BattleSound.Cursor);
                Refresh();
            }
        }

        private void Trade(int slot)
        {
            var item = _shown[_page * PageSize + slot];
            var definition = StoryItems.Get(item);
            string message;
            if (_selling)
            {
                Shop.Sell(_state, item);
                message = string.Format(SoldFormat, definition.Name, definition.SellPrice);
            }
            else
            {
                message = Shop.Buy(_state, item) ? string.Format(BoughtFormat, definition.Name) : NoMoney;
            }
            BattleSounds.Play(message == NoMoney ? BattleSound.StatDown : BattleSound.Cursor);
            Refresh();
            _window.SetText(message);
        }
    }
}
