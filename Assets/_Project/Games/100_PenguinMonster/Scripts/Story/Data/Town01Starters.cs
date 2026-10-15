using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 博士からもらう最初の3体（海・火・苔）。本物の No.151〜159 はフェーズ8a で作るので、今は既存のヒナを仮に使う。
    /// 進化前（Young）に付け替えるのは、ストーリーの子にヒナの綿毛を付けないため（PLAN_STORY_TOWN01.md §1）
    /// </summary>
    public static class Town01Starters
    {
        // 仮: ウキワヒナペン（海）・マッチヒナペン（火）・フタバヒナペン（苔）
        private const int SeaNo = 128;
        private const int FireNo = 130;
        private const int MossNo = 132;
        // 本家の最初の3体と同じ「やや遅い」・基礎経験値 62
        private const int StarterBaseExp = 62;

        public static readonly IReadOnlyList<StorySpecies> All = new[] { Placeholder(SeaNo), Placeholder(FireNo), Placeholder(MossNo) };

        /// <summary>覚える技の表はまだ無いので、定義表の4つを最初から覚えている形にする</summary>
        private static StorySpecies Placeholder(int no)
        {
            var source = MonsterTable.ByNo(no);
            var definition = new MonsterDefinition(source.No, source.Name, (MonsterType[])source.Types, source.BaseStats,
                (MoveDefinition[])source.Moves, source.Ability, rank: MonsterRank.Young);
            var learnset = new List<LevelMove>();
            foreach (var move in source.Moves)
            {
                learnset.Add(new LevelMove(StoryBalance.MinLevel, move));
            }
            return new StorySpecies(definition, GrowthRate.MediumSlow, StarterBaseExp, learnset.ToArray());
        }
    }
}
