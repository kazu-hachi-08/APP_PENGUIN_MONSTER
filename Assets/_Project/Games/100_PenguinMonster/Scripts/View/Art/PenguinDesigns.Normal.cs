using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>ノーマルタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.11 ペンタ: お調子者。のけぞって胸を張り、片ヒレを大きく振ってウインク。片足でリズムを取る。未登録の No の代わりにも使う</summary>
        private static void Penta(PenguinBrush b)
        {
            var navy = Rgb(52, 64, 110);
            var navyShade = PenguinPalette.Shade(navy);
            b.Flipper(V(40, 28), V(55, 46), 3f, navyShade);
            b.Foot(38f, 3f);
            b.Foot(23f, 6f, 25f);
            b.ShadedTilted(32f, 17f, 13f, 14f, -12f, navy);
            b.BellyTilted(29f, 17f, 9f, 11f, -12f);
            b.Shaded(31f, 37f, 13f, 12.5f, navy);
            b.White(28f, 36f, 10f, 8.5f);
            b.Line(0.8f, navyShade, V(34, 49), V(33, 54), V(37, 56));
            b.Flipper(V(22, 22), V(17, 13), 2.5f, navyShade);
            b.TurnedFace(28f, 37f, FaceLeft, EyeStyle.Round, EyeStyle.Smile, open: true);
        }

        /// <summary>No.12 モフペン: 怠け者。まんまるの綿毛の体で寝そべり、ヒレでほおづえ。足は投げ出し、鼻ちょうちんで居眠り</summary>
        private static void Mofu(PenguinBrush b)
        {
            var fluff = Rgb(196, 198, 212);
            var fluffShade = PenguinPalette.Darken(fluff, 0.75f);
            var cap = Rgb(60, 62, 80);
            var bubble = Rgb(190, 230, 250);
            b.Foot(55f, 17f, 75f);
            b.Foot(60f, 13f, 55f);
            FluffBall(b, 38f, 10f, 19f, 8f, fluff, fluffShade);
            FluffBall(b, 17f, 21f, 11f, 10f, fluff, fluffShade);
            b.Ellipse(19f, 28f, 9f, 4.5f, cap);
            b.Ellipse(24f, 33f, 2.5f, 2.5f, cap);
            b.White(15f, 20f, 8f, 6f);
            b.Flipper(V(30, 10), V(26, 23), 3f, fluffShade);
            b.TurnedFace(15f, 21f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            if (!b.Front) return;
            b.Ellipse(5f, 14f, 3.5f, 3.5f, bubble);
            b.Dot(4, 16, PenguinPalette.EyeLight);
        }

        /// <summary>綿毛のふち（小さな丸）を回りに並べた楕円。右下の綿毛は影として残す</summary>
        private static void FluffBall(PenguinBrush b, float x, float y, float radiusX, float radiusY, Color32 fluff, Color32 shade)
        {
            const int Puffs = 14;
            const float PuffRadius = 3.5f;
            b.Ellipse(x, y, radiusX, radiusY, shade);
            for (int i = 0; i < Puffs; i++)
            {
                float angle = i * Mathf.PI * 2f / Puffs;
                b.Ellipse(x + Mathf.Cos(angle) * radiusX, y + Mathf.Sin(angle) * radiusY, PuffRadius, PuffRadius, shade);
            }
            b.Ellipse(x - 1f, y + 1f, radiusX - 1.5f, radiusY - 1.5f, fluff);
            for (int i = 0; i < Puffs; i++)
            {
                float angle = i * Mathf.PI * 2f / Puffs;
                if (Mathf.Sin(angle) < -0.2f || Mathf.Cos(angle) > 0.5f) continue;
                b.Ellipse(x - 1f + Mathf.Cos(angle) * (radiusX - 1f), y + 1f + Mathf.Sin(angle) * (radiusY - 1f), PuffRadius - 1f, PuffRadius - 1f, fluff);
            }
        }

        /// <summary>No.31 マジシャンペン: 気取ったお調子者。のけぞってシルクハットを高く掲げ、ステッキを振ってウインク。広がるマントと掲げた帽子で、上と横に大きく張り出したシルエット</summary>
        private static void Magician(PenguinBrush b)
        {
            var body = Rgb(80, 70, 120);
            var bodyShade = PenguinPalette.Shade(body);
            var cape = Rgb(40, 36, 60);
            var lining = Rgb(200, 50, 70);
            var hat = Rgb(40, 40, 56);
            // マントは肩の高さで横へひるがえし、裏地の赤を見せて舞台の決めポーズにする。
            // すそを地面まで垂らすコウテイペンと違い、足元を空けて身軽さを出す
            b.Polygon(V(46, 28), cape, V(36, 40), V(50, 38), V(60, 30), V(62, 20), V(57, 12), V(52, 16), V(46, 12), V(36, 18));
            b.Polygon(V(47, 28), lining, V(40, 36), V(50, 34), V(57, 28), V(59, 20), V(56, 15), V(51, 19), V(45, 16), V(40, 20));
            b.Line(0.7f, Rgb(120, 90, 60), V(44, 28), V(52, 46));
            b.Star(53f, 49f, 5f, 2.2f, PenguinPalette.Gold);
            b.Sparkle(59, 56, PenguinPalette.Gold);
            b.Sparkle(46, 56, PenguinPalette.Gold);
            b.Flipper(V(40, 26), V(47, 31), 2.5f, bodyShade);
            // 片足を軽く上げ、つま先立ちで踊るように
            b.Foot(27f, 3f, -10f);
            b.Foot(42f, 6f, 30f);
            b.ShadedTilted(32f, 18f, 11f, 14f, -10f, body);
            b.BellyTilted(29f, 17f, 7f, 11f, -10f);
            b.Shaded(31f, 37f, 10.5f, 10f, body);
            b.White(28f, 36f, 7.5f, 6.5f);
            // 蝶ネクタイ。燕尾服のペンギンに見せる
            if (b.Front)
            {
                b.Triangle(V(26, 30), V(26, 26), V(29, 28), lining);
                b.Triangle(V(32, 30), V(32, 26), V(29, 28), lining);
            }
            // 手前のヒレで帽子を高く掲げる。頭の上が空いているのが「脱いだ」合図
            b.Flipper(V(22, 24), V(12, 42), 3f, bodyShade);
            b.Polygon(V(12, 52), hat, V(5, 45), V(19, 43), V(20, 46), V(18, 58), V(8, 60), V(6, 48), V(2, 47));
            b.Line(1.5f, lining, V(7, 49), V(18, 47));
            b.TurnedFace(28f, 37f, FaceLeft, EyeStyle.Sharp, EyeStyle.Smile, open: true);
        }

        /// <summary>No.32 ボクサーペン: 攻撃的。深く前傾して足を大きく開き、前のグローブを突き出して吠える。はちまきが後ろへなびく</summary>
        private static void Boxer(PenguinBrush b)
        {
            var navy = Rgb(60, 70, 110);
            var glove = Rgb(220, 50, 50);
            var band = Rgb(240, 240, 240);
            b.Line(0.8f, band, V(36, 41), V(46, 44), V(55, 41));
            b.Line(0.8f, band, V(36, 41), V(45, 39), V(52, 35));
            b.Shaded(44f, 26f, 5.5f, 5.5f, glove);
            b.Foot(49f, 3f, -15f);
            b.Foot(17f, 3f, 10f);
            b.ShadedTilted(35f, 17f, 14f, 12f, 25f, navy);
            b.BellyTilted(31f, 15f, 9f, 8f, 25f);
            b.Shaded(24f, 33f, 12f, 11f, navy);
            b.White(21f, 32f, 9f, 7f);
            b.Rect(13, 38, 24, 3, band);
            b.Line(2f, PenguinPalette.Shade(navy), V(24, 23), V(13, 24));
            b.Shaded(9f, 25f, 6f, 5.5f, glove);
            b.Rect(13, 21, 2, 8, band);
            b.TurnedFace(21f, 32f, FaceLeft, EyeStyle.Sharp, open: true);
        }

        /// <summary>No.53 ビンタペン: せっかちで手が早い。両方のヒレを大きく振りかぶって高速で往復ビンタ。ヒレの残像が弧を描く</summary>
        private static void Binta(PenguinBrush b)
        {
            var navy = Rgb(70, 80, 140);
            var navyShade = PenguinPalette.Shade(navy);
            var blur = Rgb(200, 210, 240);
            b.Line(1f, blur, V(3, 28), V(4, 40), V(10, 50));
            b.Line(1f, blur, V(61, 28), V(60, 40), V(54, 50));

            b.Feet(32f, 8f);
            b.Shaded(32f, 18f, 13f, 13f, navy);
            b.Belly(31f, 17f, 9f, 10f);
            b.Flipper(V(21, 26), V(7, 44), 4.5f, navyShade);
            b.Flipper(V(43, 26), V(57, 44), 4.5f, navyShade);
            b.Shaded(32f, 37f, 11.5f, 11f, navy);
            b.White(31f, 36f, 8.5f, 7f);
            b.Face(31f, 37f, 4f, EyeStyle.Sharp);
        }
    }
}
