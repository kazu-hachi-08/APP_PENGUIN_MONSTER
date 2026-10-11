using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>うみタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Water = Rgb(90, 170, 230);
        private static readonly Color32 DeepWater = Rgb(50, 110, 190);

        /// <summary>No.2 ナミペン: 大波にのって登場。右に巻いた波の頭と、足元をかくす波しぶき</summary>
        private static void Nami(PenguinBrush b)
        {
            var body = Rgb(60, 120, 210);
            // 背後の大波: ペンギンより高い水の柱から、頭の上へ巻いて垂れ下がる
            b.Ellipse(52f, 26f, 9f, 20f, Water);
            b.Ellipse(42f, 47f, 12f, 5f, Water);
            b.Ellipse(30f, 44f, 3f, 3.5f, Water);
            b.Ellipse(45f, 41f, 6f, 2.5f, DeepWater);
            b.Line(0.6f, DeepWater, V(55, 14), V(56, 32));
            b.Ellipse(44f, 51f, 8f, 2f, PenguinPalette.Belly);
            b.Ellipse(31f, 48f, 2.5f, 1.5f, PenguinPalette.Belly);
            b.Ellipse(27f, 41f, 1.5f, 1.5f, PenguinPalette.Belly);
            b.Triangle(V(17, 24), V(20, 29), V(8, 37), PenguinPalette.Shade(body));
            b.Shaded(28f, 20f, 13f, 12f, body);
            b.Belly(28f, 19f, 9f, 9f);
            b.Shaded(28f, 35f, 12f, 12f, body);
            b.White(28f, 34f, 9f, 7f);
            b.Face(28f, 34f, 4.5f, smile: true);
            b.Shaded(CenterX, 7f, 28f, 7f, Water);
            b.Line(0.7f, DeepWater, V(8, 6), V(20, 8), V(32, 6), V(44, 8), V(56, 6));
            for (int x = 7; x <= 57; x += 7)
            {
                b.Ellipse(x, 13f, 3f, 2f, PenguinPalette.Belly);
            }
        }

        /// <summary>No.13 ウズペン: 腰のまわりを うず潮の輪が回る。頭のくせ毛も うず巻き</summary>
        private static void Uzu(PenguinBrush b)
        {
            var body = Rgb(40, 150, 170);
            var ring = Rgb(110, 190, 240);
            const float RingY = 16f;
            const float RingRadiusX = 26f;
            const float RingRadiusY = 7f;
            // 輪の奥側を先に塗り、体を描いてから手前側を重ねて、体に巻き付いて見せる
            b.Ellipse(CenterX, RingY, RingRadiusX, RingRadiusY, DeepWater);
            b.Feet(CenterX, 6f);
            b.Shaded2(21f, 20f, 3f, 6.5f, PenguinPalette.Shade(body));
            b.Shaded(CenterX, 18f, 12f, 13f, body);
            b.Belly(CenterX, 17f, 8f, 10f);
            b.Shaded(CenterX, 35f, 12f, 12f, body);
            b.White(CenterX, 34f, 9f, 7f);
            b.Face(CenterX, 34f, 4.5f);
            b.Line(2f, ring, ArcPoints(CenterX, RingY, RingRadiusX, RingRadiusY, Mathf.PI, Mathf.PI * 2f));
            b.Line(0.6f, PenguinPalette.Belly, ArcPoints(CenterX, RingY - 1f, RingRadiusX - 3f, RingRadiusY - 2f, Mathf.PI * 1.2f, Mathf.PI * 1.6f));
            b.Line(1f, PenguinPalette.Shade(body), V(32, 46), V(34, 50), V(31, 52), V(29, 49), V(31, 48));
            b.Ellipse(8f, 26f, 1.5f, 2f, ring);
            b.Ellipse(56f, 28f, 1.5f, 2f, ring);
            b.Ellipse(12f, 32f, 1f, 1.5f, ring);
            b.Ellipse(52f, 35f, 1f, 1.5f, ring);
        }

        /// <summary>No.14 サンゴペン: 頭から枝分かれしたサンゴの角</summary>
        private static void Sango(PenguinBrush b)
        {
            var coral = Rgb(250, 110, 100);
            var coralTip = Rgb(255, 180, 160);
            StandardBody(b, Rgb(235, 140, 170), 15f, 14f, 13f, 33f, 13f);
            b.Line2(1.6f, coral, V(26, 42), V(19, 52));
            b.Line2(1.3f, coral, V(22, 48), V(14, 50));
            b.Line2(1.3f, coral, V(19, 52), V(16, 59));
            b.Line2(1.3f, coral, V(19, 52), V(23, 58));
            b.Ellipse2(14f, 50f, 1.6f, 1.6f, coralTip);
            b.Ellipse2(16f, 59f, 1.6f, 1.6f, coralTip);
            b.Ellipse2(23f, 58f, 1.6f, 1.6f, coralTip);
            b.Face(CenterX, 32f, 5f);
        }

        /// <summary>楕円の弧を折れ線の点にする（角度はラジアン、0 が右）</summary>
        private static Vector2[] ArcPoints(float x, float y, float radiusX, float radiusY, float from, float to)
        {
            const int Steps = 12;
            var points = new Vector2[Steps + 1];
            for (int i = 0; i <= Steps; i++)
            {
                float angle = Mathf.Lerp(from, to, i / (float)Steps);
                points[i] = V(x + Mathf.Cos(angle) * radiusX, y + Mathf.Sin(angle) * radiusY);
            }
            return points;
        }
    }
}
