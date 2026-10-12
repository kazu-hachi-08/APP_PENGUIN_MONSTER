using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    public class StatsTests
    {
        [Test]
        public void FromBase_HpIsBasePlus75_OthersArePlus20()
        {
            var stats = Stats.FromBase(new Stats(100, 80, 60, 120, 40, 90));

            Assert.AreEqual(175, stats.Hp);
            Assert.AreEqual(100, stats.Attack);
            Assert.AreEqual(80, stats.Defense);
            Assert.AreEqual(140, stats.SpAttack);
            Assert.AreEqual(60, stats.SpDefense);
            Assert.AreEqual(110, stats.Speed);
        }
    }
}
