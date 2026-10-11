using System.Collections;
using System.Linq;
using PenguinMonster.Battle;
using PenguinMonster.Common.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// CPU との 3vs3 の進行役。ターンの計算は BattleEngine に任せ、ここは入力待ちと表示の順番だけを持つ。
    /// イベントの再生は BattleController.Playback.cs
    /// </summary>
    public partial class BattleController : MonoBehaviour
    {
        private const int PlayerSide = 0;
        private const int CpuSide = 1;

        [SerializeField] private MonsterView _playerView;
        [SerializeField] private MonsterView _enemyView;
        [SerializeField] private MonsterInfoBox _playerInfo;
        [SerializeField] private MonsterInfoBox _enemyInfo;
        [SerializeField] private MessageWindow _messageWindow;
        [SerializeField] private CommandPanel _commandPanel;
        [SerializeField] private MovePanel _movePanel;
        [SerializeField] private PartyPanel _partyPanel;
        [SerializeField] private Button _retryButton;

        private BattleEngine _engine;
        private CpuPlayer _cpu;
        private BattleAction _playerAction;
        private int _replacementIndex = -1;

        private BattleState State => _engine.State;
        private BattleSide Player => State.Sides[PlayerSide];
        private BattleSide Cpu => State.Sides[CpuSide];

        private void Start()
        {
            _commandPanel.Hide();
            _movePanel.gameObject.SetActive(false);
            _partyPanel.gameObject.SetActive(false);
            _playerInfo.Hide();
            _enemyInfo.Hide();
            _messageWindow.SetText("");
            _retryButton.gameObject.SetActive(false);
            _retryButton.onClick.AddListener(() => SceneManager.LoadScene(SceneNames.PenguinMonster));

            var random = new System.Random();
            // Phase 1 は選出が無いので、定義表の先頭3体 vs 次の3体で固定
            var party0 = MonsterTable.All.Take(BattleBalance.PartySize).ToList();
            var party1 = MonsterTable.All.Skip(BattleBalance.PartySize).Take(BattleBalance.PartySize).ToList();
            _engine = new BattleEngine(new BattleState(party0, party1), random);
            _cpu = new CpuPlayer(random);

            StartCoroutine(BattleLoop());
        }

        private IEnumerator BattleLoop()
        {
            yield return Play(_engine.StartBattle());
            while (!State.IsOver)
            {
                yield return WaitForPlayerAction();
                yield return Play(_engine.ResolveTurn(_playerAction, _cpu.ChooseAction(Cpu)));
                if (!State.IsOver)
                {
                    yield return ReplaceFainted();
                }
            }
            _retryButton.gameObject.SetActive(true);
        }

        private IEnumerator WaitForPlayerAction()
        {
            _playerAction = null;
            ShowCommands();
            yield return new WaitUntil(() => _playerAction != null);
        }

        private void ShowCommands()
        {
            _messageWindow.SetText($"{Player.Active.Name}は どうする？");
            _commandPanel.Show(Player, onFight: ShowMoveSelect, onParty: ShowSwitchSelect);
        }

        private void ShowMoveSelect()
        {
            _movePanel.Show(Player.Active,
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
                _replacementIndex = -1;
                _messageWindow.SetText("つぎの ペンモンを えらんでください");
                _partyPanel.Show(Player, onSelect: index => _replacementIndex = index, onBack: null);
                yield return new WaitUntil(() => _replacementIndex >= 0);
                yield return Play(_engine.Replace(PlayerSide, _replacementIndex));
            }
            if (Cpu.NeedsReplacement)
            {
                yield return Play(_engine.Replace(CpuSide, _cpu.ChooseReplacement(Cpu)));
            }
        }
    }
}
