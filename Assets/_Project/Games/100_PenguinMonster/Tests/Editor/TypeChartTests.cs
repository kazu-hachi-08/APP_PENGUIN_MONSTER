using NUnit.Framework;
using static PenguinMonster.Battle.MonsterType;

namespace PenguinMonster.Battle.Tests
{
    public class TypeChartTests
    {
        [TestCase(Normal, Ice, 1f)]
        [TestCase(Ice, Moss, 2f)]
        [TestCase(Fire, Sea, 0.5f)]
        [TestCase(Normal, Aurora, 0f)]
        [TestCase(Thunder, Rock, 0f)]
        [TestCase(Dark, Normal, 2f)]
        public void SingleType(MonsterType attack, MonsterType defense, float expected)
        {
            Assert.AreEqual(expected, TypeChart.GetMultiplier(attack, defense));
        }

        [Test]
        public void DualType_DoubleWeakness_Is4x()
        {
            Assert.AreEqual(4f, TypeChart.GetMultiplier(Ice, new[] { Moss, Bird }));
        }

        [Test]
        public void DualType_DoubleResist_IsQuarter()
        {
            Assert.AreEqual(0.25f, TypeChart.GetMultiplier(Moss, new[] { Fire, Bird }));
        }

        [Test]
        public void DualType_WeakAndResist_CancelOut()
        {
            Assert.AreEqual(1f, TypeChart.GetMultiplier(Ice, new[] { Moss, Sea }));
        }

        [Test]
        public void DualType_ImmunityWins()
        {
            Assert.AreEqual(0f, TypeChart.GetMultiplier(Thunder, new[] { Sea, Rock }));
        }
    }
}
