using System;
using NUnit.Framework;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    public class EventTests
    {
        private const string Flag = "test.flag";

        [Test]
        public void Say_StopsUntilAdvanced()
        {
            var runner = new EventRunner(new StoryState(Town01Maps.Start), new[] { EventStep.Say("1つめ"), EventStep.Say("2つめ") });

            Assert.AreEqual("1つめ", runner.Message);
            runner.Advance();
            Assert.AreEqual("2つめ", runner.Message);
            runner.Advance();
            Assert.IsTrue(runner.IsDone);
        }

        [Test]
        public void SetFlag_IsDoneWithoutStopping()
        {
            var state = new StoryState(Town01Maps.Start);

            var runner = new EventRunner(state, new[] { EventStep.SetFlag(Flag), EventStep.Say("立てた") });

            Assert.IsTrue(state.Flags.Has(Flag));
            Assert.AreEqual("立てた", runner.Message);
        }

        [Test]
        public void Ask_Yes_ContinuesScript()
        {
            var runner = new EventRunner(new StoryState(Town01Maps.Start), new[] { EventStep.Ask("いい？", EventStep.Say("残念")), EventStep.Say("やった") });

            runner.Answer(true);

            Assert.AreEqual("やった", runner.Message);
        }

        [Test]
        public void Ask_No_RunsOnNoInsteadOfRest()
        {
            var state = new StoryState(Town01Maps.Start);
            var runner = new EventRunner(state, new[]
            {
                EventStep.Ask("いい？", EventStep.Say("残念")), EventStep.SetFlag(Flag), EventStep.Say("やった"),
            });

            runner.Answer(false);
            Assert.AreEqual("残念", runner.Message);
            runner.Advance();

            Assert.IsTrue(runner.IsDone);
            Assert.IsFalse(state.Flags.Has(Flag));
        }

        [Test]
        public void ChooseStarter_GivesChosenAtLevel()
        {
            var state = new StoryState(Town01Maps.Start);
            var runner = new EventRunner(state, new[] { EventStep.ChooseStarter(new[] { Chick, Junior, Adult }, 5, "test.starter."), EventStep.SetFlag(Flag) });
            Assert.AreEqual(EventStepKind.ChooseStarter, runner.Current.Kind);

            runner.Choose(Junior);

            Assert.AreEqual(EventStepKind.Give, runner.Current.Kind);
            Assert.AreEqual("テストワカを もらった！", runner.Message);
            Assert.AreEqual(1, state.Party.Count);
            Assert.AreEqual(Junior, state.Party.Members[0].Species);
            Assert.AreEqual(5, state.Party.Members[0].Level);
            Assert.IsFalse(state.Flags.Has(Flag));
            runner.Advance();
            Assert.IsTrue(state.Flags.Has(Flag));
        }

        [Test]
        public void WrongAnswer_Throws()
        {
            var runner = new EventRunner(new StoryState(Town01Maps.Start), new[] { EventStep.Say("話すだけ") });

            Assert.Throws<InvalidOperationException>(() => runner.Answer(true));
            Assert.Throws<InvalidOperationException>(() => runner.Choose(Chick));
        }

        [Test]
        public void Party_WhenFull_RefusesMore()
        {
            var party = new Party();
            for (int i = 0; i < StoryBalance.MaxPartySize; i++)
            {
                Assert.IsTrue(party.Add(Owned(Chick, 5)));
            }

            Assert.IsTrue(party.IsFull);
            Assert.IsFalse(party.Add(Owned(Chick, 5)));
            Assert.AreEqual(StoryBalance.MaxPartySize, party.Count);
        }
    }
}
