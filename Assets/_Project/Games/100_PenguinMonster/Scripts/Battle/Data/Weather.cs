namespace PenguinMonster.Battle
{
    /// <summary>
    /// 天候。場全体に1つだけで、技か特性で始まり、数ターンで止む
    /// </summary>
    public enum Weather
    {
        None,
        /// <summary>氷タイプ以外はターン終わりに少しダメージ。氷タイプは物理で受けるときの防御が上がる</summary>
        Snow,
        /// <summary>海の技が強く、火の技が弱くなる</summary>
        Rain,
        /// <summary>火の技が強く、海の技が弱くなる</summary>
        Sun,
    }

    public static class WeatherNames
    {
        private static readonly string[] Names = { "", "吹雪", "大雨", "日差し" };

        public static string ToJapanese(this Weather weather) => Names[(int)weather];
    }
}
