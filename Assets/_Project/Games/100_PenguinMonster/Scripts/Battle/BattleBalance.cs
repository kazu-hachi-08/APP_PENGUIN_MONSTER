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

        // ドラフト（仕様書 §2.1）。3体の提示から1体取る × 6回
        public const int DraftOfferCount = 3;
        public const int DraftRounds = 6;

        // ドラフトの出やすさの重み（仕様書 §2.1 / §3.5）。伝説はたまに出る「当たり」にするため小さく
        public const int DraftWeightHina = 10;
        public const int DraftWeightNormal = 10;
        public const int DraftWeightAce = 5;
        public const int DraftWeightLegend = 2;
        /// <summary>伝説は1チームにこの数まで（伝説だけで固めた壊れたチームを防ぐ）</summary>
        public const int MaxLegendsPerTeam = 1;

        // 勝ち抜き戦（仕様書 §2.3 / §4.8）。連勝数がこの数に届いたら CPU の賢さが1段上がる
        public const int NormalCpuFromWins = 3;
        public const int StrongCpuFromWins = 6;
        /// <summary>この戦数ごとにボス（7戦目・14戦目…）</summary>
        public const int BossInterval = 7;
        /// <summary>CPU がドラフトで「種族値の合計が一番高い」のを取る確率（%）。1連勝ごとにこれだけ上がる</summary>
        public const int CpuStrongPickPercentPerWin = 10;
        public const int PercentMax = 100;

        // オンラインの制限時間（秒。仕様書 §2.1 / §2.2 / §4.7）。過ぎたら自動で決める
        public const int OnlineDraftSeconds = 15;
        public const int OnlineSelectSeconds = 60;
        public const int OnlineTurnSeconds = 30;

        /// <summary>1陣営あたりの場の数。シングルは1、ダブルスは2</summary>
        public const int SingleActiveCount = 1;
        public const int DoublesActiveCount = 2;

        // ダブルス（仕様書 §4.9）。場に2体ずつなので、控えが残るよう選出は1体多くする
        public const int DoublesPartySize = 4;
        /// <summary>相手全員に当たる技が、2体以上に当たったときのダメージの倍率</summary>
        public const float SpreadMultiplier = 0.75f;

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

        // 特性（仕様書 §4.6）
        public const int IntimidateStages = -1;
        public const int TailwindStages = 1;
        public const int MomentumStages = 1;
        /// <summary>せとぎわ: HPが最大HPのこの分の1以下で発動</summary>
        public const int LastStandHpDenominator = 3;
        public const float LastStandMultiplier = 1.5f;
        public const float ThickFatMultiplier = 0.5f;
        /// <summary>ビリビリボディ・どくぬめり が起きる確率（%）</summary>
        public const int ContactAbilityPercent = 30;
        public const int SpikySkinDamageDenominator = 8;
        public const float HugePowerMultiplier = 2f;
        /// <summary>仇討ち: 選出の味方が1体瀕死になるごとに掛ける倍率</summary>
        public const float AvengerMultiplierPerFainted = 1.3f;

        // 持ち物（仕様書 §4.6）。回復・反動は最大HPを割る数
        public const int StarFishHealDenominator = 16;
        public const int SquidSnackHealDenominator = 4;
        /// <summary>おやつのイカ: HPが最大HPのこの分の1以下で使う</summary>
        public const int SquidSnackHpDenominator = 2;
        public const float FeatherMultiplier = 1.2f;
        public const float LifeIcicleMultiplier = 1.3f;
        public const int LifeIcicleRecoilDenominator = 10;
        public const float SharpGlassesMultiplier = 1.2f;
        public const float LightFeatherSpeedMultiplier = 1.2f;
        public const float SnowCoatMultiplier = 0.75f;
        public const float ChickDownDefenseMultiplier = 1.5f;
    }
}
