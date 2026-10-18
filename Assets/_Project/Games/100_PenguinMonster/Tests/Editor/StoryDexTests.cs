using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>図鑑の見せ方（PLAN_STORY_TOWN01.md フェーズ8）: ストーリーの子は見た子だけ見せる</summary>
    public class StoryDexTests
    {
        private static readonly HashSet<int> NoneSeen = new HashSet<int>();

        [Test]
        public void DraftMonsters_AreAlwaysRevealed_StoryOnesOnlyWhenSeen()
        {
            var kobune = MonsterTable.ByNo(151);

            Assert.IsTrue(StoryDex.IsRevealed(MonsterTable.All[0], NoneSeen));
            Assert.IsFalse(StoryDex.IsRevealed(kobune, NoneSeen));
            Assert.IsTrue(StoryDex.IsRevealed(kobune, new HashSet<int> { 151 }));
        }

        [Test]
        public void All_ListsUnseenStoryMonstersToo()
        {
            Assert.AreEqual(MonsterTable.Dex.Count, StoryDex.Filter(null, NoneSeen).Count);
        }

        [Test]
        public void Dex_HasTown03UpTo240_AndUnseenOnesAreHidden()
        {
            // 図鑑のマスは Dex の体数ぶん作るので、区切り3を足したら 240 マスになる（PLAN_STORY_TOWN03.md フェーズ3c）
            const int LastNo = 240;
            Assert.AreEqual(LastNo, MonsterTable.Dex.Count);
            Assert.AreEqual(LastNo, MonsterTable.Dex[MonsterTable.Dex.Count - 1].No);
            Assert.IsFalse(StoryDex.IsRevealed(MonsterTable.ByNo(211), NoneSeen));
            Assert.IsTrue(StoryDex.IsRevealed(MonsterTable.ByNo(LastNo), new HashSet<int> { LastNo }));
        }

        [Test]
        public void TypeFilter_HidesUnseenStoryMonsters()
        {
            // 153 サイヒョウセンペン（海/氷）だけ見たことにする
            var seen = new HashSet<int> { 153 };

            var sea = StoryDex.Filter(MonsterType.Sea, seen);

            Assert.IsTrue(sea.Any(monster => monster.No == 153));
            Assert.IsFalse(sea.Any(monster => monster.No == 151));
            Assert.IsTrue(sea.All(monster => monster.HasType(MonsterType.Sea)));
        }
    }
}
