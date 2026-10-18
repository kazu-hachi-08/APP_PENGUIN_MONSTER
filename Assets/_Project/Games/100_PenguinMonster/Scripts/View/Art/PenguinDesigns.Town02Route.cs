using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>3番・4番道路の子（No.181〜196）。雲雀・湖の魚・湯たんぽ・ごろた石・冬の雷・冬芽・いたち</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 CastIron = Rgb(55, 55, 62);
        private static readonly Color32 Steam = Rgb(225, 230, 240);

        /// <summary>No.181 ヒバリペン: 歌うのが大好きな おしゃべり。頭の冠羽を立て、雪原から空へ ぴーちく鳴きながら舞い上がる</summary>
        private static void Hibari(PenguinBrush b)
        {
            var body = Rgb(180, 150, 110);
            var bodyShade = PenguinPalette.Shade(body);
            var streak = Rgb(120, 90, 60);
            b.Ellipse(32f, 3f, 14f, 2f, SnowShade);
            b.Foot(29f, 9f, 30f, 0.55f);
            b.Foot(36f, 8f, -20f, 0.55f);
            b.Flipper(V(39, 20), V(52, 30), 2.5f, bodyShade);
            b.Shaded(32f, 17f, 8.5f, 7.5f, body);
            b.Belly(30f, 16f, 5.5f, 5f);
            b.Shaded(31f, 27f, 8f, 7.5f, body);
            // 冠羽は頭の上に三角を2つ重ね、ぴんと立った寝ぐせのように見せる
            b.Triangle(V(30, 33), V(35, 33), V(36, 40), streak);
            b.Triangle(V(32, 33), V(36, 32), V(40, 37), streak);
            b.White(29f, 26.5f, 6f, 4.5f);
            b.Flipper(V(25, 20), V(13, 29), 2.5f, bodyShade);
            b.TurnedFace(29f, 28f, FaceLeft, EyeStyle.Smile, eyeGap: 5f, open: true);
            b.Line(0.5f, streak, V(12, 40), V(15, 42), V(18, 40));
            b.Line(0.5f, streak, V(16, 46), V(19, 48), V(22, 46));
        }

        /// <summary>No.182 ハヤテペン: 誰よりも速く飛びたい負けず嫌い。体を矢のように細く倒し、風を切って一直線に突き進む</summary>
        private static void Hayate(PenguinBrush b)
        {
            var body = Rgb(80, 110, 160);
            var bodyShade = PenguinPalette.Shade(body);
            var wind = Rgb(210, 230, 250);
            b.Line(0.8f, wind, V(40, 50), V(62, 50));
            b.Line(0.8f, wind, V(46, 44), V(63, 44));
            b.Line(0.8f, wind, V(44, 10), V(62, 10));
            b.Polygon(V(44, 36), bodyShade, V(36, 34), V(58, 52), V(62, 46), V(52, 30));
            b.Triangle(V(48, 22), V(62, 18), V(60, 28), bodyShade);
            b.Foot(52f, 14f, -60f, 0.8f);
            b.ShadedTilted(36f, 26f, 16f, 10f, -25f, body);
            b.BellyTilted(33f, 23f, 11f, 6f, -25f);
            b.Shaded(18f, 36f, 10f, 9f, body);
            b.White(15f, 35f, 7f, 6f);
            b.Line(1f, Rgb(240, 240, 250), V(9, 40), V(20, 42));
            b.Polygon(V(28, 22), bodyShade, V(30, 26), V(14, 10), V(8, 12), V(24, 20));
            b.TurnedFace(15f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
        }

        /// <summary>No.183 ワカサギペン: 群れでいると元気な さみしがり。透きとおった銀の細い体で、凍った湖の釣り穴から ぴょこんと顔を出す</summary>
        private static void Wakasagi(PenguinBrush b)
        {
            var body = Rgb(200, 215, 225);
            var bodyShade = PenguinPalette.Shade(body);
            var silver = Rgb(150, 175, 200);
            b.Ellipse(32f, 6f, 22f, 5f, IceLight);
            b.Ellipse(32f, 6f, 10f, 3f, DeepWater);
            b.Shaded(32f, 12f, 6.5f, 7f, body);
            b.Belly(31f, 11f, 4f, 4.5f);
            if (b.Front)
            {
                b.Line(0.6f, silver, V(26, 13), V(38, 13));
            }
            b.Shaded(32f, 22f, 7f, 6.5f, body);
            b.White(30f, 21.5f, 5f, 4f);
            b.Flipper2(V(26, 12), V(19, 16), 2f, bodyShade);
            b.Ellipse(32f, 4.5f, 10f, 1.5f, Water);
            b.TurnedFace(30f, 23f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
            b.Ellipse(14f, 22f, 1.2f, 1.2f, IceLight);
            b.Ellipse(50f, 26f, 1.5f, 1.5f, IceLight);
            b.Ellipse(46f, 32f, 1f, 1f, IceLight);
        }

        /// <summary>No.184 ヒメマスペン: 湖育ちの おすまし屋。赤く色づいた背中を誇らしげに見せ、湖面から弧を描いて跳ねる</summary>
        private static void Himemasu(PenguinBrush b)
        {
            var body = Rgb(205, 90, 80);
            var bodyShade = PenguinPalette.Shade(body);
            var head = Rgb(90, 120, 100);
            b.Ellipse(34f, 4f, 20f, 3f, Water);
            b.Line(0.7f, DeepWater, V(18, 4), V(50, 4));
            b.Triangle(V(46, 16), V(58, 8), V(58, 22), bodyShade);
            b.ShadedTilted(36f, 19f, 12f, 8.5f, -25f, body);
            b.BellyTilted(33f, 17f, 8f, 5f, -25f);
            b.Shaded(22f, 29f, 9f, 8.5f, head);
            b.White(19f, 28.5f, 6.5f, 5.5f);
            b.Flipper(V(32, 24), V(40, 30), 2f, bodyShade);
            b.Flipper(V(30, 16), V(24, 9), 2.5f, bodyShade);
            b.TurnedFace(19f, 30f, FaceLeft, EyeStyle.Round, eyeGap: 6f, open: true);
            b.Ellipse(12f, 10f, 1.5f, 1.5f, Water);
            b.Ellipse(56f, 30f, 1.5f, 1.5f, Water);
            b.Ellipse(8f, 16f, 1f, 1f, IceLight);
        }

        /// <summary>No.185 イトウペン: 湖の主と呼ばれる 物静かな大物。大きな口と赤い尾びれを持ち、湖の底から ゆっくり浮かび上がる</summary>
        private static void Itou(PenguinBrush b)
        {
            var body = Rgb(95, 110, 85);
            var bodyShade = PenguinPalette.Shade(body);
            var red = Rgb(200, 70, 60);
            var spot = Rgb(55, 65, 50);
            b.Polygon(V(52, 14), red, V(46, 16), V(62, 4), V(63, 26));
            b.Foot(38f, 3f);
            b.Foot(24f, 3f);
            b.Shaded(32f, 19f, 15f, 15f, body);
            b.Belly(29f, 18f, 10f, 11f);
            if (b.Front)
            {
                foreach (var p in new[] { V(42, 26), V(44, 18), V(41, 12), V(20, 26), V(18, 18) })
                {
                    b.Ellipse(p.x, p.y, 1.2f, 1.2f, spot);
                }
            }
            b.Flipper(V(44, 24), V(56, 32), 3f, bodyShade);
            b.Shaded(30f, 39f, 13f, 10.5f, body);
            b.White(27f, 37f, 10f, 6.5f);
            b.Flipper(V(19, 24), V(8, 16), 3f, bodyShade);
            b.TurnedFace(27f, 40f, FaceLeft, EyeStyle.Sleepy, eyeGap: 8f, open: true);
            if (b.Front)
            {
                b.Line(0.6f, spot, V(16, 34), V(30, 33));
            }
            b.Ellipse(8f, 50f, 2f, 2f, IceLight);
            b.Ellipse(12f, 56f, 1.5f, 1.5f, IceLight);
            b.Ellipse(54f, 54f, 1.5f, 1.5f, IceLight);
        }

        /// <summary>No.186 ユタンポペン: 寒がりの ぬくぬく好き。ぎざぎざの湯たんぽの体に栓の帽子をかぶり、ほかほか湯気を立てて うとうとする</summary>
        private static void Yutanpo(PenguinBrush b)
        {
            var body = Rgb(230, 150, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var cap = Rgb(200, 60, 50);
            b.Foot(26f, 3f, 0f, 0.6f);
            b.Foot(38f, 3f, 0f, 0.6f);
            b.Shaded(32f, 14f, 12f, 10f, body);
            // 湯たんぽの波の筋を横に何本も入れる
            for (int y = 8; y <= 20; y += 4)
            {
                b.Line(0.6f, bodyShade, V(22, y), V(42, y));
            }
            b.Belly(30f, 13f, 5f, 4.5f);
            b.Flipper2(V(21, 14), V(15, 10), 2f, bodyShade);
            b.Shaded(31f, 27f, 7.5f, 6.5f, body);
            b.Shaded(33f, 34f, 3.5f, 2.5f, cap);
            b.White(29f, 26.5f, 5.5f, 4f);
            b.TurnedFace(29f, 28f, FaceLeft, EyeStyle.Sleepy, eyeGap: 5f);
            b.Line(0.7f, Steam, V(36, 38), V(38, 42), V(36, 46));
            b.Line(0.7f, Steam, V(41, 37), V(43, 41), V(41, 45), V(43, 49));
        }

        /// <summary>No.187 ダルマストーブペン: みんなが集まる町の世話焼き。鉄の胴の窓で炎が燃え、頭の煙突から もくもく煙を吐いて部屋じゅうを暖める</summary>
        private static void DarumaStove(PenguinBrush b)
        {
            var ironShade = PenguinPalette.Shade(CastIron);
            var ironLight = Rgb(95, 95, 105);
            b.Rect(28, 48, 8, 10, ironShade);
            b.Rect(29, 49, 5, 9, CastIron);
            b.Ellipse(36f, 60f, 5f, 3f, Steam);
            b.Ellipse(44f, 62f, 4f, 2f, Steam);
            b.Rect(18, 0, 6, 4, ironShade);
            b.Rect(40, 0, 6, 4, ironShade);
            b.Shaded(32f, 19f, 17f, 16f, CastIron);
            b.Rect(15, 18, 34, 2, ironLight);
            // 胴の窓から見える炎。窓は丸く抜き、中に炎を3色で重ねる
            b.Ellipse(32f, 12f, 8f, 6f, ironShade);
            b.Ellipse(32f, 12f, 6.5f, 4.5f, FlameOuter);
            b.Ellipse(32f, 11f, 4.5f, 3f, FlameMiddle);
            b.Ellipse(32f, 10.5f, 2.5f, 1.5f, FlameInner);
            b.Flipper2(V(16, 24), V(6, 16), 3f, ironShade);
            b.Shaded(32f, 39f, 12f, 10f, CastIron);
            b.White(30f, 38f, 9f, 6.5f);
            b.TurnedFace(29f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
            b.Line(0.7f, FlameInner, V(10, 34), V(12, 30));
            b.Line(0.7f, FlameInner, V(54, 34), V(52, 30));
        }

        /// <summary>No.188 ゴロタペン: 川原の石の上で ごろごろしたい のんびり屋。丸くすり減った石の体を、小石の山にどっかり預けている</summary>
        private static void Gorota(PenguinBrush b)
        {
            var body = Rgb(165, 160, 150);
            var bodyShade = PenguinPalette.Shade(body);
            var pebble = Rgb(130, 125, 120);
            b.Shaded(18f, 4f, 6f, 4f, pebble);
            b.Shaded(46f, 4f, 7f, 4f, pebble);
            b.Shaded(32f, 3f, 8f, 3f, Stone);
            b.Shaded(32f, 13f, 11f, 9f, body);
            b.Belly(30f, 12f, 6f, 5f);
            b.Flipper2(V(22, 13), V(15, 9), 2f, bodyShade);
            b.Shaded(31f, 25f, 8f, 6.5f, body);
            b.White(29f, 24.5f, 6f, 4f);
            b.Line(0.6f, bodyShade, V(36, 29), V(38, 26));
            b.TurnedFace(29f, 26f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
        }

        /// <summary>No.189 ガンクツペン: 山の洞窟そのものになった 無口な守り手。岩の体からつららのような鍾乳石が垂れ、背中に暗い岩屋の口が開く</summary>
        private static void Gankutsu(PenguinBrush b)
        {
            var body = Rgb(130, 120, 110);
            var bodyShade = PenguinPalette.Shade(body);
            var cave = Rgb(40, 35, 40);
            b.Polygon(V(32, 24), bodyShade, V(4, 2), V(10, 28), V(20, 44), V(44, 44), V(54, 28), V(60, 2));
            b.Foot(40f, 3f);
            b.Foot(24f, 3f);
            b.Shaded(32f, 19f, 15f, 15f, body);
            if (!b.Front)
            {
                b.Ellipse(32f, 16f, 7f, 9f, cave);
            }
            b.Belly(30f, 17f, 9f, 10f);
            b.Line(0.7f, bodyShade, V(44, 30), V(40, 24), V(44, 18));
            // 鍾乳石は山の裾の内側から下向きに垂らす（顔の近くだと涙に見えるので）
            b.Triangle(V(8, 14), V(12, 14), V(10, 6), SnowShade);
            b.Triangle(V(52, 14), V(56, 14), V(54, 5), SnowShade);
            b.Shaded(31f, 38f, 12f, 10f, body);
            b.Shaded(31f, 46f, 9f, 4f, Stone);
            b.White(29f, 37f, 9f, 6.5f);
            b.Flipper2(V(18, 24), V(8, 18), 3f, bodyShade);
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
        }

        /// <summary>No.190 ヒバナペン: じっとしていられない やんちゃ坊主。体から小さな火花を ぱちぱち散らし、両足で跳びはねる</summary>
        private static void Hibana(PenguinBrush b)
        {
            var body = Rgb(250, 180, 70);
            var bodyShade = PenguinPalette.Shade(body);
            b.SplayedFeet(28f, 37f, 6f, 30f, 0.55f);
            b.Shaded(32f, 15f, 8f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.Line(0.8f, Bolt, V(30, 33), V(33, 37), V(31, 39), V(35, 43));
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(16, 24), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
            b.Sparkle(12, 30, Bolt);
            b.Sparkle(52, 30, Bolt);
            b.Sparkle(16, 12, BoltShade);
            b.Sparkle(50, 12, BoltShade);
            b.Sparkle(44, 42, Bolt);
        }

        /// <summary>No.191 イカズチペン: 気が短い雷さま見習い。背中に小太鼓の輪を背負い、ヒレで打ち鳴らして稲妻を落とす</summary>
        private static void Ikazuchi(PenguinBrush b)
        {
            var body = Rgb(120, 110, 190);
            var bodyShade = PenguinPalette.Shade(body);
            var drum = Rgb(220, 60, 60);
            // 背中の太鼓の輪は体より先に描き、頭の後ろに半円が見えるようにする
            b.Line(1.2f, Wood, V(14, 30), V(18, 44), V(32, 50), V(46, 44), V(50, 30));
            foreach (var p in new[] { V(14, 30), V(18, 44), V(32, 50), V(46, 44), V(50, 30) })
            {
                b.Shaded(p.x, p.y, 3f, 3f, drum);
            }
            b.Foot(37f, 3f);
            b.Foot(26f, 3f);
            b.Shaded(32f, 15f, 10.5f, 11f, body);
            b.Belly(30f, 14f, 6.5f, 7.5f);
            b.Shaded(31f, 32f, 9.5f, 9f, body);
            b.White(28f, 31f, 7f, 6f);
            b.Flipper(V(22, 18), V(10, 24), 2.5f, bodyShade);
            b.Flipper(V(42, 18), V(52, 12), 2.5f, bodyShade);
            b.TurnedFace(28f, 32f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
            b.Line(1f, Bolt, V(8, 22), V(4, 14), V(9, 13), V(5, 4));
            b.Line(1f, Bolt, V(56, 12), V(60, 6), V(57, 5), V(61, 0));
        }

        /// <summary>No.192 ブリオコシペン: 冬の嵐を呼ぶ 荒ぶる主。雪雲の外套をまとい、雷鳴と一緒に雪をたたきつけて冬の海を目覚めさせる</summary>
        private static void Briokoshi(PenguinBrush b)
        {
            var body = Rgb(55, 65, 110);
            var bodyShade = PenguinPalette.Shade(body);
            var cloud = Rgb(110, 115, 140);
            var cloudShade = PenguinPalette.Shade(cloud);
            b.Ellipse(14f, 46f, 12f, 8f, cloudShade);
            b.Ellipse(50f, 46f, 12f, 8f, cloudShade);
            b.Ellipse(32f, 52f, 16f, 8f, cloudShade);
            b.Ellipse(14f, 47f, 10f, 6.5f, cloud);
            b.Ellipse(50f, 47f, 10f, 6.5f, cloud);
            b.Ellipse(32f, 53f, 14f, 6.5f, cloud);
            YamiBolt(b, BoltShade, Bolt, V(6, 40), V(10, 28), V(4, 26), V(8, 12));
            YamiBolt(b, BoltShade, Bolt, V(58, 40), V(54, 28), V(60, 26), V(56, 12));
            b.Foot(38f, 3f);
            b.Foot(26f, 3f);
            b.Shaded(32f, 18f, 14f, 15f, body);
            b.Belly(30f, 17f, 9f, 11f);
            b.Line(1f, Bolt, V(24, 24), V(30, 20), V(26, 16), V(32, 10));
            b.Shaded(32f, 37f, 11.5f, 10f, body);
            b.White(29f, 36f, 8.5f, 6.5f);
            b.Flipper2(V(20, 26), V(10, 36), 3f, bodyShade);
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f, open: true);
            Snowflake(b, 20f, 6f, 3f, SnowWhite);
            Snowflake(b, 50f, 8f, 3f, SnowWhite);
        }

        /// <summary>No.193 フユメペン: 春まで眠っていたい ねぼすけ。かたい芽のうろこに くるまり、頭の先から ちょこんと緑をのぞかせる</summary>
        private static void Fuyume(PenguinBrush b)
        {
            var body = Rgb(150, 105, 75);
            var bodyShade = PenguinPalette.Shade(body);
            var sprout = Rgb(120, 200, 100);
            b.Foot(28f, 3f, 0f, 0.55f);
            b.Foot(36f, 3f, 0f, 0.55f);
            // 芽のうろこは体の左右から重なる涙形で描き、つぼみの形にする
            b.Polygon(V(32, 18), bodyShade, V(32, 36), V(22, 22), V(24, 6), V(40, 6), V(42, 22));
            b.Shaded(32f, 14f, 8.5f, 9f, body);
            b.Belly(31f, 13f, 4.5f, 5.5f);
            b.Shaded(32f, 25f, 7f, 6.5f, body);
            b.Triangle(V(29, 30), V(35, 30), V(33, 38), sprout);
            b.Line(0.6f, bodyShade, V(24, 18), V(30, 8));
            b.Line(0.6f, bodyShade, V(40, 18), V(34, 8));
            b.White(30f, 24.5f, 5f, 4f);
            b.TurnedFace(30f, 25f, FaceLeft, EyeStyle.Sleepy, eyeGap: 5f);
        }

        /// <summary>No.194 ネコヤナギペン: 雪解けを知らせる やさしい子。銀色の ふわふわの花穂を頭と枝いっぱいにつけ、春風にそよぐ</summary>
        private static void Nekoyanagi(PenguinBrush b)
        {
            var body = Rgb(120, 85, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var catkin = Rgb(225, 225, 230);
            var catkinShade = Rgb(185, 185, 195);
            b.Line(1.2f, body, V(42, 28), V(56, 44), V(58, 58));
            b.Line(1.2f, body, V(22, 28), V(8, 42), V(6, 56));
            foreach (var p in new[] { V(52, 40), V(57, 50), V(58, 58), V(12, 38), V(7, 48), V(6, 56) })
            {
                Catkin(b, p.x, p.y, catkin, catkinShade);
            }
            b.Foot(38f, 3f);
            b.Foot(26f, 3f);
            b.Shaded(32f, 18f, 13f, 14f, body);
            b.Belly(30f, 17f, 8.5f, 10f);
            b.Shaded(32f, 37f, 11f, 10f, body);
            Catkin(b, 26f, 47f, catkin, catkinShade);
            Catkin(b, 33f, 49f, catkin, catkinShade);
            Catkin(b, 40f, 46f, catkin, catkinShade);
            b.White(29f, 36f, 8.5f, 6.5f);
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
        }

        /// <summary>ネコヤナギの花穂。縦長の小さな楕円に影を付ける</summary>
        private static void Catkin(PenguinBrush b, float x, float y, Color32 catkin, Color32 shade)
        {
            b.Ellipse(x, y, 2.5f, 3.5f, shade);
            b.Ellipse(x - 0.5f, y + 0.5f, 1.8f, 2.8f, catkin);
        }

        /// <summary>No.195 イタチペン: すばしこい いたずらっ子。冬毛の白い体に黒い尾の先、低く身をかがめて雪の上を走り回る</summary>
        private static void Itachi(PenguinBrush b)
        {
            var body = Rgb(240, 238, 230);
            var bodyShade = PenguinPalette.Shade(body);
            var tail = Rgb(40, 35, 40);
            b.Line(2f, body, V(42, 12), V(54, 18));
            b.Ellipse(56f, 19f, 3f, 2.5f, tail);
            b.Foot(40f, 3f, -30f, 0.6f);
            b.Foot(24f, 4f, 30f, 0.6f);
            b.ShadedTilted(34f, 11f, 10f, 6.5f, -10f, body);
            b.Shaded(22f, 19f, 7.5f, 7f, body);
            b.Triangle(V(18, 24), V(21, 25), V(18, 29), bodyShade);
            b.Triangle(V(25, 25), V(28, 24), V(28, 29), bodyShade);
            b.Flipper(V(30, 9), V(22, 4), 2f, bodyShade);
            b.TurnedFace(20f, 20f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f, open: true);
        }

        /// <summary>No.196 カマイタチペン: つむじ風にのって現れる 闇の剣客。両ヒレが鎌になり、すれ違いざまに すぱっと切り抜ける</summary>
        private static void Kamaitachi(PenguinBrush b)
        {
            var body = Rgb(85, 70, 110);
            var bodyShade = PenguinPalette.Shade(body);
            var blade = Rgb(210, 220, 235);
            var wind = Rgb(180, 200, 220);
            NightArc(b, wind, 32f, 26f, 28f, 22f, 200f, 340f);
            NightArc(b, wind, 32f, 30f, 24f, 18f, 20f, 160f);
            b.Line(2.5f, body, V(42, 14), V(58, 22));
            b.Foot(38f, 3f, -20f);
            b.Foot(24f, 3f, 20f);
            b.ShadedTilted(32f, 18f, 11f, 13f, 15f, body);
            b.BellyTilted(30f, 17f, 7f, 9f, 15f);
            b.Shaded(30f, 35f, 10f, 9.5f, body);
            b.Triangle(V(22, 41), V(26, 43), V(22, 50), bodyShade);
            b.Triangle(V(34, 43), V(38, 41), V(38, 50), bodyShade);
            b.White(27f, 34f, 7.5f, 6.5f);
            b.Flipper(V(20, 24), V(10, 28), 2.5f, bodyShade);
            b.Polygon(V(8, 32), blade, V(10, 28), V(2, 44), V(4, 30));
            b.Flipper(V(42, 24), V(52, 30), 2.5f, bodyShade);
            b.Polygon(V(54, 34), blade, V(52, 30), V(62, 44), V(58, 30));
            b.TurnedFace(27f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
        }
    }
}
