using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>こけ・いわタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Stone = Rgb(150, 140, 130);

        /// <summary>No.5 コケペン: こけむした小山のような体。キノコと芽が生えている</summary>
        private static void Koke(PenguinBrush b)
        {
            var moss = Rgb(90, 160, 80);
            b.Feet(CenterX, 10f);
            b.Shaded2(10f, 10f, 3f, 5f, PenguinPalette.Shade(moss));
            b.Shaded(CenterX, 14f, 22f, 14f, moss);
            for (int degree = 20; degree <= 160; degree += 20)
            {
                float angle = degree * Mathf.Deg2Rad;
                b.Ellipse(CenterX + Mathf.Cos(angle) * 21f, 14f + Mathf.Sin(angle) * 13f, 4f, 4f, moss);
            }
            b.Shaded(10f, 4f, 4f, 3f, Stone);
            b.Shaded(54f, 5f, 3.5f, 3f, Stone);
            b.Rect(37, 26, 3, 6, PenguinPalette.Belly);
            b.Shaded(38.5f, 33f, 6f, 3.5f, Rgb(200, 80, 60));
            b.Dot(36, 34, PenguinPalette.Belly);
            b.Dot(40, 33, PenguinPalette.Belly);
            b.Line(0.6f, Rgb(50, 110, 50), V(24, 27), V(23, 33));
            b.Ellipse(21f, 34f, 2.5f, 1.5f, Rgb(120, 200, 90));
            b.Ellipse(26f, 35f, 2.5f, 1.5f, Rgb(120, 200, 90));
            b.White(CenterX, 15f, 10f, 7.5f);
            b.Face(CenterX, 16f, 5f, EyeStyle.Smile);
        }

        /// <summary>No.6 イワペン: 体より大きな岩を背負った ずんぐり</summary>
        private static void Iwa(PenguinBrush b)
        {
            var brown = Rgb(150, 104, 70);
            var stoneShade = PenguinPalette.Darken(Stone, 0.7f);
            b.Polygon(V(32, 32), stoneShade, V(12, 14), V(8, 30), V(14, 46), V(28, 55), V(44, 52), V(55, 40), V(56, 20), V(42, 10));
            b.Polygon(V(30, 34), Stone, V(12, 18), V(10, 31), V(16, 44), V(28, 52), V(42, 49), V(52, 38), V(52, 22), V(40, 14));
            b.Polygon(V(24, 44), Rgb(186, 176, 164), V(16, 40), V(20, 47), V(28, 50), V(32, 46), V(26, 40));
            b.Line(0.6f, stoneShade, V(44, 46), V(48, 38), V(46, 32));
            b.Line(0.6f, stoneShade, V(14, 30), V(18, 26));
            b.Feet(CenterX, 9f);
            b.Shaded2(18f, 17f, 3.5f, 6f, PenguinPalette.Shade(brown));
            b.Shaded(CenterX, 13f, 14f, 11f, brown);
            b.Belly(CenterX, 12f, 9f, 8f);
            b.Shaded(CenterX, 27f, 11f, 9.5f, brown);
            b.White(CenterX, 26f, 8.5f, 6.5f);
            b.Face(CenterX, 27f, 4.5f, EyeStyle.Dot);
        }

        /// <summary>No.19 ノリペン: 長い海藻の髪が体の両わきまで垂れる</summary>
        private static void Nori(PenguinBrush b)
        {
            var kelp = Rgb(80, 170, 90);
            var kelpShade = PenguinPalette.Shade(kelp);
            float[] strandX = { 14f, 19f, 24f };
            for (int i = 0; i < strandX.Length; i++)
            {
                b.Line2(1.7f, i % 2 == 0 ? kelpShade : kelp, WavyStrand(strandX[i], 44f, 8f));
            }
            StandardBody(b, Rgb(60, 110, 80), 15f, 13f, 13f, 32f, 12f);
            b.Ellipse(CenterX, 41f, 12.5f, 5f, kelp);
            b.Line2(1f, kelp, V(27, 40), V(26, 37));
            b.Face(CenterX, 31f, 4.5f, EyeStyle.Sleepy);
        }

        /// <summary>上から下へゆらゆら波打つ線。海藻が水にゆれているように見せる</summary>
        private static Vector2[] WavyStrand(float x, float top, float bottom)
        {
            const float Step = 4f;
            const float Sway = 1.8f;
            const float Frequency = 0.45f;
            int count = Mathf.FloorToInt((top - bottom) / Step) + 1;
            var points = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                float y = top - i * Step;
                points[i] = V(x + Mathf.Sin(y * Frequency) * Sway, y);
            }
            return points;
        }

        /// <summary>No.29 キノコペン: 紫に白い水玉の毒キノコの傘をかぶる</summary>
        private static void Kinoko(PenguinBrush b)
        {
            var cap = Rgb(150, 70, 170);
            StandardBody(b, Rgb(100, 130, 80), 14f, 13f, 12f, 30f, 12f);
            // 傘は下が平らな半円にする。楕円を消しゴムで切ると頭まで消えてしまうため
            b.Polygon(V(CenterX, 38f), PenguinPalette.Shade(cap), ArcPoints(CenterX, 36f, 21f, 15f, 0f, Mathf.PI));
            b.Polygon(V(CenterX, 39f), cap, ArcPoints(CenterX - 1f, 37f, 19.5f, 13.5f, 0f, Mathf.PI));
            foreach (var p in new[] { V(20, 44), V(32, 48), V(44, 43), V(26, 40), V(39, 39), V(14, 39), V(50, 38) })
            {
                b.Ellipse(p.x, p.y, 2f, 1.8f, PenguinPalette.Belly);
            }
            b.Face(CenterX, 29f, 4.5f, EyeStyle.Dot);
        }

        /// <summary>No.30 ツタペン: 体にツタが巻きつき、葉っぱの翼を広げる</summary>
        private static void Tsuta(PenguinBrush b)
        {
            var leaf = Rgb(110, 190, 80);
            var leafShade = PenguinPalette.Shade(leaf);
            var vine = Rgb(60, 120, 50);
            foreach (var p in new[] { V(9, 40), V(6, 28), V(10, 17) })
            {
                b.Line2(0.6f, vine, V(20, 22), p);
                b.Ellipse2(p.x, p.y, 6f, 3.5f, leafShade);
                b.Ellipse2(p.x - 0.5f, p.y + 0.5f, 4.5f, 2.3f, leaf);
            }
            StandardBody(b, Rgb(150, 175, 100), 15f, 12f, 13f, 32f, 12f);
            b.Line(0.7f, vine, V(20, 10), V(30, 14), V(38, 20), V(44, 22));
            b.Ellipse(26f, 13f, 2f, 1.3f, leaf);
            b.Ellipse(40f, 21f, 2f, 1.3f, leaf);
            b.Line(0.7f, vine, V(32, 43), V(34, 48), V(31, 51));
            b.Ellipse(34f, 51f, 2.5f, 1.5f, leaf);
            b.Face(CenterX, 31f, 4.5f, EyeStyle.Round);
        }
    }
}
