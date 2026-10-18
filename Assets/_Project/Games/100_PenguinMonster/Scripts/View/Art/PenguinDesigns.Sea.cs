using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>うみタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Water = Rgb(90, 170, 230);
        private static readonly Color32 DeepWater = Rgb(50, 110, 190);

        /// <summary>No.2 ナミペン: 陽気な波乗り。右から巻いてくる大波の斜面に低く構え、両ヒレを広げてバランスを取る</summary>
        private static void Nami(PenguinBrush b)
        {
            var body = Rgb(60, 120, 210);
            var bodyShade = PenguinPalette.Shade(body);
            // 波は右ほど高くして頭の上へ巻かせる。斜面に乗っていることがシルエットだけで分かるように
            b.Ellipse(53f, 28f, 9f, 18f, Water);
            b.Ellipse(44f, 45f, 12f, 6f, Water);
            b.Ellipse(33f, 42f, 3.5f, 4f, Water);
            b.Ellipse(46f, 39f, 6f, 2.5f, DeepWater);
            b.Line(0.6f, DeepWater, V(56, 14), V(57, 32));
            b.Ellipse(46f, 49f, 8f, 2f, PenguinPalette.Belly);
            b.Ellipse(33f, 46f, 2.5f, 1.5f, PenguinPalette.Belly);
            b.Ellipse(30f, 39f, 1.5f, 1.5f, PenguinPalette.Belly);
            b.Tilted(34f, 10f, 30f, 8f, 12f, Water);
            b.Line(0.7f, DeepWater, V(10, 4), V(24, 7), V(38, 9), V(52, 13));
            b.Flipper(V(34, 26), V(46, 32), 2.5f, bodyShade);
            b.Foot(37f, 15f, 15f);
            b.Foot(18f, 11f, 15f);
            b.ShadedTilted(28f, 21f, 12f, 9f, 15f, body);
            b.BellyTilted(25f, 20f, 8f, 6f, 15f);
            b.Shaded(21f, 33f, 10f, 10f, body);
            b.White(18f, 32f, 7.5f, 6.5f);
            b.Flipper(V(18, 24), V(5, 25), 2.5f, bodyShade);
            b.TurnedFace(18f, 33f, FaceLeft, EyeStyle.Round, eyeGap: 6f, open: true);
            for (int x = 6; x <= 50; x += 8)
            {
                b.Ellipse(x, 12f + (x - 6) * 0.22f, 2.5f, 1.5f, PenguinPalette.Belly);
            }
        }

        /// <summary>No.13 ウズペン: 勢いまかせの暴れん坊。片足のつま先でコマのように回り、両ヒレを水平に伸ばす。腰のまわりで うず潮が渦巻く</summary>
        private static void Uzu(PenguinBrush b)
        {
            var body = Rgb(40, 150, 170);
            var bodyShade = PenguinPalette.Shade(body);
            var ring = Rgb(110, 190, 240);
            const float RingY = 20f;
            const float RingRadiusX = 27f;
            const float RingRadiusY = 6f;
            // 輪の奥側を先に塗り、体を描いてから手前側を重ねて、体に巻き付いて見せる
            b.Line(1.6f, DeepWater, ArcPoints(CenterX, RingY, RingRadiusX, RingRadiusY, 0f, Mathf.PI));
            b.Foot(31f, 4f, 70f, 0.9f);
            b.Foot(42f, 17f, -30f);
            // 下すぼまりのコマ形。足1本で立っているのが分かるよう、おなかの下を細くする
            b.Triangle(V(22, 20), V(42, 20), V(31, 5), bodyShade);
            b.Triangle(V(23, 21), V(40, 21), V(30, 7), body);
            b.Flipper(V(42, 28), V(61, 31), 2.5f, bodyShade);
            b.ShadedTilted(32f, 24f, 11f, 11f, -8f, body);
            b.BellyTilted(30f, 22f, 7f, 8f, -8f);
            b.Shaded(30f, 39f, 11f, 11f, body);
            b.White(27f, 38f, 8f, 7f);
            b.Line(1f, bodyShade, V(32, 49), V(35, 53), V(31, 56), V(28, 52), V(31, 51));
            b.Flipper(V(21, 28), V(2, 26), 2.5f, bodyShade);
            b.TurnedFace(27f, 39f, FaceLeft, EyeStyle.Cross, eyeGap: 6f, open: true);
            b.Line(2f, ring, ArcPoints(CenterX, RingY, RingRadiusX, RingRadiusY, Mathf.PI, Mathf.PI * 2f));
            b.Line(0.6f, PenguinPalette.Belly, ArcPoints(CenterX, RingY - 1f, RingRadiusX - 3f, RingRadiusY - 2f, Mathf.PI * 1.2f, Mathf.PI * 1.6f));
            b.Line(0.6f, ring, V(8, 44), V(12, 48));
            b.Line(0.6f, ring, V(54, 46), V(58, 42));
        }

        /// <summary>No.14 サンゴペン: のんびりした物知り。足を投げ出して座り込み、頭から左右ちがう大きさのサンゴの角が枝分かれする</summary>
        private static void Sango(PenguinBrush b)
        {
            var body = Rgb(235, 140, 170);
            var bodyShade = PenguinPalette.Shade(body);
            var coral = Rgb(250, 110, 100);
            var coralTip = Rgb(255, 180, 160);
            // 角は奥（右）を大きく広げ、木のような重たい頭にする。体は低く座らせて上下の差を出す
            b.Line(1.8f, coral, V(34, 34), V(42, 46), V(50, 52), V(58, 54));
            b.Line(1.4f, coral, V(42, 46), V(44, 58));
            b.Line(1.4f, coral, V(50, 52), V(54, 61));
            b.Line(1.2f, coral, V(46, 50), V(56, 44));
            b.Line(1.6f, coral, V(24, 36), V(18, 46), V(12, 50));
            b.Line(1.2f, coral, V(18, 46), V(20, 55));
            foreach (var p in new[] { V(58, 54), V(44, 58), V(54, 61), V(56, 44), V(12, 50), V(20, 55) })
            {
                b.Ellipse(p.x, p.y, 1.8f, 1.8f, coralTip);
            }
            b.Ellipse(44f, 7f, 7f, 5f, bodyShade);
            b.ShadedTilted(36f, 14f, 17f, 11f, 5f, body);
            b.BellyTilted(31f, 12f, 11f, 8f, 5f);
            b.Foot(15f, 5f, 20f, 1.1f);
            b.Foot(22f, 3f, 10f, 1.1f);
            b.Shaded(27f, 31f, 11f, 10.5f, body);
            b.White(24f, 30f, 8f, 6.5f);
            // ヒレはおなかの上で組んで、くつろいでいる感じを出す
            b.Flipper(V(40, 20), V(28, 14), 3f, bodyShade);
            b.TurnedFace(24f, 31f, FaceLeft, EyeStyle.Dot, eyeGap: 6f);
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

        /// <summary>No.33 シンジュペン: 神秘的でもの静か。開いた貝の中にひざをつき、目を閉じて大きな真珠を胸に抱いて祈る。貝の上ぶたが後光のように立つ</summary>
        private static void Shinju(PenguinBrush b)
        {
            var shell = Rgb(250, 190, 200);
            var shellShade = PenguinPalette.Shade(shell);
            var pearl = Rgb(225, 225, 240);
            const int Ridges = 7;
            // 上ぶたは右へ傾けて立て、ふつうの貝より「開いた」形をシルエットで見せる
            b.Tilted(38f, 34f, 23f, 26f, -15f, shellShade);
            b.Tilted(37f, 35f, 21f, 24f, -15f, shell);
            b.Erase(38f, -2f, 40f, 16f);
            for (int i = 1; i < Ridges; i++)
            {
                float angle = Mathf.PI * 0.05f + i * Mathf.PI * 0.9f / Ridges;
                b.Line(0.5f, shellShade, V(40, 14), OnEllipse(40f, 14f, 20f, 40f, angle));
            }
            b.Polygon(V(34, 7), shellShade, V(6, 12), V(60, 12), V(52, 2), V(14, 2));
            b.Polygon(V(34, 8), shell, V(8, 12), V(58, 12), V(51, 3), V(15, 3));
            b.ShadedTilted(32f, 19f, 10f, 9f, 5f, Water);
            b.BellyTilted(30f, 18f, 6.5f, 6f, 5f);
            b.Shaded(28f, 32f, 9.5f, 9.5f, Water);
            b.White(25f, 31f, 7f, 6f);
            b.TurnedFace(25f, 32f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            b.Flipper(V(36, 22), V(29, 17), 2.5f, PenguinPalette.Shade(Water));
            // 真珠は胸に抱えているので、後ろ姿では体にかくれて見えない
            if (b.Front)
            {
                b.Shaded(25f, 20f, 4.5f, 4.5f, pearl);
                b.Dot(23, 22, PenguinPalette.EyeLight);
            }
            b.Sparkle(14, 26, PenguinPalette.Belly);
            b.Sparkle(10, 38, PenguinPalette.Belly);
        }

        /// <summary>No.34 イルカペン: 遊び好きの人気者。水面から弧を描いて跳び上がり、体をそらせて尾びれを高く振り上げる</summary>
        private static void Iruka(PenguinBrush b)
        {
            var body = Rgb(110, 195, 230);
            var bodyShade = PenguinPalette.Shade(body);
            // 体を斜めの長い流線形にして、ほかの子にない「横に長い・浮いている」シルエットにする
            b.Ellipse(36f, 5f, 18f, 3f, Water);
            b.Ellipse(36f, 5f, 12f, 1.5f, DeepWater);
            // 跳ねたしぶきは水面から離して浮かせる。くっつくと台の脚のように見えた
            b.Ellipse(27f, 13f, 1.5f, 1.5f, Water);
            b.Ellipse(46f, 14f, 1.5f, 1.5f, Water);
            b.Ellipse(21f, 9f, 1f, 1f, Water);
            b.Ellipse(52f, 10f, 1f, 1f, Water);
            // 尾びれは2つに分かれた形にして、イルカと分かるようにする
            b.Triangle(V(47, 37), V(62, 50), V(61, 38), bodyShade);
            b.Triangle(V(47, 37), V(58, 58), V(51, 56), bodyShade);
            b.Foot(46f, 30f, 120f, 0.8f);
            b.ShadedTilted(40f, 32f, 11f, 6f, 40f, body);
            b.ShadedTilted(30f, 27f, 14f, 10f, 25f, body);
            b.BellyTilted(28f, 24f, 10f, 6f, 25f);
            b.Triangle(V(32, 34), V(42, 34), V(40, 44), bodyShade);
            b.Shaded(17f, 23f, 9.5f, 9f, body);
            b.White(14f, 22f, 7f, 5.5f);
            b.Flipper(V(26, 22), V(32, 13), 2.5f, bodyShade);
            b.TurnedFace(14f, 23f, FaceLeft, EyeStyle.Smile, eyeGap: 6f, open: true);
        }

        /// <summary>No.56 アマグモペン: 物静かな雨降らし。頭の上に小さな雨雲を連れて歩き、ヒレを広げて雨を受ける</summary>
        private static void Amagumo(PenguinBrush b)
        {
            var body = Rgb(90, 120, 160);
            var bodyShade = PenguinPalette.Shade(body);
            var cloud = Rgb(160, 168, 192);
            b.Feet(32f, 6f);
            b.Shaded(32f, 17f, 13f, 13f, body);
            b.Belly(31f, 16f, 9f, 10f);
            b.Flipper2(V(21, 24), V(11, 30), 3f, bodyShade);
            b.Shaded(32f, 34f, 11f, 10.5f, body);
            b.White(31f, 33f, 8f, 6.5f);
            b.Face(31f, 34f, 4f, EyeStyle.Line);
            b.Shaded(22f, 52f, 7f, 5f, cloud);
            b.Shaded(42f, 52f, 7f, 5f, cloud);
            b.Shaded(32f, 55f, 9f, 6.5f, cloud);
            b.Rect(17, 48, 31, 3, PenguinPalette.Shade(cloud));
            foreach (var x in new[] { 14, 20, 46, 52 })
            {
                b.Line(0.6f, Water, V(x, 45 - x % 3), V(x - 1, 41 - x % 3));
            }
            b.Line(0.6f, Water, V(10, 36), V(9, 33));
            b.Line(0.6f, Water, V(56, 37), V(55, 34));
        }

        /// <summary>No.66 カイガラペン: 用心深いが一途。大きなホタテの貝殻を背負ってかがみ、頭を低くして前へ押し出す</summary>
        private static void Kaigara(PenguinBrush b)
        {
            var body = Rgb(70, 110, 150);
            var shell = Rgb(240, 200, 170);
            var shellShade = PenguinPalette.Shade(shell);
            const int Ridges = 7;
            var rim = new Vector2[Ridges * 2 + 1];
            for (int i = 0; i < rim.Length; i++)
            {
                rim[i] = OnEllipse(44f, 10f, 19f, 22f, Mathf.Lerp(15f, 165f, i / (float)(rim.Length - 1)) * Mathf.Deg2Rad);
            }
            b.Polygon(V(44, 10), shellShade, rim);
            b.Polygon(V(44, 11), shell, ShrinkTo(rim, V(44, 10), 0.9f));
            for (int i = 1; i < Ridges * 2; i += 2)
            {
                b.Line(0.6f, shellShade, V(44, 10), rim[i]);
            }
            b.Foot(30f, 3f, 10f);
            b.Foot(18f, 4f, -10f);
            b.ShadedTilted(32f, 15f, 12f, 10f, 30f, body);
            b.BellyTilted(28f, 13f, 7f, 6.5f, 30f);
            b.Shaded(17f, 22f, 9.5f, 9f, body);
            b.White(15f, 21f, 7f, 6f);
            b.Flipper(V(26, 16), V(20, 6), 2.5f, PenguinPalette.Shade(body));
            b.TurnedFace(15f, 22f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f);
        }

        /// <summary>外周の点を中心へ寄せる（貝殻のふちを1回り細く残すため）</summary>
        private static Vector2[] ShrinkTo(Vector2[] points, Vector2 center, float rate)
        {
            var result = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                result[i] = Vector2.Lerp(center, points[i], rate);
            }
            return result;
        }
    }
}
