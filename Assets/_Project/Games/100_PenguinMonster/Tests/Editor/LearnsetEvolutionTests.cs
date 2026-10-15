using NUnit.Framework;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    public class LearnsetEvolutionTests
    {
        [Test]
        public void Create_TakesMovesUpToLevel()
        {
            var owned = OwnedMonster.Create(Chick, 5);

            CollectionAssert.AreEqual(new[] { Peck, Growl }, owned.Moves);
        }

        [Test]
        public void Create_MoreThanFour_KeepsLatestFour()
        {
            var owned = OwnedMonster.Create(Chick, 13);

            CollectionAssert.AreEqual(new[] { Growl, Splash, Bite, Kick }, owned.Moves);
        }

        [Test]
        public void LevelUp_UnderFourMoves_LearnsAutomatically()
        {
            var owned = OwnedMonster.Create(Chick, 5);

            var levelUps = GrowTo(owned, 7);

            CollectionAssert.AreEqual(new[] { Splash }, levelUps[1].Moves.Learned);
            CollectionAssert.IsEmpty(levelUps[1].Moves.ToChoose);
            CollectionAssert.AreEqual(new[] { Peck, Growl, Splash }, owned.Moves);
        }

        [Test]
        public void LevelUp_FourMoves_BecomesForgetChoice()
        {
            var owned = OwnedMonster.Create(Chick, 12);

            var levelUps = GrowTo(owned, 13);

            CollectionAssert.IsEmpty(levelUps[0].Moves.Learned);
            CollectionAssert.AreEqual(new[] { Kick }, levelUps[0].Moves.ToChoose);
            CollectionAssert.AreEqual(new[] { Peck, Growl, Splash, Bite }, owned.Moves);
        }

        [Test]
        public void ReplaceMove_ForgetsChosenSlot()
        {
            var owned = OwnedMonster.Create(Chick, 12);

            owned.ReplaceMove(0, Kick);

            CollectionAssert.AreEqual(new[] { Kick, Growl, Splash, Bite }, owned.Moves);
        }

        [Test]
        public void LevelUp_AlreadyKnownMove_IsSkipped()
        {
            var owned = new OwnedMonster(Chick, 6, new[] { Splash });

            var levelUps = GrowTo(owned, 7);

            CollectionAssert.IsEmpty(levelUps[0].Moves.Learned);
            CollectionAssert.IsEmpty(levelUps[0].Moves.ToChoose);
        }

        [Test]
        public void CanEvolve_OnlyFromEvolutionLevel()
        {
            var owned = Owned(Chick, 15);
            Assert.IsFalse(owned.CanEvolve);

            GrowTo(owned, 16);

            Assert.IsTrue(owned.CanEvolve);
        }

        [Test]
        public void CanEvolve_FinalStage_IsFalse()
        {
            Assert.IsFalse(Owned(Adult, 100).CanEvolve);
        }

        [Test]
        public void Evolve_BecomesNewSpeciesAndLearnsItsMoveAtThatLevel()
        {
            var owned = Owned(Chick, 16);

            var learning = owned.Evolve(Junior);

            Assert.AreEqual(902, owned.No);
            Assert.AreEqual(16, owned.Level);
            CollectionAssert.AreEqual(new[] { WingStrike }, learning.Learned);
            Assert.IsFalse(owned.CanEvolve);
        }

        [Test]
        public void Evolve_HurtMonster_HpGrowsByMaxHpGainAndKeepsDamage()
        {
            var owned = Owned(Chick, 16);
            Hurt(owned, 10);
            int maxHpBefore = owned.MaxHp;

            owned.Evolve(Junior);

            Assert.Greater(owned.MaxHp, maxHpBefore);
            Assert.AreEqual(owned.MaxHp - 10, owned.CurrentHp);
        }

        [Test]
        public void Evolve_KeepsExp()
        {
            var owned = Owned(Chick, 16);
            int exp = owned.Exp;

            owned.Evolve(Junior);

            Assert.AreEqual(exp, owned.Exp);
        }
    }
}
