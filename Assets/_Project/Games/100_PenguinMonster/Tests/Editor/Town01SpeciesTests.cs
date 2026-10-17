using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り1の30体（PLAN_STORY_TOWN01.md §1・フェーズ8a）の定義表がつながっていること</summary>
    public class Town01SpeciesTests
    {
        private const int FirstNo = 151;
        private const int Count = 30;
        private const int YoungMinTotal = 250;
        private const int YoungMaxTotal = 330;
        private const int FinalMinTotal = 425;
        private const int FinalMaxTotal = 530;
        private const int GymLeaderAceLevel = 14;

        [Test]
        public void Species_Are151To180InOrderAndMatchStoryTable()
        {
            CollectionAssert.AreEqual(Enumerable.Range(FirstNo, Count), Town01Species.All.Select(species => species.No));
            CollectionAssert.AreEqual(MonsterTable.Story, Town01Species.All.Select(species => species.Definition));
            Assert.AreSame(MonsterTable.Story[0], MonsterTable.ByNo(FirstNo));
        }

        [Test]
        public void StoryMonsters_AreNotInDraftAndNamesAreUnique()
        {
            Assert.IsTrue(MonsterTable.All.All(monster => monster.No < FirstNo));
            var names = MonsterTable.All.Concat(MonsterTable.Story).Select(monster => monster.Name).ToList();
            CollectionAssert.AllItemsAreUnique(names);
        }

        [Test]
        public void Evolutions_PointToLaterSpeciesAndTeachAMoveOnEvolving()
        {
            foreach (var species in Town01Species.All.Where(species => species.Evolution != null))
            {
                var into = Town01Species.ByNo(species.Evolution.IntoNo);
                Assert.IsNotNull(into, species.Name);
                Assert.Greater(into.No, species.No, species.Name);
                Assert.IsTrue(into.MovesLearnedAt(species.Evolution.Level).Any(), into.Name);
            }
        }

        [Test]
        public void Learnsets_AreSortedStartAtLv1AndUseSavableMoves()
        {
            foreach (var species in Town01Species.All)
            {
                var levels = species.Learnset.Select(entry => entry.Level).ToList();
                CollectionAssert.IsOrdered(levels, species.Name);
                Assert.AreEqual(StoryBalance.MinLevel, levels[0], species.Name);
                CollectionAssert.AllItemsAreUnique(species.Learnset.Select(entry => entry.Move), species.Name);
                // 記録は技を名前で持つので、名前から引けないと読み込めない
                Assert.IsTrue(species.Learnset.All(entry => MoveTable.ByName(entry.Move.Name) == entry.Move), species.Name);
            }
        }

        [Test]
        public void BaseStatTotals_FitTheBands()
        {
            foreach (var species in Town01Species.All)
            {
                int total = species.Definition.BaseStats.Total;
                if (species.Definition.Rank == MonsterRank.Young)
                {
                    Assert.That(total, Is.InRange(YoungMinTotal, YoungMaxTotal), species.Name);
                }
                else if (species.Evolution == null)
                {
                    Assert.That(total, Is.InRange(FinalMinTotal, FinalMaxTotal), species.Name);
                }
            }
        }

        [Test]
        public void FirstStages_AreYoungAndEvolvedAreNot()
        {
            var evolvedNos = Town01Species.All.Where(species => species.Evolution != null).Select(species => species.Evolution.IntoNo).ToList();
            foreach (var species in Town01Species.All)
            {
                bool isFirstStage = !evolvedNos.Contains(species.No) && species.Evolution != null;
                Assert.AreEqual(isFirstStage, species.Definition.Rank == MonsterRank.Young, species.Name);
            }
        }

        [Test]
        public void GymLeaderAce_AtLv14_KnowsRockMovesAndSetup()
        {
            var ace = OwnedMonster.Create(Town01Species.Kawara, GymLeaderAceLevel);

            CollectionAssert.AreEqual(new[] { MoveTable.RockThrow, MoveTable.Fluff, MoveTable.SandSmoke, MoveTable.RockSkin }, ace.Moves);
            Assert.IsFalse(ace.CanEvolve);
        }
    }
}
