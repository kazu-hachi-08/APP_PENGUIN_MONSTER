using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>エースランクのデザイン（仕様書 §6）。ふつうより大きな武器（ひれ・翼・しっぽ・岩）を付けて強そうに見せる</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.43 サメペン: サメの着ぐるみ。大きな背びれと、顔のまわりのギザギザの歯</summary>
        private static void Same(PenguinBrush b)
        {
            var shark = Rgb(90, 110, 150);
            var sharkShade = PenguinPalette.Shade(shark);
            b.Triangle(V(44, 14), V(61, 26), V(56, 12), sharkShade);
            b.Triangle(V(44, 12), V(61, 2), V(56, 12), sharkShade);
            b.Triangle(V(22, 42), V(42, 42), V(40, 63), sharkShade);
            b.Triangle(V(25, 42), V(39, 42), V(38, 59), shark);
            StandardBody(b, shark, 15f, 15f, 13f, 32f, 14f);
            if (b.Front)
            {
                b.Ellipse(CenterX, 30f, 11.5f, 10f, Rgb(160, 40, 60));
                SharkTeeth(b, CenterX, 30f);
                b.Ellipse(CenterX, 29.5f, 8.5f, 7f, PenguinPalette.Belly);
            }
            b.Line2(0.5f, sharkShade, V(20, 18), V(21, 14));
            b.Face(CenterX, 30f, 4f, EyeStyle.Sharp);
        }

        /// <summary>口の輪のふちから内側へ向けて三角の歯を並べる</summary>
        private static void SharkTeeth(PenguinBrush b, float x, float y)
        {
            const int Teeth = 14;
            const float Half = 0.16f;
            for (int i = 0; i < Teeth; i++)
            {
                float angle = i * Mathf.PI * 2f / Teeth;
                b.Triangle(OnEllipse(x, y, 11.5f, 10f, angle - Half), OnEllipse(x, y, 11.5f, 10f, angle + Half),
                    OnEllipse(x, y, 8f, 6.5f, angle), PenguinPalette.Belly);
            }
        }

        private static Vector2 OnEllipse(float x, float y, float radiusX, float radiusY, float angle)
            => V(x + Mathf.Cos(angle) * radiusX, y + Mathf.Sin(angle) * radiusY);

        /// <summary>No.44 ヒノトリペン: 炎の大きな翼と尾羽、頭に炎のとさか</summary>
        private static void Hinotori(PenguinBrush b)
        {
            var shoulder = V(24, 24);
            b.Triangle2(shoulder, V(24, 30), V(3, 50), FlameOuter);
            b.Triangle2(shoulder, V(3, 50), V(1, 36), FlameMiddle);
            b.Triangle2(shoulder, V(1, 36), V(4, 24), FlameOuter);
            b.Triangle2(shoulder, V(4, 24), V(12, 14), FlameMiddle);
            b.Triangle2(V(3, 50), V(1, 36), V(6, 44), FlameInner);
            b.Triangle2(V(1, 36), V(4, 24), V(5, 31), FlameInner);
            b.Triangle2(V(26, 8), V(30, 10), V(14, 1), FlameOuter);
            b.Triangle(V(29, 44), V(35, 44), V(32, 62), FlameOuter);
            b.Triangle2(V(27, 43), V(31, 44), V(22, 58), FlameOuter);
            b.Triangle(V(30, 44), V(34, 44), V(32, 56), FlameInner);
            StandardBody(b, Rgb(220, 70, 50), 15f, 12f, 13f, 33f, 12f);
            b.Face(CenterX, 32f, 4f, EyeStyle.Sharp);
        }

        /// <summary>No.45 ユキヒョウペン: 白地に黒い斑点。長いしっぽを高く巻き上げ、ヒレに氷のつめ</summary>
        private static void YukiHyou(PenguinBrush b)
        {
            var fur = Rgb(220, 224, 234);
            var spot = Rgb(70, 72, 92);
            var tail = new[] { V(42, 8), V(52, 10), V(58, 18), V(59, 28), V(56, 38), V(51, 42) };
            b.Line(3.2f, PenguinPalette.Shade(fur), tail);
            b.Line(2.4f, fur, tail);
            b.Ellipse(51f, 42f, 3f, 3f, spot);
            b.Ellipse(57.5f, 24f, 1.3f, 1.8f, spot);
            b.Ellipse(55f, 13f, 1.6f, 1.3f, spot);
            b.Triangle2(V(21, 41), V(27, 44), V(21, 49), PenguinPalette.Shade(fur));
            b.Triangle2(V(14, 10), V(18, 12), V(8, 4), IceLight);
            b.Triangle2(V(15, 13), V(18, 14), V(7, 11), IceShade);
            StandardBody(b, fur, 15f, 14f, 13f, 32f, 13f);
            foreach (var p in new[] { V(21, 22), V(24, 9), V(22, 37), V(25, 43), V(32, 44) })
            {
                b.Ellipse2(p.x, p.y, 1.5f, 1.2f, spot);
            }
            b.Face(CenterX, 31f, 4.5f, EyeStyle.Sharp);
        }

        /// <summary>No.46 ヒカリペン: 背中に光る大きな花を背負い、まわりに光が舞う</summary>
        private static void Hikari(PenguinBrush b)
        {
            var petal = Rgb(255, 190, 225);
            var petalShade = PenguinPalette.Darken(petal, 0.8f);
            const int Petals = 8;
            const float FlowerY = 34f;
            for (int i = 0; i < Petals; i++)
            {
                float angle = (i + 0.5f) * Mathf.PI * 2f / Petals;
                var p = OnEllipse(CenterX, FlowerY, 19f, 18f, angle);
                b.Ellipse(p.x, p.y, 7f, 7f, petalShade);
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 5.5f, 5.5f, petal);
            }
            b.Ellipse(CenterX, FlowerY, 17f, 17f, Rgb(255, 240, 160));
            StandardBody(b, Rgb(160, 215, 160), 14f, 12f, 12f, 33f, 12f);
            b.Sparkle(6, 10, PenguinPalette.Gold);
            b.Sparkle(10, 56, PenguinPalette.Gold);
            b.Sparkle(54, 58, PenguinPalette.Gold);
            b.Sparkle(58, 8, PenguinPalette.Gold);
            b.Face(CenterX, 32f, 4.5f, EyeStyle.Cross);
        }

        /// <summary>No.47 ライガンペン: 両肩に大きな岩を担いだ岩のよろい。体に稲妻の模様</summary>
        private static void Raigan(PenguinBrush b)
        {
            var rock = Rgb(130, 118, 108);
            var rockShade = PenguinPalette.Shade(rock);
            b.Triangle(V(29, 44), V(35, 44), V(33, 60), BoltShade);
            b.Triangle(V(30, 44), V(34, 44), V(33, 56), Bolt);
            b.Feet(CenterX, 9f);
            b.Shaded(CenterX, 15f, 16f, 13f, rock);
            b.Belly(CenterX, 14f, 10f, 9.5f);
            b.Line(1.2f, BoltShade, V(26, 22), V(34, 15), V(28, 12), V(37, 4));
            b.Line(0.6f, Bolt, V(26, 22), V(34, 15), V(28, 12), V(37, 4));
            b.Shaded(CenterX, 33f, 12f, 11f, rock);
            b.White(CenterX, 32f, 9f, 7f);
            b.Polygon(V(11, 31), rockShade, V(2, 24), V(3, 36), V(10, 42), V(19, 38), V(21, 27), V(13, 22));
            b.Polygon(V(10, 32), Stone, V(4, 26), V(5, 35), V(10, 39), V(17, 36), V(18, 28), V(12, 25));
            b.Polygon(V(53, 31), rockShade, V(62, 24), V(61, 36), V(54, 42), V(45, 38), V(43, 27), V(51, 22));
            b.Polygon(V(52, 32), Stone, V(60, 26), V(59, 35), V(54, 39), V(47, 36), V(46, 28), V(52, 25));
            b.Line(0.6f, Bolt, V(8, 37), V(11, 33), V(8, 31), V(12, 27));
            b.Line(0.6f, Bolt, V(56, 37), V(53, 33), V(56, 31), V(52, 27));
            b.Face(CenterX, 32f, 4.5f, EyeStyle.Dot);
        }
    }
}
