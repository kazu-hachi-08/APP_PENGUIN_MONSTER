using NUnit.Framework;

namespace PenguinMonster.Story.Tests
{
    /// <summary>自転車・大橋の下り坂と展望マス・隠しコマンド（PLAN_STORY_TOWN04.md フェーズ3）</summary>
    public class BicycleTests
    {
        private const string Bridge = "Bridge";
        private const string Gate = "Gate";

        // 大橋のつもりのマップ（9x5）。1行目: 下り坂の先に橋と展望マス (6,1)。2行目: 東の端は手すり。
        // 3行目: 東の端は関所への出入口 (8,3)
        private static FieldWorld CreateWorld()
        {
            var bridge = new FieldMap(Bridge, new[]
            {
                "IIIIIIIII",
                "#>>>>#*##",
                "#>>>>>>>I",
                "#>>>>>>>D",
                "IIIIIIIII",
            }, new Warp(new GridPos(8, 3), new FieldSpot(Gate, new GridPos(1, 1), Direction.Right)));
            var gate = new FieldMap(Gate, new[]
            {
                "WWW",
                "W_W",
                "WDW",
            }).AsIndoors();
            return new FieldWorld(new[] { bridge, gate });
        }

        private static FieldPlayer PlayerAt(int x, int y, FieldPeople people = null)
        {
            return new FieldPlayer(CreateWorld(), new FieldSpot(Bridge, new GridPos(x, y), Direction.Right), people);
        }

        /// <summary>十字キーを押さずに、止まるまで滑らせる。滑った歩数を返す</summary>
        private static int SlideUntilStopped(FieldPlayer player)
        {
            int steps = 0;
            for (var next = player.NextStep(null); next.HasValue; next = player.NextStep(null))
            {
                Assert.AreEqual(StepResult.Moved, player.Step(next.Value));
                steps++;
                Assert.Less(steps, 20, "滑り続けて止まらない");
            }
            return steps;
        }

        private static StoryFlags WithBicycle()
        {
            var flags = new StoryFlags();
            Bicycle.Give(flags);
            return flags;
        }

        // ---- 自転車 ----

        [Test]
        public void Give_HasButNotRiding()
        {
            var flags = WithBicycle();

            Assert.IsTrue(Bicycle.Has(flags));
            Assert.IsFalse(Bicycle.IsOn(flags));
        }

        [Test]
        public void Toggle_Outside_GetsOnAndOff()
        {
            var flags = WithBicycle();
            var outside = CreateWorld().Get(Bridge);

            Bicycle.Toggle(flags, outside);
            Assert.IsTrue(Bicycle.IsOn(flags));

            Bicycle.Toggle(flags, outside);
            Assert.IsFalse(Bicycle.IsOn(flags));
        }

        [Test]
        public void Toggle_Indoors_CannotGetOn()
        {
            var flags = WithBicycle();

            Bicycle.Toggle(flags, CreateWorld().Get(Gate));

            Assert.IsFalse(Bicycle.IsOn(flags));
        }

        [Test]
        public void WithoutBicycle_CannotRide()
        {
            var flags = new StoryFlags();
            var outside = CreateWorld().Get(Bridge);

            Bicycle.Toggle(flags, outside);
            Assert.IsFalse(Bicycle.IsOn(flags));

            // 乗っているフラグだけ立っていても、自転車が無ければ歩く
            flags.Set(Bicycle.OnFlag);
            Assert.IsFalse(Bicycle.IsOn(flags));
        }

        [Test]
        public void GetOffIfIndoors_OnlyIndoors()
        {
            var flags = WithBicycle();
            var world = CreateWorld();
            Bicycle.Toggle(flags, world.Get(Bridge));

            Bicycle.GetOffIfIndoors(flags, world.Get(Bridge));
            Assert.IsTrue(Bicycle.IsOn(flags));

            Bicycle.GetOffIfIndoors(flags, world.Get(Gate));
            Assert.IsFalse(Bicycle.IsOn(flags));
            Assert.IsTrue(Bicycle.Has(flags));
        }

        [Test]
        public void GetOnIfBikeRoad_OnlyWithBicycleOnBikeRoad()
        {
            var road = new FieldMap("Road", new[] { "##" }).AsBikeRoad();
            var flags = new StoryFlags();

            Bicycle.GetOnIfBikeRoad(flags, road);
            Assert.IsFalse(flags.Has(Bicycle.OnFlag), "自転車が無いと乗らない");

            Bicycle.Give(flags);
            Bicycle.GetOnIfBikeRoad(flags, CreateWorld().Get(Bridge));
            Assert.IsFalse(Bicycle.IsOn(flags), "自転車の道でなければ乗らない");

            Bicycle.GetOnIfBikeRoad(flags, road);
            Assert.IsTrue(Bicycle.IsOn(flags));
        }

        [TestCase(Town01Maps.PlayerHouse)]
        [TestCase(Town01Maps.Lab)]
        [TestCase(Town01Maps.WarmHut)]
        [TestCase(Town01Maps.Gym)]
        [TestCase(Town02Maps.Shop)]
        [TestCase(Town03Maps.Gym)]
        [TestCase(Town03Maps.PowerPlant)]
        public void StoryWorld_BuildingsAreIndoors(string mapId)
        {
            Assert.IsTrue(StoryWorld.Create().Get(mapId).IsIndoors);
        }

        [TestCase(Town01Maps.Village)]
        [TestCase(Town01Maps.Route01)]
        [TestCase(Town02Maps.Town)]
        [TestCase(Town03Maps.Route06)]
        public void StoryWorld_OutsideMapsAreNotIndoors(string mapId)
        {
            Assert.IsFalse(StoryWorld.Create().Get(mapId).IsIndoors);
        }

        // ---- 大橋のマス ----

        [Test]
        public void Railing_IsNotWalkable_BridgeTilesAre()
        {
            Assert.IsFalse(TileKind.Railing.IsWalkable());
            Assert.IsTrue(TileKind.Bridge.IsWalkable());
            Assert.IsTrue(TileKind.Downhill.IsWalkable());
            Assert.IsTrue(TileKind.Lookout.IsWalkable());
        }

        [Test]
        public void Downhill_SlidesEastUntilWall()
        {
            var player = PlayerAt(1, 2);

            int steps = SlideUntilStopped(player);

            // 東の端の手すりの手前で止まる
            Assert.AreEqual(6, steps);
            Assert.AreEqual(new GridPos(7, 2), player.Position);
        }

        [Test]
        public void Downhill_StopsBeforeWarp_ButCanWalkIn()
        {
            var player = PlayerAt(1, 3);

            SlideUntilStopped(player);

            Assert.AreEqual(new GridPos(7, 3), player.Position);
            // 出入口へは自分で東を押せば入れる
            Assert.AreEqual(Direction.Right, player.NextStep(Direction.Right));
            player.Step(Direction.Right);
            Assert.IsNotNull(player.WarpHere);
        }

        [Test]
        public void Downhill_StopsBeforePerson()
        {
            var person = new FieldNpc("rider", NpcLook.Youngster,
                NpcPlacement.Always(new FieldSpot(Bridge, new GridPos(4, 2), Direction.Left), EventStep.Say("いい 風！")));
            var people = new FieldPeople(new[] { person }, new StoryFlags());
            var player = PlayerAt(1, 2, people);

            SlideUntilStopped(player);

            Assert.AreEqual(new GridPos(3, 2), player.Position);
        }

        [TestCase(Direction.Up)]
        [TestCase(Direction.Down)]
        [TestCase(Direction.Left)]
        public void Downhill_CanStillMoveOtherWays(Direction direction)
        {
            var player = PlayerAt(2, 2);

            Assert.AreEqual(direction, player.NextStep(direction));
        }

        [Test]
        public void OffDownhill_DoesNotSlide()
        {
            var player = PlayerAt(0, 2);

            Assert.IsNull(player.NextStep(null));
        }

        [Test]
        public void SlopeOf_EastIsDown_WestIsUp()
        {
            var onSlope = PlayerAt(2, 2);
            Assert.AreEqual(Slope.Down, onSlope.SlopeOf(Direction.Right));
            Assert.AreEqual(Slope.Up, onSlope.SlopeOf(Direction.Left));
            Assert.AreEqual(Slope.None, onSlope.SlopeOf(Direction.Up));

            // 下り坂のすぐ東の橋から西へ戻るのも上り
            var belowSlope = PlayerAt(5, 1);
            Assert.AreEqual(Slope.Up, belowSlope.SlopeOf(Direction.Left));
            Assert.AreEqual(Slope.None, belowSlope.SlopeOf(Direction.Right));
        }

        // ---- 展望マスと隠しコマンド ----

        private static FieldPlayer GazingAtLookout(bool riding)
        {
            var player = PlayerAt(5, 1);
            player.Step(Direction.Right);
            Assert.IsTrue(player.IsOnLookout);
            player.StartGazing(riding);
            return player;
        }

        private static bool Enter(FieldPlayer player, params Direction[] directions)
        {
            bool passed = false;
            foreach (var direction in directions)
            {
                passed = player.TurnWhileGazing(direction);
            }
            return passed;
        }

        [Test]
        public void Gazing_TurnsWithoutMoving()
        {
            var player = GazingAtLookout(riding: true);

            player.TurnWhileGazing(Direction.Left);

            Assert.AreEqual(new GridPos(6, 1), player.Position);
            Assert.AreEqual(Direction.Left, player.Facing);
            Assert.IsTrue(player.IsGazing);
        }

        [Test]
        public void StopGazing_ThenWalksAsUsual()
        {
            var player = GazingAtLookout(riding: true);

            player.StopGazing();

            Assert.IsFalse(player.IsGazing);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Right));
        }

        [Test]
        public void RightCode_WhileRiding_Passes()
        {
            var player = GazingAtLookout(riding: true);

            Assert.IsTrue(Enter(player, SecretCode.Rainbow));
        }

        [Test]
        public void RightCode_AfterMistakes_Passes()
        {
            var player = GazingAtLookout(riding: true);
            Enter(player, Direction.Up, Direction.Down, Direction.Left);

            Assert.IsTrue(Enter(player, SecretCode.Rainbow));
        }

        [Test]
        public void WrongOrder_DoesNotPass()
        {
            var player = GazingAtLookout(riding: true);

            Assert.IsFalse(Enter(player, Direction.Up, Direction.Down, Direction.Up, Direction.Down,
                Direction.Left, Direction.Right, Direction.Left, Direction.Right));
            Assert.IsFalse(Enter(player, Direction.Up, Direction.Up, Direction.Down, Direction.Down, Direction.Left, Direction.Right));
        }

        [Test]
        public void RightCode_OnFoot_DoesNotPass()
        {
            var player = GazingAtLookout(riding: false);

            Assert.IsFalse(Enter(player, SecretCode.Rainbow));
        }

        [Test]
        public void StopGazing_ForgetsInput()
        {
            var player = GazingAtLookout(riding: true);
            Enter(player, Direction.Up, Direction.Up, Direction.Down, Direction.Down);

            player.StopGazing();
            player.StartGazing(riding: true);

            Assert.IsFalse(Enter(player, Direction.Left, Direction.Right, Direction.Left, Direction.Right));
        }

        [Test]
        public void MoveTo_StopsGazing()
        {
            var player = GazingAtLookout(riding: true);

            player.MoveTo(new FieldSpot(Bridge, new GridPos(0, 1), Direction.Down));

            Assert.IsFalse(player.IsGazing);
        }
    }
}
