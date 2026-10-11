namespace PenguinMonster.Battle
{
    /// <summary>
    /// バトルの調整値。仕様書 §3.2 / §4.3 の数値をここに集める
    /// </summary>
    public static class BattleBalance
    {
        public const int Level = 50;

        // Lv50・個体値31・努力値0 で計算したときの上乗せ分
        public const int HpBonus = 75;
        public const int StatBonus = 20;

        /// <summary>選出する数（3vs3）</summary>
        public const int PartySize = 3;

        /// <summary>1陣営あたりの場の数。シングルは1、ダブルスは2</summary>
        public const int SingleActiveCount = 1;

        // ダメージ式の定数（ポケモンと同じ式）
        public const int DamageDivisor = 50;
        public const int DamageBase = 2;
        public const int CriticalChanceDenominator = 24;
        public const float CriticalMultiplier = 1.5f;
        public const float StabMultiplier = 1.5f;
        public const int DamageRandomMinPercent = 85;
        public const int DamageRandomMaxPercent = 100;

        /// <summary>命中率の上限（%）。技の命中がこれ以上なら必ず当たる</summary>
        public const int AccuracyPercentMax = 100;

        /// <summary>ダメージ式の「2×Lv÷5＋2」の部分。Lv固定なので定数で持つ</summary>
        public const int LevelFactor = 2 * Level / 5 + 2;

        // 能力変化（仕様書 §4.5）。倍率は 能力 (2+n)/2、命中・回避 (3+n)/3（下がるときは分母に足す）
        public const int MaxStatStage = 6;
        public const int StatStageBase = 2;
        public const int AccuracyStageBase = 3;

        // 状態異常（仕様書 §4.4）。ダメージは最大HPを割る数
        public const int ParalysisSpeedDivisor = 2;
        public const int ParalysisImmobileDenominator = 4;
        public const int PoisonDamageDenominator = 8;
        public const int BadPoisonDamageDenominator = 16;
        public const int BurnDamageDenominator = 16;
        public const float BurnPhysicalMultiplier = 0.5f;
        /// <summary>動けないターン数（両端を含む）</summary>
        public const int SleepMinTurns = 1;
        public const int SleepMaxTurns = 3;
        public const int FreezeThawDenominator = 5;

        // こんらん。ターン数は「技を使おうとした回数」で数え、0 になった回は治って普通に動ける（1〜4回 こんらんする）
        public const int ConfusionMinTurns = 2;
        public const int ConfusionMaxTurns = 5;
        public const int ConfusionSelfHitDenominator = 3;
        public const int ConfusionSelfHitPower = 40;
    }
}
