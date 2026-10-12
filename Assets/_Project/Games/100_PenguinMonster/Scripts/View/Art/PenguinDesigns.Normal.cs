using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>ノーマルタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.11 ペンタ: いちばん ふつうのペンギン。片手を上げて あいさつ。未登録の No の代わりにも使う</summary>
        private static void Penta(PenguinBrush b)
        {
            var navy = Rgb(52, 64, 110);
            var navyShade = PenguinPalette.Shade(navy);
            b.Feet(CenterX, 7.5f);
            b.Shaded(18f, 15f, 3.5f, 7f, navyShade);
            b.Line(3f, navyShade, V(46, 20), V(53, 30));
            b.Shaded(CenterX, 15f, 15f, 13f, navy);
            b.Belly(CenterX, 14f, 11f, 10.5f);
            b.Shaded(CenterX, 32f, 14f, 14f, navy);
            b.White(CenterX, 31f, 11f, 9.5f);
            b.Line(0.8f, navyShade, V(32, 45), V(30, 50), V(33, 52));
            b.Face(CenterX, 31f, 5f, EyeStyle.Smile);
        }

        /// <summary>No.12 モフペン: コウテイペンギンのひなの綿毛で まんまる。黒いぼうしと白いお面</summary>
        private static void Mofu(PenguinBrush b)
        {
            var fluff = Rgb(196, 198, 212);
            var fluffShade = PenguinPalette.Darken(fluff, 0.75f);
            var cap = Rgb(60, 62, 80);
            const int Puffs = 14;
            b.Feet(CenterX, 8f);
            b.Ellipse(CenterX, 24f, 20f, 19f, fluffShade);
            for (int i = 0; i < Puffs; i++)
            {
                float angle = i * Mathf.PI * 2f / Puffs;
                b.Ellipse(CenterX + Mathf.Cos(angle) * 18f, 24f + Mathf.Sin(angle) * 17f, 4.5f, 4.5f, fluffShade);
            }
            b.Ellipse(31f, 25f, 18.5f, 17.5f, fluff);
            for (int i = 0; i < Puffs; i++)
            {
                float angle = i * Mathf.PI * 2f / Puffs;
                // 右下は影として残すので、明るい綿毛を重ねない
                if (Mathf.Sin(angle) < -0.2f || Mathf.Cos(angle) > 0.5f) continue;
                b.Ellipse(31f + Mathf.Cos(angle) * 17f, 25f + Mathf.Sin(angle) * 16f, 3.5f, 3.5f, fluff);
            }
            b.Ellipse(CenterX, 37f, 15f, 8f, cap);
            b.Ellipse(29f, 47f, 3f, 3f, cap);
            b.Ellipse(33f, 49f, 3.5f, 3.5f, cap);
            b.Ellipse(36f, 46f, 2.5f, 2.5f, cap);
            if (b.Front)
            {
                b.Ellipse2(25f, 30f, 8f, 7f, PenguinPalette.Belly);
                b.Ellipse(CenterX, 27f, 9f, 6f, PenguinPalette.Belly);
            }
            b.Face(CenterX, 31f, 6f, EyeStyle.Dot);
            b.Ellipse2(12f, 20f, 3.5f, 6f, fluffShade);
        }

        /// <summary>No.31 マジシャンペン: シルクハットとマント。手に星のステッキ</summary>
        private static void Magician(PenguinBrush b)
        {
            var cape = Rgb(70, 50, 120);
            var lining = Rgb(200, 50, 70);
            var hat = Rgb(40, 40, 56);
            b.Triangle(V(32, 34), V(6, 2), V(58, 2), cape);
            b.Triangle(V(32, 30), V(12, 3), V(52, 3), lining);
            b.Line(0.7f, Rgb(120, 90, 60), V(46, 18), V(55, 30));
            b.Star(56f, 32f, 4.5f, 2f, PenguinPalette.Gold);
            StandardBody(b, Rgb(220, 220, 230), 15f, 12f, 12f, 31f, 12f);
            b.Rect(19, 41, 26, 3, hat);
            b.Rect(23, 44, 18, 12, hat);
            b.Rect(23, 45, 18, 2, lining);
            b.Face(CenterX, 30f, 4.5f, EyeStyle.Cross);
        }

        /// <summary>No.32 ボクサーペン: 大きな赤いグローブを構え、はちまきの端がなびく</summary>
        private static void Boxer(PenguinBrush b)
        {
            var glove = Rgb(220, 50, 50);
            var band = Rgb(240, 240, 240);
            StandardBody(b, Rgb(60, 70, 110), 15f, 14f, 13f, 32f, 12f);
            b.Rect(20, 38, 24, 3, band);
            b.Line(0.8f, band, V(44, 39), V(50, 42), V(55, 40));
            b.Line(0.8f, band, V(44, 39), V(49, 37), V(54, 35));
            b.Shaded2(14f, 20f, 6f, 6f, glove);
            b.Rect(10, 13, 9, 2, band);
            b.Rect(45, 13, 9, 2, band);
            b.Face(CenterX, 31f, 4.5f, EyeStyle.Sharp);
        }
    }
}
