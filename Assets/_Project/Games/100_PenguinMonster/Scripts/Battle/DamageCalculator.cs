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
            int randomPercent = random.Next(BattleBalance.DamageRandomMinPercent, BattleBalance.DamageRandomMaxPercent + 1);
            bool isPhysical = move.Category == MoveCategory.Physical;
            int attack = isPhysical ? attacker.Stats.Attack : attacker.Stats.SpAttack;
            int defense = isPhysical ? defender.Stats.Defense : defender.Stats.SpDefense;
            bool isStab = attacker.Definition.HasType(move.Type);

            int damage = Compute(move.Power, attack, defense, isCritical, randomPercent, isStab, effectiveness);
            return new DamageResult(damage, isCritical, effectiveness);
        }

        /// <summary>
        /// 乱数を引き終えた値からダメージを出す。テストで期待値と突き合わせられるよう分けてある。
        /// 本家と同じく段階ごとに切り捨てる（掛ける順で1ずれることがあるため順番を変えないこと）
        /// </summary>
        public static int Compute(int power, int attack, int defense, bool isCritical, int randomPercent, bool isStab, float effectiveness)
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

            // 効果がある技は最低1ダメージ入る
            return Math.Max(1, damage);
        }
    }
}
