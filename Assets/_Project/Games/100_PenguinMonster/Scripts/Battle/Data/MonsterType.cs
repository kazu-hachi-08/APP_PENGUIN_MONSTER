namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモン・技のタイプ。
    /// 並び順は TypeChart の表の行・列と一致させること
    /// </summary>
    public enum MonsterType
    {
        Normal,
        Ice,
        Sea,
        Fire,
        Thunder,
        Moss,
        Rock,
        Bird,
        Dark,
        Aurora,
    }

    public static class MonsterTypeNames
    {
        private static readonly string[] Names =
        {
            "ノーマル", "氷", "海", "火", "雷", "苔", "岩", "鳥", "闇", "オーロラ",
        };

        public static string ToJapanese(this MonsterType type) => Names[(int)type];
    }
}
