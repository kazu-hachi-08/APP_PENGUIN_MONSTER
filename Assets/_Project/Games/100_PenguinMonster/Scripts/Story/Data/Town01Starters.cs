using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 博士からもらう最初の3体（海・火・苔の順）。本物の No.151〜159 はフェーズ8a で作るので、今は Town01Species の仮の子
    /// </summary>
    public static class Town01Starters
    {
        public static readonly IReadOnlyList<StorySpecies> All = new[] { Town01Species.SeaStarter, Town01Species.FireStarter, Town01Species.MossStarter };

        /// <summary>
        /// その子に強い子（ライバルが選ぶ）。海 → 火 → 苔 → 海 の三すくみなので、並びの1つ前の子が強い
        /// </summary>
        public static StorySpecies CounterOf(StorySpecies chosen)
        {
            int index = IndexOf(chosen);
            return All[(index + All.Count - 1) % All.Count];
        }

        private static int IndexOf(StorySpecies species)
        {
            for (int i = 0; i < All.Count; i++)
            {
                if (All[i] == species)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
