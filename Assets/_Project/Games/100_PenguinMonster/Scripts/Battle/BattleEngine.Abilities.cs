using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 特性の発動。場に出たとき・攻撃のあと・ターン終わりに呼ぶ。
    /// ダメージの倍率になる特性（せとぎわ・ふかふかしぼう・かたいはね）は DamageCalculator、よふかし は StatusImmunity。
    /// 特性が働いたら必ず AbilityActivated を出し、画面に特性の帯を出す（文はその後の効果の文に任せる）
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
            ShowAbility(side, monster, events);
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
            ShowAbility(side, monster, events);
            TryStartWeather(side, monster, weather, events);
        }

        /// <summary>
        /// ひと休み: 交代で引っ込むと回復する。「戻れ！」の前に帯だけ出す（Withdrawn の HP に回復後の値が乗る）。
        /// 満タンなら何も起きないので帯も出さない
        /// </summary>
        private static void RestOnWithdraw(int side, BattleMonster leaving, List<BattleEvent> events)
        {
            if (leaving.HasAbility(Ability.Rest) && !leaving.IsFainted && !leaving.IsFullHp)
            {
                ShowAbility(side, leaving, events);
                leaving.Heal(leaving.MaxHp / BattleBalance.RestHealDenominator);
            }
        }

        /// <summary>特性の帯を出す。帯だけで、効果の文はその後のイベントが出す（倍率の特性は文なし）</summary>
        private static void ShowAbility(int side, BattleMonster monster, List<BattleEvent> events)
        {
            events.Add(BattleEvent.Of(BattleEventKind.AbilityActivated, side, monster));
        }

        /// <summary>
        /// ダメージを変えた特性の帯を、ダメージの前に出す。使った側は1回の技で1回、受けた側は1体につき1回だけ
        /// （連続技で毎回出るとくどいため）。連打名人もここで出す
        /// </summary>
        private static void ShowDamageAbilities(int side, BattleMonster user, BattleMonster target, MoveDefinition move,
            DamageResult result, ref bool attackerShown, ref bool defenderShown, List<BattleEvent> events)
        {
            if (result.Effectiveness == 0f)
            {
                return;
            }
            bool skillLink = user.HasAbility(Ability.SkillLink) && move.MinHits != move.MaxHits;
            if (!attackerShown && (result.AttackerAbility || skillLink))
            {
                ShowAbility(side, user, events);
                attackerShown = true;
            }
            if (!defenderShown && result.DefenderAbility)
            {
                ShowAbility(1 - side, target, events);
                defenderShown = true;
            }
        }

        /// <summary>天候が始まったとき、その天候で速くなる子（雪滑り・波乗り・光合成）の帯を出す</summary>
        private void ShowWeatherRiders(List<BattleEvent> events)
        {
            foreach (var battleSide in _state.Sides)
            {
                foreach (int slot in battleSide.AliveSlots())
                {
                    var monster = battleSide.ActiveAt(slot);
                    if (monster.RidesWeather)
                    {
                        ShowAbility(battleSide.Index, monster, events);
                    }
                }
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
                    ShowAbility(defenderSide, defender, events);
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
            ShowAbility(defenderSide, defender, events);
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
            ShowAbility(side, monster, events);
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
            ShowAbility(side, monster, events);
            ChangeStage(side, monster, stat, stages, false, events);
        }
    }
}
