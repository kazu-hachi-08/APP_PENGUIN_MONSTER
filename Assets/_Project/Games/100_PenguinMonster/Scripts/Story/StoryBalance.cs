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

        /// <summary>手持ちの数（本家と同じ）</summary>
        public const int MaxPartySize = 6;

        /// <summary>博士からもらう最初の1体のレベル（本家と同じ）</summary>
        public const int StarterLevel = 5;

        // 倒したときの経験値（本家の第5世代の簡略版）＝ 基礎値 × 相手の Lv ÷ 5。トレーナー戦は 1.5 倍
        public const int ExpYieldLevelDivisor = 5;
        // 1.5 倍を整数で掛けるため分子・分母に分ける（小数の誤差で1ずれないように）
        public const int TrainerExpNumerator = 3;
        public const int TrainerExpDenominator = 2;

        /// <summary>始めたときのお金（本家と同じ）</summary>
        public const int StartMoney = 3000;
        /// <summary>全滅したら お金を この数で割った分だけ残す（PLAN_STORY_TOWN01.md フェーズ5。半分）</summary>
        public const int BlackOutMoneyDivisor = 2;

        // 逃げる（本家の第3世代の簡略版）: 自分が速ければ必ず逃げられる。遅ければ
        // 自分の素早さ × 128 ÷ 相手の素早さ ＋ 30 × 試した回数 を 256 と比べ、試すほど逃げやすくする
        public const int FleeSpeedScale = 128;
        public const int FleeAttemptBonus = 30;
        public const int FleeRoll = 256;
    }
}
