using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>区切り1の人・看板・最初の1体をもらう出来事（PLAN_STORY_TOWN01.md フェーズ4）</summary>
    public class Town01EventTests
    {
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

        /// <summary>研究所で博士の前に立ち、話しかけた台本を「はい」で進め、選べと言われたら starter を選ぶ</summary>
        private EventRunner TalkToProfessor(StorySpecies starter = null, bool accept = true)
        {
            var player = PlayerAt(Town01Maps.Lab, new GridPos(4, 3), Direction.Up);
            var runner = new EventRunner(_state, player.ScriptAhead());
            while (!runner.IsDone)
            {
                switch (runner.Current.Kind)
                {
                    case EventStepKind.Ask: runner.Answer(accept); break;
                    case EventStepKind.ChooseStarter: runner.Choose(starter ?? runner.Current.Choices[0]); break;
                    default: runner.Advance(); break;
                }
            }
            return runner;
        }

        private static List<string> Lines(IReadOnlyList<EventStep> script) => script.Select(step => step.Text).ToList();

        [Test]
        public void Professor_GivesChosenStarterAndSetsFlag()
        {
            var fire = Town01Starters.All[1];

            TalkToProfessor(fire);

            Assert.IsTrue(_state.Flags.Has(Town01Flags.GotStarter));
            Assert.AreEqual(1, _state.Party.Count);
            Assert.AreEqual(fire, _state.Party.Members[0].Species);
            Assert.AreEqual(StoryBalance.StarterLevel, _state.Party.Members[0].Level);
        }

        [Test]
        public void Professor_Declined_GivesNothing()
        {
            TalkToProfessor(accept: false);

            Assert.IsFalse(_state.Flags.Has(Town01Flags.GotStarter));
            Assert.AreEqual(0, _state.Party.Count);
        }

        [Test]
        public void Professor_AfterStarter_SaysOtherLinesAndGivesNoMore()
        {
            TalkToProfessor();

            var script = PlayerAt(Town01Maps.Lab, new GridPos(4, 3), Direction.Up).ScriptAhead();
            Assert.IsFalse(script.Any(step => step.Kind == EventStepKind.ChooseStarter));
            TalkToProfessor();
            Assert.AreEqual(1, _state.Party.Count);
        }

        [Test]
        public void Mom_BlocksNorthExitUntilStarter()
        {
            var belowMom = Town01People.MomAtExit.Position + Direction.Down.Offset();
            var player = PlayerAt(Town01Maps.Village, belowMom, Direction.Up);

            Assert.AreEqual(StepResult.Blocked, player.Step(Direction.Up));
            Assert.AreEqual(NpcLook.Mom, player.PersonAhead.Value.Npc.Look);

            _state.Flags.Set(Town01Flags.GotStarter);
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Up));
            Assert.AreEqual(StepResult.Moved, player.Step(Direction.Up));
            Assert.AreEqual(Town01Maps.NorthExit, player.Position);
        }

        [Test]
        public void Mom_AfterStarter_IsAtHome()
        {
            _state.Flags.Set(Town01Flags.GotStarter);

            Assert.IsFalse(_people.On(Town01Maps.Village).Any(placed => placed.Npc.Look == NpcLook.Mom));
            Assert.IsTrue(_people.On(Town01Maps.PlayerHouse).Any(placed => placed.Npc.Look == NpcLook.Mom));
        }

        [Test]
        public void Sign_InFront_ReadsItsLines()
        {
            var player = PlayerAt(Town01Maps.Village, new GridPos(7, 7), Direction.Right);

            CollectionAssert.AreEqual(new[] { "ここは ハツユキ村", "雪の 降る 静かな 村" }, Lines(player.ScriptAhead()));
        }

        [Test]
        public void NothingInFront_HasNoScript()
        {
            Assert.IsNull(PlayerAt(Town01Maps.Village, new GridPos(10, 12), Direction.Down).ScriptAhead());
        }

        [Test]
        public void EverySignTile_HasLines()
        {
            foreach (var map in _world.Maps)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        var pos = new GridPos(x, y);
                        if (map.TileAt(pos) == TileKind.Sign)
                        {
                            Assert.IsNotNull(map.SignAt(pos), $"{map.Id} {pos}");
                        }
                    }
                }
            }
        }

        [Test]
        public void EveryPlacement_StandsOnWalkableTileWithoutWarp()
        {
            foreach (var npc in Town01People.Create())
            {
                foreach (var placement in npc.Placements)
                {
                    var map = _world.Get(placement.Spot.MapId);
                    Assert.IsTrue(map.IsWalkable(placement.Spot.Position), $"{npc.Id} {placement.Spot.Position}");
                    Assert.IsNull(map.WarpAt(placement.Spot.Position), $"{npc.Id} {placement.Spot.Position}");
                }
            }
        }

        [Test]
        public void Rival_AfterStarter_SpotsPlayerAtProfessor()
        {
            TalkToProfessor();

            var sighting = PlayerAt(Town01Maps.Lab, new GridPos(4, 3), Direction.Up).SpottedBy();

            Assert.IsNotNull(sighting);
            Assert.AreEqual(NpcLook.Rival, sighting.Value.Trainer.Npc.Look);
            Assert.AreEqual(1, sighting.Value.Steps);
        }

        [Test]
        public void Rival_ChoosesStarterStrongAgainstPlayers()
        {
            var sea = Town01Starters.All[0];
            TalkToProfessor(sea);

            var trainer = PlayerAt(Town01Maps.Lab, new GridPos(4, 3), Direction.Up).SpottedBy().Value.Trainer.Placement.Trainer;

            Assert.AreEqual(MonsterType.Moss, trainer.Party[0].Species.Definition.Types[0]);
        }

        [Test]
        public void Rival_AfterBattle_IsGone()
        {
            TalkToProfessor();
            _state.Flags.Set(Town01Trainers.RivalBeaten);

            Assert.IsFalse(_people.On(Town01Maps.Lab).Any(placed => placed.Npc.Look == NpcLook.Rival));
        }

        [Test]
        public void NorthExit_LeadsToRoute01AndBack()
        {
            _state.Flags.Set(Town01Flags.GotStarter);
            var player = PlayerAt(Town01Maps.Village, Town01Maps.NorthExit + Direction.Down.Offset(), Direction.Up);

            player.Step(Direction.Up);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.Route01, player.Map.Id);

            player.Step(Direction.Down);
            player.Enter(player.WarpHere);
            Assert.AreEqual(Town01Maps.Village, player.Map.Id);
        }

        [Test]
        public void Mom_AtHome_HealsAndSetsHomeAsReturnSpot()
        {
            _state.Flags.Set(Town01Flags.GotStarter);
            _state.Party.Add(OwnedMonster.Create(Town01Starters.All[0], StoryBalance.StarterLevel));
            var owned = _state.Party.Members[0];
            var fighter = owned.ToBattle();
            fighter.TakeDamage(5);
            owned.ApplyBattleResult(fighter);

            var script = PlayerAt(Town01Maps.PlayerHouse, new GridPos(4, 2), Direction.Right).ScriptAhead();
            var runner = new EventRunner(_state, script);
            while (!runner.IsDone)
            {
                runner.Advance();
            }

            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(Town01Maps.PlayerHouse, _state.LastHealSpot.MapId);
        }

        [Test]
        public void Species_EvolutionsAndEncountersAreDefined()
        {
            foreach (var starter in Town01Starters.All)
            {
                var evolved = Town01Species.ByNo(starter.Evolution.IntoNo);
                Assert.IsNotNull(evolved, starter.Name);
                Assert.AreEqual(starter.Definition.Types[0], evolved.Definition.Types[0], starter.Name);
            }
            foreach (var map in _world.Maps.Where(map => map.Encounters != null))
            {
                foreach (var slot in map.Encounters.Slots)
                {
                    Assert.AreEqual(slot.Species, Town01Species.ByNo(slot.Species.No), map.Id);
                }
            }
        }

        [Test]
        public void Starters_AreSeaFireMossAndYoung()
        {
            var types = Town01Starters.All.Select(species => species.Definition.Types[0]).ToArray();

            CollectionAssert.AreEqual(new[] { MonsterType.Sea, MonsterType.Fire, MonsterType.Moss }, types);
            Assert.IsTrue(Town01Starters.All.All(species => species.Definition.Rank == MonsterRank.Young));
            Assert.IsTrue(Town01Starters.All.All(species => OwnedMonster.Create(species, StoryBalance.StarterLevel).Moves.Count == StoryBalance.MaxMoves));
        }
    }
}
