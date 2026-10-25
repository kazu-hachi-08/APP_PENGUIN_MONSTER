using System;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 自転車に乗った主人公。4方向 × 2コマ（こぐ足の左右）。
    /// 16x16 に車輪を収めるため、上着の裾と足の代わりに下の4行へ自転車を描く（上半身は歩きの絵と同じ）
    /// </summary>
    public static partial class HeroArt
    {
        // 上着の上の3行（手はハンドルの高さ）
        private const int RiderBodyRows = 3;

        // 前・後ろから見た自転車。ハンドル・両足・前（後ろ）の車輪。左右反転でもう片方の足をこいだ絵にする
        private static readonly string[] FrontBicycle =
        {
            "..oggggffggggo..",
            "....oBokkoBo....",
            "....okokkoBo....",
            "......okko......",
        };

        // 横向きの上半身は、手をハンドルへ伸ばした形にする
        private static readonly string[] SideRiderHands =
        {
            "...sobbbbbbo....",
            "...ogobbbbbo....",
        };

        // 横から見た自転車（左向き）。前後の車輪と、ペダルを踏む足（コマごとに前・後ろ）
        private static readonly string[] SideBicyclePedalFront =
        {
            "..gfffffBffo....",
            ".kkkk.fBBf.kkkk.",
            "k.ff.kfkk.k.fk.k",
            ".kkkk...k..kkkk.",
        };
        private static readonly string[] SideBicyclePedalBack =
        {
            "..gfffffBffo....",
            ".kkkk.fBBf.kkkk.",
            "k.ff.kf.kkk.fk.k",
            ".kkkk......kkkk.",
        };

        private static Sprite[,] _bicycleSprites;

        /// <param name="frame">0 と 1 を1歩ごとに替えて、こいで見せる</param>
        public static Sprite GetBicycle(Direction facing, int frame)
        {
            _bicycleSprites ??= CreateBicycle();
            return _bicycleSprites[(int)facing, frame % 2];
        }

        /// <summary>[向き, コマ] の自転車の絵を全部作る</summary>
        private static Sprite[,] CreateBicycle()
        {
            var front = Join(new[] { Head, FrontFace, Take(FrontBody, RiderBodyRows) }, FrontBicycle);
            var back = Join(new[] { Head, BackHair, Take(BackBody, RiderBodyRows) }, FrontBicycle);
            var sideTop = Concat(Take(Side, Side.Length - 4), SideRiderHands);
            var sprites = new Sprite[4, 2];
            FillBicycle(sprites, Direction.Down, front, front, mirrorSecond: true);
            FillBicycle(sprites, Direction.Up, back, back, mirrorSecond: true);
            var pedalFront = Join(new[] { sideTop }, SideBicyclePedalFront);
            var pedalBack = Join(new[] { sideTop }, SideBicyclePedalBack);
            FillBicycle(sprites, Direction.Left, pedalFront, pedalBack, mirrorSecond: false);
            FillBicycle(sprites, Direction.Right, pedalFront, pedalBack, mirrorSecond: false, flip: true);
            return sprites;
        }

        /// <param name="mirrorSecond">前後の向きは、1コマ目の左右反転を2コマ目にする（逆の足をこぐ）</param>
        /// <param name="flip">横向きの絵は左向きで書いてあるので、右向きは反転する</param>
        private static void FillBicycle(Sprite[,] sprites, Direction facing, string[] first, string[] second, bool mirrorSecond,
            bool flip = false)
        {
            sprites[(int)facing, 0] = Paint(first, Palette, flip).ToSprite($"HeroBike_{facing}_0", FeetPivot);
            sprites[(int)facing, 1] = Paint(second, Palette, flip != mirrorSecond).ToSprite($"HeroBike_{facing}_1", FeetPivot);
        }

        private static string[] Take(string[] rows, int count)
        {
            var taken = new string[count];
            Array.Copy(rows, taken, count);
            return taken;
        }

        private static string[] Concat(string[] first, string[] second)
        {
            var rows = new string[first.Length + second.Length];
            first.CopyTo(rows, 0);
            second.CopyTo(rows, first.Length);
            return rows;
        }
    }
}
