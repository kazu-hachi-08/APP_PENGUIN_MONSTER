using System.Collections.Generic;
using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモンの定義表（仮の6体）。Phase 3 で24体に差し替える
    /// </summary>
    public static class MonsterTable
    {
        public static readonly IReadOnlyList<MonsterDefinition> All = new[]
        {
            new MonsterDefinition(1, "ユキペン", new[] { Ice },
                new Stats(75, 80, 70, 70, 70, 80),
                new[] { IcicleBeak, BlizzardBreath, QuickDash, RockDrop },
                new MonsterLook(BodyShape.Normal, BodyColor.Sky, HeadPart.Horn, NeckPart.None)),
            new MonsterDefinition(2, "ナミペン", new[] { Sea },
                new Stats(85, 70, 80, 85, 80, 55),
                new[] { BigWave, SeaGun, BlizzardBreath, Tackle },
                new MonsterLook(BodyShape.Chubby, BodyColor.Blue, HeadPart.None, NeckPart.Scarf)),
            new MonsterDefinition(3, "ヒノペン", new[] { Fire },
                new Stats(65, 85, 60, 90, 65, 90),
                new[] { FlameDive, HeatWave, SparkBeak, QuickDash },
                new MonsterLook(BodyShape.Slim, BodyColor.Red, HeadPart.Flame, NeckPart.None)),
            new MonsterDefinition(4, "イナペン", new[] { Thunder, Bird },
                new Stats(60, 70, 60, 95, 70, 100),
                new[] { ThunderDrop, SparkBeak, WindCutter, QuickDash },
                new MonsterLook(BodyShape.Slim, BodyColor.Yellow, HeadPart.Bolt, NeckPart.Scarf)),
            new MonsterDefinition(5, "コケペン", new[] { Moss, Rock },
                new Stats(90, 85, 95, 55, 75, 40),
                new[] { MossWhip, RockDrop, SeaGun, Tackle },
                new MonsterLook(BodyShape.Chubby, BodyColor.Green, HeadPart.Leaf, NeckPart.Bell)),
            new MonsterDefinition(6, "イワペン", new[] { Rock },
                new Stats(80, 100, 100, 40, 60, 45),
                new[] { RockDrop, Tackle, IcicleBeak, FlameDive },
                new MonsterLook(BodyShape.Chubby, BodyColor.Brown, HeadPart.Crest, NeckPart.None)),
        };
    }
}
