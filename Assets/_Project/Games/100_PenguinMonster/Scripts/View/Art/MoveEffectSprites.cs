using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 技のエフェクトの粒（16x16 のドット絵）をタイプごとに作って使い回す。
    /// ペンモンの絵と同じくコードで描くので、素材が無くても全タイプにエフェクトを付けられる
    /// </summary>
    public static class MoveEffectSprites
    {
        public const int Size = 16;
        private const float Center = Size / 2f;

        private static readonly Vector2 MiddlePivot = new Vector2(0.5f, 0.5f);
        private static readonly Dictionary<MonsterType, Sprite> Cache = new Dictionary<MonsterType, Sprite>();

        public static Sprite Get(MonsterType type)
        {
            if (Cache.TryGetValue(type, out var sprite) && sprite != null)
            {
                return sprite;
            }

            sprite = Paint(type).ToSprite($"MoveEffect_{type}", MiddlePivot);
            Cache[type] = sprite;
            return sprite;
        }

        private static PixelCanvas Paint(MonsterType type)
        {
            var canvas = new PixelCanvas(Size, Size);
            switch (type)
            {
                case MonsterType.Ice: PaintShard(canvas); break;
                case MonsterType.Sea: PaintDrop(canvas); break;
                case MonsterType.Fire: PaintFlame(canvas); break;
                case MonsterType.Thunder: PaintBolt(canvas); break;
                case MonsterType.Moss: PaintLeaf(canvas); break;
                case MonsterType.Rock: PaintRock(canvas); break;
                case MonsterType.Bird: PaintFeather(canvas); break;
                case MonsterType.Dark: PaintCrescent(canvas); break;
                // オーロラは白で描き、出すときに色を変えて虹色に見せる。縁取りも色が付くので付けない
                case MonsterType.Aurora: PaintSparkle(canvas); return canvas;
                default: PaintImpact(canvas); break;
            }
            return canvas;
        }

        /// <summary>ノーマル: 「バシッ」の4方向の星</summary>
        private static void PaintImpact(PixelCanvas c)
        {
            var fill = Rgb(255, 230, 120);
            c.FillTriangle(new Vector2(6, 8), new Vector2(10, 8), new Vector2(8, 15), fill);
            c.FillTriangle(new Vector2(6, 8), new Vector2(10, 8), new Vector2(8, 1), fill);
            c.FillTriangle(new Vector2(8, 6), new Vector2(8, 10), new Vector2(1, 8), fill);
            c.FillTriangle(new Vector2(8, 6), new Vector2(8, 10), new Vector2(15, 8), fill);
            c.FillEllipse(Center, Center, 2.5f, 2.5f, Rgb(255, 255, 240));
            c.Outline(Rgb(200, 110, 30));
        }

        /// <summary>氷: 先が上を向いた細いひし形の結晶。飛ぶ向きに回して刺さるように見せる</summary>
        private static void PaintShard(PixelCanvas c)
        {
            var fill = Rgb(170, 230, 255);
            c.FillTriangle(new Vector2(8, 16), new Vector2(5, 7), new Vector2(11, 7), fill);
            c.FillTriangle(new Vector2(8, 1), new Vector2(5, 7), new Vector2(11, 7), fill);
            c.DrawLine(new Vector2(7, 4), new Vector2(7, 12), 0.5f, Rgb(250, 255, 255));
            c.Outline(Rgb(40, 90, 170));
        }

        private static void PaintDrop(PixelCanvas c)
        {
            var fill = Rgb(70, 140, 235);
            c.FillEllipse(Center, 6f, 4.5f, 4.5f, fill);
            c.FillTriangle(new Vector2(4, 7), new Vector2(12, 7), new Vector2(8, 15), fill);
            c.FillEllipse(6.5f, 6.5f, 1.2f, 1.6f, Rgb(220, 240, 255));
            c.Outline(Rgb(20, 50, 120));
        }

        private static void PaintFlame(PixelCanvas c)
        {
            c.FillEllipse(Center, 5f, 5f, 4f, Rgb(235, 80, 40));
            c.FillTriangle(new Vector2(3, 6), new Vector2(13, 6), new Vector2(8, 15), Rgb(235, 80, 40));
            c.FillEllipse(Center, 4.5f, 3f, 2.5f, Rgb(255, 210, 70));
            c.FillTriangle(new Vector2(5.5f, 5), new Vector2(10.5f, 5), new Vector2(8, 11), Rgb(255, 210, 70));
            c.Outline(Rgb(120, 25, 15));
        }

        /// <summary>雷: 上下2つの三角を少し重ねたギザギザの稲妻</summary>
        private static void PaintBolt(PixelCanvas c)
        {
            var fill = Rgb(255, 235, 60);
            c.FillTriangle(new Vector2(11, 16), new Vector2(4, 7), new Vector2(10, 7), fill);
            c.FillTriangle(new Vector2(6, 9), new Vector2(12, 9), new Vector2(5, 0), fill);
            c.Outline(Rgb(130, 90, 10));
        }

        private static void PaintLeaf(PixelCanvas c)
        {
            c.FillTiltedEllipse(Center, Center, 6.5f, 3f, 45f, Rgb(110, 200, 80));
            c.DrawLine(new Vector2(4, 4), new Vector2(12, 12), 0.45f, Rgb(50, 120, 40));
            c.Outline(Rgb(30, 80, 25));
        }

        private static void PaintRock(PixelCanvas c)
        {
            c.FillEllipse(Center, 7f, 6.5f, 5.5f, Rgb(150, 125, 95));
            c.FillEllipse(4.5f, 4.5f, 3.5f, 1.5f, Rgb(110, 90, 70));
            c.FillEllipse(6f, 9.5f, 2.5f, 1.5f, Rgb(200, 180, 150));
            c.Outline(Rgb(60, 45, 35));
        }

        private static void PaintFeather(PixelCanvas c)
        {
            c.FillTiltedEllipse(Center, Center, 6.5f, 2.5f, 30f, Rgb(235, 245, 255));
            c.DrawLine(new Vector2(2, 4.5f), new Vector2(14, 11.5f), 0.45f, Rgb(140, 160, 190));
            c.Outline(Rgb(70, 90, 130));
        }

        /// <summary>闇: 円から一回り小さい円を消して作る三日月。ひっかいた跡に見せる</summary>
        private static void PaintCrescent(PixelCanvas c)
        {
            c.FillEllipse(Center, Center, 7f, 7f, Rgb(90, 50, 130));
            c.FillEllipse(10.5f, 10.5f, 6.5f, 6.5f, new Color32(0, 0, 0, 0));
            c.Outline(Rgb(210, 150, 255));
        }

        private static void PaintSparkle(PixelCanvas c)
        {
            var white = Rgb(255, 255, 255);
            c.FillRect(7, 1, 2, 14, white);
            c.FillRect(1, 7, 14, 2, white);
            c.FillEllipse(Center, Center, 3f, 3f, white);
        }

        private static Color32 Rgb(byte r, byte g, byte b) => new Color32(r, g, b, 255);
    }
}
