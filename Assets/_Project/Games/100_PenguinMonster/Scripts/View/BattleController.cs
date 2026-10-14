using System;
using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 1試合の進行役（シングル 3vs3 / ダブルス 4vs4）。ターンの計算は BattleEngine に任せ、ここは入力待ちと表示の順番だけを持つ。
    /// チームは GameFlow から渡される（ドラフト・選出・結果はバトルの外）。
    /// 行動の選び方は BattleController.Commands.cs、イベントの再生は .Playback.cs、オンライン対戦は .Online.cs
    /// </summary>
    public partial class BattleController : MonoBehaviour
    {
        /// <summary>画面の手前（自分）。オンラインのゲストも、写しの陣営を入れ替えて自分を 0 にしている</summary>
        private const int PlayerSide = 0;
        private const int OpponentSide = 1;

        [Header("シングルの場")]
        [SerializeField] private MonsterView _playerView;
        [SerializeField] private MonsterView _enemyView;
        [SerializeField] private MonsterInfoBox _playerInfo;
        [SerializeField] private MonsterInfoBox _enemyInfo;
        [Header("ダブルスの場（場の順。0 が左）")]
        [SerializeField] private MonsterView[] _playerDoubleViews;
        [SerializeField] private MonsterView[] _enemyDoubleViews;
        [SerializeField] private MonsterInfoBox[] _playerDoubleInfos;
        [SerializeField] private MonsterInfoBox[] _enemyDoubleInfos;
        [Tooltip("技が当たったときのエフェクト。場全体に重ねた層")]
        [SerializeField] private MoveEffectView _moveEffect;
        [Tooltip("場の天候の色と粒。場全体に重ねた層")]
        [SerializeField] private WeatherView _weather;
        [Tooltip("特性が働いたときの帯。陣営ごとに1つ")]
        [SerializeField] private AbilityBanner _playerBanner;
        [SerializeField] private AbilityBanner _enemyBanner;
        [Tooltip("選出の残りを見せるタマゴの列。陣営ごとに1つ")]
        [SerializeField] private PartyEggTray _playerTray;
        [SerializeField] private PartyEggTray _enemyTray;
        [Tooltip("繰り出すときにタマゴを投げる演出。場全体に重ねた層")]
        [SerializeField] private EggThrowView _eggThrow;
        [Header("窓")]
        [SerializeField] private MessageWindow _messageWindow;
        [SerializeField] private CommandPanel _commandPanel;
        [SerializeField] private MovePanel _movePanel;
        [SerializeField] private TargetPanel _targetPanel;
        [SerializeField] private PartyPanel _partyPanel;
        [Tooltip("オンラインの1ターンの制限時間。オフラインでは隠れたまま")]
        [SerializeField] private CountdownTimer _turnTimer;

        private BattleEngine _engine;
        /// <summary>オフラインとホストは _engine の状態、ゲストはホストから届いたイベントで作る写し</summary>
        private BattleState _state;
        private CpuPlayer _cpu;
        private int _replacementIndex = -1;
        /// <summary>行動・入れ替えを選ぶ制限時間。0 なら制限なし（CPU 戦）</summary>
        private float _choiceSeconds;
        /// <summary>陣営ごとのタマゴの種類。見た目だけなのでオンラインでも送らない</summary>
        private readonly EggKind[] _eggs = { EggKind.Normal, EggKind.Normal };

        private BattleSide Player => _state.Sides[PlayerSide];
        private BattleSide Opponent => _state.Sides[OpponentSide];

        public bool PlayerWon => _state.Winner == PlayerSide;

        /// <summary>
        /// 次の試合から使うタマゴの種類。シングル・ダブルスは呼ばずに Normal のまま。
        /// ストーリーの相手ごとにタマゴを変えるときは Run の前に呼ぶ
        /// </summary>
        public void SetEggs(EggKind player, EggKind opponent)
        {
            _eggs[PlayerSide] = player;
            _eggs[OpponentSide] = opponent;
        }

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
            _weather.Hide();
            _playerBanner.Hide();
            _enemyBanner.Hide();
            _playerTray.Hide();
            _enemyTray.Hide();
            foreach (var view in AllViews())
            {
                view.Hide();
            }
            foreach (var info in AllInfos())
            {
                info.Hide();
            }
            _messageWindow.SetText("");
        }

        private IEnumerable<MonsterView> AllViews()
        {
            yield return _playerView;
            yield return _enemyView;
            foreach (var view in _playerDoubleViews) yield return view;
            foreach (var view in _enemyDoubleViews) yield return view;
        }

        private IEnumerable<MonsterInfoBox> AllInfos()
        {
            yield return _playerInfo;
            yield return _enemyInfo;
            foreach (var info in _playerDoubleInfos) yield return info;
            foreach (var info in _enemyDoubleInfos) yield return info;
        }

        private void HideChoices()
        {
            _turnTimer.Stop();
            _commandPanel.Hide();
            _movePanel.gameObject.SetActive(false);
            _targetPanel.gameObject.SetActive(false);
            _partyPanel.gameObject.SetActive(false);
        }

        /// <summary>CPU とのバトルを決着まで進める。勝敗は終わったあと PlayerWon で見る</summary>
        public IEnumerator Run(List<MonsterDefinition> playerParty, List<MonsterDefinition> cpuParty,
            System.Random random, CpuPlayer cpu, BattleFormat format)
        {
            BeginLocal(playerParty, cpuParty, random, format, choiceSeconds: 0f);
            _cpu = cpu;

            yield return Play(_engine.StartBattle());
            while (!_state.IsOver)
            {
                yield return WaitForPlayerActions();
                yield return Play(_engine.ResolveTurn(_playerActions, _cpu.ChooseActions(Opponent, Player)));
                if (!_state.IsOver)
                {
                    yield return ReplaceFainted();
                }
            }
        }

        /// <summary>計算を自分の端末でする試合（CPU 戦・オンラインのホスト）を始める</summary>
        private void BeginLocal(List<MonsterDefinition> playerParty, List<MonsterDefinition> opponentParty,
            System.Random random, BattleFormat format, float choiceSeconds)
        {
            ClearField();
            _engine = new BattleEngine(new BattleState(playerParty, opponentParty, format.ActiveCount()), random);
            _state = _engine.State;
            _choiceSeconds = choiceSeconds;
            ShowTrays();
        }

        /// <summary>試合の始めに、両陣営の選出ぶんのタマゴを並べる</summary>
        private void ShowTrays()
        {
            _playerTray.Show(Player, _eggs[PlayerSide]);
            _enemyTray.Show(Opponent, _eggs[OpponentSide]);
        }

        /// <summary>空いた場を前から1つずつ埋める。自分が先、CPU が後</summary>
        private IEnumerator ReplaceFainted()
        {
            foreach (int slot in Player.ReplaceableSlots())
            {
                yield return ChooseReplacement(slot, new List<int>());
                yield return Play(_engine.Replace(PlayerSide, slot, _replacementIndex));
            }
            foreach (int slot in Opponent.ReplaceableSlots())
            {
                yield return Play(_engine.Replace(OpponentSide, slot, _cpu.ChooseReplacement(Opponent, Player)));
            }
        }

        /// <summary>
        /// 瀕死の後に場の slot 番目へ出すペンモンを選ばせ、_replacementIndex に入れる。
        /// reserved は同じ入れ替えで先に選んだ子（オンラインはまとめて送るので、写しの場にはまだ出ていない）。時間切れは出せる中の先頭
        /// </summary>
        private IEnumerator ChooseReplacement(int slot, List<int> reserved)
        {
            _replacementIndex = -1;
            string fainted = Player.ActiveCount > 1 ? $"{Player.ActiveAt(slot).Name}の 代わりに " : "";
            _messageWindow.SetText($"{fainted}次の ペンモンを 選んでください");
            _partyPanel.Show(Player, onSelect: index => _replacementIndex = index, onBack: null, reserved);
            StartChoiceTimer(() => _replacementIndex = FirstReplaceable(reserved));
            yield return new WaitUntil(() => _replacementIndex >= 0);
            _turnTimer.Stop();
        }

        private int FirstReplaceable(List<int> reserved)
        {
            for (int i = 0; i < Player.Party.Count; i++)
            {
                if (Player.CanSwitchTo(i) && !reserved.Contains(i))
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
