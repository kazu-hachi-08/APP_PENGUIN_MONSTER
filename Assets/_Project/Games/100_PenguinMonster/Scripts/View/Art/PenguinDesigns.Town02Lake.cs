using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>湖のほとりと道場・町の子（No.197〜210）。月・はんてん・霜柱・湖の氷・白鳥・お守り・蜃気楼</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 MoonYellow = Rgb(250, 235, 160);
        private static readonly Color32 ThinIce = Rgb(215, 240, 250);

        /// <summary>No.197 ツキアカリペン: 夜ふかしの ものしずかな子。頭に三日月をのせ、雪原をほのかに照らしながら ぼんやり座っている</summary>
        private static void Tsukiakari(PenguinBrush b)
        {
            var body = Rgb(165, 160, 215);
            var bodyShade = PenguinPalette.Shade(body);
            b.Foot(28f, 3f, 0f, 0.55f);
            b.Foot(36f, 3f, 0f, 0.55f);
            b.Shaded(32f, 12f, 8.5f, 8f, body);
            b.Belly(30f, 11f, 5.5f, 5.5f);
            b.Shaded(32f, 23f, 8f, 7.5f, body);
            // 三日月は円を塗ってから少しずらした円で抜く
            b.Ellipse(34f, 35f, 5f, 5f, MoonYellow);
            b.Erase(36.5f, 36.5f, 4f, 4f);
            b.White(30f, 22.5f, 6f, 4.5f);
            b.Flipper(V(25, 13), V(19, 8), 2f, bodyShade);
            b.Flipper(V(39, 13), V(45, 8), 2f, bodyShade);
            b.TurnedFace(30f, 24f, FaceLeft, EyeStyle.Sleepy, eyeGap: 5f);
            b.Sparkle(12, 30, MoonYellow);
            b.Sparkle(52, 22, MoonYellow);
            b.Sparkle(48, 44, IceLight);
        }

        /// <summary>No.198 オボロヅキペン: 春の夜の夢のような ふしぎな子。背中に かすんだ満月の光の輪を背負い、霧の衣をたなびかせて浮かぶ</summary>
        private static void Oborozuki(PenguinBrush b)
        {
            var body = Rgb(95, 90, 150);
            var bodyShade = PenguinPalette.Shade(body);
            var haze = Rgb(200, 195, 230);
            b.Ellipse(32f, 40f, 22f, 22f, haze);
            b.Ellipse(32f, 40f, 19f, 19f, MoonYellow);
            b.Ellipse(32f, 40f, 17f, 17f, Rgb(255, 245, 200));
            b.Ellipse(10f, 8f, 10f, 4f, haze);
            b.Ellipse(54f, 10f, 9f, 3.5f, haze);
            b.Shaded(32f, 19f, 13f, 14f, body);
            b.Belly(30f, 18f, 8.5f, 10f);
            b.Shaded(32f, 37f, 11f, 10f, body);
            b.White(29f, 36f, 8.5f, 6.5f);
            b.Flipper(V(20, 24), V(8, 30), 3f, bodyShade);
            b.Flipper(V(44, 24), V(56, 30), 3f, bodyShade);
            b.Line(1f, haze, V(6, 32), V(14, 28), V(20, 20));
            b.Line(1f, haze, V(58, 32), V(50, 28), V(44, 20));
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
            b.Ellipse(32f, 5f, 8f, 3f, haze);
        }

        /// <summary>No.199 ハンテンペン: 外で遊ぶのが好きな 元気っ子。おばあちゃんの縫った赤いはんてんを着て、まんまるに着ぶくれている</summary>
        private static void Hanten(PenguinBrush b)
        {
            var body = Rgb(70, 70, 90);
            var bodyShade = PenguinPalette.Shade(body);
            var coat = Rgb(200, 60, 60);
            var coatShade = PenguinPalette.Shade(coat);
            var pattern = Rgb(240, 200, 120);
            b.Foot(27f, 3f, 0f, 0.6f);
            b.Foot(37f, 3f, 0f, 0.6f);
            b.Shaded(32f, 13f, 11f, 9.5f, coat);
            b.Belly(31f, 12f, 4f, 7f);
            if (b.Front)
            {
                b.Line(0.6f, coatShade, V(27, 20), V(28, 5));
                b.Line(0.6f, coatShade, V(35, 20), V(34, 5));
            }
            foreach (var p in new[] { V(24, 16), V(40, 14), V(23, 9), V(41, 8) })
            {
                b.Dot((int)p.x, (int)p.y, pattern);
            }
            b.Flipper(V(22, 15), V(15, 18), 2.5f, coatShade);
            b.Flipper(V(42, 15), V(49, 18), 2.5f, coatShade);
            b.Shaded(32f, 26f, 7.5f, 7f, body);
            b.White(30f, 25.5f, 5.5f, 4.5f);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Smile, eyeGap: 5f, open: true);
        }

        /// <summary>No.200 ドテラペン: こたつから出たくない どっしり親分。分厚い綿入れのどてらを何枚も重ね着して、袖に両ヒレを突っ込んで構える</summary>
        private static void Dotera(PenguinBrush b)
        {
            var body = Rgb(60, 60, 80);
            var coat = Rgb(60, 75, 130);
            var coatShade = PenguinPalette.Shade(coat);
            var collar = Rgb(120, 60, 40);
            var pattern = Rgb(200, 210, 235);
            b.Foot(38f, 3f);
            b.Foot(26f, 3f);
            b.Shaded(32f, 19f, 18f, 16f, coat);
            if (b.Front)
            {
                b.Polygon(V(32, 22), collar, V(26, 34), V(38, 34), V(34, 6), V(30, 6));
                b.Belly(32f, 22f, 3f, 10f);
            }
            foreach (var p in new[] { V(20, 26), V(44, 26), V(18, 14), V(46, 14), V(24, 8), V(40, 8) })
            {
                b.Line(0.6f, pattern, V(p.x - 2, p.y), V(p.x + 2, p.y));
                b.Line(0.6f, pattern, V(p.x, p.y - 2), V(p.x, p.y + 2));
            }
            // 袖に手を入れた形は、胸の前に横長の袖を重ねて描く
            b.Shaded(32f, 20f, 15f, 5f, coat);
            b.Line(0.7f, coatShade, V(32, 16), V(32, 24));
            b.Shaded(32f, 41f, 11f, 9.5f, body);
            b.White(29f, 40f, 8.5f, 6.5f);
            b.TurnedFace(29f, 41f, FaceLeft, EyeStyle.Sleepy, eyeGap: 7f);
        }

        /// <summary>No.201 シモバシラペン: 地面を押し上げる 力持ちの小さな子。足もとの霜柱で ぐんと背伸びし、頭に土のかたまりを のせている</summary>
        private static void Shimobashira(PenguinBrush b)
        {
            var body = Rgb(185, 215, 240);
            var bodyShade = PenguinPalette.Shade(body);
            var soil = Rgb(120, 90, 65);
            b.Rect(14, 0, 36, 2, soil);
            for (int x = 20; x <= 42; x += 4)
            {
                b.Rect(x, 2, 2, 7, IceLight);
                b.Rect(x + 1, 2, 1, 7, IceShade);
            }
            b.Foot(29f, 10f, 0f, 0.55f);
            b.Foot(36f, 10f, 0f, 0.55f);
            b.Shaded(32f, 18f, 7.5f, 7f, body);
            b.Belly(30f, 17f, 4.5f, 4.5f);
            b.Shaded(32f, 28f, 7.5f, 7f, body);
            b.Shaded(32f, 36f, 7f, 3f, soil);
            b.Rect(28, 36, 2, 2, Rgb(90, 160, 80));
            b.White(30f, 27.5f, 5.5f, 4.5f);
            b.Flipper(V(25, 20), V(17, 26), 2f, bodyShade);
            b.Flipper(V(39, 20), V(47, 26), 2f, bodyShade);
            b.TurnedFace(30f, 29f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f);
        }

        /// <summary>No.202 ヒョウザンペン: 海に浮かぶ ゆるがない巨体。体の大半が とがった氷山で、見えているのは ほんの一部だと胸を張る</summary>
        private static void Hyouzan(PenguinBrush b)
        {
            var body = Rgb(150, 200, 235);
            var bodyShade = PenguinPalette.Shade(body);
            b.Polygon(V(32, 20), IceShade, V(4, 4), V(10, 26), V(18, 30), V(22, 46), V(42, 46), V(46, 30), V(56, 24), V(60, 4));
            b.Polygon(V(28, 22), IceLight, V(10, 6), V(14, 24), V(22, 28), V(26, 42), V(32, 40), V(30, 6));
            b.Shaded(32f, 34f, 11f, 10f, body);
            b.White(29f, 33f, 8.5f, 6.5f);
            // とがった氷は肩の外側に立てる（頭のすぐ上だと耳に見えるので）
            b.Triangle(V(8, 24), V(16, 28), V(9, 40), IceLight);
            b.Triangle(V(48, 28), V(56, 24), V(55, 42), IceShade);
            b.Flipper(V(20, 30), V(8, 34), 3f, bodyShade);
            b.Flipper(V(44, 30), V(56, 34), 3f, bodyShade);
            b.TurnedFace(29f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Rect(0, 0, 64, 4, Water);
            b.Line(0.7f, DeepWater, V(0, 3), V(64, 3));
        }

        /// <summary>No.203 ハスハペン: ぷかぷか浮かぶ おっとり屋。蓮の葉のような丸い氷を まわりに浮かべ、自分も氷の皿にのって漂う</summary>
        private static void Hasuha(PenguinBrush b)
        {
            var body = Rgb(130, 190, 215);
            var bodyShade = PenguinPalette.Shade(body);
            b.Ellipse(32f, 5f, 26f, 4f, Water);
            b.Shaded(10f, 6f, 6f, 2.5f, ThinIce);
            b.Shaded(54f, 7f, 5f, 2f, ThinIce);
            b.Shaded(32f, 6f, 13f, 3.5f, ThinIce);
            b.Shaded(32f, 15f, 8.5f, 7f, body);
            b.Belly(30f, 14f, 5.5f, 4.5f);
            b.Shaded(32f, 25f, 8f, 7f, body);
            b.White(30f, 24.5f, 6f, 4.5f);
            b.Flipper(V(24, 15), V(17, 12), 2f, bodyShade);
            b.Flipper(V(40, 15), V(47, 12), 2f, bodyShade);
            b.TurnedFace(30f, 26f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
        }

        /// <summary>No.204 オミワタリペン: 凍った湖を渡る 湖の守り神。背中に盛り上がった氷の筋が走り、しめ縄を締めて おごそかに歩む</summary>
        private static void Omiwatari(PenguinBrush b)
        {
            var body = Rgb(90, 150, 200);
            var bodyShade = PenguinPalette.Shade(body);
            var rope = Rgb(220, 195, 130);
            var paper = Rgb(250, 250, 250);
            b.Ellipse(32f, 3f, 30f, 3f, ThinIce);
            // 氷の筋は背中から頭の後ろへ、とがった氷を一列に並べて盛り上がりを見せる
            for (int i = 0; i < 5; i++)
            {
                float x = 12f + i * 10f;
                float top = 44f + (i == 2 ? 12f : i % 2 == 0 ? 2f : 7f);
                b.Triangle(V(x - 5, 30), V(x + 5, 30), V(x, top), i % 2 == 0 ? IceLight : IceShade);
            }
            b.Foot(40f, 3f);
            b.Foot(24f, 3f);
            b.Shaded(32f, 19f, 15f, 15f, body);
            b.Belly(30f, 18f, 9.5f, 11f);
            b.Line(1.5f, rope, V(17, 24), V(32, 21), V(47, 24));
            if (b.Front)
            {
                b.Rect(25, 15, 3, 5, paper);
                b.Rect(36, 15, 3, 5, paper);
            }
            b.Shaded(32f, 37f, 11.5f, 10f, body);
            b.White(29f, 36f, 8.5f, 6.5f);
            b.Flipper(V(18, 22), V(8, 14), 3f, bodyShade);
            b.Flipper(V(46, 22), V(56, 14), 3f, bodyShade);
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
        }

        /// <summary>No.205 ウスライペン: 割れそうで割れない しんの強い子。透きとおる薄氷の体で、片足を上げて氷の上をすうっと滑る</summary>
        private static void Usurai(PenguinBrush b)
        {
            var body = Rgb(200, 230, 245);
            var bodyShade = PenguinPalette.Shade(body);
            b.Ellipse(32f, 3f, 24f, 2.5f, ThinIce);
            b.Line(0.5f, IceShade, V(12, 3), V(18, 2), V(22, 4));
            b.Line(0.6f, IceLight, V(40, 4), V(58, 4));
            b.Foot(30f, 6f, 10f, 0.6f);
            b.Foot(44f, 12f, -50f, 0.6f);
            b.ShadedTilted(32f, 15f, 7.5f, 8f, -10f, body);
            b.BellyTilted(30f, 14f, 4.5f, 5.5f, -10f);
            b.Shaded(31f, 26f, 7.5f, 7f, body);
            b.Line(0.5f, IceShade, V(34, 31), V(36, 28), V(35, 25));
            b.White(29f, 25.5f, 5.5f, 4.5f);
            b.Flipper(V(25, 18), V(14, 22), 2f, bodyShade);
            b.Flipper(V(38, 18), V(50, 22), 2f, bodyShade);
            b.TurnedFace(29f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
            b.Sparkle(10, 20, IceLight);
            b.Sparkle(54, 30, IceLight);
        }

        /// <summary>No.206 ヒョウキョウペン: 凍った湖の女王。鏡のように磨いた氷の円盤を背に掲げ、つま先立ちで くるりと回って冷気を放つ</summary>
        private static void Hyoukyou(PenguinBrush b)
        {
            var body = Rgb(120, 180, 225);
            var bodyShade = PenguinPalette.Shade(body);
            var mirror = Rgb(235, 248, 255);
            b.Ellipse(32f, 40f, 22f, 22f, IceShade);
            b.Ellipse(31f, 41f, 19f, 19f, mirror);
            b.Line(1.5f, IceLight, V(18, 52), V(26, 58));
            b.Line(1f, IceLight, V(14, 46), V(18, 50));
            b.Foot(32f, 3f, 80f, 0.8f);
            b.ShadedTilted(32f, 17f, 10f, 13f, -5f, body);
            b.BellyTilted(30f, 16f, 6.5f, 9f, -5f);
            b.Shaded(31f, 34f, 10f, 9.5f, body);
            b.White(28f, 33f, 7.5f, 6.5f);
            // 冠は氷の結晶を3本、頭のてっぺんに立てる
            b.Triangle(V(27, 41), V(30, 41), V(28, 48), IceLight);
            b.Triangle(V(30, 42), V(34, 42), V(32, 51), IceLight);
            b.Triangle(V(34, 41), V(37, 41), V(36, 48), IceLight);
            b.Flipper(V(22, 26), V(6, 30), 2.5f, bodyShade);
            b.Flipper(V(41, 26), V(58, 30), 2.5f, bodyShade);
            b.TurnedFace(28f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            Snowflake(b, 6f, 12f, 3.5f, IceLight);
            Snowflake(b, 58f, 12f, 3.5f, IceLight);
        }

        /// <summary>No.207 コハクチョウペン: 遠い北から来た はずかしがり屋。長い首を くにゃりと曲げ、黄色いくちばしの先を雪に うずめている</summary>
        private static void Kohakuchou(PenguinBrush b)
        {
            var body = Rgb(245, 245, 248);
            var bodyShade = PenguinPalette.Shade(body);
            b.Ellipse(32f, 3f, 18f, 2.5f, Water);
            b.Foot(30f, 4f, 0f, 0.55f);
            b.Foot(38f, 4f, 0f, 0.55f);
            b.Triangle(V(40, 12), V(50, 18), V(48, 10), bodyShade);
            b.Shaded(34f, 13f, 10f, 7.5f, body);
            b.Line(3f, body, V(28, 16), V(26, 24));
            b.Shaded(27f, 28f, 6.5f, 6f, body);
            b.Flipper(V(38, 15), V(46, 20), 2f, bodyShade);
            b.TurnedFace(25f, 29f, FaceLeft, EyeStyle.Worried, eyeGap: 5f);
            b.Sparkle(12, 18, IceLight);
            b.Sparkle(54, 30, IceLight);
        }

        /// <summary>No.208 オオハクチョウペン: 湖に冬を告げる 気高い旅鳥。純白の大きな翼を広げ、長い首を まっすぐ伸ばして舞い降りる</summary>
        private static void Oohakuchou(PenguinBrush b)
        {
            var body = Rgb(245, 245, 248);
            var bodyShade = PenguinPalette.Shade(body);
            var tip = Rgb(200, 215, 230);
            Wing(b, V(22, 26), new[] { V(14, 30), V(2, 46), V(4, 58), V(14, 48), V(22, 36) }, body, bodyShade, tip);
            Wing(b, V(42, 26), new[] { V(50, 30), V(62, 46), V(60, 58), V(50, 48), V(42, 36) }, body, bodyShade, tip);
            b.Foot(38f, 3f);
            b.Foot(26f, 3f);
            b.Shaded(32f, 17f, 13f, 13f, body);
            b.Line(4f, body, V(30, 26), V(28, 38));
            b.Shaded(28f, 42f, 8.5f, 7.5f, body);
            b.TurnedFace(26f, 43f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            Snowflake(b, 8f, 14f, 3f, IceLight);
            Snowflake(b, 56f, 16f, 3f, IceLight);
        }

        /// <summary>No.209 オマモリペン: みんなの無事を願う 律儀な子。赤い錦のお守り袋の体で、頭の結び目の紐を ぴんと立てている</summary>
        private static void Omamori(PenguinBrush b)
        {
            var cloth = Rgb(200, 45, 60);
            var clothShade = PenguinPalette.Shade(cloth);
            var cord = Rgb(240, 200, 60);
            b.Foot(26f, 3f, 0f, 0.7f);
            b.Foot(38f, 3f, 0f, 0.7f);
            // お守り袋は角の丸い縦長の長方形。上の角を斜めに落として袋の形にする
            b.Rect(18, 4, 28, 30, clothShade);
            b.Rect(19, 5, 25, 28, cloth);
            b.Triangle(V(18, 34), V(23, 34), V(18, 29), PenguinPalette.Clear);
            b.Triangle(V(46, 34), V(41, 34), V(46, 29), PenguinPalette.Clear);
            b.Rect(21, 7, 21, 1, cord);
            b.Rect(21, 30, 21, 1, cord);
            if (b.Front)
            {
                b.Rect(28, 9, 8, 9, Rgb(250, 245, 230));
                b.Line(0.5f, clothShade, V(30, 16), V(34, 16));
                b.Line(0.5f, clothShade, V(32, 16), V(32, 11));
                b.Line(0.5f, clothShade, V(30, 13), V(34, 13));
            }
            b.Line(1f, cord, V(32, 34), V(28, 40), V(32, 38), V(36, 40), V(32, 34));
            b.Line(1f, cord, V(32, 34), V(30, 46));
            b.Line(1f, cord, V(32, 34), V(35, 46));
            b.Flipper(V(18, 20), V(10, 14), 2.5f, clothShade);
            b.Flipper(V(46, 20), V(54, 14), 2.5f, clothShade);
            b.White(31f, 25f, 7f, 4.5f);
            b.Face(31f, 26f, 3.5f, EyeStyle.Dot);
        }

        /// <summary>No.210 シンキロウペン: 見えるのに つかめない ふしぎな子。凍った湖にゆらめく幻で、体がずれて二重に見え、まわりに虹色の光の帯がたゆたう</summary>
        private static void Shinkirou(PenguinBrush b)
        {
            var body = Rgb(70, 60, 110);
            var ghost = Rgb(170, 150, 220);
            b.Ellipse(32f, 3f, 24f, 2.5f, ThinIce);
            // 少し右にずらした薄い色の体を先に描き、幻の二重写しに見せる
            b.Ellipse(38f, 17f, 10f, 11f, ghost);
            b.Ellipse(38f, 33f, 9f, 8.5f, ghost);
            Ribbon(b, AuroraGreen, V(4, 46), V(16, 52), V(28, 46), V(40, 52), V(52, 46), V(62, 50));
            Ribbon(b, AuroraPink, V(2, 40), V(14, 44), V(26, 40), V(38, 44), V(50, 40), V(62, 44));
            b.Foot(36f, 4f, 0f, 0.8f);
            b.Foot(26f, 4f, 0f, 0.8f);
            b.Shaded(31f, 17f, 10f, 11f, body);
            b.Belly(29f, 16f, 6.5f, 7.5f);
            b.Shaded(31f, 33f, 9f, 8.5f, body);
            b.White(28f, 32f, 6.5f, 5.5f);
            b.Line(0.6f, ghost, V(18, 26), V(44, 26));
            b.Line(0.6f, ghost, V(18, 12), V(46, 12));
            b.Flipper(V(22, 20), V(12, 26), 2.5f, PenguinPalette.Shade(body));
            b.TurnedFace(28f, 33f, FaceLeft, EyeStyle.Cross, eyeGap: 6f);
            b.Sparkle(54, 20, AuroraGreen);
            b.Sparkle(8, 18, AuroraPink);
        }
    }
}
