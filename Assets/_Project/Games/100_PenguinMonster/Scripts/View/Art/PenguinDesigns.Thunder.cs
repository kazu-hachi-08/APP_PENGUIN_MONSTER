using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>かみなりタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Bolt = Rgb(255, 236, 70);
        private static readonly Color32 BoltShade = Rgb(200, 150, 30);

        /// <summary>No.4 イナペン: せっかちな走り屋。頭を突き出して前のめりに駆け、いなずま形の翼とくせ毛を後ろへ流す</summary>
        private static void Ina(PenguinBrush b)
        {
            var body = Rgb(240, 200, 50);
            var bodyShade = PenguinPalette.Shade(body);
            // 翼は2枚とも後ろ上へ流して、速さをシルエットの傾きで見せる
            var farWing = new[] { V(36, 30), V(41, 43), V(37, 45), V(44, 60) };
            b.Line(2.2f, BoltShade, farWing);
            b.Line(1.2f, body, farWing);
            b.Foot(48f, 5f, 40f);
            b.Foot(25f, 3f, -10f);
            b.ShadedTilted(34f, 18f, 10f, 15f, 40f, body);
            b.BellyTilted(30f, 16f, 6f, 11f, 40f);
            var tuft = new[] { V(22, 38), V(30, 42), V(28, 45), V(36, 50) };
            b.Line(1.3f, BoltShade, tuft);
            b.Line(0.7f, body, tuft);
            // 頭は体より前・低めに置き、前へ突っ込んでいる勢いを出す
            b.Shaded(19f, 29f, 10.5f, 10f, body);
            b.White(16f, 28f, 7.5f, 6f);
            var nearWing = new[] { V(42, 24), V(51, 29), V(49, 32), V(60, 38) };
            b.Line(2.2f, BoltShade, nearWing);
            b.Line(1.3f, Bolt, nearWing);
            b.TurnedFace(16f, 29f, FaceLeft, EyeStyle.Dot, eyeGap: 6f);
            b.Line(0.5f, BoltShade, V(52, 22), V(60, 22));
            b.Line(0.5f, BoltShade, V(55, 16), V(61, 16));
        }

        /// <summary>No.17 ライペン: 怒りんぼの雷さま。どっしり足を開き、背中の太鼓の輪を2本のばちで打ち鳴らして吠える</summary>
        private static void Rai(PenguinBrush b)
        {
            var body = Rgb(90, 90, 170);
            var bodyShade = PenguinPalette.Shade(body);
            var drum = Rgb(210, 60, 50);
            var stick = Rgb(150, 100, 60);
            const float RingX = 34f;
            const float RingY = 30f;
            const float RingRadius = 23f;
            const int Drums = 6;
            // 太鼓は背中の上半分だけに並べる。輪を閉じると丸い枠に見え、体の形が埋もれた
            b.Line(1.2f, PenguinPalette.GoldShade, ArcPoints(RingX, RingY, RingRadius, RingRadius, -0.2f, Mathf.PI + 0.2f));
            for (int i = 0; i < Drums; i++)
            {
                float angle = -0.2f + i * (Mathf.PI + 0.4f) / (Drums - 1);
                var p = OnEllipse(RingX, RingY, RingRadius, RingRadius, angle);
                b.Shaded(p.x, p.y, 4.5f, 4.5f, drum);
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 1.8f, 1.8f, PenguinPalette.Gold);
            }
            b.Line(0.8f, stick, V(46, 26), V(56, 32));
            b.Flipper(V(42, 24), V(48, 28), 3f, bodyShade);
            b.Foot(18f, 4f, 15f, 1.2f);
            b.Foot(48f, 4f, -15f, 1.2f);
            b.ShadedTilted(34f, 18f, 16f, 13f, 5f, body);
            b.BellyTilted(31f, 16f, 11f, 9f, 5f);
            b.Shaded(27f, 34f, 12f, 11.5f, body);
            b.White(24f, 33f, 9f, 7f);
            b.Line(0.8f, stick, V(16, 38), V(12, 48));
            b.Flipper(V(20, 28), V(15, 38), 3f, bodyShade);
            b.TurnedFace(24f, 34f, FaceLeft, EyeStyle.Sharp, open: true);
        }

        /// <summary>No.18 アラシペン: 不機嫌な雨雲。横に広がった雲の体から頭だけ出して浮かび、下へいなずまを落とす</summary>
        private static void Arashi(PenguinBrush b)
        {
            var cloud = Rgb(120, 125, 150);
            var cloudShade = PenguinPalette.Shade(cloud);
            YamiBolt(b, BoltShade, Bolt, V(30, 18), V(24, 12), V(29, 10), V(20, 1));
            b.Line(0.6f, Water, V(42, 12), V(41, 9));
            b.Line(0.6f, Water, V(50, 14), V(49, 11));
            b.Line(0.6f, Water, V(56, 9), V(55, 6));
            b.Line(0.6f, Water, V(38, 6), V(37, 3));
            // 雲は右へ長くたなびかせ、頭を左端に寄せて、ゆっくり流れてくる向きを出す
            var puffs = new[]
            {
                new Vector4(40, 22, 10, 7), new Vector4(54, 22, 8, 6), new Vector4(48, 31, 9, 7),
                new Vector4(36, 30, 9, 7), new Vector4(58, 30, 5, 5), new Vector4(26, 21, 8, 6),
            };
            foreach (var p in puffs)
            {
                b.Ellipse(p.x, p.y, p.z, p.w, cloudShade);
            }
            foreach (var p in puffs)
            {
                b.Ellipse(p.x - 1f, p.y + 1f, p.z - 1.5f, p.w - 1.5f, cloud);
            }
            b.Shaded(22f, 33f, 11f, 10.5f, cloud);
            b.White(19f, 32f, 8f, 6.5f);
            b.Flipper(V(30, 28), V(30, 19), 2.5f, cloudShade);
            b.TurnedFace(19f, 33f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            b.Line(1f, cloudShade, V(22, 43), V(25, 47), V(29, 46));
        }

        /// <summary>No.36 ホタルペン: 恥ずかしがり屋。光るおしりを高く上げてうずくまり、低い頭で こちらをうかがう。触角は前へ垂れる</summary>
        private static void Hotaru(PenguinBrush b)
        {
            var body = Rgb(70, 100, 80);
            var bodyShade = PenguinPalette.Shade(body);
            // おしりの光を絵の主役にするため、体を前へ倒して光る所をいちばん高く大きく置く
            b.Ellipse(46f, 30f, 14f, 13f, Rgb(230, 245, 170));
            b.Foot(30f, 3f);
            b.Foot(42f, 4f, -10f);
            b.ShadedTilted(36f, 16f, 10f, 14f, 55f, body);
            b.BellyTilted(32f, 12f, 6f, 9f, 55f);
            b.Shaded(47f, 29f, 9f, 8.5f, Bolt);
            b.Ellipse(45f, 31f, 4f, 3.5f, Rgb(255, 250, 170));
            b.Line(0.6f, bodyShade, V(16, 26), V(10, 30), V(6, 27));
            b.Line(0.6f, bodyShade, V(20, 27), V(18, 33), V(13, 34));
            b.Ellipse(6f, 26f, 1.8f, 1.8f, Bolt);
            b.Ellipse(13f, 34f, 1.8f, 1.8f, Bolt);
            b.Shaded(19f, 18f, 9f, 8.5f, body);
            b.White(16f, 17f, 6.5f, 5f);
            b.Flipper(V(26, 16), V(21, 9), 2.5f, bodyShade);
            b.TurnedFace(16f, 18f, FaceLeft, EyeStyle.Worried, eyeGap: 6f);
            b.Sparkle(58, 46, Bolt);
            b.Sparkle(34, 44, Bolt);
        }

        /// <summary>No.63 イナズマペン: 短気で負けず嫌い。稲妻の軌跡を引いて前のめりに駆け出し、頭のとさかも稲妻の形</summary>
        private static void Inazuma(PenguinBrush b)
        {
            var body = Rgb(60, 60, 82);
            var bodyShade = PenguinPalette.Shade(body);
            b.Line(2.5f, BoltShade, V(62, 44), V(50, 34), V(57, 28), V(42, 18));
            b.Line(1.2f, Bolt, V(62, 44), V(50, 34), V(57, 28), V(42, 18));
            b.Foot(33f, 3f, -25f);
            b.Foot(15f, 5f, 25f);
            b.ShadedTilted(29f, 19f, 11f, 12f, 30f, body);
            b.BellyTilted(25f, 17f, 6.5f, 8f, 30f);
            if (b.Front)
            {
                b.Line(0.8f, Bolt, V(25, 25), V(28, 20), V(24, 17), V(27, 11));
            }
            b.Flipper(V(34, 24), V(46, 28), 2.5f, bodyShade);
            b.Triangle(V(19, 41), V(26, 41), V(28, 54), Bolt);
            b.Triangle(V(24, 44), V(29, 45), V(36, 56), BoltShade);
            b.Shaded(19f, 33f, 10f, 9.5f, body);
            b.White(17f, 32f, 7f, 6f);
            b.TurnedFace(17f, 33f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f, open: true);
        }

        /// <summary>No.67 ライウンペン: 気分屋の雨男。黒い雷雲に乗って空を漂い、片ヒレを上げて雷と雨を呼ぶ。雲の下から稲妻</summary>
        private static void Raiun(PenguinBrush b)
        {
            var body = Rgb(60, 110, 170);
            var bodyShade = PenguinPalette.Shade(body);
            var cloud = Rgb(95, 100, 130);
            YamiBolt(b, BoltShade, Bolt, V(18, 16), V(14, 10), V(19, 7), V(16, 2));
            YamiBolt(b, BoltShade, Bolt, V(46, 16), V(50, 11), V(45, 7), V(48, 3));
            b.Shaded(32f, 31f, 11f, 9f, body);
            b.Belly(31f, 30f, 7.5f, 6.5f);
            b.Shaded(18f, 21f, 9f, 6f, cloud);
            b.Shaded(46f, 21f, 9f, 6f, cloud);
            b.Shaded(32f, 22f, 12f, 7f, cloud);
            b.Flipper(V(22, 32), V(13, 29), 3f, bodyShade);
            b.Flipper(V(42, 34), V(52, 48), 3f, bodyShade);
            b.Shaded(32f, 45f, 10f, 9.5f, body);
            b.White(31f, 44f, 7.5f, 6f);
            b.Face(31f, 45f, 3.5f, EyeStyle.Round);
            b.Sparkle(54, 52, Bolt);
        }
    }
}
