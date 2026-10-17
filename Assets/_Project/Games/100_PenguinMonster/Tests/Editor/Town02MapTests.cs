using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り2のマップのつながり・出口をふさぐ人・小屋と店・出現表（PLAN_STORY_TOWN02.md フェーズ3）</summary>
    public class Town02MapTests
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
        public void Route02Exit_BlockedUntilGantetsuBeaten()
        {
            // 区切り1の終わりの記録の場所（2番道路の奥の看板の前）から右へ
            var player = PlayerAt(Town01Maps.Route02, new GridPos(26, 21), Direction.Up);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Right));
            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Right));
            StringAssert.Contains("雪", player.ScriptAhead()[0].Text);

            _state.Flags.Set(Town01Trainers.GymLeader.DefeatedFlag);

            WalkAndEnter(player, Direction.Right, 2);
            Assert.AreEqual(Town02Maps.Route03, player.Map.Id);
        }

        [Test]
        public void Route03_LeadsToTown_AndTownEastToRoute04_AndBack()
        {
            _state.Flags.Set(Town01Trainers.GymLeader.DefeatedFlag);
            var player = PlayerAt(Town02Maps.Route03, new GridPos(9, 1), Direction.Up);
            WalkAndEnter(player, Direction.Up, 1);
            Assert.AreEqual(Town02Maps.Town, player.Map.Id);
            Assert.AreEqual(new GridPos(12, 20), player.Position);

            player.MoveTo(new FieldSpot(Town02Maps.Town, new GridPos(24, 7), Direction.Right));
            WalkAndEnter(player, Direction.Right, 1);
            Assert.AreEqual(Town02Maps.Route04, player.Map.Id);

            WalkAndEnter(player, Direction.Left, 1);
            Assert.AreEqual(Town02Maps.Town, player.Map.Id);

            player.MoveTo(new FieldSpot(Town02Maps.Town, new GridPos(12, 20), Direction.Down));
            WalkAndEnter(player, Direction.Down, 1);
            Assert.AreEqual(Town02Maps.Route03, player.Map.Id);

            player.MoveTo(new FieldSpot(Town02Maps.Route03, new GridPos(1, 28), Direction.Left));
            WalkAndEnter(player, Direction.Left, 1);
            Assert.AreEqual(Town01Maps.Route02, player.Map.Id);
            Assert.AreEqual(new GridPos(28, 21), player.Position);
        }

        [Test]
        public void TownBuildings_LeadInsideAndBack()
        {
            var doors = new[]
            {
                (new GridPos(13, 5), Town02Maps.WarmHut), (new GridPos(19, 5), Town02Maps.Shop), (new GridPos(3, 18), Town02Maps.TownHouse),
            };
            foreach (var (door, mapId) in doors)
            {
                var player = PlayerAt(Town02Maps.Town, door + Direction.Down.Offset(), Direction.Up);
                WalkAndEnter(player, Direction.Up, 1);
                Assert.AreEqual(mapId, player.Map.Id);

                WalkAndEnter(player, Direction.Down, 1);
                Assert.AreEqual(Town02Maps.Town, player.Map.Id);
                Assert.AreEqual(door + Direction.Down.Offset(), player.Position);
            }
        }

        [Test]
        public void GymDoor_BlockedUntilRoute04End()
        {
            var belowDisciple = Town02People.DiscipleAtGymDoor.Position + Direction.Down.Offset();
            Assert.AreEqual(StepResult.Blocked, PlayerAt(Town02Maps.Town, belowDisciple, Direction.Up).Step(Direction.Up));

            // 4番道路の奥の出来事（ユキトの3戦目。Town02StoryTests）を終えると、門下生がどく
            _state.Flags.Set(Town02Flags.Route04Cleared);

            var player = PlayerAt(Town02Maps.Town, belowDisciple, Direction.Up);
            WalkAndEnter(player, Direction.Up, 2);
            Assert.AreEqual(Town02Maps.Gym, player.Map.Id);
        }

        [Test]
        public void HutKeeper_HealsAndReturnsToKagamikoHut()
        {
            _state.Party.Add(OwnedMonster.Create(Town01Starters.All[0], StoryBalance.StarterLevel));
            var owned = _state.Party.Members[0];
            var fighter = owned.ToBattle();
            fighter.TakeDamage(5);
            owned.ApplyBattleResult(fighter);

            var kinds = RunToEnd(PlayerAt(Town02Maps.WarmHut, new GridPos(3, 2), Direction.Up).ScriptAhead());

            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(Town02Maps.WarmHut, _state.LastHealSpot.MapId);
            CollectionAssert.Contains(kinds, EventStepKind.Storage);
        }

        [Test]
        public void Clerk_OpensShop()
        {
            var script = PlayerAt(Town02Maps.Shop, new GridPos(3, 2), Direction.Up).ScriptAhead();

            Assert.IsTrue(script.Any(step => step.Kind == EventStepKind.Shop));
        }

        [Test]
        public void DroppedItems_ArePickedUpWithTown02Flags()
        {
            RunToEnd(PlayerAt(Town02Maps.Route03, new GridPos(2, 30), Direction.Left).ScriptAhead());
            RunToEnd(PlayerAt(Town02Maps.Route04, new GridPos(2, 1), Direction.Left).ScriptAhead());
            RunToEnd(PlayerAt(Town02Maps.Route04, new GridPos(28, 16), Direction.Down).ScriptAhead());

            Assert.AreEqual(2, _state.Bag.CountOf(StoryItem.Potion));
            Assert.AreEqual(3, _state.Bag.CountOf(StoryItem.CaptureEgg));
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.ParalyzeHeal));
            Assert.IsTrue(_state.Flags.Has(Town02Flags.ItemPrefix + "route03Potion"));
        }

        [Test]
        public void Route05Sign_SaysToBeContinued()
        {
            var sign = _world.Get(Town02Maps.Town).SignAt(Town02Maps.Route05Sign);

            StringAssert.Contains("5番道路", sign[0]);
        }

        [Test]
        public void EncounterSpecies_AreInSpeciesTable()
        {
            foreach (var map in Town02Maps.CreateMaps())
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
        public void GymLeaderOnlySpecies_NeverWild()
        {
            // 道場主の切り札（No.205・206）と、もらう子（No.209）は野生では出ない（PLAN_STORY_TOWN02.md §1）
            var wild = _world.Maps.SelectMany(map => new[] { map.Encounters, map.DeepEncounters }).Where(table => table != null)
                .SelectMany(table => table.Slots).Select(slot => slot.Species.No).ToList();
            CollectionAssert.DoesNotContain(wild, 205);
            CollectionAssert.DoesNotContain(wild, 206);
            CollectionAssert.DoesNotContain(wild, 209);
        }

        [Test]
        public void EveryPerson_StandsOnWalkableTileWithoutWarp_AndHasUniqueId()
        {
            var people = StoryWorld.People();
            CollectionAssert.AllItemsAreUnique(people.Select(npc => npc.Id));
            foreach (var placement in people.SelectMany(npc => npc.Placements))
            {
                var map = _world.Get(placement.Spot.MapId);
                Assert.IsTrue(map.IsWalkable(placement.Spot.Position), $"{map.Id} {placement.Spot.Position}");
                Assert.IsNull(map.WarpAt(placement.Spot.Position), $"{map.Id} {placement.Spot.Position}");
            }
        }
    }
}
