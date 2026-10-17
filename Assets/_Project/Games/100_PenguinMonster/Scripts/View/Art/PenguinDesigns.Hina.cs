using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>ヒナランクのデザイン。体をふつうの6割ほどにして、頭に綿毛のアホ毛を立てる</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.37 ヒナペン: 力持ちでがんばり屋。足を大きく開いて踏んばり、体をのけぞらせて自分より大きなダンベルを斜めに差し上げる</summary>
        private static void HinaPen(PenguinBrush b)
        {
            var fluff = HinaGrayFluff;
            var fluffShade = PenguinPalette.Shade(fluff);
            var iron = Rgb(80, 82, 96);
            var ironLight = Rgb(140, 142, 160);
            b.Foot(20f, 3f, 20f, 0.7f);
            b.Foot(44f, 3f, -20f, 0.7f);
            b.Line(2f, fluffShade, V(24, 6), V(28, 12));
            b.Line(2f, fluffShade, V(40, 6), V(36, 12));
            // 片側を高く持ち上げて傾け、重さに振られている途中に見せる
            b.Flipper(V(28, 22), V(20, 34), 2.5f, fluffShade);
            b.Flipper(V(40, 22), V(46, 38), 2.5f, fluffShade);
            b.Line(1f, iron, V(12, 32), V(54, 41));
            b.Tilted(13f, 32f, 3f, 7f, 12f, iron);
            b.Tilted(53f, 41f, 3f, 7f, 12f, iron);
            b.Rect(12, 34, 1, 3, ironLight);
            b.Rect(52, 43, 1, 3, ironLight);
            b.ShadedTilted(33f, 13f, 8.5f, 7.5f, -8f, fluff);
            b.BellyTilted(31f, 12f, 5.5f, 5f, -8f);
            b.Shaded(34f, 23f, 8.5f, 7.5f, fluff);
            b.White(32f, 22f, 6f, 4.5f);
            b.Line(0.9f, fluff, V(36, 30), V(36, 34), V(39, 35));
            b.TurnedFace(32f, 23f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f, open: true);
        }

        /// <summary>No.38 ユキヒナペン: のんびり屋。雪の上にぺたんと座りこみ、足を投げ出して、舞い落ちる雪を口を開けて見上げる。下ぶくれの低い体</summary>
        private static void YukiHina(PenguinBrush b)
        {
            var fluff = Rgb(205, 228, 248);
            var fluffShade = PenguinPalette.Shade(fluff);
            b.Flipper(V(42, 12), V(51, 5), 2.5f, fluffShade);
            b.Shaded(35f, 10f, 11f, 8.5f, fluff);
            b.Belly(31f, 9f, 7f, 5.5f);
            // 投げ出した足は裏を見せて立て、座りこんでいるのがわかるようにする
            b.Foot(20f, 6f, 70f, 0.7f);
            b.Foot(25f, 4f, 45f, 0.7f);
            b.Shaded(31f, 22f, 8.5f, 7.5f, fluff);
            b.White(29f, 22f, 6f, 4.5f);
            b.Line(0.9f, fluff, V(34, 29), V(36, 33), V(33, 35));
            b.Flipper(V(26, 13), V(20, 15), 2f, fluffShade);
            b.TurnedFace(29f, 23f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
            b.Sparkle(18, 32, IceLight);
            b.Sparkle(11, 22, IceLight);
            b.Sparkle(49, 26, IceLight);
        }

        /// <summary>No.39 ハネヒナペン: 元気いっぱいのせっかち。前のめりに駆け出して宙に浮き、体のわりに大きな羽を後ろへ長く伸ばして風を切る（足の速さの印）</summary>
        private static void HaneHina(PenguinBrush b)
        {
            var feather = Rgb(205, 165, 115);
            var featherShade = PenguinPalette.Shade(feather);
            var tip = Rgb(250, 240, 220);
            Wing(b, V(40, 16), new[] { V(40, 20), V(51, 19), V(60, 15), V(61, 9), V(55, 5), V(47, 8) }, feather, featherShade, tip);
            b.Foot(46f, 9f, -40f, 0.7f);
            b.Foot(40f, 6f, -15f, 0.7f);
            // 奥の羽は低く後ろへ、手前の羽は高く後ろへ伸ばし、横から見て矢じりのような形にする
            // 体を前へ倒して、頭がおなかより前に出る駆け足の重心にする
            b.ShadedTilted(36f, 15f, 7f, 8.5f, 35f, feather);
            b.BellyTilted(34f, 14f, 4f, 5.5f, 35f);
            b.Shaded(27f, 23f, 8f, 7.5f, feather);
            b.White(25f, 23f, 5.5f, 4.5f);
            b.Line(0.9f, feather, V(27, 30), V(25, 34), V(28, 36));
            Wing(b, V(35, 21), new[] { V(33, 25), V(41, 36), V(49, 43), V(57, 42), V(61, 35), V(54, 26), V(42, 19) }, feather, featherShade, tip);
            b.TurnedFace(25f, 24f, FaceLeft, EyeStyle.Sparkle, eyeGap: 5f, open: true);
        }

        /// <summary>No.40 ナキムシペン: 臆病な泣き虫。小さく体を丸め、ヒレで自分を抱えて相手から目をそらす。涙と震え</summary>
        private static void Nakimushi(PenguinBrush b)
        {
            var black = Rgb(78, 76, 100);
            var blackShade = PenguinPalette.Shade(black);
            var tear = Rgb(120, 200, 250);
            var shiver = Rgb(150, 160, 190);
            b.Foot(28f, 2f, 0f, 0.6f);
            b.Foot(37f, 2f, 0f, 0.6f);
            b.Shaded(33f, 9f, 10f, 7.5f, black);
            b.Shaded(34f, 16f, 9.5f, 8.5f, black);
            b.White(36f, 15f, 6.5f, 5f);
            b.Flipper(V(24, 12), V(34, 6), 2.5f, blackShade);
            b.Flipper(V(43, 12), V(36, 6), 2.5f, blackShade);
            b.Line(0.9f, black, V(33, 24), V(31, 27), V(34, 29));
            b.TurnedFace(36f, 16f, FaceRight, EyeStyle.Worried, eyeGap: 6f);
            b.Line(0.5f, shiver, V(19, 16), V(17, 14), V(19, 12));
            b.Line(0.5f, shiver, V(48, 16), V(50, 14), V(48, 12));
            if (!b.Front) return;
            b.Ellipse(31f, 11f, 1f, 1.8f, tear);
            b.Ellipse(44f, 22f, 1.2f, 2f, tear);
        }

        /// <summary>No.41 カラヒナペン: 引っ込み思案で頑固。まだ割れていない卵に閉じこもり、ひびの窓からのぞく。殻のふたを斜めにかぶり、足だけ出して立つ</summary>
        private static void KaraHina(PenguinBrush b)
        {
            var chick = Rgb(165, 145, 125);
            var shell = Rgb(245, 235, 210);
            var shellShade = PenguinPalette.Darken(shell, 0.82f);
            var spot = Rgb(170, 130, 90);
            b.Foot(28f, 3f, 10f, 0.6f);
            b.Foot(38f, 3f, -10f, 0.6f);
            // 少し傾けた縦長の卵。ヒナで唯一、体ごと殻の形のシルエットにする
            b.Tilted(33f, 16f, 10f, 13f, 8f, shellShade);
            b.Tilted(32f, 17f, 8.5f, 11.5f, 8f, shell);
            b.Ellipse(38f, 9f, 2f, 1.5f, spot);
            b.Ellipse(29f, 7f, 1.5f, 1.2f, spot);
            b.Ellipse(40f, 20f, 1.2f, 1f, spot);
            // ふたは傾けてかぶり、すき間から綿毛のアホ毛だけ出す
            b.Line(0.9f, chick, V(39, 28), V(42, 33), V(45, 32));
            b.Polygon(V(31, 32), shellShade, V(22, 28), V(26, 31), V(30, 28), V(34, 31), V(38, 28), V(40, 32), V(36, 37), V(28, 38), V(22, 34));
            b.Polygon(V(30, 33), shell, V(23, 29), V(26, 32), V(30, 29), V(34, 32), V(37, 30), V(37, 33), V(34, 36), V(28, 37), V(23, 33));
            b.Ellipse(28f, 35f, 1.2f, 1f, spot);
            KaraHinaWindow(b, chick, shell);
        }

        /// <summary>殻のひびの窓。中の顔は正面からしか見えないので、後ろ姿では閉じた殻のまま</summary>
        private static void KaraHinaWindow(PenguinBrush b, Color32 chick, Color32 shell)
        {
            if (!b.Front) return;
            b.Ellipse(29f, 18f, 6.5f, 4.5f, PenguinPalette.Darken(chick, 0.6f));
            b.Ellipse(29f, 18f, 5.5f, 3.5f, chick);
            // 窓の縁に殻のかけらを残して、丸い穴ではなく割れたひびに見せる
            b.Triangle(V(25, 23), V(29, 23), V(27, 20), shell);
            b.Triangle(V(31, 23), V(35, 22), V(33, 20), shell);
            b.Triangle(V(26, 13), V(31, 13), V(29, 16), shell);
            b.SideBeak(24f, 16.5f, FaceLeft, false, 3f);
            b.Eye(27f, 19f, EyeStyle.Dot, false);
            b.Eye(32f, 19f, EyeStyle.Dot, true);
        }

        /// <summary>No.42 ピリヒナペン: お調子者で騒がしい。静電気で綿毛がとげとげに逆立ったまま跳び上がり、ヒレをばたつかせて叫ぶ。目はしびれて十字</summary>
        private static void PiriHina(PenguinBrush b)
        {
            var fluff = Rgb(250, 215, 80);
            var fluffShade = PenguinPalette.Shade(fluff);
            var spark = Rgb(150, 220, 255);
            b.Foot(28f, 4f, -30f, 0.6f);
            b.Foot(37f, 5f, 20f, 0.6f);
            b.Flipper(V(24, 20), V(14, 30), 2.5f, fluffShade);
            b.Flipper(V(41, 20), V(50, 32), 2.5f, fluffShade);
            // 頭と体を1つのとげとげの玉にして、他のヒナと形で見分けられるようにする
            PiriSpikes(b, 33f, 19f, 13f, 9f, fluffShade);
            b.Shaded(33f, 19f, 10f, 10f, fluff);
            b.Belly(32f, 14f, 6f, 4.5f);
            b.White(30f, 22f, 6f, 4f);
            b.Line(0.8f, fluff, V(34, 28), V(32, 32), V(36, 34), V(34, 38));
            b.TurnedFace(30f, 23f, FaceLeft, EyeStyle.Cross, eyeGap: 6f, open: true);
            b.Line(0.6f, spark, V(10, 18), V(7, 15), V(10, 13), V(7, 10));
            b.Line(0.6f, spark, V(54, 22), V(57, 19), V(54, 17), V(57, 14));
            b.Line(0.6f, spark, V(44, 36), V(46, 39), V(43, 41));
        }

        /// <summary>玉のまわりに三角のとげを放射状に並べる。下側は足が出るので空ける</summary>
        private static void PiriSpikes(PenguinBrush b, float x, float y, float outer, float inner, Color32 color)
        {
            const int Spikes = 9;
            const float HalfWidth = 0.35f;
            for (int i = 0; i < Spikes; i++)
            {
                float angle = (i * 360f / Spikes + 10f) * Mathf.Deg2Rad;
                if (Mathf.Sin(angle) < -0.7f) continue;
                var tip = V(x + Mathf.Cos(angle) * outer, y + Mathf.Sin(angle) * outer);
                var left = V(x + Mathf.Cos(angle - HalfWidth) * inner, y + Mathf.Sin(angle - HalfWidth) * inner);
                var right = V(x + Mathf.Cos(angle + HalfWidth) * inner, y + Mathf.Sin(angle + HalfWidth) * inner);
                b.Triangle(left, right, tip, color);
            }
        }

        /// <summary>No.68 コツブヒナペン: いたずら好き。足元の小石の山から ひとつかみずつ、ヒレを振りかぶって次々に投げる。小石が弧を描いて飛ぶ</summary>
        private static void KotsubuHina(PenguinBrush b)
        {
            var fluff = Rgb(165, 155, 145);
            var fluffShade = PenguinPalette.Shade(fluff);
            b.Shaded(50f, 5f, 4.5f, 3f, Stone);
            b.Shaded(57f, 5f, 3.5f, 3f, Stone);
            b.Shaded(53f, 9f, 3f, 2.5f, Stone);
            b.Foot(27f, 3f, -10f, 0.7f);
            b.Foot(38f, 3f, 10f, 0.7f);
            b.Shaded(33f, 12f, 8.5f, 8f, fluff);
            b.Belly(31f, 11f, 5.5f, 5f);
            b.Flipper(V(38, 18), V(45, 29), 2.5f, fluffShade);
            b.Shaded(45f, 30f, 2f, 2f, Stone);
            b.Shaded(31f, 23f, 8.5f, 7.5f, fluff);
            b.White(29f, 22f, 6f, 4.5f);
            b.Line(0.9f, fluff, V(33, 30), V(31, 34), V(34, 36));
            b.Flipper(V(26, 14), V(21, 8), 2f, fluffShade);
            b.TurnedFace(29f, 23f, FaceLeft, EyeStyle.Sparkle, eyeGap: 5f, open: true);
            b.Shaded(16f, 37f, 2.5f, 2f, Stone);
            b.Shaded(8f, 33f, 2f, 2f, Stone);
            b.Shaded(4f, 25f, 2f, 1.5f, Stone);
            b.Line(0.5f, Rgb(230, 230, 230), V(20, 40), V(23, 41));
            b.Line(0.5f, Rgb(230, 230, 230), V(11, 37), V(14, 38));
        }

        /// <summary>No.69 ネボスケヒナペン: いつも眠い。ふかふかの苔の巣にすっぽり収まり、ナイトキャップで鼻ちょうちんを ふくらませる</summary>
        private static void NebosukeHina(PenguinBrush b)
        {
            var fluff = Rgb(160, 195, 140);
            var moss = Rgb(100, 150, 70);
            var cap = Rgb(120, 140, 220);
            var bubble = Rgb(190, 230, 250);
            b.Shaded(32f, 15f, 9f, 8f, fluff);
            b.Shaded(30f, 25f, 8.5f, 7.5f, fluff);
            b.White(28f, 24f, 6f, 4.5f);
            b.Polygon(V(35, 34), cap, V(23, 30), V(39, 29), V(49, 38), V(46, 41));
            b.Shaded(48f, 40f, 2.5f, 2.5f, PenguinPalette.Belly);
            b.TurnedFace(28f, 25f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
            FluffBall(b, 32f, 8f, 18f, 5f, moss, PenguinPalette.Darken(moss, 0.75f));
            if (!b.Front) return;
            b.Ellipse(18f, 21f, 3f, 3f, bubble);
            b.Dot(17, 22, PenguinPalette.EyeLight);
            b.Line(0.6f, PenguinPalette.Belly, V(8, 38), V(11, 38), V(8, 35), V(11, 35));
        }

        /// <summary>No.70 ポカポカヒナペン: 陽だまりのような甘えん坊。頭の上の小さなお日さまに温められて、ヒレを広げてにっこり</summary>
        private static void PokapokaHina(PenguinBrush b)
        {
            var fluff = Rgb(250, 190, 120);
            var fluffShade = PenguinPalette.Shade(fluff);
            const int Rays = 8;
            for (int i = 0; i < Rays; i++)
            {
                float angle = i * Mathf.PI * 2f / Rays;
                b.Line(0.7f, FlameMiddle, OnEllipse(32f, 46f, 7f, 7f, angle), OnEllipse(32f, 46f, 10f, 10f, angle));
            }
            b.Ellipse(32f, 46f, 5.5f, 5.5f, FlameMiddle);
            b.Ellipse(31.5f, 46.5f, 4f, 4f, FlameInner);
            b.Foot(26f, 3f, -15f, 0.7f);
            b.Foot(37f, 3f, 15f, 0.7f);
            b.Shaded(31f, 12f, 9f, 8f, fluff);
            b.Belly(30f, 11f, 6f, 5f);
            b.Flipper(V(24, 15), V(15, 21), 2.5f, fluffShade);
            b.Flipper(V(38, 15), V(47, 21), 2.5f, fluffShade);
            b.Shaded(31f, 25f, 8.5f, 7.5f, fluff);
            b.White(30f, 24f, 6f, 4.5f);
            b.Face(30f, 25f, 3f, EyeStyle.Smile);
            b.Line(0.5f, FlameOuter, V(10, 30), V(12, 33), V(10, 36));
            b.Line(0.5f, FlameOuter, V(52, 30), V(54, 33), V(52, 36));
        }
    }
}
