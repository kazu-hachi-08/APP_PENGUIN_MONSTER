using System.Collections.Generic;
using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ストーリーの子（No.151〜）。ドラフトには出さないので All とは別の一覧にする。
    /// 種族値の合計は 進化前（Young）250〜330 / 最後の進化 425〜530 / 途中の段はその間。
    /// ここの技は図鑑に出す代表の4つで、実際に覚える技はストーリーの定義表（Town01Species・Town02Species・Town03Species）の覚える技の表で決まる
    /// </summary>
    public static partial class MonsterTable
    {
        public static readonly IReadOnlyList<MonsterDefinition> Story = new[]
        {
            // ---- 151〜180 ----
            // 最初の3体（海 → 火 → 苔 → 海 の三すくみ）。本家の最初の3体と同じ種族値の帯（309〜318 / 405 / 525〜530）
            Monster(151, "コブネペン", new[] { Sea }, new Stats(44, 48, 65, 50, 64, 43),
                new[] { Tackle, Growl, WaterGun, BubbleSpit }, Ability.LastStand, MonsterRank.Young),
            Monster(152, "ホバシラペン", new[] { Sea }, new Stats(59, 63, 80, 65, 80, 58),
                new[] { SeaGun, DiveJet, BubbleSpit, Fluff }, Ability.LastStand),
            Monster(153, "サイヒョウセンペン", new[] { Sea, Ice }, new Stats(79, 83, 100, 85, 105, 78),
                new[] { Whirlpool, IcicleBeak, BigWave, Fluff }, Ability.LastStand),
            Monster(154, "ヒダネペン", new[] { Fire }, new Stats(39, 52, 43, 60, 50, 65),
                new[] { Tackle, Growl, Ember, WispFlame }, Ability.LastStand, MonsterRank.Young),
            Monster(155, "フイゴペン", new[] { Fire }, new Stats(58, 64, 58, 80, 65, 80),
                new[] { FlameDive, Ember, FlareUp, WispFlame }, Ability.LastStand),
            Monster(156, "カタナカジペン", new[] { Fire, Rock }, new Stats(78, 100, 78, 104, 85, 85),
                new[] { CampfireBreath, Boulder, FlameRush, FlareUp }, Ability.LastStand),
            Monster(157, "メブキペン", new[] { Moss }, new Stats(45, 49, 49, 65, 65, 45),
                new[] { Tackle, Growl, MossPellet, SleepMoss }, Ability.LastStand, MonsterRank.Young),
            Monster(158, "フキノトウペン", new[] { Moss }, new Stats(60, 62, 63, 80, 80, 60),
                new[] { MossWhip, MossDrain, PoisonMoss, MossArmor }, Ability.LastStand),
            Monster(159, "ユキツバキペン", new[] { Moss, Aurora }, new Stats(80, 82, 83, 100, 100, 80),
                new[] { MossCannon, SparkleLaser, MossDrain, AuroraHeal }, Ability.LastStand),

            // 序盤の道路の子
            Monster(160, "コガラペン", new[] { MonsterType.Normal, Bird }, new Stats(40, 45, 40, 35, 35, 56),
                new[] { Peck, Growl, BellySlide, DoublePeck }, Ability.Momentum, MonsterRank.Young),
            Monster(161, "ツグミペン", new[] { MonsterType.Normal, Bird }, new Stats(63, 60, 55, 50, 50, 71),
                new[] { FlapAttack, Whirlwind, DoublePeck, SkyDream }, Ability.Momentum),
            Monster(162, "オオワシペン", new[] { MonsterType.Normal, Bird }, new Stats(83, 80, 75, 70, 70, 101),
                new[] { DiveBeak, BraveDive, PeckDrain, SkyDream }, Ability.Momentum),
            Monster(163, "オモチペン", new[] { MonsterType.Normal }, new Stats(60, 48, 55, 30, 55, 32),
                new[] { Pound, Growl, Fluff, Tackle }, Ability.ThickFat, MonsterRank.Young),
            Monster(164, "カガミモチペン", new[] { MonsterType.Normal }, new Stats(105, 85, 80, 50, 80, 40),
                new[] { PenguinPress, FlipperSlap, BraceUp, Preen }, Ability.ThickFat),
            Monster(165, "コナユキペン", new[] { Ice }, new Stats(45, 30, 45, 60, 55, 50),
                new[] { PowderSnow, Growl, ColdBreath, Fluff }, Ability.SnowSlide, MonsterRank.Young),
            Monster(166, "ボタンユキペン", new[] { Ice }, new Stats(60, 45, 60, 80, 70, 65),
                new[] { FrostKiss, ColdBreath, SnowCall, Fluff }, Ability.SnowSlide),
            Monster(167, "ゴウセツペン", new[] { Ice }, new Stats(85, 60, 80, 110, 90, 80),
                new[] { BlizzardBreath, FrostKiss, AuroraWave, QuietPrayer }, Ability.SnowCloud),
            Monster(168, "ツクシペン", new[] { Moss }, new Stats(45, 60, 50, 30, 40, 55),
                new[] { Pound, MossPellet, PoisonMoss, BellySlide }, Ability.SkillLink, MonsterRank.Young),
            Monster(169, "スギナペン", new[] { Moss }, new Stats(70, 95, 75, 50, 65, 90),
                new[] { MossWhip, SeedBarrage, FlapDance, PoisonMoss }, Ability.SkillLink),
            Monster(170, "ヤマメペン", new[] { Sea }, new Stats(45, 60, 40, 35, 40, 80),
                new[] { WaterGun, TailWag, BellySlide, DiveJet }, Ability.RainSurf, MonsterRank.Young),
            Monster(171, "サクラマスペン", new[] { Sea }, new Stats(70, 95, 65, 60, 60, 110),
                new[] { SeaGun, SprayBurst, Whirlpool, RainDance }, Ability.RainSurf),
            Monster(172, "パチリペン", new[] { Thunder }, new Stats(40, 35, 35, 65, 45, 90),
                new[] { StaticShock, Growl, NumbWave, ZapJab }, Ability.StaticBody, MonsterRank.Young),
            Monster(173, "ライチョウペン", new[] { Thunder }, new Stats(65, 60, 60, 100, 75, 120),
                new[] { SparkWing, Discharge, WindCutter, ChargeDance }, Ability.StaticBody),
            Monster(174, "イシコロペン", new[] { Rock }, new Stats(40, 70, 95, 25, 30, 20),
                new[] { Tackle, RockThrow, Fluff, PebbleShot }, Ability.RockHead, MonsterRank.Young),
            Monster(175, "イシドウロウペン", new[] { Rock }, new Stats(75, 105, 130, 55, 65, 45),
                new[] { Boulder, CliffBreak, RockHeadbutt, RockSkin }, Ability.RockHead),
            Monster(176, "ワラシペン", new[] { Dark }, new Stats(40, 50, 45, 60, 50, 65),
                new[] { Pester, Growl, Twinkle, DarkWhisper }, Ability.Prankster, MonsterRank.Young),
            Monster(177, "ザシキワラシペン", new[] { Dark }, new Stats(65, 75, 65, 95, 80, 100),
                new[] { LifeSap, PolarNight, CutePose, Scheme }, Ability.Prankster),

            // 道場主の切り札（野生では出ない）。道場では Lv14 の進化前で出る
            Monster(178, "カワラペン", new[] { Rock }, new Stats(55, 75, 100, 30, 50, 20),
                new[] { RockThrow, Fluff, SandSmoke, RockSkin }, Ability.HardFeathers, MonsterRank.Young),
            Monster(179, "オニガワラペン", new[] { Rock }, new Stats(90, 115, 140, 50, 75, 40),
                new[] { CliffBreak, Boulder, RockHeadbutt, BraceUp }, Ability.Intimidate),

            // とてもまれな不思議な子（進化しない）
            Monster(180, "ダイヤダストペン", new[] { Aurora }, new Stats(70, 50, 65, 95, 90, 85),
                new[] { SparkleLaser, FrostKiss, AuroraHeal, AuroraDance }, Ability.Multiscale),

            // ---- 181〜210 ----
            // 3番・4番道路の子
            Monster(181, "ヒバリペン", new[] { Bird }, new Stats(40, 50, 35, 35, 35, 65),
                new[] { Peck, Growl, DoublePeck, Whirlwind }, Ability.Tailwind, MonsterRank.Young),
            Monster(182, "ハヤテペン", new[] { Bird }, new Stats(65, 85, 60, 55, 55, 110),
                new[] { FlapAttack, WindCutter, DiveBeak, SkyDream }, Ability.Tailwind),
            Monster(183, "ワカサギペン", new[] { Sea }, new Stats(45, 40, 40, 50, 45, 70),
                new[] { WaterGun, TailWag, BubbleSpit, SprayBurst }, Ability.SkillLink, MonsterRank.Young),
            Monster(184, "ヒメマスペン", new[] { Sea }, new Stats(60, 70, 55, 70, 60, 85),
                new[] { SeaGun, SprayBurst, DiveJet, Splash }, Ability.SkillLink),
            Monster(185, "イトウペン", new[] { Sea }, new Stats(95, 105, 80, 70, 75, 90),
                new[] { Whirlpool, SprayBurst, OrcaBite, BigWave }, Ability.SkillLink),
            // 氷の道場の対策になる火の子（まれ）
            Monster(186, "ユタンポペン", new[] { Fire }, new Stats(60, 40, 60, 55, 60, 25),
                new[] { Ember, Fluff, WispFlame, Sunbathe }, Ability.ThickFat, MonsterRank.Young),
            Monster(187, "ダルマストーブペン", new[] { Fire }, new Stats(95, 70, 95, 100, 95, 45),
                new[] { CampfireBreath, Eruption, Sunbathe, BraceUp }, Ability.ThickFat),
            Monster(188, "ゴロタペン", new[] { Rock }, new Stats(50, 65, 75, 25, 35, 30),
                new[] { Tackle, RockThrow, PebbleShot, RockSkin }, Ability.SpikySkin, MonsterRank.Young),
            Monster(189, "ガンクツペン", new[] { Rock }, new Stats(85, 100, 115, 45, 70, 50),
                new[] { CliffBreak, Boulder, SandSmoke, RockSkin }, Ability.SpikySkin),
            Monster(190, "ヒバナペン", new[] { Thunder }, new Stats(35, 35, 30, 60, 40, 70),
                new[] { StaticShock, Growl, ZapJab, NumbWave }, Ability.Technician, MonsterRank.Young),
            Monster(191, "イカズチペン", new[] { Thunder }, new Stats(55, 55, 50, 80, 60, 90),
                new[] { SparkWing, ZapJab, ChargeDance, NumbWave }, Ability.Technician),
            Monster(192, "ブリオコシペン", new[] { Thunder }, new Stats(85, 70, 75, 115, 85, 95),
                new[] { StormDrop, Discharge, SparkWing, ChargeDance }, Ability.Intimidate),
            Monster(193, "フユメペン", new[] { Moss }, new Stats(45, 40, 55, 50, 60, 30),
                new[] { MossPellet, Growl, SleepMoss, MossArmor }, Ability.Rest, MonsterRank.Young),
            Monster(194, "ネコヤナギペン", new[] { Moss }, new Stats(80, 70, 90, 85, 100, 55),
                new[] { MossDrain, MossCannon, SporeCloud, MossBed }, Ability.Rest),
            Monster(195, "イタチペン", new[] { Dark }, new Stats(40, 60, 35, 30, 35, 75),
                new[] { Pester, TailWag, Sneak, SharpGlare }, Ability.Momentum, MonsterRank.Young),
            Monster(196, "カマイタチペン", new[] { Dark }, new Stats(65, 105, 60, 50, 60, 115),
                new[] { OrcaBite, Sneak, WindCutter, Scheme }, Ability.Momentum),
            Monster(197, "ツキアカリペン", new[] { Aurora }, new Stats(50, 30, 45, 65, 60, 50),
                new[] { Twinkle, Growl, QuietPrayer, ShootingStar }, Ability.NightOwl, MonsterRank.Young),
            Monster(198, "オボロヅキペン", new[] { Aurora }, new Stats(85, 50, 75, 110, 100, 75),
                new[] { AuroraWave, PolarNight, AuroraHeal, QuietPrayer }, Ability.NightOwl),
            Monster(199, "ハンテンペン", new[] { MonsterType.Normal }, new Stats(55, 50, 50, 35, 45, 45),
                new[] { Tackle, Growl, Fluff, FeatherWrap }, Ability.ThickFat, MonsterRank.Young),
            Monster(200, "ドテラペン", new[] { MonsterType.Normal }, new Stats(100, 85, 80, 50, 75, 55),
                new[] { PenguinPress, FeatherWrap, Shout, BraceUp }, Ability.ThickFat),
            Monster(201, "シモバシラペン", new[] { Ice }, new Stats(45, 60, 65, 30, 40, 40),
                new[] { SnowPellet, Growl, IcicleBarrage, Fluff }, Ability.HardFeathers, MonsterRank.Young),
            Monster(202, "ヒョウザンペン", new[] { Ice }, new Stats(90, 105, 110, 50, 70, 40),
                new[] { IcebergDrop, IcicleBarrage, Boulder, BraceUp }, Ability.HardFeathers),
            Monster(203, "ハスハペン", new[] { Ice, Sea }, new Stats(55, 35, 55, 55, 55, 45),
                new[] { PowderSnow, WaterGun, Splash, IceStep }, Ability.RainSurf, MonsterRank.Young),
            Monster(204, "オミワタリペン", new[] { Ice, Sea }, new Stats(95, 80, 90, 95, 85, 70),
                new[] { BlizzardBreath, BigWave, IcicleBeak, IceStep }, Ability.RainSurf),

            // 道場主サエの切り札（野生では出ない）。道場では Lv20 の進化前で出る。場を吹雪にして氷の味方を助ける
            Monster(205, "ウスライペン", new[] { Ice }, new Stats(50, 40, 55, 70, 60, 55),
                new[] { ColdBreath, IceStep, FrostKiss, SnowCall }, Ability.SnowCloud, MonsterRank.Young),
            Monster(206, "ヒョウキョウペン", new[] { Ice }, new Stats(85, 60, 90, 115, 105, 70),
                new[] { BlizzardBreath, FrostKiss, SparkleLaser, IceStep }, Ability.SnowCloud),

            Monster(207, "コハクチョウペン", new[] { Bird, Ice }, new Stats(50, 45, 45, 50, 55, 55),
                new[] { Peck, PowderSnow, Whirlwind, Preen }, Ability.SnowSlide, MonsterRank.Young),
            Monster(208, "オオハクチョウペン", new[] { Bird, Ice }, new Stats(90, 75, 80, 85, 90, 90),
                new[] { WindCutter, BlizzardBreath, SkyDream, Preen }, Ability.SnowSlide),

            // カガミコ町の人から1回だけもらえる子（進化しない。Lv15 でもらう）
            Monster(209, "オマモリペン", new[] { MonsterType.Normal }, new Stats(75, 60, 75, 70, 90, 60),
                new[] { Shout, Preen, FeatherWrap, QuietPrayer }, Ability.Prankster),

            // 4番道路の奥の とてもまれな不思議な子（進化しない）
            Monster(210, "シンキロウペン", new[] { Aurora, Dark }, new Stats(70, 55, 65, 100, 80, 85),
                new[] { SparkleLaser, PolarNight, ShadowStitch, AuroraDance }, Ability.Moody),

            // ---- 211〜240 ----
            // 5番・6番道路の子
            Monster(211, "マメデンペン", new[] { Thunder }, new Stats(40, 35, 35, 55, 40, 65),
                new[] { StaticShock, Growl, ZapJab, NumbWave }, Ability.StaticBody, MonsterRank.Young),
            Monster(212, "ゴロゴロペン", new[] { Thunder }, new Stats(60, 55, 55, 80, 60, 80),
                new[] { SparkWing, SparkBeak, ChargeDance, NumbWave }, Ability.StaticBody),
            Monster(213, "ナルカミペン", new[] { Thunder }, new Stats(85, 75, 75, 115, 85, 90),
                new[] { StormDrop, Discharge, SparkBeak, ChargeDance }, Ability.StaticBody),
            // 雷の道場の対策になる岩の子。岩石頭突きの反動を石頭で受けない
            Monster(214, "ジャリペン", new[] { Rock }, new Stats(50, 60, 70, 25, 30, 35),
                new[] { Tackle, RockThrow, PebbleShot, SandSmoke }, Ability.RockHead, MonsterRank.Young),
            Monster(215, "ガンペキペン", new[] { Rock }, new Stats(90, 105, 120, 40, 65, 45),
                new[] { RockHeadbutt, CliffBreak, Boulder, RockSkin }, Ability.RockHead),
            Monster(216, "ササペン", new[] { Moss }, new Stats(50, 55, 50, 40, 50, 45),
                new[] { MossPellet, Growl, Fluff, SeedBarrage }, Ability.Photosynthesis, MonsterRank.Young),
            Monster(217, "クマザサペン", new[] { Moss }, new Stats(85, 100, 85, 60, 75, 70),
                new[] { MossWhip, SeedBarrage, FlapDance, RecklessTackle }, Ability.Photosynthesis),
            Monster(218, "ツバメペン", new[] { Bird }, new Stats(40, 55, 30, 30, 30, 85),
                new[] { Peck, TailWag, BellySlide, DoublePeck }, Ability.Technician, MonsterRank.Young),
            Monster(219, "ハヤブサペン", new[] { Bird }, new Stats(70, 100, 60, 50, 60, 120),
                new[] { DiveBeak, PeckDrain, BraveDive, SkyDream }, Ability.Technician),
            // 雷の季節に獲れる魚（鰰）。海の子だが体に電気をためている
            Monster(220, "ハタハタペン", new[] { Sea }, new Stats(50, 40, 45, 55, 50, 60),
                new[] { WaterGun, Growl, BubbleSpit, StaticShock }, Ability.StaticBody, MonsterRank.Young),
            Monster(221, "カンブリペン", new[] { Sea }, new Stats(90, 95, 75, 70, 70, 85),
                new[] { Whirlpool, SeaGun, SparkBeak, BigWave }, Ability.StaticBody),
            Monster(222, "アラレペン", new[] { Ice }, new Stats(45, 40, 40, 60, 50, 55),
                new[] { PowderSnow, SnowPellet, IcicleBarrage, ColdBreath }, Ability.Technician, MonsterRank.Young),
            Monster(223, "ヒサメペン", new[] { Ice }, new Stats(80, 70, 70, 105, 85, 90),
                new[] { BlizzardBreath, FrostKiss, SnowCall, IcicleBeak }, Ability.SnowSlide),
            // 停電の暗がりにひそむ子（夜タマゴの見せ場）
            Monster(224, "テイデンペン", new[] { Dark }, new Stats(45, 50, 40, 50, 40, 65),
                new[] { Pester, Sneak, DarkWhisper, StaticShock }, Ability.NightOwl, MonsterRank.Young),
            Monster(225, "クラヤミペン", new[] { Dark }, new Stats(75, 80, 70, 100, 80, 95),
                new[] { PolarNight, LifeSap, Scheme, SparkWing }, Ability.NightOwl),
            Monster(226, "ユキアカリペン", new[] { Aurora }, new Stats(50, 30, 45, 65, 65, 45),
                new[] { Twinkle, QuietPrayer, ShootingStar, AuroraHeal }, Ability.Rest, MonsterRank.Young),
            Monster(227, "ギンガペン", new[] { Aurora }, new Stats(85, 50, 80, 115, 105, 75),
                new[] { SparkleLaser, AuroraWave, AuroraDance, AuroraHeal }, Ability.Rest),
            Monster(228, "ユキグツペン", new[] { MonsterType.Normal }, new Stats(55, 55, 45, 30, 40, 55),
                new[] { Tackle, TailWag, BellySlide, IceStep }, Ability.Momentum, MonsterRank.Young),
            Monster(229, "カンジキペン", new[] { MonsterType.Normal }, new Stats(85, 95, 75, 45, 65, 85),
                new[] { PenguinPress, RecklessTackle, FlipperSlap, IceStep }, Ability.Momentum),
            // 電線にとまる鳥 → 鉄塔
            Monster(230, "デンセンペン", new[] { Thunder, Bird }, new Stats(40, 40, 35, 55, 40, 70),
                new[] { Peck, StaticShock, Whirlwind, ZapJab }, Ability.Tailwind, MonsterRank.Young),
            Monster(231, "テットウペン", new[] { Thunder, Bird }, new Stats(80, 75, 90, 100, 80, 75),
                new[] { Discharge, WindCutter, StormDrop, NorthWindCall }, Ability.Tailwind),
            // 5番道路の岩場の子（重いタマゴの見せ場）
            Monster(232, "ユキイシペン", new[] { Rock, Ice }, new Stats(55, 60, 75, 25, 35, 25),
                new[] { Tackle, PowderSnow, RockThrow, SnowPellet }, Ability.ThickFat, MonsterRank.Young),
            Monster(233, "ヒョウヘキペン", new[] { Rock, Ice }, new Stats(95, 105, 125, 40, 65, 35),
                new[] { IcebergDrop, CliffBreak, Boulder, RockSkin }, Ability.ThickFat),
            Monster(234, "カイロペン", new[] { Fire }, new Stats(45, 45, 45, 55, 45, 55),
                new[] { Ember, Fluff, WispFlame, FlareUp }, Ability.Reckless, MonsterRank.Young),
            Monster(235, "ダンロペン", new[] { Fire }, new Stats(90, 100, 80, 85, 70, 60),
                new[] { FlameRush, CampfireBreath, Eruption, BraceUp }, Ability.Reckless),

            // 道場主ライゾウの切り札（野生では出ない）。道場では Lv26 の進化前で出る。充電ダンスを積んで撃つ
            Monster(236, "ハツデンペン", new[] { Thunder }, new Stats(55, 45, 50, 65, 55, 60),
                new[] { Discharge, SparkWing, ChargeDance, NumbWave }, Ability.StaticBody, MonsterRank.Young),
            Monster(237, "ライジンペン", new[] { Thunder }, new Stats(85, 70, 85, 120, 95, 75),
                new[] { StormDrop, Discharge, Shout, ChargeDance }, Ability.StaticBody),

            // ユキオコシ町の人から1回だけもらえる子（進化しない。Lv20 でもらう）。停電の町の灯り
            Monster(238, "カンテラペン", new[] { MonsterType.Normal }, new Stats(70, 60, 70, 85, 80, 65),
                new[] { Shout, Ember, Preen, FeatherWrap }, Ability.NightOwl),

            // 発電所に居座る子（進化しない。1回だけ Lv24 で戦う）。雷様のようにへそを狙う、強めの子
            Monster(239, "ヘソトリペン", new[] { Thunder }, new Stats(80, 100, 75, 95, 70, 70),
                new[] { SparkBeak, Discharge, SharpGlare, ZapJab }, Ability.Intimidate),

            // 6番道路の奥の とてもまれな不思議な子（進化しない）
            Monster(240, "セントエルモペン", new[] { Thunder, Aurora }, new Stats(65, 50, 65, 105, 85, 90),
                new[] { SparkleLaser, Discharge, AuroraWave, ChargeDance }, Ability.Prankster),
        };

        private static IReadOnlyList<MonsterDefinition> _dex;

        /// <summary>図鑑に並べる全員（ドラフトの子 → ストーリーの子の No 順）</summary>
        public static IReadOnlyList<MonsterDefinition> Dex => _dex ??= Concat(All, Story);

        private static List<MonsterDefinition> Concat(IReadOnlyList<MonsterDefinition> first, IReadOnlyList<MonsterDefinition> second)
        {
            var all = new List<MonsterDefinition>(first);
            all.AddRange(second);
            return all;
        }

        /// <summary>ストーリーの子（ドラフトに出ない子）</summary>
        public static bool IsStory(int no) => no > All.Count;
    }
}
