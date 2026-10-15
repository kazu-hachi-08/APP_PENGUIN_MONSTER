using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    public class StatsTests
    {
        private static readonly Stats Base = new Stats(100, 80, 60, 120, 40, 90);

        [Test]
        public void FromBase_HpIsBasePlus75_OthersArePlus20()
        {
            var stats = Stats.FromBase(Base);

            Assert.AreEqual(175, stats.Hp);
            Assert.AreEqual(100, stats.Attack);
            Assert.AreEqual(80, stats.Defense);
            Assert.AreEqual(140, stats.SpAttack);
            Assert.AreEqual(60, stats.SpDefense);
            Assert.AreEqual(110, stats.Speed);
        }

        [Test]
        public void FromBase_Lv50_SameAsDefault()
        {
            Assert.AreEqual(Stats.FromBase(Base), Stats.FromBase(Base, 50));
        }

        [Test]
        public void FromBase_Lv5_MatchesMainSeriesFormula()
        {
            // HP ⌊231×5÷100⌋＋5＋10 = 26 / 攻撃 ⌊191×5÷100⌋＋5 = 14 …
            var stats = Stats.FromBase(Base, 5);

            Assert.AreEqual(26, stats.Hp);
            Assert.AreEqual(14, stats.Attack);
            Assert.AreEqual(12, stats.Defense);
            Assert.AreEqual(18, stats.SpAttack);
            Assert.AreEqual(10, stats.SpDefense);
            Assert.AreEqual(15, stats.Speed);
        }

        [Test]
        public void FromBase_Lv100_MatchesMainSeriesFormula()
        {
            // HP 231＋100＋10 = 341 / 攻撃 191＋5 = 196 …
            var stats = Stats.FromBase(Base, 100);

            Assert.AreEqual(341, stats.Hp);
            Assert.AreEqual(196, stats.Attack);
            Assert.AreEqual(156, stats.Defense);
            Assert.AreEqual(276, stats.SpAttack);
            Assert.AreEqual(116, stats.SpDefense);
            Assert.AreEqual(216, stats.Speed);
        }

        [Test]
        public void BattleMonster_UsesDefinitionLevel()
        {
            var lv5 = TestMonsters.Create("A", 50, TestMonsters.Strike).WithLoadout(5, new[] { TestMonsters.Strike }, HeldItem.None);

            var monster = new BattleMonster(lv5);

            Assert.AreEqual(Stats.FromBase(lv5.BaseStats, 5), monster.Stats);
            Assert.AreEqual(monster.MaxHp, monster.CurrentHp);
        }

        [Test]
        public void BattleMonster_StartsWithGivenHpAndStatus()
        {
            var monster = new BattleMonster(TestMonsters.Create("A", 50, TestMonsters.Strike), 30, StatusCondition.Burn);

            Assert.AreEqual(30, monster.CurrentHp);
            Assert.AreEqual(StatusCondition.Burn, monster.Status);
        }

        [Test]
        public void WithLoadout_ReplacesLevelMovesItem_WithItemKeepsLevel()
        {
            var moves = new[] { TestMonsters.Strike };
            var loadout = TestMonsters.Create("A", 50, TestMonsters.Strike).WithLoadout(12, moves, HeldItem.SnowCoat);

            Assert.AreEqual(12, loadout.Level);
            Assert.AreSame(moves, loadout.Moves);
            Assert.AreEqual(HeldItem.SnowCoat, loadout.Item);
            Assert.AreEqual(12, loadout.WithItem(HeldItem.None).Level);
        }
    }
}
