using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>ひタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 FlameOuter = Rgb(255, 110, 40);
        private static readonly Color32 FlameMiddle = Rgb(255, 150, 50);
        private static readonly Color32 FlameInner = Rgb(255, 220, 80);

        /// <summary>No.3 ヒノペン: ほっそりした体に、燃え上がる炎の髪と しっぽの火</summary>
        private static void Hino(PenguinBrush b)
        {
            b.Triangle(V(20, 36), V(44, 36), V(32, 62), FlameOuter);
            b.Triangle(V(17, 34), V(29, 40), V(12, 54), FlameOuter);
            b.Triangle(V(35, 40), V(47, 34), V(52, 54), FlameOuter);
            b.Triangle(V(25, 38), V(39, 38), V(32, 56), FlameInner);
            b.Triangle(V(40, 6), V(44, 12), V(54, 12), FlameOuter);
            b.Triangle(V(42, 8), V(44, 11), V(50, 11), FlameInner);
            StandardBody(b, Rgb(215, 75, 55), 15f, 11f, 13f, 32f, 12f);
            b.Face(CenterX, 31f, 4f);
        }

        /// <summary>No.15 カザンペン: 体そのものが火山。てっぺんの火口から溶岩が噴き出す</summary>
        private static void Kazan(PenguinBrush b)
        {
            var rock = Rgb(130, 90, 70);
            var rockShade = PenguinPalette.Shade(rock);
            b.Feet(CenterX, 10f);
            b.Polygon(V(32, 20), rockShade, V(5, 2), V(59, 2), V(45, 41), V(19, 41));
            b.Polygon(V(30, 22), rock, V(8, 4), V(55, 4), V(43, 39), V(21, 39));
            b.Line(0.6f, rockShade, V(12, 10), V(16, 14), V(15, 18));
            b.Ellipse(CenterX, 40f, 12f, 3f, Rgb(70, 45, 35));
            b.Ellipse(CenterX, 40.5f, 10f, 2f, FlameOuter);
            b.Line(1.2f, FlameOuter, V(24, 39), V(23, 34));
            b.Ellipse(23f, 33f, 1.8f, 2f, FlameOuter);
            b.Line(1.2f, FlameOuter, V(41, 39), V(42, 36));
            b.Ellipse(42f, 35.5f, 1.5f, 1.8f, FlameOuter);
            b.Ellipse(CenterX, 48f, 4f, 5f, FlameOuter);
            b.Ellipse(CenterX, 47f, 2.5f, 3f, FlameInner);
            b.Ellipse(25f, 53f, 2.5f, 2.5f, FlameOuter);
            b.Ellipse(39f, 56f, 2f, 2f, FlameInner);
            b.White(CenterX, 19f, 11f, 8f);
            b.Face(CenterX, 20f, 5f);
        }

        /// <summary>No.16 タキビペン: たき火の炎そのものの体。足元に薪</summary>
        private static void Takibi(PenguinBrush b)
        {
            var log = Rgb(130, 80, 50);
            var logEnd = Rgb(200, 150, 100);
            var flame = Rgb(240, 90, 40);
            b.Ellipse(22f, 4f, 9f, 2.5f, log);
            b.Ellipse(42f, 4f, 9f, 2.5f, log);
            b.Ellipse2(13.5f, 4f, 2f, 2.2f, logEnd);
            b.Triangle(V(17, 24), V(47, 24), V(32, 58), flame);
            b.Triangle(V(15, 22), V(27, 30), V(9, 42), flame);
            b.Triangle(V(37, 30), V(49, 22), V(55, 40), flame);
            b.Ellipse(CenterX, 18f, 16f, 14f, flame);
            b.Ellipse(CenterX, 17f, 13f, 11f, FlameMiddle);
            b.Triangle(V(21, 22), V(43, 22), V(32, 50), FlameMiddle);
            b.Ellipse(CenterX, 16f, 10f, 8f, FlameInner);
            b.Face(CenterX, 18f, 5f, smile: true);
        }
    }
}
