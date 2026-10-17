using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 博士からもらう最初の3体（海・火・苔の順。コブネペン・ヒダネペン・メブキペン）
    /// </summary>
    public static class Town01Starters
    {
        public static readonly IReadOnlyList<StorySpecies> All = new[] { Town01Species.Kobune, Town01Species.Hidane, Town01Species.Mebuki };

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
