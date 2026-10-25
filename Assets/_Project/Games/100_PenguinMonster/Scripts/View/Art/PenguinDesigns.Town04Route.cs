using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>7番道路の子（No.241〜255）。そよ風・追い風・木枯らし・みぞれ・霧氷・がれ場・断崖・ハゼ・スズキ・コマイ・真鱈・よもぎ・すすき・風船・気球</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 WindStreak = Rgb(235, 245, 250);
        private static readonly Color32 DryLeaf = Rgb(215, 130, 60);
        private static readonly Color32 Rime = Rgb(225, 240, 250);
        private static readonly Color32 FishSilver = Rgb(175, 190, 200);

        /// <summary>風の筋。後ろへ流れる短い線で、動いている向きを見せる</summary>
        private static void WindLines(PenguinBrush b, Color32 color, params (float X, float Y, float Length)[] lines)
        {
            foreach (var (x, y, length) in lines)
            {
                b.Line(0.5f, color, V(x, y), V(x + length, y));
            }
        }

        /// <summary>No.241 ソヨカゼペン: 野原を ふわっと吹き抜ける そよ風の子。頭の若葉を揺らして、風の来る方を教えてくれる</summary>
        private static void Soyokaze(PenguinBrush b)
        {
            var body = Rgb(130, 200, 180);
            var bodyShade = PenguinPalette.Shade(body);
            WindLines(b, WindStreak, (40f, 30f, 10f), (44f, 22f, 12f), (42f, 14f, 9f));
            b.SplayedFeet(28f, 37f, 6f, 25f, 0.55f);
            b.Shaded(32f, 15f, 8f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            // 若葉は風下へ倒して、そよ風に吹かれているように見せる
            b.Line(0.5f, Wood, V(32, 33), V(34, 36));
            b.Tilted(37f, 37f, 4f, 1.8f, 20f, BambooLeaf);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper(V(25, 17), V(18, 21), 2f, bodyShade);
            b.Flipper(V(39, 17), V(45, 14), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Smile, eyeGap: 5f);
        }

        /// <summary>No.242 オイカゼペン: 背中に風を受けて 走りだしたら止まらない。後ろへなびく冠羽が 速さの印</summary>
        private static void Oikaze(PenguinBrush b)
        {
            var body = Rgb(80, 165, 160);
            var bodyShade = PenguinPalette.Shade(body);
            WindLines(b, WindStreak, (44f, 36f, 14f), (48f, 26f, 12f), (46f, 16f, 14f), (40f, 8f, 10f));
            // 冠羽は後ろへ3本。走る向きの反対へ流す
            b.Line(1f, bodyShade, V(36, 44), V(46, 47), V(52, 45));
            b.Line(1f, bodyShade, V(36, 42), V(47, 42), V(53, 39));
            b.Foot(25f, 5f, 35f, 0.65f);
            b.Foot(38f, 6f, -35f, 0.65f);
            b.ShadedTilted(33f, 18f, 11f, 9f, -15f, body);
            b.BellyTilted(30f, 16f, 7f, 6f, -15f);
            b.Shaded(28f, 34f, 9f, 8.5f, body);
            b.White(26f, 33f, 6.5f, 5f);
            b.Flipper(V(38, 22), V(50, 18), 2.5f, bodyShade);
            b.Flipper(V(24, 22), V(16, 14), 2.5f, bodyShade);
            b.TurnedFace(26f, 35f, FaceLeft, EyeStyle.Round, eyeGap: 6f, open: true);
        }

        /// <summary>No.243 コガラシペン: 秋の終わりを告げる 冷たい大風。翼をひと振りすると 木の葉が舞い、冬がやって来る</summary>
        private static void Kogarashi(PenguinBrush b)
        {
            var body = Rgb(95, 110, 125);
            var bodyShade = PenguinPalette.Shade(body);
            var wingTip = Rgb(60, 70, 85);
            // 大きく広げた翼は先を暗くして、風を切る羽の重なりを見せる
            b.Polygon(V(14, 34), bodyShade, V(20, 38), V(2, 52), V(4, 42), V(1, 34), V(8, 28), V(20, 24));
            b.Polygon(V(50, 34), bodyShade, V(44, 38), V(62, 52), V(60, 42), V(63, 34), V(56, 28), V(44, 24));
            b.Line(1f, wingTip, V(3, 50), V(5, 41), V(2, 34));
            b.Line(1f, wingTip, V(61, 50), V(59, 41), V(62, 34));
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(29f, 18f, 9f, 9f);
            b.Shaded(31f, 38f, 11f, 9.5f, body);
            b.White(28f, 37f, 8f, 6.5f);
            // 冠羽を風上へ逆立て、舞い上がる木の葉を周りに散らす
            b.Triangle(V(30, 46), V(36, 46), V(40, 56), wingTip);
            b.Triangle(V(26, 45), V(31, 47), V(28, 55), wingTip);
            foreach (var (x, y, angle) in new[] { (8f, 58f, 30f), (55f, 58f, -40f), (12f, 14f, 60f), (54f, 12f, -20f) })
            {
                b.Tilted(x, y, 3f, 1.6f, angle, DryLeaf);
            }
            b.TurnedFace(28f, 39f, FaceLeft, EyeStyle.Sharp, eyeGap: 8f);
        }

        /// <summary>No.244 ミゾレペン: 雪と雨の 間で揺れる 気分屋。体の半分に雪、半分に雨粒をまとい、泣いたり笑ったり忙しい</summary>
        private static void Mizore(PenguinBrush b)
        {
            var body = Rgb(150, 175, 205);
            var bodyShade = PenguinPalette.Shade(body);
            // 左に雪、右に雨。混ざり合う天気を体の左右で描き分ける
            foreach (var p in new[] { V(14, 40), V(19, 48), V(11, 30), V(17, 22) })
            {
                b.Ellipse(p.x, p.y, 1.5f, 1.5f, SnowWhite);
            }
            foreach (var p in new[] { V(48, 44), V(52, 34), V(46, 26), V(54, 22) })
            {
                b.Line(0.5f, Water, p, V(p.x - 1f, p.y - 3f));
            }
            b.Feet(32f, 5f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.Ellipse(29f, 33f, 5f, 2f, SnowWhite);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(19, 11), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Worried, eyeGap: 5f);
        }

        /// <summary>No.245 ムヒョウペン: 吹きつける風に 氷の花を咲かせる 霧氷の子。風上へ向かって 白いとげが伸び続ける</summary>
        private static void Muhyou(PenguinBrush b)
        {
            var body = Rgb(170, 200, 225);
            var bodyShade = PenguinPalette.Shade(body);
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(30f, 18f, 9f, 9f);
            b.Shaded(32f, 37f, 11f, 9.5f, body);
            b.White(29f, 36f, 8f, 6.5f);
            // 霧氷は風上（左）にだけ伸びるので、とげを片側へそろえて生やす
            foreach (var (y, length) in new[] { (46f, 10f), (40f, 14f), (33f, 13f), (26f, 15f), (18f, 13f), (11f, 9f) })
            {
                float rootX = y > 30f ? 22f : 19f;
                b.Triangle(V(rootX, y + 2.5f), V(rootX, y - 2.5f), V(rootX - length, y + 1f), Rime);
                b.Line(0.4f, IceShade, V(rootX, y), V(rootX - length + 2f, y + 0.8f));
            }
            b.Flipper(V(44, 24), V(52, 14), 3f, bodyShade);
            b.TurnedFace(29f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Sparkles(SnowWhite, V(50, 50), V(56, 40));
        }

        /// <summary>No.246 ガレバペン: 崩れた岩の坂に住む 角ばった子。転んでも転んでも ごつごつの体で また起き上がる</summary>
        private static void Gareba(PenguinBrush b)
        {
            var rock = Rgb(150, 135, 120);
            var rockShade = PenguinPalette.Shade(rock);
            // 体を丸でなく角ばった多角形にして、割れた石らしく見せる
            b.Feet(32f, 5f);
            b.Polygon(V(32, 15), rockShade, V(22, 8), V(42, 7), V(44, 17), V(40, 23), V(24, 23), V(20, 15));
            b.Polygon(V(31, 16), rock, V(23, 9), V(40, 9), V(42, 17), V(38, 22), V(25, 22), V(22, 15));
            b.Belly(30f, 14f, 5f, 4.5f);
            b.Polygon(V(32, 28), rockShade, V(24, 22), V(40, 22), V(42, 30), V(37, 36), V(26, 36), V(22, 29));
            b.Polygon(V(31, 29), rock, V(25, 23), V(39, 23), V(40, 30), V(36, 35), V(27, 35), V(24, 29));
            b.Line(0.5f, rockShade, V(36, 34), V(34, 30), V(38, 27));
            b.White(30f, 27.5f, 5.5f, 4f);
            b.Flipper2(V(23, 17), V(17, 12), 2.5f, rockShade);
            b.TurnedFace(30f, 29f, FaceLeft, EyeStyle.Dot, eyeGap: 5f);
            b.Shaded(48f, 4f, 3f, 2.5f, Gravel);
            b.Shaded(15f, 3f, 2.5f, 2f, Gravel);
        }

        /// <summary>No.247 ダンガイペン: 海に切り立つ 断崖そのもの。てっぺんの草は どんな大風にも 飛ばされたことがない</summary>
        private static void Dangai(PenguinBrush b)
        {
            var rock = Rgb(120, 105, 95);
            var rockShade = PenguinPalette.Shade(rock);
            var grass = Rgb(110, 160, 80);
            b.Feet(32f, 10f);
            // 縦に長く、片側（右）を垂直に切り落として崖の形にする
            b.Polygon(V(32, 28), rockShade, V(12, 4), V(52, 4), V(52, 50), V(30, 56), V(16, 48), V(10, 26));
            b.Polygon(V(31, 29), rock, V(14, 6), V(50, 6), V(50, 48), V(30, 54), V(17, 47), V(12, 26));
            b.Line(0.6f, rockShade, V(50, 40), V(44, 36), V(50, 30));
            b.Line(0.6f, rockShade, V(50, 18), V(42, 14), V(50, 10));
            b.Line(0.6f, rockShade, V(14, 18), V(20, 21));
            b.Belly(29f, 18f, 9f, 11f);
            b.White(27f, 41f, 8f, 6f);
            // 崖のてっぺんの草。風下へ倒しておく
            b.Line(1f, grass, V(22, 52), V(26, 57), V(30, 56));
            b.Line(1f, grass, V(30, 55), V(36, 59), V(40, 57));
            b.Flipper2(V(13, 30), V(6, 16), 3.5f, rockShade);
            b.TurnedFace(27f, 43f, FaceLeft, EyeStyle.Sharp, eyeGap: 8f);
        }

        /// <summary>No.248 ハゼペン: 池の浅瀬で じっと待つ 釣られ上手。大きな頭のてっぺんの目で、上を通る影を 見逃さない</summary>
        private static void Haze(PenguinBrush b)
        {
            var body = Rgb(190, 165, 120);
            var bodyShade = PenguinPalette.Shade(body);
            var spot = Rgb(130, 105, 75);
            b.Ellipse(32f, 4f, 20f, 3f, Water);
            b.Feet(32f, 5f, 6f);
            b.Shaded(32f, 14f, 9f, 7f, body);
            b.Belly(30f, 13f, 5.5f, 4.5f);
            // ハゼらしく頭を横に大きく、腹びれのヒレを地面へ広げる
            b.Shaded(31f, 25f, 11f, 8f, body);
            foreach (var p in new[] { V(38, 15), V(40, 21), V(24, 30), V(36, 31) })
            {
                b.Ellipse(p.x, p.y, 1.2f, 1.2f, spot);
            }
            b.White(29f, 24f, 7f, 4.5f);
            b.Flipper2(V(24, 12), V(14, 8), 2f, bodyShade);
            b.TurnedFace(29f, 28f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
        }

        /// <summary>No.249 スズキペン: 育つたびに名前を変える 出世魚の子。銀色の体と背びれのとげで、大橋の下の急な流れを のぼる</summary>
        private static void Suzuki(PenguinBrush b)
        {
            var body = FishSilver;
            var bodyShade = PenguinPalette.Shade(body);
            var fin = Rgb(90, 105, 120);
            b.Line(0.6f, Water, V(6, 10), V(16, 12));
            b.Line(0.6f, Water, V(48, 6), V(58, 8));
            // 背びれのとげは尖った三角を並べ、尾びれは後ろへ大きく割る
            foreach (float x in new[] { 26f, 31f, 36f, 41f })
            {
                b.Triangle(V(x - 3f, 32f), V(x + 2f, 32f), V(x + 1f, 40f), fin);
            }
            b.Triangle(V(46, 20), V(60, 30), V(58, 12), fin);
            b.Foot(24f, 4f, 20f, 0.7f);
            b.Foot(36f, 4f, -20f, 0.7f);
            b.ShadedTilted(34f, 20f, 15f, 10f, 5f, body);
            b.BellyTilted(30f, 16f, 10f, 5f, 5f);
            b.Line(0.6f, bodyShade, V(22, 22), V(46, 23));
            b.Shaded(20f, 28f, 9f, 8f, body);
            b.White(18f, 27f, 6f, 4.5f);
            b.Flipper(V(32, 16), V(38, 8), 2.5f, bodyShade);
            b.TurnedFace(18f, 29f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
        }

        /// <summary>No.250 コマイペン: 氷の下で群れて泳ぐ 小さな魚の子。凍った湖の穴から ひょっこり顔を出す</summary>
        private static void Komai(PenguinBrush b)
        {
            var body = Rgb(185, 200, 190);
            var bodyShade = PenguinPalette.Shade(body);
            // 足元は氷に開いた丸い穴。体は穴から出てきたところ
            b.Ellipse(32f, 7f, 20f, 5f, ThinIce);
            b.Ellipse(32f, 7f, 12f, 3f, DeepWater);
            b.Shaded(32f, 15f, 8f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Ellipse(32f, 6f, 11f, 2f, DeepWater);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.Triangle(V(29, 33), V(35, 33), V(36, 39), bodyShade);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 16), V(18, 18), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
            b.Ellipse(14f, 9f, 2f, 1.5f, SnowWhite);
            b.Ellipse(50f, 10f, 2.5f, 1.5f, SnowWhite);
        }

        /// <summary>No.251 マダラペン: 雪の海の底を ゆうゆう泳ぐ 大食らい。まだら模様の太い体と あごのひげで、冷たい海でも 動じない</summary>
        private static void Madara(PenguinBrush b)
        {
            var body = Rgb(150, 135, 110);
            var bodyShade = PenguinPalette.Shade(body);
            var mottle = Rgb(85, 70, 55);
            b.Feet(32f, 9f);
            b.Shaded(32f, 19f, 16f, 14f, body);
            b.Belly(29f, 17f, 11f, 10f);
            b.Shaded(31f, 38f, 12f, 10f, body);
            b.White(28f, 37f, 8f, 6f);
            // まだら模様は大小の点を散らす。おなかと顔の白には付けない
            foreach (var (x, y, r) in new[] { (44f, 24f, 2.2f), (45f, 13f, 1.8f), (40f, 31f, 1.6f), (16f, 26f, 1.8f), (39f, 44f, 2f), (22f, 46f, 1.6f), (33f, 47f, 1.4f) })
            {
                b.Ellipse(x, y, r, r, mottle);
            }
            // あごのひげ（マダラの印）
            b.Line(0.7f, mottle, V(24, 32), V(23, 28));
            b.Flipper2(V(17, 22), V(8, 14), 3.5f, bodyShade);
            b.Ellipse(31f, 48f, 8f, 2.5f, SnowWhite);
            b.TurnedFace(28f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 8f);
        }

        /// <summary>No.252 ヨモギペン: 道ばたの よもぎの葉を 頭に広げた子。葉をこすると いい香りがして、けんかが止まる</summary>
        private static void Yomogi(PenguinBrush b)
        {
            var body = Rgb(140, 175, 130);
            var bodyShade = PenguinPalette.Shade(body);
            var leaf = Rgb(90, 140, 90);
            var leafBack = Rgb(200, 215, 200);
            b.Feet(32f, 5f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            // よもぎの葉はぎざぎざ。裏の白っぽい色を縁に少し見せる
            foreach (var (x, y, angle) in new[] { (24f, 38f, 50f), (32f, 41f, 90f), (40f, 38f, 130f) })
            {
                b.Tilted(x, y, 6f, 2.8f, angle, leafBack);
                b.Tilted(x, y, 5.5f, 2.2f, angle, leaf);
            }
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(19, 11), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
        }

        /// <summary>No.253 ススキペン: 秋の野原で 風と踊る すすきの子。頭の穂が 光を受けて 金色に波打つ</summary>
        private static void Susuki(PenguinBrush b)
        {
            var body = Rgb(120, 150, 95);
            var bodyShade = PenguinPalette.Shade(body);
            var plume = Rgb(235, 210, 150);
            var stem = Rgb(150, 160, 90);
            // 穂は3本。風下（右）へしなって垂れる
            foreach (var (root, mid, tip) in new[] { (V(28, 44), V(30, 54), V(40, 60)), (V(32, 45), V(38, 54), V(48, 56)), (V(36, 44), V(44, 50), V(54, 50)) })
            {
                b.Line(0.6f, stem, root, mid);
                b.Line(1.8f, plume, mid, tip);
            }
            b.Feet(32f, 7f);
            b.Shaded(32f, 18f, 12f, 12f, body);
            b.Belly(30f, 17f, 8f, 8f);
            b.Shaded(31f, 36f, 10f, 9f, body);
            b.White(28f, 35f, 7f, 5.5f);
            b.Flipper(V(20, 22), V(12, 30), 3f, bodyShade);
            b.Flipper(V(43, 22), V(52, 16), 3f, bodyShade);
            b.TurnedFace(28f, 37f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
        }

        /// <summary>No.254 フウセンペン: 風に乗って ふわふわ漂う 風船の子。ひもを つかんでもらうと 安心して眠る</summary>
        private static void Fuusen(PenguinBrush b)
        {
            var body = Rgb(240, 140, 160);
            var bodyShade = PenguinPalette.Shade(body);
            // 地面から浮いているので足を体に引っこめ、下へひもを垂らす
            b.Line(0.5f, Wood, V(32, 10), V(30, 6), V(33, 3), V(31, 0));
            b.Shaded(32f, 26f, 13f, 14f, body);
            b.Belly(30f, 23f, 8f, 8f);
            b.Triangle(V(30, 12), V(34, 12), V(32, 9), bodyShade);
            b.Ellipse(26f, 34f, 3f, 2f, PenguinPalette.EyeLight);
            b.Flipper2(V(20, 24), V(15, 28), 2f, bodyShade);
            b.TurnedFace(30f, 30f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
        }

        /// <summary>No.255 キキュウペン: 大きな気球を ふくらませて 空を旅する子。かごの中から 遠くの町を 見下ろすのが好き</summary>
        private static void Kikyuu(PenguinBrush b)
        {
            var balloon = Rgb(230, 110, 90);
            var stripe = Rgb(250, 210, 90);
            var body = Rgb(90, 95, 120);
            var bodyShade = PenguinPalette.Shade(body);
            // 気球は縦じまの大きな丸。下の口からかごへ綱を4本おろす
            b.Shaded(32f, 44f, 20f, 18f, balloon);
            b.Tilted(24f, 44f, 3f, 17f, -8f, stripe);
            b.Tilted(40f, 44f, 3f, 17f, 8f, stripe);
            b.Rect(30, 26, 5, 34, stripe);
            b.Line(0.5f, Wood, V(18, 30), V(22, 16));
            b.Line(0.5f, Wood, V(46, 30), V(42, 16));
            b.Shaded(32f, 21f, 9f, 7f, body);
            b.White(30f, 20f, 6f, 4.5f);
            b.TurnedFace(30f, 22f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
            // かごは体の下半分を隠すように前に描く
            b.Rect(21, 4, 22, 11, PenguinPalette.Shade(WoodLight));
            b.Rect(22, 5, 20, 9, WoodLight);
            b.Line(0.5f, Wood, V(22, 10), V(42, 10));
            b.Flipper(V(40, 17), V(48, 20), 2.5f, bodyShade);
        }
    }
}
