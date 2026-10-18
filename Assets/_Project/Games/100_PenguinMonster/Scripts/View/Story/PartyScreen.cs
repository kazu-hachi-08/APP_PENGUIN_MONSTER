using System.Collections;
using PenguinMonster.Battle;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// メニューの「手持ち」。左に手持ちの一覧。子を押すと「詳しく 見る / 並べ替え / 道具を 使う / 戻る」。
    /// 詳しくはドラフトの詳細と同じ形（技はその子が今覚えている4つ）で、Lv・HP・状態異常は下の行に出す。
    /// バッグから回復の道具を選んで開いたときは、押した子にその道具を使う
    /// </summary>
    public class PartyScreen : MonoBehaviour
    {
        private const int SubLineSize = 28;
        /// <summary>瀕死の子は HP の代わりにこれを出す（あずかり箱でも同じ）</summary>
        internal const string FaintedLabel = "瀕死";
        private const string PickPrompt = "どの 子に しますか？";
        private const string SwapPrompt = "どの 子と 入れ替えますか？";
        private const string SwappedFormat = "{0}と {1}を 入れ替えた";
        private const string UseOnPromptFormat = "{0}を だれに 使いますか？";
        private const string NoEffect = "使っても 効果が ないよ";
        private const string HealedFormat = "{0}の HPが {1} 回復した！";
        private const string CuredFormat = "{0}の {1}が 治った！";
        private const string DetailFormat = "{0}   Lv{1}   HP {2}/{3}   {4}";

        [SerializeField] private Button[] _slotButtons;
        [SerializeField] private Text[] _slotLabels;
        [SerializeField] private Image[] _slotIcons;
        [Tooltip("下の1行。何をするかの案内と結果を出す")]
        [SerializeField] private Text _hint;
        [SerializeField] private Button _closeButton;
        [Tooltip("子を押したときの選択肢")]
        [SerializeField] private GameObject _actions;
        [SerializeField] private Button _detailButton;
        [SerializeField] private Button _swapButton;
        [SerializeField] private Button _itemButton;
        [SerializeField] private Button _actionsBackButton;
        [SerializeField] private DraftDetailView _detail;
        [SerializeField] private Button _detailCloseButton;
        [Tooltip("手持ちの子に道具を使うときのバッグ。メニューのものと同じ")]
        [SerializeField] private BagPanel _bag;

        private StoryState _state;
        /// <summary>選択肢を出している子</summary>
        private int _selected;
        /// <summary>並べ替えで先に選んだ子。並べ替え中でなければ -1</summary>
        private int _swapFrom = -1;
        /// <summary>バッグから開いたときの道具。null なら選択肢を出す</summary>
        private StoryItem? _item;
        private bool _closed;

        private void Awake()
        {
            ButtonHighlight.AddIndexedClick(_slotButtons, OnSlot);
            _closeButton.onClick.AddListener(() => Tap(() => _closed = true));
            _detailButton.onClick.AddListener(() => Tap(ShowDetail));
            _swapButton.onClick.AddListener(() => Tap(StartSwap));
            _itemButton.onClick.AddListener(() => Tap(OpenBag));
            _actionsBackButton.onClick.AddListener(() => Tap(ShowList));
            _detailCloseButton.onClick.AddListener(() => Tap(ShowList));
        }

        /// <param name="item">バッグから選んだ道具。null なら手持ちの画面として開く</param>
        public IEnumerator Open(StoryState state, StoryItem? item = null)
        {
            _state = state;
            _item = item;
            _closed = false;
            gameObject.SetActive(true);
            ShowList();
            yield return new WaitUntil(() => _closed);
            gameObject.SetActive(false);
        }

        private void Tap(System.Action action)
        {
            BattleSounds.Play(BattleSound.Cursor);
            action();
        }

        /// <summary>選択肢・詳しく・並べ替えを閉じて、一覧だけの状態に戻す</summary>
        private void ShowList()
        {
            _swapFrom = -1;
            _actions.SetActive(false);
            _detail.gameObject.SetActive(false);
            Refresh();
            _hint.text = _item.HasValue ? string.Format(UseOnPromptFormat, StoryItems.Get(_item.Value).Name) : PickPrompt;
        }

        private void Refresh()
        {
            var members = _state.Party.Members;
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                bool exists = i < members.Count;
                _slotButtons[i].gameObject.SetActive(exists);
                if (!exists)
                {
                    continue;
                }
                var member = members[i];
                string hp = member.IsFainted ? FaintedLabel : $"HP {member.CurrentHp}/{member.MaxHp}  {member.Status.ToJapanese()}";
                _slotLabels[i].SetFitted(MemberLabel(member, hp));
                _slotIcons[i].sprite = MonsterSprites.Front(member.Species.Definition);
            }
        }

        /// <summary>手持ちの枠の文。1行目に名前と Lv、2行目（HP など）は小さく</summary>
        private static string MemberLabel(OwnedMonster member, string subLine)
        {
            return $"{member.Name}   Lv{member.Level}\n<size={SubLineSize}>{subLine}</size>";
        }

        private void OnSlot(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            if (_item.HasValue)
            {
                UseItem(_item.Value, index);
                return;
            }
            if (_swapFrom >= 0)
            {
                FinishSwap(index);
                return;
            }
            _selected = index;
            _detail.gameObject.SetActive(false);
            _actions.SetActive(true);
            _hint.text = $"{_state.Party.Members[index].Name}を どうする？";
        }

        private void ShowDetail()
        {
            _actions.SetActive(false);
            var member = _state.Party.Members[_selected];
            _detail.gameObject.SetActive(true);
            _detail.Show(member.ToBattle().Definition);
            string status = member.IsFainted ? FaintedLabel :member.Status.ToJapanese();
            _hint.text = string.Format(DetailFormat, member.Name, member.Level, member.CurrentHp, member.MaxHp, status);
        }

        private void StartSwap()
        {
            _actions.SetActive(false);
            _swapFrom = _selected;
            _hint.text = SwapPrompt;
        }

        /// <summary>同じ子を押したら並べ替えをやめる</summary>
        private void FinishSwap(int index)
        {
            int from = _swapFrom;
            ShowList();
            if (index == from)
            {
                return;
            }
            var members = _state.Party.Members;
            _hint.text = string.Format(SwappedFormat, members[from].Name, members[index].Name);
            _state.Party.Swap(from, index);
            Refresh();
        }

        /// <summary>選んだ子に効く回復の道具だけ押せるバッグを出す</summary>
        private void OpenBag()
        {
            _actions.SetActive(false);
            var member = _state.Party.Members[_selected];
            _bag.Show(_state.Bag, BagPocket.Medicine, item => StoryItems.Get(item).IsMedicine && member.CanBeTreatedWith(StoryItems.Get(item)),
                item => UseItem(item, _selected), ShowList);
        }

        private void UseItem(StoryItem item, int index)
        {
            var member = _state.Party.Members[index];
            int hpBefore = member.CurrentHp;
            var statusBefore = member.Status;
            if (!_state.UseItem(item, member))
            {
                _hint.text = NoEffect;
                return;
            }
            BattleSounds.Play(BattleSound.StatUp);
            // バッグから開いたときは続けて使えるよう、道具が残っていれば道具の案内のまま
            if (_item.HasValue && !_state.Bag.Has(_item.Value))
            {
                _item = null;
            }
            ShowList();
            _hint.text = member.CurrentHp > hpBefore
                ? string.Format(HealedFormat, member.Name, member.CurrentHp - hpBefore)
                : string.Format(CuredFormat, member.Name, statusBefore.ToJapanese());
        }
    }
}
