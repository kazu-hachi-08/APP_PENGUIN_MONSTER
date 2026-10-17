using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り1のマップのつながり・道場の前の門下生・落とし物・深い草むら（PLAN_STORY_TOWN01.md フェーズ8b）</summary>
    public class Town01MapTests
    {
        private StoryState _state;
        private FieldWorld _world;
        private FieldPeople _people;

        [SetUp]
        public void SetUp()
        {
            _state = new StoryState(Town01Maps.Start);
            _world = StoryWorld.Create();
            _people = new FieldPeople(Town01People.Create(), _state.Flags);
        }

        private FieldPlayer PlayerAt(string mapId, GridPos pos, Direction facing)
        {
            return new FieldPlayer(_world, new FieldSpot(mapId, pos, facing), _people);
        }

        private void RunToEnd(IReadOnlyList<EventStep> script)
        {
            var runner = new EventRunner(_state, script);
            while (!runner.IsDone)
            {
                runner.Advance();
            }
        }

        [Test]
        public void EveryWarp_LeadsToWalkableSpot()
        {
            foreach (var map in _world.Maps)
            {
                foreach (var warp in map.Warps)
                {
                    Assert.IsTrue(_world.Contains(warp.To.MapId), $"{map.Id} {warp.From}");
                    Assert.IsTrue(_world.Get(warp.To.MapId).IsWalkable(warp.To.Position), $"{map.Id} {warp.From} → {warp.To.MapId}");
                }
            }
        }

        [Test]
        public void EveryMap_IsReachableFromStart()
        {
            var reached = new HashSet<string>();
            var queue = new Queue<FieldSpot>();
            queue.Enqueue(Town01Maps.Start);
            var visited = new HashSet<(string, GridPos)>();
            while (queue.Count > 0)
            {
                var spot = queue.Dequeue();
                if (!visited.Add((spot.MapId, spot.Position)))
                {
                    continue;
                }
                var map = _world.Get(spot.MapId);
                reached.Add(map.Id);
                var warp = map.WarpAt(spot.Position);
                if (warp != null)
                {
                    queue.Enqueue(warp.To);
                }
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    var next = spot.Position + direction.Offset();
                    if (map.IsWalkable(next))
                    {
                        queue.Enqueue(new FieldSpot(map.Id, next, direction));
                    }
                }
            }

            CollectionAssert.AreEquivalent(_world.Maps.Select(map => map.Id), reached);
        }

        [Test]
        public void Route01North_LeadsToTown_AndTownEastToRoute02()
        {
            var player = PlayerAt(Town01Maps.Route01, new GridPos(7, 1), Direction.Up);
            player.Step(Direction.Up);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.Town, player.Map.Id);

            player.MoveTo(new FieldSpot(Town01Maps.Town, new GridPos(22, 9), Direction.Right));
            player.Step(Direction.Right);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.Route02, player.Map.Id);

            player.Step(Direction.Left);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.Town, player.Map.Id);
        }

        [Test]
        public void GymDoor_BlockedUntilRoute02End()
        {
            var belowDisciple = Town01People.DiscipleAtGymDoor.Position + Direction.Down.Offset();
            Assert.AreEqual(StepResult.Blocked, PlayerAt(Town01Maps.Town, belowDisciple, Direction.Up).Step(Direction.Up));

            // 2番道路の奥のユキトとの2戦目で立つフラグ（出来事そのものは Town01StoryTests）
            _state.Flags.Set(Town01Flags.Route02Cleared);

            var player = PlayerAt(Town01Maps.Town, belowDisciple, Direction.Up);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Up));
            player.Step(Direction.Up);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.Gym, player.Map.Id);
        }

        [Test]
        public void DroppedItem_IsPickedUpOnceAndDisappears()
        {
            var itemSpot = new GridPos(14, 5);

            RunToEnd(PlayerAt(Town01Maps.Route01, new GridPos(13, 5), Direction.Right).ScriptAhead());

            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.Potion));
            Assert.IsNull(_people.At(Town01Maps.Route01, itemSpot));
        }

        [Test]
        public void FindItem_SaysFound()
        {
            var runner = new EventRunner(_state, new[] { EventStep.FindItem(StoryItem.Potion, 1) });

            StringAssert.Contains("見つけた", runner.Message);
        }

        [Test]
        public void DeepGrass_UsesSecondTable()
        {
            var map = new FieldMap("deep", new[] { "TTTT", "T,;T", "TTTT" }).WithEncounters(
                new EncounterTable(100, new EncounterSlot(Chick, 2, 2, 1)),
                new EncounterTable(100, new EncounterSlot(Junior, 7, 7, 1)));
            var world = new FieldWorld(new[] { map });
            var party = new Party();
            party.Add(Owned(Chick, 5));
            var random = new Random(1);

            Assert.AreEqual(Chick, new FieldPlayer(world, new FieldSpot("deep", new GridPos(1, 1), Direction.Down)).RollEncounter(random, party).Species);
            Assert.AreEqual(Junior, new FieldPlayer(world, new FieldSpot("deep", new GridPos(2, 1), Direction.Down)).RollEncounter(random, party).Species);
        }

        [Test]
        public void EncounterSpecies_AreTown01Species_AndPlacedAsPlanned()
        {
            var town01Maps = Town01Maps.CreateMaps().ToList();
            var tables = town01Maps.SelectMany(map => new[] { map.Encounters, map.DeepEncounters }).Where(table => table != null).ToList();
            foreach (var slot in tables.SelectMany(table => table.Slots))
            {
                Assert.AreSame(slot.Species, Town01Species.ByNo(slot.Species.No), slot.Species.Name);
            }
            // 川の子は1番道路の川べり、闇の子は2番道路の林の奥だけ
            CollectionAssert.Contains(_world.Get(Town01Maps.Route01).DeepEncounters.Slots.Select(slot => slot.Species), Town01Species.Yamame);
            CollectionAssert.Contains(_world.Get(Town01Maps.Route02).DeepEncounters.Slots.Select(slot => slot.Species), Town01Species.Warashi);
            Assert.IsFalse(town01Maps.Where(map => map.Encounters != null)
                .Any(map => map.Encounters.Slots.Any(slot => slot.Species == Town01Species.Yamame || slot.Species == Town01Species.Warashi)));
        }
    }
}
