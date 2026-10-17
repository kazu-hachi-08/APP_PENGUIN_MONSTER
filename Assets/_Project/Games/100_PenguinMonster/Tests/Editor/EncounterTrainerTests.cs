using System;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    /// <summary>草むらの出会い・トレーナーと目が合う・台本のバトルと回復（PLAN_STORY_TOWN01.md フェーズ5）</summary>
    public class EncounterTrainerTests
    {
        private static readonly string[] GrassRows =
        {
            "TTTTTT",
            "T,,..T",
            "T,,..T",
            "TTTTTT",
        };

        private static readonly TrainerDefinition Kid = new TrainerDefinition("test.kid", "テストの 子",
            new[] { new TrainerMon(Chick, 3), new TrainerMon(Junior, 16) }, prize: 100, sightRange: 3);

        private static StoryState StateWithMember()
        {
            var state = new StoryState(Town01Maps.Start);
            state.Party.Add(Owned(Chick, 5));
            return state;
        }

        private static FieldPlayer PlayerOn(FieldMap map, GridPos pos, FieldPeople people = null)
        {
            return new FieldPlayer(new FieldWorld(new[] { map }), new FieldSpot(map.Id, pos, Direction.Down), people);
        }

        private static FieldMap GrassMap(int ratePercent)
        {
            return new FieldMap("grass", GrassRows).WithEncounters(new EncounterTable(ratePercent, new EncounterSlot(Chick, 2, 4, 1)));
        }

        [Test]
        public void Encounter_AlwaysRate_GivesSpeciesInLevelRange()
        {
            var table = new EncounterTable(100, new EncounterSlot(Chick, 2, 4, 1), new EncounterSlot(Junior, 7, 7, 1));
            var random = new Random(1);

            for (int i = 0; i < 50; i++)
            {
                var wild = table.Roll(random);
                Assert.IsNotNull(wild);
                if (wild.Species == Chick)
                {
                    Assert.That(wild.Level, Is.InRange(2, 4));
                }
                else
                {
                    Assert.AreEqual(Junior, wild.Species);
                    Assert.AreEqual(7, wild.Level);
                }
            }
        }

        [Test]
        public void Encounter_ZeroRate_NeverHappens()
        {
            var player = PlayerOn(GrassMap(0), new GridPos(1, 1));
            var random = new Random(1);

            Assert.IsTrue(Enumerable.Range(0, 100).All(_ => player.RollEncounter(random, StateWithMember().Party) == null));
        }

        [Test]
        public void Encounter_OnlyOnGrassWithFighter()
        {
            var map = GrassMap(100);
            var random = new Random(1);

            Assert.IsNotNull(PlayerOn(map, new GridPos(1, 1)).RollEncounter(random, StateWithMember().Party));
            Assert.IsNull(PlayerOn(map, new GridPos(3, 1)).RollEncounter(random, StateWithMember().Party), "草むらでない");
            Assert.IsNull(PlayerOn(map, new GridPos(1, 1)).RollEncounter(random, new Party()), "手持ちがいない");
        }

        private static FieldPeople KidFacingLeftAt(GridPos pos, StoryFlags flags)
        {
            var npc = new FieldNpc("kid", NpcLook.Youngster,
                NpcPlacement.Until(Kid.DefeatedFlag, new FieldSpot("open", pos, Direction.Left), EventStep.Say("勝負だ！"), EventStep.Battle(Kid)),
                NpcPlacement.Always(new FieldSpot("open", pos, Direction.Left), EventStep.Say("負けた")));
            return new FieldPeople(new[] { npc }, flags);
        }

        private static FieldMap OpenMap()
        {
            return new FieldMap("open", new[] { "........", "....T..." });
        }

        [Test]
        public void Trainer_SeesPlayerInLine_AndWalksNextToThem()
        {
            var people = KidFacingLeftAt(new GridPos(6, 0), new StoryFlags());

            var sighting = PlayerOn(OpenMap(), new GridPos(3, 0), people).SpottedBy();

            Assert.IsNotNull(sighting);
            Assert.AreEqual(2, sighting.Value.Steps);
            Assert.AreEqual(Kid, sighting.Value.Trainer.Placement.Trainer);
        }

        [Test]
        public void Trainer_DoesNotSeeBeyondRangeOrBehind()
        {
            var people = KidFacingLeftAt(new GridPos(6, 0), new StoryFlags());

            Assert.IsNull(PlayerOn(OpenMap(), new GridPos(2, 0), people).SpottedBy(), "見える距離の外");
            Assert.IsNull(PlayerOn(OpenMap(), new GridPos(7, 0), people).SpottedBy(), "後ろ");
            Assert.IsNull(PlayerOn(OpenMap(), new GridPos(5, 1), people).SpottedBy(), "横の列");
        }

        [Test]
        public void Trainer_SightBlockedByTree()
        {
            var people = KidFacingLeftAt(new GridPos(6, 1), new StoryFlags());

            Assert.IsNull(PlayerOn(OpenMap(), new GridPos(3, 1), people).SpottedBy());
        }

        [Test]
        public void Trainer_AfterDefeat_DoesNotChallenge()
        {
            var flags = new StoryFlags();
            flags.Set(Kid.DefeatedFlag);
            var people = KidFacingLeftAt(new GridPos(6, 0), flags);

            Assert.IsNull(PlayerOn(OpenMap(), new GridPos(4, 0), people).SpottedBy());
        }

        [Test]
        public void Trainer_CreatesPartyFromLearnset()
        {
            var party = Kid.CreateParty();

            Assert.AreEqual(2, party.Count);
            Assert.AreEqual(16, party[1].Level);
            CollectionAssert.AreEqual(new[] { Splash, Bite, Kick, WingStrike }, party[1].Moves.ToArray());
        }

        private static EventRunner RunToBattle(StoryState state, bool canLose)
        {
            var runner = new EventRunner(state, new[] { EventStep.Battle(Kid, canLose), EventStep.Say("あとの話") });
            Assert.AreEqual(EventStepKind.Battle, runner.Current.Kind);
            return runner;
        }

        [Test]
        public void BattleStep_Won_PaysPrizeAndSetsFlag()
        {
            var state = StateWithMember();
            var runner = RunToBattle(state, canLose: false);

            runner.FinishBattle(won: true);

            Assert.AreEqual(StoryBalance.StartMoney + Kid.Prize, state.Money);
            Assert.IsTrue(state.Flags.Has(Kid.DefeatedFlag));
            Assert.AreEqual("あとの話", runner.Message);
        }

        [Test]
        public void BattleStep_Lost_StopsScriptWithoutFlag()
        {
            var state = StateWithMember();
            var runner = RunToBattle(state, canLose: false);

            runner.FinishBattle(won: false);

            Assert.IsTrue(runner.IsDone);
            Assert.IsFalse(state.Flags.Has(Kid.DefeatedFlag));
            Assert.AreEqual(StoryBalance.StartMoney, state.Money);
        }

        [Test]
        public void BattleStep_LostButAllowed_ContinuesAndSetsFlag()
        {
            var state = StateWithMember();
            var runner = RunToBattle(state, canLose: true);

            runner.FinishBattle(won: false);

            Assert.IsTrue(state.Flags.Has(Kid.DefeatedFlag));
            Assert.AreEqual("あとの話", runner.Message);
        }

        [Test]
        public void HealStep_HealsPartyAndRemembersSpot()
        {
            var state = StateWithMember();
            var owned = state.Party.Members[0];
            Hurt(owned, 5);
            var spot = new FieldSpot("inn", new GridPos(1, 2), Direction.Up);

            var runner = new EventRunner(state, new[] { EventStep.Heal("元気に なった", spot) });

            Assert.AreEqual("元気に なった", runner.Message);
            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual("inn", state.LastHealSpot.MapId);
            runner.Advance();
            Assert.IsTrue(runner.IsDone);
        }

        [Test]
        public void ChooseStarter_SetsChosenFlag()
        {
            var state = new StoryState(Town01Maps.Start);
            var runner = new EventRunner(state, new[] { EventStep.ChooseStarter(new[] { Chick, Junior }, 5, "test.starter.") });

            runner.Choose(Junior);
            runner.Advance();

            Assert.IsTrue(state.Flags.Has("test.starter." + Junior.No));
            Assert.IsFalse(state.Flags.Has("test.starter." + Chick.No));
        }

        [Test]
        public void BattleSide_FromMonsters_StartsWithFirstConscious()
        {
            var fainted = new BattleMonster(Chick.Definition, 0, StatusCondition.None);
            var healthy = new BattleMonster(Chick.Definition);

            var state = new BattleState(new[] { fainted, healthy }, new[] { new BattleMonster(Chick.Definition) });

            Assert.AreEqual(healthy, state.Sides[0].Active);
            Assert.IsFalse(state.Sides[0].CanSwitchTo(0));
        }
    }
}
