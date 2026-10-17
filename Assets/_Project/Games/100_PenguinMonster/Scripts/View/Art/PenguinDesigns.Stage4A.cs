using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>段階4のふつう前半（No.76〜100）のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 S4aWood = Rgb(150, 100, 60);

        /// <summary>No.76 スノードームペン: 夢見がちでおっとり。ガラスのドームの中にちょこんと座る。丸い大きなドームと台形の木の台座のシルエット</summary>
        private static void SnowDome(PenguinBrush b)
        {
            var glassEdge = Rgb(120, 190, 225);
            var glass = Rgb(214, 238, 248);
            var body = Rgb(110, 160, 220);
            var bodyShade = PenguinPalette.Shade(body);
            // 中は淡く、縁だけ濃くして、透けたガラスの球に見せる
            b.Ellipse(32f, 34f, 23f, 23f, glassEdge);
            b.Ellipse(32f, 34f, 21f, 21f, glass);
            b.Ellipse(32f, 15f, 17f, 4f, SnowWhite);
            b.Shaded(33f, 21f, 7.5f, 6.5f, body);
            b.Belly(31f, 20f, 5f, 4.5f);
            b.Foot(27f, 16f, 0f, 0.6f);
            b.Foot(37f, 16f, 0f, 0.6f);
            b.Flipper(V(39, 22), V(43, 18), 2f, bodyShade);
            b.Shaded(32f, 31f, 7.5f, 7f, body);
            b.White(30f, 30f, 5.5f, 4.5f);
            b.TurnedFace(30f, 31f, FaceLeft, EyeStyle.Smile, eyeGap: 5f);
            // 舞う雪の粒。ドームの中だけに散らして、外の世界と分ける
            foreach (var p in new[] { V(18, 40), V(24, 48), V(36, 50), V(45, 42), V(47, 30), V(17, 27), V(30, 44), V(42, 24) })
            {
                b.Rect((int)p.x, (int)p.y, 2, 2, SnowWhite);
            }
            // ガラスの光の弧。左上に入れて球の丸みを出す
            b.Line(0.8f, PenguinPalette.Belly, V(14, 38), V(16, 45), V(20, 50), V(25, 53));
            // 台座は台形。上の細い縁と下の広い板で、置物らしさを出す
            b.Polygon(V(32, 7), PenguinPalette.Shade(S4aWood), V(14, 2), V(50, 2), V(46, 13), V(18, 13));
            b.Polygon(V(31, 8), S4aWood, V(16, 4), V(47, 4), V(44, 12), V(19, 12));
            b.Line(0.6f, PenguinPalette.Shade(S4aWood), V(18, 8), V(45, 8));
        }

        /// <summary>No.77 カキゴオリペン: 甘えん坊で涼しげ。脚付きのガラスの器から体が山盛りのかき氷になって生える。てっぺんの3色シロップと斜めのスプーンで、とんがり山のシルエット</summary>
        private static void Kakigori(PenguinBrush b)
        {
            var ice = Rgb(188, 222, 246);
            var cup = Rgb(170, 210, 240);
            var cupShade = PenguinPalette.Shade(cup);
            var spoon = Rgb(200, 205, 215);
            // スプーンは山に斜めに刺して、器だけの左右対称な形を崩す
            b.Line(1f, spoon, V(40, 40), V(55, 58));
            b.Ellipse(55f, 58f, 1.5f, 1.5f, spoon);
            b.Ellipse(32f, 3f, 11f, 2f, cupShade);
            b.Rect(30, 4, 5, 6, cupShade);
            // 氷の山。丸いすそと三角の頂上で、山盛りに見せる
            b.Polygon(V(32, 30), PenguinPalette.Shade(ice), V(12, 18), V(52, 18), V(52, 28), V(43, 42), V(32, 57), V(20, 42), V(12, 28));
            b.Polygon(V(31, 31), ice, V(13, 20), V(50, 20), V(50, 28), V(41, 42), V(31, 55), V(20, 42), V(13, 28));
            // シロップは頂上から3方向へ垂らし、色で味の違いを見せる
            b.Polygon(V(25, 44), Rgb(230, 60, 70), V(23, 51), V(29, 53), V(26, 44), V(22, 38), V(19, 42));
            b.Polygon(V(31, 47), Rgb(90, 200, 90), V(28, 54), V(33, 54), V(34, 44), V(31, 38), V(29, 45));
            b.Polygon(V(37, 46), Rgb(250, 220, 80), V(32, 55), V(36, 52), V(41, 42), V(40, 36), V(37, 44));
            b.Flipper(V(14, 24), V(8, 30), 2.5f, ice);
            b.Flipper(V(50, 24), V(56, 30), 2.5f, ice);
            b.White(28f, 28f, 10f, 7.5f);
            b.TurnedFace(28f, 29f, FaceLeft, EyeStyle.Round, eyeGap: 7f);
            // 器は氷より手前。縁を少しかぶせて、氷が器から盛り上がっているのを見せる
            b.Polygon(V(32, 14), cupShade, V(10, 20), V(54, 20), V(46, 9), V(18, 9));
            b.Polygon(V(31, 15), cup, V(12, 19), V(51, 19), V(45, 11), V(19, 11));
            b.Line(0.6f, PenguinPalette.Belly, V(16, 17), V(20, 12));
        }

        /// <summary>No.78 フィギュアペン: 優雅で自信家。片足で立ち、もう片足を後ろへ高く上げるアラベスク。前へ傾けた体と両ヒレ・後ろ足で大きく開いた星形のシルエット</summary>
        private static void Figure(PenguinBrush b)
        {
            var body = Rgb(130, 200, 235);
            var bodyShade = PenguinPalette.Shade(body);
            var tutu = Rgb(250, 190, 210);
            var blade = Rgb(210, 216, 228);
            // 後ろ足は高く上げ、体と一直線にして伸びやかに見せる
            b.Line(2f, bodyShade, V(40, 22), V(55, 34));
            b.Foot(57f, 36f, 40f, 0.8f);
            b.Line(2f, bodyShade, V(32, 16), V(31, 8));
            b.Foot(29f, 6f, 0f, 0.8f);
            // 銀のブレード。氷の上に立っているのを細い線だけで見せる
            b.Line(0.6f, blade, V(21, 2), V(38, 2));
            b.Line(0.5f, blade, V(26, 3), V(26, 4));
            b.Line(0.5f, blade, V(33, 3), V(33, 4));
            b.Flipper(V(36, 34), V(50, 48), 2.5f, bodyShade);
            b.ShadedTilted(32f, 27f, 8f, 13f, 40f, body);
            b.BellyTilted(29f, 26f, 5f, 10f, 40f);
            // チュチュは腰に平たく広げ、体の傾きに合わせる
            b.Tilted(35f, 22f, 12f, 3.5f, 40f, PenguinPalette.Shade(tutu));
            b.Tilted(34f, 23f, 11f, 2.5f, 40f, tutu);
            b.Shaded(22f, 42f, 8f, 7.5f, body);
            b.White(20f, 41f, 5.5f, 5f);
            b.Flipper(V(26, 34), V(10, 32), 2.5f, bodyShade);
            b.TurnedFace(20f, 42f, FaceLeft, EyeStyle.Line, eyeGap: 5.5f);
            b.Sparkle(8, 50, IceLight);
            b.Sparkle(50, 10, IceLight);
        }

        /// <summary>No.79 ヒゲペン: 向こう見ずの突撃屋。頭を低くして前へ突進する。地をはう水平の体と、くちばしの下に垂れるつららのヒゲ</summary>
        private static void Hige(PenguinBrush b)
        {
            var body = Rgb(110, 130, 160);
            var bodyShade = PenguinPalette.Shade(body);
            // 後ろの足は蹴り出しで伸ばし、勢いを出す
            b.Foot(55f, 5f, 40f, 0.9f);
            b.Foot(36f, 4f, -10f, 0.9f);
            b.Flipper(V(44, 24), V(60, 30), 2.5f, bodyShade);
            b.ShadedTilted(38f, 17f, 9.5f, 17f, 70f, body);
            // おなかは地面側に寄せ、低く伏せた体勢に見せる
            b.BellyTilted(36f, 13f, 5f, 12f, 70f);
            b.Shaded(18f, 22f, 10f, 9.5f, body);
            b.White(15f, 21f, 8f, 7f);
            b.TurnedFace(15f, 23f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            // あごの下の黒いひもはヒゲペンギンの印。顔の白の中を横切らせる
            if (b.Front)
            {
                b.Line(0.5f, PenguinPalette.Eye, V(8, 17), V(14, 15), V(21, 17));
            }
            // つららのヒゲ。くちばしの下から何本も垂らし、突撃の向きを強める
            b.Triangle(V(6, 16), V(9, 16), V(7, 9), IceLight);
            b.Triangle(V(10, 14), V(13, 14), V(11, 6), IceShade);
            b.Triangle(V(14, 13), V(17, 13), V(15, 7), IceLight);
            b.Flipper(V(30, 22), V(28, 12), 2.5f, bodyShade);
            b.Line(0.6f, IceLight, V(54, 40), V(62, 40));
            b.Line(0.6f, IceLight, V(48, 35), V(58, 35));
        }

        /// <summary>No.80 イグルーペン: のんびり屋のマイペース。ヤドカリのように背中にイグルーを背負い、前の入口から頭と足だけ出す。大きなドームのシルエット</summary>
        private static void Igloo(PenguinBrush b)
        {
            var block = Rgb(225, 238, 250);
            var blockShade = Rgb(160, 190, 220);
            var navy = Rgb(50, 66, 120);
            b.Foot(14f, 4f, -10f, 0.9f);
            b.Foot(26f, 4f, 0f, 0.9f);
            // 背負っているので少し地面から浮かせ、足との間にすき間を作る
            b.Ellipse(40f, 7f, 23f, 34f, blockShade);
            b.Ellipse(39f, 8f, 21.5f, 32.5f, block);
            b.Rect(0, 0, 64, 7, PenguinPalette.Clear);
            // 氷ブロックの目地。横の段と、段ごとにずらした縦の線でれんがのように見せる
            b.Line(0.5f, blockShade, V(18, 17), V(61, 17));
            b.Line(0.5f, blockShade, V(19, 27), V(60, 27));
            b.Line(0.5f, blockShade, V(24, 35), V(55, 35));
            foreach (var p in new[] { V(40, 7), V(52, 7), V(34, 17), V(46, 17), V(56, 17), V(40, 27), V(52, 27), V(32, 35), V(46, 35) })
            {
                b.Line(0.5f, blockShade, p, V(p.x, p.y + (p.y < 30 ? 9 : 5)));
            }
            // 前のアーチの入口。暗い穴から頭が出て、背負っているのがわかる
            b.Ellipse(22f, 10f, 10f, 14f, blockShade);
            b.Ellipse(22f, 10f, 8f, 12f, Rgb(40, 44, 70));
            b.Rect(10, 0, 24, 7, PenguinPalette.Clear);
            b.Foot(14f, 4f, -10f, 0.9f);
            b.Foot(26f, 4f, 0f, 0.9f);
            b.Shaded(18f, 16f, 9f, 8.5f, navy);
            b.White(15f, 15f, 7f, 6f);
            b.TurnedFace(15f, 16f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
        }

        /// <summary>No.81 ユキオンナペン: 物静かでひんやり。足が無く、白い着物のすそが細く消えて宙に浮く。長い袖と後ろへなびく黒紫の髪の羽で、縦に流れるシルエット</summary>
        private static void Yukionna(PenguinBrush b)
        {
            var kimono = Rgb(238, 240, 250);
            var kimonoShade = Rgb(190, 190, 220);
            var hair = Rgb(60, 40, 84);
            var obi = Rgb(140, 210, 240);
            // 髪の羽は後ろへ長く流し、風もないのになびく不気味さを出す
            b.Polygon(V(42, 44), PenguinPalette.Shade(hair), V(30, 54), V(46, 52), V(58, 46), V(62, 36), V(58, 24), V(54, 32), V(50, 26), V(46, 34), V(36, 36));
            b.Polygon(V(40, 46), hair, V(30, 53), V(44, 51), V(55, 45), V(58, 37), V(53, 37), V(48, 33), V(44, 38), V(36, 38));
            // すそは右下へ細く消え、足の無い幽霊のように浮かせる
            b.Polygon(V(32, 22), kimonoShade, V(22, 36), V(42, 36), V(46, 18), V(44, 10), V(52, 2), V(36, 8), V(22, 14), V(18, 22));
            b.Polygon(V(31, 23), kimono, V(23, 35), V(40, 35), V(43, 19), V(41, 12), V(46, 5), V(35, 10), V(24, 15), V(20, 22));
            b.Line(0.5f, kimonoShade, V(30, 34), V(34, 20), V(40, 10));
            // 帯は水色。白一色の着物に区切りを入れて、腰の位置を見せる
            b.Polygon(V(31, 27), obi, V(21, 29), V(43, 29), V(43, 25), V(20, 25));
            // 襟の合わせ。白い着物だと分かるように胸元に重ね目を入れる
            b.Line(0.5f, kimonoShade, V(25, 35), V(31, 30), V(37, 35));
            // 長い袖。手前の袖をだらりと垂らして、うらめしい雰囲気にする
            b.Polygon(V(16, 26), kimonoShade, V(20, 36), V(24, 30), V(20, 14), V(14, 12), V(10, 16), V(13, 30));
            b.Polygon(V(16, 26), kimono, V(19, 34), V(22, 30), V(19, 16), V(14, 14), V(12, 17), V(14, 30));
            b.Shaded(30f, 44f, 10f, 9.5f, hair);
            b.White(27f, 43f, 7.5f, 6.5f);
            b.TurnedFace(27f, 44f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            b.Sparkle(6, 44, IceLight);
            b.Sparkle(10, 56, IceLight);
            b.Sparkle(6, 6, IceLight);
            b.Sparkle(58, 12, IceLight);
            b.Dot(14, 50, IceLight);
        }

        /// <summary>No.82 シロクマペン: いたずら好きの力持ち。シロクマの着ぐるみで両ヒレを上げて「がおー」と脅かす。丸い耳のフードと太い腕で、ずんぐり大きいシルエット</summary>
        private static void Shirokuma(PenguinBrush b)
        {
            var fur = Rgb(246, 242, 228);
            var furShade = Rgb(206, 196, 172);
            var navy = Rgb(50, 66, 120);
            var pad = Rgb(70, 60, 70);
            b.Ellipse(19f, 4f, 7f, 3.5f, furShade);
            b.Ellipse(45f, 4f, 7f, 3.5f, furShade);
            // 腕はクマの手袋。太く上げ、指先の爪で脅かすポーズを強める
            b.Line(4f, furShade, V(16, 26), V(7, 44));
            b.Line(4f, furShade, V(48, 26), V(57, 44));
            b.Ellipse(7f, 46f, 5f, 4.5f, fur);
            b.Ellipse(57f, 46f, 5f, 4.5f, fur);
            for (int i = 0; i < 3; i++)
            {
                b.Dot(4 + i * 3, 51, pad);
                b.Dot(54 + i * 3, 51, pad);
            }
            b.Ellipse(19f, 4f, 6f, 3f, fur);
            b.Ellipse(45f, 4f, 6f, 3f, fur);
            b.Shaded(32f, 19f, 18f, 15f, fur);
            b.Shaded(31f, 39f, 14f, 13f, fur);
            // 丸い耳。クマだとシルエットだけでわかる一番の印
            b.Shaded(20f, 51f, 4.5f, 4.5f, fur);
            b.Shaded(42f, 51f, 4.5f, 4.5f, fur);
            b.Ellipse(20f, 51f, 2f, 2f, furShade);
            b.Ellipse(42f, 51f, 2f, 2f, furShade);
            // フードの鼻づら。額に乗せて、中の顔と重ならないようにする
            b.Ellipse(29f, 47f, 4f, 2.5f, furShade);
            b.Ellipse(28f, 48f, 2f, 1.2f, pad);
            // 着ぐるみの顔の穴から中の紺の体がのぞく
            if (b.Front)
            {
                b.Ellipse(28f, 36f, 10f, 8f, navy);
            }
            b.White(27f, 36f, 8f, 6.5f);
            b.TurnedFace(27f, 37f, FaceLeft, EyeStyle.Round, eyeGap: 6.5f, open: true);
            b.Belly(30f, 17f, 10f, 9f);
        }

        /// <summary>No.83 ユキガッセンペン: 元気な負けず嫌い。片ヒレで雪玉を大きく後ろへ振りかぶる。のけぞった体と高く上げた腕、足元の雪玉の山のシルエット</summary>
        private static void Yukigassen(PenguinBrush b)
        {
            var body = Rgb(70, 120, 210);
            var bodyShade = PenguinPalette.Shade(body);
            var red = Rgb(220, 60, 60);
            // 足元の雪玉の山。投げ続ける気満々なのを見せる
            b.Shaded(8f, 5f, 4.5f, 4f, SnowWhite);
            b.Shaded(17f, 5f, 4.5f, 4f, SnowWhite);
            b.Shaded(12f, 12f, 4.5f, 4f, SnowWhite);
            b.Foot(26f, 4f, -15f);
            b.Foot(44f, 4f, 15f);
            // 振りかぶった腕と雪玉。体の後ろ上へ大きく引き、投げる直前のためを作る
            b.Flipper(V(40, 32), V(53, 46), 3f, bodyShade);
            b.Shaded(55f, 50f, 5f, 5f, SnowWhite);
            b.ShadedTilted(35f, 19f, 11f, 13f, -15f, body);
            b.BellyTilted(32f, 18f, 7.5f, 9.5f, -15f);
            b.Shaded(32f, 37f, 11f, 10.5f, body);
            b.White(29f, 36f, 8f, 7f);
            // マフラーの端を後ろへなびかせ、体をひねった勢いを出す
            b.Polygon(V(32, 28), red, V(21, 30), V(43, 30), V(43, 26), V(21, 26));
            b.Polygon(V(44, 27), PenguinPalette.Shade(red), V(40, 28), V(52, 22), V(54, 18), V(44, 24));
            b.Flipper(V(24, 22), V(12, 28), 2.5f, bodyShade);
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
            // ニット帽。折り返しの白線とぼんぼんで冬の遊び着に見せる
            b.Polygon(V(33, 47), red, V(21, 44), V(43, 44), V(41, 51), V(34, 54), V(25, 52));
            b.Line(1f, SnowWhite, V(21, 44), V(43, 44));
            b.Shaded(36f, 56f, 3f, 3f, SnowWhite);
        }

        /// <summary>No.84 クリオネペン: 天使のようにやさしい（でもおなかが空くと怖い）。下にいくほど細く尾になって浮き、小さな翼のヒレをパタパタさせる。しずく形のシルエット</summary>
        private static void Clione(PenguinBrush b)
        {
            var body = Rgb(196, 230, 248);
            var edge = Rgb(150, 200, 232);
            var core = Rgb(240, 100, 60);
            // しずく形の体。外側を少し濃くして、半透明の体のふちに見せる
            b.Polygon(V(32, 26), edge, V(20, 36), V(44, 36), V(42, 24), V(37, 12), V(34, 4), V(29, 2), V(30, 10), V(23, 22));
            b.Polygon(V(32, 26), body, V(22, 35), V(42, 35), V(40, 24), V(35, 13), V(32, 6), V(31, 12), V(25, 22));
            // 小さな翼のヒレ。左右へ広げ、片方を上げて羽ばたいている途中に見せる
            b.Polygon(V(18, 30), edge, V(22, 31), V(12, 38), V(8, 34), V(21, 27));
            b.Polygon(V(46, 30), edge, V(42, 31), V(54, 26), V(56, 22), V(42, 27));
            b.Shaded(32f, 42f, 11f, 10f, body);
            b.White(29f, 41f, 8f, 6.5f);
            b.TurnedFace(29f, 42f, FaceLeft, EyeStyle.Dot, eyeGap: 6f);
            // 頭の橙の突起。クリオネの印
            b.Ellipse(27f, 52f, 1.5f, 2.5f, core);
            b.Ellipse(36f, 52f, 1.5f, 2.5f, core);
            // おなかのハートの核。透けた体の中で光らせ、見どころにする
            b.Ellipse(30f, 26f, 2.5f, 2.5f, core);
            b.Ellipse(35f, 26f, 2.5f, 2.5f, core);
            b.Triangle(V(27.5f, 25.5f), V(37.5f, 25.5f), V(32.5f, 19f), core);
            b.Dot(29, 27, Rgb(255, 190, 150));
            b.Sparkle(10, 50, IceLight);
            b.Sparkle(54, 46, IceLight);
            b.Dot(48, 8, IceLight);
        }

        /// <summary>No.85 スキーペン: 怖いもの知らずの目立ちたがり。スキージャンプの空中姿勢で体を大きく前に倒す。足の下で V 字に開いた長い2本の板の、斜めに伸びたシルエット</summary>
        private static void Ski(PenguinBrush b)
        {
            var body = Rgb(60, 110, 200);
            var bodyShade = PenguinPalette.Shade(body);
            var ski = Rgb(220, 50, 50);
            var goggle = Rgb(250, 220, 80);
            // 板は後ろでそろえ、先を開いて V 字にする。ジャンプの空中姿勢の一番の印
            b.Line(1.2f, PenguinPalette.Shade(ski), V(56, 14), V(4, 30));
            b.Line(1.2f, ski, V(56, 12), V(8, 4));
            b.Foot(48f, 15f, 15f, 0.8f);
            // 腕は体に沿わせて後ろへ流し、風の抵抗を減らす姿勢にする
            b.Flipper(V(42, 28), V(58, 18), 2.5f, bodyShade);
            b.ShadedTilted(38f, 27f, 9f, 17f, 60f, body);
            b.BellyTilted(35f, 25f, 5.5f, 13f, 60f);
            b.Shaded(20f, 39f, 9.5f, 9f, body);
            b.White(17f, 38f, 7.5f, 6.5f);
            b.TurnedFace(17f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            // ゴーグルの帯は後ろ姿にも回し、レンズは正面だけに描く
            b.Line(1f, Rgb(40, 40, 60), V(12, 42), V(29, 42));
            if (b.Front)
            {
                b.Ellipse(16f, 41f, 6f, 2.5f, PenguinPalette.Shade(goggle));
                b.Ellipse(16f, 41.5f, 5f, 1.8f, goggle);
                b.Dot(13, 42, PenguinPalette.EyeLight);
            }
            b.Line(0.6f, SnowWhite, V(60, 40), V(52, 44));
            b.Line(0.6f, SnowWhite, V(62, 32), V(56, 35));
        }

        /// <summary>No.86 ジェンツーペン: せっかちなスピード自慢。魚雷のように体を水平にのばして泳ぐ。横に細長い流線形と、後ろへ流れる足と泡のシルエット</summary>
        private static void Gentoo(PenguinBrush b)
        {
            var body = Rgb(60, 64, 80);
            var bodyShade = PenguinPalette.Shade(body);
            var foot = Rgb(245, 100, 50);
            var bubble = Rgb(190, 230, 250);
            // 泡は後ろへ散らし、速さの跡にする
            b.Ellipse(58f, 40f, 2f, 2f, bubble);
            b.Ellipse(61f, 46f, 1.5f, 1.5f, bubble);
            b.Ellipse(55f, 48f, 1f, 1f, bubble);
            b.Ellipse(60f, 34f, 1f, 1f, bubble);
            // 足はそろえて後ろへのばし、かじにする
            b.Tilted(56f, 24f, 5f, 2f, -10f, PenguinPalette.Shade(foot));
            b.Tilted(56f, 27f, 5f, 2f, 5f, foot);
            b.Flipper(V(36, 26), V(50, 16), 2.5f, bodyShade);
            b.ShadedTilted(34f, 29f, 21f, 9f, -5f, body);
            b.BellyTilted(32f, 26f, 16f, 5f, -5f);
            b.Shaded(16f, 31f, 9f, 8.5f, body);
            b.White(14f, 30f, 7f, 6f);
            // 頭のてっぺんの白い帯はジェンツーペンギンの印。後ろ姿でも見えるよう、白ではなく色で描く
            b.Line(1f, PenguinPalette.Belly, V(12, 38), V(17, 39.5f), V(22, 38));
            b.TurnedFace(14f, 31f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
            b.Flipper(V(30, 30), V(44, 36), 2.5f, bodyShade);
            b.Line(0.6f, bubble, V(44, 44), V(54, 44));
            b.Line(0.6f, bubble, V(48, 14), V(60, 14));
        }

        /// <summary>No.87 ショウボウペン: 正義感の強い頑張り屋。ホースを抱えて足を踏んばり、水の勢いに負けずに少しのけぞる。前へ大きく弧を描いて噴き出す水のシルエット</summary>
        private static void Shoubou(PenguinBrush b)
        {
            var navy = Rgb(44, 56, 110);
            var navyShade = PenguinPalette.Shade(navy);
            var red = Rgb(220, 50, 40);
            var redShade = PenguinPalette.Shade(red);
            var hose = Rgb(140, 140, 150);
            var water = Rgb(120, 200, 245);
            // ホースの後ろ側は背中から地面へ垂らし、長いホースを引いてきたのを見せる
            b.Line(2f, PenguinPalette.Shade(hose), V(48, 22), V(56, 14), V(58, 6), V(63, 3));
            b.Foot(32f, 4f, -20f);
            b.Foot(50f, 4f, 10f);
            b.ShadedTilted(42f, 19f, 11f, 14f, -12f, navy);
            b.BellyTilted(39f, 18f, 7.5f, 10f, -12f);
            // 胸の反射帯。消防の服らしさを1本の黄色で出す
            b.Line(0.8f, Rgb(250, 220, 80), V(32, 11), V(52, 13));
            b.Line(2f, hose, V(36, 24), V(24, 28));
            b.Rect(18, 27, 6, 3, Rgb(200, 200, 210));
            b.Flipper(V(36, 22), V(26, 27), 2.5f, navyShade);
            // 水は前へ大きく弧を描かせ、勢いと届く遠さを見せる
            b.Line(1.6f, PenguinPalette.Shade(water), V(17, 30), V(12, 38), V(7, 42), V(3, 40), V(2, 33));
            b.Line(1f, water, V(17, 30), V(12, 38), V(7, 42), V(3, 40), V(2, 33));
            b.Dot(2, 27, water);
            b.Dot(5, 24, water);
            b.Dot(1, 21, water);
            b.Shaded(41f, 37f, 10.5f, 10f, navy);
            b.White(38f, 36f, 7.5f, 6.5f);
            b.TurnedFace(38f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            // 消防のヘルメット。後ろのつばを長くのばして、ふつうの帽子と見分ける
            b.Ellipse(41f, 46f, 11f, 7f, red);
            b.Erase(41f, 37f, 20f, 6.5f);
            b.Shaded(41f, 37f, 10.5f, 10f, navy);
            b.White(38f, 36f, 7.5f, 6.5f);
            b.TurnedFace(38f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            b.Line(1f, redShade, V(29, 44), V(52, 44), V(58, 40));
            b.Line(0.6f, redShade, V(42, 52), V(44, 45));
            if (b.Front)
            {
                b.Polygon(V(34, 48), Rgb(250, 220, 80), V(31, 50), V(37, 50), V(37, 47), V(34, 45), V(31, 47));
            }
        }

        /// <summary>No.88 スミペン: いたずらっ子。頭にタコを帽子のように乗せ、くちばしから黒いスミの雲をぷーっと吹く。頭の上の丸いタコと前へもくもく広がる雲のシルエット</summary>
        private static void Sumi(PenguinBrush b)
        {
            var navy = Rgb(44, 56, 110);
            var navyShade = PenguinPalette.Shade(navy);
            var tako = Rgb(200, 70, 120);
            var takoShade = PenguinPalette.Shade(tako);
            var ink = Rgb(40, 36, 50);
            b.Foot(30f, 4f, -10f);
            b.Foot(44f, 4f, 10f);
            b.Flipper(V(44, 22), V(52, 12), 2.5f, navyShade);
            b.Shaded(37f, 18f, 11f, 13f, navy);
            b.Belly(34f, 16f, 7.5f, 9.5f);
            b.Shaded(33f, 36f, 10.5f, 10f, navy);
            b.White(30f, 35f, 7.5f, 6.5f);
            // タコの足は顔の左右に垂らし、くるんと巻いた先で帽子らしさとかわいさを出す
            b.Line(1.4f, takoShade, V(42, 42), V(46, 34), V(46, 26), V(43, 24));
            b.Line(1.4f, takoShade, V(38, 42), V(41, 32), V(39, 28));
            b.Line(1.4f, tako, V(24, 43), V(20, 36), V(20, 30), V(23, 28));
            b.Shaded(33f, 49f, 11f, 8f, tako);
            b.Dot(28, 52, Rgb(240, 150, 190));
            b.Dot(33, 54, Rgb(240, 150, 190));
            b.Dot(38, 51, Rgb(240, 150, 190));
            b.TurnedFace(30f, 36f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
            // スミの雲。くちばしの先からもくもく広げ、いたずらの最中に見せる
            if (b.Front)
            {
                b.Ellipse(19f, 32f, 2.5f, 2f, ink);
            }
            b.Ellipse(13f, 33f, 5f, 4.5f, ink);
            b.Ellipse(7f, 28f, 5f, 4.5f, ink);
            b.Ellipse(6f, 38f, 4.5f, 4f, ink);
            b.Ellipse(9f, 26f, 2f, 2f, Rgb(80, 74, 96));
            b.Ellipse(11f, 35f, 1.5f, 1.5f, Rgb(80, 74, 96));
        }

        /// <summary>No.89 クラゲペン: ふわふわの寝ぼすけ。クラゲの傘をかぶって宙に浮き、長い触手を体のまわりに垂らす。上の丸い傘と下へ流れる何本もの線のシルエット</summary>
        private static void Kurage(PenguinBrush b)
        {
            var body = Rgb(196, 176, 230);
            var bodyShade = PenguinPalette.Shade(body);
            var cap = Rgb(250, 170, 210);
            var capShade = PenguinPalette.Shade(cap);
            var tentacle = Rgb(240, 200, 230);
            var spark = Rgb(255, 240, 120);
            // 触手は長さを変えて揺らし、浮いて漂う動きを出す
            b.Line(0.8f, tentacle, V(14, 40), V(10, 30), V(14, 20), V(9, 10), V(12, 2));
            b.Line(0.8f, tentacle, V(22, 38), V(18, 28), V(22, 18), V(18, 8));
            b.Line(0.8f, tentacle, V(44, 38), V(48, 28), V(44, 18), V(49, 8));
            b.Line(0.8f, tentacle, V(52, 40), V(56, 30), V(52, 20), V(57, 10), V(54, 2));
            b.Foot(28f, 9f, -30f, 0.8f);
            b.Foot(37f, 9f, 20f, 0.8f);
            b.Shaded(33f, 20f, 10f, 10f, body);
            b.Belly(31f, 18f, 6.5f, 7f);
            b.Flipper(V(24, 22), V(17, 16), 2.5f, bodyShade);
            b.Shaded(32f, 34f, 10f, 9.5f, body);
            b.White(29f, 33f, 7.5f, 6.5f);
            b.TurnedFace(29f, 34f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            // 傘は半透明に見えるよう、下のふちをほたてのように波打たせる
            b.Ellipse(33f, 42f, 20f, 13f, capShade);
            b.Ellipse(32f, 43f, 18.5f, 11.5f, cap);
            b.Rect(10, 30, 46, 11, PenguinPalette.Clear);
            for (int x = 15; x <= 51; x += 6)
            {
                b.Ellipse(x, 42f, 3f, 2f, cap);
            }
            b.Shaded(32f, 34f, 10f, 9.5f, body);
            b.White(29f, 33f, 7.5f, 6.5f);
            b.TurnedFace(29f, 34f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            b.Line(0.6f, PenguinPalette.Belly, V(20, 48), V(25, 52), V(31, 54));
            // 触手の火花。かみなりタイプなのをしびれる小さな光で見せる
            b.Sparkle(10, 26, spark);
            b.Sparkle(50, 16, spark);
            b.Sparkle(55, 34, spark);
            b.Dot(19, 12, spark);
        }

        /// <summary>No.90 カメノリペン: おっとりした旅好き。大きなウミガメの背中に小さく乗って、のんびり海をゆく。下半分を占める横長の甲羅と、左に出たカメの頭のシルエット</summary>
        private static void Kamenori(PenguinBrush b)
        {
            var shell = Rgb(90, 160, 90);
            var shellShade = PenguinPalette.Shade(shell);
            var plate = Rgb(150, 110, 60);
            var skin = Rgb(160, 190, 140);
            var skinShade = PenguinPalette.Shade(skin);
            var weed = Rgb(60, 130, 70);
            var navy = Rgb(60, 90, 150);
            // カメの頭とヒレは左へ出し、進む向きをはっきりさせる
            b.Flipper(V(50, 10), V(62, 6), 3f, skinShade);
            b.Flipper(V(18, 10), V(4, 2), 3.5f, skinShade);
            b.Line(3f, skinShade, V(16, 16), V(8, 20));
            b.Shaded(6f, 21f, 5f, 4.5f, skin);
            b.Dot(4, 22, PenguinPalette.Eye);
            b.Shaded(34f, 16f, 22f, 11f, shell);
            b.Rect(10, 0, 50, 8, PenguinPalette.Clear);
            b.Flipper(V(18, 10), V(4, 2), 3.5f, skin);
            b.Flipper(V(50, 10), V(62, 6), 3f, skin);
            b.Line(1f, shellShade, V(13, 9), V(56, 9));
            // 六角の模様で甲羅だとわかるようにする
            foreach (var p in new[] { V(24, 14), V(34, 18), V(44, 14), V(29, 22), V(40, 22) })
            {
                b.Polygon(p, plate, V(p.x - 3, p.y), V(p.x - 1.5f, p.y + 2.5f), V(p.x + 1.5f, p.y + 2.5f), V(p.x + 3, p.y), V(p.x + 1.5f, p.y - 2.5f), V(p.x - 1.5f, p.y - 2.5f));
            }
            // 甲羅から垂れる海藻。長く海を旅してきた年月を見せる
            b.Line(0.8f, weed, V(52, 12), V(54, 6), V(52, 2));
            b.Line(0.8f, weed, V(46, 9), V(47, 4));
            b.Line(0.8f, weed, V(57, 15), V(60, 11), V(59, 6));
            // ペンギンは小さく乗せ、カメの大きさを引き立てる
            b.Shaded(36f, 31f, 6.5f, 6f, navy);
            b.Belly(34f, 30f, 4f, 4f);
            b.Flipper(V(40, 32), V(45, 29), 2f, PenguinPalette.Shade(navy));
            b.Shaded(34f, 41f, 7f, 6.5f, navy);
            b.White(32f, 40f, 5.5f, 4.5f);
            b.TurnedFace(32f, 41f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
        }

        /// <summary>No.91 ニンギョペン: 歌の好きなロマンチスト。足の代わりの魚の尾びれを岩の上で巻き、腰かけている。岩の台と前へくるんと巻いた尾のシルエット</summary>
        private static void Ningyo(PenguinBrush b)
        {
            var body = Rgb(120, 130, 200);
            var bodyShade = PenguinPalette.Shade(body);
            var tail = Rgb(60, 180, 170);
            var tailShade = PenguinPalette.Shade(tail);
            var pink = Rgb(250, 160, 200);
            var rock = Rgb(130, 130, 140);
            // 背中に流すピンクの羽毛を髪のように長くし、後ろ姿でも人魚らしくする
            b.Polygon(V(44, 38), PenguinPalette.Shade(pink), V(34, 50), V(46, 48), V(52, 40), V(54, 28), V(50, 20), V(46, 28), V(42, 24), V(38, 36));
            b.Polygon(V(44, 40), pink, V(36, 49), V(45, 46), V(50, 39), V(51, 30), V(47, 32), V(42, 30), V(40, 38));
            b.Polygon(V(44, 7), PenguinPalette.Shade(rock), V(28, 2), V(62, 2), V(60, 12), V(48, 16), V(32, 13));
            b.Polygon(V(44, 9), rock, V(30, 4), V(59, 4), V(58, 11), V(48, 14), V(33, 12));
            // 尾は岩の前へ垂らして地面ぞいにのばし、先だけくるんと巻き上げて尾びれを扇に広げる
            b.Line(3.5f, tailShade, V(36, 18), V(30, 10), V(22, 5), V(12, 5), V(8, 9));
            b.Line(2.8f, tail, V(36, 19), V(30, 11), V(22, 6), V(12, 6), V(8, 9));
            b.Polygon(V(7, 13), tailShade, V(8, 10), V(1, 14), V(4, 16), V(5, 20), V(10, 14));
            b.Polygon(V(7, 13), tail, V(8, 11), V(2, 14), V(5, 15), V(6, 18), V(9, 14));
            foreach (var p in new[] { V(16, 6), V(21, 6), V(26, 8), V(31, 12) })
            {
                b.Dot((int)p.x, (int)p.y, Rgb(150, 230, 220));
            }
            b.Shaded(38f, 22f, 8f, 8f, body);
            b.Belly(36f, 21f, 5f, 5.5f);
            b.Flipper(V(42, 22), V(46, 14), 2f, bodyShade);
            b.Shaded(36f, 37f, 10f, 9.5f, body);
            b.White(33f, 36f, 7.5f, 6.5f);
            b.TurnedFace(33f, 37f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
            // 真珠の首飾り。白い粒を首の線に並べる
            for (int x = 28; x <= 44; x += 3)
            {
                b.Ellipse(x, 28f - (x - 36) * (x - 36) / 64f, 1.2f, 1.2f, PenguinPalette.Belly);
            }
            // 貝の髪飾り。頭の後ろ寄りに付けて、横顔の飾りにする
            b.Polygon(V(42, 46), pink, V(38, 44), V(46, 44), V(48, 49), V(44, 52), V(40, 51), V(37, 48));
            b.Line(0.5f, PenguinPalette.Shade(pink), V(42, 44), V(42, 51));
            b.Line(0.5f, PenguinPalette.Shade(pink), V(42, 44), V(39, 50));
            b.Line(0.5f, PenguinPalette.Shade(pink), V(42, 44), V(45, 50));
            b.Flipper(V(30, 23), V(26, 16), 2f, bodyShade);
        }

        /// <summary>No.92 ガラパゴスペン: 陽気でだらけ屋。暑さにへばってのけぞり、うちわでパタパタあおぐ。大きなつばの麦わら帽子と、横に出たうちわのシルエット</summary>
        private static void Galapagos(PenguinBrush b)
        {
            var body = Rgb(70, 72, 86);
            var bodyShade = PenguinPalette.Shade(body);
            var straw = Rgb(240, 210, 110);
            var strawShade = PenguinPalette.Shade(straw);
            var flower = Rgb(245, 100, 60);
            var sweat = Rgb(150, 210, 250);
            b.Foot(26f, 4f, -25f);
            b.Foot(42f, 4f, 15f);
            b.Flipper(V(44, 22), V(52, 14), 2.5f, bodyShade);
            // おなかを突き出してのけぞり、暑さでぐったりした姿勢にする
            b.ShadedTilted(35f, 19f, 12f, 14f, -18f, body);
            b.BellyTilted(32f, 18f, 8.5f, 10f, -18f);
            b.Shaded(36f, 37f, 10f, 9.5f, body);
            b.White(33f, 36f, 7.5f, 6.5f);
            b.TurnedFace(33f, 37f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            // 花のレイ。赤橙の花を首まわりに並べて南の島を見せる
            foreach (var p in new[] { V(26, 29), V(31, 27), V(36, 27), V(41, 28), V(45, 30) })
            {
                b.Ellipse(p.x, p.y, 2.2f, 2f, flower);
                b.Dot((int)p.x, (int)p.y, Rgb(255, 230, 120));
            }
            // つばの広い麦わら帽子。シルエットの上を横に広げる
            b.Ellipse(37f, 45f, 16f, 3f, strawShade);
            b.Ellipse(36f, 46f, 15f, 2.5f, straw);
            b.Shaded(37f, 50f, 8f, 5f, straw);
            b.Rect(29, 46, 16, 2, Rgb(200, 60, 60));
            // 額に上げたサングラス
            if (b.Front)
            {
                b.Rect(25, 40, 5, 2, Rgb(40, 40, 60));
                b.Rect(31, 40, 5, 2, Rgb(40, 40, 60));
                b.Dot(30, 41, Rgb(40, 40, 60));
            }
            // うちわであおぎ、汗のしずくで暑さを見せる
            b.Line(0.6f, Rgb(160, 110, 60), V(24, 22), V(13, 30));
            b.Ellipse(10f, 34f, 6f, 6.5f, Rgb(240, 245, 250));
            b.Ellipse(10f, 34f, 3f, 3f, Rgb(80, 150, 230));
            b.Flipper(V(28, 22), V(22, 24), 2.5f, bodyShade);
            b.Tilted(48f, 40f, 1.2f, 2f, 0f, sweat);
            b.Tilted(50f, 34f, 1f, 1.6f, 0f, sweat);
            b.Tilted(20f, 46f, 1f, 1.6f, 0f, sweat);
            b.Line(0.5f, PenguinPalette.Belly, V(2, 26), V(4, 24));
            b.Line(0.5f, PenguinPalette.Belly, V(3, 42), V(5, 44));
        }

        /// <summary>No.93 イワトビペン: 短気なけんかっ早い子。岩の上で両足を縮めて跳び上がった瞬間。後ろへツンツン跳ねる黄色い飾り羽と、下の岩から離れて浮いた体のシルエット</summary>
        private static void Iwatobi(PenguinBrush b)
        {
            var body = Rgb(64, 66, 80);
            var bodyShade = PenguinPalette.Shade(body);
            var crest = Rgb(250, 220, 60);
            var rock = Rgb(140, 138, 140);
            b.Polygon(V(30, 6), PenguinPalette.Shade(rock), V(12, 2), V(50, 2), V(46, 9), V(36, 13), V(22, 12), V(15, 8));
            b.Polygon(V(30, 7), rock, V(14, 4), V(48, 4), V(44, 9), V(36, 11), V(23, 10), V(17, 7));
            b.Line(0.5f, PenguinPalette.Shade(rock), V(28, 10), V(30, 6));
            // 跳んだ勢いの線。岩から離れたばかりなのを見せる
            b.Line(0.5f, PenguinPalette.Belly, V(14, 14), V(16, 18));
            b.Line(0.5f, PenguinPalette.Belly, V(50, 14), V(48, 18));
            // 足は縮めて体の下に引き寄せる
            b.Foot(26f, 21f, 20f, 0.8f);
            b.Foot(38f, 21f, -20f, 0.8f);
            b.Flipper(V(42, 32), V(54, 40), 2.5f, bodyShade);
            b.Shaded(33f, 30f, 10.5f, 9.5f, body);
            b.Belly(30f, 29f, 7f, 6.5f);
            b.Shaded(31f, 44f, 10f, 9.5f, body);
            b.White(28f, 43f, 7.5f, 6.5f);
            // 頭のてっぺんの黒い羽もツンツン立て、短気な性格を形で見せる
            b.Triangle(V(26, 51), V(30, 52), V(25, 59), body);
            b.Triangle(V(30, 52), V(35, 52), V(33, 61), body);
            b.Triangle(V(34, 51), V(39, 50), V(41, 58), body);
            // 黄色い眉の飾り羽は目の上から後ろへ跳ねさせる。イワトビペンギンの印
            b.Line(0.8f, crest, V(22, 48), V(36, 49), V(46, 54));
            b.Line(0.6f, crest, V(36, 49), V(48, 50));
            b.Line(0.6f, crest, V(38, 50), V(44, 58));
            b.Line(0.6f, crest, V(40, 48), V(50, 46));
            b.Flipper(V(26, 32), V(15, 38), 2.5f, bodyShade);
            b.TurnedFace(28f, 44f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            if (b.Front)
            {
                b.Dot(25, 44, Rgb(220, 40, 40));
            }
        }

        /// <summary>No.94 ユウビンペン: 律儀なはたらき者。片足を上げて駆け、片ヒレで手紙を高く掲げる。前のめりの体と、肩から下げた鞄が後ろで揺れるシルエット</summary>
        private static void Yuubin(PenguinBrush b)
        {
            var navy = Rgb(44, 60, 120);
            var navyShade = PenguinPalette.Shade(navy);
            var red = Rgb(210, 50, 50);
            var bag = Rgb(150, 100, 60);
            var bagShade = PenguinPalette.Shade(bag);
            // 後ろ足を跳ね上げ、駆けている途中に見せる
            b.Foot(48f, 10f, 45f, 0.9f);
            b.Foot(28f, 4f, -10f);
            // 鞄は後ろへ振らせ、走る勢いを出す
            b.Line(0.7f, bagShade, V(28, 34), V(44, 20));
            b.ShadedTilted(36f, 20f, 11f, 13f, 15f, navy);
            b.BellyTilted(33f, 19f, 7.5f, 9.5f, 15f);
            b.Polygon(V(49, 17), bagShade, V(42, 23), V(56, 23), V(57, 12), V(43, 11));
            b.Polygon(V(49, 18), bag, V(43, 22), V(55, 22), V(55, 15), V(44, 14));
            b.Rect(45, 22, 7, 4, PenguinPalette.Belly);
            b.Line(0.5f, Rgb(200, 60, 60), V(46, 24), V(50, 24));
            b.Line(0.7f, bagShade, V(28, 34), V(44, 20));
            b.Shaded(28f, 37f, 10f, 9.5f, navy);
            b.White(25f, 36f, 7.5f, 6.5f);
            b.TurnedFace(25f, 37f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
            // 赤い帽子。つばを前へ出して走る向きを強める
            b.Shaded(30f, 46f, 8.5f, 4f, red);
            b.Rect(19, 43, 22, 3, red);
            b.Rect(19, 43, 10, 1, PenguinPalette.Shade(red));
            b.Rect(14, 43, 6, 2, PenguinPalette.Shade(red));
            // 掲げた手紙。白い封筒を空へ突き出し、シルエットの角にする
            b.Flipper(V(22, 26), V(10, 44), 2.5f, navyShade);
            b.Polygon(V(8, 49), Rgb(200, 200, 210), V(2, 45), V(14, 45), V(14, 54), V(2, 54));
            b.Polygon(V(8, 49), PenguinPalette.Belly, V(3, 46), V(13, 46), V(13, 53), V(3, 53));
            b.Line(0.5f, Rgb(200, 200, 210), V(3, 53), V(8, 49), V(13, 53));
            b.Rect(11, 50, 2, 2, Rgb(220, 60, 60));
            b.Line(0.6f, Rgb(200, 220, 240), V(56, 34), V(62, 34));
            b.Line(0.6f, Rgb(200, 220, 240), V(58, 30), V(63, 30));
        }

        /// <summary>No.95 タコアゲペン: 夢見る冒険家。凧の糸をつかんで、引っぱられて足が地面から浮く。上の大きなひし形の凧と、斜めの糸でつながった縦長のシルエット</summary>
        private static void Takoage(PenguinBrush b)
        {
            var body = Rgb(120, 190, 240);
            var bodyShade = PenguinPalette.Shade(body);
            var kite = Rgb(225, 55, 55);
            var kiteShade = PenguinPalette.Shade(kite);
            var frame = Rgb(150, 110, 70);
            // 凧のしっぽ。風に流して、空高く揚がっているのを見せる
            b.Line(0.6f, Rgb(250, 220, 90), V(44, 36), V(50, 32), V(46, 26), V(52, 22));
            b.Rect(48, 31, 3, 2, Rgb(250, 220, 90));
            b.Rect(49, 23, 3, 2, Rgb(250, 220, 90));
            // 糸は凧からヒレへまっすぐ張り、引っぱられている力を見せる
            b.Line(0.5f, Rgb(240, 240, 240), V(43, 38), V(30, 34));
            b.Polygon(V(44, 50), kiteShade, V(44, 63), V(58, 50), V(44, 36), V(30, 50));
            b.Polygon(V(43, 51), kite, V(43, 61), V(56, 51), V(44, 38), V(31, 50));
            b.Line(0.5f, frame, V(44, 62), V(44, 37));
            b.Line(0.5f, frame, V(31, 50), V(57, 50));
            // 凧の顔の模様。にっこりの目と口だけで、遠くからでも凧とわかる飾りにする
            b.Line(0.5f, PenguinPalette.Belly, V(38, 54), V(40, 56), V(42, 54));
            b.Line(0.5f, PenguinPalette.Belly, V(46, 54), V(48, 56), V(50, 54));
            b.Line(0.5f, PenguinPalette.Belly, V(40, 45), V(44, 43), V(48, 45));
            // 足はぶらりと浮かせ、地面に影だけ残す
            b.Ellipse(20f, 2f, 7f, 1f, Rgb(90, 140, 90));
            b.Foot(15f, 11f, -40f, 0.8f);
            b.Foot(24f, 10f, 30f, 0.8f);
            b.ShadedTilted(21f, 19f, 8.5f, 10f, -15f, body);
            b.BellyTilted(19f, 18f, 5.5f, 7f, -15f);
            b.Shaded(18f, 33f, 8.5f, 8f, body);
            b.White(16f, 32f, 6.5f, 5.5f);
            b.TurnedFace(16f, 33f, FaceLeft, EyeStyle.Smile, eyeGap: 5.5f);
            // 両ヒレを凧へのばして糸をつかみ、引っぱり上げられているのを見せる
            b.Flipper(V(25, 24), V(30, 34), 2.5f, bodyShade);
        }

        /// <summary>No.96 カザミドリペン: 気まぐれなお天気屋。屋根の上の風見になって1本の棒の上に立ち、風に吹かれて向きを変える。細い棒と十字の方位、後ろへのびる矢の尾のシルエット</summary>
        private static void Kazamidori(PenguinBrush b)
        {
            var copper = Rgb(190, 115, 70);
            var copperShade = PenguinPalette.Shade(copper);
            var patina = Rgb(100, 180, 155);
            var pole = Rgb(90, 90, 100);
            var wind = Rgb(220, 235, 245);
            // 棒と十字の方位。奥行きの棒は短く斜めにして、立体の十字に見せる
            b.Line(0.8f, pole, V(32, 2), V(32, 26));
            b.Line(0.6f, pole, V(18, 10), V(46, 10));
            b.Line(0.6f, pole, V(27, 6), V(37, 14));
            b.Ellipse(18f, 10f, 1.5f, 1.5f, pole);
            b.Ellipse(46f, 10f, 1.5f, 1.5f, pole);
            b.Ellipse(32f, 2f, 3f, 1.5f, pole);
            // 矢の尾は後ろへ長くのばし、風見だと形だけでわかるようにする
            b.Line(1f, copperShade, V(38, 30), V(56, 34));
            b.Polygon(V(56, 35), copperShade, V(50, 33), V(56, 46), V(63, 46), V(59, 35), V(63, 24), V(56, 24));
            b.Polygon(V(56, 35), patina, V(52, 34), V(57, 44), V(61, 44), V(58, 35), V(61, 26), V(57, 26));
            b.Foot(30f, 27f, 0f, 0.7f);
            b.ShadedTilted(32f, 35f, 9f, 9f, 0f, copper);
            // 緑青のしみ。長く雨風にさらされた金属の古さを出す
            b.Ellipse(36f, 32f, 3f, 2f, patina);
            b.Ellipse(29f, 38f, 2f, 1.5f, patina);
            b.Flipper(V(34, 36), V(42, 38), 2.5f, copperShade);
            b.Shaded(28f, 48f, 9f, 8.5f, copper);
            b.Ellipse(33f, 53f, 2.5f, 1.5f, patina);
            b.White(25f, 47f, 6.5f, 5.5f);
            b.TurnedFace(25f, 48f, FaceLeft, EyeStyle.Dot, eyeGap: 6f);
            // 回る風の線。弧を描かせて、くるくる向きを変える様子にする
            b.Line(0.5f, wind, V(4, 30), V(8, 34), V(14, 35));
            b.Line(0.5f, wind, V(2, 50), V(6, 56), V(12, 58));
            b.Line(0.5f, wind, V(50, 54), V(56, 58), V(62, 56));
            b.Line(0.5f, wind, V(46, 18), V(52, 20), V(58, 18));
        }

        /// <summary>No.97 フクロウペン: もの知りの夜ふかし。枝にとまって首をかしげる。2本の羽角が立つフードと、横にのびる枝のシルエット</summary>
        private static void Fukurou(PenguinBrush b)
        {
            var brown = Rgb(150, 105, 70);
            var brownShade = PenguinPalette.Shade(brown);
            var cream = Rgb(240, 225, 190);
            var branch = Rgb(90, 60, 40);
            var leaf = Rgb(90, 150, 80);
            b.Line(2f, branch, V(2, 8), V(30, 10), V(62, 8));
            b.Line(1f, branch, V(48, 9), V(56, 14), V(60, 18));
            b.Tilted(60f, 19f, 3f, 1.5f, 30f, leaf);
            b.Tilted(6f, 11f, 3f, 1.5f, -20f, leaf);
            // 羽をたたんだ翼は体の両脇に縦長に付け、フクロウのずんぐりした形にする
            b.Shaded(33f, 22f, 12f, 12f, brown);
            // 胸のクリーム色はおなかと同じく正面だけ。後ろ姿は茶色の背中にする
            if (b.Front)
            {
                b.Ellipse(33f, 22f, 7f, 9f, cream);
                for (int i = 0; i < 3; i++)
                {
                    b.Line(0.5f, brownShade, V(29 + i * 4, 19 + (i % 2) * 3), V(30 + i * 4, 17 + (i % 2) * 3));
                }
            }
            b.Tilted(22f, 21f, 4f, 10f, -10f, brownShade);
            b.Tilted(44f, 21f, 4f, 10f, 10f, brownShade);
            // 枝をつかむ足
            b.Foot(28f, 10f, 0f, 0.7f);
            b.Foot(38f, 10f, 0f, 0.7f);
            // 頭は少し傾け、羽角も傾けて首をかしげて見せる
            b.Triangle(V(19, 44), V(25, 48), V(16, 58), brown);
            b.Triangle(V(37, 50), V(43, 46), V(44, 60), brown);
            b.ShadedTilted(31f, 41f, 12f, 10.5f, 12f, brown);
            b.WhiteTilted(29f, 40f, 9f, 7f, 12f);
            // 目のまわりの茶色の輪。フクロウの大きな目を、黒目を大きくせずに見せる
            if (b.Front)
            {
                b.Ellipse(27f, 41f, 3.2f, 3.2f, brownShade);
                b.Ellipse(33f, 41f, 3.2f, 3.2f, brownShade);
                b.Ellipse(27f, 41f, 2.2f, 2.2f, PenguinPalette.Belly);
                b.Ellipse(33f, 41f, 2.2f, 2.2f, PenguinPalette.Belly);
            }
            b.TurnedFace(29f, 41f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
        }

        /// <summary>No.98 テングペン: うぬぼれ屋の修行者。一本歯の下駄で立ち、大きなヤツデのうちわを掲げる。横へ大きく突き出す長いくちばしと、高く上がった葉のシルエット</summary>
        private static void Tengu(PenguinBrush b)
        {
            var mask = Rgb(220, 60, 50);
            var maskShade = PenguinPalette.Shade(mask);
            var robe = Rgb(240, 240, 236);
            var robeShade = Rgb(196, 196, 200);
            var black = Rgb(40, 40, 50);
            var leaf = Rgb(70, 160, 70);
            var leafShade = PenguinPalette.Shade(leaf);
            var geta = Rgb(170, 120, 70);
            // ヤツデのうちわ。指のような葉を放射状に広げて、遠くからでもわかる形にする
            b.Line(0.8f, Rgb(120, 90, 50), V(44, 30), V(50, 42));
            for (int i = 0; i < 7; i++)
            {
                float angle = (20f + i * 25f) * Mathf.Deg2Rad;
                var tip = V(51 + Mathf.Cos(angle) * 11f, 45 + Mathf.Sin(angle) * 11f);
                b.Line(1.6f, leafShade, V(51, 45), tip);
                b.Line(1f, leaf, V(51, 45), tip);
            }
            b.Ellipse(51f, 45f, 3f, 3f, leaf);
            b.Flipper(V(40, 26), V(46, 32), 2.5f, black);
            // 一本歯の下駄。細い1本の歯の上に立たせ、修行者らしい不安定さを出す
            b.Rect(31, 2, 3, 5, geta);
            b.Rect(22, 6, 22, 3, geta);
            b.Foot(28f, 10f, 0f, 0.8f);
            b.Foot(37f, 10f, 0f, 0.8f);
            b.Shaded(33f, 21f, 10f, 11f, black);
            // 山伏の白い衣と梵天は胸側だけ。後ろ姿で白いおなかのように見えないようにする
            if (b.Front)
            {
                b.Shaded(32f, 22f, 8f, 10f, robe);
                b.Line(0.5f, robeShade, V(28, 30), V(32, 22), V(36, 30));
                b.Ellipse(27f, 24f, 2f, 2f, Rgb(250, 160, 60));
                b.Ellipse(37f, 24f, 2f, 2f, Rgb(250, 160, 60));
            }
            b.Shaded(32f, 39f, 10f, 9.5f, mask);
            b.White(29f, 38f, 6.5f, 5.5f);
            b.TurnedFace(29f, 39f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            // 長いくちばし。ふつうのくちばしに重ねて大きく横へ突き出す
            if (b.Front)
            {
                b.SideBeak(25.4f, 35.5f, FaceLeft, length: 18f);
            }
            // 頭襟は額の小さな黒い多角形
            b.Polygon(V(32, 50), black, V(28, 48), V(36, 48), V(36, 51), V(32, 53), V(28, 51));
            b.Flipper(V(24, 26), V(18, 18), 2.5f, black);
        }

        /// <summary>No.99 コタツペン: ぐうたらな寒がり。こたつに入って上半身だけ出し、天板にあごを乗せて眠そうにする。下半分の四角い布団の横長のシルエット</summary>
        private static void Kotatsu(PenguinBrush b)
        {
            var navy = Rgb(60, 80, 140);
            var navyShade = PenguinPalette.Shade(navy);
            var futon = Rgb(240, 140, 60);
            var futonShade = PenguinPalette.Shade(futon);
            var red = Rgb(210, 60, 60);
            var top = Rgb(160, 110, 60);
            var mikan = Rgb(250, 150, 30);
            b.Shaded(34f, 34f, 11f, 10.5f, navy);
            b.Belly(31f, 32f, 7.5f, 7f);
            // 布団は角の丸い四角。下半分を横いっぱいに占め、どっしり動かない感じを出す
            b.Polygon(V(32, 14), futonShade, V(2, 2), V(62, 2), V(62, 22), V(2, 22));
            b.Polygon(V(31, 15), futon, V(3, 4), V(60, 4), V(60, 21), V(3, 21));
            // 布団の模様。赤の格子で和柄の雰囲気を出す
            for (int x = 10; x <= 54; x += 11)
            {
                b.Line(0.5f, red, V(x, 4), V(x, 21));
            }
            b.Line(0.5f, red, V(3, 12), V(60, 12));
            foreach (var p in new[] { V(15, 8), V(37, 8), V(26, 16), V(48, 16) })
            {
                b.Ellipse(p.x, p.y, 2f, 2f, red);
            }
            // 天板は布団より少し広くして、こたつの形をはっきりさせる
            b.Rect(0, 22, 64, 3, PenguinPalette.Shade(top));
            b.Rect(0, 24, 64, 2, top);
            // 天板に乗せたヒレ
            b.Flipper(V(24, 28), V(14, 27), 2.5f, navyShade);
            b.Flipper(V(42, 28), V(48, 27), 2.5f, navyShade);
            b.Shaded(32f, 42f, 10f, 9.5f, navy);
            b.White(29f, 41f, 7.5f, 6.5f);
            b.TurnedFace(29f, 42f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            // みかん。こたつの定番を天板に置く
            b.Shaded(53f, 30f, 4f, 3.5f, mikan);
            b.Shaded(58f, 29f, 3.5f, 3f, mikan);
            b.Rect(53, 33, 1, 1, Rgb(80, 150, 60));
            b.Shaded(8f, 29f, 3.5f, 3f, mikan);
            b.Line(0.5f, PenguinPalette.Belly, V(40, 52), V(44, 54));
            b.Line(0.5f, PenguinPalette.Belly, V(44, 50), V(48, 52));
        }

        /// <summary>No.100 マカロニペン: パンクな反抗期。ヒレを空へ突き上げて叫ぶ。額から後ろへ燃え上がる炎のような飾り羽と大きなくちばしのシルエット</summary>
        private static void Macaroni(PenguinBrush b)
        {
            var body = Rgb(58, 60, 74);
            var bodyShade = PenguinPalette.Shade(body);
            var crestOut = Rgb(245, 120, 40);
            var crestIn = Rgb(255, 210, 70);
            var beakRed = Rgb(220, 70, 40);
            // 飾り羽は額から後ろ上へ炎の形に広げる。マカロニペンギンの印を火の勢いにする
            b.Polygon(V(38, 48), crestOut, V(22, 43), V(30, 52), V(32, 62), V(38, 55), V(44, 63), V(46, 55), V(54, 60), V(52, 51), V(61, 50), V(50, 44), V(34, 42));
            b.Polygon(V(39, 49), crestIn, V(25, 44), V(32, 51), V(35, 58), V(39, 53), V(44, 59), V(45, 53), V(51, 56), V(49, 49), V(40, 45));
            b.Foot(24f, 4f, -20f);
            b.Foot(42f, 4f, 15f);
            b.Flipper(V(44, 22), V(52, 14), 2.5f, bodyShade);
            b.ShadedTilted(34f, 19f, 11f, 13f, -12f, body);
            b.BellyTilted(31f, 18f, 7.5f, 9.5f, -12f);
            b.Shaded(32f, 37f, 10.5f, 10f, body);
            b.White(29f, 36f, 7.5f, 6.5f);
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
            // 大きなくちばし。ふつうより長く開いて重ね、ふちを赤くして叫ぶ勢いを出す
            if (b.Front)
            {
                b.SideBeak(25.4f, 33.5f, FaceLeft, open: true, length: 8f);
                b.Line(0.5f, beakRed, V(24, 35.5f), V(17.5f, 34));
            }
            // 突き上げたヒレ。シルエットの左上へ角のように飛び出させる
            b.Flipper(V(24, 26), V(12, 50), 3f, bodyShade);
            b.Rect(11, 50, 3, 3, bodyShade);
            b.Sparkle(6, 58, crestIn);
        }
    }
}
