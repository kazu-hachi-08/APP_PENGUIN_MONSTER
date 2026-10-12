using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 伝説ランクのデザイン（仕様書 §6）。キャンバスいっぱいに描き、金の飾りを付ける。
    /// Paint が外側に黒と金の輪郭を2周回すので、外周 2 ドットは空けておく
    /// </summary>
    public static partial class PenguinDesigns
    {
        private const float LegendFeetY = 4.5f;

        /// <summary>No.48 ナンキョクペン: 巨大な体に氷の王冠。背中に氷山を背負う（ユキオの切り札）</summary>
        private static void Nankyoku(PenguinBrush b)
        {
            var body = Rgb(70, 120, 190);
            b.Polygon(V(32, 30), IceShade, V(3, 12), V(4, 38), V(10, 56), V(17, 44), V(23, 50), V(32, 44),
                V(41, 50), V(47, 44), V(54, 58), V(60, 38), V(60, 12));
            b.Polygon(V(30, 32), IceLight, V(5, 14), V(6, 37), V(10, 52), V(16, 41), V(23, 47), V(30, 42), V(30, 14));
            b.Feet(CenterX, 10f, LegendFeetY);
            b.Shaded2(11f, 20f, 4.5f, 9f, PenguinPalette.Shade(body));
            b.Shaded(CenterX, 20f, 20f, 16f, body);
            b.Belly(CenterX, 19f, 14f, 12.5f);
            b.Shaded(CenterX, 41f, 13f, 12f, body);
            b.White(CenterX, 40f, 10f, 8f);
            b.Face(CenterX, 40f, 5f, EyeStyle.Sharp);
            b.Rect(23, 51, 18, 3, PenguinPalette.Gold);
            b.Triangle2(V(23, 53), V(28, 53), V(23, 59), IceLight);
            b.Triangle(V(28, 53), V(36, 53), V(32, 61), IceLight);
            b.Triangle(V(32, 53), V(36, 53), V(32, 61), IceShade);
            b.Rect(31, 52, 2, 1, Rgb(90, 200, 255));
            b.Sparkle(14, 50, PenguinPalette.Belly);
            b.Sparkle(50, 30, PenguinPalette.Belly);
        }

        /// <summary>No.49 ライテイペン: 雷雲の翼から稲妻を落とす。背中に金の輪（ライカの切り札）</summary>
        private static void Raitei(PenguinBrush b)
        {
            var body = Rgb(250, 228, 130);
            var cloud = Rgb(110, 115, 145);
            b.Ellipse(CenterX, 40f, 19f, 19f, PenguinPalette.GoldShade);
            b.Ellipse(CenterX - 0.5f, 40.5f, 18f, 18f, PenguinPalette.Gold);
            b.Erase(CenterX, 40f, 16f, 16f);
            for (int i = 0; i < 4; i++)
            {
                var p = OnEllipse(CenterX, 40f, 17f, 17f, Mathf.PI / 4f + i * Mathf.PI / 2f);
                b.Shaded(p.x, p.y, 2.5f, 2.5f, PenguinPalette.Gold);
            }
            b.Line2(1.4f, Bolt, V(10, 26), V(14, 18), V(9, 15), V(13, 6));
            StormWing(b, cloud, 11f);
            StormWing(b, cloud, 53f);
            b.Feet(CenterX, 8f, LegendFeetY);
            b.Shaded2(19f, 18f, 3.5f, 7f, PenguinPalette.Shade(body));
            b.Shaded(CenterX, 18f, 15f, 14f, body);
            b.Belly(CenterX, 17f, 10f, 10.5f);
            b.Shaded(CenterX, 37f, 12f, 12f, body);
            b.White(CenterX, 36f, 9f, 7f);
            var crest = new[] { V(33, 48), V(29, 53), V(34, 55), V(30, 61) };
            b.Line(1.4f, BoltShade, crest);
            b.Line(0.7f, Bolt, crest);
            b.Face(CenterX, 36f, 4.5f, EyeStyle.Cross);
        }

        /// <summary>雷雲の翼。ふくらみを全部影色で塗ってから明るい色を重ねる（1つずつ影を付けると中に線が残るため）</summary>
        private static void StormWing(PenguinBrush b, Color32 cloud, float x)
        {
            var puffs = new[] { V(x - 6f, 34f), V(x + 1f, 38f), V(x + 6f, 32f), V(x - 2f, 29f) };
            foreach (var p in puffs)
            {
                b.Ellipse(p.x, p.y, 5.5f, 5f, PenguinPalette.Shade(cloud));
            }
            foreach (var p in puffs)
            {
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 4.3f, 3.8f, cloud);
            }
        }

        /// <summary>No.50 キョクヤペン: 星空のマントを大きく広げ、すそにオーロラの帯。頭に三日月の冠（ヨミの切り札）</summary>
        private static void Kyokuya(PenguinBrush b)
        {
            var cape = Rgb(28, 30, 72);
            var body = Rgb(80, 64, 130);
            b.Polygon(V(32, 30), cape, V(2, 4), V(62, 4), V(61, 30), V(54, 44), V(42, 46), V(22, 46), V(10, 44), V(3, 30));
            Ribbon(b, Rgb(120, 240, 180), V(4, 12), V(14, 16), V(24, 12), V(34, 16), V(44, 12), V(54, 16), V(60, 12));
            Ribbon(b, Rgb(240, 140, 220), V(4, 7), V(14, 9), V(24, 6), V(34, 9), V(44, 6), V(54, 9), V(60, 6));
            foreach (var p in new[] { V(8, 36), V(55, 38), V(12, 24), V(50, 26), V(6, 20), V(58, 21) })
            {
                b.Dot((int)p.x, (int)p.y, PenguinPalette.Gold);
            }
            b.Sparkle(15, 40, PenguinPalette.Gold);
            b.Sparkle(49, 34, PenguinPalette.Gold);
            b.Feet(CenterX, 8f, LegendFeetY);
            b.Shaded(CenterX, 19f, 14f, 14f, body);
            b.Belly(CenterX, 18f, 9f, 10.5f);
            b.Shaded(CenterX, 38f, 13f, 12f, body);
            b.White(CenterX, 37f, 10f, 7.5f);
            b.Face(CenterX, 37f, 5f, EyeStyle.Line);
            b.Rect(24, 48, 16, 3, PenguinPalette.Gold);
            b.Triangle2(V(24, 50), V(28, 50), V(25, 54), PenguinPalette.Gold);
            b.Ellipse(31f, 56f, 5.5f, 5.5f, Rgb(255, 230, 120));
            b.Erase(34.5f, 57.5f, 4.5f, 4.5f);
        }
    }
}
