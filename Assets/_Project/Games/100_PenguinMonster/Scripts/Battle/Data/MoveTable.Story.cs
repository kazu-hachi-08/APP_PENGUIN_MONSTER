using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveCategory;
using static PenguinMonster.Battle.MoveEffect;
using static PenguinMonster.Battle.StatKind;
using static PenguinMonster.Battle.StatusCondition;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ストーリーの序盤の技（PLAN_STORY_TOWN01.md フェーズ8a）。ドラフトの技は Lv50 で戦う前提で強いので、
    /// Lv5 前後で覚える弱い技（威力 40 前後）を各タイプ1つずつと、相手の能力を1段だけ下げる技を足す
    /// </summary>
    public static partial class MoveTable
    {
        public static readonly MoveDefinition Pound = new MoveDefinition("ヒレではたく", Normal, Physical, 40, Sure);
        public static readonly MoveDefinition Growl = new MoveDefinition("ぴいぴい鳴く", Normal, Status, 0, Sure,
            effects: new[] { FoeStat(Attack, -1) });
        public static readonly MoveDefinition TailWag = new MoveDefinition("おしりふりふり", Normal, Status, 0, Sure,
            effects: new[] { FoeStat(Defense, -1) });
        public static readonly MoveDefinition PowderSnow = new MoveDefinition("粉雪", Ice, Special, 40, Sure,
            effects: new[] { Ailment(Freeze, 10) });
        public static readonly MoveDefinition WaterGun = new MoveDefinition("水鉄砲", Sea, Special, 40, Sure);
        public static readonly MoveDefinition Ember = new MoveDefinition("火の粉", Fire, Special, 40, Sure,
            effects: new[] { Ailment(Burn, 10) });
        public static readonly MoveDefinition StaticShock = new MoveDefinition("静電気", Thunder, Special, 40, Sure,
            effects: new[] { Ailment(Paralysis, 10) });
        public static readonly MoveDefinition MossPellet = new MoveDefinition("苔つぶて", Moss, Physical, 40, Sure);
        public static readonly MoveDefinition RockThrow = new MoveDefinition("岩投げ", Rock, Physical, 50, 90);
        public static readonly MoveDefinition Peck = new MoveDefinition("ついばみ", Bird, Physical, 35, Sure);
        public static readonly MoveDefinition Pester = new MoveDefinition("ちょっかい", Dark, Physical, 40, Sure);
        public static readonly MoveDefinition Twinkle = new MoveDefinition("きらきら", Aurora, Special, 40, Sure);
    }
}
