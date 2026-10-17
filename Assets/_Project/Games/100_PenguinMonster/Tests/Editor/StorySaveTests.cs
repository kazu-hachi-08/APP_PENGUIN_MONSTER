using NUnit.Framework;
using PenguinMonster.Battle;
using UnityEngine;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    /// <summary>セーブ（PLAN_STORY_TOWN01.md フェーズ7）。View と同じく JsonUtility で JSON を通して確かめる</summary>
    public class StorySaveTests
    {
        private static readonly FieldSpot Standing = new FieldSpot(Town01Maps.Route01, new GridPos(5, 7), Direction.Left);
        private static readonly FieldSpot HutCounter = new FieldSpot(Town01Maps.WarmHut, new GridPos(3, 2), Direction.Up);

        private FieldWorld _world;

        [SetUp]
        public void SetUp()
        {
            _world = Town01Maps.CreateWorld();
        }

        private static StoryState PlayedState()
        {
            var state = new StoryState(Town01Maps.Start) { LastHealSpot = HutCounter };
            var starter = OwnedMonster.Create(Town01Species.SeaStarter, StoryBalance.StarterLevel);
            starter.GainExp(30);
            Hurt(starter, 4);
            starter.Item = HeldItem.LightFeather;
            var poisoned = starter.ToBattle();
            poisoned.SetStatus(StatusCondition.Poison, 0);
            starter.ApplyBattleResult(poisoned);
            state.Party.Add(starter);
            state.Box.Add(OwnedMonster.Create(Town01Species.WildBird, 3));
            state.Bag.Add(StoryItem.CaptureEgg, 5);
            state.Bag.Add(StoryItem.Potion, 2);
            state.SpendMoney(700);
            state.Flags.Set(Town01Flags.GotStarter);
            state.SeenNos.Add(39);
            state.CaughtNos.Add(39);
            state.Badges.Add("test.badge");
            return state;
        }

        private RestoredStory ThroughJson(StorySaveData data)
        {
            return FromJson(JsonUtility.ToJson(data));
        }

        private RestoredStory FromJson(string json)
        {
            var data = new StorySaveData();
            JsonUtility.FromJsonOverwrite(json, data);
            return StorySave.Restore(data, _world, Town01Maps.Start);
        }

        [Test]
        public void SaveThenLoad_RestoresSameContents()
        {
            var original = PlayedState();

            var restored = ThroughJson(StorySave.Capture(original, Standing));

            Assert.IsNotNull(restored);
            var state = restored.State;
            Assert.AreEqual(Standing, restored.Position);
            Assert.AreEqual(HutCounter, state.LastHealSpot);
            Assert.AreEqual(original.Money, state.Money);
            Assert.AreEqual(5, state.Bag.CountOf(StoryItem.CaptureEgg));
            Assert.AreEqual(2, state.Bag.CountOf(StoryItem.Potion));
            Assert.IsTrue(state.Flags.Has(Town01Flags.GotStarter));
            CollectionAssert.AreEquivalent(original.SeenNos, state.SeenNos);
            CollectionAssert.AreEquivalent(original.CaughtNos, state.CaughtNos);
            CollectionAssert.AreEquivalent(original.Badges, state.Badges);
            Assert.AreEqual(1, state.Box.Count);
            Assert.AreEqual(Town01Species.WildBird, state.Box.Members[0].Species);
            AssertSameMonster(original.Party.Members[0], state.Party.Members[0]);
        }

        private static void AssertSameMonster(OwnedMonster expected, OwnedMonster actual)
        {
            Assert.AreEqual(expected.Species, actual.Species);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Exp, actual.Exp);
            Assert.AreEqual(expected.CurrentHp, actual.CurrentHp);
            Assert.AreEqual(expected.Status, actual.Status);
            Assert.AreEqual(expected.Item, actual.Item);
            CollectionAssert.AreEqual(expected.Moves, actual.Moves);
        }

        [Test]
        public void Load_MissingItemsAreFilledWithDefaults()
        {
            var restored = FromJson("{\"Version\":1,\"Money\":500}");

            Assert.IsNotNull(restored);
            Assert.AreEqual(Town01Maps.Start, restored.Position);
            Assert.AreEqual(Town01Maps.Start, restored.State.LastHealSpot);
            Assert.AreEqual(500, restored.State.Money);
            Assert.AreEqual(0, restored.State.Party.Count);
            Assert.AreEqual(0, restored.State.Box.Count);
            Assert.AreEqual(0, restored.State.Bag.Items().Count);
            Assert.AreEqual(0, restored.State.Badges.Count);
        }

        [Test]
        public void Load_NewerVersionIsNotRead()
        {
            Assert.IsNull(FromJson("{\"Version\":" + (StorySaveData.CurrentVersion + 1) + "}"));
        }

        [Test]
        public void Load_UnknownSpeciesIsNotRead()
        {
            var data = StorySave.Capture(PlayedState(), Standing);
            data.Party[0].No = 999;

            Assert.IsNull(ThroughJson(data));
        }

        [Test]
        public void Load_UnknownMapStartsAtHome()
        {
            var data = StorySave.Capture(PlayedState(), new FieldSpot("RemovedMap", new GridPos(1, 1), Direction.Up));

            Assert.AreEqual(Town01Maps.Start, ThroughJson(data).Position);
        }
    }
}
