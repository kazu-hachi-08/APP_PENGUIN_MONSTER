namespace PenguinMonster.Battle
{
    /// <summary>
    /// バトルの調整値。数値をここに集める
    /// </summary>
    public static class BattleBalance
    {
        /// <summary>ドラフト・勝ち抜き戦・オンラインのレベル。レベルを渡さないときの既定</summary>
        public const int Level = 50;

        // 能力値の式（本家の式で個体値31・努力値0）。Lv50 なら HP＝種族値＋75、ほか＝種族値＋20 になる
        public const int IndividualValue = 31;
        public const int StatLevelDivisor = 100;
        public const int HpFlatBonus = 10;
        public const int StatFlatBonus = 5;

        /// <summary>選出する数（3vs3）</summary>
        public const int PartySize = 3;

        // ドラフト。3体の提示から1体取る × 6回
        public const int DraftOfferCount = 3;
        public const int DraftRounds = 6;

        // ドラフトの出やすさの重み。伝説はたまに出る「当たり」にするため小さく
        public const int DraftWeightHina = 10;
        public const int DraftWeightNormal = 10;
        public const int DraftWeightAce = 5;
        public const int DraftWeightLegend = 2;
        /// <summary>伝説は1チームにこの数まで（伝説だけで固めた壊れたチームを防ぐ）</summary>
        public const int MaxLegendsPerTeam = 1;

        // 勝ち抜き戦。連勝数がこの数に届いたら CPU の賢さが1段上がる
        public const int NormalCpuFromWins = 3;
        public const int StrongCpuFromWins = 6;
        /// <summary>この戦数ごとにボス（7戦目・14戦目…）</summary>
        public const int BossInterval = 7;
        /// <summary>CPU がドラフトで「種族値の合計が一番高い」のを取る確率（%）。1連勝ごとにこれだけ上がる</summary>
        public const int CpuStrongPickPercentPerWin = 10;
        /// <summary>CPU（ふつう・つよい）は HP が最大HPのこの分の1以下なら回復の技を使う</summary>
        public const int CpuHealHpDenominator = 2;
        public const int PercentMax = 100;

        // オンラインの制限時間（秒）。過ぎたら自動で決める
        public const int OnlineDraftSeconds = 15;
        public const int OnlineSelectSeconds = 60;
        public const int OnlineTurnSeconds = 30;

        /// <summary>
        /// バトル中に相手の返事をこれ以上待ったら、通信が止まったとみなす。
        /// 普通は制限時間（ダブルスで2体×30秒）＋再生の差で収まるので、それより十分長くする
        /// </summary>
        public const int OnlineReplyTimeoutSeconds = 180;

        /// <summary>1陣営あたりの場の数。シングルは1、ダブルスは2</summary>
        public const int SingleActiveCount = 1;
        public const int DoublesActiveCount = 2;

        // ダブルス。場に2体ずつなので、控えが残るよう選出は1体多くする
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

        /// <summary>ダメージ式の「2×Lv÷5＋2」の部分。攻撃する側のレベルで出す</summary>
        public static int LevelFactor(int level) => 2 * level / 5 + 2;

        // 能力変化。倍率は 能力 (2+n)/2、命中・回避 (3+n)/3（下がるときは分母に足す）
        public const int MaxStatStage = 6;
        public const int StatStageBase = 2;
        public const int AccuracyStageBase = 3;

        // 状態異常。ダメージは最大HPを割る数
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

        // 特性
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

        // 持ち物。回復・反動は最大HPを割る数
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
        /// <summary>ヒナの綿毛: ランク「ヒナ」全員の防御・特防に掛ける（持ち物ではなく種族の補正）</summary>
        public const float ChickDownDefenseMultiplier = 1.5f;
        /// <summary>
        /// 配る候補から外す持ち物。None は「持ち物なし」、ヒナの綿毛はヒナの補正に変えたので配らない
        /// </summary>
        public static readonly HeldItem[] UndrawableItems = { HeldItem.None, HeldItem.ChickDown };
        /// <summary>貝の首飾り: 与えたダメージのこの分の1を回復</summary>
        public const int ShellNecklaceHealDenominator = 8;

        // 技の効果。反動・吸収は与えたダメージを、回復は最大HPを割る数
        public const int RecoilDenominator = 3;
        public const int DrainDenominator = 2;
        public const int HealMoveDenominator = 2;
        /// <summary>2〜5回当たる技で、2回・3回・4回・5回になる確率（%）。原作と同じ</summary>
        public static readonly int[] MultiHitPercents = { 35, 35, 15, 15 };
        /// <summary>CPU が見込みダメージに掛ける 2〜5回の技の平均回数（上の確率から）</summary>
        public const float AverageMultiHits = 3.1f;

        // 特性
        /// <summary>ひと休み: 交代で引っ込むと最大HPのこの分の1を回復</summary>
        public const int RestHealDenominator = 3;
        public const int WeatherSpeedMultiplier = 2;

        // 特性
        /// <summary>小技名人: 威力がこれ以下の技が強くなる</summary>
        public const int TechnicianPowerMax = 60;
        public const float TechnicianMultiplier = 1.5f;
        /// <summary>向こう見ず: 反動のある技の倍率</summary>
        public const float RecklessMultiplier = 1.2f;
        /// <summary>まっさら: HP 満タンで受けるダメージの倍率</summary>
        public const float MultiscaleMultiplier = 0.5f;
        public const int MoodyRaiseStages = 2;
        public const int MoodyLowerStages = -1;
        /// <summary>ちゃっかり: 変化技の優先度に足す数</summary>
        public const int PranksterPriorityBonus = 1;

        // 天候
        public const int WeatherTurns = 5;
        /// <summary>お天気石を持った子が始めた天候のターン数</summary>
        public const int WeatherStoneTurns = 8;
        /// <summary>大雨の海技・日差しの火技</summary>
        public const float WeatherBoostMultiplier = 1.5f;
        /// <summary>大雨の火技・日差しの海技</summary>
        public const float WeatherWeakenMultiplier = 0.5f;
        public const int SnowDamageDenominator = 16;
        /// <summary>吹雪のとき、氷タイプが物理技を受ける防御の倍率</summary>
        public const float SnowIceDefenseMultiplier = 1.5f;

        // 戦況を変える技
        /// <summary>追い風が続くターン（始めたターンも1と数える）</summary>
        public const int TailwindTurns = 4;
        public const int TailwindSpeedMultiplier = 2;
        /// <summary>手助けを受けた味方の技のダメージ倍率</summary>
        public const float HelpingHandMultiplier = 1.5f;
    }
}
