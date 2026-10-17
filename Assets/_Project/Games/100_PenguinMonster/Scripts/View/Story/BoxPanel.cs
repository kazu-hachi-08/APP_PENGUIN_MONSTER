using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// あずかり箱の画面。左に手持ち、右に箱（1ページ6体、◀ ▶ で送る）。
    /// 押した子が反対側へ移る（入れ替えは「預ける → 引き出す」の2回押し）。結果の文は会話窓に出す
    /// </summary>
    public class BoxPanel : MonoBehaviour
    {
        private const string Prompt = "押した 子が 反対側へ 移ります";
        private const string DepositedFormat = "{0}を あずかり箱へ 預けた";
        private const string WithdrewFormat = "{0}を 手持ちに 加えた";
        private const string LastFighter = "戦える 子が いなくなるので 預けられない";
        private const string PartyFull = "手持ちが いっぱい！ 先に だれかを 預けてね";

        [SerializeField] private Button[] _partyButtons;
        [SerializeField] private Text[] _partyLabels;
        [SerializeField] private Image[] _partyIcons;
        [Tooltip("1ページぶん")]
        [SerializeField] private Button[] _boxButtons;
        [SerializeField] private Text[] _boxLabels;
        [SerializeField] private Image[] _boxIcons;
        [SerializeField] private Text _boxTitle;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private MessageWindow _window;

        private StoryState _state;
        private int _page;
        private bool _closed;

        private int PageSize => _boxButtons.Length;
        private int PageCount => Mathf.Max(1, (_state.Box.Count + PageSize - 1) / PageSize);

        private void Awake()
        {
            for (int i = 0; i < _partyButtons.Length; i++)
            {
                int index = i;
                _partyButtons[i].onClick.AddListener(() => Deposit(index));
            }
            for (int i = 0; i < _boxButtons.Length; i++)
            {
                int index = i;
                _boxButtons[i].onClick.AddListener(() => Withdraw(index));
            }
            _prevButton.onClick.AddListener(() => TurnPage(-1));
            _nextButton.onClick.AddListener(() => TurnPage(1));
            _closeButton.onClick.AddListener(() =>
            {
                BattleSounds.Play(BattleSound.Cursor);
                _closed = true;
            });
        }

        /// <summary>「やめる」が押されるまで開いておく。出し入れは StoryState に直接行う</summary>
        public IEnumerator Open(StoryState state)
        {
            _state = state;
            _page = 0;
            _closed = false;
            gameObject.SetActive(true);
            Refresh();
            _window.SetText(Prompt);
            yield return new WaitUntil(() => _closed);
            gameObject.SetActive(false);
        }

        private void Refresh()
        {
            _page = Mathf.Clamp(_page, 0, PageCount - 1);
            Fill(_partyButtons, _partyLabels, _partyIcons, _state.Party.Members, 0);
            Fill(_boxButtons, _boxLabels, _boxIcons, _state.Box.Members, _page * PageSize);
            _boxTitle.text = $"あずかり箱 {_page + 1}/{PageCount}";
            _prevButton.interactable = _page > 0;
            _nextButton.interactable = _page < PageCount - 1;
        }

        private static void Fill(Button[] buttons, Text[] labels, Image[] icons, IReadOnlyList<OwnedMonster> members, int first)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                bool exists = first + i < members.Count;
                buttons[i].gameObject.SetActive(exists);
                if (!exists)
                {
                    continue;
                }
                var member = members[first + i];
                string hp = member.IsFainted ? PartyScreen.FaintedLabel : $"HP {member.CurrentHp}/{member.MaxHp}";
                // 枠が低く2行は入らないが横は広いので、名前・Lv・HP を1行に並べる
                labels[i].SetFitted($"{member.Name}   Lv{member.Level}   {hp}");
                icons[i].sprite = MonsterSprites.Front(member.Species.Definition);
            }
        }

        private void Deposit(int index)
        {
            string name = _state.Party.Members[index].Name;
            Report(_state.Deposit(index), string.Format(DepositedFormat, name), LastFighter);
        }

        private void Withdraw(int index)
        {
            int boxIndex = _page * PageSize + index;
            string name = _state.Box.Members[boxIndex].Name;
            Report(_state.Withdraw(boxIndex), string.Format(WithdrewFormat, name), PartyFull);
        }

        private void Report(bool done, string doneMessage, string refusedMessage)
        {
            BattleSounds.Play(done ? BattleSound.Cursor : BattleSound.StatDown);
            Refresh();
            _window.SetText(done ? doneMessage : refusedMessage);
        }

        private void TurnPage(int delta)
        {
            BattleSounds.Play(BattleSound.Cursor);
            _page += delta;
            Refresh();
        }
    }
}
