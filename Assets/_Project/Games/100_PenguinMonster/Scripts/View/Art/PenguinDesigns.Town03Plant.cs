using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// No.226〜240の子。雪明かり・銀河・雪靴・かんじき・電線・雪の石・カイロ・暖炉と、
    /// 道場主の切り札（発電・雷神）・町でもらう灯り・発電所に居座る子・セントエルモの火
    /// </summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 LampGlow = Rgb(255, 225, 130);
        private static readonly Color32 Brick = Rgb(175, 80, 60);
        private static readonly Color32 SteelGray = Rgb(150, 155, 165);
        private static readonly Color32 ElmoFlame = Rgb(170, 200, 255);

        /// <summary>No.226 ユキアカリペン: 夜道を照らす やさしい子。小さな雪灯籠を抱え、ほんのり灯る明かりで 迷子を家まで送る</summary>
        private static void Yukiakari(PenguinBrush b)
        {
            var body = Rgb(190, 200, 235);
            var bodyShade = PenguinPalette.Shade(body);
            b.Foot(24f, 6f, 30f, 0.55f);
            b.Foot(33f, 6f, -30f, 0.55f);
            b.Shaded(28f, 15f, 8f, 7.5f, body);
            b.Belly(26f, 14f, 5f, 5f);
            b.Shaded(28f, 26f, 8f, 7.5f, body);
            b.White(26f, 25.5f, 6f, 4.5f);
            b.Flipper(V(21, 18), V(14, 12), 2f, bodyShade);
            // 足元の雪灯籠。かまくらの形に窓を開け、中の火を見せる。ヒレを添えて守っている
            b.Shaded(46f, 9f, 8f, 7f, SnowWhite);
            b.Ellipse(46f, 3f, 9f, 1.5f, SnowShade);
            b.Rect(43, 6, 6, 5, LampGlow);
            b.Ellipse(46f, 8f, 1.2f, 1.8f, FlameOuter);
            b.Flipper(V(35, 18), V(41, 16), 2f, bodyShade);
            b.TurnedFace(26f, 27f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
            b.Sparkle(14, 36, AuroraGreen);
            b.Sparkle(50, 34, AuroraPink);
            b.Sparkle(42, 46, MoonYellow);
        }

        /// <summary>No.227 ギンガペン: 星空を体に映す 夢見がちな子。紺の体に天の川がきらめき、背中の渦巻く銀河で 空を見上げる人を 惹きつける</summary>
        private static void Ginga(PenguinBrush b)
        {
            var body = Rgb(40, 45, 100);
            var bodyShade = PenguinPalette.Shade(body);
            var milky = Rgb(180, 170, 240);
            // 背中の渦巻き銀河。楕円を傾けて重ね、中心を明るくする
            b.Tilted(32f, 36f, 26f, 9f, 20f, Rgb(80, 70, 150));
            b.Tilted(32f, 36f, 20f, 5f, 20f, milky);
            b.Ellipse(32f, 36f, 5f, 4f, MoonYellow);
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(29f, 18f, 9f, 9f);
            b.Shaded(31f, 37f, 11f, 9.5f, body);
            // 体の星は正面も後ろも見えるようにする（体の模様なので）
            foreach (var p in new[] { V(40, 26), V(42, 14), V(22, 28), V(36, 44), V(24, 42) })
            {
                b.Dot((int)p.x, (int)p.y, MoonYellow);
            }
            b.Line(0.5f, milky, V(20, 10), V(28, 20), V(34, 32), V(42, 40));
            b.White(28f, 36f, 8f, 6.5f);
            b.Flipper2(V(18, 24), V(8, 18), 3.5f, bodyShade);
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Cross, eyeGap: 7f);
            b.Star(8f, 52f, 3f, 1.2f, MoonYellow);
            b.Star(56f, 54f, 2.5f, 1f, AuroraPink);
            b.Sparkle(54, 8, MoonYellow);
        }

        /// <summary>No.228 ユキグツペン: どこへでも歩いて行く 元気者。ぶかぶかの赤い雪靴を履き、深い雪でも ずんずん進む</summary>
        private static void Yukigutsu(PenguinBrush b)
        {
            var body = Rgb(150, 150, 165);
            var bodyShade = PenguinPalette.Shade(body);
            var boot = Rgb(200, 60, 55);
            var fur = SnowWhite;
            // 雪靴は足の代わりに描く。歩いている途中で、前の靴を少し浮かせる
            b.Rect(19, 2, 10, 5, PenguinPalette.Shade(boot));
            b.Rect(19, 3, 9, 3, boot);
            b.Rect(22, 6, 6, 5, boot);
            b.Ellipse(25f, 11f, 4f, 1.5f, fur);
            b.Rect(36, 5, 10, 5, PenguinPalette.Shade(boot));
            b.Rect(36, 6, 9, 3, boot);
            b.Rect(38, 9, 6, 4, boot);
            b.Ellipse(41f, 13f, 4f, 1.5f, fur);
            b.Shaded(32f, 18f, 9f, 7.5f, body);
            b.Belly(30f, 17f, 5.5f, 5f);
            b.Shaded(32f, 29f, 8f, 7.5f, body);
            b.White(30f, 28.5f, 6f, 4.5f);
            b.Flipper(V(25, 20), V(16, 14), 2f, bodyShade);
            b.Flipper(V(39, 20), V(47, 26), 2f, bodyShade);
            b.TurnedFace(30f, 30f, FaceLeft, EyeStyle.Smile, eyeGap: 5f, open: true);
            b.Ellipse(10f, 3f, 3f, 1.5f, SnowShade);
            b.Ellipse(54f, 3f, 3f, 1.5f, SnowShade);
        }

        /// <summary>No.229 カンジキペン: 雪山を知りつくした 頼れる案内役。丸いかんじきで雪に沈まず、杖を突いて 吹雪の中でも道を切り開く</summary>
        private static void Kanjiki(PenguinBrush b)
        {
            var body = Rgb(140, 105, 75);
            var bodyShade = PenguinPalette.Shade(body);
            var straw = Rgb(210, 185, 120);
            // かんじきは輪の中に縄を渡した楕円。足より先に描いて、足が上に乗るようにする
            foreach (float x in new[] { 22f, 42f })
            {
                b.Ellipse(x, 4f, 9f, 3.5f, WoodLight);
                b.Ellipse(x, 4f, 7.5f, 2.2f, SnowShade);
                b.Line(0.4f, Wood, V(x - 7f, 4f), V(x + 7f, 4f));
                b.Line(0.4f, Wood, V(x - 3f, 2f), V(x + 3f, 6f));
                b.Line(0.4f, Wood, V(x + 3f, 2f), V(x - 3f, 6f));
            }
            b.Foot(22f, 6f, 0f, 0.7f);
            b.Foot(42f, 6f, 0f, 0.7f);
            // 杖は体の前に立て、ヒレで握る
            b.Line(0.8f, Wood, V(52, 2), V(52, 38));
            b.Shaded(32f, 20f, 13f, 12f, body);
            b.Belly(30f, 19f, 8.5f, 8.5f);
            // 背中の蓑（みの）は肩に わらの房を並べる
            foreach (float x in new[] { 18f, 24f, 40f, 46f })
            {
                b.Triangle(V(x - 3f, 32f), V(x + 3f, 32f), V(x, 24f), straw);
            }
            b.Shaded(31f, 37f, 10.5f, 9f, body);
            b.White(28f, 36f, 7.5f, 6f);
            b.Flipper(V(20, 24), V(10, 16), 3f, bodyShade);
            b.Flipper(V(44, 24), V(52, 28), 3f, bodyShade);
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Line, eyeGap: 7f, open: true);
        }

        /// <summary>No.230 デンセンペン: 電線にとまるのが好きな 見張り役。電線を足でつかみ、流れる電気で ほっぺを黄色く光らせる</summary>
        private static void Densen(PenguinBrush b)
        {
            var body = Rgb(150, 115, 80);
            var bodyShade = PenguinPalette.Shade(body);
            var cap = Rgb(110, 70, 45);
            // 電線は画面の端から端まで、少したわませて渡す
            b.Line(0.6f, CastIron, V(0, 9), V(16, 7), V(32, 6), V(48, 7), V(64, 9));
            b.Foot(28f, 7f, 80f, 0.5f);
            b.Foot(36f, 7f, 100f, 0.5f);
            b.Shaded(32f, 16f, 8f, 7.5f, body);
            b.Belly(30f, 15f, 5f, 5f);
            b.Shaded(32f, 27f, 8f, 7.5f, body);
            b.Ellipse(32f, 31f, 6f, 3f, cap);
            b.White(30f, 26.5f, 6f, 4.5f);
            if (b.Front)
            {
                b.Ellipse(23f, 25f, 1.8f, 1.5f, Bolt);
            }
            b.Flipper2(V(25, 18), V(18, 14), 2f, bodyShade);
            b.TurnedFace(30f, 28f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
            b.Line(0.5f, Bolt, V(10, 12), V(12, 14), V(10, 16));
            b.Line(0.5f, Bolt, V(54, 12), V(52, 14), V(54, 16));
        }

        /// <summary>No.231 テットウペン: 町に電気を届ける 力持ち。鉄塔の骨組みを背負い、広げた翼で 何本もの電線を支える</summary>
        private static void Tettou(PenguinBrush b)
        {
            var body = Rgb(120, 125, 140);
            var bodyShade = PenguinPalette.Shade(body);
            var insulator = Rgb(230, 230, 240);
            // 背中の鉄塔。三角の骨組みと横棒、横へのびる腕の先に がいしを付ける
            b.Line(0.8f, SteelGray, V(12, 2), V(32, 62), V(52, 2));
            foreach (var (y, half) in new[] { (46, 5), (54, 3) })
            {
                b.Line(0.6f, SteelGray, V(32 - half, y), V(32 + half, y));
            }
            b.Line(0.6f, SteelGray, V(27, 46), V(35, 54));
            b.Line(0.6f, SteelGray, V(37, 46), V(29, 54));
            b.Line(0.8f, SteelGray, V(6, 50), V(58, 50));
            b.Line(0.5f, CastIron, V(6, 50), V(0, 46));
            b.Line(0.5f, CastIron, V(58, 50), V(64, 46));
            foreach (float x in new[] { 8f, 18f, 46f, 56f })
            {
                b.Rect((int)x - 1, 46, 2, 4, insulator);
            }
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 13f, 13f, body);
            b.Belly(30f, 18f, 8.5f, 9f);
            b.Shaded(31f, 35f, 10f, 9f, body);
            b.White(28f, 34f, 7.5f, 6f);
            b.Flipper2(V(19, 24), V(6, 34), 3.5f, bodyShade);
            b.TurnedFace(28f, 36f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Line(0.6f, Bolt, V(4, 38), V(2, 34), V(5, 32));
            b.Line(0.6f, Bolt, V(60, 38), V(62, 34), V(59, 32));
        }

        /// <summary>No.232 ユキイシペン: 雪をかぶって じっと座る 無口な子。石のように固い体で、頭と背中に積もった雪を いつまでも乗せている</summary>
        private static void Yukiishi(PenguinBrush b)
        {
            var body = Rgb(120, 125, 135);
            var bodyShade = PenguinPalette.Shade(body);
            b.Ellipse(32f, 4f, 14f, 2.5f, SnowShade);
            b.Foot(27f, 5f, 0f, 0.55f);
            b.Foot(37f, 5f, 0f, 0.55f);
            // 座った姿。胴を横に広く低くして、石らしい どっしりした形にする
            b.Shaded(32f, 13f, 11f, 8f, body);
            b.Belly(30f, 12f, 6f, 5f);
            b.Shaded(32f, 24f, 8f, 7f, body);
            b.Ellipse(32f, 30f, 7f, 2.5f, SnowWhite);
            b.Ellipse(42f, 19f, 3.5f, 1.8f, SnowWhite);
            b.Ellipse(21f, 18f, 3f, 1.5f, SnowWhite);
            if (b.Front)
            {
                b.Line(0.4f, bodyShade, V(36, 10), V(38, 14), V(37, 16));
            }
            b.White(30f, 23.5f, 6f, 4f);
            b.Flipper2(V(22, 14), V(18, 8), 2f, bodyShade);
            b.TurnedFace(30f, 25f, FaceLeft, EyeStyle.Dot, eyeGap: 5f);
        }

        /// <summary>No.233 ヒョウヘキペン: 氷の壁となって 雪山を守る 門番。岩の下半身に 氷の肩をとがらせ、どんな攻撃も はね返す</summary>
        private static void Hyouheki(PenguinBrush b)
        {
            var rock = Rgb(110, 110, 120);
            var rockShade = PenguinPalette.Shade(rock);
            var ice = Rgb(170, 220, 245);
            // 肩の氷柱は体より先に描き、背中から突き出して見せる
            b.Triangle(V(8, 34), V(16, 30), V(4, 50), ice);
            b.Triangle(V(14, 36), V(22, 34), V(14, 54), IceShade);
            b.Triangle(V(56, 34), V(48, 30), V(60, 50), ice);
            b.Triangle(V(50, 36), V(42, 34), V(50, 54), IceShade);
            b.Feet(32f, 9f);
            b.Shaded(32f, 17f, 16f, 13f, rock);
            // 上半身は氷。胴の上に氷の塊を重ねて、岩と氷の二段にする
            b.Shaded(32f, 30f, 13f, 7f, ice);
            b.Belly(29f, 16f, 9f, 8f);
            b.Shaded(31f, 40f, 10f, 8.5f, rock);
            b.Triangle(V(24, 46), V(38, 46), V(31, 54), ice);
            b.White(28f, 39f, 7.5f, 5.5f);
            b.Flipper2(V(17, 20), V(8, 10), 4f, rockShade);
            b.TurnedFace(28f, 41f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
        }

        /// <summary>No.234 カイロペン: さわるとほかほかの 世話焼き。カイロの袋のような四角い体で、寒がりな人のポケットに もぐりこむ</summary>
        private static void Kairo(PenguinBrush b)
        {
            var pouch = Rgb(245, 240, 230);
            var pouchShade = PenguinPalette.Shade(pouch);
            var label = Rgb(235, 120, 60);
            b.Foot(27f, 4f, 20f, 0.55f);
            b.Foot(37f, 4f, -20f, 0.55f);
            // 袋の体は角を丸めた四角。下の2隅を丸く、四角を重ねて作る
            b.Rect(20, 7, 24, 24, pouchShade);
            b.Rect(19, 8, 24, 23, pouch);
            b.Rect(20, 6, 22, 2, pouchShade);
            b.Ellipse(20f, 7f, 1.5f, 1.5f, pouchShade);
            b.Ellipse(43f, 7f, 1.5f, 1.5f, pouchShade);
            if (b.Front)
            {
                b.Rect(22, 9, 18, 8, label);
                b.Line(0.5f, MoonYellow, V(24, 13), V(28, 13));
                b.Line(0.5f, MoonYellow, V(30, 13), V(37, 13));
            }
            // 上の縁は袋の閉じ目。ぎざぎざにする
            for (int x = 19; x < 43; x += 3)
            {
                b.Triangle(V(x, 31), V(x + 3, 31), V(x + 1.5f, 33), pouchShade);
            }
            b.Flipper(V(20, 18), V(13, 14), 2f, pouchShade);
            b.Flipper(V(43, 18), V(50, 22), 2f, pouchShade);
            b.TurnedFace(30f, 24f, FaceLeft, EyeStyle.Smile, eyeGap: 5f);
            b.Line(0.6f, Steam, V(24, 37), V(22, 40), V(24, 43));
            b.Line(0.6f, Steam, V(32, 37), V(30, 40), V(32, 43), V(30, 46));
            b.Line(0.6f, Steam, V(40, 37), V(38, 40), V(40, 43));
        }

        /// <summary>No.235 ダンロペン: 家族みんなを温める 大らかな父さん。レンガの体のおなかで火が燃え、頭の煙突から ぽっぽと煙を上げる</summary>
        private static void Danro(PenguinBrush b)
        {
            var brickShade = PenguinPalette.Shade(Brick);
            var mortar = Rgb(215, 190, 170);
            var soot = Rgb(45, 30, 30);
            // 頭の煙突と煙は一番後ろ
            b.Ellipse(44f, 58f, 4f, 3f, Steam);
            b.Ellipse(40f, 54f, 3f, 2.5f, Steam);
            b.Rect(36, 44, 7, 8, brickShade);
            b.Rect(36, 45, 6, 7, Brick);
            b.Feet(32f, 9f);
            b.Shaded(32f, 19f, 15f, 14f, Brick);
            // レンガの目地。横の線と、段ごとにずらした縦の線
            foreach (int y in new[] { 10, 16, 22, 28 })
            {
                b.Line(0.4f, mortar, V(19, y), V(45, y));
                int shift = (y / 6) % 2 == 0 ? 0 : 4;
                for (int x = 21 + shift; x < 45; x += 8)
                {
                    b.Line(0.4f, mortar, V(x, y), V(x, y + 6));
                }
            }
            b.Shaded(31f, 38f, 11f, 9f, Brick);
            if (b.Front)
            {
                // おなかは暖炉の口。暗いアーチの中で炎を3段に重ねる
                b.Ellipse(30f, 14f, 8f, 9f, mortar);
                b.Ellipse(30f, 14f, 6.5f, 7.5f, soot);
                b.Rect(23, 5, 15, 4, mortar);
                b.Ellipse(30f, 11f, 5f, 4f, FlameOuter);
                b.Ellipse(30f, 11f, 3.5f, 3f, FlameMiddle);
                b.Ellipse(30f, 10f, 2f, 1.5f, FlameInner);
                b.Triangle(V(26, 12), V(34, 12), V(30, 19), FlameOuter);
            }
            b.White(28f, 37f, 8f, 6f);
            b.Flipper2(V(18, 24), V(8, 16), 4f, brickShade);
            b.TurnedFace(28f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 7f, open: true);
        }

        /// <summary>No.236 ハツデンペン: 町の電気を作る 働き者の見習い。頭の風車を回して電気をため、体じゅうを ぴりぴりさせる</summary>
        private static void Hatsuden(PenguinBrush b)
        {
            var body = Rgb(240, 220, 110);
            var bodyShade = PenguinPalette.Shade(body);
            var blade = Rgb(240, 245, 250);
            b.SplayedFeet(28f, 37f, 6f, 30f, 0.55f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            // 頭の風車。軸を立て、3枚の羽根を斜めに回す
            b.Line(0.6f, SteelGray, V(32, 32), V(32, 42));
            b.Tilted(32f, 49f, 6f, 1.8f, 90f, blade);
            b.Tilted(26f, 39f, 6f, 1.8f, 30f, blade);
            b.Tilted(38f, 39f, 6f, 1.8f, -30f, blade);
            b.Ellipse(32f, 42f, 2f, 2f, CastIron);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(17, 23), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
            b.Line(0.5f, BoltShade, V(14, 30), V(16, 28), V(14, 26));
            b.Line(0.5f, BoltShade, V(50, 30), V(48, 28), V(50, 26));
        }

        /// <summary>No.237 ライジンペン: 雷を自在に操る 道場の守り神。小さな角と なびく羽衣をまとい、巨大な稲妻を槍のように構える</summary>
        private static void Raijin(PenguinBrush b)
        {
            var body = Rgb(60, 70, 160);
            var bodyShade = PenguinPalette.Shade(body);
            var horn = Rgb(240, 235, 215);
            var cloth = Rgb(230, 240, 255);
            // 羽衣は背中の後ろで大きく弧を描かせる
            b.Line(1.2f, cloth, V(10, 18), V(6, 34), V(14, 50), V(32, 56), V(50, 50), V(58, 34), V(54, 18));
            // 構えた巨大な稲妻
            b.Polygon(V(52, 30), Bolt, V(50, 58), V(58, 58), V(54, 40), V(60, 40), V(48, 6), V(52, 28), V(46, 28));
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(29f, 18f, 9f, 9f);
            b.Shaded(30f, 37f, 11f, 9.5f, body);
            b.Triangle(V(20, 42), V(24, 44), V(20, 50), horn);
            b.Triangle(V(36, 44), V(40, 42), V(40, 50), horn);
            b.White(27f, 36f, 8f, 6.5f);
            b.Flipper(V(18, 24), V(8, 14), 3.5f, bodyShade);
            b.Flipper(V(46, 24), V(52, 30), 3.5f, bodyShade);
            b.TurnedFace(27f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f, open: true);
        }

        /// <summary>No.238 カンテラペン: 停電の夜に頼りになる 灯り番。カンテラを提げて町を回り、困っている人の足元を照らす</summary>
        private static void Kantera(PenguinBrush b)
        {
            var body = Rgb(130, 95, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var cap = Rgb(60, 70, 90);
            b.Feet(32f, 7f);
            b.Shaded(32f, 18f, 12f, 11f, body);
            b.Belly(30f, 17f, 8f, 8f);
            b.Shaded(31f, 34f, 10f, 9f, body);
            // 見回りの帽子
            b.Ellipse(31f, 41f, 10f, 2.5f, cap);
            b.Shaded(31f, 44f, 7f, 3.5f, cap);
            b.White(29f, 33f, 7.5f, 5.5f);
            b.Flipper(V(21, 22), V(12, 16), 3f, bodyShade);
            // 提げたカンテラ。灯りの輪の上に、取っ手・枠・ガラスの順に重ねる
            b.Flipper(V(43, 24), V(50, 28), 3f, bodyShade);
            b.Line(0.5f, Iron, V(47, 25), V(50, 28), V(53, 25));
            b.Rect(46, 22, 9, 3, Iron);
            b.Rect(47, 11, 7, 11, LampGlow);
            b.Ellipse(50.5f, 15f, 1.5f, 2.5f, FlameOuter);
            b.Line(0.5f, Iron, V(47, 11), V(47, 22));
            b.Line(0.5f, Iron, V(54, 11), V(54, 22));
            b.Rect(46, 9, 9, 2, Iron);
            b.TurnedFace(29f, 35f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
            b.Sparkle(58, 18, LampGlow);
            b.Sparkle(44, 6, LampGlow);
            b.Sparkle(58, 8, MoonYellow);
        }

        /// <summary>No.239 ヘソトリペン: 雷さまの いたずら者。虎じまの腰巻に2本の角、にやりと笑って へそを狙い、発電所の電気を 食べてしまう</summary>
        private static void Hesotori(PenguinBrush b)
        {
            var body = Rgb(240, 170, 50);
            var bodyShade = PenguinPalette.Shade(body);
            var horn = Rgb(245, 240, 225);
            var tigerDark = Rgb(40, 35, 30);
            b.Line(1f, Bolt, V(6, 52), V(10, 46), V(6, 42), V(10, 36));
            b.Line(1f, Bolt, V(58, 52), V(54, 46), V(58, 42), V(54, 36));
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(29f, 20f, 8.5f, 8f);
            // 虎じまの腰巻。黄色の帯に黒い縞を斜めに並べる
            b.Rect(18, 6, 28, 7, MoonYellow);
            for (int x = 20; x < 45; x += 5)
            {
                b.Polygon(V(x + 2f, 9.5f), tigerDark, V(x, 6), V(x + 2f, 6), V(x + 4f, 13), V(x + 2f, 13));
            }
            b.Shaded(31f, 37f, 11f, 9.5f, body);
            b.Triangle(V(20, 42), V(25, 44), V(17, 54), horn);
            b.Triangle(V(37, 44), V(42, 42), V(45, 54), horn);
            b.White(28f, 36f, 8f, 6.5f);
            // 片方のヒレは へそを狙うように前へ突き出す
            b.Flipper(V(18, 24), V(6, 22), 3.5f, bodyShade);
            b.Flipper(V(46, 24), V(56, 30), 3.5f, bodyShade);
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f, open: true);
        }

        /// <summary>No.240 セントエルモペン: 嵐の夜にだけ姿を見せる 不思議な子。頭とヒレの先に青白い炎を灯し、ふわりと浮かんで 船乗りを導く</summary>
        private static void SaintElmo(PenguinBrush b)
        {
            var body = Rgb(170, 160, 220);
            var bodyShade = PenguinPalette.Shade(body);
            // 浮いているので足は描かず、下に影だけ落とす
            b.Ellipse(32f, 3f, 9f, 1.5f, SnowShade);
            b.Shaded(32f, 19f, 10f, 9f, body);
            b.Belly(30f, 18f, 6.5f, 6f);
            b.Triangle(V(26, 12), V(38, 12), V(32, 6), bodyShade);
            b.Shaded(32f, 32f, 9f, 8f, body);
            b.White(30f, 31f, 6.5f, 5f);
            b.Flipper2(V(23, 22), V(12, 30), 2.5f, bodyShade);
            // 頭とヒレの先の炎。外を青白く、内を緑に光らせる
            foreach (var p in new[] { V(32, 44), V(11, 33), V(53, 33) })
            {
                b.Triangle(V(p.x - 3f, p.y - 2f), V(p.x + 3f, p.y - 2f), V(p.x, p.y + 6f), ElmoFlame);
                b.Ellipse(p.x, p.y - 1f, 3f, 2.5f, ElmoFlame);
                b.Ellipse(p.x, p.y - 1f, 1.5f, 1.5f, AuroraGreen);
            }
            b.TurnedFace(30f, 33f, FaceLeft, EyeStyle.Cross, eyeGap: 6f);
            b.Sparkle(18, 48, AuroraPink);
            b.Sparkle(48, 50, AuroraGreen);
            b.Sparkle(8, 14, ElmoFlame);
            b.Sparkle(56, 14, ElmoFlame);
        }
    }
}
