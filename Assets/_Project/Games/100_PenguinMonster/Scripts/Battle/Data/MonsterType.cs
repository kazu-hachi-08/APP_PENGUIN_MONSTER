namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモン・技のタイプ（仕様書 §3.3）。
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
            "ノーマル", "こおり", "うみ", "ひ", "かみなり", "こけ", "いわ", "とり", "やみ", "オーロラ",
        };

        public static string ToJapanese(this MonsterType type) => Names[(int)type];
    }
}
