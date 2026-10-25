using System;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    /// <summary>釣り竿と水の出現表（PLAN_STORY_TOWN04.md フェーズ4）</summary>
    public class FishingTests
    {
        private const int Rolls = 2000;

        // (1,1) に立ち、右 (2,1) が水。下 (1,2) は雪
        private static readonly string[] PondRows =
        {
            "TTTT",
            "T.~T",
            "T..T",
            "TTTT",
        };

        private static FieldMap Pond(bool withFishing = true)
        {
            var map = new FieldMap("pond", PondRows);
            return withFishing ? map.WithFishing(EncounterTable.Fishing(new EncounterSlot(Chick, 3, 5, 1))) : map;
        }

        private static FieldPlayer PlayerFacing(FieldMap map, Direction facing)
        {
            return new FieldPlayer(new FieldWorld(new[] { map }), new FieldSpot(map.Id, new GridPos(1, 1), facing));
        }

        [Test]
        public void Give_HasRod()
        {
            var flags = new StoryFlags();
            Assert.IsFalse(FishingRod.Has(flags));

            FishingRod.Give(flags);

            Assert.IsTrue(FishingRod.Has(flags));
            Assert.IsTrue(flags.Has("town04.fishingRod"), "記録に入るフラグ名を変えない");
        }

        [Test]
        public void NoRod_DoesNotFish()
        {
            var player = PlayerFacing(Pond(), Direction.Right);
            var random = new Random(1);

            Assert.IsTrue(Enumerable.Range(0, 50).All(_ => player.Fish(false, random, out var wild) == FishResult.None && wild == null));
        }

        [Test]
        public void NotFacingWater_DoesNotFish()
        {
            var random = new Random(1);

            Assert.AreEqual(FishResult.None, PlayerFacing(Pond(), Direction.Down).Fish(true, random, out _), "雪");
            Assert.AreEqual(FishResult.None, PlayerFacing(Pond(), Direction.Up).Fish(true, random, out _), "木");
        }

        [Test]
        public void WaterWithoutTable_NothingHere()
        {
            var player = PlayerFacing(Pond(withFishing: false), Direction.Right);

            Assert.AreEqual(FishResult.NothingHere, player.Fish(true, new Random(1), out var wild));
            Assert.IsNull(wild);
        }

        [Test]
        public void Bite_AtFishBitePercent_WithTableSpecies()
        {
            var player = PlayerFacing(Pond(), Direction.Right);
            var random = new Random(1);
            int bites = 0;

            for (int i = 0; i < Rolls; i++)
            {
                var result = player.Fish(true, random, out var wild);
                Assert.AreEqual(result == FishResult.Bite, wild != null);
                if (result == FishResult.Bite)
                {
                    bites++;
                    Assert.AreEqual(Chick, wild.Species);
                    Assert.That(wild.Level, Is.InRange(3, 5));
                }
                else
                {
                    Assert.AreEqual(FishResult.Miss, result);
                }
            }

            // 2000回なら 50% から ±5% には収まる
            Assert.That(bites * StoryBalance.Percent / Rolls, Is.InRange(StoryBalance.FishBitePercent - 5, StoryBalance.FishBitePercent + 5));
        }

        [Test]
        public void FishingTable_UsesFishBitePercent()
        {
            Assert.AreEqual(StoryBalance.FishBitePercent, EncounterTable.Fishing(new EncounterSlot(Chick, 1, 1, 1)).RatePercent);
        }

        [Test]
        public void StoryWater_AllHaveFishingTable()
        {
            var watery = StoryWorld.Create().Maps
                .Where(map => Enumerable.Range(0, map.Width).Any(x => Enumerable.Range(0, map.Height)
                    .Any(y => map.TileAt(new GridPos(x, y)) == TileKind.Water)))
                .ToList();

            Assert.IsNotEmpty(watery);
            foreach (var map in watery)
            {
                Assert.IsNotNull(map.FishingEncounters, $"{map.Id} の水が釣れない");
            }
        }

        [Test]
        public void StoryFishingSpecies_AreSeaAndInSpeciesTable()
        {
            foreach (var map in StoryWorld.Create().Maps.Where(map => map.FishingEncounters != null))
            {
                Assert.AreEqual(StoryBalance.FishBitePercent, map.FishingEncounters.RatePercent, map.Id);
                foreach (var slot in map.FishingEncounters.Slots)
                {
                    Assert.AreSame(slot.Species, StorySpeciesTable.ByNo(slot.Species.No), $"{map.Id} {slot.Species.Name}");
                    CollectionAssert.Contains(slot.Species.Definition.Types, MonsterType.Sea, $"{map.Id} {slot.Species.Name}");
                }
            }
        }
    }
}
