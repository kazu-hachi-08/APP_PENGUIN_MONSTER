using NUnit.Framework;

namespace PenguinMonster.Story.Tests
{
    /// <summary>フラグを下ろす・ランニングシューズ（PLAN_STORY_TOWN03.md フェーズ2）</summary>
    public class RunningShoesTests
    {
        [Test]
        public void Clear_RemovesOnlyThatFlag()
        {
            var flags = new StoryFlags();
            flags.Set("a");
            flags.Set("b");

            flags.Clear("a");
            flags.Clear("notSet");

            Assert.IsFalse(flags.Has("a"));
            Assert.IsTrue(flags.Has("b"));
        }

        [Test]
        public void Give_TurnsRunOn()
        {
            var flags = new StoryFlags();

            RunningShoes.Give(flags);

            Assert.IsTrue(RunningShoes.Has(flags));
            Assert.IsTrue(RunningShoes.IsOn(flags));
        }

        [Test]
        public void Toggle_SwitchesOffAndOn()
        {
            var flags = new StoryFlags();
            RunningShoes.Give(flags);

            RunningShoes.Toggle(flags);
            Assert.IsFalse(RunningShoes.IsOn(flags));

            RunningShoes.Toggle(flags);
            Assert.IsTrue(RunningShoes.IsOn(flags));
        }

        [Test]
        public void WithoutShoes_CannotRun()
        {
            var flags = new StoryFlags();

            RunningShoes.Toggle(flags);
            Assert.IsFalse(RunningShoes.IsOn(flags));

            // 走る ON だけ立っていても、シューズが無ければ歩く
            flags.Set(RunningShoes.RunOnFlag);
            Assert.IsFalse(RunningShoes.IsOn(flags));
        }
    }
}
