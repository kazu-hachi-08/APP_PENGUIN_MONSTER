using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// ターンの行動を選ばせる。ダブルスは場の左から1体ずつ「戦う（技 → 狙う相手）/ ペンモン（交代）」を選ぶ。
    /// 1体目の行動は決めたら戻せない（戻せるようにすると窓の行き来が増えるため）
    /// </summary>
    public partial class BattleController
    {
        /// <summary>場の順の行動。倒れて空いた場は技の 0 番（エンジンが使わない）</summary>
        private BattleAction[] _playerActions;
        private BattleAction _pendingAction;

        private IEnumerator WaitForPlayerActions()
        {
            _playerActions = new BattleAction[Player.ActiveCount];
            // 2体が同じ控えへ交代しないよう、先に選んだ交代先を外す
            var reserved = new List<int>();
            for (int slot = 0; slot < Player.ActiveCount; slot++)
            {
                if (Player.ActiveAt(slot).IsFainted)
                {
                    _playerActions[slot] = BattleAction.UseMove(0);
                    continue;
                }
                yield return WaitForAction(slot, reserved);
                _playerActions[slot] = _pendingAction;
                if (_pendingAction.Kind == BattleActionKind.Switch)
                {
                    reserved.Add(_pendingAction.Index);
                }
            }
        }

        private IEnumerator WaitForAction(int slot, List<int> reserved)
        {
            _pendingAction = null;
            var view = PlayerViewAt(slot);
            view.StartBob();
            ShowCommands(slot, reserved);
            // 時間切れは一番上の技。狙いは倒れていればエンジンがもう片方へ向ける
            StartChoiceTimer(() => _pendingAction = BattleAction.UseMove(0));
            yield return new WaitUntil(() => _pendingAction != null);
            _turnTimer.Stop();
            view.StopBob();
        }

        private MonsterView PlayerViewAt(int slot)
        {
            return IsDoubles ? _playerDoubleViews[slot] : _playerView;
        }

        private void ShowCommands(int slot, List<int> reserved)
        {
            _messageWindow.SetText($"{Player.ActiveAt(slot).Name}は どうする？");
            if (_story != null)
            {
                ShowStoryCommands(slot, reserved);
                return;
            }
            _commandPanel.Show(Player, reserved,
                onFight: () => ShowMoveSelect(slot, reserved),
                onParty: () => ShowSwitchSelect(slot, reserved));
        }

        private void ShowMoveSelect(int slot, List<int> reserved)
        {
            _movePanel.Show(Player.ActiveAt(slot), AliveFoes(),
                onMove: index => ChooseTarget(slot, index, reserved),
                onBack: () => ShowCommands(slot, reserved));
        }

        /// <summary>相手が2体いて、1体だけに当たる技のときだけ狙う相手を選ばせる</summary>
        private void ChooseTarget(int slot, int moveIndex, List<int> reserved)
        {
            var move = Player.ActiveAt(slot).Definition.Moves[moveIndex];
            var alive = Opponent.AliveSlots();
            if (move.HitsAllFoes || move.TargetsSelfOnly || alive.Count < 2)
            {
                _pendingAction = BattleAction.UseMove(moveIndex, alive.Count > 0 ? alive[0] : 0);
                return;
            }
            _targetPanel.Show(Opponent, move,
                onSelect: target => _pendingAction = BattleAction.UseMove(moveIndex, target),
                onBack: () => ShowMoveSelect(slot, reserved));
        }

        private void ShowSwitchSelect(int slot, List<int> reserved)
        {
            _partyPanel.Show(Player,
                onSelect: index => _pendingAction = BattleAction.SwitchTo(index),
                onBack: () => ShowCommands(slot, reserved),
                reserved);
        }

        private List<BattleMonster> AliveFoes()
        {
            var foes = new List<BattleMonster>();
            foreach (int slot in Opponent.AliveSlots())
            {
                foes.Add(Opponent.ActiveAt(slot));
            }
            return foes;
        }
    }
}
