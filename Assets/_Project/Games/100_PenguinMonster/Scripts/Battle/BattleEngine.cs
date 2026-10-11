using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ターンを進める（仕様書 §4.1）。状態を書き換え、起きたことを BattleEvent の列で返す。
    /// 行動順の決め方は BattleEngine.Order.cs
    /// </summary>
    public sealed partial class BattleEngine
    {
        private readonly BattleState _state;
        private readonly Random _random;

        public BattleState State => _state;

        public BattleEngine(BattleState state, Random random)
        {
            _state = state;
            _random = random;
        }

        /// <summary>バトル開始時、先頭のペンモンを場に出す</summary>
        public List<BattleEvent> StartBattle()
        {
            var events = new List<BattleEvent>();
            foreach (var side in _state.Sides)
            {
                events.Add(BattleEvent.Of(BattleEventKind.SentOut, side.Index, side.Active));
            }
            return events;
        }

        public List<BattleEvent> ResolveTurn(BattleAction action0, BattleAction action1)
        {
            if (_state.IsOver)
            {
                throw new InvalidOperationException("バトルは終わっています");
            }

            var events = new List<BattleEvent>();
            foreach (var queued in OrderActions(action0, action1))
            {
                if (_state.IsOver)
                {
                    break;
                }
                Execute(queued.Side, queued.Action, events);
            }
            return events;
        }

        /// <summary>ひんしになった場のペンモンを入れ替える</summary>
        public List<BattleEvent> Replace(int side, int partyIndex)
        {
            var battleSide = _state.Sides[side];
            if (!battleSide.CanSwitchTo(partyIndex))
            {
                throw new ArgumentException($"選出の {partyIndex} 番目は出せません", nameof(partyIndex));
            }

            battleSide.SetActive(partyIndex);
            return new List<BattleEvent> { BattleEvent.Of(BattleEventKind.SentOut, side, battleSide.Active) };
        }

        private void Execute(int side, BattleAction action, List<BattleEvent> events)
        {
            if (action.Kind == BattleActionKind.Switch)
            {
                ExecuteSwitch(side, action.Index, events);
            }
            else
            {
                ExecuteMove(side, action.Index, events);
            }
        }

        private void ExecuteSwitch(int side, int partyIndex, List<BattleEvent> events)
        {
            var battleSide = _state.Sides[side];
            events.Add(BattleEvent.Of(BattleEventKind.Withdrawn, side, battleSide.Active));
            events.AddRange(Replace(side, partyIndex));
        }

        private void ExecuteMove(int side, int moveIndex, List<BattleEvent> events)
        {
            var user = _state.Sides[side].Active;
            var target = _state.Opponent(side).Active;
            // 先に倒されたペンモンは行動しない
            if (user.IsFainted || target.IsFainted)
            {
                return;
            }

            var move = user.Definition.Moves[moveIndex];
            events.Add(BattleEvent.MoveUsed(side, user, move));

            if (_random.Next(BattleBalance.AccuracyPercentMax) >= move.Accuracy)
            {
                events.Add(BattleEvent.Of(BattleEventKind.Missed, side, user));
                return;
            }

            ApplyDamage(1 - side, target, DamageCalculator.Calculate(user, target, move, _random), events);
        }

        private void ApplyDamage(int targetSide, BattleMonster target, DamageResult result, List<BattleEvent> events)
        {
            if (result.Effectiveness == 0f)
            {
                events.Add(BattleEvent.Of(BattleEventKind.NoEffect, targetSide, target));
                return;
            }

            target.TakeDamage(result.Damage);
            events.Add(BattleEvent.Of(BattleEventKind.Damaged, targetSide, target));
            if (result.IsCritical)
            {
                events.Add(BattleEvent.Of(BattleEventKind.Critical, targetSide, target));
            }
            if (result.Effectiveness > 1f)
            {
                events.Add(BattleEvent.Of(BattleEventKind.SuperEffective, targetSide, target));
            }
            else if (result.Effectiveness < 1f)
            {
                events.Add(BattleEvent.Of(BattleEventKind.NotVeryEffective, targetSide, target));
            }

            if (target.IsFainted)
            {
                events.Add(BattleEvent.Of(BattleEventKind.Fainted, targetSide, target));
                CheckBattleEnd(events);
            }
        }

        private void CheckBattleEnd(List<BattleEvent> events)
        {
            foreach (var side in _state.Sides)
            {
                if (!side.HasRemaining)
                {
                    int winner = 1 - side.Index;
                    _state.Finish(winner);
                    events.Add(BattleEvent.BattleEnd(winner));
                    return;
                }
            }
        }
    }
}
