using System;
using NUnit.Framework;

namespace PenguinMonster.Story.Tests
{
    public class FieldTests
    {
        private const string Room = "Room";
        private const string Hall = "Hall";

        // 部屋（4x4）の下の扉 (1,3) から、廊下（5x3）の (2,1) へ。廊下の扉 (2,2) から部屋の (1,2) へ戻る
        private static FieldWorld CreateWorld()
        {
            var room = new FieldMap(Room, new[]
            {
                "WWWW",
                "W__W",
                "W_~W",
                "WDWW",
            }, new Warp(new GridPos(1, 3), new FieldSpot(Hall, new GridPos(2, 1), Direction.Down)));
            var hall = new FieldMap(Hall, new[]
            {
                "=====",
                "=====",
                "==D==",
            }, new Warp(new GridPos(2, 2), new FieldSpot(Room, new GridPos(1, 2), Direction.Up)));
            return new FieldWorld(new[] { room, hall });
        }

        private static FieldPlayer PlayerAt(string mapId, int x, int y)
        {
            return new FieldPlayer(CreateWorld(), new FieldSpot(mapId, new GridPos(x, y), Direction.Down));
        }

        [Test]
        public void Step_OntoFloor_Moves()
        {
            var player = PlayerAt(Room, 1, 1);

            var result = player.Step(Direction.Right);

            Assert.AreEqual(StepResult.Moved, result);
            Assert.AreEqual(new GridPos(2, 1), player.Position);
            Assert.AreEqual(Direction.Right, player.Facing);
        }

        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        public void Step_IntoWall_StaysButTurns(Direction direction)
        {
            var player = PlayerAt(Room, 1, 1);

            var result = player.Step(direction);

            Assert.AreEqual(StepResult.Blocked, result);
            Assert.AreEqual(new GridPos(1, 1), player.Position);
            Assert.AreEqual(direction, player.Facing);
        }

        [Test]
        public void Step_IntoWater_IsBlocked()
        {
            var player = PlayerAt(Room, 2, 1);

            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Down));
        }

        [Test]
        public void Step_OutsideMap_IsBlocked()
        {
            // 端に壁の無いマップでも外へは出られない
            var player = PlayerAt(Hall, 0, 0);

            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Up));
            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Left));
            Assert.AreEqual(new GridPos(0, 0), player.Position);
        }

        [Test]
        public void StepOntoDoor_ThenEnter_ArrivesAtDestination()
        {
            var player = PlayerAt(Room, 1, 2);

            player.Step(Direction.Down);
            Assert.IsNotNull(player.WarpHere);
            player.Enter(player.WarpHere);

            Assert.AreEqual(Hall, player.Map.Id);
            Assert.AreEqual(new GridPos(2, 1), player.Position);
            Assert.AreEqual(Direction.Down, player.Facing);
            // 行き先は扉の手前なので、乗ったまま入り直すことはない
            Assert.IsNull(player.WarpHere);
        }

        [Test]
        public void WarpHere_OnPlainTile_IsNull()
        {
            Assert.IsNull(PlayerAt(Room, 1, 1).WarpHere);
        }

        [Test]
        public void Map_RowOfWrongLength_Throws()
        {
            Assert.Throws<ArgumentException>(() => new FieldMap("Bad", new[] { "WWW", "WW" }));
        }

        [Test]
        public void Map_UnknownLetter_Throws()
        {
            Assert.Throws<ArgumentException>(() => new FieldMap("Bad", new[] { "W?W" }));
        }

        [Test]
        public void Town01_StartIsWalkable()
        {
            var world = Town01Maps.CreateWorld();
            var start = Town01Maps.Start;

            Assert.IsTrue(world.Get(start.MapId).IsWalkable(start.Position));
        }

        [Test]
        public void Town01_EveryWarpLeadsToWalkableTileWithoutWarp()
        {
            var world = Town01Maps.CreateWorld();
            foreach (var map in world.Maps)
            {
                foreach (var warp in map.Warps)
                {
                    // 出入口は扉か、マップの端の道（道路への出口）
                    bool onEdge = warp.From.X == 0 || warp.From.Y == 0 || warp.From.X == map.Width - 1 || warp.From.Y == map.Height - 1;
                    Assert.IsTrue(map.TileAt(warp.From) == TileKind.Door || onEdge, $"{map.Id} {warp.From}");
                    Assert.IsTrue(world.Contains(warp.To.MapId), $"{map.Id} → {warp.To.MapId}");
                    var target = world.Get(warp.To.MapId);
                    Assert.IsTrue(target.IsWalkable(warp.To.Position), $"{map.Id} → {target.Id} {warp.To.Position}");
                    Assert.IsNull(target.WarpAt(warp.To.Position), $"{map.Id} → {target.Id} {warp.To.Position}");
                }
            }
        }

        [Test]
        public void Town01_LeaveHouseAndComeBack_ArrivesInFrontOfDoors()
        {
            // 自分の家の中で目覚め、扉 (3,5) から村へ出て、また入る
            var player = new FieldPlayer(Town01Maps.CreateWorld(), Town01Maps.Start);
            Assert.AreEqual(Town01Maps.PlayerHouse, player.Map.Id);

            player.Step(Direction.Right);
            player.Step(Direction.Down);
            player.Step(Direction.Down);
            player.Step(Direction.Down);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.Village, player.Map.Id);
            Assert.AreEqual(new GridPos(4, 5), player.Position);

            player.Step(Direction.Up);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.PlayerHouse, player.Map.Id);
            Assert.AreEqual(new GridPos(3, 4), player.Position);
        }
    }
}
