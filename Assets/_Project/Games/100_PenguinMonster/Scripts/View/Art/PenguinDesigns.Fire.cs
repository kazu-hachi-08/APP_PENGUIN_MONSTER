using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>ひタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 FlameOuter = Rgb(255, 110, 40);
        private static readonly Color32 FlameMiddle = Rgb(255, 150, 50);
        private static readonly Color32 FlameInner = Rgb(255, 220, 80);

        /// <summary>No.3 ヒノペン: 熱血で活発。細い体を深く前に倒して駆け出し、炎の髪としっぽの火が後ろへなびく</summary>
        private static void Hino(PenguinBrush b)
        {
            var red = Rgb(215, 75, 55);
            var redShade = PenguinPalette.Shade(red);
            // 髪の炎は真上ではなく後ろへ流し、走る勢いを形で見せる
            b.Polygon(V(30, 38), FlameOuter, V(18, 38), V(24, 50), V(36, 60), V(36, 52), V(52, 60), V(46, 48), V(62, 50), V(50, 40), V(34, 30));
            b.Polygon(V(32, 42), FlameInner, V(26, 42), V(32, 50), V(36, 48), V(46, 54), V(42, 46), V(52, 46), V(36, 38));
            b.Polygon(V(52, 16), FlameOuter, V(44, 12), V(62, 24), V(56, 16), V(62, 10), V(46, 18));
            b.Triangle(V(47, 15), V(48, 17), V(57, 17), FlameInner);
            b.Flipper(V(42, 24), V(58, 32), 3f, redShade);
            b.Foot(52f, 10f, -40f);
            b.ShadedTilted(38f, 18f, 9f, 13f, 50f, red);
            b.Flipper(V(36, 16), V(48, 8), 3f, redShade);
            b.BellyTilted(35f, 16f, 5f, 9f, 50f);
            b.Foot(30f, 5f, 20f);
            b.Shaded(24f, 31f, 11f, 10.5f, red);
            b.White(21f, 30f, 8f, 7f);
            b.TurnedFace(21f, 31f, FaceLeft, EyeStyle.Round, eyeGap: 6f, open: true);
        }

        /// <summary>No.15 カザンペン: 怒りっぽくて頑固。体そのものが傾いた火山で、両ヒレを振り上げて吠え、火口が噴火する</summary>
        private static void Kazan(PenguinBrush b)
        {
            var rock = Rgb(130, 90, 70);
            var rockShade = PenguinPalette.Shade(rock);
            var lava = Rgb(70, 45, 35);
            // 噴煙は右上へ大きく吹き上げ、山の頂も右へずらして左右非対称の山にする
            b.Polygon(V(44, 48), FlameOuter, V(38, 42), V(34, 54), V(40, 52), V(44, 62), V(48, 54), V(56, 58), V(52, 46), V(48, 42));
            b.Polygon(V(44, 47), FlameInner, V(41, 43), V(40, 50), V(44, 55), V(48, 50), V(47, 43));
            b.Ellipse(28f, 52f, 2f, 2f, FlameMiddle);
            b.Ellipse(58f, 40f, 1.5f, 1.5f, FlameMiddle);
            b.Foot(46f, 3f);
            b.Foot(18f, 3f);
            b.Polygon(V(32, 18), rockShade, V(3, 2), V(61, 2), V(56, 18), V(50, 42), V(36, 44), V(16, 30));
            b.Polygon(V(31, 19), rock, V(6, 4), V(57, 4), V(53, 18), V(48, 40), V(37, 41), V(18, 29));
            b.Ellipse(43f, 42f, 7f, 2.5f, lava);
            b.Ellipse(43f, 42.5f, 5.5f, 1.5f, FlameOuter);
            // 溶岩が右の斜面を伝って流れ落ち、噴火の勢いを足す
            b.Line(1.5f, FlameOuter, V(48, 41), V(51, 34), V(50, 28));
            b.Ellipse(50f, 27f, 1.5f, 2f, FlameOuter);
            b.Line(1.2f, FlameOuter, V(38, 41), V(37, 37));
            b.Line(0.6f, rockShade, V(40, 12), V(44, 16), V(43, 20));
            b.WhiteTilted(22f, 20f, 10f, 8f, 20f);
            // ヒレは太い楕円にする。細い三角だと枯れ枝に見えたため
            b.ShadedTilted(13f, 31f, 4f, 9f, 40f, rockShade);
            b.TurnedFace(21f, 21f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f, open: true);
        }

        /// <summary>No.16 タキビペン: おだやかで面倒見がいい。薪の上でゆらゆら燃える炎の体。先がくるりと右へ流れ、小さなヒレを広げてあたたまる</summary>
        private static void Takibi(PenguinBrush b)
        {
            var log = Rgb(130, 80, 50);
            var logEnd = Rgb(200, 150, 100);
            var flame = Rgb(240, 90, 40);
            // 薪は交差させて置き、体は薪から少し浮かせて「燃えている」感じを出す
            b.Line(2.5f, log, V(14, 3), V(46, 8));
            b.Line(2.5f, log, V(18, 9), V(50, 3));
            b.Ellipse(13f, 3f, 2f, 2.2f, logEnd);
            b.Ellipse(51f, 3f, 2f, 2.2f, logEnd);
            b.Polygon(V(34, 32), flame, V(18, 26), V(16, 40), V(24, 36), V(26, 50), V(32, 44), V(40, 60), V(46, 56), V(42, 48), V(50, 46), V(46, 36), V(54, 30), V(44, 22));
            b.Tilted(31f, 21f, 14f, 13f, 10f, flame);
            b.Polygon(V(34, 30), FlameMiddle, V(22, 26), V(24, 34), V(30, 40), V(38, 52), V(40, 44), V(44, 40), V(42, 24));
            b.Tilted(30f, 20f, 11f, 10f, 10f, FlameMiddle);
            b.Tilted(29f, 18f, 8f, 7f, 10f, FlameInner);
            b.Flipper(V(20, 16), V(12, 20), 2.5f, flame);
            b.Flipper(V(42, 16), V(50, 18), 2.5f, flame);
            b.TurnedFace(26f, 23f, FaceLeft, EyeStyle.Smile, eyeGap: 7f);
        }

        /// <summary>No.27 オンセンペン: のんびり屋。桶の湯にあおむけで浸かり、ふちにヒレと頭をあずけて足を投げ出す。頭に手ぬぐい、湯気がのぼる</summary>
        private static void Onsen(PenguinBrush b)
        {
            var body = Rgb(200, 110, 90);
            var bodyShade = PenguinPalette.Shade(body);
            var wood = Rgb(190, 140, 90);
            var hoop = Rgb(110, 80, 60);
            var steam = Rgb(235, 240, 248);
            // 体は湯に沈めて頭と足先だけ出す。ふちから外へはみ出す頭とヒレで、横長の桶に凹凸を付ける
            b.Line(2f, bodyShade, V(46, 17), V(50, 21));
            b.Foot(51f, 22f, 35f);
            b.Foot(57f, 21f, 55f);
            b.Rect(8, 2, 48, 17, PenguinPalette.Shade(wood));
            b.Rect(9, 3, 45, 15, wood);
            b.Rect(8, 6, 48, 2, hoop);
            b.Rect(8, 13, 48, 2, hoop);
            b.Ellipse(32f, 19f, 23f, 2.5f, Water);
            b.Flipper(V(14, 20), V(6, 10), 3f, bodyShade);
            b.Shaded(17f, 27f, 10.5f, 9.5f, body);
            b.WhiteTilted(14f, 27f, 7.5f, 6f, -15f);
            b.Rect(14, 34, 12, 3, PenguinPalette.Belly);
            b.Rect(16, 37, 8, 2, PenguinPalette.Belly);
            b.Rect(15, 35, 10, 1, Rgb(120, 170, 230));
            b.TurnedFace(14f, 28f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            b.Line(0.6f, steam, V(32, 24), V(30, 28), V(32, 32), V(30, 36));
            b.Line(0.6f, steam, V(40, 26), V(42, 30), V(40, 34), V(42, 38), V(40, 42));
            b.Line(0.6f, steam, V(56, 32), V(58, 36), V(56, 40));
        }

        /// <summary>No.28 ハナビペン: 目立ちたがりのお調子者。火花の尾を引いて打ち上がり、空中で両ヒレを広げて万歳。背後で花火が開く</summary>
        private static void Hanabi(PenguinBrush b)
        {
            var orange = Rgb(235, 100, 70);
            var orangeShade = PenguinPalette.Shade(orange);
            var colors = new[] { Rgb(255, 120, 160), Rgb(255, 220, 80), Rgb(120, 220, 255) };
            HanabiBurst(b, 46f, 50f, 13f, colors);
            HanabiBurst(b, 12f, 54f, 7f, colors);
            // 打ち上げの軌跡。足元から下へ細く伸ばして、浮いていることをはっきり見せる
            b.Line(0.6f, colors[1], V(33, 9), V(32, 5), V(33, 1));
            b.Sparkle(29, 4, colors[1]);
            b.Sparkle(37, 6, colors[0]);
            b.Sparkle(34, 1, colors[2]);
            b.Flipper(V(38, 30), V(52, 34), 2.5f, orangeShade);
            b.Foot(27f, 14f, -30f, 0.8f);
            b.Foot(38f, 14f, 30f, 0.8f);
            b.ShadedTilted(32f, 23f, 9f, 10f, -8f, orange);
            b.BellyTilted(30f, 22f, 5.5f, 7f, -8f);
            b.Shaded(28f, 38f, 10.5f, 10f, orange);
            b.White(25f, 37f, 7.5f, 6.5f);
            b.Flipper(V(24, 29), V(9, 40), 2.5f, orangeShade);
            b.TurnedFace(25f, 38f, FaceLeft, EyeStyle.Cross, eyeGap: 6f, open: true);
        }

        /// <summary>花火の玉。3色を順に回した光の筋を放射状に並べる</summary>
        private static void HanabiBurst(PenguinBrush b, float x, float y, float radius, Color32[] colors)
        {
            const int Rays = 12;
            const float InnerRate = 0.3f;
            for (int i = 0; i < Rays; i++)
            {
                float angle = i * Mathf.PI * 2f / Rays;
                var color = colors[i % colors.Length];
                var from = V(x + Mathf.Cos(angle) * radius * InnerRate, y + Mathf.Sin(angle) * radius * InnerRate);
                var tip = V(x + Mathf.Cos(angle) * radius, y + Mathf.Sin(angle) * radius);
                b.Line(0.5f, color, from, tip);
                b.Ellipse(tip.x, tip.y, 1.2f, 1.2f, color);
            }
        }

        /// <summary>No.58 ヒデリペン: 陽気な目立ちたがり。背中に燃える太陽を背負い、サングラスをかけて岩の上で両ヒレを空へ突き上げる</summary>
        private static void Hideri(PenguinBrush b)
        {
            var body = Rgb(200, 110, 60);
            var bodyShade = PenguinPalette.Shade(body);
            var glasses = Rgb(30, 30, 40);
            const int Rays = 12;
            for (int i = 0; i < Rays; i++)
            {
                float angle = i * Mathf.PI * 2f / Rays;
                b.Triangle(OnEllipse(32f, 40f, 15f, 15f, angle - 0.18f), OnEllipse(32f, 40f, 15f, 15f, angle + 0.18f),
                    OnEllipse(32f, 40f, 22f, 21f, angle), FlameOuter);
            }
            b.Ellipse(32f, 40f, 16f, 16f, FlameMiddle);
            b.Ellipse(31.5f, 40.5f, 14f, 14f, FlameInner);
            b.Polygon(V(32, 5), Stone, V(8, 1), V(56, 1), V(52, 9), V(12, 9));
            b.Line(0.6f, PenguinPalette.Shade(Stone), V(20, 9), V(23, 5), V(21, 2));
            b.Line(0.6f, PenguinPalette.Shade(Stone), V(42, 9), V(40, 4));
            b.Feet(32f, 7f, 10f);
            b.Shaded(32f, 22f, 13f, 12f, body);
            b.Belly(31f, 21f, 9f, 9f);
            b.Flipper(V(21, 27), V(10, 41), 3f, bodyShade);
            b.Flipper(V(43, 27), V(54, 41), 3f, bodyShade);
            b.Shaded(32f, 38f, 11f, 10.5f, body);
            b.White(31f, 37f, 8f, 6.5f);
            b.Face(31f, 38f, 4f, EyeStyle.Sharp);
            if (!b.Front) return;
            b.Rect(23, 37, 7, 3, glasses);
            b.Rect(32, 37, 7, 3, glasses);
            b.Rect(30, 39, 2, 1, glasses);
            b.Dot(24, 39, PenguinPalette.EyeLight);
            b.Dot(33, 39, PenguinPalette.EyeLight);
        }

        /// <summary>No.62 カエンペン: 猪突猛進。全身に炎をまとって前のめりに突っ走り、炎が後ろへ長く尾を引く</summary>
        private static void Kaen(PenguinBrush b)
        {
            var red = Rgb(210, 60, 40);
            var redShade = PenguinPalette.Shade(red);
            b.Triangle(V(34, 8), V(34, 34), V(62, 18), FlameOuter);
            b.Triangle(V(36, 12), V(36, 30), V(58, 20), FlameMiddle);
            b.Triangle(V(38, 16), V(38, 26), V(52, 21), FlameInner);
            b.Triangle(V(26, 38), V(36, 32), V(58, 48), FlameOuter);
            b.Triangle(V(28, 37), V(35, 33), V(50, 44), FlameMiddle);
            b.Foot(32f, 3f, -25f);
            b.Foot(14f, 5f, 25f);
            b.ShadedTilted(28f, 19f, 12f, 11f, 35f, red);
            b.BellyTilted(24f, 17f, 7f, 7f, 35f);
            b.Flipper(V(32, 24), V(46, 29), 2.5f, redShade);
            b.Shaded(17f, 31f, 10f, 9.5f, red);
            b.White(15f, 30f, 7f, 6f);
            b.TurnedFace(15f, 31f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f, open: true);
        }
    }
}
