using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>とりタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.20 ツバサペン: 勇敢な夢見がち。胸を張って空を見上げ、大きさの違う翼を前後に広げる。冠羽は後ろへ流れる</summary>
        private static void Tsubasa(PenguinBrush b)
        {
            var sky = Rgb(110, 180, 230);
            var skyShade = PenguinPalette.Shade(sky);
            var tip = Rgb(240, 248, 255);
            // 奥の翼は高く掲げ、手前の翼は低く大きく広げて、左右非対称のシルエットにする
            Wing(b, V(40, 30), new[] { V(42, 38), V(52, 56), V(61, 52), V(62, 40), V(56, 28) }, sky, skyShade, tip);
            b.Triangle(V(34, 47), V(38, 45), V(52, 58), skyShade);
            b.Triangle(V(31, 48), V(35, 48), V(44, 62), sky);
            b.Foot(29f, 3f);
            b.Foot(41f, 4f, -20f);
            b.ShadedTilted(35f, 19f, 9f, 15f, -10f, sky);
            b.BellyTilted(33f, 18f, 5.5f, 11f, -10f);
            b.Shaded(31f, 40f, 10f, 10f, sky);
            b.White(28f, 40f, 7.5f, 7f);
            Wing(b, V(28, 27), new[] { V(30, 32), V(12, 40), V(3, 32), V(5, 22), V(14, 15), V(28, 20) }, sky, skyShade, tip);
            b.TurnedFace(28f, 42f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
        }

        /// <summary>付け根から扇形に羽を並べた翼。明暗を交互にして、羽先（外周の点の間）を白く塗る</summary>
        private static void Wing(PenguinBrush b, Vector2 root, Vector2[] rim, Color32 light, Color32 shade, Color32 tip)
        {
            const float TipRate = 0.15f;
            for (int i = 0; i + 1 < rim.Length; i++)
            {
                b.Triangle(root, rim[i], rim[i + 1], i % 2 == 0 ? shade : light);
            }
            for (int i = 1; i + 2 < rim.Length; i++)
            {
                var inner = Vector2.Lerp(Vector2.Lerp(rim[i], rim[i + 1], 0.5f), root, TipRate);
                b.Triangle(rim[i], rim[i + 1], inner, tip);
            }
        }

        /// <summary>No.21 カゼペン: 気まぐれで元気。小さな竜巻に乗って前のめりに飛び、片ヒレで行き先を指す。斜めに伸びた体と、細い竜巻・長いマフラーの斜めのシルエット</summary>
        private static void Kaze(PenguinBrush b)
        {
            var wind = Rgb(220, 235, 245);
            var windShade = Rgb(170, 195, 215);
            var mint = Rgb(120, 210, 170);
            var mintShade = PenguinPalette.Shade(mint);
            var scarf = Rgb(255, 200, 60);
            // 竜巻は輪を左右にずらして重ね、ねじれて回っているように見せる。下ほど細くして浮いている足場にする
            float[] ringY = { 3f, 7f, 11f, 15f, 20f, 25f };
            float[] ringX = { 46f, 43f, 46f, 42f, 45f, 41f };
            float[] ringRadius = { 2.5f, 4.5f, 6.5f, 9f, 11.5f, 14f };
            for (int i = 0; i < ringY.Length; i++)
            {
                b.Ellipse(ringX[i], ringY[i], ringRadius[i], 3f, windShade);
                b.Ellipse(ringX[i] - 1f, ringY[i] + 0.5f, ringRadius[i] - 1f, 2f, wind);
            }
            // マフラーは体より長く後ろへなびかせ、速さを見せる
            b.Line(2f, scarf, V(28, 42), V(38, 47), V(50, 48), V(61, 53));
            b.Line(1.5f, PenguinPalette.Shade(scarf), V(29, 40), V(40, 43), V(51, 42), V(59, 46));
            b.Flipper(V(38, 32), V(55, 33), 3f, mintShade);
            b.Foot(46f, 26f, 30f, 0.9f);
            b.Foot(41f, 26f, -10f, 0.9f);
            b.ShadedTilted(34f, 32f, 8f, 12f, 55f, mint);
            b.BellyTilted(31f, 30f, 5f, 9f, 55f);
            b.Shaded(21f, 41f, 10f, 9.5f, mint);
            b.White(18f, 40f, 7f, 6f);
            // 手前のヒレは前上へ突き出し、行き先を指さす
            b.Flipper(V(22, 35), V(6, 44), 3f, mintShade);
            b.TurnedFace(18f, 41f, FaceLeft, EyeStyle.Round, eyeGap: 6f, open: true);
        }
    }
}
