using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>ヒナランクのデザイン（仕様書 §6）。体をふつうの6割ほどにして、頭に綿毛のアホ毛を立てる</summary>
    public static partial class PenguinDesigns
    {
        private const float ChickEyeY = 20f;
        private const float ChickEyeOffset = 3.5f;

        /// <summary>ヒナの体。並んだときに小ささで見分けられるよう、どのヒナもこの大きさにそろえる</summary>
        private static void ChickBody(PenguinBrush b, Color32 color, bool tuft = true)
        {
            b.Feet(CenterX, 4f);
            b.Shaded2(24f, 10f, 2.5f, 4f, PenguinPalette.Shade(color));
            b.Shaded(CenterX, 10f, 9f, 8f, color);
            b.Belly(CenterX, 9f, 5.5f, 5.5f);
            b.Shaded(CenterX, 21f, 10f, 9f, color);
            b.White(CenterX, 20f, 7f, 5.5f);
            if (tuft)
            {
                b.Line(0.9f, color, V(32, 29), V(30, 33), V(33, 35));
            }
        }

        private static void ChickFace(PenguinBrush b, EyeStyle eyes) => b.Face(CenterX, ChickEyeY, ChickEyeOffset, eyes);

        /// <summary>No.37 ヒナペン: 灰色のふわふわ。力持ちなので自分より大きなダンベルを持ち上げる</summary>
        private static void HinaPen(PenguinBrush b)
        {
            var fluff = Rgb(170, 172, 186);
            var iron = Rgb(80, 82, 96);
            b.Line2(1.6f, PenguinPalette.Shade(fluff), V(24, 13), V(19, 28), V(18, 37));
            b.Line(0.8f, iron, V(12, 38), V(52, 38));
            b.Rect(8, 32, 6, 12, iron);
            b.Rect(50, 32, 6, 12, iron);
            b.Rect(9, 40, 2, 3, Rgb(140, 142, 160));
            b.Rect(51, 40, 2, 3, Rgb(140, 142, 160));
            ChickBody(b, fluff);
            ChickFace(b, EyeStyle.Sparkle);
        }

        /// <summary>No.38 ユキヒナペン: 白い綿毛。頭に小さな雪だま</summary>
        private static void YukiHina(PenguinBrush b)
        {
            ChickBody(b, Rgb(205, 228, 248));
            b.Shaded(39f, 31f, 3.5f, 3.5f, PenguinPalette.Belly);
            b.Sparkle(14, 26, IceLight);
            b.Sparkle(50, 18, IceLight);
            ChickFace(b, EyeStyle.Sparkle);
        }

        /// <summary>No.39 ハネヒナペン: 体のわりに大きな羽を広げる（足の速さの印）</summary>
        private static void HaneHina(PenguinBrush b)
        {
            var feather = Rgb(205, 165, 115);
            var featherShade = PenguinPalette.Shade(feather);
            var tip = Rgb(250, 240, 220);
            var shoulder = V(25, 13);
            b.Triangle2(shoulder, V(25, 18), V(5, 34), featherShade);
            b.Triangle2(shoulder, V(5, 34), V(2, 24), feather);
            b.Triangle2(shoulder, V(2, 24), V(6, 13), featherShade);
            b.Triangle2(shoulder, V(6, 13), V(24, 7), feather);
            b.Triangle2(V(5, 34), V(2, 24), V(6, 30), tip);
            b.Triangle2(V(2, 24), V(6, 13), V(5, 20), tip);
            ChickBody(b, feather);
            ChickFace(b, EyeStyle.Sparkle);
        }

        /// <summary>No.40 ナキムシペン: 黒い綿毛の泣き虫。涙のしずくが飛ぶ</summary>
        private static void Nakimushi(PenguinBrush b)
        {
            var tear = Rgb(120, 200, 250);
            ChickBody(b, Rgb(78, 76, 100));
            ChickFace(b, EyeStyle.Sparkle);
            if (!b.Front) return;
            b.Ellipse2(26.5f, 15.5f, 1.2f, 2f, tear);
            b.Ellipse(18f, 24f, 1.5f, 2.2f, tear);
            b.Ellipse(46f, 22f, 1.3f, 2f, tear);
        }

        /// <summary>No.41 カラヒナペン: 割れた卵の殻から顔を出す。殻の上半分を帽子のようにかぶる</summary>
        private static void KaraHina(PenguinBrush b)
        {
            var chick = Rgb(165, 145, 125);
            var shell = Rgb(245, 235, 210);
            var shellShade = PenguinPalette.Darken(shell, 0.82f);
            var spot = Rgb(170, 130, 90);
            b.Line(0.9f, chick, V(38, 30), V(40, 34), V(43, 34));
            b.Shaded(CenterX, 21f, 10f, 9f, chick);
            b.White(CenterX, 20f, 7f, 5.5f);
            ChickFace(b, EyeStyle.Dot);
            b.Polygon(V(32, 8), shellShade, V(16, 1), V(12, 9), V(15, 17), V(20, 13), V(24, 17), V(28, 13),
                V(32, 17), V(36, 13), V(40, 17), V(44, 13), V(49, 17), V(52, 9), V(48, 1));
            b.Polygon(V(31, 9), shell, V(17, 3), V(14, 9), V(16, 14), V(20, 11), V(24, 15), V(28, 11),
                V(32, 15), V(36, 11), V(40, 15), V(44, 11), V(48, 14), V(49, 8), V(46, 3));
            b.Ellipse(21f, 7f, 2f, 1.5f, spot);
            b.Ellipse(40f, 5f, 1.5f, 1.2f, spot);
            b.Ellipse(45f, 10f, 1.2f, 1f, spot);
            b.Polygon(V(28, 31), shellShade, V(20, 27), V(36, 28), V(34, 34), V(27, 36), V(21, 33));
            b.Polygon(V(27, 32), shell, V(21, 28), V(25, 30), V(29, 28), V(33, 30), V(32, 33), V(26, 35), V(22, 32));
            b.Ellipse(26f, 33f, 1.2f, 1f, spot);
        }

        /// <summary>No.42 ピリヒナペン: 黄色の綿毛が静電気で逆立つ</summary>
        private static void PiriHina(PenguinBrush b)
        {
            var fluff = Rgb(250, 215, 80);
            var spark = Rgb(150, 220, 255);
            b.Triangle(V(29, 27), V(35, 27), V(32, 38), fluff);
            b.Triangle2(V(24, 26), V(29, 29), V(20, 36), fluff);
            b.Triangle2(V(22, 22), V(25, 27), V(15, 29), fluff);
            ChickBody(b, fluff, false);
            b.Line(0.6f, spark, V(12, 20), V(9, 17), V(12, 15), V(9, 12));
            b.Line(0.6f, spark, V(52, 26), V(55, 23), V(52, 21), V(55, 18));
            b.Line(0.6f, spark, V(44, 36), V(46, 39), V(43, 40));
            ChickFace(b, EyeStyle.Sparkle);
        }
    }
}
