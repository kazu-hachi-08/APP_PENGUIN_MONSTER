using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 特性の発動（仕様書 §4.6）。場に出たとき・攻撃のあと・ターン終わりに呼ぶ。
    /// ダメージの倍率になる特性（せとぎわ・ふかふかしぼう・かたいはね）は DamageCalculator、よふかし は StatusImmunity
    /// </summary>
    public sealed partial class BattleEngine
    {
        /// <summary>場に出たとき。こわもて は場の相手全員の こうげき を下げる</summary>
        private void OnSentOut(int side, int slot, List<BattleEvent> events)
        {
            var monster = _state.Sides[side].ActiveAt(slot);
            var foe = _state.Opponent(side);
            var foeSlots = foe.AliveSlots();
            // 両方ひんしの入れ替えで、相手がまだ出ていないときは空振りさせる
            if (monster.IsFainted || !monster.HasAbility(Ability.Intimidate) || foeSlots.Count == 0)
            {
                return;
            }
            events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, side, monster));
            foreach (int foeSlot in foeSlots)
            {
                ChangeStage(1 - side, foe.ActiveAt(foeSlot), StatKind.Attack, BattleBalance.IntimidateStages, true, events);
            }
        }

        /// <summary>
        /// 攻撃技が相手1体に当たったあと。相手をたおした・物理で殴られた の特性。
        /// いのちのつらら の反動は、全体技でも技ごとに1回なので BattleEngine.Moves.cs で済ませる
        /// </summary>
        private void AfterHit(int side, BattleMonster user, BattleMonster target, MoveDefinition move, List<BattleEvent> events)
        {
            if (target.IsFainted)
            {
                OnKnockedOut(side, user, events);
            }
            if (move.Category == MoveCategory.Physical)
            {
                OnHitByPhysical(side, user, 1 - side, target, events);
            }
        }

        /// <summary>いきおいづく: 相手をたおしたら こうげき が上がる</summary>
        private void OnKnockedOut(int side, BattleMonster user, List<BattleEvent> events)
        {
            if (!_state.IsOver && user.HasAbility(Ability.Momentum))
            {
                RaiseByAbility(side, user, StatKind.Attack, BattleBalance.MomentumStages, events);
            }
        }

        /// <summary>物理技を受けた側の特性で、殴った側に何か起きる</summary>
        private void OnHitByPhysical(int attackerSide, BattleMonster attacker, int defenderSide, BattleMonster defender,
            List<BattleEvent> events)
        {
            if (_state.IsOver || attacker.IsFainted)
            {
                return;
            }
            switch (defender.Definition.Ability)
            {
                case Ability.StaticBody:
                    TryContactStatus(attackerSide, attacker, defenderSide, defender, StatusCondition.Paralysis, events);
                    break;
                case Ability.PoisonSlime:
                    TryContactStatus(attackerSide, attacker, defenderSide, defender, StatusCondition.Poison, events);
                    break;
                case Ability.SpikySkin:
                    events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, defenderSide, defender));
                    int damage = Math.Max(1, attacker.MaxHp / BattleBalance.SpikySkinDamageDenominator);
                    ApplyDirectDamage(attackerSide, attacker, damage, BattleEventKind.AbilityDamage, events);
                    break;
            }
        }

        /// <summary>かかる相手のときだけ確率を引き、決まったら特性の文 → 状態異常の順に出す</summary>
        private void TryContactStatus(int attackerSide, BattleMonster attacker, int defenderSide, BattleMonster defender,
            StatusCondition status, List<BattleEvent> events)
        {
            if (!attacker.CanReceive(status) || _random.Next(BattleBalance.PercentMax) >= BattleBalance.ContactAbilityPercent)
            {
                return;
            }
            events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, defenderSide, defender));
            TryInflict(attackerSide, attacker, status, false, events);
        }

        /// <summary>ターン終わりの特性。おいかぜのり は すばやさ が上がる</summary>
        private void ApplyEndOfTurnAbility(int side, BattleMonster monster, List<BattleEvent> events)
        {
            if (!_state.IsOver && !monster.IsFainted && monster.HasAbility(Ability.Tailwind))
            {
                RaiseByAbility(side, monster, StatKind.Speed, BattleBalance.TailwindStages, events);
            }
        }

        /// <summary>もう上がらないときは特性の文も出さない（毎ターン「もう あがらない」が出るとうるさいため）</summary>
        private static void RaiseByAbility(int side, BattleMonster monster, StatKind stat, int stages, List<BattleEvent> events)
        {
            if (monster.GetStage(stat) >= BattleBalance.MaxStatStage)
            {
                return;
            }
            events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, side, monster));
            ChangeStage(side, monster, stat, stages, false, events);
        }
    }
}
