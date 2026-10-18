using System.Collections.Generic;
using PenguinMonster.Battle;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 最初の章の種（No.151〜180）。種族値・タイプ・特性は MonsterTable.Story に持ち、
    /// ここは育成の情報（覚える技・進化・経験値・捕まえやすさ）を足す。
    /// 進化前は同じ技を進化後より遅く覚え、進化後は進化したレベルでその段の技を1つ覚える（本家と同じ形）
    /// </summary>
    public static class Town01Species
    {
        // 捕まえやすさ（本家に合わせる）: 最初の3体・最後の進化 45 / 序盤のよくいる子 255 / 少し珍しい子 190 / 途中の段 120 / とてもまれ 30
        internal const int RareCatchRate = 45;
        internal const int CommonCatchRate = 255;
        internal const int UncommonCatchRate = 190;
        internal const int MiddleCatchRate = 120;
        internal const int VeryRareCatchRate = 30;

        public static readonly StorySpecies Kobune = Species(151, GrowthRate.MediumSlow, 62, RareCatchRate, new Evolution(16, 152),
            (1, Tackle), (1, Growl), (5, WaterGun), (9, Splash), (13, BubbleSpit), (20, DiveJet), (26, SeaGun), (32, Fluff), (40, Whirlpool));
        public static readonly StorySpecies Hobashira = Species(152, GrowthRate.MediumSlow, 142, RareCatchRate, new Evolution(36, 153),
            (1, Tackle), (1, Growl), (5, WaterGun), (9, Splash), (13, BubbleSpit), (16, SeaGun), (22, DiveJet), (28, Fluff), (34, Whirlpool),
            (42, BigWave));
        public static readonly StorySpecies Saihyousen = Species(153, GrowthRate.MediumSlow, 239, RareCatchRate, null,
            (1, Tackle), (1, Growl), (5, WaterGun), (9, Splash), (13, BubbleSpit), (16, SeaGun), (22, DiveJet), (28, Fluff), (34, Whirlpool),
            (36, IcicleBeak), (42, BigWave), (48, BlizzardBreath), (54, IcebergDrop));

        public static readonly StorySpecies Hidane = Species(154, GrowthRate.MediumSlow, 62, RareCatchRate, new Evolution(16, 155),
            (1, Tackle), (1, Growl), (5, Ember), (9, TailWag), (13, WispFlame), (20, FlameDive), (26, FlareUp), (32, CampfireBreath));
        public static readonly StorySpecies Fuigo = Species(155, GrowthRate.MediumSlow, 142, RareCatchRate, new Evolution(36, 156),
            (1, Tackle), (1, Growl), (5, Ember), (9, TailWag), (13, WispFlame), (16, FlameDive), (22, FlareUp), (28, CampfireBreath),
            (40, Eruption));
        public static readonly StorySpecies Katanakaji = Species(156, GrowthRate.MediumSlow, 240, RareCatchRate, null,
            (1, Tackle), (1, Growl), (5, Ember), (9, TailWag), (13, WispFlame), (16, FlameDive), (22, FlareUp), (28, CampfireBreath),
            (36, Boulder), (42, Eruption), (48, FlameRush), (54, CliffBreak));

        public static readonly StorySpecies Mebuki = Species(157, GrowthRate.MediumSlow, 64, RareCatchRate, new Evolution(16, 158),
            (1, Tackle), (1, Growl), (5, MossPellet), (9, PoisonMoss), (13, SleepMoss), (20, MossWhip), (26, MossDrain), (32, MossArmor));
        public static readonly StorySpecies Fukinotou = Species(158, GrowthRate.MediumSlow, 142, RareCatchRate, new Evolution(36, 159),
            (1, Tackle), (1, Growl), (5, MossPellet), (9, PoisonMoss), (13, SleepMoss), (16, MossWhip), (22, MossDrain), (28, MossArmor),
            (34, MossCannon));
        public static readonly StorySpecies Yukitsubaki = Species(159, GrowthRate.MediumSlow, 236, RareCatchRate, null,
            (1, Tackle), (1, Growl), (5, MossPellet), (9, PoisonMoss), (13, SleepMoss), (16, MossWhip), (22, MossDrain), (28, MossArmor),
            (34, MossCannon), (36, AuroraWave), (44, SparkleLaser), (50, AuroraHeal));

        public static readonly StorySpecies Kogara = Species(160, GrowthRate.MediumFast, 50, CommonCatchRate, new Evolution(18, 161),
            (1, Peck), (1, Growl), (6, BellySlide), (11, DoublePeck), (16, SkyDream), (21, FlapAttack), (27, PeckDrain));
        public static readonly StorySpecies Tsugumi = Species(161, GrowthRate.MediumFast, 122, MiddleCatchRate, new Evolution(34, 162),
            (1, Peck), (1, Growl), (6, BellySlide), (11, DoublePeck), (16, SkyDream), (18, Whirlwind), (23, FlapAttack), (29, PeckDrain));
        public static readonly StorySpecies Oowashi = Species(162, GrowthRate.MediumFast, 216, RareCatchRate, null,
            (1, Peck), (1, Growl), (6, BellySlide), (11, DoublePeck), (16, SkyDream), (18, Whirlwind), (23, FlapAttack), (29, PeckDrain),
            (34, DiveBeak), (40, BraveDive), (46, GaleFeather));

        public static readonly StorySpecies Omochi = Species(163, GrowthRate.MediumFast, 55, CommonCatchRate, new Evolution(20, 164),
            (1, Pound), (1, Growl), (6, Fluff), (11, Tackle), (16, FlipperSlap), (22, Preen), (28, PenguinPress));
        public static readonly StorySpecies Kagamimochi = Species(164, GrowthRate.MediumFast, 160, RareCatchRate, null,
            (1, Pound), (1, Growl), (6, Fluff), (11, Tackle), (16, FlipperSlap), (20, BraceUp), (25, Preen), (32, PenguinPress),
            (40, RecklessTackle));

        public static readonly StorySpecies Konayuki = Species(165, GrowthRate.MediumFast, 56, UncommonCatchRate, new Evolution(18, 166),
            (1, PowderSnow), (1, Growl), (8, ColdBreath), (14, Fluff), (20, FrostKiss), (26, SnowCall));
        public static readonly StorySpecies Botanyuki = Species(166, GrowthRate.MediumFast, 130, MiddleCatchRate, new Evolution(32, 167),
            (1, PowderSnow), (1, Growl), (8, ColdBreath), (14, Fluff), (18, FrostKiss), (24, SnowCall), (30, BlizzardBreath));
        public static readonly StorySpecies Gousetsu = Species(167, GrowthRate.MediumFast, 230, RareCatchRate, null,
            (1, PowderSnow), (1, Growl), (8, ColdBreath), (14, Fluff), (18, FrostKiss), (24, SnowCall), (30, BlizzardBreath),
            (32, QuietPrayer), (40, AuroraWave));

        public static readonly StorySpecies Tsukushi = Species(168, GrowthRate.MediumFast, 56, UncommonCatchRate, new Evolution(22, 169),
            (1, Pound), (1, Growl), (5, MossPellet), (10, PoisonMoss), (15, BellySlide), (20, MossWhip), (26, SeedBarrage), (32, MossArmor));
        public static readonly StorySpecies Sugina = Species(169, GrowthRate.MediumFast, 165, RareCatchRate, null,
            (1, Pound), (1, Growl), (5, MossPellet), (10, PoisonMoss), (15, BellySlide), (20, MossWhip), (22, FlapDance), (28, SeedBarrage),
            (34, MossArmor));

        public static readonly StorySpecies Yamame = Species(170, GrowthRate.MediumFast, 60, UncommonCatchRate, new Evolution(22, 171),
            (1, WaterGun), (1, TailWag), (6, BellySlide), (11, DiveJet), (16, Splash), (21, SeaGun), (27, SprayBurst));
        public static readonly StorySpecies Sakuramasu = Species(171, GrowthRate.MediumFast, 170, RareCatchRate, null,
            (1, WaterGun), (1, TailWag), (6, BellySlide), (11, DiveJet), (16, Splash), (21, SeaGun), (22, RainDance), (28, SprayBurst),
            (34, Whirlpool));

        public static readonly StorySpecies Pachiri = Species(172, GrowthRate.MediumFast, 60, MiddleCatchRate, new Evolution(26, 173),
            (1, StaticShock), (1, Growl), (7, NumbWave), (12, ZapJab), (18, ChargeDance), (24, SparkWing));
        public static readonly StorySpecies Raichou = Species(173, GrowthRate.MediumFast, 175, RareCatchRate, null,
            (1, StaticShock), (1, Growl), (7, NumbWave), (12, ZapJab), (18, ChargeDance), (24, SparkWing), (26, WindCutter),
            (32, Discharge), (40, StormDrop));

        public static readonly StorySpecies Ishikoro = Species(174, GrowthRate.MediumFast, 60, UncommonCatchRate, new Evolution(24, 175),
            (1, Tackle), (1, Growl), (5, RockThrow), (10, Fluff), (15, PebbleShot), (20, SandSmoke), (26, RockSkin), (32, Boulder));
        public static readonly StorySpecies Ishidourou = Species(175, GrowthRate.MediumFast, 165, RareCatchRate, null,
            (1, Tackle), (1, Growl), (5, RockThrow), (10, Fluff), (15, PebbleShot), (20, SandSmoke), (24, Boulder), (30, RockSkin),
            (36, CliffBreak), (42, RockHeadbutt));

        public static readonly StorySpecies Warashi = Species(176, GrowthRate.MediumFast, 60, UncommonCatchRate, new Evolution(24, 177),
            (1, Pester), (1, Growl), (6, Twinkle), (11, DarkWhisper), (16, ShadowStitch), (22, LifeSap), (28, Scheme));
        public static readonly StorySpecies Zashikiwarashi = Species(177, GrowthRate.MediumFast, 170, RareCatchRate, null,
            (1, Pester), (1, Growl), (6, Twinkle), (11, DarkWhisper), (16, ShadowStitch), (22, LifeSap), (24, CutePose), (30, Scheme),
            (36, PolarNight));

        // 道場主の切り札。Lv14 で「岩投げ・膨らむ・砂けむり・岩肌」になるように並べる
        public static readonly StorySpecies Kawara = Species(178, GrowthRate.MediumSlow, 70, RareCatchRate, new Evolution(30, 179),
            (1, Tackle), (1, RockThrow), (5, Fluff), (9, SandSmoke), (13, RockSkin), (18, PebbleShot), (24, Boulder));
        public static readonly StorySpecies Onigawara = Species(179, GrowthRate.MediumSlow, 180, RareCatchRate, null,
            (1, Tackle), (1, RockThrow), (5, Fluff), (9, SandSmoke), (13, RockSkin), (18, PebbleShot), (24, Boulder), (30, CliffBreak),
            (36, BraceUp), (42, RockHeadbutt));

        public static readonly StorySpecies DiamondDust = Species(180, GrowthRate.MediumSlow, 180, VeryRareCatchRate, null,
            (1, Twinkle), (1, PowderSnow), (8, ColdBreath), (14, QuietPrayer), (20, FrostKiss), (26, AuroraWave), (32, AuroraHeal),
            (38, SparkleLaser), (44, AuroraDance));

        public static readonly IReadOnlyList<StorySpecies> All = new[]
        {
            Kobune, Hobashira, Saihyousen, Hidane, Fuigo, Katanakaji, Mebuki, Fukinotou, Yukitsubaki,
            Kogara, Tsugumi, Oowashi, Omochi, Kagamimochi, Konayuki, Botanyuki, Gousetsu, Tsukushi, Sugina,
            Yamame, Sakuramasu, Pachiri, Raichou, Ishikoro, Ishidourou, Warashi, Zashikiwarashi, Kawara, Onigawara, DiamondDust,
        };

        // All より後ろに置く（静的フィールドは上から順に初期化されるため、All が先に入っている必要がある）
        private static readonly Dictionary<int, StorySpecies> ByNumber = StorySpecies.IndexByNo(All);

        /// <summary>No から種を引く（進化先・記録を戻すのに使う）。無ければ null</summary>
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
