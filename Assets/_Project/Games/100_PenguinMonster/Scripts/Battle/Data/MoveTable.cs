using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveCategory;
using static PenguinMonster.Battle.MoveEffect;
using static PenguinMonster.Battle.StatKind;
using static PenguinMonster.Battle.StatusCondition;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 技の定義表（24体ぶん・48個）。技名は原作と被らないオリジナル（仕様書 §1.2）。
    /// 変化技の威力は 0、自分にかける変化技の命中は使わない（必ず決まる）
    /// </summary>
    public static class MoveTable
    {
        private const int Sure = 100;

        // ノーマル
        public static readonly MoveDefinition Tackle = new MoveDefinition("はらばいタックル", Normal, Physical, 50, Sure);
        public static readonly MoveDefinition BellySlide = new MoveDefinition("すべりこみ", Normal, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition PenguinPress = new MoveDefinition("ペンギンプレス", Normal, Physical, 85, Sure,
            effects: new[] { Ailment(Paralysis, 30) });
        public static readonly MoveDefinition Shout = new MoveDefinition("さけびごえ", Normal, Special, 80, Sure,
            effects: new[] { Confusion(10) });
        public static readonly MoveDefinition FlapDance = new MoveDefinition("はばたきダンス", Normal, Status, 0, Sure,
            effects: new[] { SelfStat(Attack, 2) });
        public static readonly MoveDefinition CutePose = new MoveDefinition("かわいいポーズ", Normal, Status, 0, Sure,
            effects: new[] { FoeStat(Attack, -2) });
        public static readonly MoveDefinition Fluff = new MoveDefinition("ふくらむ", Normal, Status, 0, Sure,
            effects: new[] { SelfStat(Defense, 2) });

        // こおり
        public static readonly MoveDefinition IcicleBeak = new MoveDefinition("つららばし", Ice, Physical, 75, Sure,
            effects: new[] { Ailment(Freeze, 10) });
        public static readonly MoveDefinition BlizzardBreath = new MoveDefinition("ふぶきブレス", Ice, Special, 90, 85,
            effects: new[] { Ailment(Freeze, 10) });
        public static readonly MoveDefinition SnowPellet = new MoveDefinition("ゆきつぶて", Ice, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition IcebergDrop = new MoveDefinition("ひょうざんおとし", Ice, Physical, 100, 80);
        public static readonly MoveDefinition ColdBreath = new MoveDefinition("つめたいいき", Ice, Special, 55, 95,
            effects: new[] { FoeStat(Speed, -1) });

        // うみ
        public static readonly MoveDefinition BigWave = new MoveDefinition("おおなみ", Sea, Special, 80, Sure);
        public static readonly MoveDefinition SeaGun = new MoveDefinition("しおでっぽう", Sea, Physical, 70, Sure);
        public static readonly MoveDefinition Whirlpool = new MoveDefinition("うずしおアタック", Sea, Physical, 90, 90);
        public static readonly MoveDefinition BubbleSpit = new MoveDefinition("あぶくはき", Sea, Special, 40, Sure,
            effects: new[] { FoeStat(Speed, -1, 30) });
        public static readonly MoveDefinition Splash = new MoveDefinition("みずかけ", Sea, Status, 0, Sure,
            effects: new[] { FoeStat(Accuracy, -1) });

        // ひ
        public static readonly MoveDefinition FlameDive = new MoveDefinition("ひのこダイブ", Fire, Physical, 75, Sure,
            effects: new[] { Ailment(Burn, 10) });
        public static readonly MoveDefinition CampfireBreath = new MoveDefinition("たきびブレス", Fire, Special, 85, 90,
            effects: new[] { Ailment(Burn, 10) });
        public static readonly MoveDefinition Eruption = new MoveDefinition("ふんかブラスト", Fire, Special, 110, 80,
            effects: new[] { Ailment(Burn, 10) });
        public static readonly MoveDefinition WispFlame = new MoveDefinition("ゆらゆらび", Fire, Status, 0, 85,
            effects: new[] { Ailment(Burn) });
        public static readonly MoveDefinition FlareUp = new MoveDefinition("もえあがる", Fire, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 2) });

        // かみなり
        public static readonly MoveDefinition SparkBeak = new MoveDefinition("いなずまクチバシ", Thunder, Physical, 70, Sure,
            effects: new[] { Ailment(Paralysis, 10) });
        public static readonly MoveDefinition SparkWing = new MoveDefinition("ビリビリはね", Thunder, Special, 70, Sure,
            effects: new[] { Ailment(Paralysis, 10) });
        public static readonly MoveDefinition StormDrop = new MoveDefinition("らいうんおとし", Thunder, Special, 110, 70,
            effects: new[] { Ailment(Paralysis, 30) });
        public static readonly MoveDefinition NumbWave = new MoveDefinition("しびれのはどう", Thunder, Status, 0, 90,
            effects: new[] { Ailment(Paralysis) });

        // こけ
        public static readonly MoveDefinition MossWhip = new MoveDefinition("こけムチ", Moss, Physical, 70, Sure);
        public static readonly MoveDefinition SlimySeaweed = new MoveDefinition("ぬるぬるかいそう", Moss, Special, 75, Sure,
            effects: new[] { Ailment(Poison, 30) });
        public static readonly MoveDefinition MossCannon = new MoveDefinition("こけだまキャノン", Moss, Special, 90, Sure);
        public static readonly MoveDefinition SleepMoss = new MoveDefinition("ねむりごけ", Moss, Status, 0, 75,
            effects: new[] { Ailment(Sleep) });
        public static readonly MoveDefinition PoisonMoss = new MoveDefinition("どくごけ", Moss, Status, 0, 90,
            effects: new[] { Ailment(Poison) });
        public static readonly MoveDefinition ToxicMoss = new MoveDefinition("もうどくごけ", Moss, Status, 0, 85,
            effects: new[] { Ailment(BadPoison) });

        // いわ
        public static readonly MoveDefinition Boulder = new MoveDefinition("ごろごろおとし", Rock, Physical, 80, 90);
        public static readonly MoveDefinition CliffBreak = new MoveDefinition("がけくずし", Rock, Physical, 100, 80);
        public static readonly MoveDefinition RockSkin = new MoveDefinition("いわはだ", Rock, Status, 0, Sure,
            effects: new[] { SelfStat(Defense, 2) });

        // とり
        public static readonly MoveDefinition WindCutter = new MoveDefinition("かぜきり", Bird, Special, 75, Sure);
        public static readonly MoveDefinition FlapAttack = new MoveDefinition("はばたきアタック", Bird, Physical, 80, Sure);
        public static readonly MoveDefinition DiveBeak = new MoveDefinition("きゅうこうかダイブ", Bird, Physical, 100, 90);
        public static readonly MoveDefinition Whirlwind = new MoveDefinition("ぐるぐるかぜ", Bird, Special, 60, Sure,
            effects: new[] { Confusion(30) });
        public static readonly MoveDefinition SkyDream = new MoveDefinition("そらへのあこがれ", Bird, Status, 0, Sure,
            effects: new[] { SelfStat(Speed, 2) });

        // やみ
        public static readonly MoveDefinition OrcaBite = new MoveDefinition("シャチがみ", Dark, Physical, 80, Sure,
            effects: new[] { FoeStat(Defense, -1, 20) });
        public static readonly MoveDefinition PolarNight = new MoveDefinition("ながいよる", Dark, Special, 80, Sure,
            effects: new[] { FoeStat(Accuracy, -1, 20) });
        public static readonly MoveDefinition Sneak = new MoveDefinition("しのびより", Dark, Physical, 40, Sure, priority: 1);
        public static readonly MoveDefinition DarkWhisper = new MoveDefinition("やみのささやき", Dark, Status, 0, 90,
            effects: new[] { Confusion() });
        public static readonly MoveDefinition SharpGlare = new MoveDefinition("するどいめつき", Dark, Status, 0, Sure,
            effects: new[] { FoeStat(Defense, -2) });

        // オーロラ
        public static readonly MoveDefinition AuroraWave = new MoveDefinition("オーロラウェーブ", Aurora, Special, 80, Sure,
            effects: new[] { Confusion(10) });
        public static readonly MoveDefinition SparkleLaser = new MoveDefinition("きらめきレーザー", Aurora, Special, 100, 85);
        public static readonly MoveDefinition QuietPrayer = new MoveDefinition("しずかないのり", Aurora, Status, 0, Sure,
            effects: new[] { SelfStat(SpAttack, 1), SelfStat(SpDefense, 1) });
    }
}
