using System.Collections.Generic;
using PenguinMonster.Battle;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 種（No.241〜270）。作りは Town03Species と同じ。
    /// 7番道路・大橋の野生は Lv22〜28 なので、進化前の姿で出て、2段の子は Lv30〜34 で進化するように並べる
    /// </summary>
    public static class Town04Species
    {
        // 捕まえやすさの値は Town01Species と同じ

        public static readonly StorySpecies Soyokaze = Species(241, GrowthRate.MediumFast, 56, Town01Species.CommonCatchRate, new Evolution(25, 242),
            (1, Peck), (1, Growl), (5, BellySlide), (9, BlankStare), (13, DoublePeck), (17, Whirlwind), (21, Headwind),
            (29, PeckDrain));
        public static readonly StorySpecies Oikaze = Species(242, GrowthRate.MediumFast, 135, Town01Species.MiddleCatchRate, new Evolution(38, 243),
            (1, Peck), (1, Growl), (5, BellySlide), (9, BlankStare), (13, DoublePeck), (17, Whirlwind), (21, Headwind),
            (25, WindCutter), (31, PeckDrain), (37, NorthWindCall));
        public static readonly StorySpecies Kogarashi = Species(243, GrowthRate.MediumFast, 225, Town01Species.RareCatchRate, null,
            (1, Peck), (1, Growl), (5, BellySlide), (9, BlankStare), (13, DoublePeck), (17, Whirlwind), (21, Headwind),
            (25, WindCutter), (31, PeckDrain), (37, NorthWindCall), (38, GaleFeather), (46, BraveDive));

        public static readonly StorySpecies Mizore = Species(244, GrowthRate.MediumFast, 62, Town01Species.UncommonCatchRate, new Evolution(32, 245),
            (1, PowderSnow), (1, Growl), (6, SnowPellet), (11, ColdBreath), (16, IceStep), (22, IcicleBarrage), (28, IcicleBeak));
        public static readonly StorySpecies Muhyou = Species(245, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, PowderSnow), (1, Growl), (6, SnowPellet), (11, ColdBreath), (16, IceStep), (22, IcicleBarrage), (28, IcicleBeak),
            (32, FrostKiss), (40, BlizzardBreath));

        public static readonly StorySpecies Gareba = Species(246, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(32, 247),
            (1, Tackle), (1, TailWag), (5, PebbleShot), (10, RockThrow), (15, SandSmoke), (21, RockSkin), (27, Boulder));
        public static readonly StorySpecies Dangai = Species(247, GrowthRate.MediumFast, 185, Town01Species.RareCatchRate, null,
            (1, Tackle), (1, TailWag), (5, PebbleShot), (10, RockThrow), (15, SandSmoke), (21, RockSkin), (27, Boulder),
            (32, CliffBreak), (40, RockHeadbutt));

        public static readonly StorySpecies Haze = Species(248, GrowthRate.MediumFast, 58, Town01Species.CommonCatchRate, new Evolution(30, 249),
            (1, WaterGun), (1, TailWag), (5, DiveJet), (10, BubbleSpit), (15, Splash), (21, SprayBurst), (26, SeaGun));
        public static readonly StorySpecies Suzuki = Species(249, GrowthRate.MediumFast, 185, Town01Species.RareCatchRate, null,
            (1, WaterGun), (1, TailWag), (5, DiveJet), (10, BubbleSpit), (15, Splash), (21, SprayBurst), (26, SeaGun),
            (30, Whirlpool), (36, RainDance), (42, RecklessTackle));

        public static readonly StorySpecies Komai = Species(250, GrowthRate.MediumFast, 62, Town01Species.UncommonCatchRate, new Evolution(32, 251),
            (1, WaterGun), (1, PowderSnow), (6, Growl), (11, BubbleSpit), (16, ColdBreath), (22, SeaGun), (28, IcicleBeak));
        public static readonly StorySpecies Madara = Species(251, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, WaterGun), (1, PowderSnow), (6, Growl), (11, BubbleSpit), (16, ColdBreath), (22, SeaGun), (28, IcicleBeak),
            (32, BigWave), (40, BlizzardBreath));

        public static readonly StorySpecies Yomogi = Species(252, GrowthRate.MediumFast, 60, Town01Species.UncommonCatchRate, new Evolution(30, 253),
            (1, MossPellet), (1, Growl), (6, Fluff), (11, SeedBarrage), (16, PoisonMoss), (22, MossDrain), (27, MossArmor));
        public static readonly StorySpecies Susuki = Species(253, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, MossPellet), (1, Growl), (6, Fluff), (11, SeedBarrage), (16, PoisonMoss), (22, MossDrain), (27, MossArmor),
            (30, Whirlwind), (36, SporeCloud), (42, MossCannon));

        public static readonly StorySpecies Fuusen = Species(254, GrowthRate.MediumFast, 55, Town01Species.CommonCatchRate, new Evolution(30, 255),
            (1, Pound), (1, Growl), (5, Fluff), (9, BlankStare), (14, ShrillVoice), (19, Whirlwind), (24, Shout));
        public static readonly StorySpecies Kikyuu = Species(255, GrowthRate.MediumFast, 175, Town01Species.RareCatchRate, null,
            (1, Pound), (1, Growl), (5, Fluff), (9, BlankStare), (14, ShrillVoice), (19, Whirlwind), (24, Shout),
            (30, FeatherWrap), (37, SkyDream));

        public static readonly StorySpecies Yokaze = Species(256, GrowthRate.MediumFast, 62, Town01Species.UncommonCatchRate, new Evolution(32, 257),
            (1, Pester), (1, Growl), (6, Sneak), (11, Whirlwind), (16, DarkWhisper), (21, SeeThrough), (27, ShadowStitch));
        public static readonly StorySpecies Yoarashi = Species(257, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, Pester), (1, Growl), (6, Sneak), (11, Whirlwind), (16, DarkWhisper), (21, SeeThrough), (27, ShadowStitch),
            (32, PolarNight), (38, WindCutter), (44, Scheme));

        public static readonly StorySpecies Hiuchi = Species(258, GrowthRate.MediumFast, 62, Town01Species.MiddleCatchRate, new Evolution(32, 259),
            (1, Ember), (1, TailWag), (5, RockThrow), (10, WispFlame), (15, FlareUp), (20, SandSmoke), (26, FlameDive));
        public static readonly StorySpecies Noroshi = Species(259, GrowthRate.MediumFast, 190, Town01Species.RareCatchRate, null,
            (1, Ember), (1, TailWag), (5, RockThrow), (10, WispFlame), (15, FlareUp), (20, SandSmoke), (26, FlameDive),
            (32, CampfireBreath), (40, FlameRush));

        public static readonly StorySpecies Umineko = Species(260, GrowthRate.MediumFast, 58, Town01Species.CommonCatchRate, new Evolution(30, 261),
            (1, Peck), (1, Growl), (5, BellySlide), (9, ShrillVoice), (14, DoublePeck), (19, Headwind), (25, FlapAttack));
        public static readonly StorySpecies Ookamome = Species(261, GrowthRate.MediumFast, 175, Town01Species.RareCatchRate, null,
            (1, Peck), (1, Growl), (5, BellySlide), (9, ShrillVoice), (14, DoublePeck), (19, Headwind), (25, FlapAttack),
            (30, DiveBeak), (36, PenguinPress), (42, BraveDive));

        public static readonly StorySpecies Shabon = Species(262, GrowthRate.MediumFast, 64, Town01Species.MiddleCatchRate, new Evolution(34, 263),
            (1, Twinkle), (1, Growl), (7, BubbleSpit), (12, QuietPrayer), (17, ShootingStar), (23, AuroraHeal), (29, AuroraWave));
        public static readonly StorySpecies Fuurin = Species(263, GrowthRate.MediumFast, 200, Town01Species.RareCatchRate, null,
            (1, Twinkle), (1, Growl), (7, BubbleSpit), (12, QuietPrayer), (17, ShootingStar), (23, AuroraHeal), (29, AuroraWave),
            (34, ShrillVoice), (40, SparkleLaser), (46, AuroraDance));

        public static readonly StorySpecies Kazaguruma = Species(264, GrowthRate.MediumFast, 62, Town01Species.UncommonCatchRate, new Evolution(32, 265),
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, Whirlwind), (16, ChargeDance), (21, SparkWing), (27, WindCutter));
        public static readonly StorySpecies Fuusha = Species(265, GrowthRate.MediumFast, 200, Town01Species.RareCatchRate, null,
            (1, StaticShock), (1, Growl), (6, ZapJab), (11, Whirlwind), (16, ChargeDance), (21, SparkWing), (27, WindCutter),
            (32, Discharge), (38, NorthWindCall), (44, StormDrop));

        // 道場主の切り札。Lv32 で「向かい風・羽ばたきアタック・北風の号令・風切り」になるように並べる
        public static readonly StorySpecies Tsumuji = Species(266, GrowthRate.MediumSlow, 70, Town01Species.RareCatchRate, new Evolution(34, 267),
            (1, Peck), (1, Growl), (5, Whirlwind), (9, BlankStare), (14, Headwind), (20, FlapAttack), (26, NorthWindCall),
            (31, WindCutter));
        public static readonly StorySpecies Tatsumaki = Species(267, GrowthRate.MediumSlow, 235, Town01Species.RareCatchRate, null,
            (1, Peck), (1, Growl), (5, Whirlwind), (9, BlankStare), (14, Headwind), (20, FlapAttack), (26, NorthWindCall),
            (31, WindCutter), (34, GaleFeather), (42, BraveDive));

        public static readonly StorySpecies Hiraishin = Species(268, GrowthRate.MediumSlow, 180, Town01Species.VeryRareCatchRate, null,
            (1, Peck), (1, StaticShock), (8, ZapJab), (14, Whirlwind), (20, SparkBeak), (26, FlapAttack), (32, Discharge),
            (38, DiveBeak), (44, VoltRush));

        public static readonly StorySpecies Oonamazu = Species(269, GrowthRate.MediumSlow, 180, Town01Species.VeryRareCatchRate, null,
            (1, WaterGun), (1, Growl), (8, BubbleSpit), (14, Splash), (20, SeaGun), (26, RainDance), (32, Whirlpool),
            (38, Boulder), (44, BigWave));

        // 隠しコマンドの子。Lv30 で「オーロラの癒し・向かい風・オーロラウェーブ・風切り」になる
        public static readonly StorySpecies Nijibane = Species(270, GrowthRate.MediumSlow, 200, Town01Species.VeryRareCatchRate, null,
            (1, Peck), (1, Twinkle), (8, Whirlwind), (14, ShootingStar), (20, AuroraHeal), (24, Headwind), (27, AuroraWave),
            (30, WindCutter), (38, SparkleLaser), (46, GaleFeather));

        public static readonly IReadOnlyList<StorySpecies> All = new[]
        {
            Soyokaze, Oikaze, Kogarashi, Mizore, Muhyou, Gareba, Dangai, Haze, Suzuki, Komai, Madara, Yomogi, Susuki,
            Fuusen, Kikyuu, Yokaze, Yoarashi, Hiuchi, Noroshi, Umineko, Ookamome, Shabon, Fuurin, Kazaguruma, Fuusha,
            Tsumuji, Tatsumaki, Hiraishin, Oonamazu, Nijibane,
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
