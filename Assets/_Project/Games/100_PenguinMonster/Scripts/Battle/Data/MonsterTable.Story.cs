using System.Collections.Generic;
using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ストーリーの子（No.151〜。PLAN_STORY_TOWN01.md §1・フェーズ8a）。ドラフトには出さないので All とは別の一覧にする。
    /// 種族値の合計は 進化前（Young）250〜330 / 最後の進化 425〜530 / 途中の段はその間。
    /// ここの技は図鑑に出す代表の4つで、実際に覚える技はストーリーの定義表（Town01Species）の覚える技の表で決まる
    /// </summary>
    public static partial class MonsterTable
    {
        public static readonly IReadOnlyList<MonsterDefinition> Story = new[]
        {
            // ---- 区切り1（151〜180） ----
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
        };
    }
}
