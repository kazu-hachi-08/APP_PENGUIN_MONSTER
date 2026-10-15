using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    public class DamageCalculatorTests
    {
        // 威力80・攻撃120・防御100 の基本ダメージ: 22×80×120÷100 = 2112 → ÷50 = 42 → +2 = 44
        private const int Power = 80;
        private const int Attack = 120;
        private const int Defense = 100;

        [Test]
        public void Basic_MaxRoll()
        {
            Assert.AreEqual(44, DamageCalculator.Compute(Power, Attack, Defense, false, 100, false, 1f));
        }

        [Test]
        public void MinRoll_IsFloored()
        {
            // 44 × 85 ÷ 100 = 37.4 → 37
            Assert.AreEqual(37, DamageCalculator.Compute(Power, Attack, Defense, false, 85, false, 1f));
        }

        [Test]
        public void Critical_Is1_5x()
        {
            Assert.AreEqual(66, DamageCalculator.Compute(Power, Attack, Defense, true, 100, false, 1f));
        }

        [Test]
        public void Stab_Is1_5x()
        {
            Assert.AreEqual(66, DamageCalculator.Compute(Power, Attack, Defense, false, 100, true, 1f));
        }

        [Test]
        public void StabAndSuperEffective()
        {
            // 44 → タイプ一致 66 → ばつぐん 132
            Assert.AreEqual(132, DamageCalculator.Compute(Power, Attack, Defense, false, 100, true, 2f));
        }

        [Test]
        public void AllModifiers_AppliedInOrder()
        {
            // 44 → 急所 66 → 乱数92% 60 → 一致 90 → いまひとつ 45
            Assert.AreEqual(45, DamageCalculator.Compute(Power, Attack, Defense, true, 92, true, 0.5f));
        }

        [Test]
        public void NoEffect_IsZero()
        {
            Assert.AreEqual(0, DamageCalculator.Compute(Power, Attack, Defense, false, 100, false, 0f));
        }

        [Test]
        public void Effective_IsAtLeastOne()
        {
            Assert.AreEqual(1, DamageCalculator.Compute(1, 1, 999, false, 85, false, 0.25f));
        }

        [Test]
        public void Level50_SameAsDefault()
        {
            Assert.AreEqual(44, DamageCalculator.Compute(Power, Attack, Defense, false, 100, false, 1f, level: 50));
        }

        [Test]
        public void Level_ChangesLevelFactor()
        {
            // Lv100: 42×80×120÷100 = 4032 → ÷50 = 80 → +2 = 82 / Lv5: 4×80×120÷100 = 384 → ÷50 = 7 → +2 = 9
            Assert.AreEqual(82, DamageCalculator.Compute(Power, Attack, Defense, false, 100, false, 1f, level: 100));
            Assert.AreEqual(9, DamageCalculator.Compute(Power, Attack, Defense, false, 100, false, 1f, level: 5));
        }

        [Test]
        public void Calculate_UsesAttackerLevel()
        {
            var attacker = new BattleMonster(TestMonsters.Create("A", 50, TestMonsters.Strike)
                .WithLoadout(10, new[] { TestMonsters.Strike }, HeldItem.None));
            var defender = new BattleMonster(TestMonsters.Create("B", 50, TestMonsters.Strike));

            var expectedRandom = new System.Random(1234);
            bool isCritical = expectedRandom.Next(BattleBalance.CriticalChanceDenominator) == 0;
            int roll = expectedRandom.Next(BattleBalance.DamageRandomMinPercent, BattleBalance.DamageRandomMaxPercent + 1);
            int expected = DamageCalculator.Compute(50, attacker.AttackStat(true, isCritical), defender.DefenseStat(true, isCritical),
                isCritical, roll, true, 1f, level: 10);

            var result = DamageCalculator.Calculate(attacker, defender, TestMonsters.Strike, new System.Random(1234));

            Assert.AreEqual(expected, result.Damage);
        }

        [Test]
        public void Calculate_WithSeededRandom_MatchesCompute()
        {
            var attacker = new BattleMonster(TestMonsters.Create("A", 50, TestMonsters.Strike, attack: 100));
            var defender = new BattleMonster(TestMonsters.Create("B", 50, TestMonsters.Strike, defense: 80));

            // 同じシードの乱数を、Calculate と同じ順（急所 → 乱数）で引いて期待値を作る
            var expectedRandom = new System.Random(1234);
            bool isCritical = expectedRandom.Next(BattleBalance.CriticalChanceDenominator) == 0;
            int roll = expectedRandom.Next(BattleBalance.DamageRandomMinPercent, BattleBalance.DamageRandomMaxPercent + 1);
            int expected = DamageCalculator.Compute(50, attacker.Stats.Attack, defender.Stats.Defense, isCritical, roll, true, 1f);

            var result = DamageCalculator.Calculate(attacker, defender, TestMonsters.Strike, new System.Random(1234));

            Assert.AreEqual(expected, result.Damage);
            Assert.AreEqual(isCritical, result.IsCritical);
        }
    }
}
