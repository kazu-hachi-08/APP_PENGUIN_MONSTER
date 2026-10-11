using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 技の効果（状態異常にする・こんらんさせる・ランクを変える）。
    /// 変化技は効果そのもので、決まらなければ文を出す。攻撃技の追加効果は確率で起き、決まらなくても何も出さない
    /// </summary>
    public sealed partial class BattleEngine
    {
        private void ApplyStatusMove(int side, MoveDefinition move, List<BattleEvent> events)
        {
            foreach (var effect in move.Effects)
            {
                ApplyEffect(side, effect, true, events);
            }
        }

        private void ApplySecondaryEffects(int side, MoveDefinition move, List<BattleEvent> events)
        {
            foreach (var effect in move.Effects)
            {
                // 決着がついた後や、受ける側がたおれた後は何も起こさない
                if (_state.IsOver)
                {
                    return;
                }
                if (Receiver(side, effect).IsFainted)
                {
                    continue;
                }
                if (effect.Chance >= MoveEffect.AlwaysPercent || _random.Next(MoveEffect.AlwaysPercent) < effect.Chance)
                {
                    ApplyEffect(side, effect, false, events);
                }
            }
        }

        private void ApplyEffect(int side, MoveEffect effect, bool reportFailure, List<BattleEvent> events)
        {
            int receiverSide = effect.Target == EffectTarget.Self ? side : 1 - side;
            var receiver = _state.Sides[receiverSide].Active;
            if (effect.Stages != 0)
            {
                ChangeStage(receiverSide, receiver, effect.Stat, effect.Stages, reportFailure, events);
            }
            else if (effect.Confuses)
            {
                TryConfuse(receiverSide, receiver, reportFailure, events);
            }
            else
            {
                TryInflict(receiverSide, receiver, effect.Status, reportFailure, events);
            }
        }

        private BattleMonster Receiver(int side, MoveEffect effect)
        {
            return effect.Target == EffectTarget.Self ? _state.Sides[side].Active : _state.Opponent(side).Active;
        }

        private void TryInflict(int side, BattleMonster receiver, StatusCondition status, bool reportFailure, List<BattleEvent> events)
        {
            if (!receiver.CanReceive(status))
            {
                AddFailed(side, receiver, reportFailure, events);
                return;
            }
            int sleepTurns = status == StatusCondition.Sleep
                ? _random.Next(BattleBalance.SleepMinTurns, BattleBalance.SleepMaxTurns + 1)
                : 0;
            receiver.SetStatus(status, sleepTurns);
            events.Add(BattleEvent.Of(BattleEventKind.StatusInflicted, side, receiver));
        }

        private void TryConfuse(int side, BattleMonster receiver, bool reportFailure, List<BattleEvent> events)
        {
            if (receiver.IsConfused)
            {
                AddFailed(side, receiver, reportFailure, events);
                return;
            }
            receiver.Confuse(_random.Next(BattleBalance.ConfusionMinTurns, BattleBalance.ConfusionMaxTurns + 1));
            events.Add(BattleEvent.Of(BattleEventKind.Confused, side, receiver));
        }

        private static void ChangeStage(int side, BattleMonster receiver, StatKind stat, int stages, bool reportFailure, List<BattleEvent> events)
        {
            int changed = receiver.ChangeStage(stat, stages);
            if (changed != 0)
            {
                events.Add(BattleEvent.StatChange(BattleEventKind.StatChanged, side, receiver, stat, changed));
            }
            else if (reportFailure)
            {
                events.Add(BattleEvent.StatChange(BattleEventKind.StatUnchanged, side, receiver, stat, stages));
            }
        }

        private static void AddFailed(int side, BattleMonster receiver, bool reportFailure, List<BattleEvent> events)
        {
            if (reportFailure)
            {
                events.Add(BattleEvent.Of(BattleEventKind.Failed, side, receiver));
            }
        }
    }
}
