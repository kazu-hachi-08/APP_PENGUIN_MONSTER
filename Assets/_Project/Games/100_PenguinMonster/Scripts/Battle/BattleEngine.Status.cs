using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 状態異常・こんらん（仕様書 §4.4）。技を出す前の判定と、ターン終わりのダメージ
    /// </summary>
    public sealed partial class BattleEngine
    {
        /// <summary>技を出す前の判定。ねむり・こおり → こんらん → まひ の順に見て、動けないなら false</summary>
        private bool TryAct(int side, BattleMonster user, List<BattleEvent> events)
        {
            return PassesSleepOrFreeze(side, user, events)
                && PassesConfusion(side, user, events)
                && PassesParalysis(side, user, events);
        }

        private bool PassesSleepOrFreeze(int side, BattleMonster user, List<BattleEvent> events)
        {
            if (user.Status == StatusCondition.Sleep)
            {
                if (user.TickSleep())
                {
                    events.Add(BattleEvent.Of(BattleEventKind.Immobilized, side, user));
                    return false;
                }
                user.CureStatus();
                events.Add(BattleEvent.Of(BattleEventKind.WokeUp, side, user));
            }
            else if (user.Status == StatusCondition.Freeze)
            {
                if (_random.Next(BattleBalance.FreezeThawDenominator) != 0)
                {
                    events.Add(BattleEvent.Of(BattleEventKind.Immobilized, side, user));
                    return false;
                }
                user.CureStatus();
                events.Add(BattleEvent.Of(BattleEventKind.Thawed, side, user));
            }
            return true;
        }

        private bool PassesConfusion(int side, BattleMonster user, List<BattleEvent> events)
        {
            if (!user.IsConfused)
            {
                return true;
            }
            if (!user.TickConfusion())
            {
                events.Add(BattleEvent.Of(BattleEventKind.ConfusionEnded, side, user));
                return true;
            }

            events.Add(BattleEvent.Of(BattleEventKind.ConfusionCheck, side, user));
            if (_random.Next(BattleBalance.ConfusionSelfHitDenominator) != 0)
            {
                return true;
            }
            ApplyDirectDamage(side, user, DamageCalculator.SelfHit(user, _random), BattleEventKind.HurtItself, events);
            return false;
        }

        private bool PassesParalysis(int side, BattleMonster user, List<BattleEvent> events)
        {
            if (user.Status != StatusCondition.Paralysis || _random.Next(BattleBalance.ParalysisImmobileDenominator) != 0)
            {
                return true;
            }
            events.Add(BattleEvent.Of(BattleEventKind.Immobilized, side, user));
            return false;
        }

        /// <summary>こおっている相手は ひ の技を受けると解ける</summary>
        private static void ThawByFire(int targetSide, BattleMonster target, MoveDefinition move, List<BattleEvent> events)
        {
            if (target.Status != StatusCondition.Freeze || move.Type != MonsterType.Fire || target.IsFainted)
            {
                return;
            }
            target.CureStatus();
            events.Add(BattleEvent.Of(BattleEventKind.Thawed, targetSide, target));
        }

        /// <summary>
        /// ターン終わり。先に天候の残りを数え、続いて場の全員を すばやさの高い順に（同速は陣営0から）1体ずつ、
        /// 吹雪のダメージ → どく・やけど のダメージ → 持ち物（ほしざかな）→ 特性（おいかぜのり）を全部済ませる
        /// </summary>
        private void ApplyEndOfTurn(List<BattleEvent> events)
        {
            AdvanceWeather(events);
            foreach (var position in ActivesBySpeed())
            {
                if (_state.IsOver)
                {
                    return;
                }
                var monster = _state.Sides[position.Side].ActiveAt(position.Slot);
                if (monster.IsFainted)
                {
                    continue;
                }
                ApplyWeatherDamage(position.Side, monster, events);
                if (_state.IsOver || monster.IsFainted)
                {
                    continue;
                }
                int damage = StatusDamage(monster);
                if (damage > 0)
                {
                    ApplyDirectDamage(position.Side, monster, damage, BattleEventKind.StatusDamage, events);
                }
                ApplyEndOfTurnItem(position.Side, monster, events);
                ApplyEndOfTurnAbility(position.Side, monster, events);
            }
        }

        /// <summary>状態異常によるターン終わりのダメージ。もうどくは呼ぶたびに段階が1つ上がる</summary>
        private static int StatusDamage(BattleMonster monster)
        {
            switch (monster.Status)
            {
                case StatusCondition.Poison:
                    return Math.Max(1, monster.MaxHp / BattleBalance.PoisonDamageDenominator);
                case StatusCondition.BadPoison:
                    return Math.Max(1, monster.MaxHp * monster.AdvanceBadPoison() / BattleBalance.BadPoisonDamageDenominator);
                case StatusCondition.Burn:
                    return Math.Max(1, monster.MaxHp / BattleBalance.BurnDamageDenominator);
                default:
                    return 0;
            }
        }
    }
}
