using System;
using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 3vs3 の進行役（1試合ぶん）。ターンの計算は BattleEngine に任せ、ここは入力待ちと表示の順番だけを持つ。
    /// チームは GameFlow から渡される（ドラフト・選出・結果はバトルの外）。
    /// イベントの再生は BattleController.Playback.cs、オンライン対戦は BattleController.Online.cs
    /// </summary>
    public partial class BattleController : MonoBehaviour
    {
        /// <summary>画面の手前（自分）。オンラインのゲストも、写しの陣営を入れ替えて自分を 0 にしている</summary>
        private const int PlayerSide = 0;
        private const int OpponentSide = 1;

        [SerializeField] private MonsterView _playerView;
        [SerializeField] private MonsterView _enemyView;
        [SerializeField] private MonsterInfoBox _playerInfo;
        [SerializeField] private MonsterInfoBox _enemyInfo;
        [SerializeField] private MessageWindow _messageWindow;
        [SerializeField] private CommandPanel _commandPanel;
        [SerializeField] private MovePanel _movePanel;
        [SerializeField] private PartyPanel _partyPanel;
        [Tooltip("オンラインの1ターンの制限時間。オフラインでは隠れたまま")]
        [SerializeField] private CountdownTimer _turnTimer;

        private BattleEngine _engine;
        /// <summary>オフラインとホストは _engine の状態、ゲストはホストから届いたイベントで作る写し</summary>
        private BattleState _state;
        private CpuPlayer _cpu;
        private BattleAction _playerAction;
        private int _replacementIndex = -1;
        /// <summary>行動・入れ替えを選ぶ制限時間。0 なら制限なし（CPU 戦）</summary>
        private float _choiceSeconds;

        private BattleSide Player => _state.Sides[PlayerSide];
        private BattleSide Opponent => _state.Sides[OpponentSide];

        public bool PlayerWon => _state.Winner == PlayerSide;

        private void Start()
        {
            ClearField();
        }

        /// <summary>
        /// 場を空にする。勝ち抜き戦では同じ場で続けて戦うので、前の試合で残ったペンモンや窓を消してから始める
        /// </summary>
        private void ClearField()
        {
            HideChoices();
            _playerView.Hide();
            _enemyView.Hide();
            _playerInfo.Hide();
            _enemyInfo.Hide();
            _messageWindow.SetText("");
        }

        private void HideChoices()
        {
            _turnTimer.Stop();
            _commandPanel.Hide();
            _movePanel.gameObject.SetActive(false);
            _partyPanel.gameObject.SetActive(false);
        }

        /// <summary>CPU とのバトルを決着まで進める。勝敗は終わったあと PlayerWon で見る</summary>
        public IEnumerator Run(List<MonsterDefinition> playerParty, List<MonsterDefinition> cpuParty,
            System.Random random, CpuPlayer cpu)
        {
            BeginLocal(playerParty, cpuParty, random, choiceSeconds: 0f);
            _cpu = cpu;

            yield return Play(_engine.StartBattle());
            while (!_state.IsOver)
            {
                yield return WaitForPlayerAction();
                yield return Play(_engine.ResolveTurn(_playerAction, _cpu.ChooseAction(Opponent, Player)));
                if (!_state.IsOver)
                {
                    yield return ReplaceFainted();
                }
            }
        }

        /// <summary>計算を自分の端末でする試合（CPU 戦・オンラインのホスト）を始める</summary>
        private void BeginLocal(List<MonsterDefinition> playerParty, List<MonsterDefinition> opponentParty,
            System.Random random, float choiceSeconds)
        {
            ClearField();
            _engine = new BattleEngine(new BattleState(playerParty, opponentParty), random);
            _state = _engine.State;
            _choiceSeconds = choiceSeconds;
        }

        private IEnumerator WaitForPlayerAction()
        {
            _playerAction = null;
            ShowCommands();
            // 時間切れは一番上の技（仕様書 §4.7）
            StartChoiceTimer(() => _playerAction = BattleAction.UseMove(0));
            yield return new WaitUntil(() => _playerAction != null);
            _turnTimer.Stop();
        }

        private void ShowCommands()
        {
            _messageWindow.SetText($"{Player.Active.Name}は どうする？");
            _commandPanel.Show(Player, onFight: ShowMoveSelect, onParty: ShowSwitchSelect);
        }

        private void ShowMoveSelect()
        {
            _movePanel.Show(Player.Active, Opponent.Active,
                onMove: index => _playerAction = BattleAction.UseMove(index),
                onBack: ShowCommands);
        }

        private void ShowSwitchSelect()
        {
            _partyPanel.Show(Player,
                onSelect: index => _playerAction = BattleAction.SwitchTo(index),
                onBack: ShowCommands);
        }

        private IEnumerator ReplaceFainted()
        {
            if (Player.NeedsReplacement)
            {
                yield return ChooseReplacement();
                yield return Play(_engine.Replace(PlayerSide, _replacementIndex));
            }
            if (Opponent.NeedsReplacement)
            {
                yield return Play(_engine.Replace(OpponentSide, _cpu.ChooseReplacement(Opponent, Player)));
            }
        }

        /// <summary>ひんしの後に出すペンモンを選ばせ、_replacementIndex に入れる。時間切れは出せる中の先頭</summary>
        private IEnumerator ChooseReplacement()
        {
            _replacementIndex = -1;
            _messageWindow.SetText("つぎの ペンモンを えらんでください");
            _partyPanel.Show(Player, onSelect: index => _replacementIndex = index, onBack: null);
            StartChoiceTimer(() => _replacementIndex = FirstReplaceable());
            yield return new WaitUntil(() => _replacementIndex >= 0);
            _turnTimer.Stop();
        }

        private int FirstReplaceable()
        {
            for (int i = 0; i < Player.Party.Count; i++)
            {
                if (Player.CanSwitchTo(i))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>制限時間があるときだけ数える。時間切れなら開いている選択の窓を閉じてから代わりに決める</summary>
        private void StartChoiceTimer(Action onTimeout)
        {
            _turnTimer.Begin(_choiceSeconds, () =>
            {
                HideChoices();
                onTimeout();
            });
        }
    }
}
