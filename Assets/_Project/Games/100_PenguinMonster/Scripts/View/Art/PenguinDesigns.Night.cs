using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>やみ・オーロラタイプ（極夜の空）が主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.22 シャチペン: 攻撃的。海から飛び出して獲物に食らいつく瞬間。体を斜めに反らせ、大きな背びれと尾びれを跳ね上げる</summary>
        private static void Shachi(PenguinBrush b)
        {
            var black = Rgb(64, 68, 92);
            var blackShade = PenguinPalette.Shade(black);
            var white = PenguinPalette.Belly;
            // 尾びれは体の延長で右上へ跳ね上げ、飛びかかる勢いを線で見せる
            b.Triangle(V(50, 34), V(62, 48), V(58, 33), blackShade);
            b.Triangle(V(50, 34), V(54, 50), V(58, 36), blackShade);
            b.Foot(50f, 11f, -30f);
            b.Foot(57f, 16f, -55f);
            b.ShadedTilted(37f, 24f, 19f, 12f, 22f, black);
            // 背びれは背中から高く突き出す。威嚇の特性をシルエットで一番大きく見せる
            b.Triangle(V(30, 33), V(44, 38), V(36, 61), blackShade);
            b.Triangle(V(32, 33), V(42, 37), V(36, 57), black);
            // シャチの白い模様は背中側からも見えるので White ではなく常に塗る
            b.Tilted(37f, 16f, 15f, 5f, 22f, white);
            b.Shaded(16f, 21f, 11f, 10f, black);
            b.White(13f, 19f, 8f, 6f);
            b.Ellipse(19f, 28f, 4f, 2f, white);
            // 前ヒレは頭の下から前へ突き出し、つかみかかる手のように見せる
            b.Flipper(V(24, 13), V(9, 7), 3.5f, blackShade);
            b.TurnedFace(13f, 21f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f, open: true);
        }

        /// <summary>No.23 ヨルペン: クールで神秘的。細身で横を向き、星空のマントをヒレで大きく翻す。頭上に三日月</summary>
        private static void Yoru(PenguinBrush b)
        {
            var cape = Rgb(40, 40, 90);
            var lining = Rgb(70, 60, 140);
            var body = Rgb(110, 70, 150);
            var bodyShade = PenguinPalette.Shade(body);
            // マントは肩から右下へ翼のように広げ、細い体との対比で横長のシルエットにする
            b.Polygon(V(40, 22), cape, V(30, 40), V(44, 40), V(54, 34), V(62, 22), V(60, 10), V(56, 2), V(44, 6), V(36, 2), V(24, 4));
            b.Polygon(V(46, 20), lining, V(40, 34), V(52, 30), V(58, 20), V(56, 9), V(50, 5), V(42, 8));
            b.Sparkle(50, 22, PenguinPalette.Gold);
            b.Sparkle(44, 12, PenguinPalette.Gold);
            b.Dot(54, 14, PenguinPalette.Gold);
            b.Dot(48, 30, PenguinPalette.Gold);
            b.Dot(38, 20, PenguinPalette.Gold);
            b.Foot(24f, 3f, 0f, 0.8f);
            b.Foot(32f, 3f, 0f, 0.8f);
            b.ShadedTilted(29f, 17f, 8f, 13f, -4f, body);
            b.BellyTilted(26f, 16f, 4.5f, 10f, -4f);
            b.Flipper(V(34, 28), V(52, 31), 2.5f, bodyShade);
            b.Shaded(27f, 36f, 10f, 9.5f, body);
            b.White(24f, 35f, 7f, 6f);
            b.TurnedFace(24f, 36f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            // 月は少し離して浮かべ、頭の上の飾りではなく夜空の一部に見せる
            b.Ellipse(14f, 54f, 6f, 6f, Rgb(255, 230, 120));
            b.Erase(17f, 56f, 5f, 5f);
        }

        /// <summary>No.24 オーロペン: 神秘的で穏やか。目を閉じて座ったまま宙に浮き、祈るヒレのまわりをオーロラの輪がめぐる</summary>
        private static void Auro(PenguinBrush b)
        {
            var body = Rgb(200, 190, 240);
            var bodyShade = PenguinPalette.Shade(body);
            var green = Rgb(120, 240, 180);
            var blue = Rgb(120, 200, 250);
            var pink = Rgb(240, 140, 220);
            // 腰の輪は奥半分を先に、手前半分を体の後に塗って、体に巻き付いているように見せる。ピンクの帯は背中から立ちのぼらせる
            NightArc(b, green, 32f, 25f, 25f, 7f, 15f, 165f);
            Ribbon(b, pink, V(40, 33), V(48, 38), V(51, 46), V(57, 50), V(60, 58));
            b.Foot(26f, 17f, 15f, 0.8f);
            b.Foot(38f, 16f, -10f, 0.8f);
            b.Shaded(32f, 26f, 12f, 10f, body);
            b.Belly(30f, 25f, 8f, 7f);
            b.Shaded(31f, 42f, 10.5f, 10f, body);
            b.White(28f, 41f, 7.5f, 6.5f);
            b.Flipper(V(20, 24), V(28, 29), 2.5f, bodyShade);
            b.Flipper(V(40, 24), V(30, 29), 2.5f, bodyShade);
            b.TurnedFace(28f, 42f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            // 頭上の輪は天使の輪のように浮かべ、祈りの神秘さを出す
            NightArc(b, blue, 31f, 56f, 9f, 3f, 0f, 360f);
            NightArc(b, green, 32f, 25f, 25f, 7f, 195f, 345f);
        }

        /// <summary>太い帯の中心に細い明るい線を通して、光っているように見せる</summary>
        private static void Ribbon(PenguinBrush b, Color32 color, params Vector2[] points)
        {
            b.Line(2.5f, color, points);
            b.Line(0.8f, PenguinPalette.Belly, points);
        }

        /// <summary>楕円の一部をオーロラの帯で描く。角度は度で、0 が右・反時計回り</summary>
        private static void NightArc(PenguinBrush b, Color32 color, float x, float y, float radiusX, float radiusY, float fromDeg, float toDeg)
        {
            const int Steps = 8;
            var points = new Vector2[Steps + 1];
            for (int i = 0; i <= Steps; i++)
            {
                float angle = Mathf.Lerp(fromDeg, toDeg, (float)i / Steps) * Mathf.Deg2Rad;
                points[i] = V(x + Mathf.Cos(angle) * radiusX, y + Mathf.Sin(angle) * radiusY);
            }
            Ribbon(b, color, points);
        }

        /// <summary>No.35 コウモリペン: いたずら好き。枝に足でぶら下がって逆さまになり、片方の翼を広げてにやりと笑う</summary>
        private static void Koumori(PenguinBrush b)
        {
            var wing = Rgb(60, 45, 90);
            var body = Rgb(120, 80, 160);
            var bodyShade = PenguinPalette.Shade(body);
            var branch = Rgb(110, 80, 60);
            b.Line(2f, branch, V(4, 59), V(60, 58));
            b.Foot(30f, 58f, 70f, 0.7f);
            b.Foot(38f, 58f, 110f, 0.7f);
            // 広げた翼は下へ垂れるので、ぎざぎざの縁を下向きにしてコウモリらしくする
            b.Polygon(V(22, 40), wing, V(28, 50), V(16, 52), V(6, 44), V(2, 32), V(8, 34), V(10, 26), V(16, 30), V(20, 22), V(26, 32));
            b.Line(0.5f, body, V(26, 46), V(6, 42));
            b.Line(0.5f, body, V(26, 44), V(10, 28));
            b.Line(0.5f, body, V(26, 42), V(20, 24));
            b.ShadedTilted(34f, 44f, 9f, 11f, -5f, body);
            b.BellyTilted(31f, 44f, 5f, 8f, -5f);
            b.Flipper(V(40, 42), V(44, 30), 2.5f, wing);
            // 逆さまなので耳は頭の下に付ける
            b.Triangle(V(24, 20), V(30, 16), V(22, 8), PenguinPalette.Shade(body));
            b.Triangle(V(34, 16), V(40, 20), V(42, 8), PenguinPalette.Shade(body));
            b.Shaded(32f, 25f, 10f, 9.5f, body);
            b.White(29f, 26f, 7f, 6f);
            KoumoriFace(b, 29f, 25f);
        }

        /// <summary>逆さまの顔。TurnedFace はくちばしを目の下に描くので、ここでは目の上にくちばしを置く</summary>
        private static void KoumoriFace(PenguinBrush b, float x, float eyeY)
        {
            if (!b.Front) return;
            b.Eye(x + 2f, eyeY, EyeStyle.Smile, true);
            b.Eye(x - 4f, eyeY, EyeStyle.Smile, false);
            b.Cheek(x - 5f, eyeY + 2f);
            b.SideBeak(x - 2f, eyeY + 4f, FaceLeft, true);
        }
    }
}
