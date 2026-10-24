using System;

namespace PenguinMonster.Story
{
    /// <summary>A で釣った結果</summary>
    public enum FishResult
    {
        /// <summary>竿が無い・水に向いていない。釣りはしない（A は今まで通り何もしない）</summary>
        None,
        /// <summary>水の出現表が無いマップの水（「何も 釣れそうに ない」）</summary>
        NothingHere,
        Miss,
        Bite,
    }

    /// <summary>水に向いて釣る（PLAN_STORY_TOWN04.md §5）</summary>
    public sealed partial class FieldPlayer
    {
        /// <summary>向いている先が水のマスか</summary>
        public bool IsFacingWater => Map.Contains(Ahead) && Map.TileAt(Ahead) == TileKind.Water;

        /// <summary>
        /// 向いている水で釣る。かかるかどうかはマップの水の出現表の確率（StoryBalance.FishBitePercent）で決め、
        /// かかったら wild にその子を入れる
        /// </summary>
        public FishResult Fish(bool hasRod, Random random, out OwnedMonster wild)
        {
            wild = null;
            if (!hasRod || !IsFacingWater)
            {
                return FishResult.None;
            }
            if (Map.FishingEncounters == null)
            {
                return FishResult.NothingHere;
            }
            wild = Map.FishingEncounters.Roll(random);
            return wild != null ? FishResult.Bite : FishResult.Miss;
        }
    }
}
