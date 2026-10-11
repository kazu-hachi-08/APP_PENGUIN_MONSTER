using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>とりタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.20 ツバサペン: 空にあこがれて大きな翼を広げた ほっそり。頭に冠羽</summary>
        private static void Tsubasa(PenguinBrush b)
        {
            var sky = Rgb(110, 180, 230);
            var skyShade = PenguinPalette.Shade(sky);
            var tip = Rgb(240, 248, 255);
            var shoulder = V(25, 24);
            // 風切り羽を明暗交互の三角で並べ、先に白い羽先を付ける
            b.Triangle2(shoulder, V(25, 30), V(4, 46), skyShade);
            b.Triangle2(shoulder, V(4, 46), V(1, 36), sky);
            b.Triangle2(shoulder, V(1, 36), V(3, 26), skyShade);
            b.Triangle2(shoulder, V(3, 26), V(8, 17), sky);
            b.Triangle2(shoulder, V(8, 17), V(25, 16), skyShade);
            b.Triangle2(V(4, 46), V(1, 36), V(5, 43), tip);
            b.Triangle2(V(1, 36), V(3, 26), V(3, 33), tip);
            b.Triangle2(V(3, 26), V(8, 17), V(6, 24), tip);
            b.Triangle(V(29, 45), V(32, 46), V(26, 58), skyShade);
            b.Triangle(V(31, 46), V(34, 46), V(33, 60), sky);
            b.Triangle(V(33, 45), V(36, 44), V(40, 56), skyShade);
            b.Feet(CenterX, 6f);
            b.Shaded(CenterX, 17f, 10f, 14f, sky);
            b.Belly(CenterX, 15f, 6.5f, 10f);
            b.Shaded(CenterX, 36f, 11f, 11f, sky);
            b.White(CenterX, 35f, 8.5f, 7.5f);
            b.Face(CenterX, 36f, 4.5f);
        }

        /// <summary>No.21 カゼペン: 小さな竜巻にのって浮かぶ。マフラーが風になびく</summary>
        private static void Kaze(PenguinBrush b)
        {
            var wind = Rgb(220, 235, 245);
            var windShade = Rgb(170, 195, 215);
            var mint = Rgb(120, 210, 170);
            var scarf = Rgb(255, 200, 60);
            b.Triangle(V(15, 24), V(49, 24), V(32, 2), wind);
            b.Line(0.6f, windShade, V(17, 21), V(46, 19));
            b.Line(0.6f, windShade, V(21, 15), V(42, 14));
            b.Line(0.6f, windShade, V(25, 9), V(38, 8));
            b.Line(0.6f, windShade, V(29, 5), V(34, 4));
            b.Shaded2(22f, 30f, 3f, 5.5f, PenguinPalette.Shade(mint));
            b.Shaded(CenterX, 30f, 11f, 8f, mint);
            b.Belly(CenterX, 29f, 7f, 5.5f);
            // マフラーの端は頭の後ろから出して、首に巻いているように見せる
            b.Line(1.8f, scarf, V(36, 33), V(42, 35), V(48, 36), V(54, 35), V(59, 33));
            b.Shaded(CenterX, 42f, 11f, 11f, mint);
            b.White(CenterX, 41f, 8f, 6.5f);
            b.Face(CenterX, 41f, 4f);
        }
    }
}
