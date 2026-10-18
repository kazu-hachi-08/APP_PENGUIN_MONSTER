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
            // ゲームと同じく、全区切りのマップの世界に戻す
            _world = StoryWorld.Create();
        }

        /// <summary>
        /// 区切り1の終わりの記録（2番道路の奥の看板の前・ガンテツに勝って いわおこし羽章あり）。
        /// 区切り1の版が書く形のまま残しておき、区切りを足しても読めることを守る（中身を書き換えないこと）
        /// </summary>
        private const string Town01EndJson =
            "{\"Version\":1," +
            "\"Position\":{\"MapId\":\"Route02\",\"X\":26,\"Y\":21,\"Facing\":1}," +
            "\"LastHealSpot\":{\"MapId\":\"WarmHut\",\"X\":3,\"Y\":2,\"Facing\":1}," +
            "\"Party\":[" +
            "{\"No\":155,\"Level\":17,\"Exp\":3300,\"Moves\":[\"腹ばいタックル\",\"火の粉\",\"ゆらゆら火\",\"火の粉ダイブ\"],\"Hp\":40,\"Status\":0,\"Item\":0}," +
            "{\"No\":161,\"Level\":15,\"Exp\":3500,\"Moves\":[\"ついばみ\",\"ぴいぴい鳴く\",\"二段つつき\"],\"Hp\":30,\"Status\":0,\"Item\":0}]," +
            "\"Box\":[{\"No\":160,\"Level\":5,\"Exp\":150,\"Moves\":[\"ついばみ\",\"ぴいぴい鳴く\"],\"Hp\":20,\"Status\":0,\"Item\":0}]," +
            "\"Bag\":[{\"Item\":\"CaptureEgg\",\"Count\":7},{\"Item\":\"Potion\",\"Count\":3}]," +
            "\"Money\":2480," +
            "\"Flags\":[\"town01.gotStarter\",\"town01.starter.154\",\"trainer.town01.rival1\",\"trainer.town01.route01.kid\"," +
            "\"town01.item.route02Eggs\",\"trainer.town01.rival2\",\"town01.route02Cleared\",\"trainer.town01.gym.disciple1\"," +
            "\"trainer.town01.gym.leader\"]," +
            "\"SeenNos\":[154,155,160,161,165]," +
            "\"CaughtNos\":[154,155,160,161]," +
            "\"Badges\":[\"town01.badge\"]}";

        private static StoryState PlayedState()
        {
            var state = new StoryState(Town01Maps.Start) { LastHealSpot = HutCounter };
            var starter = OwnedMonster.Create(Town01Species.Kobune, StoryBalance.StarterLevel);
            starter.GainExp(30);
            Hurt(starter, 4);
            starter.Item = HeldItem.LightFeather;
            var poisoned = starter.ToBattle();
            poisoned.SetStatus(StatusCondition.Poison, 0);
            starter.ApplyBattleResult(poisoned);
            state.Party.Add(starter);
            state.Box.Add(OwnedMonster.Create(Town01Species.Kogara, 3));
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
            Assert.AreEqual(Town01Species.Kogara, state.Box.Members[0].Species);
            AssertSameMonster(original.Party.Members[0], state.Party.Members[0]);
        }

        [Test]
        public void SaveThenLoad_KeepsEveryEggKind()
        {
            var original = PlayedState();
            original.Bag.Add(StoryItem.GreatEgg, 3);
            original.Bag.Add(StoryItem.SkyEgg, 1);
            original.Bag.Add(StoryItem.Awakening, 4);

            var data = StorySave.Capture(original, Standing);
            var restored = ThroughJson(data);

            Assert.IsNotNull(restored);
            var bag = restored.State.Bag;
            CollectionAssert.AreEqual(original.Bag.Items(), bag.Items());
            foreach (var item in original.Bag.Items())
            {
                Assert.AreEqual(original.Bag.CountOf(item), bag.CountOf(item), item.ToString());
            }
            // 番号ではなく名前で書いていること（enum の並びが変わっても読めるように）
            StringAssert.Contains("\"GreatEgg\"", JsonUtility.ToJson(data));
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
        public void Load_Town01EndSave_ContinuesWhereItWasSaved()
        {
            var restored = FromJson(Town01EndJson);

            Assert.IsNotNull(restored);
            Assert.AreEqual(new FieldSpot(Town01Maps.Route02, new GridPos(26, 21), Direction.Up), restored.Position);
            var state = restored.State;
            Assert.AreEqual(new FieldSpot(Town01Maps.WarmHut, new GridPos(3, 2), Direction.Up), state.LastHealSpot);
            Assert.AreEqual(Town01Species.Fuigo, state.Party.Members[0].Species);
            Assert.AreEqual(17, state.Party.Members[0].Level);
            Assert.AreEqual(Town01Species.Tsugumi, state.Party.Members[1].Species);
            Assert.AreEqual(Town01Species.Kogara, state.Box.Members[0].Species);
            Assert.AreEqual(7, state.Bag.CountOf(StoryItem.CaptureEgg));
            // 道具の enum の途中に新しいタマゴを足しても、記録は名前なので同じ道具に戻る
            Assert.AreEqual(3, state.Bag.CountOf(StoryItem.Potion));
            Assert.AreEqual(2, state.Bag.Items().Count);
            Assert.AreEqual(2480, state.Money);
            Assert.IsTrue(state.Flags.Has(Town01Trainers.GymLeader.DefeatedFlag));
            Assert.IsTrue(state.Flags.Has(Town01Flags.Route02Cleared));
            CollectionAssert.Contains(state.Badges, Town01Trainers.BadgeId);
        }

        [Test]
        public void Load_Town01EndSave_CanWalkOnToRoute03()
        {
            // 区切り1の終わりで記録した人が、区切り2を足した版でそのまま 3番道路へ進めること（PLAN_STORY_TOWN02.md §4）
            var restored = FromJson(Town01EndJson);
            var people = new FieldPeople(StoryWorld.People(), restored.State.Flags);
            var player = new FieldPlayer(_world, restored.Position, people);

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(StepResult.Moved, player.Step(Direction.Right), $"{player.Position}");
            }
            player.Enter(player.WarpHere);

            Assert.AreEqual(Town02Maps.Route03, player.Map.Id);
        }

        [Test]
        public void StoryWorld_HasEveryTown01Map()
        {
            // 区切り1で記録した場所が、全区切りの世界でも「今は無いマップ」にならないこと
            foreach (var map in Town01Maps.CreateMaps())
            {
                Assert.IsTrue(_world.Contains(map.Id), map.Id);
            }
        }

        [Test]
        public void StorySpeciesTable_FindsEveryTown01Species()
        {
            foreach (var species in Town01Species.All)
            {
                Assert.AreSame(species, StorySpeciesTable.ByNo(species.No), species.Name);
            }
            Assert.IsNull(StorySpeciesTable.ByNo(999));
        }

        [Test]
        public void Load_UnknownMapStartsAtHome()
        {
            var data = StorySave.Capture(PlayedState(), new FieldSpot("RemovedMap", new GridPos(1, 1), Direction.Up));

            Assert.AreEqual(Town01Maps.Start, ThroughJson(data).Position);
        }
    }
}
