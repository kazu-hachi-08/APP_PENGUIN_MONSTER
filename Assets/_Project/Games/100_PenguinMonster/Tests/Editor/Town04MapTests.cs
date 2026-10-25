using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り4のマップのつながり・出口をふさぐ人・大橋・出現表（PLAN_STORY_TOWN04.md フェーズ6。人・出来事は Town04StoryTests）</summary>
    public class Town04MapTests
    {
        private StoryState _state;
        private FieldWorld _world;
        private FieldPeople _people;

        [SetUp]
        public void SetUp()
        {
            _state = new StoryState(Town01Maps.Start);
            _world = StoryWorld.Create();
            _people = new FieldPeople(StoryWorld.People(), _state.Flags);
        }

        private FieldPlayer PlayerAt(string mapId, GridPos pos, Direction facing)
        {
            return new FieldPlayer(_world, new FieldSpot(mapId, pos, facing), _people);
        }

        /// <summary>Ask にはすべて「はい」で答えて最後まで進める</summary>
        private void RunToEnd(IReadOnlyList<EventStep> script)
        {
            var runner = new EventRunner(_state, script);
            while (!runner.IsDone)
            {
                if (runner.Current.Kind == EventStepKind.Ask)
                {
                    runner.Answer(true);
                }
                else
                {
                    runner.Advance();
                }
            }
        }

        private static void Walk(FieldPlayer player, Direction direction, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(StepResult.Moved, player.Step(direction), $"{player.Map.Id} {player.Position} から {direction}");
            }
        }

        /// <summary>direction へ count 歩き、最後に乗った出入口をくぐる</summary>
        private static void WalkAndEnter(FieldPlayer player, Direction direction, int count)
        {
            Walk(player, direction, count);
            Assert.IsNotNull(player.WarpHere, $"{player.Map.Id} {player.Position}");
            player.Enter(player.WarpHere);
        }

        /// <summary>十字キーを押さずに、下り坂で止まるまで滑らせる</summary>
        private static void SlideUntilStopped(FieldPlayer player)
        {
            int steps = 0;
            for (var next = player.NextStep(null); next.HasValue; next = player.NextStep(null))
            {
                Assert.AreEqual(StepResult.Moved, player.Step(next.Value));
                Assert.Less(++steps, 40, "滑り続けて止まらない");
            }
        }

        [Test]
        public void YukiokoshiNorthExit_BlockedUntilRaizoBeaten()
        {
            // 区切り3の終わりの記録の場所（北の看板の前だったマス）から上へ
            var player = PlayerAt(Town03Maps.Town, new GridPos(12, 2), Direction.Up);
            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Up));
            StringAssert.Contains("ライゾウ", player.ScriptAhead()[1].Text);

            _state.Flags.Set(Town03Trainers.GymLeader.DefeatedFlag);

            WalkAndEnter(player, Direction.Up, 2);
            Assert.AreEqual(Town04Maps.Route07, player.Map.Id);
            Assert.AreEqual(new GridPos(10, 32), player.Position);
        }

        [Test]
        public void Route07_LeadsToTown_AndBack()
        {
            _state.Flags.Set(Town03Trainers.GymLeader.DefeatedFlag);
            var player = PlayerAt(Town04Maps.Route07, new GridPos(10, 1), Direction.Up);
            WalkAndEnter(player, Direction.Up, 1);
            Assert.AreEqual(Town04Maps.Town, player.Map.Id);
            Assert.AreEqual(new GridPos(12, 20), player.Position);

            WalkAndEnter(player, Direction.Down, 1);
            Assert.AreEqual(Town04Maps.Route07, player.Map.Id);

            player.MoveTo(new FieldSpot(Town04Maps.Route07, new GridPos(10, 32), Direction.Down));
            WalkAndEnter(player, Direction.Down, 1);
            Assert.AreEqual(Town03Maps.Town, player.Map.Id);
            Assert.AreEqual(new GridPos(12, 1), player.Position);
        }

        [Test]
        public void Gate_BlocksBridgeUntilBicycle()
        {
            var player = PlayerAt(Town04Maps.Town, new GridPos(24, 12), Direction.Right);
            WalkAndEnter(player, Direction.Right, 1);
            Assert.AreEqual(Town04Maps.Gate, player.Map.Id);
            Walk(player, Direction.Right, 4);
            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Right));
            Assert.IsTrue(player.ScriptAhead().Any(step => step.Text != null && step.Text.Contains("自転車")));

            _state.Flags.Set(Town04Flags.Bicycle);

            WalkAndEnter(player, Direction.Right, 2);
            Assert.AreEqual(Town04Maps.Bridge, player.Map.Id);
            Assert.AreEqual(new GridPos(1, 5), player.Position);

            WalkAndEnter(player, Direction.Left, 1);
            Assert.AreEqual(Town04Maps.Gate, player.Map.Id);
            WalkAndEnter(player, Direction.Left, 6);
            Assert.AreEqual(Town04Maps.Town, player.Map.Id);
            Assert.AreEqual(new GridPos(24, 12), player.Position);
        }

        [Test]
        public void Bridge_IsBikeRoad_GateIsIndoors()
        {
            Assert.IsTrue(_world.Get(Town04Maps.Bridge).IsBikeRoad);
            Assert.IsTrue(_world.Get(Town04Maps.Gate).IsIndoors);
            Assert.IsFalse(_world.Get(Town04Maps.Route07).IsBikeRoad);
        }

        [Test]
        public void Bridge_SlidesDownToLookoutAndHayate()
        {
            var player = PlayerAt(Town04Maps.Bridge, new GridPos(1, 5), Direction.Right);
            Walk(player, Direction.Right, 7);
            SlideUntilStopped(player);
            Assert.AreEqual(new GridPos(14, 5), player.Position);

            // 展望マスは真ん中の北のふくらみ
            player.MoveTo(new FieldSpot(Town04Maps.Bridge, Town04Maps.Lookout + Direction.Down.Offset(), Direction.Up));
            Walk(player, Direction.Up, 1);
            Assert.IsTrue(player.IsOnLookout);

            Walk(player, Direction.Down, 1);
            Walk(player, Direction.Right, 3);
            SlideUntilStopped(player);
            Assert.AreEqual(new GridPos(34, 4), player.Position);

            player.MoveTo(new FieldSpot(Town04Maps.Bridge, Town04People.HayateAtLookoutDeck.Position + Direction.Left.Offset(), Direction.Right));
            RunToEnd(player.ScriptAhead());
            Assert.IsTrue(_state.Flags.Has(Town04Flags.MetHayate));
            Assert.IsNull(_people.At(Town04Maps.Bridge, Town04People.HayateAtLookoutDeck.Position));
        }

        [Test]
        public void Bridge_UphillWalksBackToGate()
        {
            // 帰りは下り坂を上って戻れる（大橋は行き止まり）
            var player = PlayerAt(Town04Maps.Bridge, new GridPos(35, 5), Direction.Left);
            WalkAndEnter(player, Direction.Left, 35);
            Assert.AreEqual(Town04Maps.Gate, player.Map.Id);
        }

        [Test]
        public void GymDoor_BlockedUntilMetHayate()
        {
            var belowDisciple = Town04People.DiscipleAtGymDoor.Position + Direction.Down.Offset();
            var player = PlayerAt(Town04Maps.Town, belowDisciple, Direction.Up);
            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Up));
            Assert.IsTrue(player.ScriptAhead().Any(step => step.Text != null && step.Text.Contains("展望台")));

            _state.Flags.Set(Town04Flags.MetHayate);

            WalkAndEnter(player, Direction.Up, 2);
            Assert.AreEqual(Town04Maps.Gym, player.Map.Id);
        }

        [Test]
        public void TownBuildings_LeadInsideAndBack()
        {
            var doors = new[]
            {
                (new GridPos(16, 17), Town04Maps.WarmHut), (new GridPos(21, 17), Town04Maps.Shop),
                (new GridPos(19, 5), Town04Maps.BikeShop), (new GridPos(3, 11), Town04Maps.TownHouse),
            };
            foreach (var (door, mapId) in doors)
            {
                var player = PlayerAt(Town04Maps.Town, door + Direction.Down.Offset(), Direction.Up);
                WalkAndEnter(player, Direction.Up, 1);
                Assert.AreEqual(mapId, player.Map.Id);
                Assert.IsTrue(player.Map.IsIndoors, mapId);

                WalkAndEnter(player, Direction.Down, 1);
                Assert.AreEqual(Town04Maps.Town, player.Map.Id);
                Assert.AreEqual(door + Direction.Down.Offset(), player.Position);
            }
        }

        [Test]
        public void HutKeeperAndClerk_StandInKazahana()
        {
            var hut = PlayerAt(Town04Maps.WarmHut, new GridPos(3, 2), Direction.Up).ScriptAhead();
            Assert.IsTrue(hut.Any(step => step.Kind == EventStepKind.Heal));
            // 品ぞろえはユキオコシ町の店と同じ（区切り4の新しいタマゴは無い）
            var shop = ShopItemsOf(Town04Maps.Shop);
            CollectionAssert.AreEqual(ShopItemsOf(Town03Maps.Shop), shop);
        }

        private StoryItem[] ShopItemsOf(string shopMapId)
        {
            return PlayerAt(shopMapId, new GridPos(3, 2), Direction.Up).ScriptAhead()
                .Single(step => step.Kind == EventStepKind.Shop).ShopItems.ToArray();
        }

        [Test]
        public void Fishing_PondAndBridgePier()
        {
            var random = new System.Random(1);
            var pond = PlayerAt(Town04Maps.Route07, new GridPos(9, 10), Direction.Left);
            var pier = PlayerAt(Town04Maps.Bridge, new GridPos(22, 10), Direction.Down);
            foreach (var player in new[] { pond, pier })
            {
                Assert.AreNotEqual(FishResult.None, player.Fish(true, random, out _), player.Map.Id);
                Assert.AreNotEqual(FishResult.NothingHere, player.Fish(true, random, out _), player.Map.Id);
            }
        }

        [Test]
        public void DroppedItem_IsPickedUpWithTown04Flag()
        {
            RunToEnd(PlayerAt(Town04Maps.Route07, new GridPos(20, 3), Direction.Up).ScriptAhead());

            Assert.AreEqual(2, _state.Bag.CountOf(StoryItem.UltraEgg));
            Assert.IsTrue(_state.Flags.Has(Town04Flags.ItemPrefix + "route07UltraEggs"));
        }

        [Test]
        public void Route09Sign_SaysToBeContinued()
        {
            var sign = _world.Get(Town04Maps.Town).SignAt(Town04Maps.Route09Sign);

            StringAssert.Contains("9番道路", sign[0]);
            StringAssert.Contains("次の 更新", sign[1]);
        }

        [Test]
        public void EncounterSpecies_AreInSpeciesTable()
        {
            foreach (var map in Town04Maps.CreateMaps())
            {
                foreach (var table in new[] { map.Encounters, map.DeepEncounters, map.FishingEncounters }.Where(table => table != null))
                {
                    foreach (var slot in table.Slots)
                    {
                        Assert.AreSame(slot.Species, StorySpeciesTable.ByNo(slot.Species.No), $"{map.Id} {slot.Species.Name}");
                    }
                }
            }
        }

        [Test]
        public void GymLeaderAndRainbowSpecies_NeverWild()
        {
            // 道場主の切り札（No.266・267）・虹の羽の子（No.270）は野生でも釣りでも出ない（PLAN_STORY_TOWN04.md §1）
            var wild = _world.Maps.SelectMany(map => new[] { map.Encounters, map.DeepEncounters, map.FishingEncounters })
                .Where(table => table != null).SelectMany(table => table.Slots).Select(slot => slot.Species.No).ToList();
            foreach (var no in new[] { 266, 267, 270 })
            {
                CollectionAssert.DoesNotContain(wild, no);
            }
        }

        [Test]
        public void TownAndGate_HaveNoWild()
        {
            Assert.IsNull(_world.Get(Town04Maps.Town).Encounters);
            Assert.IsNull(_world.Get(Town04Maps.Gate).Encounters);
        }
    }
}
