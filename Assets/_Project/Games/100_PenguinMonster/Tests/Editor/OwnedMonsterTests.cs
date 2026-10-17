using NUnit.Framework;
using PenguinMonster.Battle;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    public class OwnedMonsterTests
    {
        [Test]
        public void New_StartsAtFullHpWithoutStatus()
        {
            var owned = Owned(Chick, 5);

            Assert.AreEqual(Stats.FromBase(Chick.Definition.BaseStats, 5).Hp, owned.CurrentHp);
            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(StatusCondition.None, owned.Status);
        }

        [Test]
        public void ToBattle_UsesOwnLevelMovesAndItem()
        {
            var owned = Owned(Chick, 7);
            owned.Item = HeldItem.LightFeather;

            var fighter = owned.ToBattle();

            Assert.AreEqual(7, fighter.Definition.Level);
            Assert.AreEqual(Stats.FromBase(Chick.Definition.BaseStats, 7), fighter.Stats);
            Assert.AreEqual(1, fighter.Definition.Moves.Count);
            Assert.AreEqual(Peck, fighter.Definition.Moves[0]);
            Assert.AreEqual(HeldItem.LightFeather, fighter.Definition.Item);
        }

        [Test]
        public void ApplyBattleResult_KeepsDamageAndStatusForNextBattle()
        {
            var owned = Owned(Chick, 10);
            var fighter = owned.ToBattle();
            fighter.TakeDamage(5);
            fighter.SetStatus(StatusCondition.Poison, 0);

            owned.ApplyBattleResult(fighter);
            var next = owned.ToBattle();

            Assert.AreEqual(owned.MaxHp - 5, next.CurrentHp);
            Assert.AreEqual(StatusCondition.Poison, next.Status);
        }
    }
}
