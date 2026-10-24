using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り3のマップのつながり・出口をふさぐ人・小屋と店・出現表（PLAN_STORY_TOWN03.md フェーズ4）</summary>
    public class Town03MapTests
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

        /// <summary>Ask にはすべて answer で答えて最後まで進め、出てきた手順の種類を返す</summary>
        private List<EventStepKind> RunToEnd(IReadOnlyList<EventStep> script, bool answer = true)
        {
            var kinds = new List<EventStepKind>();
            var runner = new EventRunner(_state, script);
            while (!runner.IsDone)
            {
                kinds.Add(runner.Current.Kind);
                if (runner.Current.Kind == EventStepKind.Ask)
                {
                    runner.Answer(answer);
                }
                else
                {
                    runner.Advance();
                }
            }
            return kinds;
        }

        /// <summary>direction へ count 歩き、最後に乗った出入口をくぐる</summary>
        private static void WalkAndEnter(FieldPlayer player, Direction direction, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(StepResult.Moved, player.Step(direction), $"{player.Map.Id} {player.Position} から {direction}");
            }
            Assert.IsNotNull(player.WarpHere, $"{player.Map.Id} {player.Position}");
            player.Enter(player.WarpHere);
        }

        [Test]
        public void KagamikoNorthExit_BlockedUntilSaeBeaten()
        {
            // 区切り2の終わりの記録の場所（北の看板の前だったマス）から上へ
            var player = PlayerAt(Town02Maps.Town, new GridPos(11, 2), Direction.Up);
            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Up));
            StringAssert.Contains("サエ", player.ScriptAhead()[1].Text);

            _state.Flags.Set(Town02Trainers.GymLeader.DefeatedFlag);

            WalkAndEnter(player, Direction.Up, 2);
            Assert.AreEqual(Town03Maps.Route05, player.Map.Id);
        }

        [Test]
        public void Route05_LeadsToTown_TownEastToRoute06_AndBack()
        {
            _state.Flags.Set(Town02Trainers.GymLeader.DefeatedFlag);
            var player = PlayerAt(Town03Maps.Route05, new GridPos(9, 1), Direction.Up);
            WalkAndEnter(player, Direction.Up, 1);
            Assert.AreEqual(Town03Maps.Town, player.Map.Id);
            Assert.AreEqual(new GridPos(12, 20), player.Position);

            player.MoveTo(new FieldSpot(Town03Maps.Town, new GridPos(24, 12), Direction.Right));
            WalkAndEnter(player, Direction.Right, 1);
            Assert.AreEqual(Town03Maps.Route06, player.Map.Id);

            WalkAndEnter(player, Direction.Left, 1);
            Assert.AreEqual(Town03Maps.Town, player.Map.Id);

            player.MoveTo(new FieldSpot(Town03Maps.Town, new GridPos(12, 20), Direction.Down));
            WalkAndEnter(player, Direction.Down, 1);
            Assert.AreEqual(Town03Maps.Route05, player.Map.Id);

            player.MoveTo(new FieldSpot(Town03Maps.Route05, new GridPos(9, 30), Direction.Down));
            WalkAndEnter(player, Direction.Down, 1);
            Assert.AreEqual(Town02Maps.Town, player.Map.Id);
            Assert.AreEqual(new GridPos(11, 1), player.Position);
        }

        [Test]
        public void Route06_LeadsIntoPowerPlantAndBack()
        {
            var belowDoor = Town03Maps.PowerPlantDoor + Direction.Down.Offset();
            var player = PlayerAt(Town03Maps.Route06, belowDoor, Direction.Up);
            WalkAndEnter(player, Direction.Up, 1);
            Assert.AreEqual(Town03Maps.PowerPlant, player.Map.Id);

            WalkAndEnter(player, Direction.Down, 1);
            Assert.AreEqual(Town03Maps.Route06, player.Map.Id);
            Assert.AreEqual(belowDoor, player.Position);
        }

        [Test]
        public void Route06_WestEntranceReachesPowerPlantDoor()
        {
            // 町から入ってすぐの道を東へ歩き、発電所の前の道を北へ上がれば扉に着く（道がふさがっていないこと）
            var player = PlayerAt(Town03Maps.Route06, new GridPos(1, 10), Direction.Right);
            for (int i = 0; i < 23; i++)
            {
                Assert.AreEqual(StepResult.Moved, player.Step(Direction.Right), $"{player.Position}");
            }
            WalkAndEnter(player, Direction.Up, 6);
            Assert.AreEqual(Town03Maps.PowerPlant, player.Map.Id);
        }

        [Test]
        public void TownBuildings_LeadInsideAndBack()
        {
            var doors = new[]
            {
                (new GridPos(16, 17), Town03Maps.WarmHut), (new GridPos(21, 17), Town03Maps.Shop), (new GridPos(19, 5), Town03Maps.TownHouse),
            };
            foreach (var (door, mapId) in doors)
            {
                var player = PlayerAt(Town03Maps.Town, door + Direction.Down.Offset(), Direction.Up);
                WalkAndEnter(player, Direction.Up, 1);
                Assert.AreEqual(mapId, player.Map.Id);

                WalkAndEnter(player, Direction.Down, 1);
                Assert.AreEqual(Town03Maps.Town, player.Map.Id);
                Assert.AreEqual(door + Direction.Down.Offset(), player.Position);
            }
        }

        [Test]
        public void GymDoor_BlockedUntilPowerRestored()
        {
            var belowLeader = Town03People.LeaderAtGymDoor.Position + Direction.Down.Offset();
            var blocked = PlayerAt(Town03Maps.Town, belowLeader, Direction.Up);
            Assert.AreEqual(StepResult.Blocked, blocked.Step(Direction.Up));
            Assert.IsTrue(blocked.ScriptAhead().Any(step => step.Text != null && step.Text.Contains("停電")));

            _state.Flags.Set(Town03Flags.PowerRestored);

            var player = PlayerAt(Town03Maps.Town, belowLeader, Direction.Up);
            WalkAndEnter(player, Direction.Up, 2);
            Assert.AreEqual(Town03Maps.Gym, player.Map.Id);
        }

        [Test]
        public void HutKeeper_HealsAndReturnsToYukiokoshiHut()
        {
            _state.Party.Add(OwnedMonster.Create(Town01Starters.All[0], StoryBalance.StarterLevel));
            var owned = _state.Party.Members[0];
            var fighter = owned.ToBattle();
            fighter.TakeDamage(5);
            owned.ApplyBattleResult(fighter);

            var kinds = RunToEnd(PlayerAt(Town03Maps.WarmHut, new GridPos(3, 2), Direction.Up).ScriptAhead());

            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(Town03Maps.WarmHut, _state.LastHealSpot.MapId);
            CollectionAssert.Contains(kinds, EventStepKind.Storage);
        }

        [Test]
        public void Clerk_SellsEveryItem_AndOlderShopsStayTheSame()
        {
            var shop = PlayerAt(Town03Maps.Shop, new GridPos(3, 2), Direction.Up).ScriptAhead().Single(step => step.Kind == EventStepKind.Shop);
            // 持たせる道具（区切り4）は売らないので、売れる道具だけを比べる
            var sellable = System.Enum.GetValues(typeof(StoryItem)).Cast<StoryItem>().Where(item => StoryItems.Get(item).CanSell);
            CollectionAssert.AreEqual(sellable.ToArray(), shop.ShopItems.ToArray());

            // 区切り1・2の店には新しいタマゴは並ばない（PLAN_STORY_TOWN03.md §3.1）
            foreach (var oldShop in new[] { Town01Maps.Shop, Town02Maps.Shop })
            {
                var items = PlayerAt(oldShop, new GridPos(3, 2), Direction.Up).ScriptAhead().Single(step => step.Kind == EventStepKind.Shop).ShopItems;
                CollectionAssert.DoesNotContain(items, StoryItem.GreatEgg);
                Assert.AreEqual(5, items.Count);
            }
        }

        [Test]
        public void DroppedItems_ArePickedUpWithTown03Flags()
        {
            RunToEnd(PlayerAt(Town03Maps.Route05, new GridPos(17, 13), Direction.Right).ScriptAhead());
            RunToEnd(PlayerAt(Town03Maps.Route06, new GridPos(28, 2), Direction.Up).ScriptAhead());
            RunToEnd(PlayerAt(Town03Maps.Route06, new GridPos(2, 18), Direction.Left).ScriptAhead());

            Assert.AreEqual(2, _state.Bag.CountOf(StoryItem.Potion));
            Assert.AreEqual(2, _state.Bag.CountOf(StoryItem.GreatEgg));
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.Awakening));
            Assert.IsTrue(_state.Flags.Has(Town03Flags.ItemPrefix + "route06GreatEggs"));
        }

        [Test]
        public void Route07Sign_SaysToBeContinued()
        {
            var sign = _world.Get(Town03Maps.Town).SignAt(Town03Maps.Route07Sign);

            StringAssert.Contains("7番道路", sign[0]);
            StringAssert.Contains("次の 更新", sign[1]);
        }

        [Test]
        public void KagamikoRoute05Sign_NoLongerSaysToBeContinued()
        {
            var sign = _world.Get(Town02Maps.Town).SignAt(Town02Maps.Route05Sign);

            Assert.AreEqual(1, sign.Count);
        }

        [Test]
        public void EncounterSpecies_AreInSpeciesTable()
        {
            foreach (var map in Town03Maps.CreateMaps())
            {
                foreach (var table in new[] { map.Encounters, map.DeepEncounters }.Where(table => table != null))
                {
                    foreach (var slot in table.Slots)
                    {
                        Assert.AreSame(slot.Species, StorySpeciesTable.ByNo(slot.Species.No), $"{map.Id} {slot.Species.Name}");
                    }
                }
            }
        }

        [Test]
        public void GymLeaderAndGiftSpecies_NeverWild()
        {
            // 道場主の切り札（No.236・237）・もらう子（No.238）・発電所の子（No.239）は野生では出ない（PLAN_STORY_TOWN03.md §1）
            var wild = _world.Maps.SelectMany(map => new[] { map.Encounters, map.DeepEncounters }).Where(table => table != null)
                .SelectMany(table => table.Slots).Select(slot => slot.Species.No).ToList();
            foreach (var no in new[] { 236, 237, 238, 239 })
            {
                CollectionAssert.DoesNotContain(wild, no);
            }
        }

        [Test]
        public void TownAndPowerPlant_HaveNoWild()
        {
            Assert.IsNull(_world.Get(Town03Maps.Town).Encounters);
            Assert.IsNull(_world.Get(Town03Maps.PowerPlant).Encounters);
        }
    }
}
