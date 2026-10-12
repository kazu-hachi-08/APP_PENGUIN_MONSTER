using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>やみ・オーロラタイプ（極夜の空）が主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.22 シャチペン: 腹ばいで滑る横長の体に、シャチの背びれ・尾びれ・目の上の白い模様</summary>
        private static void Shachi(PenguinBrush b)
        {
            var black = Rgb(64, 68, 92);
            var blackShade = PenguinPalette.Shade(black);
            var white = PenguinPalette.Belly;
            b.Triangle(V(50, 20), V(61, 30), V(57, 18), blackShade);
            b.Triangle(V(50, 16), V(61, 8), V(57, 18), blackShade);
            b.Triangle(V(32, 26), V(44, 26), V(47, 47), blackShade);
            b.Triangle(V(34, 26), V(42, 26), V(45, 44), black);
            b.Shaded(36f, 18f, 22f, 11f, black);
            // シャチの白い模様は背中側からも見えるので White ではなく常に塗る
            b.Ellipse(36f, 12f, 18f, 5f, white);
            b.Ellipse(46f, 20f, 5f, 2.5f, white);
            b.Ellipse(30f, 7f, 6f, 3f, blackShade);
            b.Shaded(17f, 29f, 13f, 12f, black);
            b.White(17f, 26f, 10f, 7f);
            b.Ellipse(10.5f, 37f, 3.5f, 2f, white);
            b.Ellipse(23.5f, 37f, 3.5f, 2f, white);
            b.Face(17f, 29f, 5f, EyeStyle.Sharp);
        }

        /// <summary>No.23 ヨルペン: 星をちりばめた夜空のマントと、頭の三日月</summary>
        private static void Yoru(PenguinBrush b)
        {
            var cape = Rgb(40, 40, 90);
            var body = Rgb(110, 70, 150);
            b.Triangle(V(32, 44), V(4, 2), V(60, 2), cape);
            b.Sparkle(12, 10, PenguinPalette.Gold);
            b.Sparkle(52, 7, PenguinPalette.Gold);
            b.Dot(18, 22, PenguinPalette.Gold);
            b.Dot(46, 26, PenguinPalette.Gold);
            b.Dot(8, 4, PenguinPalette.Gold);
            b.Dot(56, 16, PenguinPalette.Gold);
            b.Feet(CenterX, 6f);
            b.Shaded(CenterX, 16f, 11f, 12f, body);
            b.Belly(CenterX, 15f, 7f, 9f);
            b.Shaded(CenterX, 33f, 12f, 12f, body);
            b.White(CenterX, 32f, 9f, 7f);
            b.Face(CenterX, 32f, 4.5f, EyeStyle.Sleepy);
            b.Ellipse(CenterX, 51f, 6f, 6f, Rgb(255, 230, 120));
            b.Erase(35f, 53f, 5f, 5f);
        }

        /// <summary>No.24 オーロペン: 背中からオーロラの帯が3本たなびく</summary>
        private static void Auro(PenguinBrush b)
        {
            Ribbon(b, Rgb(120, 240, 180), V(26, 32), V(18, 38), V(14, 46), V(8, 52), V(4, 60));
            Ribbon(b, Rgb(120, 200, 250), V(32, 40), V(30, 48), V(34, 54), V(31, 61));
            Ribbon(b, Rgb(240, 140, 220), V(38, 32), V(46, 38), V(50, 46), V(56, 52), V(60, 58));
            StandardBody(b, Rgb(200, 190, 240), 15f, 12f, 12f, 31f, 12f);
            b.Ellipse(CenterX, 41f, 1.5f, 1.5f, Rgb(120, 200, 250));
            b.Face(CenterX, 30f, 4.5f, EyeStyle.Dot);
        }

        /// <summary>太い帯の中心に細い明るい線を通して、光っているように見せる</summary>
        private static void Ribbon(PenguinBrush b, Color32 color, params Vector2[] points)
        {
            b.Line(2.5f, color, points);
            b.Line(0.8f, PenguinPalette.Belly, points);
        }

        /// <summary>No.35 コウモリペン: コウモリの翼ととがった耳</summary>
        private static void Koumori(PenguinBrush b)
        {
            var wing = Rgb(60, 45, 90);
            var body = Rgb(120, 80, 160);
            b.Polygon(V(22, 30), wing, V(22, 24), V(18, 38), V(4, 44), V(6, 36), V(2, 30), V(7, 25), V(4, 18), V(12, 20), V(20, 18));
            b.Polygon(V(42, 30), wing, V(42, 24), V(46, 38), V(60, 44), V(58, 36), V(62, 30), V(57, 25), V(60, 18), V(52, 20), V(44, 18));
            b.Line2(0.5f, body, V(20, 30), V(5, 40));
            b.Line2(0.5f, body, V(20, 28), V(4, 28));
            b.Line2(0.5f, body, V(20, 26), V(6, 20));
            b.Triangle2(V(22, 37), V(28, 41), V(20, 49), PenguinPalette.Shade(body));
            b.Triangle2(V(23, 38), V(27, 41), V(21, 46), Rgb(230, 150, 200));
            StandardBody(b, body, 15f, 11f, 12f, 32f, 12f);
            b.Face(CenterX, 31f, 4.5f, EyeStyle.Sleepy);
        }
    }
}
