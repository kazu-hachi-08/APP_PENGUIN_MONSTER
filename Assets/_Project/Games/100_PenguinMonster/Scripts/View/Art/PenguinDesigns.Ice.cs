using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>こおりタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 IceLight = Rgb(200, 240, 255);
        private static readonly Color32 IceShade = Rgb(130, 196, 230);

        /// <summary>No.1 ユキペン: 主役。いちばん素直なペンギンに、氷の結晶の冠</summary>
        private static void Yuki(PenguinBrush b)
        {
            StandardBody(b, Rgb(120, 190, 235), 15f, 15f, 13f, 33f, 14f);
            b.Triangle2(V(21, 41), V(27, 45), V(17, 54), IceLight);
            b.Triangle(V(28, 45), V(36, 45), V(32, 60), IceLight);
            b.Triangle(V(32, 45), V(36, 45), V(32, 60), IceShade);
            b.Face(CenterX, 32f, 5f);
        }

        /// <summary>No.7 ヒョウガペン: 氷河のよろいで角ばった体。肩に氷のとげ</summary>
        private static void Hyouga(PenguinBrush b)
        {
            var armor = Rgb(160, 210, 236);
            var armorShade = PenguinPalette.Shade(armor);
            b.Feet(CenterX, 10f);
            b.Triangle2(V(11, 28), V(19, 34), V(6, 47), IceLight);
            b.Triangle2(V(14, 30), V(19, 34), V(6, 47), IceShade);
            b.Polygon(V(32, 20), armorShade, V(14, 3), V(9, 12), V(9, 30), V(16, 36), V(48, 36), V(55, 30), V(55, 12), V(50, 3));
            b.Polygon(V(30, 22), armor, V(15, 6), V(12, 13), V(12, 28), V(18, 33), V(46, 33), V(51, 28), V(51, 14), V(47, 6));
            b.Line(0.6f, armorShade, V(12, 20), V(32, 16), V(52, 20));
            b.Line(0.6f, armorShade, V(32, 33), V(32, 6));
            b.Sparkle(24, 25, IceLight);
            b.Shaded(CenterX, 40f, 11f, 11f, Rgb(60, 80, 130));
            b.White(CenterX, 39f, 8f, 6.5f);
            b.Face(CenterX, 39f, 4f);
        }

        /// <summary>No.8 ホシペン: 頭に星をのせて ふわふわ浮く。足は無く、まわりにきらめき</summary>
        private static void Hoshi(PenguinBrush b)
        {
            var body = Rgb(196, 206, 250);
            var sparkle = Rgb(255, 240, 150);
            b.Shaded2(22f, 24f, 3f, 5.5f, PenguinPalette.Shade(body));
            b.Shaded(CenterX, 23f, 11f, 10f, body);
            b.Belly(CenterX, 22f, 7f, 7f);
            b.Shaded(CenterX, 36f, 11f, 11f, body);
            b.White(CenterX, 35f, 8f, 6.5f);
            b.Star(CenterX, 52f, 7f, 3f, PenguinPalette.Gold);
            b.Star(31f, 53f, 4f, 1.7f, sparkle);
            b.Face(CenterX, 35f, 4f, smile: true);
            b.Sparkle(12, 40, sparkle);
            b.Sparkle(52, 28, sparkle);
            b.Sparkle(14, 20, sparkle);
            b.Sparkle(53, 46, sparkle);
            b.Sparkle(36, 7, sparkle);
        }

        /// <summary>No.9 コウテイペン: 背が高い王さま。赤いマントと冠、耳の黄色はコウテイペンギンの印</summary>
        private static void Koutei(PenguinBrush b)
        {
            var navy = Rgb(50, 60, 100);
            var cape = Rgb(200, 50, 70);
            b.Triangle(V(32, 44), V(5, 2), V(59, 2), PenguinPalette.Shade(cape));
            b.Triangle(V(32, 40), V(9, 4), V(55, 4), cape);
            b.Rect(5, 1, 55, 3, PenguinPalette.Belly);
            for (int x = 8; x < 58; x += 6)
            {
                b.Dot(x, 2, PenguinPalette.Eye);
            }
            b.Feet(CenterX, 7f, 5f);
            b.Shaded2(19f, 22f, 3.5f, 8f, PenguinPalette.Shade(navy));
            b.Shaded(CenterX, 20f, 14f, 16f, navy);
            b.Belly(CenterX, 18f, 9.5f, 12.5f);
            if (b.Front)
            {
                b.Ellipse(CenterX, 28f, 8f, 3.5f, Rgb(255, 230, 150));
            }
            b.Shaded(CenterX, 41f, 11f, 11f, navy);
            b.White(CenterX, 40f, 8f, 6.5f);
            b.Ellipse2(22.5f, 39f, 2.5f, 4f, Rgb(250, 190, 60));
            b.Face(CenterX, 40f, 4f);
            b.Rect(26, 50, 12, 3, PenguinPalette.Gold);
            b.Triangle2(V(26, 52), V(30, 52), V(27, 58), PenguinPalette.Gold);
            b.Triangle(V(30, 52), V(34, 52), V(32, 59), PenguinPalette.Gold);
            b.Rect(31, 51, 2, 1, Rgb(230, 60, 90));
        }

        /// <summary>No.10 リュウヒョウペン: 流氷にのって海をただよう。横に広い氷の板が目印</summary>
        private static void Ryuuhyou(PenguinBrush b)
        {
            var body = Rgb(70, 170, 200);
            b.Ellipse(CenterX, 3f, 30f, 3f, Rgb(70, 140, 220));
            b.Rect(5, 4, 54, 7, IceShade);
            b.Polygon(V(32, 13), IceLight, V(5, 11), V(59, 11), V(54, 16), V(10, 16));
            b.Triangle(V(7, 14), V(13, 14), V(9, 26), IceLight);
            b.Triangle(V(51, 14), V(57, 14), V(54, 23), IceLight);
            b.Feet(CenterX, 6f, 17f);
            b.Shaded2(22f, 25f, 3f, 6f, PenguinPalette.Shade(body));
            b.Shaded(CenterX, 25f, 11f, 9f, body);
            b.Belly(CenterX, 24f, 7f, 6.5f);
            b.Shaded(CenterX, 38f, 11f, 11f, body);
            b.White(CenterX, 37f, 8f, 6.5f);
            b.Face(CenterX, 37f, 4f);
        }
    }
}
