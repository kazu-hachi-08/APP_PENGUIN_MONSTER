using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り2の人・トレーナー・出来事（PLAN_STORY_TOWN02.md フェーズ4）</summary>
    public class Town02StoryTests
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

        /// <summary>台本を最後まで流す。Ask には answer で答え、バトルは won で決着させる</summary>
        private void Run(IReadOnlyList<EventStep> script, bool answer = true, bool won = true)
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
                    default:
                        runner.Advance();
                        break;
                }
            }
        }

        private IReadOnlyList<EventStep> OmamoriGiverScript()
        {
            var below = Town02People.OmamoriGiverSpot.Position + Direction.Down.Offset();
            return PlayerAt(Town02Maps.TownHouse, below, Direction.Up).ScriptAhead();
        }

        private static IEnumerable<TrainerDefinition> Town02TrainersOnField()
        {
            return Town02People.Create()
                .SelectMany(npc => npc.Placements)
                .Select(placement => placement.Trainer)
                .Where(trainer => trainer != null);
        }

        [Test]
        public void Omamori_GivenOnceAtLv15()
        {
            Run(OmamoriGiverScript());
            Run(OmamoriGiverScript());

            Assert.AreEqual(1, _state.Party.Count);
            Assert.AreEqual(209, _state.Party.Members[0].Species.No);
            Assert.AreEqual(Town02People.OmamoriLevel, _state.Party.Members[0].Level);
            Assert.IsTrue(_state.Flags.Has(Town02Flags.GotOmamori));
        }

        [Test]
        public void Omamori_Declined_GivesNothingAndCanAskAgain()
        {
            Run(OmamoriGiverScript(), answer: false);

            Assert.AreEqual(0, _state.Party.Count);
            Assert.IsFalse(_state.Flags.Has(Town02Flags.GotOmamori));
            Assert.IsTrue(OmamoriGiverScript().Any(step => step.Kind == EventStepKind.Give));
        }

        [Test]
        public void Omamori_WithFullParty_GoesToBox()
        {
            while (!_state.Party.IsFull)
            {
                _state.Party.Add(OwnedMonster.Create(Town01Species.Kogara, 5));
            }

            Run(OmamoriGiverScript());

            Assert.AreEqual(1, _state.Box.Count);
            Assert.AreEqual(209, _state.Box.Members[0].Species.No);
        }

        /// <summary>最初の1体をもらい、区切り1の道場まで終えたところ（3番道路へ進める）</summary>
        private void AfterTown01(StorySpecies starter)
        {
            _state.Flags.Set(Town01Flags.GotStarter);
            _state.Flags.Set(EventStep.ChosenFlag(Town01Flags.StarterPrefix, starter));
            _state.Flags.Set(Town01Trainers.RivalBeaten);
            _state.Flags.Set(Town01Trainers.Rival2Beaten);
            _state.Flags.Set(Town01Trainers.GymLeader.DefeatedFlag);
            _state.Badges.Add(Town01Trainers.BadgeId);
            _state.Party.Add(OwnedMonster.Create(starter, StoryBalance.StarterLevel));
        }

        /// <summary>4番道路の奥の入口へ左から歩いてきて、ユキトと目が合ったところ</summary>
        private TrainerSighting SpotRival3()
        {
            var pos = Town02People.Route04EndSpot.Position + Direction.Left.Offset() + Direction.Left.Offset();
            var sighting = PlayerAt(Town02Maps.Route04, pos, Direction.Right).SpottedBy();
            Assert.IsNotNull(sighting);
            return sighting.Value;
        }

        private IReadOnlyList<EventStep> GymLeaderScript()
        {
            var below = Town02People.GymLeaderSpot.Position + Direction.Down.Offset();
            return PlayerAt(Town02Maps.Gym, below, Direction.Up).ScriptAhead();
        }

        [Test]
        public void Rival3_HasEvolvedCounterStarterAtLv18()
        {
            foreach (var starter in Town01Starters.All)
            {
                SetUp();
                AfterTown01(starter);

                var party = SpotRival3().Trainer.Placement.Trainer.Party;

                var counter = Town01Starters.CounterOf(starter);
                Assert.AreEqual(counter.Evolution.IntoNo, party.Last().Species.No, starter.Name);
                Assert.AreEqual(18, party.Last().Level);
                Assert.IsTrue(party.Any(mon => mon.Species == Town01Species.Tsugumi && mon.Level == 16));
            }
        }

        [Test]
        public void Rival3_EvenIfLost_HealsAtKagamikoHutAndOpensGym()
        {
            AfterTown01(Town01Starters.All[1]);
            var owned = _state.Party.Members[0];
            var fighter = owned.ToBattle();
            fighter.TakeDamage(owned.MaxHp);
            owned.ApplyBattleResult(fighter);

            Run(SpotRival3().Trainer.Placement.Script, won: false);

            Assert.IsTrue(_state.Flags.Has(Town02Flags.Route04Cleared));
            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(Town02Maps.WarmHut, _state.LastHealSpot.MapId);
            Assert.IsNull(_people.At(Town02Maps.Route04, Town02People.Route04EndSpot.Position));
            Assert.IsNull(_people.At(Town02Maps.Town, Town02People.DiscipleAtGymDoor.Position));
        }

        [Test]
        public void GymLeader_Won_GivesTown02BadgeAndPrize()
        {
            int money = _state.Money;

            Run(GymLeaderScript(), won: true);

            Assert.IsTrue(_state.Badges.Contains(Town02Trainers.BadgeId));
            Assert.AreEqual(money + Town02Trainers.GymLeader.Prize, _state.Money);
            Assert.IsTrue(GymLeaderScript().All(step => step.Kind == EventStepKind.Say));
            Assert.IsTrue(GymLeaderScript().Any(step => step.Text.Contains("5番道路")));
        }

        [Test]
        public void GymLeader_Lost_GivesNoBadge()
        {
            Run(GymLeaderScript(), won: false);

            Assert.IsFalse(_state.Badges.Contains(Town02Trainers.BadgeId));
            Assert.IsTrue(GymLeaderScript().Any(step => step.Kind == EventStepKind.Battle));
        }

        [Test]
        public void GymLeader_DoesNotChallengeOnSight()
        {
            Assert.IsNull(PlayerAt(Town02Maps.Gym, new GridPos(5, 3), Direction.Up).SpottedBy());
        }

        [Test]
        public void GymLeader_IsIceWithAceUsuraiLv20()
        {
            var party = Town02Trainers.GymLeader.Party;

            Assert.AreEqual(3, party.Count);
            Assert.IsTrue(party.All(mon => mon.Species.Definition.HasType(PenguinMonster.Battle.MonsterType.Ice)));
            Assert.AreSame(Town02Species.Usurai, party.Last().Species);
            Assert.AreEqual(20, party.Last().Level);
        }

        [Test]
        public void GymDisciples_CanBeAvoided()
        {
            // 門下生と戦わずに、扉から道場主の前まで歩ける（PLAN_STORY_TOWN02.md §3「避けて道場主に挑める」）
            var player = PlayerAt(Town02Maps.Town, Town02Maps.GymDoor + Direction.Down.Offset(), Direction.Up);
            _state.Flags.Set(Town02Flags.Route04Cleared);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Up));
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town02Maps.Gym, player.Map.Id);

            // 右の壁ぞいを上がり、奥の段を左へ
            var front = Town02People.GymLeaderSpot.Position + Direction.Down.Offset();
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

        [Test]
        public void RouteTrainers_SpotPlayerOnRoad()
        {
            // 3番道路は真ん中の道（x=9）を、4番道路は入口からの道を歩くと目が合う
            var spots = new[]
            {
                (Town02Maps.Route03, new GridPos(9, 19), Town02Trainers.Route03SnowfieldKid),
                (Town02Maps.Route03, new GridPos(9, 14), Town02Trainers.Route03Hiker),
                (Town02Maps.Route03, new GridPos(9, 23), Town02Trainers.Route03Angler),
                (Town02Maps.Route04, new GridPos(6, 9), Town02Trainers.Route04ThunderKid),
            };
            foreach (var (mapId, pos, trainer) in spots)
            {
                var sighting = PlayerAt(mapId, pos, Direction.Up).SpottedBy();
                Assert.IsNotNull(sighting, $"{mapId} {pos}");
                Assert.AreSame(trainer, sighting.Value.Trainer.Placement.Trainer);
            }
        }

        [Test]
        public void RouteTrainers_LevelsMatchPlan()
        {
            var route03 = new[] { Town02Trainers.Route03SnowfieldKid, Town02Trainers.Route03Angler, Town02Trainers.Route03Hiker };
            var route04 = new[] { Town02Trainers.Route04ThunderKid, Town02Trainers.Route04Skater, Town02Trainers.Route04NightWalker };

            Assert.IsTrue(route03.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 11 && mon.Level <= 14));
            Assert.IsTrue(route04.SelectMany(trainer => trainer.Party).All(mon => mon.Level >= 13 && mon.Level <= 16));
        }

        [Test]
        public void EveryTrainer_UsesStorySpecies_AndHasTown02Id()
        {
            var trainers = Town02TrainersOnField().ToList();

            Assert.GreaterOrEqual(trainers.Select(trainer => trainer.Id).Distinct().Count(), 6);
            foreach (var trainer in trainers)
            {
                StringAssert.StartsWith("town02.", trainer.Id);
                foreach (var mon in trainer.Party)
                {
                    Assert.AreSame(mon.Species, StorySpeciesTable.ByNo(mon.Species.No), $"{trainer.Id} {mon.Species?.Name}");
                }
            }
        }
    }
}
