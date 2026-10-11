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

            bool isCritical = random.Next(BattleBalance.CriticalChanceDenominator) == 0;
            int randomPercent = RollRandomPercent(random);
            bool isPhysical = move.Category == MoveCategory.Physical;
            int attack = attacker.AttackStat(isPhysical, isCritical);
            int defense = defender.DefenseStat(isPhysical, isCritical);
            bool isStab = attacker.Definition.HasType(move.Type);

            int damage = Compute(move.Power, attack, defense, isCritical, randomPercent, isStab, effectiveness,
                IsBurnHalved(attacker, isPhysical));
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
                IsBurnHalved(attacker, isPhysical));
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

        /// <summary>
        /// 乱数を引き終えた値からダメージを出す。テストで期待値と突き合わせられるよう分けてある。
        /// 本家と同じく段階ごとに切り捨てる（掛ける順で1ずれることがあるため順番を変えないこと）
        /// </summary>
        public static int Compute(int power, int attack, int defense, bool isCritical, int randomPercent, bool isStab, float effectiveness,
            bool isBurned = false)
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

            // 効果がある技は最低1ダメージ入る
            return Math.Max(1, damage);
        }
    }
}
