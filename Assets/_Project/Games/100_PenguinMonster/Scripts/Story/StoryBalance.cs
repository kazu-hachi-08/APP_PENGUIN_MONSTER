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

        // 倒したときの経験値を割る数（相手の Lv を割る）。式とトレーナー戦の扱いは Experience.Defeated を見る
        public const int ExpYieldLevelDivisor = 5;
        // 1.5 倍を整数で掛けるため分子・分母に分ける（小数の誤差で1ずれないように）
        public const int TrainerExpNumerator = 3;
        public const int TrainerExpDenominator = 2;
        /// <summary>学習装置: 向き合っていない手持ちの子は 1体分の経験値を この数で割った分をもらう（本家の第6世代と同じく半分）</summary>
        public const int ExpShareDivisor = 2;

        /// <summary>始めたときのお金（本家と同じ）</summary>
        public const int StartMoney = 3000;
        /// <summary>全滅したら お金を この数で割った分だけ残す（半分）</summary>
        public const int BlackOutMoneyDivisor = 2;

        // 逃げる（本家の第3世代の簡略版）: 自分が速ければ必ず逃げられる。遅ければ
        // 自分の素早さ × 128 ÷ 相手の素早さ ＋ 30 × 試した回数 を 256 と比べ、試すほど逃げやすくする
        public const int FleeSpeedScale = 128;
        public const int FleeAttemptBonus = 30;
        public const int FleeRoll = 256;

        public const int Percent = 100;

        /// <summary>釣り竿で1回釣ったときにかかる確率（％）。外れても A ですぐ投げ直せるので半分にする</summary>
        public const int FishBitePercent = 50;

        /// <summary>よろず屋で売ると 買う値段を この数で割った額（本家と同じく半分）</summary>
        public const int SellPriceDivisor = 2;

        // 捕獲（本家の第3〜4世代の簡略版）の定数。式は Capture.Odds を見る。
        // CaptureMaxOdds 以上なら必ず捕まえる。それより下は CaptureChecks 回の揺れの判定を引く
        public const int CaptureMaxHpWeight = 3;
        public const int CaptureHpWeight = 2;
        public const int CaptureMaxOdds = 255;
        // タマゴの倍率（百分率）。ふつうの捕獲タマゴとタイプ特化のタマゴの「合わない相手」は Percent（×1）
        public const int GreatEggCatchPercent = 150;
        public const int UltraEggCatchPercent = 200;
        /// <summary>タイプ特化のタマゴを、合うタイプの相手に投げたときの倍率</summary>
        public const int TypeEggCatchPercent = 300;
        public const int CaptureSleepBonusPercent = 200;
        public const int CaptureStatusBonusPercent = 150;
        public const int CaptureChecks = 4;
        /// <summary>捕まえたときに見せる揺れの回数（4回目の判定は揺らさずに捕まえる）</summary>
        public const int CaptureShownShakes = 3;
        public const int CaptureShakeRoll = 65536;
        public const double CaptureShakeNumerator = 1048560.0;
        public const double CaptureShakeDivisor = 16711680.0;
    }
}
