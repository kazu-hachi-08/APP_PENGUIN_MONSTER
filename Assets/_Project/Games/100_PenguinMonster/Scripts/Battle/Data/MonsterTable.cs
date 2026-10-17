using System;
using System.Collections.Generic;
using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモンの定義表（段階4の150体。仕様書 §3.5）。こおりは主役なので一番多くする（2タイプも数える）。
    /// 51〜75 は Phase 11 で足した、反動・吸収・連続・回復・天候の技や特性を軸にした子たち。
    /// 76〜150 は段階4で足した、新しい特性（小技名人・ひねくれ者など）と題材の幅（本物のペンギン・日本の物・妖怪など）で個性を出す子たち。
    /// 種族値の合計はランクごとの帯（ヒナ 360〜400 / ふつう 425〜475 / エース 520〜540 / 伝説 580〜600）にそろえ、
    /// 同じランクの中の差は技とタイプで付ける。特性は1体1つずつ。持ち物は定義に持たず、ドラフトの後に1チーム1個配る（仕様書 §4.6）
    /// </summary>
    public static partial class MonsterTable
    {
        public static readonly IReadOnlyList<MonsterDefinition> All = new[]
        {
            Monster(1, "ユキペン", new[] { Ice }, new Stats(75, 80, 70, 70, 70, 80),
                new[] { IcicleBeak, Boulder, SnowPellet, FlapDance }, Ability.LastStand),
            Monster(2, "ナミペン", new[] { Sea }, new Stats(85, 70, 80, 85, 80, 55),
                new[] { BigWave, BubbleSpit, BlizzardBreath, Splash }, Ability.ThickFat),
            Monster(3, "ヒノペン", new[] { Fire }, new Stats(65, 85, 60, 90, 65, 90),
                new[] { FlameDive, CampfireBreath, SparkBeak, FlareUp }, Ability.LastStand),
            Monster(4, "イナペン", new[] { Thunder, Bird }, new Stats(60, 70, 60, 95, 70, 100),
                new[] { SparkWing, WindCutter, NumbWave, BellySlide }, Ability.StaticBody),
            Monster(5, "コケペン", new[] { Moss, Rock }, new Stats(90, 85, 95, 55, 75, 40),
                new[] { MossWhip, Boulder, SleepMoss, Tackle }, Ability.SpikySkin),
            Monster(6, "イワペン", new[] { Rock }, new Stats(80, 100, 100, 40, 60, 45),
                new[] { Boulder, CliffBreak, FeatherWrap, IcicleBeak }, Ability.HardFeathers),
            Monster(7, "ヒョウガペン", new[] { Ice, Rock }, new Stats(95, 95, 100, 50, 70, 40),
                new[] { IcebergDrop, Boulder, RockSkin, SnowPellet }, Ability.SpikySkin),
            Monster(8, "ホシペン", new[] { Ice, Aurora }, new Stats(70, 50, 65, 95, 75, 95),
                new[] { BlizzardBreath, AuroraWave, ColdBreath, QuietPrayer }, Ability.Tailwind),
            Monster(9, "コウテイペン", new[] { Ice, MonsterType.Normal }, new Stats(100, 90, 80, 75, 80, 50),
                new[] { IcicleBeak, PenguinPress, IcebergDrop, Fluff }, Ability.Intimidate),
            Monster(10, "リュウヒョウペン", new[] { Ice, Sea }, new Stats(80, 70, 85, 85, 80, 55),
                new[] { BlizzardBreath, BigWave, ColdBreath, Splash }, Ability.HardFeathers),
            Monster(11, "ペンタ", new[] { MonsterType.Normal }, new Stats(75, 75, 75, 75, 75, 75),
                new[] { PenguinPress, OrcaBite, BellySlide, FlapDance }, Ability.Momentum),
            Monster(12, "モフペン", new[] { MonsterType.Normal }, new Stats(125, 65, 75, 60, 85, 35),
                new[] { PenguinPress, Shout, CutePose, FeatherWrap }, Ability.Rest),
            Monster(13, "ウズペン", new[] { Sea }, new Stats(80, 95, 75, 60, 70, 80),
                new[] { Whirlpool, SeaGun, Sneak, FlapDance }, Ability.Momentum),
            Monster(14, "サンゴペン", new[] { Sea, Moss }, new Stats(75, 55, 75, 90, 95, 60),
                new[] { BubbleSpit, MossCannon, PoisonMoss, BigWave }, Ability.PoisonSlime),
            Monster(15, "カザンペン", new[] { Fire, Rock }, new Stats(95, 100, 85, 60, 60, 50),
                new[] { FlameDive, CliffBreak, RockSkin, WispFlame }, Ability.Intimidate),
            Monster(16, "タキビペン", new[] { Fire }, new Stats(70, 50, 65, 110, 80, 85),
                new[] { Eruption, CampfireBreath, WispFlame, FlareUp }, Ability.LastStand),
            Monster(17, "ライペン", new[] { Thunder }, new Stats(70, 95, 65, 65, 65, 100),
                new[] { SparkBeak, BellySlide, FlapAttack, NumbWave }, Ability.StaticBody),
            Monster(18, "アラシペン", new[] { Thunder }, new Stats(90, 60, 80, 100, 85, 45),
                new[] { StormDrop, BigWave, SparkWing, NumbWave }, Ability.NightOwl),
            Monster(19, "ノリペン", new[] { Moss }, new Stats(85, 60, 80, 85, 90, 60),
                new[] { SlimySeaweed, MossCannon, ToxicMoss, FeatherWrap }, Ability.PoisonSlime),
            Monster(20, "ツバサペン", new[] { Bird }, new Stats(65, 95, 60, 60, 60, 110),
                new[] { FlapAttack, DiveBeak, NorthWindCall, BellySlide }, Ability.Tailwind),
            Monster(21, "カゼペン", new[] { Bird }, new Stats(70, 55, 65, 100, 75, 95),
                new[] { WindCutter, Whirlwind, SparkWing, SkyDream }, Ability.HardFeathers),
            Monster(22, "シャチペン", new[] { Dark }, new Stats(90, 110, 80, 50, 65, 70),
                new[] { OrcaBite, Sneak, Whirlpool, SharpGlare }, Ability.Intimidate),
            Monster(23, "ヨルペン", new[] { Dark }, new Stats(65, 60, 65, 105, 80, 90),
                new[] { PolarNight, AuroraWave, DarkWhisper, CampfireBreath }, Ability.NightOwl),
            Monster(24, "オーロペン", new[] { Aurora }, new Stats(80, 55, 75, 105, 95, 65),
                new[] { SparkleLaser, AuroraWave, QuietPrayer, FeatherWrap }, Ability.ThickFat),

            // ---- 段階2: ふつう（25〜36）。闇・オーロラ・火・苔など少なかったタイプを埋める ----
            Monster(25, "ツララペン", new[] { Ice }, new Stats(70, 55, 70, 100, 80, 85),
                new[] { BlizzardBreath, ColdBreath, AuroraWave, QuietPrayer }, Ability.LastStand),
            Monster(26, "カマクラペン", new[] { Ice, Fire }, new Stats(85, 70, 85, 85, 80, 55),
                new[] { BlizzardBreath, CampfireBreath, WispFlame, FeatherWrap }, Ability.ThickFat),
            Monster(27, "オンセンペン", new[] { Fire, Sea }, new Stats(90, 60, 80, 90, 90, 55),
                new[] { HotSpring, CampfireBreath, WispFlame, QuietPrayer }, Ability.NightOwl),
            Monster(28, "ハナビペン", new[] { Fire, Aurora }, new Stats(65, 70, 60, 105, 70, 95),
                new[] { Eruption, SparkleLaser, SparkWing, FlareUp }, Ability.LastStand),
            Monster(29, "キノコペン", new[] { Moss, Dark }, new Stats(85, 85, 80, 60, 80, 60),
                new[] { MossWhip, OrcaBite, SleepMoss, PoisonMoss }, Ability.PoisonSlime),
            Monster(30, "ツタペン", new[] { Moss, Bird }, new Stats(70, 80, 65, 75, 70, 95),
                new[] { MossWhip, DiveBeak, BellySlide, SkyDream }, Ability.Momentum),
            Monster(31, "マジシャンペン", new[] { MonsterType.Normal, Aurora }, new Stats(70, 60, 70, 100, 90, 75),
                new[] { Shout, SparkleLaser, DarkWhisper, QuietPrayer }, Ability.HardFeathers),
            Monster(32, "ボクサーペン", new[] { MonsterType.Normal }, new Stats(75, 105, 70, 45, 65, 95),
                new[] { PenguinPress, BellySlide, SparkBeak, FlapDance }, Ability.Intimidate),
            Monster(33, "シンジュペン", new[] { Sea, Aurora }, new Stats(80, 50, 85, 95, 100, 60),
                new[] { BigWave, SparkleLaser, ColdBreath, QuietPrayer }, Ability.SpikySkin),
            Monster(34, "イルカペン", new[] { Sea }, new Stats(75, 90, 70, 65, 65, 95),
                new[] { Whirlpool, SnowPellet, DiveBeak, FlapDance }, Ability.RainSurf),
            Monster(35, "コウモリペン", new[] { Dark, Bird }, new Stats(70, 85, 65, 70, 65, 105),
                new[] { OrcaBite, FlapAttack, DarkWhisper, Sneak }, Ability.NightOwl),
            Monster(36, "ホタルペン", new[] { Thunder, Moss }, new Stats(70, 55, 70, 95, 85, 80),
                new[] { SparkWing, MossCannon, SleepMoss, NumbWave }, Ability.StaticBody),

            // ---- ヒナ（37〜42）。種族値は低いが、特性・持ち物・技と編成しだいで化ける ----
            Monster(37, "ヒナペン", new[] { MonsterType.Normal }, new Stats(70, 60, 55, 40, 55, 80),
                new[] { PenguinPress, BellySlide, IcicleBeak, FlapDance }, Ability.HugePower, MonsterRank.Hina),
            Monster(38, "ユキヒナペン", new[] { Ice }, new Stats(85, 50, 75, 60, 75, 40),
                new[] { ColdBreath, ToxicMoss, Fluff, FeatherRelay }, Ability.ThickFat, MonsterRank.Hina),
            Monster(39, "ハネヒナペン", new[] { Bird }, new Stats(50, 55, 50, 50, 50, 115),
                new[] { WindCutter, SkyDream, NorthWindCall, FeatherRelay }, Ability.Tailwind, MonsterRank.Hina),
            Monster(40, "ナキムシペン", new[] { Dark }, new Stats(65, 90, 60, 50, 60, 75),
                new[] { OrcaBite, Sneak, FlameDive, IcicleBeak }, Ability.Avenger, MonsterRank.Hina),
            Monster(41, "カラヒナペン", new[] { Rock }, new Stats(85, 40, 95, 40, 95, 25),
                new[] { Boulder, ToxicMoss, RockSkin, FeatherRelay }, Ability.SpikySkin, MonsterRank.Hina),
            Monster(42, "ピリヒナペン", new[] { Thunder }, new Stats(60, 45, 60, 80, 65, 80),
                new[] { SparkWing, StormDrop, AuroraWave, NumbWave }, Ability.Avenger, MonsterRank.Hina),

            // ---- エース（43〜47）。純粋に強いが、4倍などはっきりした弱点がある ----
            Monster(43, "サメペン", new[] { Sea, Dark }, new Stats(95, 115, 85, 60, 75, 95),
                new[] { Whirlpool, OrcaBite, CliffBreak, FlapDance }, Ability.SpikySkin, MonsterRank.Ace),
            Monster(44, "ヒノトリペン", new[] { Fire, Bird }, new Stats(85, 110, 75, 95, 80, 90),
                new[] { FlameDive, DiveBeak, Eruption, FlapDance }, Ability.LastStand, MonsterRank.Ace),
            Monster(45, "ユキヒョウペン", new[] { Ice, Dark }, new Stats(80, 115, 75, 60, 75, 115),
                new[] { IcicleBeak, OrcaBite, SnowPellet, SharpGlare }, Ability.Momentum, MonsterRank.Ace),
            Monster(46, "ヒカリペン", new[] { Aurora, Moss }, new Stats(95, 60, 90, 115, 105, 70),
                new[] { SparkleLaser, MossCannon, SleepMoss, QuietPrayer }, Ability.HardFeathers, MonsterRank.Ace),
            Monster(47, "ライガンペン", new[] { Thunder, Rock }, new Stats(100, 115, 105, 55, 75, 75),
                new[] { SparkBeak, CliffBreak, IcicleBeak, RockSkin }, Ability.Intimidate, MonsterRank.Ace),

            // ---- 伝説（48〜50）。ボスの切り札で、専用の技を持つ ----
            Monster(48, "ナンキョクペン", new[] { Ice }, new Stats(110, 125, 105, 90, 90, 80),
                new[] { GreatGlacier, CliffBreak, SnowPellet, FlapDance }, Ability.SnowCloud, MonsterRank.Legend),
            Monster(49, "ライテイペン", new[] { Thunder, Bird }, new Stats(105, 80, 80, 110, 95, 110),
                new[] { HeavenlyThunder, WindCutter, AuroraWave, QuietPrayer }, Ability.HardFeathers,
                MonsterRank.Legend),
            Monster(50, "キョクヤペン", new[] { Dark, Aurora }, new Stats(105, 110, 90, 110, 90, 85),
                new[] { EndlessNight, OrcaBite, SparkleLaser, CampfireBreath }, Ability.Intimidate,
                MonsterRank.Legend),

            // ---- 段階3: ふつう（51〜67）。反動・連続・天候・回復で、同じタイプでも戦い方が違う子を作る ----
            Monster(51, "ガンコペン", new[] { Rock }, new Stats(80, 105, 95, 40, 65, 60),
                new[] { RockHeadbutt, RecklessTackle, PebbleShot, RockSkin }, Ability.RockHead),
            Monster(52, "ツツキペン", new[] { Bird }, new Stats(65, 95, 60, 45, 60, 110),
                new[] { BraveDive, DoublePeck, BellySlide, SkyDream }, Ability.RockHead),
            Monster(53, "ビンタペン", new[] { MonsterType.Normal }, new Stats(85, 90, 70, 45, 65, 85),
                new[] { FlipperSlap, IcicleBarrage, PebbleShot, FlapDance }, Ability.SkillLink),
            Monster(54, "ユキダルマペン", new[] { Ice }, new Stats(100, 55, 85, 90, 90, 45),
                new[] { BlizzardBreath, BigWave, ColdBreath, Preen }, Ability.SnowCloud),
            Monster(55, "ソリペン", new[] { Ice }, new Stats(70, 95, 65, 50, 60, 100),
                new[] { IcicleBarrage, IcicleBeak, RecklessTackle, SnowCall }, Ability.SnowSlide),
            Monster(56, "アマグモペン", new[] { Sea }, new Stats(90, 50, 80, 95, 90, 60),
                new[] { BigWave, StormDrop, BubbleSpit, Preen }, Ability.RainCall),
            Monster(57, "ナミノリペン", new[] { Ice, Sea }, new Stats(75, 100, 70, 55, 65, 95),
                new[] { Whirlpool, IcicleBeak, DoubleBite, RainDance }, Ability.RainSurf),
            Monster(58, "ヒデリペン", new[] { Fire, Rock }, new Stats(90, 60, 85, 95, 80, 55),
                new[] { Eruption, CampfireBreath, Boulder, RockSkin }, Ability.SunCall),
            Monster(59, "ヒマワリペン", new[] { Moss }, new Stats(75, 55, 65, 105, 75, 95),
                new[] { MossDrain, MossCannon, Sunbathe, SleepMoss }, Ability.Photosynthesis),
            Monster(60, "コケマクラペン", new[] { Moss }, new Stats(105, 60, 95, 65, 95, 40),
                new[] { MossDrain, SlimySeaweed, ToxicMoss, MossBed }, Ability.Rest),
            Monster(61, "ワタユキペン", new[] { Ice, MonsterType.Normal }, new Stats(95, 75, 80, 60, 90, 70),
                new[] { RecklessTackle, ColdBreath, CutePose, Preen }, Ability.Rest),
            Monster(62, "カエンペン", new[] { Fire }, new Stats(70, 95, 60, 50, 60, 105),
                new[] { FlameRush, RecklessTackle, BellySlide, FlapDance }, Ability.RockHead),
            Monster(63, "イナズマペン", new[] { Thunder }, new Stats(70, 90, 65, 60, 65, 100),
                new[] { VoltRush, FlapAttack, DoubleBite, BellySlide }, Ability.Momentum),
            Monster(64, "ヨイヤミペン", new[] { Dark }, new Stats(85, 60, 75, 95, 85, 70),
                new[] { LifeSap, PolarNight, AuroraWave, DarkWhisper }, Ability.NightOwl),
            Monster(65, "ハゴロモペン", new[] { Aurora }, new Stats(85, 50, 75, 100, 100, 60),
                new[] { SparkleLaser, WindCutter, QuietPrayer, AuroraHeal }, Ability.Rest),
            Monster(66, "カイガラペン", new[] { Sea, Rock }, new Stats(80, 90, 110, 50, 70, 45),
                new[] { RockHeadbutt, Whirlpool, PebbleShot, RockSkin }, Ability.RockHead),
            Monster(67, "ライウンペン", new[] { Thunder, Sea }, new Stats(75, 55, 70, 90, 80, 85),
                new[] { StormDrop, BigWave, SparkWing, RainDance }, Ability.RainSurf),

            // ---- 段階3: ヒナ（68〜70） ----
            Monster(68, "コツブヒナペン", new[] { Rock }, new Stats(70, 65, 75, 35, 60, 55),
                new[] { PebbleShot, FlipperSlap, IcicleBarrage, RockSkin }, Ability.SkillLink, MonsterRank.Hina),
            Monster(69, "ネボスケヒナペン", new[] { Moss }, new Stats(90, 40, 70, 60, 75, 35),
                new[] { MossDrain, SleepMoss, PoisonMoss, MossBed }, Ability.Rest, MonsterRank.Hina),
            Monster(70, "ポカポカヒナペン", new[] { Fire }, new Stats(75, 40, 60, 75, 70, 65),
                new[] { CampfireBreath, WispFlame, FeatherRelay, Preen }, Ability.SunCall, MonsterRank.Hina),

            // ---- 段階3: エース（71〜73）。天候に乗ると すばやさ 2 倍で暴れるが、4倍の弱点がある ----
            Monster(71, "フブキオウペン", new[] { Ice, Bird }, new Stats(95, 120, 75, 60, 75, 115),
                new[] { IcicleBarrage, BraveDive, IcicleBeak, SnowCall }, Ability.SnowSlide, MonsterRank.Ace),
            Monster(72, "ウミドリペン", new[] { Sea, Bird }, new Stats(90, 110, 80, 65, 75, 110),
                new[] { Whirlpool, BraveDive, NorthWindCall, RainDance }, Ability.RainSurf, MonsterRank.Ace),
            Monster(73, "ジュカイペン", new[] { Moss, Bird }, new Stats(95, 120, 80, 90, 75, 80),
                new[] { BraveDive, MossDrain, CliffBreak, Sunbathe }, Ability.Photosynthesis, MonsterRank.Ace),

            // ---- 段階3: 伝説（74〜75）。天候を呼ぶボスの切り札 ----
            Monster(74, "ワダツミペン", new[] { Sea }, new Stats(110, 90, 95, 120, 105, 70),
                new[] { SeaGodVortex, StormDrop, LifeSap, Preen }, Ability.RainCall, MonsterRank.Legend),
            Monster(75, "アマテラスペン", new[] { Fire, Aurora }, new Stats(100, 120, 95, 100, 90, 85),
                new[] { SunWheelFlame, CliffBreak, SparkleLaser, FlapDance }, Ability.SunCall,
                MonsterRank.Legend),

            // ---- 段階4: ふつう（76〜125）。新しい特性（小技名人・向こう見ず・ひねくれ者・気まぐれ・ちゃっかり・まっさら）と、
            // 本物のペンギン・日本の物・仕事・妖怪などを題材にした子。同じタイプでも題材と戦い方で見分けられるようにする ----
            Monster(76, "スノードームペン", new[] { Ice, Aurora }, new Stats(80, 50, 85, 90, 100, 55),
                new[] { BlizzardBreath, AuroraWave, FrostKiss, AuroraHeal }, Ability.Multiscale),
            Monster(77, "カキゴオリペン", new[] { Ice }, new Stats(75, 70, 70, 90, 75, 75),
                new[] { BlizzardBreath, FrostKiss, IcicleBeak, BubbleSpit }, Ability.Moody),
            Monster(78, "フィギュアペン", new[] { Ice }, new Stats(65, 95, 60, 55, 70, 110),
                new[] { IcicleBeak, PrismKick, SnowPellet, IceStep }, Ability.Momentum),
            Monster(79, "ヒゲペン", new[] { Ice }, new Stats(80, 105, 70, 45, 60, 90),
                new[] { Avalanche, IcicleBeak, RecklessTackle, SnowPellet }, Ability.Reckless),
            Monster(80, "イグルーペン", new[] { Ice, Rock }, new Stats(95, 75, 105, 55, 80, 40),
                new[] { IcebergDrop, Boulder, FeatherWrap, Preen }, Ability.Rest),
            Monster(81, "ユキオンナペン", new[] { Ice, Dark }, new Stats(70, 50, 65, 105, 85, 85),
                new[] { BlizzardBreath, FrostKiss, ShadowStitch, DarkWhisper }, Ability.SnowCloud),
            Monster(82, "シロクマペン", new[] { Ice, MonsterType.Normal }, new Stats(100, 95, 80, 45, 70, 60),
                new[] { PenguinPress, IcicleBeak, OrcaBite, FeatherWrap }, Ability.Intimidate),
            Monster(83, "ユキガッセンペン", new[] { Ice }, new Stats(70, 90, 65, 50, 65, 110),
                new[] { IcicleBarrage, SnowPellet, DoublePeck, FlipperSlap }, Ability.Technician),
            Monster(84, "クリオネペン", new[] { Ice, Sea }, new Stats(85, 50, 75, 90, 95, 60),
                new[] { FrostKiss, BigWave, LifeSap, AuroraHeal }, Ability.Prankster),
            Monster(85, "スキーペン", new[] { Ice, Bird }, new Stats(70, 95, 65, 50, 60, 110),
                new[] { BraveDive, IcicleBarrage, SnowPellet, SnowCall }, Ability.SnowSlide),
            Monster(86, "ジェンツーペン", new[] { Sea }, new Stats(70, 95, 65, 55, 65, 110),
                new[] { Whirlpool, DiveJet, IcicleBeak, RainDance }, Ability.RainSurf),
            Monster(87, "ショウボウペン", new[] { Sea, MonsterType.Normal }, new Stats(85, 90, 75, 70, 75, 60),
                new[] { SprayBurst, BigWave, PenguinPress, Preen }, Ability.RainCall),
            Monster(88, "スミペン", new[] { Sea, Dark }, new Stats(80, 60, 75, 90, 85, 65),
                new[] { BigWave, PolarNight, ShadowStitch, FeatherWrap }, Ability.Prankster),
            Monster(89, "クラゲペン", new[] { Sea, Thunder }, new Stats(80, 50, 70, 95, 90, 70),
                new[] { Discharge, BigWave, NumbWave, Preen }, Ability.StaticBody),
            Monster(90, "カメノリペン", new[] { Sea, Moss }, new Stats(95, 80, 95, 60, 80, 45),
                new[] { Whirlpool, SeedBarrage, Boulder, MossBed }, Ability.Rest),
            Monster(91, "ニンギョペン", new[] { Sea, Aurora }, new Stats(75, 50, 70, 100, 90, 70),
                new[] { BigWave, SparkleLaser, Shout, AuroraDance }, Ability.RainSurf),
            Monster(92, "ガラパゴスペン", new[] { Sea, Fire }, new Stats(75, 60, 70, 95, 75, 80),
                new[] { CampfireBreath, HotSpring, BubbleSpit, Preen }, Ability.SunCall),
            Monster(93, "イワトビペン", new[] { Rock, Bird }, new Stats(100, 80, 85, 45, 65, 80),
                new[] { RockHeadbutt, BraveDive, BellySlide, SkyDream }, Ability.Reckless),
            Monster(94, "ユウビンペン", new[] { Bird, MonsterType.Normal }, new Stats(80, 90, 65, 60, 65, 105),
                new[] { FlapAttack, PenguinPress, Cheer, FeatherRelay }, Ability.Prankster),
            Monster(95, "タコアゲペン", new[] { Bird }, new Stats(65, 55, 65, 100, 75, 95),
                new[] { GaleFeather, WindCutter, ColdBreath, NorthWindCall }, Ability.Tailwind),
            Monster(96, "カザミドリペン", new[] { Bird }, new Stats(75, 80, 75, 75, 75, 75),
                new[] { FlapAttack, WindCutter, PeckDrain, SparkBeak }, Ability.Moody),
            Monster(97, "フクロウペン", new[] { Bird, Dark }, new Stats(85, 55, 75, 100, 90, 60),
                new[] { WindCutter, PolarNight, GaleFeather, Preen }, Ability.NightOwl),
            Monster(98, "テングペン", new[] { Bird, Moss }, new Stats(75, 60, 70, 95, 75, 80),
                new[] { GaleFeather, Whirlwind, MossCannon, NorthWindCall }, Ability.HardFeathers),
            Monster(99, "コタツペン", new[] { Fire, MonsterType.Normal }, new Stats(100, 60, 85, 80, 90, 45),
                new[] { CampfireBreath, Shout, PenguinPress, Preen }, Ability.Rest),
            Monster(100, "マカロニペン", new[] { Fire }, new Stats(70, 100, 65, 55, 65, 100),
                new[] { FlameRush, FlameDive, DoubleBite, IceStep }, Ability.Momentum),
            Monster(101, "サカダチペン", new[] { Fire }, new Stats(70, 60, 70, 105, 75, 75),
                new[] { BurnOut, SparkWing, ShadowStitch, Preen }, Ability.Contrary),
            Monster(102, "ヤキイモペン", new[] { Fire, Moss }, new Stats(90, 80, 85, 65, 80, 55),
                new[] { FlameDive, MossWhip, Boulder, Preen }, Ability.ThickFat),
            Monster(103, "ヒノマイペン", new[] { Fire, Aurora }, new Stats(70, 55, 65, 100, 85, 80),
                new[] { CampfireBreath, SparkleLaser, Eruption, AuroraDance }, Ability.LastStand),
            Monster(104, "チョウチンペン", new[] { Fire, Dark }, new Stats(80, 60, 70, 95, 85, 65),
                new[] { CampfireBreath, PolarNight, LifeSap, WispFlame }, Ability.NightOwl),
            Monster(105, "ロボペン", new[] { Thunder, MonsterType.Normal }, new Stats(85, 95, 90, 50, 70, 60),
                new[] { VoltRush, PenguinPress, ZapJab, BraceUp }, Ability.RockHead),
            Monster(106, "アンテナペン", new[] { Thunder, Aurora }, new Stats(80, 50, 75, 90, 85, 70),
                new[] { SparkWing, SparkleLaser, AuroraWave, ChargeDance }, Ability.StaticBody),
            Monster(107, "ロックンペン", new[] { Thunder, Rock }, new Stats(75, 90, 80, 80, 65, 65),
                new[] { VoltRush, CliffBreak, Shout, Discharge }, Ability.Intimidate),
            Monster(108, "タイコペン", new[] { Thunder }, new Stats(75, 90, 70, 50, 70, 100),
                new[] { ZapJab, SparkBeak, FlipperSlap, DoubleBite }, Ability.Technician),
            Monster(109, "パチパチペン", new[] { Thunder }, new Stats(85, 60, 80, 85, 80, 65),
                new[] { Discharge, SparkWing, Shout, FeatherWrap }, Ability.StaticBody),
            Monster(110, "タイフウペン", new[] { Thunder, Sea }, new Stats(85, 55, 75, 80, 85, 75),
                new[] { StormDrop, Whirlpool, Discharge, Whirlwind }, Ability.RainCall),
            Monster(111, "サボテンペン", new[] { Moss }, new Stats(80, 85, 90, 55, 75, 65),
                new[] { MossWhip, SeedBarrage, PebbleShot, MossArmor }, Ability.SpikySkin),
            Monster(112, "マリモペン", new[] { Moss }, new Stats(105, 50, 85, 75, 95, 40),
                new[] { SporeCloud, MossDrain, BigWave, FeatherWrap }, Ability.Multiscale),
            Monster(113, "マッチャペン", new[] { Moss, MonsterType.Normal }, new Stats(85, 60, 80, 90, 90, 50),
                new[] { MossCannon, SlimySeaweed, Shout, MossBed }, Ability.ThickFat),
            Monster(114, "ボンサイペン", new[] { Moss, Rock }, new Stats(90, 85, 95, 50, 80, 50),
                new[] { MossWhip, CliffBreak, SeedBarrage, Sunbathe }, Ability.Photosynthesis),
            Monster(115, "ハエトリソウペン", new[] { Moss, Dark }, new Stats(80, 95, 70, 60, 70, 80),
                new[] { MossWhip, DoubleBite, OrcaBite, Sneak }, Ability.PoisonSlime),
            Monster(116, "コイシドロボウペン", new[] { Rock, Dark }, new Stats(80, 80, 75, 50, 70, 85),
                new[] { PebbleShot, Sneak, PeckDrain, SharpGlare }, Ability.Technician),
            Monster(117, "ツルハシペン", new[] { Rock }, new Stats(85, 105, 90, 40, 65, 65),
                new[] { CliffBreak, RockHeadbutt, SparkBeak, BraceUp }, Ability.RockHead),
            Monster(118, "ハニワペン", new[] { Rock, MonsterType.Normal }, new Stats(90, 70, 100, 70, 90, 35),
                new[] { GemBeam, Boulder, Shout, RockSkin }, Ability.HardFeathers),
            Monster(119, "ドグウペン", new[] { Rock, Aurora }, new Stats(85, 55, 95, 90, 85, 45),
                new[] { GemBeam, SparkleLaser, SandSmoke, AuroraHeal }, Ability.LastStand),
            Monster(120, "ウチュウペン", new[] { Aurora }, new Stats(75, 55, 70, 100, 80, 75),
                new[] { SparkleLaser, AuroraWave, BlizzardBreath, AuroraDance }, Ability.NightOwl),
            Monster(121, "ペンキペン", new[] { Aurora, MonsterType.Normal }, new Stats(75, 60, 70, 95, 80, 75),
                new[] { AuroraWave, SparkleLaser, Shout, Cheer }, Ability.Prankster),
            Monster(122, "ユメクイペン", new[] { Aurora, Dark }, new Stats(85, 55, 75, 95, 90, 55),
                new[] { LifeSap, AuroraWave, ShadowStitch, SleepMoss }, Ability.NightOwl),
            Monster(123, "カイトウペン", new[] { Dark, MonsterType.Normal }, new Stats(70, 90, 65, 70, 65, 95),
                new[] { OrcaBite, Sneak, PenguinPress, SharpGlare }, Ability.Momentum),
            Monster(124, "キュウケツキペン", new[] { Dark }, new Stats(85, 80, 70, 80, 75, 65),
                new[] { LifeSap, PeckDrain, DoubleBite, DarkWhisper }, Ability.LastStand),
            Monster(125, "サイコロペン", new[] { MonsterType.Normal }, new Stats(75, 75, 75, 75, 75, 75),
                new[] { PenguinPress, Shout, IcicleBeak, BellySlide }, Ability.Moody),

            // ---- 段階4: ヒナ（126〜135）。新しい特性や積み技で「化ける」 ----
            Monster(126, "タマゴペン", new[] { MonsterType.Normal }, new Stats(75, 70, 80, 45, 70, 50),
                new[] { ShellShed, RecklessTackle, IcicleBeak, BellySlide }, Ability.Multiscale, MonsterRank.Hina),
            Monster(127, "ギョウレツヒナペン", new[] { MonsterType.Normal }, new Stats(60, 70, 55, 40, 55, 100),
                new[] { FlipperSlap, DoublePeck, BellySlide, Cheer }, Ability.Technician, MonsterRank.Hina),
            Monster(128, "ウキワヒナペン", new[] { Sea }, new Stats(75, 55, 60, 65, 70, 65),
                new[] { DiveJet, Whirlpool, RainDance, FeatherRelay }, Ability.RainSurf, MonsterRank.Hina),
            Monster(129, "デンキュウヒナペン", new[] { Thunder }, new Stats(60, 45, 65, 80, 70, 70),
                new[] { SparkWing, ChargeDance, NumbWave, FeatherRelay }, Ability.Moody, MonsterRank.Hina),
            Monster(130, "マッチヒナペン", new[] { Fire }, new Stats(70, 45, 60, 75, 65, 80),
                new[] { BurnOut, WispFlame, FeatherRelay, Preen }, Ability.Contrary, MonsterRank.Hina),
            Monster(131, "オバケヒナペン", new[] { Dark, Aurora }, new Stats(65, 45, 65, 75, 80, 60),
                new[] { WispFlame, DarkWhisper, LifeSap, AuroraHeal }, Ability.Prankster, MonsterRank.Hina),
            Monster(132, "フタバヒナペン", new[] { Moss }, new Stats(70, 45, 65, 90, 70, 60),
                new[] { MossCannon, MossDrain, Sunbathe, CampfireBreath }, Ability.Photosynthesis, MonsterRank.Hina),
            Monster(133, "ムテッポウヒナペン", new[] { MonsterType.Normal, Bird }, new Stats(60, 85, 50, 35, 45, 115),
                new[] { BraveDive, RecklessTackle, BellySlide, SkyDream }, Ability.Reckless, MonsterRank.Hina),
            Monster(134, "ユキダマヒナペン", new[] { Ice, Rock }, new Stats(85, 70, 85, 35, 70, 45),
                new[] { Avalanche, IcicleBarrage, PebbleShot, Fluff }, Ability.ThickFat, MonsterRank.Hina),
            Monster(135, "ホシクズヒナペン", new[] { Aurora }, new Stats(60, 75, 55, 60, 60, 80),
                new[] { ShootingStar, PrismKick, IceStep, FeatherRelay }, Ability.SkillLink, MonsterRank.Hina),

            // ---- 段階4: エース（136〜145）。純粋に強いが、4倍などはっきりした弱点がある ----
            Monster(136, "ライセツペン", new[] { Ice, Thunder }, new Stats(85, 70, 80, 105, 85, 95),
                new[] { BlizzardBreath, SparkWing, ColdBreath, ChargeDance }, Ability.SnowCloud, MonsterRank.Ace),
            Monster(137, "クジラノリペン", new[] { Sea, MonsterType.Normal }, new Stats(120, 105, 90, 60, 80, 70),
                new[] { Whirlpool, PenguinPress, DiveJet, Cheer }, Ability.Multiscale, MonsterRank.Ace),
            Monster(138, "スイショウペン", new[] { Aurora, Rock }, new Stats(85, 65, 100, 105, 95, 70),
                new[] { SparkleLaser, SandSmoke, ColdBreath, AuroraDance }, Ability.SpikySkin, MonsterRank.Ace),
            Monster(139, "プラズマペン", new[] { Fire, Thunder }, new Stats(75, 70, 70, 115, 80, 110),
                new[] { BurnOut, SparkWing, SparkleLaser, ChargeDance }, Ability.LastStand, MonsterRank.Ace),
            Monster(140, "オニペン", new[] { Rock, Dark }, new Stats(100, 120, 95, 60, 70, 75),
                new[] { CliffBreak, OrcaBite, Sneak, BraceUp }, Ability.Intimidate, MonsterRank.Ace),
            Monster(141, "ニンジャペン", new[] { Dark }, new Stats(80, 100, 70, 70, 70, 130),
                new[] { Sneak, DoubleBite, FlipperSlap, IceStep }, Ability.Technician, MonsterRank.Ace),
            Monster(142, "ヤミイナズマペン", new[] { Thunder, Dark }, new Stats(85, 110, 75, 80, 70, 100),
                new[] { SparkBeak, OrcaBite, ZapJab, FlapDance }, Ability.Momentum, MonsterRank.Ace),
            Monster(143, "ジュヒョウペン", new[] { Ice, Moss }, new Stats(110, 90, 95, 70, 90, 65),
                new[] { IcicleBarrage, SeedBarrage, SnowPellet, IceStep }, Ability.SkillLink, MonsterRank.Ace),
            Monster(144, "ヨコヅナペン", new[] { MonsterType.Normal, Rock }, new Stats(130, 120, 95, 40, 80, 60),
                new[] { PenguinPress, CliffBreak, FlipperSlap, BraceUp }, Ability.ThickFat, MonsterRank.Ace),
            Monster(145, "ユーフォーペン", new[] { Aurora }, new Stats(80, 70, 75, 105, 85, 105),
                new[] { SparkleLaser, SparkWing, ShadowStitch, AuroraHeal }, Ability.Moody, MonsterRank.Ace),

            // ---- 段階4: 伝説（146〜150）。後半のボスの切り札 ----
            Monster(146, "コダイオウペン", new[] { Rock, Ice }, new Stats(120, 135, 115, 60, 80, 80),
                new[] { AncientCharge, IcebergDrop, DiveJet, BraceUp }, Ability.Reckless, MonsterRank.Legend),
            Monster(147, "セカイジュペン", new[] { Moss }, new Stats(120, 70, 105, 115, 110, 75),
                new[] { WorldTreeBreath, SporeCloud, GemBeam, MossArmor }, Ability.Rest, MonsterRank.Legend),
            Monster(148, "ハジマリペン", new[] { MonsterType.Normal }, new Stats(110, 80, 100, 120, 100, 80),
                new[] { FirstRoar, ShadowStitch, BlizzardBreath, Preen }, Ability.Contrary, MonsterRank.Legend),
            Monster(149, "ニッショクペン", new[] { Dark, Fire }, new Stats(100, 90, 85, 130, 90, 95),
                new[] { BlackSun, Eruption, SparkWing, Scheme }, Ability.Avenger, MonsterRank.Legend),
            Monster(150, "アマツバサペン", new[] { Bird, Aurora }, new Stats(105, 125, 95, 90, 100, 75),
                new[] { SkySoar, PrismKick, IcicleBeak, IceStep }, Ability.Multiscale, MonsterRank.Legend),
        };

        /// <summary>No から引く（ストーリーの子も含む）。オンラインでは相手のチームを No の列で受け取るため</summary>
        public static MonsterDefinition ByNo(int no)
        {
            foreach (var list in new[] { All, Story })
            {
                foreach (var monster in list)
                {
                    if (monster.No == no)
                    {
                        return monster;
                    }
                }
            }
            throw new ArgumentException($"No.{no} のペンモンはいない", nameof(no));
        }

        /// <summary>ずかんの しぼりこみ（ストーリーの子も含む）。type が null なら全員。2タイプのペンモンはどちらのタイプでも出る</summary>
        public static IReadOnlyList<MonsterDefinition> WithType(MonsterType? type)
        {
            if (type == null)
            {
                return Dex;
            }
            var result = new List<MonsterDefinition>();
            foreach (var monster in Dex)
            {
                if (monster.HasType(type.Value))
                {
                    result.Add(monster);
                }
            }
            return result;
        }

        // 1行を短くするための別名。並びは MonsterDefinition のコンストラクタと同じ（持ち物はドラフトの後に配るので無し）
        private static MonsterDefinition Monster(int no, string name, MonsterType[] types, Stats stats, MoveDefinition[] moves,
            Ability ability, MonsterRank rank = MonsterRank.Normal)
            => new MonsterDefinition(no, name, types, stats, moves, ability, rank: rank);
    }
}
