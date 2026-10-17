using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>区切り1の雪の林と道場の子（No.172〜180。PLAN_STORY_TOWN01.md フェーズ8a）。雷鳥・石灯籠・座敷童・鬼瓦・ダイヤモンドダスト</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Okappa = Rgb(30, 28, 36);
        private static readonly Color32 Kimono = Rgb(205, 50, 60);
        private static readonly Color32 Tile = Rgb(95, 105, 125);
        private static readonly Color32 AuroraGreen = Rgb(140, 240, 200);
        private static readonly Color32 AuroraPink = Rgb(240, 160, 230);

        /// <summary>No.172 パチリペン: いたずら好き。静電気で綿毛が逆立ち、両ヒレの間でパチパチ火花を飛ばして笑う</summary>
        private static void Pachiri(PenguinBrush b)
        {
            var body = Rgb(240, 215, 90);
            var bodyShade = PenguinPalette.Shade(body);
            b.Foot(28f, 3f, 0f, 0.6f);
            b.Foot(37f, 3f, 0f, 0.6f);
            b.Shaded(33f, 12f, 8.5f, 8f, body);
            b.Belly(31f, 11f, 5.5f, 5.5f);
            b.Shaded(32f, 23f, 8f, 7.5f, body);
            // 逆立った綿毛はギザギザの三角で、静電気でふくらんだ頭に見せる
            b.Triangle(V(26, 27), V(29, 28), V(25, 35), body);
            b.Triangle(V(30, 29), V(33, 29), V(31, 37), body);
            b.Triangle(V(34, 28), V(37, 27), V(39, 34), body);
            b.White(30f, 22.5f, 6f, 4.5f);
            b.Flipper(V(26, 14), V(17, 18), 2f, bodyShade);
            b.Flipper(V(40, 14), V(48, 18), 2f, bodyShade);
            b.Line(0.6f, Bolt, V(17, 20), V(20, 24), V(16, 26), V(19, 30));
            b.Line(0.6f, Bolt, V(48, 20), V(45, 24), V(49, 26), V(46, 30));
            b.TurnedFace(30f, 24f, FaceLeft, EyeStyle.Round, EyeStyle.Smile, eyeGap: 5f, open: true);
        }

        /// <summary>No.173 ライチョウペン: 稲妻のように速い山の番人。冬毛の白い体に赤い眉、雷をまとって雪原を低く駆け抜ける</summary>
        private static void Raichou(PenguinBrush b)
        {
            var body = Rgb(228, 232, 240);
            var bodyShade = PenguinPalette.Shade(body);
            var tail = Rgb(45, 45, 55);
            var brow = Rgb(220, 50, 50);
            b.Triangle(V(46, 22), V(60, 26), V(58, 18), tail);
            b.Line(1f, Bolt, V(6, 44), V(12, 38), V(8, 36), V(16, 28));
            b.Line(1f, Bolt, V(54, 48), V(48, 42), V(53, 40), V(46, 33));
            b.Foot(42f, 5f, -40f);
            b.Foot(24f, 4f, 30f);
            b.ShadedTilted(36f, 19f, 12f, 10f, -25f, body);
            b.BellyTilted(33f, 18f, 8f, 6f, -25f);
            b.Flipper(V(40, 24), V(54, 32), 2.5f, bodyShade);
            b.Shaded(22f, 30f, 10f, 9.5f, body);
            b.White(19f, 29f, 7.5f, 6.5f);
            b.Line(1f, brow, V(14, 35), V(19, 36));
            b.Line(1f, brow, V(22, 36), V(26, 35));
            b.Flipper(V(26, 20), V(14, 14), 2.5f, bodyShade);
            b.TurnedFace(19f, 31f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            b.Line(0.5f, Bolt, V(2, 22), V(10, 22));
            b.Line(0.5f, Bolt, V(2, 16), V(8, 16));
        }

        /// <summary>No.174 イシコロペン: 動かないことに命をかける頑固者。体そのものが ひびの入った丸い石で、半分 地面に埋まってむすっとしている</summary>
        private static void Ishikoro(PenguinBrush b)
        {
            var crack = PenguinPalette.Darken(Stone, 0.6f);
            b.Ellipse(32f, 3f, 16f, 3.5f, Rgb(120, 95, 70));
            b.Shaded(32f, 14f, 12f, 11f, Stone);
            b.Line(0.6f, crack, V(40, 22), V(37, 17), V(41, 13));
            b.Line(0.6f, crack, V(23, 9), V(27, 6));
            b.Flipper(V(21, 12), V(16, 8), 2f, PenguinPalette.Shade(Stone));
            b.White(30f, 16f, 6.5f, 4.5f);
            b.TurnedFace(30f, 17f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
            b.Ellipse(32f, 3f, 14f, 1.5f, Rgb(120, 95, 70));
        }

        /// <summary>No.175 イシドウロウペン: 夜道を見守るまじめな番人。笠の屋根を頭にのせ、胸の火袋に あたたかい灯をともして仁王立ちする</summary>
        private static void Ishidourou(PenguinBrush b)
        {
            var stoneShade = PenguinPalette.Shade(Stone);
            var glow = Rgb(255, 210, 110);
            b.Rect(18, 0, 28, 4, stoneShade);
            b.Foot(38f, 5f);
            b.Foot(26f, 5f);
            b.Shaded(32f, 20f, 14f, 14f, Stone);
            // 火袋は四角い窓。後ろ姿では見えないので正面だけ
            if (b.Front)
            {
                b.Rect(24, 13, 14, 11, stoneShade);
                b.Rect(26, 15, 10, 7, glow);
                b.Rect(30, 15, 2, 7, stoneShade);
            }
            b.Flipper(V(18, 24), V(8, 16), 3f, stoneShade);
            b.Flipper(V(46, 24), V(56, 16), 3f, stoneShade);
            b.Shaded(32f, 38f, 11f, 9.5f, Stone);
            b.White(29f, 37f, 8f, 6f);
            b.Polygon(V(32, 48), stoneShade, V(10, 44), V(54, 44), V(42, 52), V(22, 52));
            b.Polygon(V(32, 49), Stone, V(13, 45), V(51, 45), V(41, 51), V(23, 51));
            b.Shaded(32f, 55f, 3f, 3.5f, Stone);
            b.TurnedFace(29f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
        }

        /// <summary>No.176 ワラシペン: 人なつこい いたずらっ子。おかっぱ頭に赤い着物で ふわっと浮かび、くすくす笑いながら こちらをのぞく</summary>
        private static void Warashi(PenguinBrush b)
        {
            var body = Rgb(95, 75, 115);
            var bodyShade = PenguinPalette.Shade(body);
            var wisp = Rgb(160, 140, 200);
            b.Ellipse(32f, 3f, 9f, 1.5f, wisp);
            b.Shaded(32f, 14f, 8.5f, 8f, Kimono);
            b.Line(1f, PenguinPalette.Gold, V(25, 13), V(39, 13));
            if (b.Front)
            {
                b.Triangle(V(28, 21), V(36, 21), V(32, 14), PenguinPalette.Belly);
            }
            b.Flipper(V(26, 16), V(20, 12), 2f, PenguinPalette.Shade(Kimono));
            b.Flipper(V(38, 16), V(44, 20), 2f, PenguinPalette.Shade(Kimono));
            b.Shaded(32f, 25f, 8f, 7.5f, body);
            // おかっぱは頭の上半分と、まっすぐ切りそろえた前髪
            b.Ellipse(32f, 30f, 8.5f, 4.5f, Okappa);
            b.Rect(23, 21, 3, 8, Okappa);
            b.Rect(38, 21, 3, 8, Okappa);
            b.White(31f, 23.5f, 6f, 4f);
            b.TurnedFace(31f, 24f, FaceLeft, EyeStyle.Smile, eyeGap: 5f);
            b.Ellipse(14f, 22f, 2f, 2.5f, wisp);
            b.Ellipse(50f, 30f, 1.5f, 2f, wisp);
        }

        /// <summary>No.177 ザシキワラシペン: 家に福を呼ぶ気まぐれな守り神。座布団に座って手毬をつき、まわりに青白い鬼火がゆらめく</summary>
        private static void Zashikiwarashi(PenguinBrush b)
        {
            var body = Rgb(85, 65, 110);
            var cushion = Rgb(110, 60, 140);
            var wisp = Rgb(150, 200, 255);
            var ball = Rgb(250, 240, 230);
            b.Polygon(V(32, 5), PenguinPalette.Shade(cushion), V(8, 5), V(20, 10), V(46, 10), V(58, 5), V(46, 0), V(20, 0));
            b.Polygon(V(32, 6), cushion, V(10, 5), V(21, 9), V(45, 9), V(56, 5), V(45, 1), V(21, 1));
            b.Shaded(32f, 18f, 13f, 11f, Kimono);
            b.Line(1.5f, PenguinPalette.Gold, V(20, 17), V(44, 17));
            if (b.Front)
            {
                b.Triangle(V(26, 28), V(38, 28), V(32, 17), PenguinPalette.Belly);
            }
            b.Shaded(32f, 35f, 10.5f, 10f, body);
            // 前髪は目の上で切りそろえ、顔を隠さない
            b.Ellipse(32f, 42f, 11.5f, 5.5f, Okappa);
            b.Rect(20, 28, 4, 12, Okappa);
            b.Rect(40, 28, 4, 12, Okappa);
            b.White(31f, 33.5f, 8f, 5.5f);
            b.TurnedFace(31f, 34f, FaceLeft, EyeStyle.Line, eyeGap: 6f, open: true);
            b.Shaded(51f, 30f, 5f, 5f, ball);
            b.Line(0.6f, Kimono, V(46, 30), V(56, 30));
            b.Line(0.6f, AuroraGreen, V(51, 25), V(51, 35));
            b.Flipper(V(42, 22), V(48, 28), 2.5f, PenguinPalette.Shade(Kimono));
            b.Flipper(V(21, 22), V(13, 18), 2.5f, PenguinPalette.Shade(Kimono));
            foreach (var spot in new[] { V(8, 40), V(14, 52), V(56, 48), V(6, 24) })
            {
                b.Tilted(spot.x, spot.y, 2f, 3f, -15f, wisp);
                b.Ellipse(spot.x, spot.y - 0.5f, 1f, 1.5f, SnowWhite);
            }
        }

        /// <summary>No.178 カワラペン: 打たれ強い努力家。丸い瓦を兜のようにかぶり、家紋の入った瓦の顔を前に向けて踏んばる</summary>
        private static void Kawara(PenguinBrush b)
        {
            var body = Rgb(120, 125, 140);
            var bodyShade = PenguinPalette.Shade(body);
            var tileShade = PenguinPalette.Shade(Tile);
            b.Foot(26f, 3f, 15f, 0.65f);
            b.Foot(39f, 3f, -15f, 0.65f);
            b.Shaded(32f, 12f, 9f, 8f, body);
            b.Belly(30f, 11f, 6f, 5.5f);
            b.Flipper(V(25, 14), V(18, 10), 2.2f, bodyShade);
            b.Flipper(V(39, 14), V(46, 10), 2.2f, bodyShade);
            b.Shaded(32f, 23f, 8.5f, 7.5f, body);
            // 瓦は頭の上の半円。前の端の丸い面に家紋を入れる
            b.Ellipse(32f, 28f, 11f, 6.5f, tileShade);
            b.Ellipse(31f, 29f, 10f, 5.5f, Tile);
            b.Line(0.6f, tileShade, V(26, 33), V(26, 26));
            b.Line(0.6f, tileShade, V(36, 34), V(36, 26));
            b.White(30f, 21.5f, 6f, 4f);
            b.Shaded(22f, 26f, 3f, 3f, Tile);
            b.Ellipse(22f, 26f, 1.2f, 1.2f, IronLight);
            b.TurnedFace(30f, 22f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f);
        }

        /// <summary>No.179 オニガワラペン: 屋根から悪いものをにらみ返す守り手。鬼瓦の角と牙の面をかぶり、腕組みして見下ろす</summary>
        private static void Onigawara(PenguinBrush b)
        {
            var body = Rgb(105, 112, 130);
            var bodyShade = PenguinPalette.Shade(body);
            var tileShade = PenguinPalette.Shade(Tile);
            b.Foot(40f, 3f);
            b.Foot(24f, 3f);
            b.Shaded(32f, 20f, 16f, 16f, body);
            b.Belly(30f, 19f, 10.5f, 11f);
            // 腕組み: 両ヒレを胸の前で重ねる
            b.Flipper(V(17, 24), V(40, 20), 3f, bodyShade);
            b.Flipper(V(47, 24), V(24, 22), 3f, bodyShade);
            b.Triangle(V(18, 46), V(23, 50), V(12, 61), tileShade);
            b.Triangle(V(46, 46), V(41, 50), V(52, 61), tileShade);
            b.Shaded(32f, 42f, 13f, 11f, Tile);
            b.White(29f, 40f, 9f, 6.5f);
            b.TurnedFace(29f, 41f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            if (b.Front)
            {
                b.Line(1.2f, tileShade, V(20, 46), V(27, 44));
                b.Line(1.2f, tileShade, V(31, 44), V(38, 46));
                b.Triangle(V(23, 36), V(25, 36), V(24, 33), PenguinPalette.Belly);
                b.Triangle(V(29, 36), V(31, 36), V(30, 33), PenguinPalette.Belly);
            }
            b.Shaded2(18f, 38f, 3f, 4f, Tile);
        }

        /// <summary>No.180 ダイヤダストペン: めったに姿を見せない不思議な子。透きとおった氷の結晶の体で宙に浮かび、背中にオーロラの帯をたなびかせる</summary>
        private static void DiamondDust(PenguinBrush b)
        {
            var body = Rgb(190, 235, 250);
            var bodyShade = Rgb(140, 200, 235);
            b.Line(2f, AuroraGreen, V(6, 20), V(18, 30), V(30, 26), V(44, 36), V(58, 32));
            b.Line(1.5f, AuroraPink, V(6, 25), V(18, 35), V(30, 31), V(44, 41), V(58, 37));
            b.Foot(29f, 7f, 15f, 0.6f);
            b.Foot(36f, 7f, -15f, 0.6f);
            b.Polygon(V(32, 18), bodyShade, V(32, 6), V(43, 18), V(38, 28), V(26, 28), V(21, 18));
            b.Polygon(V(31, 19), body, V(31, 8), V(41, 18), V(37, 27), V(26, 27), V(22, 18));
            b.Line(0.5f, SnowWhite, V(31, 8), V(31, 27));
            b.Line(0.5f, SnowWhite, V(22, 18), V(41, 18));
            b.Flipper(V(23, 20), V(14, 14), 2f, bodyShade);
            b.Flipper(V(40, 20), V(49, 14), 2f, bodyShade);
            b.Shaded(31f, 34f, 8.5f, 8f, body);
            b.White(29f, 33.5f, 6f, 4.5f);
            b.Polygon(V(31, 46), bodyShade, V(31, 51), V(35, 46), V(31, 41), V(27, 46));
            b.Polygon(V(31, 46), SnowWhite, V(31, 49), V(33, 46), V(31, 43), V(29, 46));
            b.TurnedFace(29f, 35f, FaceLeft, EyeStyle.Cross, eyeGap: 5f);
            foreach (var spot in new[] { V(8, 48), V(16, 56), V(50, 52), V(56, 12), V(8, 8), V(46, 4), V(56, 24) })
            {
                b.Sparkle((int)spot.x, (int)spot.y, SnowWhite);
            }
        }
    }
}
