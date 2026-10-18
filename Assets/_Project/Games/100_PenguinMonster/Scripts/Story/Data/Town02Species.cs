using System.Collections.Generic;
using PenguinMonster.Battle;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 種（No.181〜210）。作りは Town01Species と同じ。
    /// 3番・4番道路の野生は Lv10〜16 なので、威力 75 以上の技は Lv20 前後から覚えるように並べる
    /// </summary>
    public static class Town02Species
    {
        // 捕まえやすさの値は Town01Species と同じ

        public static readonly StorySpecies Hibari = Species(181, GrowthRate.MediumFast, 55, Town01Species.CommonCatchRate, new Evolution(20, 182),
            (1, Peck), (1, Growl), (6, TailWag), (10, DoublePeck), (15, Whirlwind), (22, FlapAttack), (28, SkyDream));
        public static readonly StorySpecies Hayate = Species(182, GrowthRate.MediumFast, 165, Town01Species.RareCatchRate, null,
            (1, Peck), (1, Growl), (6, TailWag), (10, DoublePeck), (15, Whirlwind), (20, WindCutter), (25, FlapAttack), (31, SkyDream),
            (38, DiveBeak));

        public static readonly StorySpecies Wakasagi = Species(183, GrowthRate.MediumFast, 58, Town01Species.UncommonCatchRate, new Evolution(18, 184),
            (1, WaterGun), (1, TailWag), (6, BubbleSpit), (11, SprayBurst), (16, Splash), (22, DiveJet), (28, SeaGun));
        public static readonly StorySpecies Himemasu = Species(184, GrowthRate.MediumFast, 130, Town01Species.MiddleCatchRate, new Evolution(34, 185),
            (1, WaterGun), (1, TailWag), (6, BubbleSpit), (11, SprayBurst), (16, Splash), (18, DiveJet), (24, SeaGun), (30, RainDance));
        public static readonly StorySpecies Itou = Species(185, GrowthRate.MediumFast, 225, Town01Species.RareCatchRate, null,
            (1, WaterGun), (1, TailWag), (6, BubbleSpit), (11, SprayBurst), (16, Splash), (18, DiveJet), (24, SeaGun), (30, RainDance),
            (34, Whirlpool), (40, OrcaBite), (46, BigWave));

        public static readonly StorySpecies Yutanpo = Species(186, GrowthRate.MediumFast, 62, Town01Species.MiddleCatchRate, new Evolution(28, 187),
            (1, Ember), (1, Growl), (7, Fluff), (12, WispFlame), (18, Sunbathe), (24, CampfireBreath));
        public static readonly StorySpecies DarumaStove = Species(187, GrowthRate.MediumFast, 185, Town01Species.RareCatchRate, null,
            (1, Ember), (1, Growl), (7, Fluff), (12, WispFlame), (18, Sunbathe), (24, CampfireBreath), (28, BraceUp), (36, Eruption));

        public static readonly StorySpecies Gorota = Species(188, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(26, 189),
            (1, Tackle), (1, Growl), (5, RockThrow), (10, PebbleShot), (15, RockSkin), (21, SandSmoke), (28, Boulder));
        public static readonly StorySpecies Gankutsu = Species(189, GrowthRate.MediumFast, 170, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, Growl), (5, RockThrow), (10, PebbleShot), (15, RockSkin), (21, SandSmoke), (26, Boulder), (32, CliffBreak),
            (40, RockHeadbutt));

        public static readonly StorySpecies Hibana = Species(190, GrowthRate.MediumFast, 55, Town01Species.UncommonCatchRate, new Evolution(18, 191),
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, NumbWave), (15, ChargeDance), (22, SparkWing));
        public static readonly StorySpecies Ikazuchi = Species(191, GrowthRate.MediumFast, 130, Town01Species.MiddleCatchRate, new Evolution(32, 192),
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, NumbWave), (15, ChargeDance), (18, SparkWing), (25, SparkBeak), (30, Discharge));
        public static readonly StorySpecies Briokoshi = Species(192, GrowthRate.MediumFast, 230, Town01Species.RareCatchRate, null,
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, NumbWave), (15, ChargeDance), (18, SparkWing), (25, SparkBeak), (30, Discharge),
            (32, StormDrop), (40, Shout));

        public static readonly StorySpecies Fuyume = Species(193, GrowthRate.MediumFast, 58, Town01Species.UncommonCatchRate, new Evolution(24, 194),
            (1, MossPellet), (1, Growl), (6, SleepMoss), (11, MossArmor), (15, PoisonMoss), (19, MossDrain), (27, SporeCloud));
        public static readonly StorySpecies Nekoyanagi = Species(194, GrowthRate.MediumFast, 175, Town01Species.RareCatchRate, null,
            (1, MossPellet), (1, Growl), (6, SleepMoss), (11, MossArmor), (15, PoisonMoss), (19, MossDrain), (24, MossBed), (29, SporeCloud),
            (35, MossCannon));

        public static readonly StorySpecies Itachi = Species(195, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(28, 196),
            (1, Pester), (1, TailWag), (6, Sneak), (11, SharpGlare), (16, DoubleBite), (22, Scheme), (30, OrcaBite));
        public static readonly StorySpecies Kamaitachi = Species(196, GrowthRate.MediumFast, 175, Town01Species.RareCatchRate, null,
            (1, Pester), (1, TailWag), (6, Sneak), (11, SharpGlare), (16, DoubleBite), (22, Scheme), (28, WindCutter), (34, OrcaBite));

        public static readonly StorySpecies Tsukiakari = Species(197, GrowthRate.MediumFast, 64, Town01Species.MiddleCatchRate, new Evolution(30, 198),
            (1, Twinkle), (1, Growl), (7, QuietPrayer), (12, ShootingStar), (17, ShadowStitch), (23, AuroraHeal), (29, AuroraWave));
        public static readonly StorySpecies Oborozuki = Species(198, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, Twinkle), (1, Growl), (7, QuietPrayer), (12, ShootingStar), (17, ShadowStitch), (23, AuroraHeal), (29, AuroraWave),
            (30, PolarNight), (38, SparkleLaser));

        public static readonly StorySpecies Hanten = Species(199, GrowthRate.MediumFast, 55, Town01Species.CommonCatchRate, new Evolution(22, 200),
            (1, Tackle), (1, Growl), (6, Fluff), (11, BellySlide), (16, FeatherWrap), (26, PenguinPress));
        public static readonly StorySpecies Dotera = Species(200, GrowthRate.MediumFast, 165, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, Growl), (6, Fluff), (11, BellySlide), (16, FeatherWrap), (22, BraceUp), (28, PenguinPress), (34, Shout));

        public static readonly StorySpecies Shimobashira = Species(201, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(30, 202),
            (1, SnowPellet), (1, Growl), (6, Fluff), (11, IcicleBarrage), (17, RockThrow), (23, IcicleBeak), (29, BraceUp));
        public static readonly StorySpecies Hyouzan = Species(202, GrowthRate.MediumFast, 175, Town01Species.RareCatchRate, null,
            (1, SnowPellet), (1, Growl), (6, Fluff), (11, IcicleBarrage), (17, RockThrow), (23, IcicleBeak), (29, BraceUp),
            (30, IcebergDrop), (38, Boulder));

        public static readonly StorySpecies Hasuha = Species(203, GrowthRate.MediumFast, 62, Town01Species.UncommonCatchRate, new Evolution(32, 204),
            (1, PowderSnow), (1, WaterGun), (6, Splash), (11, BubbleSpit), (16, IceStep), (22, ColdBreath), (28, SeaGun));
        public static readonly StorySpecies Omiwatari = Species(204, GrowthRate.MediumFast, 225, Town01Species.RareCatchRate, null,
            (1, PowderSnow), (1, WaterGun), (6, Splash), (11, BubbleSpit), (16, IceStep), (22, ColdBreath), (28, SeaGun),
            (32, IcicleBeak), (38, BigWave), (44, BlizzardBreath));

        // 道場主の切り札。Lv20 で「氷上ステップ・冷たい息・雪乞い・凍えるキッス」になるように並べる
        public static readonly StorySpecies Usurai = Species(205, GrowthRate.MediumSlow, 70, Town01Species.RareCatchRate, new Evolution(32, 206),
            (1, PowderSnow), (1, Growl), (5, IceStep), (9, ColdBreath), (13, SnowCall), (17, FrostKiss), (24, AuroraWave),
            (30, BlizzardBreath));
        public static readonly StorySpecies Hyoukyou = Species(206, GrowthRate.MediumSlow, 235, Town01Species.RareCatchRate, null,
            (1, PowderSnow), (1, Growl), (5, IceStep), (9, ColdBreath), (13, SnowCall), (17, FrostKiss), (24, AuroraWave),
            (30, BlizzardBreath), (32, SparkleLaser), (40, QuietPrayer));

        public static readonly StorySpecies Kohakuchou = Species(207, GrowthRate.MediumFast, 64, Town01Species.UncommonCatchRate, new Evolution(30, 208),
            (1, Peck), (1, PowderSnow), (6, Preen), (11, Whirlwind), (16, ColdBreath), (22, WindCutter), (28, FrostKiss));
        public static readonly StorySpecies Oohakuchou = Species(208, GrowthRate.MediumFast, 220, Town01Species.RareCatchRate, null,
            (1, Peck), (1, PowderSnow), (6, Preen), (11, Whirlwind), (16, ColdBreath), (22, WindCutter), (28, FrostKiss),
            (30, SkyDream), (36, BlizzardBreath), (42, GaleFeather));

        // 町の人からもらう子。Lv15 で「ぴいぴい鳴く・羽づくろい・羽でくるまる・叫び声」になる
        public static readonly StorySpecies Omamori = Species(209, GrowthRate.MediumFast, 165, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, Growl), (5, Preen), (10, FeatherWrap), (15, Shout), (21, QuietPrayer), (28, PenguinPress));

        public static readonly StorySpecies Shinkirou = Species(210, GrowthRate.MediumSlow, 180, Town01Species.VeryRareCatchRate, null,
            (1, Twinkle), (1, Pester), (8, ShadowStitch), (14, QuietPrayer), (20, LifeSap), (26, AuroraWave), (32, PolarNight),
            (38, SparkleLaser), (44, AuroraDance));

        public static readonly IReadOnlyList<StorySpecies> All = new[]
        {
            Hibari, Hayate, Wakasagi, Himemasu, Itou, Yutanpo, DarumaStove, Gorota, Gankutsu, Hibana, Ikazuchi, Briokoshi,
            Fuyume, Nekoyanagi, Itachi, Kamaitachi, Tsukiakari, Oborozuki, Hanten, Dotera, Shimobashira, Hyouzan,
            Hasuha, Omiwatari, Usurai, Hyoukyou, Kohakuchou, Oohakuchou, Omamori, Shinkirou,
        };

        private static readonly Dictionary<int, StorySpecies> ByNumber = new Dictionary<int, StorySpecies>();

        static Town02Species()
        {
            foreach (var species in All)
            {
                ByNumber[species.No] = species;
            }
        }

        /// <summary>No から種を引く。無ければ null</summary>
        public static StorySpecies ByNo(int no) => ByNumber.TryGetValue(no, out var species) ? species : null;

        // 1行を短くするための別名。種族値・タイプ・特性は MonsterTable.Story から引く
        private static StorySpecies Species(int no, GrowthRate growthRate, int baseExp, int catchRate, Evolution evolution,
            params (int Level, MoveDefinition Move)[] learnset)
        {
            var moves = new LevelMove[learnset.Length];
            for (int i = 0; i < learnset.Length; i++)
            {
                moves[i] = new LevelMove(learnset[i].Level, learnset[i].Move);
            }
            return new StorySpecies(MonsterTable.ByNo(no), growthRate, baseExp, moves, evolution, catchRate);
        }
    }
}
