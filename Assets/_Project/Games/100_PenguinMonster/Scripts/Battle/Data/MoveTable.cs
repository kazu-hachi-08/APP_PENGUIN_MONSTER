using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveCategory;
using static PenguinMonster.Battle.MoveEffect;
using static PenguinMonster.Battle.StatKind;
using static PenguinMonster.Battle.StatusCondition;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 技の定義表（150体ぶん）。技名は原作と被らないオリジナル（仕様書 §1.2）。
    /// 変化技の威力は 0、自分にかける変化技の命中は使わない（必ず決まる）。
    /// hitsAllFoes はダブルスで相手2体に当たる技（波・吹雪・噴火・声・風・落石など、広がるイメージのもの）。
    /// 反動・吸収・連続・回復・天候の技は Phase 11 で足した（威力は原作の同じ役割の技に合わせた）。
    /// 段階4（76〜150）では、タイプごとに足りなかった役割（岩とノーマルの特殊・オーロラの物理・先制・積み技など）を埋めた
    /// </summary>
    public static class MoveTable
    {
        private const int Sure = 100;
        // 2〜5回当たる技の回数
        private const int MultiMin = 2;
        private const int MultiMax = 5;
        private const int Twice = 2;
        // 守るは先制技より先に、手助けは守るよりさらに先に出る（原作と同じ並び。味方の技より先に掛かる必要があるため）
        private const int ProtectPriority = 4;
        private const int HelpPriority = 5;

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
        public static readonly MoveDefinition RecklessTackle = new MoveDefinition("全力体当たり", Normal, Physical, 120, Sure,
            recoils: true);
        public static readonly MoveDefinition FlipperSlap = new MoveDefinition("ヒレビンタ", Normal, Physical, 25, Sure,
            minHits: MultiMin, maxHits: MultiMax);
        public static readonly MoveDefinition Preen = new MoveDefinition("羽づくろい", Normal, Status, 0, Sure,
            effects: new[] { Heal() });
        // 守りを捨てて攻めと素早さを上げる。タマゴペンのような「殻を脱いで化ける」子の技
        public static readonly MoveDefinition ShellShed = new MoveDefinition("殻ぬぎ", Normal, Status, 0, Sure,
            effects: new[] { SelfStat(Attack, 2), SelfStat(SpAttack, 2), SelfStat(Speed, 2), SelfStat(Defense, -1), SelfStat(SpDefense, -1) });
        public static readonly MoveDefinition BraceUp = new MoveDefinition("我慢の構え", Normal, Status, 0, Sure,
            effects: new[] { SelfStat(Attack, 1), SelfStat(Defense, 1), SelfStat(Speed, -1) });
        // 伝説 ハジマリペン の専用技。自分の能力が下がるが、ひねくれ者なら上がる
        public static readonly MoveDefinition FirstRoar = new MoveDefinition("始まりの咆哮", Normal, Special, 120, 90,
            effects: new[] { SelfStat(SpAttack, -1), SelfStat(SpDefense, -1) });
        // 戦況を変える技（守る・手助け）。ダブルスで読み合いと連携を生むために足した
        public static readonly MoveDefinition FeatherWrap = new MoveDefinition("羽でくるまる", Normal, Status, 0, Sure,
            priority: ProtectPriority, effects: new[] { Protect() });
        public static readonly MoveDefinition Cheer = new MoveDefinition("声援", Normal, Status, 0, Sure,
            priority: HelpPriority, effects: new[] { HelpAlly() });

        // こおり
        public static readonly MoveDefinition IcicleBeak = new MoveDefinition("氷柱クチバシ", Ice, Physical, 75, Sure,
            effects: new[] { Ailment(Freeze, 10) });
        public static readonly MoveDefinition BlizzardBreath = new MoveDefinition("吹雪ブレス", Ice, Special, 90, 85,
            effects: new[] { Ailment(Freeze, 10) }, hitsAllFoes: true);
        public static readonly MoveDefinition SnowPellet = new MoveDefinition("雪つぶて", Ice, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition IcebergDrop = new MoveDefinition("氷山落とし", Ice, Physical, 100, 80);
        public static readonly MoveDefinition ColdBreath = new MoveDefinition("冷たい息", Ice, Special, 55, 95,
            effects: new[] { FoeStat(Speed, -1) }, hitsAllFoes: true);
        public static readonly MoveDefinition IcicleBarrage = new MoveDefinition("つらら連射", Ice, Physical, 25, Sure,
            minHits: MultiMin, maxHits: MultiMax);
        public static readonly MoveDefinition SnowCall = new MoveDefinition("雪乞い", Ice, Status, 0, Sure,
            effects: new[] { SetWeather(Weather.Snow) });
        public static readonly MoveDefinition IceStep = new MoveDefinition("氷上ステップ", Ice, Status, 0, Sure,
            effects: new[] { SelfStat(Attack, 1), SelfStat(Speed, 1) });
        public static readonly MoveDefinition FrostKiss = new MoveDefinition("凍えるキッス", Ice, Special, 75, Sure, drains: true);
        public static readonly MoveDefinition Avalanche = new MoveDefinition("雪崩落とし", Ice, Physical, 120, Sure, recoils: true);
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
        public static readonly MoveDefinition RainDance = new MoveDefinition("雨乞い踊り", Sea, Status, 0, Sure,
            effects: new[] { SetWeather(Weather.Rain) });
        public static readonly MoveDefinition DiveJet = new MoveDefinition("飛び込みジェット", Sea, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition SprayBurst = new MoveDefinition("しぶき連射", Sea, Physical, 25, Sure,
            minHits: MultiMin, maxHits: MultiMax);
        // 伝説 ワダツミペン の専用技
        public static readonly MoveDefinition SeaGodVortex = new MoveDefinition("海神の大渦", Sea, Special, 110, 85,
            hitsAllFoes: true);

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
        public static readonly MoveDefinition FlameRush = new MoveDefinition("炎の突進", Fire, Physical, 120, Sure,
            effects: new[] { Ailment(Burn, 10) }, recoils: true);
        public static readonly MoveDefinition Sunbathe = new MoveDefinition("日向ぼっこ", Fire, Status, 0, Sure,
            effects: new[] { SetWeather(Weather.Sun) });
        // 撃つと特攻ががくっと下がる大技。ひねくれ者が使うと逆に上がっていく
        public static readonly MoveDefinition BurnOut = new MoveDefinition("燃え尽きブラスト", Fire, Special, 130, 90,
            effects: new[] { SelfStat(SpAttack, -2) });
        // 伝説 アマテラスペン の専用技
        public static readonly MoveDefinition SunWheelFlame = new MoveDefinition("日輪の炎", Fire, Physical, 120, 90,
            effects: new[] { Ailment(Burn, 20) });

        // かみなり
        public static readonly MoveDefinition SparkBeak = new MoveDefinition("稲妻クチバシ", Thunder, Physical, 70, Sure,
            effects: new[] { Ailment(Paralysis, 10) });
        public static readonly MoveDefinition SparkWing = new MoveDefinition("ビリビリ羽", Thunder, Special, 70, Sure,
            effects: new[] { Ailment(Paralysis, 10) });
        public static readonly MoveDefinition StormDrop = new MoveDefinition("雷雲落とし", Thunder, Special, 110, 70,
            effects: new[] { Ailment(Paralysis, 30) });
        public static readonly MoveDefinition NumbWave = new MoveDefinition("痺れの波動", Thunder, Status, 0, 90,
            effects: new[] { Ailment(Paralysis) });
        public static readonly MoveDefinition VoltRush = new MoveDefinition("雷撃突進", Thunder, Physical, 120, Sure,
            effects: new[] { Ailment(Paralysis, 10) }, recoils: true);
        public static readonly MoveDefinition ZapJab = new MoveDefinition("電光ジャブ", Thunder, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition Discharge = new MoveDefinition("放電スパーク", Thunder, Special, 80, Sure,
            effects: new[] { Ailment(Paralysis, 30) }, hitsAllFoes: true);
        public static readonly MoveDefinition ChargeDance = new MoveDefinition("充電ダンス", Thunder, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 1), SelfStat(Speed, 1) });
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
        public static readonly MoveDefinition MossDrain = new MoveDefinition("苔吸い", Moss, Special, 75, Sure, drains: true);
        public static readonly MoveDefinition MossBed = new MoveDefinition("苔のベッド", Moss, Status, 0, Sure,
            effects: new[] { Heal() });
        public static readonly MoveDefinition MossArmor = new MoveDefinition("苔の鎧", Moss, Status, 0, Sure,
            effects: new[] { SelfStat(Defense, 1), SelfStat(SpDefense, 1) });
        public static readonly MoveDefinition SeedBarrage = new MoveDefinition("苔玉連射", Moss, Physical, 25, Sure,
            minHits: MultiMin, maxHits: MultiMax);
        public static readonly MoveDefinition SporeCloud = new MoveDefinition("胞子ばらまき", Moss, Special, 65, Sure,
            effects: new[] { Ailment(Poison, 30) }, hitsAllFoes: true);
        // 伝説 セカイジュペン の専用技
        public static readonly MoveDefinition WorldTreeBreath = new MoveDefinition("世界樹の息吹", Moss, Special, 100, Sure, drains: true);

        // いわ
        public static readonly MoveDefinition Boulder = new MoveDefinition("ごろごろ落とし", Rock, Physical, 80, 90, hitsAllFoes: true);
        public static readonly MoveDefinition CliffBreak = new MoveDefinition("崖崩し", Rock, Physical, 100, 80);
        public static readonly MoveDefinition RockSkin = new MoveDefinition("岩肌", Rock, Status, 0, Sure,
            effects: new[] { SelfStat(Defense, 2) });
        public static readonly MoveDefinition PebbleShot = new MoveDefinition("小石投げ", Rock, Physical, 25, 90,
            minHits: MultiMin, maxHits: MultiMax);
        public static readonly MoveDefinition RockHeadbutt = new MoveDefinition("岩石頭突き", Rock, Physical, 130, 80, recoils: true);
        public static readonly MoveDefinition GemBeam = new MoveDefinition("原石ビーム", Rock, Special, 85, Sure);
        public static readonly MoveDefinition SandSmoke = new MoveDefinition("砂けむり", Rock, Special, 60, Sure,
            effects: new[] { FoeStat(Accuracy, -1, 30) }, hitsAllFoes: true);
        // 伝説 コダイオウペン の専用技
        public static readonly MoveDefinition AncientCharge = new MoveDefinition("太古の大突進", Rock, Physical, 130, 90, recoils: true);

        // とり
        public static readonly MoveDefinition WindCutter = new MoveDefinition("風切り", Bird, Special, 75, Sure);
        public static readonly MoveDefinition FlapAttack = new MoveDefinition("羽ばたきアタック", Bird, Physical, 80, Sure);
        public static readonly MoveDefinition DiveBeak = new MoveDefinition("急降下ダイブ", Bird, Physical, 100, 90);
        public static readonly MoveDefinition Whirlwind = new MoveDefinition("ぐるぐる風", Bird, Special, 60, Sure,
            effects: new[] { Confusion(30) }, hitsAllFoes: true);
        public static readonly MoveDefinition SkyDream = new MoveDefinition("空への憧れ", Bird, Status, 0, Sure,
            effects: new[] { SelfStat(Speed, 2) });
        public static readonly MoveDefinition BraveDive = new MoveDefinition("捨て身ダイブ", Bird, Physical, 120, Sure, recoils: true);
        public static readonly MoveDefinition DoublePeck = new MoveDefinition("二段つつき", Bird, Physical, 40, 90,
            minHits: Twice, maxHits: Twice);
        public static readonly MoveDefinition PeckDrain = new MoveDefinition("つつき吸い", Bird, Physical, 75, Sure, drains: true);
        public static readonly MoveDefinition GaleFeather = new MoveDefinition("大嵐の羽", Bird, Special, 110, 70,
            effects: new[] { Confusion(30) });
        // 伝説 アマツバサペン の専用技。空を飛べたペンギンだけが、羽ばたくほど速くなる
        public static readonly MoveDefinition SkySoar = new MoveDefinition("天翔ける翼", Bird, Physical, 100, Sure,
            effects: new[] { SelfStat(Speed, 1) });
        // 味方全員を速くする追い風。素早さの遅い仲間と組ませる技
        public static readonly MoveDefinition NorthWindCall = new MoveDefinition("北風の号令", Bird, Status, 0, Sure,
            effects: new[] { SetTailwind() });

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
        public static readonly MoveDefinition LifeSap = new MoveDefinition("生気吸い", Dark, Special, 75, Sure, drains: true);
        public static readonly MoveDefinition DoubleBite = new MoveDefinition("二連噛み", Dark, Physical, 40, Sure,
            minHits: Twice, maxHits: Twice);
        public static readonly MoveDefinition Scheme = new MoveDefinition("腹黒い企み", Dark, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 2) });
        public static readonly MoveDefinition ShadowStitch = new MoveDefinition("影縫い", Dark, Special, 60, Sure,
            effects: new[] { FoeStat(Speed, -1) });
        // 伝説 ニッショクペン の専用技
        public static readonly MoveDefinition BlackSun = new MoveDefinition("黒い日輪", Dark, Special, 110, 90,
            effects: new[] { Ailment(Burn, 20) });
        // 伝説 キョクヤペン の専用技
        public static readonly MoveDefinition EndlessNight = new MoveDefinition("明けない夜", Dark, Special, 100, Sure,
            effects: new[] { FoeStat(SpDefense, -1, 30) });

        // オーロラ
        public static readonly MoveDefinition AuroraWave = new MoveDefinition("オーロラウェーブ", Aurora, Special, 80, Sure,
            effects: new[] { Confusion(10) }, hitsAllFoes: true);
        public static readonly MoveDefinition SparkleLaser = new MoveDefinition("煌めきレーザー", Aurora, Special, 100, 85);
        public static readonly MoveDefinition QuietPrayer = new MoveDefinition("静かな祈り", Aurora, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 1), SelfStat(SpDefense, 1) });
        public static readonly MoveDefinition AuroraHeal = new MoveDefinition("オーロラの癒し", Aurora, Status, 0, Sure,
            effects: new[] { Heal() });
        public static readonly MoveDefinition ShootingStar = new MoveDefinition("流れ星", Aurora, Physical, 25, Sure,
            minHits: MultiMin, maxHits: MultiMax);
        public static readonly MoveDefinition PrismKick = new MoveDefinition("プリズムキック", Aurora, Physical, 85, Sure);
        public static readonly MoveDefinition AuroraDance = new MoveDefinition("極光の舞", Aurora, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 1), SelfStat(SpDefense, 1), SelfStat(Speed, 1) });
    }
}
