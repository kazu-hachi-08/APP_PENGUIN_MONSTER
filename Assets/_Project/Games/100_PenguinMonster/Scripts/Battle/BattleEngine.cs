using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ターンを進める（仕様書 §4.1）。状態を書き換え、起きたことを BattleEvent の列で返す。
    /// 行動順は .Order.cs、技の対象と命中・ダメージは .Moves.cs、状態異常・こんらん・ターン終わりは .Status.cs、
    /// 技の効果は .Effects.cs、特性・持ち物は .Abilities.cs / .Items.cs、天候は .Weather.cs、守る・追い風・手助けは .Field.cs
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

        /// <summary>バトル開始時、先頭のペンモンを場に出す。全員出てから、すばやさの高い順に場に出たときの特性</summary>
        public List<BattleEvent> StartBattle()
        {
            var events = new List<BattleEvent>();
            foreach (var side in _state.Sides)
            {
                for (int slot = 0; slot < side.ActiveCount; slot++)
                {
                    events.Add(BattleEvent.Of(BattleEventKind.SentOut, side.Index, side.ActiveAt(slot)));
                }
            }
            foreach (var position in ActivesBySpeed())
            {
                OnSentOut(position.Side, position.Slot, events);
            }
            return events;
        }

        /// <summary>シングル用。お互い1つずつの行動でターンを進める</summary>
        public List<BattleEvent> ResolveTurn(BattleAction action0, BattleAction action1)
        {
            return ResolveTurn(new[] { action0 }, new[] { action1 });
        }

        /// <param name="actions0">陣営 0 の場の順の行動。倒れて空いた場のぶんは何でもよい（使わない）</param>
        public List<BattleEvent> ResolveTurn(IReadOnlyList<BattleAction> actions0, IReadOnlyList<BattleAction> actions1)
        {
            if (_state.IsOver)
            {
                throw new InvalidOperationException("バトルは終わっています");
            }

            var events = new List<BattleEvent>();
            foreach (var queued in OrderActions(actions0, actions1))
            {
                if (_state.IsOver)
                {
                    break;
                }
                Execute(queued.Side, queued.Slot, queued.Action, events);
            }
            if (!_state.IsOver)
            {
                ApplyEndOfTurn(events);
            }
            EndTurnGuards();
            return events;
        }

        /// <summary>シングル用。瀕死になった場のペンモンを入れ替える</summary>
        public List<BattleEvent> Replace(int side, int partyIndex) => Replace(side, 0, partyIndex);

        /// <summary>場の slot 番目に、選出の partyIndex 番目を出す（瀕死の入れ替え・交代・羽リレー）</summary>
        public List<BattleEvent> Replace(int side, int slot, int partyIndex)
        {
            var battleSide = _state.Sides[side];
            if (!battleSide.CanSwitchTo(partyIndex))
            {
                throw new ArgumentException($"選出の {partyIndex} 番目は出せません", nameof(partyIndex));
            }

            battleSide.SetActive(slot, partyIndex);
            var entering = battleSide.ActiveAt(slot);
            var events = new List<BattleEvent> { BattleEvent.Of(BattleEventKind.SentOut, side, entering) };
            OnSentOut(side, slot, events);
            // 吹雪などの中へ出てきたときも、速くなったことが分かるよう帯を出す。
            // バトル開始時は天候がまだ無く、天候が始まったときに ShowWeatherRiders で出すのでここだけでよい
            if (entering.RidesWeather && !entering.IsFainted)
            {
                ShowAbility(side, entering, events);
            }
            return events;
        }

        private void Execute(int side, int slot, BattleAction action, List<BattleEvent> events)
        {
            var battleSide = _state.Sides[side];
            if (action.Kind == BattleActionKind.Move)
            {
                ExecuteMove(side, slot, action, events);
            }
            // ダブルスで同じ控えを2体が選んでいたら、後の交代は取りやめる
            else if (!battleSide.ActiveAt(slot).IsFainted && battleSide.CanSwitchTo(action.Index))
            {
                ExecuteSwitch(side, slot, action.Index, events);
            }
        }

        private void ExecuteSwitch(int side, int slot, int partyIndex, List<BattleEvent> events)
        {
            var leaving = _state.Sides[side].ActiveAt(slot);
            leaving.OnWithdrawn();
            RestOnWithdraw(side, leaving, events);
            events.Add(BattleEvent.Of(BattleEventKind.Withdrawn, side, leaving));
            events.AddRange(Replace(side, slot, partyIndex));
        }

        private void ApplyDamage(int targetSide, BattleMonster target, DamageResult result, List<BattleEvent> events,
            bool reportEffectiveness = true)
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
            if (reportEffectiveness && result.Effectiveness > 1f)
            {
                events.Add(BattleEvent.Of(BattleEventKind.SuperEffective, targetSide, target));
            }
            else if (reportEffectiveness && result.Effectiveness < 1f)
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
