using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り3の人・トレーナー・出来事（PLAN_STORY_TOWN03.md フェーズ5）</summary>
    public class Town03StoryTests
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

        /// <summary>台本を最後まで流す。Ask には answer で答え、トレーナー戦は won、野生は wildCleared で決着させる</summary>
        private void Run(IReadOnlyList<EventStep> script, bool answer = true, bool won = true, bool wildCleared = true)
        {
            var runner = new EventRunner(_state, script);
            while (!runner.IsDone)
            {
                switch (runner.Current.Kind)
                {
                    case EventStepKind.Ask:
                        runner.Answer(answer);
                        break;
                    case EventStepKind.Battle:
                        runner.FinishBattle(won);
                        break;
                    case EventStepKind.WildBattle:
                        runner.FinishWildBattle(wildCleared);
                        break;
                    default:
                        runner.Advance();
                        break;
                }
            }
        }

        private IReadOnlyList<EventStep> ScriptBelow(string mapId, FieldSpot spot)
        {
            return PlayerAt(mapId, spot.Position + Direction.Down.Offset(), Direction.Up).ScriptAhead();
        }

        private IReadOnlyList<EventStep> KanteraGiverScript() => ScriptBelow(Town03Maps.TownHouse, Town03People.KanteraGiverSpot);
        private IReadOnlyList<EventStep> PowerPlantScript() => ScriptBelow(Town03Maps.PowerPlant, Town03People.PowerPlantCoreSpot);
        private IReadOnlyList<EventStep> GymLeaderScript() => ScriptBelow(Town03Maps.Gym, Town03People.GymLeaderSpot);

        private static IEnumerable<TrainerDefinition> Town03TrainersOnField()
        {
            return Town03People.Create()
                .SelectMany(npc => npc.Placements)
                .Select(placement => placement.Trainer)
                .Where(trainer => trainer != null);
        }

        // ---- 母のランニングシューズ ----

        /// <summary>5番道路の北の端から町へ入る</summary>
        private FieldPlayer EnterTownFromRoute05()
        {
            var player = PlayerAt(Town03Maps.Route05, new GridPos(9, 1), Direction.Up);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Up));
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town03Maps.Town, player.Map.Id);
            return player;
        }

        [Test]
        public void Mom_GreetsOnEnteringTown_AndGivesRunningShoesOnce()
        {
            var player = EnterTownFromRoute05();
            var greeter = player.GreeterHere();
            Assert.IsNotNull(greeter);
            Assert.AreEqual(NpcLook.Mom, greeter.Value.Npc.Look);

            Run(greeter.Value.Placement.Script);

            Assert.IsTrue(RunningShoes.Has(_state.Flags));
            Assert.IsTrue(RunningShoes.IsOn(_state.Flags));
            Assert.IsNull(EnterTownFromRoute05().GreeterHere());
            Assert.IsNull(_people.At(Town03Maps.Town, Town03People.MomAtTownSouth.Position));
        }

        [Test]
        public void Mom_StandsNextToSouthEntrance()
        {
            var player = EnterTownFromRoute05();

            Assert.AreEqual(Town03People.MomAtTownSouth.Position, player.Position + Town03People.MomAtTownSouth.Facing.Opposite().Offset());
        }

        // ---- 発電所の No.239 ----

        [Test]
        public void PowerPlant_ShowsHesotoriAsWildMonster()
        {
            var placed = _people.At(Town03Maps.PowerPlant, Town03People.PowerPlantCoreSpot.Position);

            Assert.IsNotNull(placed);
            Assert.AreEqual(NpcLook.Wild, placed.Value.Npc.Look);
            Assert.AreSame(Town03Species.Hesotori, placed.Value.Npc.Monster);
            var wild = PowerPlantScript().Single(step => step.Kind == EventStepKind.WildBattle);
            Assert.AreSame(Town03Species.Hesotori, wild.Species);
            Assert.AreEqual(24, wild.Level);
        }

        [Test]
        public void PowerPlant_DefeatedOrCaught_RestoresPowerAndOpensGym()
        {
            Run(PowerPlantScript(), wildCleared: true);

            Assert.IsTrue(_state.Flags.Has(Town03Flags.PowerRestored));
            Assert.IsNull(PowerPlantScript());
            Assert.IsNull(_people.At(Town03Maps.Town, Town03People.LeaderAtGymDoor.Position));
            Assert.IsNotNull(_people.At(Town03Maps.Gym, Town03People.GymLeaderSpot.Position));
        }

        [Test]
        public void PowerPlant_FledOrLost_KeepsPowerOff_AndCanFightAgain()
        {
            Run(PowerPlantScript(), wildCleared: false);

            Assert.IsFalse(_state.Flags.Has(Town03Flags.PowerRestored));
            Assert.IsTrue(PowerPlantScript().Any(step => step.Kind == EventStepKind.WildBattle));
            Assert.IsNotNull(_people.At(Town03Maps.Town, Town03People.LeaderAtGymDoor.Position));
        }

        [Test]
        public void Hesotori_IsNeverWild()
        {
            var tables = _world.Maps.SelectMany(map => new[] { map.Encounters, map.DeepEncounters }).Where(table => table != null);

            Assert.IsFalse(tables.SelectMany(table => table.Slots).Any(slot => slot.Species == Town03Species.Hesotori));
        }

        // ---- No.238 をくれる人 ----

        [Test]
        public void Kantera_GivenOnceAtLv20()
        {
            Run(KanteraGiverScript());
            Run(KanteraGiverScript());

            Assert.AreEqual(1, _state.Party.Count);
            Assert.AreEqual(238, _state.Party.Members[0].Species.No);
            Assert.AreEqual(Town03People.KanteraLevel, _state.Party.Members[0].Level);
            Assert.IsTrue(_state.Flags.Has(Town03Flags.GotKantera));
        }

        [Test]
        public void Kantera_Declined_GivesNothingAndCanAskAgain()
        {
            Run(KanteraGiverScript(), answer: false);

            Assert.AreEqual(0, _state.Party.Count);
            Assert.IsFalse(_state.Flags.Has(Town03Flags.GotKantera));
            Assert.IsTrue(KanteraGiverScript().Any(step => step.Kind == EventStepKind.Give));
        }

        [Test]
        public void Kantera_WithFullParty_GoesToBox()
        {
            while (!_state.Party.IsFull)
            {
                _state.Party.Add(OwnedMonster.Create(Town01Species.Kogara, 5));
            }

            Run(KanteraGiverScript());

            Assert.AreEqual(1, _state.Box.Count);
            Assert.AreEqual(238, _state.Box.Members[0].Species.No);
        }

        // ---- ユキトの4戦目 ----

        /// <summary>最初の1体をもらい、区切り2の道場まで終えたところ（5番道路へ進める）</summary>
        private void AfterTown02(StorySpecies starter)
        {
            _state.Flags.Set(Town01Flags.GotStarter);
            _state.Flags.Set(EventStep.ChosenFlag(Town01Flags.StarterPrefix, starter));
            _state.Flags.Set(Town01Trainers.RivalBeaten);
            _state.Flags.Set(Town01Trainers.Rival2Beaten);
            _state.Flags.Set(Town01Trainers.GymLeader.DefeatedFlag);
            _state.Flags.Set(Town02Trainers.Rival3Beaten);
            _state.Flags.Set(Town02Flags.Route04Cleared);
            _state.Flags.Set(Town02Trainers.GymLeader.DefeatedFlag);
            _state.Party.Add(OwnedMonster.Create(starter, StoryBalance.StarterLevel));
        }

        [Test]
        public void Rival4_SpotsPlayerWhereverTheyCrossTheGap()
        {
            AfterTown02(Town01Starters.All[0]);
            var rival = Town03People.Route05RivalSpot.Position;
            for (int x = rival.X + 1; x <= rival.X + Town03Trainers.Rival4Sight; x++)
            {
                var sighting = PlayerAt(Town03Maps.Route05, new GridPos(x, rival.Y), Direction.Up).SpottedBy();
                Assert.IsNotNull(sighting, $"x={x}");
                Assert.AreEqual(NpcLook.Rival, sighting.Value.Trainer.Npc.Look);
            }
            // 切れ目の東の端の先は木で、北へ抜けるにはこの列を通るしかない
            Assert.IsFalse(_world.Get(Town03Maps.Route05).IsWalkable(new GridPos(rival.X + Town03Trainers.Rival4Sight + 1, rival.Y)));
        }

        [Test]
        public void Rival4_HasEvolvedCounterStarterAtLv24()
        {
            foreach (var starter in Town01Starters.All)
            {
                SetUp();
                AfterTown02(starter);

                var placed = _people.At(Town03Maps.Route05, Town03People.Route05RivalSpot.Position);
                var party = placed.Value.Placement.Trainer.Party;

                var counter = Town01Starters.CounterOf(starter);
                Assert.AreEqual(counter.Evolution.IntoNo, party.Last().Species.No, starter.Name);
                Assert.AreEqual(24, party.Last().Level);
                Assert.IsTrue(party.Take(2).All(mon => mon.Level >= 21 && mon.Level <= 22));
                Assert.AreEqual(CpuLevel.Normal, placed.Value.Placement.Trainer.Cpu);
            }
        }

        [Test]
        public void Rival4_EvenIfLost_HealsAtKagamikoHutAndLeaves()
        {
            AfterTown02(Town01Starters.All[1]);
            var owned = _state.Party.Members[0];
            var fighter = owned.ToBattle();
            fighter.TakeDamage(owned.MaxHp);
            owned.ApplyBattleResult(fighter);
            var script = _people.At(Town03Maps.Route05, Town03People.Route05RivalSpot.Position).Value.Placement.Script;

            Run(script, won: false);

            Assert.IsTrue(_state.Flags.Has(Town03Trainers.Rival4Beaten));
            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(Town02Maps.WarmHut, _state.LastHealSpot.MapId);
            Assert.IsNull(_people.At(Town03Maps.Route05, Town03People.Route05RivalSpot.Position));
        }

        // ---- 道場 ----

        [Test]
        public void GymLeader_Won_GivesTown03BadgeAndPrize_ThenPointsToRoute07()
        {
            _state.Flags.Set(Town03Flags.PowerRestored);
            int money = _state.Money;

            Run(GymLeaderScript(), won: true);

            Assert.IsTrue(_state.Badges.Contains(Town03Trainers.BadgeId));
            Assert.AreEqual(money + Town03Trainers.GymLeader.Prize, _state.Money);
            Assert.IsTrue(GymLeaderScript().All(step => step.Kind == EventStepKind.Say));
            Assert.IsTrue(GymLeaderScript().Any(step => step.Text.Contains("7番道路")));
        }

        [Test]
        public void GymLeader_Lost_GivesNoBadge()
        {
            _state.Flags.Set(Town03Flags.PowerRestored);

            Run(GymLeaderScript(), won: false);

            Assert.IsFalse(_state.Badges.Contains(Town03Trainers.BadgeId));
            Assert.IsTrue(GymLeaderScript().Any(step => step.Kind == EventStepKind.Battle));
        }

        [Test]
        public void GymLeader_DoesNotChallengeOnSight()
        {
            _state.Flags.Set(Town03Flags.PowerRestored);

            Assert.IsNull(PlayerAt(Town03Maps.Gym, new GridPos(5, 3), Direction.Up).SpottedBy());
        }

        [Test]
        public void GymLeader_IsThunderWithAceHatsudenLv26()
        {
            var party = Town03Trainers.GymLeader.Party;

            Assert.AreEqual(3, party.Count);
            Assert.IsTrue(party.All(mon => mon.Species.Definition.HasType(MonsterType.Thunder)));
            Assert.AreEqual(new[] { 23, 24, 26 }, party.Select(mon => mon.Level).ToArray());
            Assert.AreSame(Town03Species.Hatsuden, party.Last().Species);
        }

        [Test]
        public void GymDisciples_CanBeAvoided()
        {
            // 門下生と戦わずに、扉から道場主の前まで歩ける（PLAN_STORY_TOWN03.md §6「避けて道場主に挑める」）
            _state.Flags.Set(Town03Flags.PowerRestored);
            var player = PlayerAt(Town03Maps.Town, Town03Maps.GymDoor + Direction.Down.Offset(), Direction.Up);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Up));
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town03Maps.Gym, player.Map.Id);

            // 右の壁ぞいを上がり、奥の段を左へ
            var front = Town03People.GymLeaderSpot.Position + Direction.Down.Offset();
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

        // ---- 道路のトレーナー ----

        [Test]
        public void RouteTrainers_SpotPlayerOnRoad()
        {
            // 5番道路は真ん中の道（x=9）を、6番道路は町からの道（y=10）と発電所への道（x=24）を歩くと目が合う
            var spots = new[]
            {
                (Town03Maps.Route05, new GridPos(9, 25), Town03Trainers.Route05Hiker),
                (Town03Maps.Route05, new GridPos(9, 18), Town03Trainers.Route05SnowfieldKid),
                (Town03Maps.Route05, new GridPos(9, 13), Town03Trainers.Route05ThunderKid),
                (Town03Maps.Route06, new GridPos(8, 10), Town03Trainers.Route06Angler),
                (Town03Maps.Route06, new GridPos(24, 7), Town03Trainers.Route06LineWorker),
                (Town03Maps.Route06, new GridPos(16, 10), Town03Trainers.Route06NightWalker),
            };
            foreach (var (mapId, pos, trainer) in spots)
            {
                var sighting = PlayerAt(mapId, pos, Direction.Up).SpottedBy();
                Assert.IsNotNull(sighting, $"{mapId} {pos}");
                Assert.AreSame(trainer, sighting.Value.Trainer.Placement.Trainer);
            }
        }

        [Test]
        public void Trainers_LevelsMatchPlan()
        {
            var route05 = new[] { Town03Trainers.Route05Hiker, Town03Trainers.Route05SnowfieldKid, Town03Trainers.Route05ThunderKid };
            var route06 = new[] { Town03Trainers.Route06Angler, Town03Trainers.Route06LineWorker, Town03Trainers.Route06NightWalker };
            var disciples = new[] { Town03Trainers.GymDisciple1, Town03Trainers.GymDisciple2 };

            Assert.IsTrue(route05.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 17 && mon.Level <= 20));
            Assert.IsTrue(route06.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 19 && mon.Level <= 22));
            Assert.IsTrue(disciples.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 22 && mon.Level <= 23));
            Assert.IsTrue(route05.Concat(route06).All(trainer => trainer.Cpu == CpuLevel.Weak));
            Assert.IsTrue(disciples.All(trainer => trainer.Cpu == CpuLevel.Normal));
        }

        [Test]
        public void EveryTrainer_UsesStorySpecies_AndHasTown03Id()
        {
            var trainers = Town03TrainersOnField().ToList();

            Assert.GreaterOrEqual(trainers.Select(trainer => trainer.Id).Distinct().Count(), 10);
            foreach (var trainer in trainers)
            {
                StringAssert.StartsWith("town03.", trainer.Id);
                foreach (var mon in trainer.Party)
                {
                    Assert.AreSame(mon.Species, StorySpeciesTable.ByNo(mon.Species.No), $"{trainer.Id} {mon.Species?.Name}");
                }
            }
        }
    }
}
