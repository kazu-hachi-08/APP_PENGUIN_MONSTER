using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ターンを進める（仕様書 §4.1）。状態を書き換え、起きたことを BattleEvent の列で返す。
    /// 行動順は .Order.cs、状態異常・こんらん・ターン終わりは .Status.cs、技の効果は .Effects.cs、特性・持ち物は .Abilities.cs / .Items.cs
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

        /// <summary>バトル開始時、先頭のペンモンを場に出す。両方出てから、すばやさの高い側から場に出たときの特性</summary>
        public List<BattleEvent> StartBattle()
        {
            var events = new List<BattleEvent>();
            foreach (var side in _state.Sides)
            {
                events.Add(BattleEvent.Of(BattleEventKind.SentOut, side.Index, side.Active));
            }
            foreach (var side in SidesBySpeed())
            {
                OnSentOut(side.Index, events);
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
            if (!_state.IsOver)
            {
                ApplyEndOfTurn(events);
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
            var events = new List<BattleEvent> { BattleEvent.Of(BattleEventKind.SentOut, side, battleSide.Active) };
            OnSentOut(side, events);
            return events;
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
            battleSide.Active.OnWithdrawn();
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
            if (!TryAct(side, user, events))
            {
                return;
            }

            var move = user.Definition.Moves[moveIndex];
            events.Add(BattleEvent.MoveUsed(side, user, move));

            if (!move.TargetsSelfOnly && !RollHit(user, target, move))
            {
                events.Add(BattleEvent.Of(BattleEventKind.Missed, side, user));
                return;
            }
            if (move.IsStatus)
            {
                ApplyStatusMove(side, move, events);
                return;
            }

            var result = DamageCalculator.Calculate(user, target, move, _random);
            ApplyDamage(1 - side, target, result, events);
            if (result.Effectiveness == 0f)
            {
                return;
            }
            ThawByFire(1 - side, target, move, events);
            ApplySecondaryEffects(side, move, events);
            AfterAttack(side, user, target, move, events);
        }

        /// <summary>命中 × 命中ランク ÷ 回避ランク（仕様書 §4.3）。ランクは差を -6〜+6 に収めて1つの倍率にする</summary>
        private bool RollHit(BattleMonster user, BattleMonster target, MoveDefinition move)
        {
            int stage = user.GetStage(StatKind.Accuracy) - target.GetStage(StatKind.Evasion);
            stage = Math.Max(-BattleBalance.MaxStatStage, Math.Min(BattleBalance.MaxStatStage, stage));
            return _random.Next(BattleBalance.AccuracyPercentMax) < StageMultiplier.ApplyAccuracy(move.Accuracy, stage);
        }

        private void ApplyDamage(int targetSide, BattleMonster target, DamageResult result, List<BattleEvent> events)
        {
            if (result.Effectiveness == 0f)
            {
                events.Add(BattleEvent.Of(BattleEventKind.NoEffect, targetSide, target));
                return;
            }

            int damage = result.Damage;
            bool endured = TryEndure(target, ref damage);
            target.TakeDamage(damage);
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
            if (endured)
            {
                events.Add(BattleEvent.Of(BattleEventKind.ItemEndured, targetSide, target));
            }
            CheckFainted(targetSide, target, events);
            CheckHpItem(targetSide, target, events);
        }

        /// <summary>技以外のダメージ（こんらんの自傷・どく・やけど・特性・持ち物の反動）。HPの変化と文を1つのイベントで表す</summary>
        private void ApplyDirectDamage(int side, BattleMonster monster, int damage, BattleEventKind kind, List<BattleEvent> events)
        {
            monster.TakeDamage(damage);
            events.Add(BattleEvent.Of(kind, side, monster));
            CheckFainted(side, monster, events);
            CheckHpItem(side, monster, events);
        }

        private void CheckFainted(int side, BattleMonster monster, List<BattleEvent> events)
        {
            if (monster.IsFainted)
            {
                events.Add(BattleEvent.Of(BattleEventKind.Fainted, side, monster));
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
