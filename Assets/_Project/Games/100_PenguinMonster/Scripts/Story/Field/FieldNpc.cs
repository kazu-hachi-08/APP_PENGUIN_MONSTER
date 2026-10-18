using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>人の見た目。View が主人公の絵の色を替えて描く</summary>
    public enum NpcLook
    {
        Mom,
        Professor,
        Assistant,
        Villager,
        Rival,
        /// <summary>道路のトレーナー</summary>
        Youngster,
        /// <summary>ぬくもり小屋の受付</summary>
        HutKeeper,
        /// <summary>よろず屋の店員</summary>
        Clerk,
        /// <summary>道場の門下生</summary>
        Disciple,
        /// <summary>道場主</summary>
        GymLeader,
        /// <summary>道路の落とし物（人ではなく道具の袋。前から A で拾う）</summary>
        Item,
        /// <summary>台本で戦う野生のペンモン（FieldNpc.Monster の絵で描く）</summary>
        Wild,
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
        /// <summary>台本で戦うトレーナー。目が合ったら近づいてくる（SightRange が 0 なら来ない）。戦わない置き場所なら null</summary>
        public TrainerDefinition Trainer { get; }
        /// <summary>そのマップに入ったとき、話しかけなくても向こうから話してくる（ユキオコシ町の入口で待つ母）</summary>
        public bool GreetsOnArrival { get; }

        private NpcPlacement(FieldSpot spot, EventStep[] script, string requiredFlag, string missingFlag, bool greetsOnArrival = false)
        {
            Spot = spot;
            Script = script;
            _requiredFlag = requiredFlag;
            _missingFlag = missingFlag;
            GreetsOnArrival = greetsOnArrival;
            Trainer = FindTrainer(script);
        }

        private static TrainerDefinition FindTrainer(EventStep[] script)
        {
            foreach (var step in script)
            {
                if (step.Kind == EventStepKind.Battle)
                {
                    return step.Trainer;
                }
            }
            return null;
        }

        public static NpcPlacement Always(FieldSpot spot, params EventStep[] script) => new NpcPlacement(spot, script, null, null);

        /// <summary>フラグが立つまで</summary>
        public static NpcPlacement Until(string flag, FieldSpot spot, params EventStep[] script) => new NpcPlacement(spot, script, null, flag);

        /// <summary>
        /// フラグが立つまで。マップに入ると向こうから話してくる。台本でそのフラグを立てて、1回だけにする
        /// （入口の横へずれても避けられないように、目が合うのではなく入ったときに話す）
        /// </summary>
        public static NpcPlacement GreetUntil(string flag, FieldSpot spot, params EventStep[] script)
        {
            return new NpcPlacement(spot, script, null, flag, greetsOnArrival: true);
        }

        /// <summary>required が立っていて、missing がまだ立っていない間</summary>
        public static NpcPlacement Between(string required, string missing, FieldSpot spot, params EventStep[] script)
        {
            return new NpcPlacement(spot, script, required, missing);
        }

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
        /// <summary>NpcLook.Wild のときに描くペンモン。人なら null</summary>
        public StorySpecies Monster { get; }

        public FieldNpc(string id, NpcLook look, params NpcPlacement[] placements)
        {
            Id = id;
            Look = look;
            Placements = placements;
        }

        private FieldNpc(string id, StorySpecies monster, NpcPlacement[] placements) : this(id, NpcLook.Wild, placements)
        {
            Monster = monster;
        }

        /// <summary>フィールドに姿を見せている野生のペンモン（発電所に居座る子）。人の代わりにその子の絵で立つ</summary>
        public static FieldNpc WildMonster(string id, StorySpecies monster, params NpcPlacement[] placements)
        {
            return new FieldNpc(id, monster, placements);
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
