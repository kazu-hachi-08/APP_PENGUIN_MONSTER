using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// よろず屋の画面（PLAN_STORY_TOWN01.md フェーズ6）。「買う / 売る」を切り替え、道具を押すたびに1個ずつ売り買いする。
    /// 結果の文は会話窓に出す（画面の下は会話窓のために空けてある）。「やめる」で閉じる
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
        [Tooltip("道具の種類の数だけ")]
        [SerializeField] private Button[] _slotButtons;
        [SerializeField] private Text[] _slotLabels;
        [SerializeField] private Text _moneyText;
        [SerializeField] private MessageWindow _window;

        private StoryState _state;
        private IReadOnlyList<StoryItem> _goods;
        /// <summary>今の枠に並んでいる道具（買う: 店の品 / 売る: バッグの中身）</summary>
        private List<StoryItem> _shown = new List<StoryItem>();
        private bool _selling;
        private bool _closed;

        private void Awake()
        {
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                int index = i;
                _slotButtons[i].onClick.AddListener(() => Trade(index));
            }
            _buyTab.onClick.AddListener(() => SetMode(false));
            _sellTab.onClick.AddListener(() => SetMode(true));
            _closeButton.onClick.AddListener(() =>
            {
                BattleSounds.Play(BattleSound.Cursor);
                _closed = true;
            });
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
            // 今の側のタブは押せなくして、どちらを開いているか見せる
            _buyTab.interactable = selling;
            _sellTab.interactable = !selling;
            Refresh();
            _window.SetText(!selling ? BuyPrompt : _shown.Count > 0 ? SellPrompt : NothingToSell);
        }

        private void Refresh()
        {
            _shown = _selling ? _state.Bag.Items() : new List<StoryItem>(_goods);
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                bool exists = i < _shown.Count;
                _slotButtons[i].gameObject.SetActive(exists);
                if (exists)
                {
                    _slotLabels[i].SetFitted(Label(_shown[i]));
                }
            }
            _moneyText.text = $"お金\n{_state.Money}円";
        }

        private string Label(StoryItem item)
        {
            var definition = StoryItems.Get(item);
            int price = _selling ? definition.SellPrice : definition.Price;
            return $"{definition.Name}   {price}円\n<size={SubLineSize}>持っている数 {_state.Bag.CountOf(item)}</size>";
        }

        private void Trade(int index)
        {
            var item = _shown[index];
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
