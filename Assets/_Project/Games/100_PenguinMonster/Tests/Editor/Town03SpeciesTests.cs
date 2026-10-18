using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り3の30体（PLAN_STORY_TOWN03.md §1・フェーズ3）の定義表がつながっていること</summary>
    public class Town03SpeciesTests
    {
        private const int FirstNo = 211;
        private const int Count = 30;
        private const int Town01FirstNo = 151;
        private const int YoungMinTotal = 250;
        private const int YoungMaxTotal = 330;
        private const int FinalMinTotal = 425;
        private const int FinalMaxTotal = 530;
        private const int GymLeaderAceLevel = 26;
        private const int GiftLevel = 20;
        private const int PowerPlantLevel = 24;

        [Test]
        public void Species_Are211To240InOrderAndMatchStoryTable()
        {
            CollectionAssert.AreEqual(Enumerable.Range(FirstNo, Count), Town03Species.All.Select(species => species.No));
            CollectionAssert.AreEqual(MonsterTable.Story.Skip(FirstNo - Town01FirstNo).Take(Count), Town03Species.All.Select(species => species.Definition));
        }

        [Test]
        public void StoryMonsters_AreNotInDraft()
        {
            Assert.IsTrue(Town03Species.All.All(species => !MonsterTable.All.Contains(species.Definition)));
            Assert.IsTrue(Town03Species.All.All(species => MonsterTable.IsStory(species.No)));
        }

        [Test]
        public void StorySpeciesTable_FindsEveryStorySpeciesFrom151To240()
        {
            foreach (int no in Enumerable.Range(Town01FirstNo, FirstNo + Count - Town01FirstNo))
            {
                var species = StorySpeciesTable.ByNo(no);
                Assert.IsNotNull(species, no.ToString());
                Assert.AreEqual(no, species.No);
            }
            Assert.AreSame(Town03Species.SaintElmo, StorySpeciesTable.ByNo(240));
        }

        [Test]
        public void Evolutions_PointToLaterSpeciesAndTeachAMoveOnEvolving()
        {
            foreach (var species in Town03Species.All.Where(species => species.Evolution != null))
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
            foreach (var species in Town03Species.All)
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
        public void BaseStatTotals_FitTheBands()
        {
            foreach (var species in Town03Species.All)
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
            var evolvedNos = Town03Species.All.Where(species => species.Evolution != null).Select(species => species.Evolution.IntoNo).ToList();
            foreach (var species in Town03Species.All)
            {
                bool isFirstStage = !evolvedNos.Contains(species.No) && species.Evolution != null;
                Assert.AreEqual(isFirstStage, species.Definition.Rank == MonsterRank.Young, species.Name);
            }
        }

        [Test]
        public void GymLeaderAce_AtLv26_KnowsChargeDanceAndDischarge()
        {
            var ace = OwnedMonster.Create(Town03Species.Hatsuden, GymLeaderAceLevel);

            CollectionAssert.AreEqual(new[] { MoveTable.NumbWave, MoveTable.ChargeDance, MoveTable.SparkWing, MoveTable.Discharge }, ace.Moves);
            Assert.IsFalse(ace.CanEvolve);
        }

        [Test]
        public void Gift_AtLv20_DoesNotEvolve()
        {
            var gift = OwnedMonster.Create(Town03Species.Kantera, GiftLevel);

            CollectionAssert.AreEqual(new[] { MoveTable.Ember, MoveTable.Preen, MoveTable.FeatherWrap, MoveTable.Shout }, gift.Moves);
            Assert.IsNull(Town03Species.Kantera.Evolution);
        }

        [Test]
        public void PowerPlantMon_AtLv24_IsAStrongThunderMonThatDoesNotEvolve()
        {
            var mon = OwnedMonster.Create(Town03Species.Hesotori, PowerPlantLevel);

            CollectionAssert.AreEqual(new[] { MoveTable.ZapJab, MoveTable.SharpGlare, MoveTable.SparkBeak, MoveTable.Discharge }, mon.Moves);
            CollectionAssert.AreEqual(new[] { MonsterType.Thunder }, Town03Species.Hesotori.Definition.Types);
            Assert.IsNull(Town03Species.Hesotori.Evolution);
            // 「強めの子」として最後の進化の帯に入れる（捕まえればすぐ戦力になる）
            Assert.That(Town03Species.Hesotori.Definition.BaseStats.Total, Is.InRange(FinalMinTotal, FinalMaxTotal));
        }
    }
}
