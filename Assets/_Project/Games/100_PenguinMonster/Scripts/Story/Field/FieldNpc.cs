using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>人の見た目。View が主人公の絵の色を替えて描く（区切りごとに足す）</summary>
    public enum NpcLook
    {
        Mom,
        Professor,
        Assistant,
        Villager,
    }

    /// <summary>
    /// 人が立つ場所と、話しかけたときの台本の1組。フラグの条件が合うときだけ使う。
    /// 同じ人でも、物語が進むと別の場所・別のセリフにする（例: 最初の1体をもらうまで母が村の出口をふさぐ）
    /// </summary>
    public sealed class NpcPlacement
    {
        private readonly string _requiredFlag;
        private readonly string _missingFlag;

        public FieldSpot Spot { get; }
        public IReadOnlyList<EventStep> Script { get; }

        private NpcPlacement(FieldSpot spot, EventStep[] script, string requiredFlag, string missingFlag)
        {
            Spot = spot;
            Script = script;
            _requiredFlag = requiredFlag;
            _missingFlag = missingFlag;
        }

        public static NpcPlacement Always(FieldSpot spot, params EventStep[] script) => new NpcPlacement(spot, script, null, null);

        /// <summary>フラグが立ってから</summary>
        public static NpcPlacement After(string flag, FieldSpot spot, params EventStep[] script) => new NpcPlacement(spot, script, flag, null);

        /// <summary>フラグが立つまで</summary>
        public static NpcPlacement Until(string flag, FieldSpot spot, params EventStep[] script) => new NpcPlacement(spot, script, null, flag);

        public bool Matches(StoryFlags flags)
        {
            return (_requiredFlag == null || flags.Has(_requiredFlag)) && (_missingFlag == null || !flags.Has(_missingFlag));
        }
    }

    /// <summary>フィールドの人1人。置き場所の候補を上から見て、最初に条件が合ったところに立つ</summary>
    public sealed class FieldNpc
    {
        public string Id { get; }
        public NpcLook Look { get; }
        public IReadOnlyList<NpcPlacement> Placements { get; }

        public FieldNpc(string id, NpcLook look, params NpcPlacement[] placements)
        {
            Id = id;
            Look = look;
            Placements = placements;
        }

        /// <summary>今のフラグで立つ場所。どれも合わなければ null（どこにも居ない）</summary>
        public NpcPlacement PlacementFor(StoryFlags flags)
        {
            foreach (var placement in Placements)
            {
                if (placement.Matches(flags))
                {
                    return placement;
                }
            }
            return null;
        }
    }
}
