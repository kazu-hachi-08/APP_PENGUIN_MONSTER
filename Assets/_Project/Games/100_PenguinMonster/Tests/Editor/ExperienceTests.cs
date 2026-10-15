using NUnit.Framework;
using PenguinMonster.Battle;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    public class ExperienceTests
    {
        [TestCase(GrowthRate.MediumFast, 1, 0)]
        [TestCase(GrowthRate.MediumFast, 10, 1000)]
        [TestCase(GrowthRate.MediumFast, 100, 1000000)]
        [TestCase(GrowthRate.MediumSlow, 1, 0)]
        [TestCase(GrowthRate.MediumSlow, 2, 9)]
        [TestCase(GrowthRate.MediumSlow, 10, 560)]
        [TestCase(GrowthRate.MediumSlow, 100, 1059860)]
        public void ForLevel_MatchesMainSeriesTable(GrowthRate rate, int level, int expected)
        {
            Assert.AreEqual(expected, Experience.ForLevel(rate, level));
        }

        [Test]
        public void Defeated_IsBaseTimesLevelOverFive_TrainerIsOneAndHalf()
        {
            Assert.AreEqual(128, Experience.Defeated(64, 10, isTrainer: false));
            Assert.AreEqual(192, Experience.Defeated(64, 10, isTrainer: true));
        }

        [Test]
        public void GainExp_NotEnough_StaysAtLevel()
        {
            var owned = Owned(Chick, 5);

            var levelUps = owned.GainExp(Experience.ForLevel(GrowthRate.MediumFast, 6) - owned.Exp - 1);

            Assert.AreEqual(0, levelUps.Count);
            Assert.AreEqual(5, owned.Level);
        }

        [Test]
        public void GainExp_ReachesNextLevels_ReturnsEachLevelUpInOrder()
        {
            var owned = Owned(Chick, 5);

            var levelUps = owned.GainExp(Experience.ForLevel(GrowthRate.MediumFast, 8) - owned.Exp);

            Assert.AreEqual(8, owned.Level);
            Assert.AreEqual(3, levelUps.Count);
            Assert.AreEqual(6, levelUps[0].Level);
            Assert.AreEqual(Stats.FromBase(Chick.Definition.BaseStats, 5), levelUps[0].Before);
            Assert.AreEqual(Stats.FromBase(Chick.Definition.BaseStats, 6), levelUps[0].After);
            Assert.AreEqual(8, levelUps[2].Level);
        }

        [Test]
        public void GainExp_HurtMonster_HpGrowsByMaxHpGainAndKeepsDamage()
        {
            var owned = Owned(Chick, 20);
            Hurt(owned, 10);

            owned.GainExp(Experience.ForLevel(GrowthRate.MediumFast, 30) - owned.Exp);

            Assert.AreEqual(owned.MaxHp - 10, owned.CurrentHp);
        }

        [Test]
        public void GainExp_AtMaxLevel_StopsAtLevel100()
        {
            var owned = Owned(Chick, 99);

            owned.GainExp(int.MaxValue / 2);

            Assert.AreEqual(100, owned.Level);
            Assert.AreEqual(Experience.ForLevel(GrowthRate.MediumFast, 100), owned.Exp);
            Assert.AreEqual(0, owned.GainExp(100).Count);
        }

        private static void Hurt(OwnedMonster owned, int damage)
        {
            var fighter = owned.ToBattle();
            fighter.TakeDamage(damage);
            owned.ApplyBattleResult(fighter);
        }
    }
}
