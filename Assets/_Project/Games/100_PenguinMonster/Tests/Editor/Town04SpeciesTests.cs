using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り4の30体（PLAN_STORY_TOWN04.md §1・フェーズ5）の定義表がつながっていること</summary>
    public class Town04SpeciesTests
    {
        private const int FirstNo = 241;
        private const int Count = 30;
        private const int Town01FirstNo = 151;
        private const int YoungMinTotal = 250;
        private const int YoungMaxTotal = 330;
        private const int FinalMinTotal = 425;
        private const int FinalMaxTotal = 530;
        private const int GymLeaderAceLevel = 32;
        private const int RainbowLevel = 30;

        [Test]
        public void Species_Are241To270InOrderAndMatchStoryTable()
        {
            CollectionAssert.AreEqual(Enumerable.Range(FirstNo, Count), Town04Species.All.Select(species => species.No));
            CollectionAssert.AreEqual(MonsterTable.Story.Skip(FirstNo - Town01FirstNo).Take(Count), Town04Species.All.Select(species => species.Definition));
        }

        [Test]
        public void StoryMonsters_AreNotInDraft()
        {
            Assert.IsTrue(Town04Species.All.All(species => !MonsterTable.All.Contains(species.Definition)));
            Assert.IsTrue(Town04Species.All.All(species => MonsterTable.IsStory(species.No)));
        }

        [Test]
        public void StorySpeciesTable_FindsEveryStorySpeciesFrom151To270()
        {
            foreach (int no in Enumerable.Range(Town01FirstNo, FirstNo + Count - Town01FirstNo))
            {
                var species = StorySpeciesTable.ByNo(no);
                Assert.IsNotNull(species, no.ToString());
                Assert.AreEqual(no, species.No);
            }
            Assert.AreSame(Town04Species.Nijibane, StorySpeciesTable.ByNo(270));
        }

        [Test]
        public void Evolutions_PointToLaterSpeciesAndTeachAMoveOnEvolving()
        {
            foreach (var species in Town04Species.All.Where(species => species.Evolution != null))
            {
                var into = StorySpeciesTable.ByNo(species.Evolution.IntoNo);
                Assert.IsNotNull(into, species.Name);
                Assert.Greater(into.No, species.No, species.Name);
                Assert.IsTrue(into.MovesLearnedAt(species.Evolution.Level).Any(), into.Name);
            }
        }

        [Test]
        public void Learnsets_AreSortedStartAtLv1AndUseSavableMoves()
        {
            foreach (var species in Town04Species.All)
            {
                var levels = species.Learnset.Select(entry => entry.Level).ToList();
                CollectionAssert.IsOrdered(levels, species.Name);
                Assert.AreEqual(StoryBalance.MinLevel, levels[0], species.Name);
                CollectionAssert.AllItemsAreUnique(species.Learnset.Select(entry => entry.Move), species.Name);
                // 記録は技を名前で持ち、オンラインは技を MoveTable.All の番号で送るので、どちらからも引けないといけない
                Assert.IsTrue(species.Learnset.All(entry => MoveTable.ByName(entry.Move.Name) == entry.Move), species.Name);
                Assert.IsTrue(species.Learnset.All(entry => MoveTable.IndexOf(entry.Move) >= 0), species.Name);
            }
        }

        [Test]
        public void NewStatDownMoves_AreLearnedBySomeone()
        {
            // §7 の4つの技は区切り4の子が覚える（覚える子がいないと、ストーリーで出てこない）
            var learned = Town04Species.All.SelectMany(species => species.Learnset.Select(entry => entry.Move)).ToList();
            foreach (var move in new[] { MoveTable.BlankStare, MoveTable.ShrillVoice, MoveTable.Headwind, MoveTable.SeeThrough })
            {
                Assert.Contains(move, learned, move.Name);
            }
        }

        [Test]
        public void BaseStatTotals_FitTheBands()
        {
            foreach (var species in Town04Species.All)
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
            var evolvedNos = Town04Species.All.Where(species => species.Evolution != null).Select(species => species.Evolution.IntoNo).ToList();
            foreach (var species in Town04Species.All)
            {
                bool isFirstStage = !evolvedNos.Contains(species.No) && species.Evolution != null;
                Assert.AreEqual(isFirstStage, species.Definition.Rank == MonsterRank.Young, species.Name);
            }
        }

        [Test]
        public void GymLeaderAce_AtLv32_KnowsHeadwindAndDoesNotEvolveYet()
        {
            var ace = OwnedMonster.Create(Town04Species.Tsumuji, GymLeaderAceLevel);

            CollectionAssert.AreEqual(new[] { MoveTable.Headwind, MoveTable.FlapAttack, MoveTable.NorthWindCall, MoveTable.WindCutter }, ace.Moves);
            Assert.IsFalse(ace.CanEvolve);
        }

        [Test]
        public void Rainbow_IsTheOnlyBirdAuroraAndNotStrongerThanFinalEvolutions()
        {
            var rainbow = Town04Species.Nijibane;
            var birdAurora = MonsterTable.Dex.Where(monster => monster.HasType(MonsterType.Bird) && monster.HasType(MonsterType.Aurora)
                && MonsterTable.IsStory(monster.No));
            CollectionAssert.AreEqual(new[] { rainbow.Definition }, birdAurora);
            Assert.IsNull(rainbow.Evolution);

            // 珍しさ重視の子なので、区切り4の最後の進化の子より強くしない
            int strongestFinal = Town04Species.All
                .Where(species => species.Evolution == null && IsEvolvedInto(species))
                .Max(species => species.Definition.BaseStats.Total);
            Assert.LessOrEqual(rainbow.Definition.BaseStats.Total, strongestFinal);
        }

        [Test]
        public void Rainbow_AtLv30_KnowsWindAndAuroraMoves()
        {
            var mon = OwnedMonster.Create(Town04Species.Nijibane, RainbowLevel);

            CollectionAssert.AreEqual(new[] { MoveTable.AuroraHeal, MoveTable.Headwind, MoveTable.AuroraWave, MoveTable.WindCutter }, mon.Moves);
        }

        private static bool IsEvolvedInto(StorySpecies species)
            => Town04Species.All.Any(other => other.Evolution != null && other.Evolution.IntoNo == species.No);
    }
}
