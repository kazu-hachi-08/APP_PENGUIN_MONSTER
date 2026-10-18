using System.Collections.Generic;
using PenguinMonster.Battle;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 種（No.211〜240）。作りは Town02Species と同じ。
    /// 5番・6番道路の野生は Lv16〜22 なので、威力 75 以上の技は Lv22 前後から覚えるように並べる
    /// </summary>
    public static class Town03Species
    {
        // 捕まえやすさの値は Town01Species と同じ

        public static readonly StorySpecies Mameden = Species(211, GrowthRate.MediumFast, 56, Town01Species.UncommonCatchRate, new Evolution(20, 212),
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, NumbWave), (15, ChargeDance), (24, SparkWing), (30, Discharge));
        public static readonly StorySpecies Gorogoro = Species(212, GrowthRate.MediumFast, 135, Town01Species.MiddleCatchRate, new Evolution(36, 213),
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, NumbWave), (15, ChargeDance), (20, SparkWing), (26, SparkBeak),
            (32, Discharge));
        public static readonly StorySpecies Narukami = Species(213, GrowthRate.MediumFast, 230, Town01Species.RareCatchRate, null,
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, NumbWave), (15, ChargeDance), (20, SparkWing), (26, SparkBeak),
            (32, Discharge), (36, StormDrop), (44, VoltRush));

        public static readonly StorySpecies Jari = Species(214, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(30, 215),
            (1, Tackle), (1, TailWag), (5, RockThrow), (10, PebbleShot), (15, SandSmoke), (20, RockSkin), (26, Boulder),
            (34, RockHeadbutt));
        public static readonly StorySpecies Ganpeki = Species(215, GrowthRate.MediumFast, 175, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, TailWag), (5, RockThrow), (10, PebbleShot), (15, SandSmoke), (20, RockSkin), (26, Boulder),
            (30, RockHeadbutt), (38, CliffBreak));

        public static readonly StorySpecies Sasa = Species(216, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(28, 217),
            (1, MossPellet), (1, Growl), (6, Fluff), (11, SeedBarrage), (17, PoisonMoss), (23, MossWhip), (31, SporeCloud));
        public static readonly StorySpecies Kumazasa = Species(217, GrowthRate.MediumFast, 180, Town01Species.RareCatchRate, null,
            (1, MossPellet), (1, Growl), (6, Fluff), (11, SeedBarrage), (17, PoisonMoss), (23, MossWhip), (28, FlapDance),
            (36, RecklessTackle));

        public static readonly StorySpecies Tsubame = Species(218, GrowthRate.MediumFast, 56, Town01Species.CommonCatchRate, new Evolution(26, 219),
            (1, Peck), (1, TailWag), (6, BellySlide), (10, DoublePeck), (15, SkyDream), (21, PeckDrain), (29, FlapAttack));
        public static readonly StorySpecies Hayabusa = Species(219, GrowthRate.MediumFast, 170, Town01Species.RareCatchRate, null,
            (1, Peck), (1, TailWag), (6, BellySlide), (10, DoublePeck), (15, SkyDream), (21, PeckDrain), (26, DiveBeak),
            (33, FlapAttack), (40, BraveDive));

        public static readonly StorySpecies Hatahata = Species(220, GrowthRate.MediumFast, 62, Town01Species.UncommonCatchRate, new Evolution(30, 221),
            (1, WaterGun), (1, Growl), (6, BubbleSpit), (11, Splash), (16, StaticShock), (22, SeaGun), (28, RainDance));
        public static readonly StorySpecies Kanburi = Species(221, GrowthRate.MediumFast, 185, Town01Species.RareCatchRate, null,
            (1, WaterGun), (1, Growl), (6, BubbleSpit), (11, Splash), (16, StaticShock), (22, SeaGun), (28, RainDance),
            (30, Whirlpool), (36, SparkBeak), (42, BigWave));

        public static readonly StorySpecies Arare = Species(222, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(30, 223),
            (1, PowderSnow), (1, Growl), (6, SnowPellet), (11, IcicleBarrage), (16, ColdBreath), (22, SnowCall), (28, IcicleBeak));
        public static readonly StorySpecies Hisame = Species(223, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, PowderSnow), (1, Growl), (6, SnowPellet), (11, IcicleBarrage), (16, ColdBreath), (22, SnowCall), (28, IcicleBeak),
            (30, FrostKiss), (38, BlizzardBreath));

        public static readonly StorySpecies Teiden = Species(224, GrowthRate.MediumFast, 62, Town01Species.UncommonCatchRate, new Evolution(30, 225),
            (1, Pester), (1, Growl), (6, Sneak), (11, DarkWhisper), (16, StaticShock), (21, ShadowStitch), (27, LifeSap));
        public static readonly StorySpecies Kurayami = Species(225, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, Pester), (1, Growl), (6, Sneak), (11, DarkWhisper), (16, StaticShock), (21, ShadowStitch), (27, LifeSap),
            (30, PolarNight), (36, Scheme), (44, SparkWing));

        public static readonly StorySpecies Yukiakari = Species(226, GrowthRate.MediumFast, 64, Town01Species.MiddleCatchRate, new Evolution(32, 227),
            (1, Twinkle), (1, Growl), (7, QuietPrayer), (12, ShootingStar), (18, AuroraHeal), (25, ColdBreath), (31, AuroraWave));
        public static readonly StorySpecies Ginga = Species(227, GrowthRate.MediumFast, 200, Town01Species.RareCatchRate, null,
            (1, Twinkle), (1, Growl), (7, QuietPrayer), (12, ShootingStar), (18, AuroraHeal), (25, ColdBreath), (31, AuroraWave),
            (32, SparkleLaser), (40, AuroraDance));

        public static readonly StorySpecies Yukigutsu = Species(228, GrowthRate.MediumFast, 55, Town01Species.CommonCatchRate, new Evolution(22, 229),
            (1, Tackle), (1, TailWag), (5, BellySlide), (10, IceStep), (15, Fluff), (20, FlipperSlap), (27, PenguinPress));
        public static readonly StorySpecies Kanjiki = Species(229, GrowthRate.MediumFast, 165, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, TailWag), (5, BellySlide), (10, IceStep), (15, Fluff), (20, FlipperSlap), (22, SnowPellet),
            (28, PenguinPress), (36, RecklessTackle));

        public static readonly StorySpecies Densen = Species(230, GrowthRate.MediumFast, 58, Town01Species.UncommonCatchRate, new Evolution(30, 231),
            (1, Peck), (1, StaticShock), (6, Growl), (11, Whirlwind), (16, ZapJab), (22, SparkBeak), (28, WindCutter));
        public static readonly StorySpecies Tettou = Species(231, GrowthRate.MediumFast, 200, Town01Species.RareCatchRate, null,
            (1, Peck), (1, StaticShock), (6, Growl), (11, Whirlwind), (16, ZapJab), (22, SparkBeak), (28, WindCutter),
            (30, Discharge), (36, NorthWindCall), (42, StormDrop));

        public static readonly StorySpecies Yukiishi = Species(232, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(32, 233),
            (1, Tackle), (1, PowderSnow), (5, RockThrow), (10, SnowPellet), (15, RockSkin), (21, IcicleBarrage), (27, Boulder));
        public static readonly StorySpecies Hyouheki = Species(233, GrowthRate.MediumFast, 180, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, PowderSnow), (5, RockThrow), (10, SnowPellet), (15, RockSkin), (21, IcicleBarrage), (27, Boulder),
            (32, IcebergDrop), (40, CliffBreak));

        public static readonly StorySpecies Kairo = Species(234, GrowthRate.MediumFast, 62, Town01Species.MiddleCatchRate, new Evolution(30, 235),
            (1, Ember), (1, Growl), (6, Fluff), (11, WispFlame), (17, FlareUp), (23, FlameDive), (29, CampfireBreath));
        public static readonly StorySpecies Danro = Species(235, GrowthRate.MediumFast, 185, Town01Species.RareCatchRate, null,
            (1, Ember), (1, Growl), (6, Fluff), (11, WispFlame), (17, FlareUp), (23, FlameDive), (29, CampfireBreath),
            (30, BraceUp), (36, FlameRush), (44, Eruption));

        // 道場主の切り札。Lv26 で「痺れの波動・充電ダンス・ビリビリ羽・放電スパーク」になるように並べる
        public static readonly StorySpecies Hatsuden = Species(236, GrowthRate.MediumSlow, 70, Town01Species.RareCatchRate, new Evolution(32, 237),
            (1, StaticShock), (1, Growl), (5, ZapJab), (9, NumbWave), (13, ChargeDance), (18, SparkWing), (24, Discharge),
            (30, StormDrop));
        public static readonly StorySpecies Raijin = Species(237, GrowthRate.MediumSlow, 235, Town01Species.RareCatchRate, null,
            (1, StaticShock), (1, Growl), (5, ZapJab), (9, NumbWave), (13, ChargeDance), (18, SparkWing), (24, Discharge),
            (30, StormDrop), (32, Shout), (40, QuietPrayer));

        // 町の人からもらう子。Lv20 で「火の粉・羽づくろい・羽でくるまる・叫び声」になる
        public static readonly StorySpecies Kantera = Species(238, GrowthRate.MediumFast, 165, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, Growl), (5, Ember), (10, Preen), (15, FeatherWrap), (20, Shout), (26, FlareUp), (32, CampfireBreath));

        // 発電所に居座る子。Lv24 で「電光ジャブ・鋭い目つき・稲妻クチバシ・放電スパーク」になる
        public static readonly StorySpecies Hesotori = Species(239, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, StaticShock), (1, Pester), (8, ZapJab), (14, SharpGlare), (19, SparkBeak), (24, Discharge), (30, OrcaBite),
            (38, VoltRush));

        public static readonly StorySpecies SaintElmo = Species(240, GrowthRate.MediumSlow, 180, Town01Species.VeryRareCatchRate, null,
            (1, Twinkle), (1, StaticShock), (8, ChargeDance), (14, ShootingStar), (20, SparkWing), (26, AuroraWave), (32, Discharge),
            (38, SparkleLaser), (44, AuroraDance));

        public static readonly IReadOnlyList<StorySpecies> All = new[]
        {
            Mameden, Gorogoro, Narukami, Jari, Ganpeki, Sasa, Kumazasa, Tsubame, Hayabusa, Hatahata, Kanburi, Arare, Hisame,
            Teiden, Kurayami, Yukiakari, Ginga, Yukigutsu, Kanjiki, Densen, Tettou, Yukiishi, Hyouheki, Kairo, Danro,
            Hatsuden, Raijin, Kantera, Hesotori, SaintElmo,
        };

        // All より後ろに置く（静的フィールドは上から順に初期化されるため、All が先に入っている必要がある）
        private static readonly Dictionary<int, StorySpecies> ByNumber = StorySpecies.IndexByNo(All);

        /// <summary>No から種を引く。無ければ null</summary>
        public static StorySpecies ByNo(int no) => ByNumber.TryGetValue(no, out var species) ? species : null;

        // 1行を短くするための別名。種族値・タイプ・特性は MonsterTable.Story から引く
        private static StorySpecies Species(int no, GrowthRate growthRate, int baseExp, int catchRate, Evolution evolution,
            params (int Level, MoveDefinition Move)[] learnset)
        {
            var moves = LevelMove.FromTable(learnset);
            return new StorySpecies(MonsterTable.ByNo(no), growthRate, baseExp, moves, evolution, catchRate);
        }
    }
}
