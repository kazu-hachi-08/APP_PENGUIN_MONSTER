using System;
using System.Collections.Generic;
using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモンの定義表（段階2の50体。仕様書 §3.5）。こおりは主役なので一番多く、他は各6〜8体（2タイプも数える）。
    /// 種族値の合計はランクごとの帯（ヒナ 360〜400 / ふつう 425〜475 / エース 520〜540 / 伝説 580〜600）にそろえ、
    /// 同じランクの中の差は技とタイプで付ける。特性・持ち物は1体1つずつ（仕様書 §4.6）
    /// </summary>
    public static class MonsterTable
    {
        public static readonly IReadOnlyList<MonsterDefinition> All = new[]
        {
            Monster(1, "ユキペン", new[] { Ice }, new Stats(75, 80, 70, 70, 70, 80),
                new[] { IcicleBeak, Boulder, SnowPellet, FlapDance }, Ability.LastStand, HeldItem.SharpGlasses),
            Monster(2, "ナミペン", new[] { Sea }, new Stats(85, 70, 80, 85, 80, 55),
                new[] { BigWave, BubbleSpit, BlizzardBreath, Splash }, Ability.ThickFat, HeldItem.StarFish),
            Monster(3, "ヒノペン", new[] { Fire }, new Stats(65, 85, 60, 90, 65, 90),
                new[] { FlameDive, CampfireBreath, SparkBeak, FlareUp }, Ability.LastStand, HeldItem.LifeIcicle),
            Monster(4, "イナペン", new[] { Thunder, Bird }, new Stats(60, 70, 60, 95, 70, 100),
                new[] { SparkWing, WindCutter, NumbWave, BellySlide }, Ability.StaticBody, HeldItem.LightFeather),
            Monster(5, "コケペン", new[] { Moss, Rock }, new Stats(90, 85, 95, 55, 75, 40),
                new[] { MossWhip, Boulder, SleepMoss, Tackle }, Ability.SpikySkin, HeldItem.StarFish),
            Monster(6, "イワペン", new[] { Rock }, new Stats(80, 100, 100, 40, 60, 45),
                new[] { Boulder, CliffBreak, RockSkin, IcicleBeak }, Ability.HardFeathers, HeldItem.GritScarf),
            Monster(7, "ヒョウガペン", new[] { Ice, Rock }, new Stats(95, 95, 100, 50, 70, 40),
                new[] { IcebergDrop, Boulder, RockSkin, SnowPellet }, Ability.SpikySkin, HeldItem.SnowCoat),
            Monster(8, "ホシペン", new[] { Ice, Aurora }, new Stats(70, 50, 65, 95, 75, 95),
                new[] { BlizzardBreath, AuroraWave, ColdBreath, QuietPrayer }, Ability.Tailwind, HeldItem.WiseFeather),
            Monster(9, "コウテイペン", new[] { Ice, MonsterType.Normal }, new Stats(100, 90, 80, 75, 80, 50),
                new[] { IcicleBeak, PenguinPress, IcebergDrop, Fluff }, Ability.Intimidate, HeldItem.SquidSnack),
            Monster(10, "リュウヒョウペン", new[] { Ice, Sea }, new Stats(80, 70, 85, 85, 80, 55),
                new[] { BlizzardBreath, BigWave, ColdBreath, Splash }, Ability.HardFeathers, HeldItem.FreshKelp),
            Monster(11, "ペンタ", new[] { MonsterType.Normal }, new Stats(75, 75, 75, 75, 75, 75),
                new[] { PenguinPress, OrcaBite, BellySlide, FlapDance }, Ability.Momentum, HeldItem.BraveFeather),
            Monster(12, "モフペン", new[] { MonsterType.Normal }, new Stats(125, 65, 75, 60, 85, 35),
                new[] { PenguinPress, Shout, CutePose, Fluff }, Ability.ThickFat, HeldItem.StarFish),
            Monster(13, "ウズペン", new[] { Sea }, new Stats(80, 95, 75, 60, 70, 80),
                new[] { Whirlpool, SeaGun, Sneak, FlapDance }, Ability.Momentum, HeldItem.LifeIcicle),
            Monster(14, "サンゴペン", new[] { Sea, Moss }, new Stats(75, 55, 75, 90, 95, 60),
                new[] { BubbleSpit, MossCannon, PoisonMoss, BigWave }, Ability.PoisonSlime, HeldItem.FreshKelp),
            Monster(15, "カザンペン", new[] { Fire, Rock }, new Stats(95, 100, 85, 60, 60, 50),
                new[] { FlameDive, CliffBreak, RockSkin, WispFlame }, Ability.Intimidate, HeldItem.BraveFeather),
            Monster(16, "タキビペン", new[] { Fire }, new Stats(70, 50, 65, 110, 80, 85),
                new[] { Eruption, CampfireBreath, WispFlame, FlareUp }, Ability.LastStand, HeldItem.WiseFeather),
            Monster(17, "ライペン", new[] { Thunder }, new Stats(70, 95, 65, 65, 65, 100),
                new[] { SparkBeak, BellySlide, FlapAttack, NumbWave }, Ability.StaticBody, HeldItem.GritScarf),
            Monster(18, "アラシペン", new[] { Thunder }, new Stats(90, 60, 80, 100, 85, 45),
                new[] { StormDrop, BigWave, SparkWing, NumbWave }, Ability.NightOwl, HeldItem.SquidSnack),
            Monster(19, "ノリペン", new[] { Moss }, new Stats(85, 60, 80, 85, 90, 60),
                new[] { SlimySeaweed, MossCannon, ToxicMoss, Fluff }, Ability.PoisonSlime, HeldItem.StarFish),
            Monster(20, "ツバサペン", new[] { Bird }, new Stats(65, 95, 60, 60, 60, 110),
                new[] { FlapAttack, DiveBeak, SkyDream, BellySlide }, Ability.Tailwind, HeldItem.LifeIcicle),
            Monster(21, "カゼペン", new[] { Bird }, new Stats(70, 55, 65, 100, 75, 95),
                new[] { WindCutter, Whirlwind, SparkWing, SkyDream }, Ability.HardFeathers, HeldItem.SharpGlasses),
            Monster(22, "シャチペン", new[] { Dark }, new Stats(90, 110, 80, 50, 65, 70),
                new[] { OrcaBite, Sneak, Whirlpool, SharpGlare }, Ability.Intimidate, HeldItem.LightFeather),
            Monster(23, "ヨルペン", new[] { Dark }, new Stats(65, 60, 65, 105, 80, 90),
                new[] { PolarNight, AuroraWave, DarkWhisper, CampfireBreath }, Ability.NightOwl, HeldItem.WiseFeather),
            Monster(24, "オーロペン", new[] { Aurora }, new Stats(80, 55, 75, 105, 95, 65),
                new[] { SparkleLaser, AuroraWave, QuietPrayer, ColdBreath }, Ability.ThickFat, HeldItem.SnowCoat),

            // ---- 段階2: ふつう（25〜36）。闇・オーロラ・火・苔など少なかったタイプを埋める ----
            Monster(25, "ツララペン", new[] { Ice }, new Stats(70, 55, 70, 100, 80, 85),
                new[] { BlizzardBreath, ColdBreath, AuroraWave, QuietPrayer }, Ability.LastStand, HeldItem.WiseFeather),
            Monster(26, "カマクラペン", new[] { Ice, Fire }, new Stats(85, 70, 85, 85, 80, 55),
                new[] { BlizzardBreath, CampfireBreath, WispFlame, Fluff }, Ability.ThickFat, HeldItem.StarFish),
            Monster(27, "オンセンペン", new[] { Fire, Sea }, new Stats(90, 60, 80, 90, 90, 55),
                new[] { HotSpring, CampfireBreath, WispFlame, QuietPrayer }, Ability.NightOwl, HeldItem.FreshKelp),
            Monster(28, "ハナビペン", new[] { Fire, Aurora }, new Stats(65, 70, 60, 105, 70, 95),
                new[] { Eruption, SparkleLaser, SparkWing, FlareUp }, Ability.LastStand, HeldItem.LifeIcicle),
            Monster(29, "キノコペン", new[] { Moss, Dark }, new Stats(85, 85, 80, 60, 80, 60),
                new[] { MossWhip, OrcaBite, SleepMoss, PoisonMoss }, Ability.PoisonSlime, HeldItem.SquidSnack),
            Monster(30, "ツタペン", new[] { Moss, Bird }, new Stats(70, 80, 65, 75, 70, 95),
                new[] { MossWhip, DiveBeak, BellySlide, SkyDream }, Ability.Momentum, HeldItem.BraveFeather),
            Monster(31, "マジシャンペン", new[] { MonsterType.Normal, Aurora }, new Stats(70, 60, 70, 100, 90, 75),
                new[] { Shout, SparkleLaser, DarkWhisper, QuietPrayer }, Ability.HardFeathers, HeldItem.SharpGlasses),
            Monster(32, "ボクサーペン", new[] { MonsterType.Normal }, new Stats(75, 105, 70, 45, 65, 95),
                new[] { PenguinPress, BellySlide, SparkBeak, FlapDance }, Ability.Intimidate, HeldItem.GritScarf),
            Monster(33, "シンジュペン", new[] { Sea, Aurora }, new Stats(80, 50, 85, 95, 100, 60),
                new[] { BigWave, SparkleLaser, ColdBreath, QuietPrayer }, Ability.SpikySkin, HeldItem.SnowCoat),
            Monster(34, "イルカペン", new[] { Sea }, new Stats(75, 90, 70, 65, 65, 95),
                new[] { Whirlpool, SnowPellet, DiveBeak, FlapDance }, Ability.HardFeathers, HeldItem.LightFeather),
            Monster(35, "コウモリペン", new[] { Dark, Bird }, new Stats(70, 85, 65, 70, 65, 105),
                new[] { OrcaBite, FlapAttack, DarkWhisper, Sneak }, Ability.NightOwl, HeldItem.BraveFeather),
            Monster(36, "ホタルペン", new[] { Thunder, Moss }, new Stats(70, 55, 70, 95, 85, 80),
                new[] { SparkWing, MossCannon, SleepMoss, NumbWave }, Ability.StaticBody, HeldItem.StarFish),

            // ---- ヒナ（37〜42）。種族値は低いが、特性・持ち物・技と編成しだいで化ける ----
            Monster(37, "ヒナペン", new[] { MonsterType.Normal }, new Stats(70, 60, 55, 40, 55, 80),
                new[] { PenguinPress, BellySlide, IcicleBeak, FlapDance }, Ability.HugePower, HeldItem.GritScarf, MonsterRank.Hina),
            Monster(38, "ユキヒナペン", new[] { Ice }, new Stats(85, 50, 75, 60, 75, 40),
                new[] { ColdBreath, ToxicMoss, Fluff, FeatherRelay }, Ability.ThickFat, HeldItem.ChickDown, MonsterRank.Hina),
            Monster(39, "ハネヒナペン", new[] { Bird }, new Stats(50, 55, 50, 50, 50, 115),
                new[] { WindCutter, SkyDream, FlapDance, FeatherRelay }, Ability.Tailwind, HeldItem.ChickDown, MonsterRank.Hina),
            Monster(40, "ナキムシペン", new[] { Dark }, new Stats(65, 90, 60, 50, 60, 75),
                new[] { OrcaBite, Sneak, FlameDive, IcicleBeak }, Ability.Avenger, HeldItem.LifeIcicle, MonsterRank.Hina),
            Monster(41, "カラヒナペン", new[] { Rock }, new Stats(85, 40, 95, 40, 95, 25),
                new[] { Boulder, ToxicMoss, RockSkin, FeatherRelay }, Ability.SpikySkin, HeldItem.ChickDown, MonsterRank.Hina),
            Monster(42, "ピリヒナペン", new[] { Thunder }, new Stats(60, 45, 60, 80, 65, 80),
                new[] { SparkWing, StormDrop, AuroraWave, NumbWave }, Ability.Avenger, HeldItem.ChickDown, MonsterRank.Hina),

            // ---- エース（43〜47）。純粋に強いが、4倍などはっきりした弱点がある ----
            Monster(43, "サメペン", new[] { Sea, Dark }, new Stats(95, 115, 85, 60, 75, 95),
                new[] { Whirlpool, OrcaBite, CliffBreak, FlapDance }, Ability.SpikySkin, HeldItem.SquidSnack, MonsterRank.Ace),
            Monster(44, "ヒノトリペン", new[] { Fire, Bird }, new Stats(85, 110, 75, 95, 80, 90),
                new[] { FlameDive, DiveBeak, Eruption, FlapDance }, Ability.LastStand, HeldItem.LifeIcicle, MonsterRank.Ace),
            Monster(45, "ユキヒョウペン", new[] { Ice, Dark }, new Stats(80, 115, 75, 60, 75, 115),
                new[] { IcicleBeak, OrcaBite, SnowPellet, SharpGlare }, Ability.Momentum, HeldItem.BraveFeather, MonsterRank.Ace),
            Monster(46, "ヒカリペン", new[] { Aurora, Moss }, new Stats(95, 60, 90, 115, 105, 70),
                new[] { SparkleLaser, MossCannon, SleepMoss, QuietPrayer }, Ability.HardFeathers, HeldItem.StarFish, MonsterRank.Ace),
            Monster(47, "ライガンペン", new[] { Thunder, Rock }, new Stats(100, 115, 105, 55, 75, 75),
                new[] { SparkBeak, CliffBreak, IcicleBeak, RockSkin }, Ability.Intimidate, HeldItem.SnowCoat, MonsterRank.Ace),

            // ---- 伝説（48〜50）。ボスの切り札で、専用の技を持つ ----
            Monster(48, "ナンキョクペン", new[] { Ice }, new Stats(110, 125, 105, 90, 90, 80),
                new[] { GreatGlacier, CliffBreak, SnowPellet, FlapDance }, Ability.ThickFat, HeldItem.SquidSnack, MonsterRank.Legend),
            Monster(49, "ライテイペン", new[] { Thunder, Bird }, new Stats(105, 80, 80, 110, 95, 110),
                new[] { HeavenlyThunder, WindCutter, AuroraWave, QuietPrayer }, Ability.HardFeathers, HeldItem.WiseFeather,
                MonsterRank.Legend),
            Monster(50, "キョクヤペン", new[] { Dark, Aurora }, new Stats(105, 110, 90, 110, 90, 85),
                new[] { EndlessNight, OrcaBite, SparkleLaser, CampfireBreath }, Ability.Intimidate, HeldItem.SharpGlasses,
                MonsterRank.Legend),
        };

        /// <summary>No から引く。オンラインでは相手のチームを No の列で受け取るため</summary>
        public static MonsterDefinition ByNo(int no)
        {
            foreach (var monster in All)
            {
                if (monster.No == no)
                {
                    return monster;
                }
            }
            throw new ArgumentException($"No.{no} のペンモンはいない", nameof(no));
        }

        /// <summary>ずかんの しぼりこみ。type が null なら全員。2タイプのペンモンはどちらのタイプでも出る</summary>
        public static IReadOnlyList<MonsterDefinition> WithType(MonsterType? type)
        {
            if (type == null)
            {
                return All;
            }
            var result = new List<MonsterDefinition>();
            foreach (var monster in All)
            {
                if (monster.HasType(type.Value))
                {
                    result.Add(monster);
                }
            }
            return result;
        }

        // 1行を短くするための別名。並びは MonsterDefinition のコンストラクタと同じ
        private static MonsterDefinition Monster(int no, string name, MonsterType[] types, Stats stats, MoveDefinition[] moves,
            Ability ability, HeldItem item, MonsterRank rank = MonsterRank.Normal)
            => new MonsterDefinition(no, name, types, stats, moves, ability, item, rank);
    }
}
