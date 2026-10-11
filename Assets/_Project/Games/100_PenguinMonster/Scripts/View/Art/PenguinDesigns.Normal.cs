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
            b.Face(CenterX, 31f, 5f, smile: true);
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
            b.Face(CenterX, 31f, 6f);
            b.Ellipse2(12f, 20f, 3.5f, 6f, fluffShade);
        }
    }
}
