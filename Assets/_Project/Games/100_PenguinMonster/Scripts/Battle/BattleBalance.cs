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
    }
}
