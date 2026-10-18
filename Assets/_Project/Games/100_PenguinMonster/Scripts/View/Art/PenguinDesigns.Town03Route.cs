using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>5番・6番道路の子（No.211〜225）。豆電球・雷鳴・砂利・笹・燕・鰰・寒鰤・霰・氷雨・停電</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Thundercloud = Rgb(110, 115, 135);
        private static readonly Color32 BulbGlass = Rgb(255, 245, 170);
        private static readonly Color32 Gravel = Rgb(165, 160, 150);
        private static readonly Color32 BambooLeaf = Rgb(90, 160, 80);

        /// <summary>No.211 マメデンペン: 暗い所が苦手な 小さな灯り。頭の豆電球を ぴかっと光らせ、うれしいと点いたり消えたりする</summary>
        private static void Mameden(PenguinBrush b)
        {
            var body = Rgb(240, 200, 90);
            var bodyShade = PenguinPalette.Shade(body);
            b.SplayedFeet(28f, 37f, 6f, 30f, 0.55f);
            b.Shaded(32f, 15f, 8f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            // 電球は口金を先に描き、ガラスの球が上に乗って見えるようにする
            b.Rect(29, 32, 6, 3, IronLight);
            b.Line(0.5f, Iron, V(29, 33), V(35, 33));
            b.Ellipse(32f, 40f, 5f, 5f, BulbGlass);
            b.Ellipse(31f, 41f, 2f, 2f, PenguinPalette.EyeLight);
            b.Line(0.5f, FlameOuter, V(30, 37), V(31, 40), V(33, 38), V(34, 41));
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper(V(25, 17), V(17, 22), 2f, bodyShade);
            b.Flipper(V(39, 17), V(46, 25), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Sparkle, eyeGap: 5f, open: true);
            b.Line(0.5f, Bolt, V(22, 42), V(19, 44));
            b.Line(0.5f, Bolt, V(42, 42), V(45, 44));
            b.Line(0.5f, Bolt, V(32, 48), V(32, 51));
        }

        /// <summary>No.212 ゴロゴロペン: いつも不機嫌そうな 雷雲の子。頭に雷雲をのせ、怒るとおなかの中から ごろごろ鳴る</summary>
        private static void Gorogoro(PenguinBrush b)
        {
            var body = Rgb(130, 140, 200);
            var bodyShade = PenguinPalette.Shade(body);
            b.Feet(32f, 6f);
            b.Shaded(32f, 18f, 12f, 11f, body);
            b.Belly(30f, 17f, 8f, 8f);
            if (b.Front)
            {
                b.Line(0.6f, BoltShade, V(26, 18), V(29, 16), V(31, 19), V(34, 16));
            }
            b.Shaded(32f, 33f, 10f, 9f, body);
            // 頭の雷雲は3つの丸を重ねてもこもこさせ、両脇から小さな稲妻を垂らす
            b.Line(0.8f, Bolt, V(18, 44), V(16, 40), V(19, 38), V(17, 34));
            b.Line(0.8f, Bolt, V(46, 44), V(48, 40), V(45, 38), V(47, 34));
            b.Shaded(24f, 44f, 6f, 4.5f, Thundercloud);
            b.Shaded(40f, 44f, 6f, 4.5f, Thundercloud);
            b.Shaded(32f, 47f, 7f, 5f, Thundercloud);
            b.White(29f, 32f, 7.5f, 6f);
            b.Flipper2(V(21, 22), V(12, 16), 3f, bodyShade);
            b.TurnedFace(29f, 34f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
        }

        /// <summary>No.213 ナルカミペン: 雪起こしの雷を呼ぶ 冬の空の主。稲妻の角を立て、肩の雷雲から地面へ雷を落とす</summary>
        private static void Narukami(PenguinBrush b)
        {
            var body = Rgb(70, 75, 140);
            var bodyShade = PenguinPalette.Shade(body);
            var cloud = Rgb(90, 92, 110);
            // 肩の雷雲から落ちる稲妻を先に描き、雲と体の後ろから伸びて見えるようにする
            b.Line(1.2f, Bolt, V(10, 34), V(6, 26), V(11, 22), V(5, 12));
            b.Line(1.2f, Bolt, V(54, 34), V(58, 26), V(53, 22), V(59, 12));
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 15f, 14f, body);
            b.Belly(29f, 18f, 10f, 10f);
            if (b.Front)
            {
                b.Line(1f, Bolt, V(26, 24), V(31, 19), V(28, 17), V(33, 11));
            }
            b.Shaded(31f, 39f, 12f, 10f, body);
            // 角は稲妻の形の多角形。左右で少し高さを変えて、勢いのある形にする
            b.Polygon(V(24, 52), Bolt, V(22, 47), V(26, 47), V(25, 52), V(28, 52), V(21, 61), V(23, 54), V(20, 54));
            b.Polygon(V(38, 52), Bolt, V(36, 47), V(40, 47), V(39, 52), V(42, 52), V(35, 60), V(37, 54), V(34, 54));
            b.Shaded(14f, 34f, 7f, 5f, cloud);
            b.Shaded(50f, 34f, 7f, 5f, cloud);
            b.White(28f, 38f, 9f, 7f);
            b.Flipper2(V(18, 24), V(8, 14), 3.5f, bodyShade);
            b.TurnedFace(28f, 40f, FaceLeft, EyeStyle.Sharp, eyeGap: 8f, open: true);
        }

        /// <summary>No.214 ジャリペン: 砂利の山で生まれた がんばり屋。体じゅうに小石をくっつけ、ころころ転がって 道を作る</summary>
        private static void Jari(PenguinBrush b)
        {
            var body = Rgb(140, 130, 120);
            var bodyShade = PenguinPalette.Shade(body);
            var pebble = Rgb(195, 190, 180);
            // 足元の砂利の山。小石の大きさと色を少しずつ変えて、ざらざらに見せる
            b.Ellipse(32f, 5f, 18f, 4f, PenguinPalette.Shade(Gravel));
            foreach (var p in new[] { V(16, 6), V(22, 3), V(42, 3), V(48, 6), V(36, 2), V(27, 2), V(31, 7), V(20, 8), V(44, 8) })
            {
                // 小石ごとに暗い縁を付け、山の中でも1粒ずつ見分けられるようにする
                b.Ellipse(p.x, p.y, 3.2f, 2.7f, PenguinPalette.Outline);
                b.Shaded(p.x, p.y, 2.5f, 2f, Gravel);
            }
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            foreach (var p in new[] { V(38, 20), V(26, 31), V(37, 32), V(40, 13) })
            {
                b.Shaded(p.x, p.y, 1.8f, 1.5f, pebble);
            }
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(17, 13), 2.5f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Dot, eyeGap: 5f);
        }

        /// <summary>No.215 ガンペキペン: 雪の岸壁のように 動かない守り手。四角い岩の体に雪をかぶり、どんな雷も びくともせず受け止める</summary>
        private static void Ganpeki(PenguinBrush b)
        {
            var rock = Rgb(130, 115, 100);
            var rockShade = PenguinPalette.Shade(rock);
            var strata = Rgb(100, 88, 76);
            b.Feet(32f, 9f);
            // 岩の体は四角く切り立たせ、横の地層の線で崖らしく見せる
            b.Polygon(V(32, 22), rockShade, V(14, 4), V(50, 4), V(52, 36), V(12, 36));
            b.Polygon(V(31, 23), rock, V(15, 6), V(48, 6), V(50, 35), V(13, 35));
            b.Line(0.6f, strata, V(14, 28), V(30, 27), V(50, 29));
            b.Line(0.6f, strata, V(14, 18), V(24, 19), V(50, 17));
            b.Line(0.6f, strata, V(15, 10), V(36, 9), V(49, 11));
            b.Belly(30f, 19f, 8f, 11f);
            b.Shaded(30f, 43f, 10f, 8.5f, rock);
            // 頭と肩の雪は岩より後に塗り、崖のてっぺんに積もって見えるようにする
            b.Ellipse(30f, 50f, 9f, 3f, SnowWhite);
            b.Ellipse(14f, 36f, 5f, 2f, SnowWhite);
            b.Ellipse(50f, 36f, 5f, 2f, SnowWhite);
            b.White(28f, 42f, 7.5f, 5.5f);
            b.Flipper2(V(12, 28), V(6, 14), 3.5f, rockShade);
            b.TurnedFace(28f, 44f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
        }

        /// <summary>No.216 ササペン: 雪の下で春を待つ のんびり屋。頭から笹の葉を2枚のばし、葉に積もった雪を ぱさっと落とす</summary>
        private static void Sasa(PenguinBrush b)
        {
            var body = Rgb(120, 180, 100);
            var bodyShade = PenguinPalette.Shade(body);
            b.Foot(28f, 6f, 20f, 0.55f);
            b.Foot(37f, 6f, -20f, 0.55f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            // 笹の葉は細長い楕円を傾けて2枚。先に雪を少しのせる
            b.Tilted(25f, 40f, 9f, 2.5f, 55f, PenguinPalette.Shade(BambooLeaf));
            b.Tilted(40f, 39f, 9f, 2.5f, -50f, BambooLeaf);
            b.Ellipse(21f, 47f, 2.5f, 1.5f, SnowWhite);
            b.Ellipse(44f, 46f, 2.5f, 1.5f, SnowWhite);
            b.Line(0.6f, Wood, V(32, 33), V(32, 36));
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(18, 11), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Sleepy, eyeGap: 5f);
        }

        /// <summary>No.217 クマザサペン: 冬の笹やぶを仕切る 力自慢。白く縁どられた大きな笹の葉を背に広げ、太いヒレで 何でも なぎ払う</summary>
        private static void Kumazasa(PenguinBrush b)
        {
            var body = Rgb(70, 130, 70);
            var bodyShade = PenguinPalette.Shade(body);
            // 背中の葉は扇のように5枚。冬のクマザサらしく、白い縁を先に塗ってから緑を少し小さく重ねる
            foreach (var (x, y, angle) in new[] { (12f, 34f, 35f), (16f, 46f, 70f), (32f, 52f, 90f), (48f, 46f, 110f), (52f, 34f, 145f) })
            {
                b.Tilted(x, y, 10f, 3.5f, angle, SnowWhite);
                b.Tilted(x, y, 9f, 2.3f, angle, BambooLeaf);
            }
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(29f, 18f, 9f, 9f);
            b.Shaded(31f, 37f, 11f, 9.5f, body);
            b.White(28f, 36f, 8f, 6.5f);
            // 力こぶを作るように、ヒレを太く曲げて上へ構える
            b.Flipper2(V(18, 24), V(10, 30), 4f, bodyShade);
            b.Ellipse(10f, 31f, 3f, 3f, bodyShade);
            b.Ellipse(54f, 31f, 3f, 3f, bodyShade);
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f, open: true);
        }

        /// <summary>No.218 ツバメペン: いつも急いでいる せっかち屋。赤い喉と二つに割れた尾で、雪の上を すいっと低く走り抜ける</summary>
        private static void Tsubame(PenguinBrush b)
        {
            var body = Rgb(40, 50, 90);
            var bodyShade = PenguinPalette.Shade(body);
            var throat = Rgb(210, 70, 60);
            // 燕の尾は後ろへ2本。走る向きの反対へ長く伸ばす
            b.Triangle(V(40, 14), V(44, 18), V(58, 8), bodyShade);
            b.Triangle(V(40, 16), V(43, 20), V(56, 18), bodyShade);
            b.Foot(26f, 5f, 30f, 0.55f);
            b.Foot(38f, 6f, -40f, 0.55f);
            b.ShadedTilted(33f, 16f, 9f, 7f, -20f, body);
            b.BellyTilted(31f, 14f, 6f, 4.5f, -20f);
            b.Shaded(26f, 27f, 8f, 7f, body);
            b.White(24f, 26f, 6f, 4f);
            if (b.Front)
            {
                b.Ellipse(23f, 22.5f, 3.5f, 1.8f, throat);
            }
            b.Flipper(V(36, 20), V(50, 26), 2.5f, bodyShade);
            b.Flipper(V(28, 18), V(18, 12), 2f, bodyShade);
            b.TurnedFace(24f, 28f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f);
            b.Line(0.6f, SnowShade, V(44, 28), V(54, 28));
            b.Line(0.6f, SnowShade, V(46, 32), V(56, 32));
        }

        /// <summary>No.219 ハヤブサペン: 空の狩人と呼ばれる 気高い子。目の下の黒い髭模様と しま模様の胸で、翼を大きく広げて 空を見すえる</summary>
        private static void Hayabusa(PenguinBrush b)
        {
            var body = Rgb(95, 110, 135);
            var bodyShade = PenguinPalette.Shade(body);
            var mask = Rgb(40, 42, 55);
            var bar = Rgb(150, 155, 170);
            // 翼は体の後ろで大きく広げ、先を3段にとがらせて風切り羽に見せる
            b.Polygon(V(18, 30), bodyShade, V(22, 22), V(2, 30), V(4, 40), V(10, 38), V(8, 46), V(24, 36));
            b.Polygon(V(46, 30), bodyShade, V(42, 22), V(62, 30), V(60, 40), V(54, 38), V(56, 46), V(40, 36));
            b.Feet(32f, 7f);
            b.Shaded(32f, 19f, 12f, 13f, body);
            b.Belly(30f, 18f, 8f, 9f);
            if (b.Front)
            {
                foreach (int y in new[] { 12, 16, 20, 24 })
                {
                    b.Line(0.5f, bar, V(25, y), V(28, y - 1), V(31, y), V(34, y - 1));
                }
            }
            b.Shaded(31f, 37f, 10f, 9f, body);
            b.White(29f, 36f, 7f, 5.5f);
            if (b.Front)
            {
                // 隼の髭模様。目の下から頬へ黒い帯を垂らす
                b.Triangle(V(23, 38), V(27, 38), V(24, 31), mask);
            }
            b.TurnedFace(29f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
        }

        /// <summary>No.220 ハタハタペン: 雷の鳴る日に浜へ来る 臆病な子。大きな胸びれを ぱたぱたさせ、驚くと体がぴりっと光る</summary>
        private static void Hatahata(PenguinBrush b)
        {
            var body = Rgb(175, 160, 140);
            var bodyShade = PenguinPalette.Shade(body);
            var spot = Rgb(120, 100, 80);
            b.Ellipse(32f, 5f, 22f, 3.5f, Water);
            b.Line(0.6f, DeepWater, V(14, 5), V(50, 5));
            // 鰰の胸びれは扇のように大きい。体より先に描いて両脇に広げる
            b.Polygon(V(24, 18), bodyShade, V(24, 22), V(10, 28), V(8, 18), V(12, 12), V(24, 14));
            b.Polygon(V(40, 18), bodyShade, V(40, 22), V(54, 28), V(56, 18), V(52, 12), V(40, 14));
            b.Shaded(32f, 15f, 8.5f, 8f, body);
            b.Belly(30f, 14f, 5.5f, 5.5f);
            b.Shaded(32f, 27f, 8f, 7f, body);
            foreach (var p in new[] { V(37, 31), V(39, 26), V(26, 31) })
            {
                b.Ellipse(p.x, p.y, 1f, 1f, spot);
            }
            b.White(30f, 26f, 6f, 4.5f);
            b.TurnedFace(30f, 28f, FaceLeft, EyeStyle.Worried, eyeGap: 5f, open: true);
            b.Line(0.5f, Bolt, V(10, 34), V(13, 36), V(11, 38));
            b.Line(0.5f, Bolt, V(52, 36), V(55, 38), V(53, 40));
        }

        /// <summary>No.221 カンブリペン: 冬の荒海を泳ぎきる 大物。青い背に黄色い帯を引き、雷の音を合図に 大波から飛び出す</summary>
        private static void Kanburi(PenguinBrush b)
        {
            var back = Rgb(50, 90, 150);
            var backShade = PenguinPalette.Shade(back);
            var stripe = Rgb(240, 210, 70);
            // 波しぶきは体より先に描き、波から飛び出したように見せる
            b.Ellipse(32f, 4f, 30f, 4f, DeepWater);
            b.Polygon(V(14, 8), Water, V(4, 4), V(8, 12), V(14, 16), V(20, 10), V(24, 4));
            b.Polygon(V(48, 8), Water, V(40, 4), V(44, 12), V(52, 14), V(58, 8), V(60, 4));
            // 二つに割れた尾びれ
            b.Triangle(V(48, 22), V(62, 30), V(58, 22), backShade);
            b.Triangle(V(48, 20), V(60, 12), V(58, 20), backShade);
            b.ShadedTilted(34f, 24f, 16f, 10f, -20f, back);
            b.BellyTilted(31f, 20f, 12f, 5.5f, -20f);
            b.Line(1f, stripe, V(20, 28), V(34, 25), V(48, 20));
            b.Shaded(19f, 34f, 10f, 9f, back);
            b.White(16f, 33f, 7f, 6f);
            b.Line(1f, stripe, V(24, 31), V(28, 29));
            b.Flipper(V(30, 20), V(24, 10), 3f, backShade);
            b.Flipper(V(38, 28), V(46, 38), 3f, backShade);
            b.TurnedFace(16f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
        }

        /// <summary>No.222 アラレペン: じっとしていられない はね回り屋。霰の粒のような ぼこぼこの丸い体で、雪の上を ぽんぽん跳ねる</summary>
        private static void Arare(PenguinBrush b)
        {
            var body = Rgb(210, 230, 245);
            var bodyShade = PenguinPalette.Shade(body);
            b.Ellipse(32f, 4f, 10f, 1.5f, SnowShade);
            b.Foot(28f, 8f, 30f, 0.55f);
            b.Foot(37f, 8f, -30f, 0.55f);
            // 体と頭をひとつの丸にして、霰の粒らしく見せる。周りのこぶで ぼこぼこにする
            foreach (var p in new[] { V(22, 14), V(42, 14), V(20, 26), V(44, 26), V(26, 34), V(38, 34) })
            {
                b.Shaded(p.x, p.y, 3f, 3f, body);
            }
            b.Shaded(32f, 22f, 12f, 12f, body);
            b.Belly(30f, 17f, 6f, 5f);
            b.White(30f, 24f, 7f, 5f);
            b.Flipper2(V(22, 20), V(14, 24), 2f, bodyShade);
            b.TurnedFace(30f, 26f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
            foreach (var p in new[] { V(10, 40), V(16, 48), V(48, 44), V(54, 36), V(40, 52) })
            {
                b.Shaded(p.x, p.y, 1.8f, 1.8f, IceLight);
            }
        }

        /// <summary>No.223 ヒサメペン: 冷たい雨の日に現れる 物静かな子。凍った番傘を差し、傘から氷のしずくを しとしと落とす</summary>
        private static void Hisame(PenguinBrush b)
        {
            var body = Rgb(120, 150, 190);
            var bodyShade = PenguinPalette.Shade(body);
            var umbrella = Rgb(110, 190, 220);
            // 番傘は頭より先に描いて後ろに差す。骨の線で和傘らしく見せる
            b.Polygon(V(30, 49), PenguinPalette.Shade(umbrella),
                V(7, 43), V(11, 50), V(19, 56), V(30, 59), V(41, 56), V(49, 50), V(53, 43), V(41, 45), V(30, 43), V(19, 45));
            b.Polygon(V(30, 50), umbrella,
                V(10, 44), V(13, 50), V(20, 55), V(30, 57), V(40, 55), V(47, 50), V(50, 44), V(41, 46), V(30, 44), V(19, 46));
            b.Line(0.5f, IceShade, V(30, 59), V(19, 45));
            b.Line(0.5f, IceShade, V(30, 59), V(30, 43));
            b.Line(0.5f, IceShade, V(30, 59), V(41, 45));
            b.Rect(29, 59, 3, 2, Wood);
            b.Line(0.8f, Wood, V(30, 45), V(36, 22));
            b.Feet(32f, 6f);
            b.Shaded(32f, 17f, 11f, 11f, body);
            b.Belly(30f, 16f, 7f, 8f);
            b.Shaded(30f, 32f, 9.5f, 8.5f, body);
            b.White(28f, 31f, 7f, 5.5f);
            b.Flipper(V(42, 20), V(37, 24), 3f, bodyShade);
            b.Flipper(V(21, 20), V(14, 12), 2.5f, bodyShade);
            b.TurnedFace(28f, 33f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            foreach (var p in new[] { V(8, 38), V(52, 38), V(6, 28), V(56, 26), V(10, 18), V(54, 14) })
            {
                b.Line(0.5f, IceLight, p, V(p.x, p.y - 3f));
            }
        }

        /// <summary>No.224 テイデンペン: いたずら好きの 停電の子。しっぽのコードの先はコンセントのプラグで、こっそり抜いて町を真っ暗にする</summary>
        private static void Teiden(PenguinBrush b)
        {
            var body = Rgb(70, 65, 90);
            var bodyShade = PenguinPalette.Shade(body);
            var cord = Rgb(40, 38, 48);
            // しっぽのコードは体の後ろから くねらせ、先に2本の刃のプラグを付ける
            b.Line(0.8f, cord, V(38, 10), V(46, 6), V(52, 10), V(50, 18), V(54, 22));
            b.Rect(52, 22, 5, 4, IronLight);
            b.Rect(53, 26, 1, 3, Iron);
            b.Rect(55, 26, 1, 3, Iron);
            b.SplayedFeet(28f, 37f, 6f, 30f, 0.55f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(31f, 26f, 8f, 7.5f, body);
            b.White(29f, 25.5f, 6f, 4.5f);
            b.Flipper(V(25, 18), V(18, 26), 2f, bodyShade);
            b.Flipper(V(39, 17), V(45, 12), 2f, bodyShade);
            b.TurnedFace(29f, 27f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f);
            b.Sparkle(14, 34, Bolt);
            b.Sparkle(44, 38, BoltShade);
        }

        /// <summary>No.225 クラヤミペン: 闇にまぎれる 夜の電気使い。頭巾のような黒いマントで身を包み、暗がりで 目と稲妻だけを光らせる</summary>
        private static void Kurayami(PenguinBrush b)
        {
            var body = Rgb(45, 40, 65);
            var cloak = Rgb(30, 28, 45);
            var cloakShade = PenguinPalette.Shade(cloak);
            // マントは裾を ぎざぎざに広げ、体を覆う大きな三角にする
            b.Polygon(V(32, 26), cloakShade, V(32, 54), V(8, 4), V(16, 8), V(24, 3), V(32, 8), V(40, 3), V(48, 8), V(56, 4));
            b.Polygon(V(31, 27), cloak, V(31, 52), V(10, 6), V(17, 10), V(24, 5), V(31, 10), V(38, 5), V(46, 10), V(53, 6));
            if (b.Front)
            {
                b.Ellipse(30f, 18f, 7f, 9f, PenguinPalette.BellyShade);
                b.Ellipse(29f, 19f, 6f, 8f, PenguinPalette.Belly);
            }
            b.Shaded(30f, 36f, 9f, 8f, body);
            // 頭巾のとがりを頭の上へ
            b.Triangle(V(22, 40), V(40, 40), V(34, 52), cloak);
            b.White(28f, 35f, 6.5f, 4.5f);
            b.TurnedFace(28f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            b.Line(0.8f, Bolt, V(6, 30), V(10, 26), V(7, 22), V(11, 18));
            b.Line(0.8f, Bolt, V(56, 34), V(52, 30), V(56, 26), V(53, 22));
            b.Sparkle(14, 46, MoonYellow);
            b.Sparkle(50, 48, MoonYellow);
        }
    }
}
