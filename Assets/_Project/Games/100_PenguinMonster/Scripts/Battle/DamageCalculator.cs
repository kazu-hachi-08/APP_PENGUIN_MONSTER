using System;

namespace PenguinMonster.Battle
{
    public readonly struct DamageResult
    {
        public readonly int Damage;
        public readonly bool IsCritical;
        public readonly float Effectiveness;
        /// <summary>使った側の特性がダメージを変えた（力持ちなど）。特性の帯を出すのに使う</summary>
        public readonly bool AttackerAbility;
        /// <summary>受けた側の特性がダメージを変えた・急所を防いだ（ふかふか脂肪・硬い羽など）</summary>
        public readonly bool DefenderAbility;

        public DamageResult(int damage, bool isCritical, float effectiveness, bool attackerAbility = false, bool defenderAbility = false)
        {
            Damage = damage;
            IsCritical = isCritical;
            Effectiveness = effectiveness;
            AttackerAbility = attackerAbility;
            DefenderAbility = defenderAbility;
        }
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    public static class DamageCalculator
    {
        /// <param name="targetMultiplier">全体技が2体に当たったときの倍率。特性・持ち物の倍率と一緒に最後に掛ける</param>
        public static DamageResult Calculate(BattleMonster attacker, BattleMonster defender, MoveDefinition move, Random random,
            float targetMultiplier = 1f)
        {
            float effectiveness = TypeChart.GetMultiplier(move.Type, defender.Definition.Types);
            if (effectiveness == 0f)
            {
                // こうかが ない技では急所・乱数を引かない（乱数の消費をそろえ、オンラインの再現をしやすくする）
                return new DamageResult(0, false, 0f);
            }

            // かたいはね でも急所の乱数は引く（特性で乱数の消費が変わらないようにする）
            bool criticalRolled = random.Next(BattleBalance.CriticalChanceDenominator) == 0;
            bool criticalBlocked = criticalRolled && defender.HasAbility(Ability.HardFeathers);
            bool isCritical = criticalRolled && !criticalBlocked;
            int randomPercent = RollRandomPercent(random);
            bool isPhysical = move.Category == MoveCategory.Physical;
            int attack = attacker.AttackStat(isPhysical, isCritical);
            int defense = DefenseInWeather(defender, isPhysical, defender.DefenseStat(isPhysical, isCritical));
            bool isStab = attacker.Definition.HasType(move.Type);

            int damage = Compute(move.Power, attack, defense, isCritical, randomPercent, isStab, effectiveness,
                IsBurnHalved(attacker, isPhysical), Modifier(attacker, defender, move, effectiveness) * targetMultiplier,
                attacker.Definition.Level);
            return new DamageResult(damage, isCritical, effectiveness,
                AttackerAbilityMultiplier(attacker, move) != 1f,
                criticalBlocked || DefenderAbilityMultiplier(defender, move) != 1f);
        }

        /// <summary>
        /// CPU が技を比べるための見込みダメージ。乱数は引かず（急所なし・乱数最大）、命中率を掛ける。
        /// 変化技と効かない技は 0
        /// </summary>
        public static float Expected(BattleMonster attacker, BattleMonster defender, MoveDefinition move, float targetMultiplier = 1f)
        {
            float effectiveness = TypeChart.GetMultiplier(move.Type, defender.Definition.Types);
            if (move.IsStatus || effectiveness == 0f)
            {
                return 0f;
            }

            bool isPhysical = move.Category == MoveCategory.Physical;
            int defense = DefenseInWeather(defender, isPhysical, defender.DefenseStat(isPhysical, false));
            int damage = Compute(move.Power, attacker.AttackStat(isPhysical, false), defense,
                false, BattleBalance.DamageRandomMaxPercent, attacker.Definition.HasType(move.Type), effectiveness,
                IsBurnHalved(attacker, isPhysical), Modifier(attacker, defender, move, effectiveness) * targetMultiplier,
                attacker.Definition.Level);
            return damage * ExpectedHits(attacker, move) * move.Accuracy / BattleBalance.AccuracyPercentMax;
        }

        /// <summary>連続技の見込みの回数。連打名人なら最大、2〜5回は原作の確率での平均</summary>
        private static float ExpectedHits(BattleMonster attacker, MoveDefinition move)
        {
            if (move.MinHits == move.MaxHits || attacker.HasAbility(Ability.SkillLink))
            {
                return move.MaxHits;
            }
            return BattleBalance.AverageMultiHits;
        }

        /// <summary>吹雪のとき、氷タイプは物理技を受ける防御が上がる（雪に埋もれて固くなるイメージ）</summary>
        private static int DefenseInWeather(BattleMonster defender, bool isPhysical, int defense)
        {
            bool snowGuard = isPhysical && defender.CurrentWeather == Weather.Snow && defender.Definition.HasType(MonsterType.Ice);
            return snowGuard ? (int)(defense * BattleBalance.SnowIceDefenseMultiplier) : defense;
        }

        /// <summary>
        /// こんらんで自分を攻撃したときのダメージ。原作と同じく威力40の物理で、タイプ・一致・急所は無し
        /// </summary>
        public static int SelfHit(BattleMonster monster, Random random)
        {
            return Compute(BattleBalance.ConfusionSelfHitPower, monster.AttackStat(true, false), monster.DefenseStat(true, false),
                false, RollRandomPercent(random), false, 1f, IsBurnHalved(monster, true), level: monster.Definition.Level);
        }

        private static int RollRandomPercent(Random random)
        {
            return random.Next(BattleBalance.DamageRandomMinPercent, BattleBalance.DamageRandomMaxPercent + 1);
        }

        private static bool IsBurnHalved(BattleMonster attacker, bool isPhysical)
        {
            return isPhysical && attacker.Status == StatusCondition.Burn;
        }

        /// <summary>特性・持ち物によるダメージの倍率をまとめたもの。1 なら変化なし</summary>
        private static float Modifier(BattleMonster attacker, BattleMonster defender, MoveDefinition move, float effectiveness)
        {
            return WeatherModifier(attacker.CurrentWeather, move.Type)
                * AttackerModifier(attacker, move, effectiveness) * DefenderModifier(defender, move, effectiveness);
        }

        /// <summary>大雨は海が強く火が弱い、日差しはその逆</summary>
        private static float WeatherModifier(Weather weather, MonsterType moveType)
        {
            bool sea = moveType == MonsterType.Sea;
            bool fire = moveType == MonsterType.Fire;
            if ((weather == Weather.Rain && sea) || (weather == Weather.Sun && fire))
            {
                return BattleBalance.WeatherBoostMultiplier;
            }
            if ((weather == Weather.Rain && fire) || (weather == Weather.Sun && sea))
            {
                return BattleBalance.WeatherWeakenMultiplier;
            }
            return 1f;
        }

        private static float AttackerModifier(BattleMonster attacker, MoveDefinition move, float effectiveness)
        {
            float modifier = AttackerAbilityMultiplier(attacker, move);
            bool isPhysical = move.Category == MoveCategory.Physical;
            if ((isPhysical && attacker.HasItem(HeldItem.BraveFeather)) || (!isPhysical && attacker.HasItem(HeldItem.WiseFeather)))
            {
                modifier *= BattleBalance.FeatherMultiplier;
            }
            if (attacker.HasItem(HeldItem.LifeIcicle))
            {
                modifier *= BattleBalance.LifeIcicleMultiplier;
            }
            if (effectiveness > 1f && attacker.HasItem(HeldItem.SharpGlasses))
            {
                modifier *= BattleBalance.SharpGlassesMultiplier;
            }
            if (attacker.IsHelped)
            {
                modifier *= BattleBalance.HelpingHandMultiplier;
            }
            return modifier;
        }

        /// <summary>
        /// 使った側の特性の倍率。1 以外なら特性が働いたとして帯を出す（持ち物・手助けとは分けておく）。
        /// 特性は1体に1つなので、当てはまるのはどれか1つ
        /// </summary>
        private static float AttackerAbilityMultiplier(BattleMonster attacker, MoveDefinition move)
        {
            switch (attacker.Definition.Ability)
            {
                case Ability.LastStand:
                    bool pinched = attacker.CurrentHp <= attacker.MaxHp / BattleBalance.LastStandHpDenominator;
                    return pinched && attacker.Definition.HasType(move.Type) ? BattleBalance.LastStandMultiplier : 1f;
                case Ability.HugePower:
                    return move.Category == MoveCategory.Physical ? BattleBalance.HugePowerMultiplier : 1f;
                case Ability.Technician:
                    return move.Power <= BattleBalance.TechnicianPowerMax ? BattleBalance.TechnicianMultiplier : 1f;
                case Ability.Reckless:
                    return move.Recoils ? BattleBalance.RecklessMultiplier : 1f;
                case Ability.Avenger:
                    return (float)Math.Pow(BattleBalance.AvengerMultiplierPerFainted, attacker.FaintedAllies);
                default:
                    return 1f;
            }
        }

        /// <summary>受けた側の特性の倍率。1 以外なら特性が働いたとして帯を出す</summary>
        private static float DefenderAbilityMultiplier(BattleMonster defender, MoveDefinition move)
        {
            switch (defender.Definition.Ability)
            {
                case Ability.ThickFat:
                    return move.Type == MonsterType.Ice || move.Type == MonsterType.Fire ? BattleBalance.ThickFatMultiplier : 1f;
                case Ability.Multiscale:
                    // 連続技は2発目から満タンでなくなるので、1発目だけ軽くなる
                    return defender.IsFullHp ? BattleBalance.MultiscaleMultiplier : 1f;
                default:
                    return 1f;
            }
        }

        private static float DefenderModifier(BattleMonster defender, MoveDefinition move, float effectiveness)
        {
            float modifier = DefenderAbilityMultiplier(defender, move);
            if (effectiveness > 1f && defender.HasItem(HeldItem.SnowCoat))
            {
                modifier *= BattleBalance.SnowCoatMultiplier;
            }
            return modifier;
        }

        /// <summary>
        /// 乱数を引き終えた値からダメージを出す。テストで期待値と突き合わせられるよう分けてある。
        /// 本家と同じく段階ごとに切り捨てる（掛ける順で1ずれることがあるため順番を変えないこと）。
        /// 特性・持ち物の倍率（modifier）は本家の「その他の補正」と同じく最後に掛ける。level は攻撃する側のレベル
        /// </summary>
        public static int Compute(int power, int attack, int defense, bool isCritical, int randomPercent, bool isStab, float effectiveness,
            bool isBurned = false, float modifier = 1f, int level = BattleBalance.Level)
        {
            if (effectiveness == 0f)
            {
                return 0;
            }

            int damage = BattleBalance.LevelFactor(level) * power * attack / defense / BattleBalance.DamageDivisor + BattleBalance.DamageBase;
            if (isCritical)
            {
                damage = (int)(damage * BattleBalance.CriticalMultiplier);
            }
            damage = damage * randomPercent / BattleBalance.DamageRandomMaxPercent;
            if (isStab)
            {
                damage = (int)(damage * BattleBalance.StabMultiplier);
            }
            damage = (int)(damage * effectiveness);
            if (isBurned)
            {
                damage = (int)(damage * BattleBalance.BurnPhysicalMultiplier);
            }
            damage = (int)(damage * modifier);

            // 効果がある技は最低1ダメージ入る
            return Math.Max(1, damage);
        }
    }
}
