using System;
using System.Collections.Generic;
using static PenguinMonster.Battle.BodyColor;
using static PenguinMonster.Battle.BodyShape;
using static PenguinMonster.Battle.MonsterType;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモンの定義表（段階1の24体。仕様書 §3.5）。こおりは主役なので多め、他は各2〜3体（2タイプも数える）。
    /// 種族値の合計は 440〜475 にそろえ、強さの差は技とタイプで付ける。特性・持ち物は1体1つずつ（仕様書 §4.6）
    /// </summary>
    public static class MonsterTable
    {
        public static readonly IReadOnlyList<MonsterDefinition> All = new[]
        {
            Monster(1, "ユキペン", new[] { Ice }, new Stats(75, 80, 70, 70, 70, 80),
                new[] { IcicleBeak, BlizzardBreath, SnowPellet, FlapDance },
                Look(BodyShape.Normal, Sky, HeadPart.Horn, NeckPart.None), Ability.LastStand, HeldItem.SharpGlasses),
            Monster(2, "ナミペン", new[] { Sea }, new Stats(85, 70, 80, 85, 80, 55),
                new[] { BigWave, BubbleSpit, BlizzardBreath, Splash },
                Look(Chubby, Blue, HeadPart.None, NeckPart.Scarf), Ability.ThickFat, HeldItem.StarFish),
            Monster(3, "ヒノペン", new[] { Fire }, new Stats(65, 85, 60, 90, 65, 90),
                new[] { FlameDive, CampfireBreath, SparkBeak, FlareUp },
                Look(Slim, Red, HeadPart.Flame, NeckPart.None), Ability.LastStand, HeldItem.LifeIcicle),
            Monster(4, "イナペン", new[] { Thunder, Bird }, new Stats(60, 70, 60, 95, 70, 100),
                new[] { SparkWing, WindCutter, NumbWave, BellySlide },
                Look(Slim, Yellow, HeadPart.Bolt, NeckPart.Scarf), Ability.StaticBody, HeldItem.LightFeather),
            Monster(5, "コケペン", new[] { Moss, Rock }, new Stats(90, 85, 95, 55, 75, 40),
                new[] { MossWhip, Boulder, SleepMoss, Tackle },
                Look(Chubby, Green, HeadPart.Leaf, NeckPart.Bell), Ability.SpikySkin, HeldItem.StarFish),
            Monster(6, "イワペン", new[] { Rock }, new Stats(80, 100, 100, 40, 60, 45),
                new[] { Boulder, CliffBreak, RockSkin, IcicleBeak },
                Look(Chubby, Brown, HeadPart.Crest, NeckPart.None), Ability.HardFeathers, HeldItem.GritScarf),
            Monster(7, "ヒョウガペン", new[] { Ice, Rock }, new Stats(95, 95, 100, 50, 70, 40),
                new[] { IcebergDrop, Boulder, RockSkin, SnowPellet },
                Look(Chubby, Sky, HeadPart.Horn, NeckPart.Bell), Ability.SpikySkin, HeldItem.SnowCoat),
            Monster(8, "ホシペン", new[] { Ice, Aurora }, new Stats(60, 50, 60, 105, 75, 100),
                new[] { BlizzardBreath, AuroraWave, ColdBreath, QuietPrayer },
                Look(Slim, Blue, HeadPart.Horn, NeckPart.Scarf), Ability.Tailwind, HeldItem.WiseFeather),
            Monster(9, "コウテイペン", new[] { Ice, MonsterType.Normal }, new Stats(100, 90, 80, 75, 80, 50),
                new[] { IcicleBeak, PenguinPress, IcebergDrop, Fluff },
                Look(BodyShape.Normal, Navy, HeadPart.Crown, NeckPart.Scarf), Ability.Intimidate, HeldItem.SquidSnack),
            Monster(10, "リュウヒョウペン", new[] { Ice, Sea }, new Stats(80, 70, 85, 85, 80, 55),
                new[] { BlizzardBreath, BigWave, ColdBreath, Splash },
                Look(BodyShape.Normal, Blue, HeadPart.None, NeckPart.Bell), Ability.HardFeathers, HeldItem.FreshKelp),
            Monster(11, "ペンタ", new[] { MonsterType.Normal }, new Stats(75, 75, 75, 75, 75, 75),
                new[] { Tackle, BellySlide, PenguinPress, FlapDance },
                Look(BodyShape.Normal, Navy, HeadPart.None, NeckPart.None), Ability.Momentum, HeldItem.BraveFeather),
            Monster(12, "モフペン", new[] { MonsterType.Normal }, new Stats(125, 65, 75, 60, 85, 35),
                new[] { PenguinPress, Shout, CutePose, Fluff },
                Look(Chubby, Pink, HeadPart.None, NeckPart.Bell), Ability.ThickFat, HeldItem.StarFish),
            Monster(13, "ウズペン", new[] { Sea }, new Stats(80, 95, 75, 60, 70, 80),
                new[] { Whirlpool, SeaGun, Sneak, FlapDance },
                Look(BodyShape.Normal, Blue, HeadPart.Crest, NeckPart.None), Ability.Momentum, HeldItem.LifeIcicle),
            Monster(14, "サンゴペン", new[] { Sea, Moss }, new Stats(75, 55, 75, 90, 95, 60),
                new[] { BubbleSpit, MossCannon, PoisonMoss, BigWave },
                Look(Slim, Pink, HeadPart.Leaf, NeckPart.None), Ability.PoisonSlime, HeldItem.FreshKelp),
            Monster(15, "カザンペン", new[] { Fire, Rock }, new Stats(95, 100, 85, 60, 60, 50),
                new[] { FlameDive, CliffBreak, RockSkin, WispFlame },
                Look(Chubby, Red, HeadPart.Flame, NeckPart.Bell), Ability.Intimidate, HeldItem.BraveFeather),
            Monster(16, "タキビペン", new[] { Fire }, new Stats(70, 50, 65, 110, 80, 85),
                new[] { Eruption, CampfireBreath, WispFlame, FlareUp },
                Look(BodyShape.Normal, Red, HeadPart.Flame, NeckPart.Scarf), Ability.LastStand, HeldItem.WiseFeather),
            Monster(17, "ライペン", new[] { Thunder }, new Stats(70, 95, 65, 65, 65, 100),
                new[] { SparkBeak, BellySlide, FlapAttack, NumbWave },
                Look(BodyShape.Normal, Yellow, HeadPart.Bolt, NeckPart.None), Ability.StaticBody, HeldItem.GritScarf),
            Monster(18, "アラシペン", new[] { Thunder }, new Stats(90, 60, 80, 100, 85, 45),
                new[] { StormDrop, BigWave, SparkWing, NumbWave },
                Look(Chubby, Navy, HeadPart.Bolt, NeckPart.Scarf), Ability.NightOwl, HeldItem.SquidSnack),
            Monster(19, "ノリペン", new[] { Moss }, new Stats(85, 60, 80, 85, 90, 60),
                new[] { SlimySeaweed, MossCannon, ToxicMoss, Fluff },
                Look(BodyShape.Normal, Green, HeadPart.Leaf, NeckPart.Scarf), Ability.PoisonSlime, HeldItem.StarFish),
            Monster(20, "ツバサペン", new[] { Bird }, new Stats(65, 95, 60, 60, 60, 110),
                new[] { FlapAttack, DiveBeak, SkyDream, BellySlide },
                Look(Slim, Sky, HeadPart.Crest, NeckPart.Scarf), Ability.Tailwind, HeldItem.LifeIcicle),
            Monster(21, "カゼペン", new[] { Bird }, new Stats(70, 55, 65, 100, 75, 95),
                new[] { WindCutter, Whirlwind, SparkWing, SkyDream },
                Look(BodyShape.Normal, Sky, HeadPart.None, NeckPart.Scarf), Ability.HardFeathers, HeldItem.SharpGlasses),
            Monster(22, "シャチペン", new[] { Dark }, new Stats(90, 110, 80, 50, 65, 70),
                new[] { OrcaBite, Sneak, Whirlpool, SharpGlare },
                Look(Chubby, Black, HeadPart.Fin, NeckPart.None), Ability.Intimidate, HeldItem.LightFeather),
            Monster(23, "ヨルペン", new[] { Dark }, new Stats(65, 60, 65, 105, 80, 90),
                new[] { PolarNight, AuroraWave, DarkWhisper, CampfireBreath },
                Look(Slim, Purple, HeadPart.Fin, NeckPart.Bell), Ability.NightOwl, HeldItem.WiseFeather),
            Monster(24, "オーロペン", new[] { Aurora }, new Stats(80, 55, 75, 105, 95, 65),
                new[] { SparkleLaser, AuroraWave, QuietPrayer, ColdBreath },
                Look(BodyShape.Normal, Purple, HeadPart.Crown, NeckPart.None), Ability.ThickFat, HeldItem.SnowCoat),
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

        // 1行を短くするための別名。並びは MonsterDefinition / MonsterLook のコンストラクタと同じ
        private static MonsterDefinition Monster(int no, string name, MonsterType[] types, Stats stats, MoveDefinition[] moves, MonsterLook look,
            Ability ability, HeldItem item)
            => new MonsterDefinition(no, name, types, stats, moves, look, ability, item);

        private static MonsterLook Look(BodyShape shape, BodyColor color, HeadPart head, NeckPart neck)
            => new MonsterLook(shape, color, head, neck);
    }
}
