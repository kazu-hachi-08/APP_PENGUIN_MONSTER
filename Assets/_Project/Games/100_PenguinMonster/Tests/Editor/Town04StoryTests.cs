using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り4の人・トレーナー・出来事（PLAN_STORY_TOWN04.md フェーズ7）</summary>
    public class Town04StoryTests
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

        /// <summary>野生の決着。捕まえるだけの子（虹の羽の子）で、倒した・逃げた・負けたを分けるため</summary>
        private enum Wild { Caught, Defeated, Fled, Lost }

        /// <summary>台本を最後まで流す。Ask にははい、トレーナー戦は won、野生は wild で決着させる。流した文を返す</summary>
        private List<string> Run(IReadOnlyList<EventStep> script, bool won = true, Wild wild = Wild.Caught)
        {
            var said = new List<string>();
            var runner = new EventRunner(_state, script);
            while (!runner.IsDone)
            {
                switch (runner.Current.Kind)
                {
                    case EventStepKind.Ask:
                        runner.Answer(true);
                        break;
                    case EventStepKind.Battle:
                        runner.FinishBattle(won);
                        break;
                    case EventStepKind.WildBattle when runner.Current.CatchOnly:
                        runner.FinishCatchOnlyBattle(wild == Wild.Caught, wild == Wild.Lost);
                        break;
                    case EventStepKind.WildBattle:
                        runner.FinishWildBattle(wild == Wild.Caught || wild == Wild.Defeated);
                        break;
                    default:
                        said.Add(runner.Message);
                        runner.Advance();
                        break;
                }
            }
            return said;
        }

        private IReadOnlyList<EventStep> ScriptBelow(FieldSpot spot)
        {
            return PlayerAt(spot.MapId, spot.Position + Direction.Down.Offset(), Direction.Up).ScriptAhead();
        }

        private IReadOnlyList<EventStep> AnglerScript() => ScriptBelow(Town04People.AnglerSpot);
        private IReadOnlyList<EventStep> BikeShopScript() => ScriptBelow(Town04People.BikeShopOwnerSpot);
        private IReadOnlyList<EventStep> GymLeaderScript() => ScriptBelow(Town04People.GymLeaderSpot);

        private IReadOnlyList<EventStep> HayateAtDeckScript()
        {
            var deck = Town04People.HayateAtLookoutDeck;
            return PlayerAt(deck.MapId, deck.Position + Direction.Left.Offset(), Direction.Right).ScriptAhead();
        }

        private static IEnumerable<TrainerDefinition> Town04TrainersOnField()
        {
            return Town04People.Create()
                .SelectMany(npc => npc.Placements)
                .Select(placement => placement.Trainer)
                .Where(trainer => trainer != null);
        }

        // ---- 釣り竿・自転車・持たせる道具 ----

        [Test]
        public void Angler_GivesRodAndFreshKelpOnce()
        {
            Run(AnglerScript());
            Run(AnglerScript());

            Assert.IsTrue(FishingRod.Has(_state.Flags));
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldFreshKelp));
        }

        [Test]
        public void BikeShopOwner_GivesBicycleOnce()
        {
            Run(BikeShopScript());

            Assert.IsTrue(Bicycle.Has(_state.Flags));
            Assert.IsFalse(BikeShopScript().Any(step => step.Kind == EventStepKind.SetFlag));
        }

        [Test]
        public void SquidSnackGiver_GivesOnce()
        {
            Run(ScriptBelow(Town04People.SquidSnackGiverSpot));
            Run(ScriptBelow(Town04People.SquidSnackGiverSpot));

            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldSquidSnack));
            Assert.IsTrue(_state.Flags.Has(Town04Flags.GotSquidSnack));
        }

        [Test]
        public void DroppedHeldItems_OnRoute07AndBridge()
        {
            Run(PlayerAt(Town04Maps.Route07, new GridPos(20, 29), Direction.Up).ScriptAhead());
            Run(PlayerAt(Town04Maps.Bridge, new GridPos(15, 2), Direction.Up).ScriptAhead());

            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldDriedFish));
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldBraveFeather));
        }

        [Test]
        public void EveryHeldItem_CanBeObtainedOnce()
        {
            // 持たせる道具5種は、区切り4のどこかで1つずつ手に入る（PLAN_STORY_TOWN04.md §6.1）
            var given = Town04People.Create().SelectMany(npc => npc.Placements).SelectMany(placement => placement.Script)
                .Where(step => step.Kind == EventStepKind.GiveItem).Select(step => step.Item).ToList();
            var held = new[]
            {
                StoryItem.HeldDriedFish, StoryItem.HeldSquidSnack, StoryItem.HeldFreshKelp, StoryItem.HeldBraveFeather, StoryItem.HeldWiseFeather,
            };
            foreach (var item in held)
            {
                Assert.AreEqual(1, given.Count(given1 => given1 == item), item.ToString());
            }
        }

        [Test]
        public void HintElder_TellsTheCodeAsAStory()
        {
            var lines = Run(ScriptBelow(Town04People.HintElderSpot));

            Assert.IsTrue(lines.Any(line => line.Contains("大橋") && line.Contains("自転車")));
            Assert.IsTrue(lines.Any(line => line.Contains("虹色")));
        }

        // ---- ユキトの5戦目 ----

        /// <summary>最初の1体をもらい、区切り3の道場まで終えたところ（7番道路へ進める）</summary>
        private void AfterTown03(StorySpecies starter)
        {
            _state.Flags.Set(Town01Flags.GotStarter);
            _state.Flags.Set(EventStep.ChosenFlag(Town01Flags.StarterPrefix, starter));
            _state.Flags.Set(Town03Trainers.Rival4Beaten);
            _state.Flags.Set(Town03Flags.PowerRestored);
            _state.Flags.Set(Town03Trainers.GymLeader.DefeatedFlag);
            _state.Party.Add(OwnedMonster.Create(starter, StoryBalance.StarterLevel));
        }

        [Test]
        public void Rival5_SpotsPlayerWhereverTheyCrossTheRow()
        {
            AfterTown03(Town01Starters.All[0]);
            var rival = Town04People.Route07RivalSpot.Position;
            for (int x = rival.X + 1; x <= rival.X + Town04Trainers.Rival5Sight; x++)
            {
                var sighting = PlayerAt(Town04Maps.Route07, new GridPos(x, rival.Y), Direction.Up).SpottedBy();
                Assert.IsNotNull(sighting, $"x={x}");
                Assert.AreEqual(NpcLook.Rival, sighting.Value.Trainer.Npc.Look);
            }
            // 段の東の端の先は木で、北へ抜けるにはこの段を通るしかない
            Assert.IsFalse(_world.Get(Town04Maps.Route07).IsWalkable(new GridPos(rival.X + Town04Trainers.Rival5Sight + 1, rival.Y)));
        }

        [Test]
        public void Rival5_HasEvolvedCounterStarterAtLv30()
        {
            foreach (var starter in Town01Starters.All)
            {
                SetUp();
                AfterTown03(starter);

                var placed = _people.At(Town04Maps.Route07, Town04People.Route07RivalSpot.Position);
                var party = placed.Value.Placement.Trainer.Party;

                var counter = Town01Starters.CounterOf(starter);
                Assert.AreEqual(4, party.Count);
                Assert.AreEqual(counter.Evolution.IntoNo, party.Last().Species.No, starter.Name);
                Assert.AreEqual(30, party.Last().Level);
                Assert.IsTrue(party.Take(3).All(mon => mon.Level >= 27 && mon.Level <= 28));
                Assert.AreEqual(CpuLevel.Normal, placed.Value.Placement.Trainer.Cpu);
            }
        }

        [Test]
        public void Rival5_EvenIfLost_HealsAtYukiokoshiHutAndLeaves()
        {
            AfterTown03(Town01Starters.All[2]);
            var script = _people.At(Town04Maps.Route07, Town04People.Route07RivalSpot.Position).Value.Placement.Script;

            Run(script, won: false);

            Assert.IsTrue(_state.Flags.Has(Town04Trainers.Rival5Beaten));
            Assert.AreEqual(Town03Maps.WarmHut, _state.LastHealSpot.MapId);
            Assert.IsNull(_people.At(Town04Maps.Route07, Town04People.Route07RivalSpot.Position));
        }

        // ---- ハヤテ・道場 ----

        [Test]
        public void Hayate_MetAtDeck_OpensGymAndWaitsInside()
        {
            Assert.IsNull(_people.At(Town04Maps.Gym, Town04People.GymLeaderSpot.Position));

            Run(HayateAtDeckScript());

            Assert.IsTrue(_state.Flags.Has(Town04Flags.MetHayate));
            Assert.IsNull(_people.At(Town04Maps.Bridge, Town04People.HayateAtLookoutDeck.Position));
            Assert.IsNull(_people.At(Town04Maps.Town, Town04People.DiscipleAtGymDoor.Position));
            Assert.IsTrue(GymLeaderScript().Any(step => step.Kind == EventStepKind.Battle));
        }

        [Test]
        public void GymLeader_Won_GivesTown04BadgeWiseFeatherAndPrize_ThenPointsToRoute09()
        {
            _state.Flags.Set(Town04Flags.MetHayate);
            int money = _state.Money;

            Run(GymLeaderScript(), won: true);

            Assert.IsTrue(_state.Badges.Contains(Town04Trainers.BadgeId));
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldWiseFeather));
            Assert.AreEqual(money + Town04Trainers.GymLeader.Prize, _state.Money);
            Assert.IsTrue(GymLeaderScript().All(step => step.Kind == EventStepKind.Say));
            Assert.IsTrue(GymLeaderScript().Any(step => step.Text.Contains("9番道路")));
        }

        [Test]
        public void GymLeader_Lost_GivesNoBadge()
        {
            _state.Flags.Set(Town04Flags.MetHayate);

            Run(GymLeaderScript(), won: false);

            Assert.IsFalse(_state.Badges.Contains(Town04Trainers.BadgeId));
            Assert.AreEqual(0, _state.Bag.CountOf(StoryItem.HeldWiseFeather));
            Assert.IsTrue(GymLeaderScript().Any(step => step.Kind == EventStepKind.Battle));
        }

        [Test]
        public void GymLeader_DoesNotChallengeOnSight()
        {
            _state.Flags.Set(Town04Flags.MetHayate);

            Assert.IsNull(PlayerAt(Town04Maps.Gym, new GridPos(5, 3), Direction.Up).SpottedBy());
        }

        [Test]
        public void GymLeader_IsBirdWithAceTsumujiLv32HoldingItem()
        {
            var party = Town04Trainers.GymLeader.Party;

            Assert.AreEqual(3, party.Count);
            Assert.IsTrue(party.All(mon => mon.Species.Definition.HasType(MonsterType.Bird)));
            Assert.AreEqual(new[] { 29, 30, 32 }, party.Select(mon => mon.Level).ToArray());
            Assert.AreSame(Town04Species.Tsumuji, party.Last().Species);
            Assert.AreNotEqual(HeldItem.None, party.Last().Item);
            Assert.IsTrue(party.Take(2).All(mon => mon.Item == HeldItem.None));
            // 切り札は 向かい風 を持つ（PLAN_STORY_TOWN04.md §7）
            var ace = Town04Trainers.GymLeader.CreateParty().Last();
            Assert.IsTrue(ace.Moves.Contains(MoveTable.Headwind));
        }

        [Test]
        public void GymDisciples_CanBeAvoided()
        {
            _state.Flags.Set(Town04Flags.MetHayate);
            var player = PlayerAt(Town04Maps.Town, Town04Maps.GymDoor + Direction.Down.Offset(), Direction.Up);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Up));
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town04Maps.Gym, player.Map.Id);

            // 右の壁ぞいを上がり、奥の段を左へ
            var front = Town04People.GymLeaderSpot.Position + Direction.Down.Offset();
            WalkWithoutBeingSpotted(player, Direction.Right, 9 - player.Position.X);
            WalkWithoutBeingSpotted(player, Direction.Up, player.Position.Y - front.Y);
            WalkWithoutBeingSpotted(player, Direction.Left, player.Position.X - front.X);
            Assert.AreEqual(front, player.Position);
        }

        private static void WalkWithoutBeingSpotted(FieldPlayer player, Direction direction, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(StepResult.Moved, player.Step(direction), $"{player.Position} から {direction}");
                Assert.IsNull(player.SpottedBy(), $"{player.Position}");
            }
        }

        // ---- 虹の羽の子 ----

        [Test]
        public void Rainbow_IsNijibaneLv30()
        {
            var wild = Town04People.RainbowScript.Single(step => step.Kind == EventStepKind.WildBattle);

            Assert.AreSame(Town04Species.Nijibane, wild.Species);
            Assert.AreEqual(30, wild.Level);
            Assert.IsTrue(wild.CatchOnly);
        }

        [Test]
        public void Rainbow_Caught_SetsFlagAndNeverAppearsAgain()
        {
            Assert.IsTrue(Town04People.RainbowAppears(_state.Flags));

            Run(Town04People.RainbowScript, wild: Wild.Caught);

            Assert.IsTrue(_state.Flags.Has(Town04Flags.RainbowCaught));
            Assert.IsFalse(Town04People.RainbowAppears(_state.Flags));
        }

        [Test]
        public void Rainbow_DefeatedOrFled_FliesAway_AndCanAppearAgain()
        {
            foreach (var result in new[] { Wild.Defeated, Wild.Fled })
            {
                SetUp();
                var lines = Run(Town04People.RainbowScript, wild: result);

                Assert.IsFalse(_state.Flags.Has(Town04Flags.RainbowCaught), result.ToString());
                Assert.IsTrue(Town04People.RainbowAppears(_state.Flags));
                StringAssert.Contains("帰っていった", lines.Last());
            }
        }

        [Test]
        public void Rainbow_Lost_EndsWithoutFlagOrFarewell()
        {
            var lines = Run(Town04People.RainbowScript, wild: Wild.Lost);

            Assert.IsFalse(_state.Flags.Has(Town04Flags.RainbowCaught));
            Assert.IsFalse(lines.Any(line => line.Contains("帰っていった")));
        }

        // ---- 道路のトレーナー ----

        [Test]
        public void RouteTrainers_SpotPlayerOnRoad()
        {
            // 7番道路は真ん中の道（x=10）を、大橋は橋の上を東へ走ると目が合う
            var spots = new[]
            {
                (Town04Maps.Route07, new GridPos(10, 27), Town04Trainers.Route07KiteKid),
                (Town04Maps.Route07, new GridPos(10, 20), Town04Trainers.Route07Hiker),
                (Town04Maps.Route07, new GridPos(10, 9), Town04Trainers.Route07Angler),
                (Town04Maps.Bridge, new GridPos(18, 6), Town04Trainers.BridgeCyclist1),
                (Town04Maps.Bridge, new GridPos(23, 5), Town04Trainers.BridgeCyclist2),
                (Town04Maps.Bridge, new GridPos(29, 4), Town04Trainers.BridgeCyclist3),
            };
            foreach (var (mapId, pos, trainer) in spots)
            {
                var sighting = PlayerAt(mapId, pos, Direction.Up).SpottedBy();
                Assert.IsNotNull(sighting, $"{mapId} {pos}");
                Assert.AreSame(trainer, sighting.Value.Trainer.Placement.Trainer);
            }
        }

        [Test]
        public void BridgeTrainers_DoNotStandOnSlopes()
        {
            // 下り坂の自動の進みを人で止めないよう、トレーナーは平らな橋の上に立つ
            var bridge = _world.Get(Town04Maps.Bridge);
            foreach (var npc in Town04People.Create())
            {
                foreach (var placement in npc.Placements.Where(placement => placement.Spot.MapId == Town04Maps.Bridge))
                {
                    Assert.AreNotEqual(TileKind.Downhill, bridge.TileAt(placement.Spot.Position), npc.Id);
                }
            }
        }

        [Test]
        public void Trainers_LevelsMatchPlan()
        {
            var route07 = new[] { Town04Trainers.Route07Angler, Town04Trainers.Route07Hiker, Town04Trainers.Route07KiteKid };
            var bridge = new[] { Town04Trainers.BridgeCyclist1, Town04Trainers.BridgeCyclist2, Town04Trainers.BridgeCyclist3 };
            var disciples = new[] { Town04Trainers.GymDisciple1, Town04Trainers.GymDisciple2 };

            Assert.IsTrue(route07.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 23 && mon.Level <= 26));
            Assert.IsTrue(bridge.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 25 && mon.Level <= 28));
            Assert.IsTrue(disciples.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 28 && mon.Level <= 29));
            Assert.IsTrue(disciples.SelectMany(trainer => trainer.Party).All(mon => mon.Species.Definition.HasType(MonsterType.Bird)));
            Assert.IsTrue(route07.Concat(bridge).All(trainer => trainer.Cpu == CpuLevel.Weak));
            Assert.IsTrue(disciples.All(trainer => trainer.Cpu == CpuLevel.Normal));
        }

        [Test]
        public void EveryTrainer_UsesStorySpecies_AndHasTown04Id()
        {
            var trainers = Town04TrainersOnField().ToList();

            Assert.GreaterOrEqual(trainers.Select(trainer => trainer.Id).Distinct().Count(), 10);
            foreach (var trainer in trainers)
            {
                StringAssert.StartsWith("town04.", trainer.Id);
                foreach (var mon in trainer.Party)
                {
                    Assert.AreSame(mon.Species, StorySpeciesTable.ByNo(mon.Species.No), $"{trainer.Id} {mon.Species?.Name}");
                }
            }
        }

        [Test]
        public void People_StandOnWalkableTiles_WithUniqueIds()
        {
            var people = Town04People.Create();

            Assert.AreEqual(people.Count, people.Select(npc => npc.Id).Distinct().Count());
            foreach (var npc in people)
            {
                foreach (var placement in npc.Placements)
                {
                    var map = _world.Get(placement.Spot.MapId);
                    Assert.IsTrue(map.IsWalkable(placement.Spot.Position), $"{npc.Id} {placement.Spot.Position}");
                    Assert.IsNull(map.WarpAt(placement.Spot.Position), npc.Id);
                }
            }
        }
    }
}
