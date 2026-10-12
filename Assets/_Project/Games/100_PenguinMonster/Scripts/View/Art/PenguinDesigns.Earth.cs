using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>こけ・いわタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Stone = Rgb(150, 140, 130);

        /// <summary>No.5 コケペン: 無口でのんびり。こけむした体を丸めて大きな毛玉のように座り込み、下から顔だけのぞかせる。背中に芽とキノコ</summary>
        private static void Koke(PenguinBrush b)
        {
            var moss = Rgb(90, 160, 80);
            var mossShade = PenguinPalette.Shade(moss);
            var sprout = Rgb(120, 200, 90);
            // 頭と体を1つの丸にまとめ、こけのこぶを上半分にだけ付けて「丸い小山」のシルエットにする
            b.Shaded(9f, 4f, 4f, 3f, Stone);
            b.Foot(22f, 3f, 10f, 0.9f);
            b.Shaded(35f, 22f, 21f, 19f, moss);
            for (int degree = 30; degree <= 170; degree += 28)
            {
                float angle = degree * Mathf.Deg2Rad;
                b.Shaded(35f + Mathf.Cos(angle) * 19f, 22f + Mathf.Sin(angle) * 17f, 5f, 4.5f, moss);
            }
            b.Line(0.6f, mossShade, V(42, 30), V(48, 26));
            b.Line(0.6f, mossShade, V(30, 36), V(36, 34));
            b.Rect(48, 31, 3, 6, PenguinPalette.Belly);
            b.Shaded(49.5f, 38f, 6f, 3.5f, Rgb(200, 80, 60));
            b.Dot(47, 39, PenguinPalette.Belly);
            b.Dot(51, 38, PenguinPalette.Belly);
            b.Line(0.6f, Rgb(50, 110, 50), V(34, 44), V(36, 52));
            b.Ellipse(32f, 53f, 3f, 1.5f, sprout);
            b.Ellipse(39f, 54f, 3f, 1.5f, sprout);
            b.Shaded(20f, 16f, 10f, 9f, moss);
            b.White(18f, 15f, 7.5f, 6.5f);
            b.Flipper(V(28, 12), V(22, 6), 2.5f, mossShade);
            b.Shaded(54f, 4f, 3.5f, 3f, Stone);
            b.TurnedFace(18f, 16f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
        }

        /// <summary>No.6 イワペン: 力持ちで のんびり。岩を甲羅のように背負い、低く前かがみでヒレを地面につく。頭は小さい</summary>
        private static void Iwa(PenguinBrush b)
        {
            var brown = Rgb(150, 104, 70);
            var brownShade = PenguinPalette.Shade(brown);
            var stoneShade = PenguinPalette.Darken(Stone, 0.7f);
            b.Foot(44f, 3f, 0f, 1.3f);
            b.ShadedTilted(32f, 14f, 20f, 11f, 8f, brown);
            b.BellyTilted(27f, 11f, 13f, 6f, 8f);
            b.Polygon(V(42, 32), stoneShade, V(20, 22), V(22, 38), V(32, 50), V(48, 52), V(60, 42), V(61, 24), V(52, 14), V(34, 14));
            b.Polygon(V(40, 34), Stone, V(23, 24), V(25, 37), V(33, 47), V(47, 49), V(57, 40), V(58, 26), V(50, 18), V(35, 18));
            b.Polygon(V(34, 42), Rgb(186, 176, 164), V(28, 38), V(32, 44), V(40, 46), V(42, 42), V(36, 38));
            b.Line(0.6f, stoneShade, V(52, 44), V(55, 36), V(52, 30));
            b.Line(0.6f, stoneShade, V(30, 28), V(36, 25));
            b.Foot(20f, 3f, 0f, 1.3f);
            b.Shaded(13f, 21f, 8.5f, 7.5f, brown);
            b.White(11f, 20f, 6f, 5f);
            b.Flipper(V(22, 18), V(14, 4), 3.5f, brownShade);
            b.TurnedFace(11f, 21f, FaceLeft, EyeStyle.Sleepy, eyeGap: 5f);
        }

        /// <summary>No.19 ノリペン: 臆病で引っ込み思案。ぬれた海藻の長い髪を背中から床まで引きずり、ヒレを胸の前で合わせて縮こまる</summary>
        private static void Nori(PenguinBrush b)
        {
            var kelp = Rgb(80, 170, 90);
            var kelpShade = PenguinPalette.Shade(kelp);
            var body = Rgb(60, 110, 80);
            // 髪は頭のてっぺんから後ろ（右）へ流して床まで垂らし、マントのような裾広がりのシルエットにする
            b.Polygon(V(42, 26), kelpShade, V(26, 46), V(40, 48), V(50, 38), V(56, 22), V(62, 6), V(54, 2), V(40, 4), V(36, 30));
            for (int i = 0; i < 4; i++)
            {
                float x = 40f + i * 5f;
                b.Line(1.4f, i % 2 == 0 ? kelp : kelpShade, NoriStrand(x, 44f - i * 3f, 3f));
            }
            b.Foot(27f, 3f, 15f);
            b.Foot(36f, 3f, -15f);
            b.ShadedTilted(32f, 16f, 10f, 13f, 6f, body);
            b.BellyTilted(29f, 15f, 6f, 9.5f, 6f);
            b.Shaded(28f, 34f, 11f, 10.5f, body);
            b.White(25f, 33f, 8f, 7f);
            b.Ellipse(31f, 42f, 11f, 5f, kelp);
            b.Line(1.6f, kelp, V(22, 42), V(19, 36), V(20, 30));
            b.Flipper(V(36, 20), V(26, 18), 2.5f, PenguinPalette.Shade(body));
            b.Flipper(V(24, 22), V(27, 17), 2f, PenguinPalette.Shade(body));
            b.TurnedFace(25f, 33f, FaceLeft, EyeStyle.Worried, eyeGap: 6f);
        }

        /// <summary>頭から後ろへ流れ、下へ垂れて床に着く海藻の毛束。少し波打たせて濡れた感じを出す</summary>
        private static Vector2[] NoriStrand(float x, float top, float bottom)
        {
            const float Step = 4f;
            const float Sway = 1.5f;
            const float Frequency = 0.5f;
            const float Spread = 0.25f;
            int count = Mathf.FloorToInt((top - bottom) / Step) + 1;
            var points = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                float y = top - i * Step;
                // 下ほど外へ広げ、裾広がりにする
                points[i] = V(x + (top - y) * Spread + Mathf.Sin(y * Frequency) * Sway, y);
            }
            return points;
        }

        /// <summary>No.29 キノコペン: ずる賢くて怪しい。大きな毒キノコの傘を前へ傾けて目元を隠し、低くかがんで上目づかいでのぞく。胞子がただよう</summary>
        private static void Kinoko(PenguinBrush b)
        {
            var cap = Rgb(150, 70, 170);
            var body = Rgb(100, 130, 80);
            const float Tilt = 0.25f;
            b.Foot(22f, 3f, 10f);
            b.Foot(42f, 3f, -10f);
            b.ShadedTilted(33f, 13f, 14f, 10f, 15f, body);
            b.BellyTilted(29f, 12f, 9f, 6.5f, 15f);
            b.Shaded(24f, 23f, 10.5f, 9.5f, body);
            b.White(21f, 22f, 7.5f, 6f);
            b.Flipper(V(32, 14), V(22, 8), 2.5f, PenguinPalette.Shade(body));
            b.TurnedFace(21f, 23f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            // 傘は下が平らな半円を前（左）へ傾けて、顔の上半分にかぶせる。楕円を消しゴムで切ると頭まで消えてしまうため
            b.Polygon(V(33f, 36f), PenguinPalette.Shade(cap), ArcPoints(33f, 33f, 30f, 22f, Tilt, Mathf.PI + Tilt));
            b.Polygon(V(32f, 37f), cap, ArcPoints(32f, 34f, 28f, 20f, Tilt, Mathf.PI + Tilt));
            foreach (var p in new[] { V(16, 38), V(28, 50), V(44, 48), V(24, 40), V(38, 41), V(52, 40), V(36, 51) })
            {
                b.Ellipse(p.x, p.y, 2.2f, 2f, PenguinPalette.Belly);
            }
            foreach (var p in new[] { V(6, 14), V(10, 8), V(56, 22), V(60, 14) })
            {
                b.Ellipse(p.x, p.y, 1.2f, 1.2f, PenguinPalette.Shade(cap));
            }
        }

        /// <summary>No.30 ツタペン: 身軽ないたずら好き。上から垂れたツタに片ヒレでぶら下がって揺れ、葉っぱの翼を広げる。足は宙に浮く</summary>
        private static void Tsuta(PenguinBrush b)
        {
            var leaf = Rgb(110, 190, 80);
            var leafShade = PenguinPalette.Shade(leaf);
            var vine = Rgb(60, 120, 50);
            var body = Rgb(150, 175, 100);
            var bodyShade = PenguinPalette.Shade(body);
            b.Line(1f, vine, V(52, 62), V(48, 56), V(50, 50), V(45, 44));
            b.Ellipse(53f, 55f, 2.5f, 1.5f, leaf);
            b.Ellipse(46f, 51f, 2.5f, 1.5f, leaf);
            b.Flipper(V(36, 34), V(45, 45), 3f, bodyShade);
            // 体はぶら下がった手の下で斜めに振れ、足を後ろへ流して勢いを見せる
            b.Foot(42f, 9f, -50f);
            b.Foot(36f, 7f, -30f);
            b.ShadedTilted(33f, 21f, 9f, 13f, -30f, body);
            b.BellyTilted(30f, 21f, 5.5f, 9.5f, -30f);
            b.Line(0.7f, vine, V(26, 14), V(32, 18), V(40, 22));
            b.Ellipse(33f, 17f, 2f, 1.3f, leaf);
            b.Shaded(27f, 37f, 10.5f, 10f, body);
            b.White(24f, 36f, 7.5f, 6.5f);
            foreach (var p in new[] { V(9, 36), V(6, 25), V(11, 15) })
            {
                b.Line(0.6f, vine, V(24, 26), p);
                b.Tilted(p.x, p.y, 6.5f, 3.5f, (p.y - 26f) * 2f, leafShade);
                b.Tilted(p.x - 0.5f, p.y + 0.5f, 5f, 2.3f, (p.y - 26f) * 2f, leaf);
            }
            b.TurnedFace(24f, 37f, FaceLeft, EyeStyle.Sparkle, eyeGap: 6f, open: true);
        }
    }
}
