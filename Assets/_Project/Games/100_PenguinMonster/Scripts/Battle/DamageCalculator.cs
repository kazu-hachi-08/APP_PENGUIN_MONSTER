using System;

namespace PenguinMonster.Battle
{
    public readonly struct DamageResult
    {
        public readonly int Damage;
        public readonly bool IsCritical;
        public readonly float Effectiveness;

        public DamageResult(int damage, bool isCritical, float effectiveness)
        {
            Damage = damage;
            IsCritical = isCritical;
            Effectiveness = effectiveness;
        }
    }

    /// <summary>
    /// ダメージ計算（仕様書 §4.3）
    /// </summary>
    public static class DamageCalculator
    {
        public static DamageResult Calculate(BattleMonster attacker, BattleMonster defender, MoveDefinition move, Random random)
        {
            float effectiveness = TypeChart.GetMultiplier(move.Type, defender.Definition.Types);
            if (effectiveness == 0f)
            {
                // こうかが ない技では急所・乱数を引かない（乱数の消費をそろえ、オンラインの再現をしやすくする）
                return new DamageResult(0, false, 0f);
            }

            // かたいはね でも急所の乱数は引く（特性で乱数の消費が変わらないようにする）
            bool isCritical = random.Next(BattleBalance.CriticalChanceDenominator) == 0 && !defender.HasAbility(Ability.HardFeathers);
            int randomPercent = RollRandomPercent(random);
            bool isPhysical = move.Category == MoveCategory.Physical;
            int attack = attacker.AttackStat(isPhysical, isCritical);
            int defense = defender.DefenseStat(isPhysical, isCritical);
            bool isStab = attacker.Definition.HasType(move.Type);

            int damage = Compute(move.Power, attack, defense, isCritical, randomPercent, isStab, effectiveness,
                IsBurnHalved(attacker, isPhysical), Modifier(attacker, defender, move, effectiveness));
            return new DamageResult(damage, isCritical, effectiveness);
        }

        /// <summary>
        /// CPU が技を比べるための見込みダメージ。乱数は引かず（急所なし・乱数最大）、命中率を掛ける。
        /// 変化技と効かない技は 0
        /// </summary>
        public static float Expected(BattleMonster attacker, BattleMonster defender, MoveDefinition move)
        {
            float effectiveness = TypeChart.GetMultiplier(move.Type, defender.Definition.Types);
            if (move.IsStatus || effectiveness == 0f)
            {
                return 0f;
            }

            bool isPhysical = move.Category == MoveCategory.Physical;
            int damage = Compute(move.Power, attacker.AttackStat(isPhysical, false), defender.DefenseStat(isPhysical, false),
                false, BattleBalance.DamageRandomMaxPercent, attacker.Definition.HasType(move.Type), effectiveness,
                IsBurnHalved(attacker, isPhysical), Modifier(attacker, defender, move, effectiveness));
            return damage * move.Accuracy / (float)BattleBalance.AccuracyPercentMax;
        }

        /// <summary>
        /// こんらんで自分を攻撃したときのダメージ。原作と同じく威力40の物理で、タイプ・一致・急所は無し
        /// </summary>
        public static int SelfHit(BattleMonster monster, Random random)
        {
            return Compute(BattleBalance.ConfusionSelfHitPower, monster.AttackStat(true, false), monster.DefenseStat(true, false),
                false, RollRandomPercent(random), false, 1f, IsBurnHalved(monster, true));
        }

        private static int RollRandomPercent(Random random)
        {
            return random.Next(BattleBalance.DamageRandomMinPercent, BattleBalance.DamageRandomMaxPercent + 1);
        }

        private static bool IsBurnHalved(BattleMonster attacker, bool isPhysical)
        {
            return isPhysical && attacker.Status == StatusCondition.Burn;
        }

        /// <summary>特性・持ち物によるダメージの倍率をまとめたもの（仕様書 §4.6）。1 なら変化なし</summary>
        private static float Modifier(BattleMonster attacker, BattleMonster defender, MoveDefinition move, float effectiveness)
        {
            return AttackerModifier(attacker, move, effectiveness) * DefenderModifier(defender, move, effectiveness);
        }

        private static float AttackerModifier(BattleMonster attacker, MoveDefinition move, float effectiveness)
        {
            float modifier = 1f;
            if (attacker.HasAbility(Ability.LastStand) && attacker.Definition.HasType(move.Type)
                && attacker.CurrentHp <= attacker.MaxHp / BattleBalance.LastStandHpDenominator)
            {
                modifier *= BattleBalance.LastStandMultiplier;
            }
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
            return modifier;
        }

        private static float DefenderModifier(BattleMonster defender, MoveDefinition move, float effectiveness)
        {
            float modifier = 1f;
            if (defender.HasAbility(Ability.ThickFat) && (move.Type == MonsterType.Ice || move.Type == MonsterType.Fire))
            {
                modifier *= BattleBalance.ThickFatMultiplier;
            }
            if (effectiveness > 1f && defender.HasItem(HeldItem.SnowCoat))
            {
                modifier *= BattleBalance.SnowCoatMultiplier;
            }
            return modifier;
        }

        /// <summary>
        /// 乱数を引き終えた値からダメージを出す。テストで期待値と突き合わせられるよう分けてある。
        /// 本家と同じく段階ごとに切り捨てる（掛ける順で1ずれることがあるため順番を変えないこと）。
        /// 特性・持ち物の倍率（modifier）は本家の「その他の補正」と同じく最後に掛ける
        /// </summary>
        public static int Compute(int power, int attack, int defense, bool isCritical, int randomPercent, bool isStab, float effectiveness,
            bool isBurned = false, float modifier = 1f)
        {
            if (effectiveness == 0f)
            {
                return 0;
            }

            int damage = BattleBalance.LevelFactor * power * attack / defense / BattleBalance.DamageDivisor + BattleBalance.DamageBase;
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
