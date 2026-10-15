namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリーの調整値。育成（レベル・経験値・技）の数値をここに集める
    /// </summary>
    public static class StoryBalance
    {
        public const int MinLevel = 1;
        public const int MaxLevel = 100;

        /// <summary>覚えていられる技の数。これ以上は1つ忘れて入れ替える</summary>
        public const int MaxMoves = 4;

        // 倒したときの経験値（本家の第5世代の簡略版）＝ 基礎値 × 相手の Lv ÷ 5。トレーナー戦は 1.5 倍
        public const int ExpYieldLevelDivisor = 5;
        // 1.5 倍を整数で掛けるため分子・分母に分ける（小数の誤差で1ずれないように）
        public const int TrainerExpNumerator = 3;
        public const int TrainerExpDenominator = 2;
    }
}
