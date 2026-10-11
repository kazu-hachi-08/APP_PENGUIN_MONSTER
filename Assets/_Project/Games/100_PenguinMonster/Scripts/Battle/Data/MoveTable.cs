using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveCategory;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 技の定義表（仮）。名前・数値は Phase 3 で本番の技表に置き換える
    /// </summary>
    public static class MoveTable
    {
        public static readonly MoveDefinition Tackle = new MoveDefinition("たいあたり", Normal, Physical, 50, 100);
        public static readonly MoveDefinition QuickDash = new MoveDefinition("でんこうダッシュ", Normal, Physical, 40, 100, priority: 1);
        public static readonly MoveDefinition IcicleBeak = new MoveDefinition("つららばし", Ice, Physical, 75, 100);
        public static readonly MoveDefinition BlizzardBreath = new MoveDefinition("ふぶきブレス", Ice, Special, 90, 85);
        public static readonly MoveDefinition BigWave = new MoveDefinition("おおなみ", Sea, Special, 80, 100);
        public static readonly MoveDefinition SeaGun = new MoveDefinition("しおでっぽう", Sea, Physical, 70, 100);
        public static readonly MoveDefinition FlameDive = new MoveDefinition("ひのこダイブ", Fire, Physical, 75, 100);
        public static readonly MoveDefinition HeatWave = new MoveDefinition("ねっぷう", Fire, Special, 85, 90);
        public static readonly MoveDefinition SparkBeak = new MoveDefinition("いなずまクチバシ", Thunder, Physical, 70, 100);
        public static readonly MoveDefinition ThunderDrop = new MoveDefinition("かみなりおとし", Thunder, Special, 90, 80);
        public static readonly MoveDefinition MossWhip = new MoveDefinition("こけムチ", Moss, Physical, 70, 100);
        public static readonly MoveDefinition RockDrop = new MoveDefinition("いわおとし", Rock, Physical, 75, 90);
        public static readonly MoveDefinition WindCutter = new MoveDefinition("かぜきり", Bird, Special, 65, 100);
    }
}
