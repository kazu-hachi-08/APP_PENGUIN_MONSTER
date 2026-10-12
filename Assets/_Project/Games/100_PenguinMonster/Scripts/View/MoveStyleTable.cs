using System.Collections.Generic;
using PenguinMonster.Battle;
using static PenguinMonster.Battle.MoveTable;

namespace PenguinMonster.View
{
    /// <summary>
    /// ダメージ技ごとのエフェクトの型。見た目だけの話なので、純C#の技の定義（MoveTable）には持たせず View 側に置く。
    /// 表にない技（新しく足した技など）はタイプの決まった型で出す。伝説の専用技などの特別な演出は MoveEffectView.Signatures.cs
    /// </summary>
    public static class MoveStyleTable
    {
        private static readonly Dictionary<MoveDefinition, MoveStyle> Styles = new Dictionary<MoveDefinition, MoveStyle>
        {
            // ノーマル
            { Tackle, MoveStyle.Hit },
            { BellySlide, MoveStyle.Hit },
            { PenguinPress, MoveStyle.Hit },
            { Shout, MoveStyle.Blow },
            { RecklessTackle, MoveStyle.Hit },
            { FlipperSlap, MoveStyle.Hit },
            { FirstRoar, MoveStyle.Blow },
            // こおり
            { IcicleBeak, MoveStyle.Pierce },
            { BlizzardBreath, MoveStyle.Blow },
            { SnowPellet, MoveStyle.Converge },
            { IcebergDrop, MoveStyle.Fall },
            { ColdBreath, MoveStyle.Blow },
            { IcicleBarrage, MoveStyle.Pierce },
            { FrostKiss, MoveStyle.Swirl },
            { Avalanche, MoveStyle.Fall },
            // うみ
            { BigWave, MoveStyle.Splash },
            { SeaGun, MoveStyle.Shot },
            { Whirlpool, MoveStyle.Swirl },
            { BubbleSpit, MoveStyle.Blow },
            { HotSpring, MoveStyle.Rise },
            { SeaGodVortex, MoveStyle.Swirl },
            { DiveJet, MoveStyle.Hit },
            { SprayBurst, MoveStyle.Shot },
            // ひ
            { FlameDive, MoveStyle.Converge },
            { CampfireBreath, MoveStyle.Blow },
            { Eruption, MoveStyle.Rise },
            { FlameRush, MoveStyle.Converge },
            { SunWheelFlame, MoveStyle.Rise },
            { BurnOut, MoveStyle.Blow },
            // かみなり
            { SparkBeak, MoveStyle.Pierce },
            { SparkWing, MoveStyle.Blow },
            { StormDrop, MoveStyle.Plunge },
            { VoltRush, MoveStyle.Hit },
            { ZapJab, MoveStyle.Hit },
            { Discharge, MoveStyle.Twinkle },
            // こけ
            { MossWhip, MoveStyle.Slash },
            { SlimySeaweed, MoveStyle.Swirl },
            { MossCannon, MoveStyle.Shot },
            { MossDrain, MoveStyle.Swirl },
            { SeedBarrage, MoveStyle.Shot },
            { SporeCloud, MoveStyle.Blow },
            // いわ
            { Boulder, MoveStyle.Fall },
            { CliffBreak, MoveStyle.Plunge },
            { PebbleShot, MoveStyle.Shot },
            { RockHeadbutt, MoveStyle.Hit },
            { GemBeam, MoveStyle.Shot },
            { SandSmoke, MoveStyle.Blow },
            // とり
            { WindCutter, MoveStyle.Slash },
            { FlapAttack, MoveStyle.Hit },
            { DiveBeak, MoveStyle.Plunge },
            { Whirlwind, MoveStyle.Swirl },
            { BraveDive, MoveStyle.Plunge },
            { DoublePeck, MoveStyle.Pierce },
            { PeckDrain, MoveStyle.Pierce },
            { GaleFeather, MoveStyle.Swirl },
            // やみ
            { OrcaBite, MoveStyle.Bite },
            { PolarNight, MoveStyle.Twinkle },
            { Sneak, MoveStyle.Slash },
            { LifeSap, MoveStyle.Swirl },
            { DoubleBite, MoveStyle.Bite },
            { ShadowStitch, MoveStyle.Converge },
            // オーロラ
            { AuroraWave, MoveStyle.Blow },
            { SparkleLaser, MoveStyle.Shot },
            { ShootingStar, MoveStyle.Fall },
            { PrismKick, MoveStyle.Hit },
        };

        public static MoveStyle Get(MoveDefinition move)
        {
            return Styles.TryGetValue(move, out var style) ? style : DefaultOf(move.Type);
        }

        /// <summary>表にない技の型。タイプらしさが一番出る動きにする</summary>
        private static MoveStyle DefaultOf(MonsterType type)
        {
            switch (type)
            {
                case MonsterType.Ice: return MoveStyle.Converge;
                case MonsterType.Sea: return MoveStyle.Splash;
                case MonsterType.Fire: return MoveStyle.Rise;
                case MonsterType.Thunder: return MoveStyle.Plunge;
                case MonsterType.Moss: return MoveStyle.Swirl;
                case MonsterType.Rock: return MoveStyle.Fall;
                case MonsterType.Bird: return MoveStyle.Blow;
                case MonsterType.Dark: return MoveStyle.Slash;
                case MonsterType.Aurora: return MoveStyle.Twinkle;
                default: return MoveStyle.Hit;
            }
        }
    }
}
