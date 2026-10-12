using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveCategory;
using static PenguinMonster.Battle.MoveEffect;
using static PenguinMonster.Battle.StatKind;
using static PenguinMonster.Battle.StatusCondition;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 技の定義表（50体ぶん）。技名は原作と被らないオリジナル（仕様書 §1.2）。
    /// 変化技の威力は 0、自分にかける変化技の命中は使わない（必ず決まる）。
    /// hitsAllFoes はダブルスで相手2体に当たる技（波・吹雪・噴火・声・風・落石など、広がるイメージのもの）
    /// </summary>
    public static class MoveTable
    {
        private const int Sure = 100;

        // ノーマル
        public static readonly MoveDefinition Tackle = new MoveDefinition("腹ばいタックル", Normal, Physical, 50, Sure);
        public static readonly MoveDefinition BellySlide = new MoveDefinition("滑り込み", Normal, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition PenguinPress = new MoveDefinition("ペンギンプレス", Normal, Physical, 85, Sure,
            effects: new[] { Ailment(Paralysis, 30) });
        public static readonly MoveDefinition Shout = new MoveDefinition("叫び声", Normal, Special, 80, Sure,
            effects: new[] { Confusion(10) }, hitsAllFoes: true);
        public static readonly MoveDefinition FlapDance = new MoveDefinition("羽ばたきダンス", Normal, Status, 0, Sure,
            effects: new[] { SelfStat(Attack, 2) });
        public static readonly MoveDefinition CutePose = new MoveDefinition("可愛いポーズ", Normal, Status, 0, Sure,
            effects: new[] { FoeStat(Attack, -2) });
        public static readonly MoveDefinition Fluff = new MoveDefinition("膨らむ", Normal, Status, 0, Sure,
            effects: new[] { SelfStat(Defense, 2) });
        public static readonly MoveDefinition FeatherRelay = new MoveDefinition("羽リレー", Normal, Status, 0, Sure,
            effects: new[] { Relay() });

        // こおり
        public static readonly MoveDefinition IcicleBeak = new MoveDefinition("氷柱クチバシ", Ice, Physical, 75, Sure,
            effects: new[] { Ailment(Freeze, 10) });
        public static readonly MoveDefinition BlizzardBreath = new MoveDefinition("吹雪ブレス", Ice, Special, 90, 85,
            effects: new[] { Ailment(Freeze, 10) }, hitsAllFoes: true);
        public static readonly MoveDefinition SnowPellet = new MoveDefinition("雪つぶて", Ice, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition IcebergDrop = new MoveDefinition("氷山落とし", Ice, Physical, 100, 80);
        public static readonly MoveDefinition ColdBreath = new MoveDefinition("冷たい息", Ice, Special, 55, 95,
            effects: new[] { FoeStat(Speed, -1) }, hitsAllFoes: true);
        // 伝説 ナンキョクペン の専用技
        public static readonly MoveDefinition GreatGlacier = new MoveDefinition("大氷河", Ice, Physical, 120, 85,
            effects: new[] { Ailment(Freeze, 10) });

        // うみ
        public static readonly MoveDefinition BigWave = new MoveDefinition("大波", Sea, Special, 80, Sure, hitsAllFoes: true);
        public static readonly MoveDefinition SeaGun = new MoveDefinition("潮鉄砲", Sea, Physical, 70, Sure);
        public static readonly MoveDefinition Whirlpool = new MoveDefinition("渦潮アタック", Sea, Physical, 90, 90);
        public static readonly MoveDefinition BubbleSpit = new MoveDefinition("泡吐き", Sea, Special, 40, Sure,
            effects: new[] { FoeStat(Speed, -1, 30) }, hitsAllFoes: true);
        public static readonly MoveDefinition Splash = new MoveDefinition("水かけ", Sea, Status, 0, Sure,
            effects: new[] { FoeStat(Accuracy, -1) });
        public static readonly MoveDefinition HotSpring = new MoveDefinition("温泉しぶき", Sea, Special, 80, Sure,
            effects: new[] { Ailment(Burn, 30) });

        // ひ
        public static readonly MoveDefinition FlameDive = new MoveDefinition("火の粉ダイブ", Fire, Physical, 75, Sure,
            effects: new[] { Ailment(Burn, 10) });
        public static readonly MoveDefinition CampfireBreath = new MoveDefinition("焚き火ブレス", Fire, Special, 85, 90,
            effects: new[] { Ailment(Burn, 10) });
        public static readonly MoveDefinition Eruption = new MoveDefinition("噴火ブラスト", Fire, Special, 110, 80,
            effects: new[] { Ailment(Burn, 10) }, hitsAllFoes: true);
        public static readonly MoveDefinition WispFlame = new MoveDefinition("ゆらゆら火", Fire, Status, 0, 85,
            effects: new[] { Ailment(Burn) });
        public static readonly MoveDefinition FlareUp = new MoveDefinition("燃え上がる", Fire, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 2) });

        // かみなり
        public static readonly MoveDefinition SparkBeak = new MoveDefinition("稲妻クチバシ", Thunder, Physical, 70, Sure,
            effects: new[] { Ailment(Paralysis, 10) });
        public static readonly MoveDefinition SparkWing = new MoveDefinition("ビリビリ羽", Thunder, Special, 70, Sure,
            effects: new[] { Ailment(Paralysis, 10) });
        public static readonly MoveDefinition StormDrop = new MoveDefinition("雷雲落とし", Thunder, Special, 110, 70,
            effects: new[] { Ailment(Paralysis, 30) });
        public static readonly MoveDefinition NumbWave = new MoveDefinition("痺れの波動", Thunder, Status, 0, 90,
            effects: new[] { Ailment(Paralysis) });
        // 伝説 ライテイペン の専用技
        public static readonly MoveDefinition HeavenlyThunder = new MoveDefinition("天の雷", Thunder, Special, 110, 90,
            effects: new[] { Ailment(Paralysis, 20) });

        // こけ
        public static readonly MoveDefinition MossWhip = new MoveDefinition("苔ムチ", Moss, Physical, 70, Sure);
        public static readonly MoveDefinition SlimySeaweed = new MoveDefinition("ぬるぬる海藻", Moss, Special, 75, Sure,
            effects: new[] { Ailment(Poison, 30) });
        public static readonly MoveDefinition MossCannon = new MoveDefinition("苔玉キャノン", Moss, Special, 90, Sure);
        public static readonly MoveDefinition SleepMoss = new MoveDefinition("眠り苔", Moss, Status, 0, 75,
            effects: new[] { Ailment(Sleep) });
        public static readonly MoveDefinition PoisonMoss = new MoveDefinition("毒苔", Moss, Status, 0, 90,
            effects: new[] { Ailment(Poison) });
        public static readonly MoveDefinition ToxicMoss = new MoveDefinition("猛毒苔", Moss, Status, 0, 85,
            effects: new[] { Ailment(BadPoison) });

        // いわ
        public static readonly MoveDefinition Boulder = new MoveDefinition("ごろごろ落とし", Rock, Physical, 80, 90, hitsAllFoes: true);
        public static readonly MoveDefinition CliffBreak = new MoveDefinition("崖崩し", Rock, Physical, 100, 80);
        public static readonly MoveDefinition RockSkin = new MoveDefinition("岩肌", Rock, Status, 0, Sure,
            effects: new[] { SelfStat(Defense, 2) });

        // とり
        public static readonly MoveDefinition WindCutter = new MoveDefinition("風切り", Bird, Special, 75, Sure);
        public static readonly MoveDefinition FlapAttack = new MoveDefinition("羽ばたきアタック", Bird, Physical, 80, Sure);
        public static readonly MoveDefinition DiveBeak = new MoveDefinition("急降下ダイブ", Bird, Physical, 100, 90);
        public static readonly MoveDefinition Whirlwind = new MoveDefinition("ぐるぐる風", Bird, Special, 60, Sure,
            effects: new[] { Confusion(30) }, hitsAllFoes: true);
        public static readonly MoveDefinition SkyDream = new MoveDefinition("空への憧れ", Bird, Status, 0, Sure,
            effects: new[] { SelfStat(Speed, 2) });

        // やみ
        public static readonly MoveDefinition OrcaBite = new MoveDefinition("シャチ噛み", Dark, Physical, 80, Sure,
            effects: new[] { FoeStat(Defense, -1, 20) });
        public static readonly MoveDefinition PolarNight = new MoveDefinition("長い夜", Dark, Special, 80, Sure,
            effects: new[] { FoeStat(Accuracy, -1, 20) });
        public static readonly MoveDefinition Sneak = new MoveDefinition("忍び寄り", Dark, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition DarkWhisper = new MoveDefinition("闇の囁き", Dark, Status, 0, 90,
            effects: new[] { Confusion() });
        public static readonly MoveDefinition SharpGlare = new MoveDefinition("鋭い目つき", Dark, Status, 0, Sure,
            effects: new[] { FoeStat(Defense, -2) });
        // 伝説 キョクヤペン の専用技
        public static readonly MoveDefinition EndlessNight = new MoveDefinition("明けない夜", Dark, Special, 100, Sure,
            effects: new[] { FoeStat(SpDefense, -1, 30) });

        // オーロラ
        public static readonly MoveDefinition AuroraWave = new MoveDefinition("オーロラウェーブ", Aurora, Special, 80, Sure,
            effects: new[] { Confusion(10) }, hitsAllFoes: true);
        public static readonly MoveDefinition SparkleLaser = new MoveDefinition("煌めきレーザー", Aurora, Special, 100, 85);
        public static readonly MoveDefinition QuietPrayer = new MoveDefinition("静かな祈り", Aurora, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 1), SelfStat(SpDefense, 1) });
    }
}
