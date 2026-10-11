using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>かみなりタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Bolt = Rgb(255, 236, 70);
        private static readonly Color32 BoltShade = Rgb(200, 150, 30);

        /// <summary>No.4 イナペン: いなずま形の翼を左右に広げる</summary>
        private static void Ina(PenguinBrush b)
        {
            var wing = new[] { V(22, 24), V(12, 30), V(17, 33), V(5, 46) };
            b.Line2(2.2f, BoltShade, wing);
            b.Line2(1.3f, Bolt, wing);
            StandardBody(b, Rgb(240, 200, 50), 15f, 12f, 13f, 32f, 12f);
            var tuft = new[] { V(34, 43), V(30, 49), V(34, 50), V(30, 57) };
            b.Line(1.3f, BoltShade, tuft);
            b.Line(0.7f, Bolt, tuft);
            b.Face(CenterX, 31f, 4f, EyeStyle.Round);
        }

        /// <summary>No.17 ライペン: 背中に雷さまの太鼓の輪を背負う</summary>
        private static void Rai(PenguinBrush b)
        {
            var body = Rgb(90, 90, 170);
            var drum = Rgb(210, 60, 50);
            const float RingY = 30f;
            const float RingRadius = 23f;
            const int Drums = 8;
            b.Ellipse(CenterX, RingY, RingRadius + 1f, RingRadius + 1f, PenguinPalette.GoldShade);
            b.Erase(CenterX, RingY, RingRadius - 1f, RingRadius - 1f);
            for (int i = 0; i < Drums; i++)
            {
                float angle = i * Mathf.PI * 2f / Drums;
                float x = CenterX + Mathf.Cos(angle) * RingRadius;
                float y = RingY + Mathf.Sin(angle) * RingRadius;
                b.Shaded(x, y, 4.5f, 4.5f, drum);
                b.Ellipse(x - 0.5f, y + 0.5f, 1.8f, 1.8f, PenguinPalette.Gold);
            }
            b.Feet(CenterX, 7f);
            b.Shaded2(20f, 16f, 3f, 6f, PenguinPalette.Shade(body));
            b.Shaded(CenterX, 16f, 13f, 12f, body);
            b.Belly(CenterX, 15f, 9f, 9f);
            b.Shaded(CenterX, 32f, 13f, 13f, body);
            b.White(CenterX, 31f, 10f, 7.5f);
            b.Face(CenterX, 31f, 5f, EyeStyle.Round);
        }

        /// <summary>No.18 アラシペン: 体が嵐の雲。下に いなずまを垂らして浮かぶ</summary>
        private static void Arashi(PenguinBrush b)
        {
            var cloud = Rgb(120, 125, 150);
            var cloudShade = PenguinPalette.Shade(cloud);
            b.Line(1.6f, Bolt, V(30, 16), V(36, 9), V(29, 7), V(33, 1));
            b.Line(0.6f, Water, V(14, 12), V(13, 9));
            b.Line(0.6f, Water, V(48, 13), V(47, 10));
            b.Line(0.6f, Water, V(22, 8), V(21, 5));
            b.Shaded2(9f, 22f, 3f, 5.5f, cloudShade);
            // ふくらみを全部影色で塗ってから明るい色を重ねる。1つずつ影を付けると雲の中に線が残る
            var puffs = new[]
            {
                new Vector4(18, 25, 9, 8), new Vector4(46, 25, 9, 8), new Vector4(24, 36, 9, 8),
                new Vector4(40, 36, 9, 8), new Vector4(32, 41, 9, 8), new Vector4(32, 25, 15, 11),
            };
            foreach (var p in puffs)
            {
                b.Ellipse(p.x, p.y, p.z, p.w, cloudShade);
            }
            foreach (var p in puffs)
            {
                b.Ellipse(p.x - 1f, p.y + 1f, p.z - 1.5f, p.w - 1.5f, cloud);
            }
            b.White(CenterX, 27f, 10f, 7f);
            b.Face(CenterX, 28f, 4.5f, EyeStyle.Sleepy);
        }
    }
}
