using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り1の物語の出来事: ユキトとの2戦目・道場主ガンテツと羽章・トレーナーの手持ち（PLAN_STORY_TOWN01.md フェーズ8c）</summary>
    public class Town01StoryTests
    {
        private const int GymLeaderAceLevel = 14;

        private StoryState _state;
        private FieldWorld _world;
        private FieldPeople _people;

        [SetUp]
        public void SetUp()
        {
            _state = new StoryState(Town01Maps.Start);
            _world = Town01Maps.CreateWorld();
            _people = new FieldPeople(Town01People.Create(), _state.Flags);
        }

        private FieldPlayer PlayerAt(string mapId, GridPos pos, Direction facing)
        {
            return new FieldPlayer(_world, new FieldSpot(mapId, pos, facing), _people);
        }

        /// <summary>最初の1体をもらって、ユキトとの1戦目を終えたところ</summary>
        private void AfterStarter(StorySpecies starter)
        {
            _state.Flags.Set(Town01Flags.GotStarter);
            _state.Flags.Set(EventStep.ChosenFlag(Town01Flags.StarterPrefix, starter));
            _state.Flags.Set(Town01Trainers.RivalBeaten);
            _state.Party.Add(OwnedMonster.Create(starter, StoryBalance.StarterLevel));
        }

        /// <summary>台本を最後まで流す。バトルは won で決着させ、通った行の種類を返す</summary>
        private List<EventStepKind> Run(IReadOnlyList<EventStep> script, bool won)
        {
            var runner = new EventRunner(_state, script);
            var kinds = new List<EventStepKind>();
            while (!runner.IsDone)
            {
                kinds.Add(runner.Current.Kind);
                if (runner.Current.Kind == EventStepKind.Battle)
                {
                    runner.FinishBattle(won);
                }
                else
                {
                    runner.Advance();
                }
            }
            return kinds;
        }

        [Test]
        public void Opening_HasLines()
        {
            Assert.IsTrue(Town01People.Opening.Count > 0);
            Assert.IsTrue(Town01People.Opening.All(step => step.Kind == EventStepKind.Say));
        }

        [Test]
        public void Rival2_SpotsPlayerAtRoute02End_WithCounterStarter()
        {
            var sea = Town01Starters.All[0];
            AfterStarter(sea);

            var sighting = PlayerAt(Town01Maps.Route02, new GridPos(27, 19), Direction.Down).SpottedBy();

            Assert.IsNotNull(sighting);
            var party = sighting.Value.Trainer.Placement.Trainer.Party;
            Assert.AreEqual(Town01Starters.CounterOf(sea), party.Last().Species);
        }

        [Test]
        public void Rival2_EvenIfLost_HealsAndOpensGym()
        {
            AfterStarter(Town01Starters.All[1]);
            var sighting = PlayerAt(Town01Maps.Route02, new GridPos(27, 19), Direction.Down).SpottedBy().Value;
            var owned = _state.Party.Members[0];
            var fighter = owned.ToBattle();
            fighter.TakeDamage(owned.MaxHp);
            owned.ApplyBattleResult(fighter);

            Run(sighting.Trainer.Placement.Script, won: false);

            Assert.IsTrue(_state.Flags.Has(Town01Flags.Route02Cleared));
            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.IsFalse(_people.On(Town01Maps.Route02).Any(placed => placed.Npc.Look == NpcLook.Rival));
        }

        [Test]
        public void GymLeader_Won_GivesBadgeAndPrize()
        {
            var script = PlayerAt(Town01Maps.Gym, Town01People.GymLeaderSpot.Position + Direction.Down.Offset(), Direction.Up).ScriptAhead();

            var kinds = Run(script, won: true);

            CollectionAssert.Contains(kinds, EventStepKind.GiveBadge);
            CollectionAssert.Contains(_state.Badges, Town01Trainers.BadgeId);
            Assert.IsTrue(_state.Flags.Has(Town01Trainers.GymLeader.DefeatedFlag));
            Assert.AreEqual(StoryBalance.StartMoney + Town01Trainers.GymLeader.Prize, _state.Money);
        }

        [Test]
        public void GymLeader_Lost_GivesNoBadge()
        {
            var script = PlayerAt(Town01Maps.Gym, Town01People.GymLeaderSpot.Position + Direction.Down.Offset(), Direction.Up).ScriptAhead();

            Run(script, won: false);

            Assert.IsEmpty(_state.Badges);
            Assert.IsFalse(_state.Flags.Has(Town01Trainers.GymLeader.DefeatedFlag));
        }

        [Test]
        public void GymLeader_DoesNotChallengeOnSight()
        {
            Assert.IsNull(PlayerAt(Town01Maps.Gym, new GridPos(5, 3), Direction.Up).SpottedBy());
        }

        [Test]
        public void GymLeader_IsRockAndAceIsLv14()
        {
            var party = Town01Trainers.GymLeader.Party;

            Assert.IsTrue(party.All(mon => mon.Species.Definition.HasType(MonsterType.Rock)));
            Assert.AreEqual(GymLeaderAceLevel, party.Max(mon => mon.Level));
        }

        [Test]
        public void EveryTrainer_UsesTown01Species()
        {
            var trainers = Town01People.Create()
                .SelectMany(npc => npc.Placements)
                .Select(placement => placement.Trainer ?? placement.Script.FirstOrDefault(step => step.Kind == EventStepKind.Battle)?.Trainer)
                .Where(trainer => trainer != null)
                .ToList();

            Assert.GreaterOrEqual(trainers.Select(trainer => trainer.Id).Distinct().Count(), 9);
            foreach (var mon in trainers.SelectMany(trainer => trainer.Party))
            {
                Assert.AreSame(mon.Species, Town01Species.ByNo(mon.Species.No), mon.Species.Name);
            }
        }
    }
}
