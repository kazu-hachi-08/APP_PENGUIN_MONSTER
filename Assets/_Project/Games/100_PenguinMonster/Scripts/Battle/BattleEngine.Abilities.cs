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
        /// <summary>場に出たとき。こわもて は場の相手全員の こうげき を下げる。天候を呼ぶ特性は天候を始める</summary>
        private void OnSentOut(int side, int slot, List<BattleEvent> events)
        {
            var monster = _state.Sides[side].ActiveAt(slot);
            if (monster.IsFainted)
            {
                return;
            }
            var weather = WeatherCalledBy(monster.Definition.Ability);
            if (weather != Weather.None)
            {
                CallWeather(side, monster, weather, events);
                return;
            }
            var foe = _state.Opponent(side);
            var foeSlots = foe.AliveSlots();
            // 両方ひんしの入れ替えで、相手がまだ出ていないときは空振りさせる
            if (!monster.HasAbility(Ability.Intimidate) || foeSlots.Count == 0)
            {
                return;
            }
            events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, side, monster));
            foreach (int foeSlot in foeSlots)
            {
                ChangeStage(1 - side, foe.ActiveAt(foeSlot), StatKind.Attack, BattleBalance.IntimidateStages, true, events);
            }
        }

        private static Weather WeatherCalledBy(Ability ability)
        {
            switch (ability)
            {
                case Ability.SnowCloud: return Weather.Snow;
                case Ability.RainCall: return Weather.Rain;
                case Ability.SunCall: return Weather.Sun;
                default: return Weather.None;
            }
        }

        /// <summary>すでに同じ天候なら特性の文も出さない。文は「〇〇の 雪雲呼び！」→「吹雪が 吹き始めた！」の順</summary>
        private void CallWeather(int side, BattleMonster monster, Weather weather, List<BattleEvent> events)
        {
            if (_state.Weather == weather)
            {
                return;
            }
            events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, side, monster));
            TryStartWeather(side, monster, weather, events);
        }

        /// <summary>ひと休み: 交代で引っ込むと回復する。引っ込めた後なので文は出さない（Withdrawn の HP に回復後の値が乗る）</summary>
        private static void RestOnWithdraw(BattleMonster leaving)
        {
            if (leaving.HasAbility(Ability.Rest) && !leaving.IsFainted)
            {
                leaving.Heal(leaving.MaxHp / BattleBalance.RestHealDenominator);
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
            if (!_state.IsOver && !user.IsFainted && user.HasAbility(Ability.Momentum))
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

        /// <summary>ターン終わりの特性。おいかぜのり は すばやさ が上がる。気まぐれ は能力が上下する</summary>
        private void ApplyEndOfTurnAbility(int side, BattleMonster monster, List<BattleEvent> events)
        {
            if (_state.IsOver || monster.IsFainted)
            {
                return;
            }
            if (monster.HasAbility(Ability.Tailwind))
            {
                RaiseByAbility(side, monster, StatKind.Speed, BattleBalance.TailwindStages, events);
            }
            else if (monster.HasAbility(Ability.Moody))
            {
                ApplyMoody(side, monster, events);
            }
        }

        // 気まぐれで動く能力。命中・回避まで動かすと運だけで決まる試合が増えるので、5つの能力に絞る
        private static readonly StatKind[] MoodyStats =
        {
            StatKind.Attack, StatKind.Defense, StatKind.SpAttack, StatKind.SpDefense, StatKind.Speed,
        };

        /// <summary>
        /// 気まぐれ: まだ上がる能力から1つを2段上げ、それ以外でまだ下がる能力から1つを1段下げる。
        /// 乱数はホストだけが引き、結果はランクの変化のイベントで送るので、オンラインでもずれない
        /// </summary>
        private void ApplyMoody(int side, BattleMonster monster, List<BattleEvent> events)
        {
            var raisable = new List<StatKind>();
            foreach (var stat in MoodyStats)
            {
                if (monster.GetStage(stat) < BattleBalance.MaxStatStage)
                {
                    raisable.Add(stat);
                }
            }
            var raised = raisable.Count > 0 ? raisable[_random.Next(raisable.Count)] : (StatKind?)null;
            var lowerable = new List<StatKind>();
            foreach (var stat in MoodyStats)
            {
                if (stat != raised && monster.GetStage(stat) > -BattleBalance.MaxStatStage)
                {
                    lowerable.Add(stat);
                }
            }
            var lowered = lowerable.Count > 0 ? lowerable[_random.Next(lowerable.Count)] : (StatKind?)null;
            if (raised == null && lowered == null)
            {
                return;
            }
            events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, side, monster));
            if (raised != null)
            {
                ChangeStage(side, monster, raised.Value, BattleBalance.MoodyRaiseStages, false, events);
            }
            if (lowered != null)
            {
                ChangeStage(side, monster, lowered.Value, BattleBalance.MoodyLowerStages, false, events);
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
