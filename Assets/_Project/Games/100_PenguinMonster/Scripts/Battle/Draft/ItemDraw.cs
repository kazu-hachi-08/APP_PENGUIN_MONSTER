using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ドラフトの後に配る持ち物。1チーム1個を候補から等確率で引き、6体の中の1体に持たせる
    /// </summary>
    public static class ItemDraw
    {
        /// <summary>配る候補。HeldItem の全部から BattleBalance.UndrawableItems を除いたもの</summary>
        public static readonly IReadOnlyList<HeldItem> Candidates = BuildCandidates();

        public static HeldItem Draw(Random random) => Candidates[random.Next(Candidates.Count)];

        /// <summary>holderIndex の1体だけ持ち物を付けた6体。元のリストと定義表は変えない</summary>
        public static List<MonsterDefinition> Give(IReadOnlyList<MonsterDefinition> six, int holderIndex, HeldItem item)
        {
            var result = new List<MonsterDefinition>(six);
            result[holderIndex] = six[holderIndex].WithItem(item);
            return result;
        }

        private static List<HeldItem> BuildCandidates()
        {
            var candidates = new List<HeldItem>();
            foreach (HeldItem item in Enum.GetValues(typeof(HeldItem)))
            {
                if (Array.IndexOf(BattleBalance.UndrawableItems, item) < 0)
                {
                    candidates.Add(item);
                }
            }
            return candidates;
        }
    }
}
