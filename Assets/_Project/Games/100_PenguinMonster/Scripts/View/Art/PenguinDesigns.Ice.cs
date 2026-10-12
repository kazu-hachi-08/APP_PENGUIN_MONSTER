using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>こおりタイプが主のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 IceLight = Rgb(200, 240, 255);
        private static readonly Color32 IceShade = Rgb(130, 196, 230);

        /// <summary>No.1 ユキペン: 主役で活発。前のめりに跳びはね、ヒレを突き上げて叫ぶ。氷の冠は勢いで後ろへなびく</summary>
        private static void Yuki(PenguinBrush b)
        {
            var blue = Rgb(120, 190, 235);
            var blueShade = PenguinPalette.Shade(blue);
            b.Triangle(V(30, 46), V(38, 44), V(50, 57), IceShade);
            b.Triangle(V(26, 48), V(33, 47), V(40, 62), IceLight);
            b.Triangle(V(22, 47), V(28, 49), V(27, 60), IceLight);
            b.Flipper(V(42, 26), V(56, 19), 3f, blueShade);
            b.Foot(43f, 7f, 30f);
            b.Foot(24f, 5f, -10f);
            b.ShadedTilted(34f, 19f, 12f, 14f, 18f, blue);
            b.BellyTilted(31f, 18f, 8f, 10.5f, 18f);
            b.Shaded(27f, 37f, 13f, 12.5f, blue);
            b.White(24f, 36f, 10f, 8.5f);
            b.Flipper(V(23, 26), V(11, 41), 3f, blueShade);
            b.TurnedFace(24f, 37f, FaceLeft, EyeStyle.Round, open: true);
        }

        /// <summary>No.7 ヒョウガペン: 頑固な力持ち。足を踏んばり腕組みして動かない。肩幅の広い角ばった氷河のよろいに、小さな頭が埋まる逆三角形</summary>
        private static void Hyouga(PenguinBrush b)
        {
            var armor = Rgb(160, 210, 236);
            var armorShade = PenguinPalette.Shade(armor);
            var navy = Rgb(60, 80, 130);
            // 氷の結晶は背中側（右）を大きくして、左右非対称の山並みのシルエットにする
            b.Polygon(V(52, 40), IceShade, V(45, 38), V(48, 60), V(53, 46), V(60, 56), V(59, 36));
            b.Polygon(V(50, 42), IceLight, V(46, 40), V(48, 56), V(52, 42));
            b.Polygon(V(9, 34), IceShade, V(4, 31), V(3, 46), V(10, 37), V(15, 33));
            b.Foot(18f, 4f, -8f, 1.2f);
            b.Foot(46f, 4f, 8f, 1.2f);
            b.Shaded(33f, 19f, 18f, 15f, navy);
            b.Belly(29f, 16f, 12f, 11f);
            b.Shaded(29f, 39f, 10.5f, 10f, navy);
            b.White(26f, 38f, 7.5f, 6.5f);
            // 肩よろいは角ばらせて外へ張り出し、丸い体の多い中で肩幅の広い逆三角形を作る
            b.Polygon(V(11, 28), armorShade, V(2, 22), V(3, 32), V(9, 36), V(19, 32), V(18, 22));
            b.Polygon(V(10, 29), armor, V(4, 24), V(5, 31), V(9, 34), V(16, 31), V(16, 24));
            b.Polygon(V(51, 32), armorShade, V(42, 26), V(43, 38), V(52, 42), V(61, 36), V(60, 25));
            b.Polygon(V(50, 33), armor, V(44, 28), V(45, 36), V(52, 39), V(58, 34), V(58, 27));
            b.Sparkle(9, 29, IceLight);
            // 組んだ腕は氷の籠手。胸の前に太く横たえ、どかない意思を見せる
            b.Line(4.5f, armorShade, V(46, 22), V(14, 20));
            b.Line(3f, armor, V(45, 23), V(15, 22));
            b.Line(0.6f, armorShade, V(30, 25), V(31, 19));
            b.TurnedFace(26f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 6.5f);
        }

        /// <summary>No.8 ホシペン: 神秘的な夢見がち。大きな星に腰かけて夜空をただよい、てっぺんの角にもたれる。星形の大きな台座と小さな体のシルエット</summary>
        private static void Hoshi(PenguinBrush b)
        {
            var body = Rgb(196, 206, 250);
            var bodyShade = PenguinPalette.Shade(body);
            var star = Rgb(255, 232, 130);
            var sparkle = Rgb(255, 240, 150);
            // 金は伝説の印なので使わず、淡い黄色の星にする。影の星を右下へずらして厚みを出す
            b.Star(39f, 21f, 20f, 9f, PenguinPalette.Shade(star));
            b.Star(38f, 22f, 18.5f, 8f, star);
            // 浮いているのを、星が流れてきた跡のきらめきで見せる
            b.Sparkle(60, 30, sparkle);
            b.Sparkle(57, 4, sparkle);
            b.Dot(61, 14, sparkle);
            // てっぺんの角にもたれてのけぞり、空を見上げる
            b.ShadedTilted(24f, 34f, 8f, 9.5f, -25f, body);
            b.BellyTilted(22f, 34f, 5f, 7f, -25f);
            b.Foot(13f, 27f, -35f, 0.8f);
            b.Foot(18f, 25f, -50f, 0.8f);
            b.Shaded(29f, 47f, 9f, 8.5f, body);
            b.White(26f, 47f, 6.5f, 5.5f);
            // 片ヒレを空へのばし、星をつかもうとする
            b.Flipper(V(21, 38), V(10, 50), 3f, bodyShade);
            b.TurnedFace(26f, 48f, FaceLeft, EyeStyle.Cross, eyeGap: 6f);
            b.Sparkle(6, 58, sparkle);
            b.Sparkle(5, 14, sparkle);
        }

        /// <summary>No.9 コウテイペン: いばりんぼの王さま。のけぞって胸を張り、くちばしを上げて見下ろす。細長い体と、後ろへ大きく翻るマントで縦長のシルエット</summary>
        private static void Koutei(PenguinBrush b)
        {
            var navy = Rgb(50, 60, 100);
            var navyShade = PenguinPalette.Shade(navy);
            var cape = Rgb(200, 50, 70);
            var yellow = Rgb(250, 190, 60);
            // マントは肩から右下へ翻らせ、すそを波打たせて風格を出す
            b.Polygon(V(46, 24), PenguinPalette.Shade(cape), V(36, 44), V(48, 40), V(58, 30), V(62, 14), V(58, 4), V(50, 8), V(42, 3), V(36, 10));
            b.Polygon(V(45, 26), cape, V(37, 41), V(47, 37), V(56, 28), V(59, 14), V(56, 7), V(50, 11), V(43, 6), V(38, 12));
            b.Line(1.5f, PenguinPalette.Belly, V(36, 10), V(42, 4), V(50, 9), V(57, 4), V(61, 14));
            b.Dot(42, 5, PenguinPalette.Eye);
            b.Dot(55, 6, PenguinPalette.Eye);
            b.Foot(25f, 3f, -5f);
            b.Foot(37f, 3f);
            b.ShadedTilted(32f, 21f, 11f, 18f, -14f, navy);
            b.BellyTilted(29f, 21f, 7.5f, 15f, -14f);
            // 胸の黄色はコウテイペンギンの印。張った胸をいちばん目立たせる
            if (b.Front)
            {
                b.Tilted(29f, 33f, 4.5f, 3f, -14f, Rgb(255, 230, 150));
            }
            // 後ろで組んだヒレ。前に出さないことで偉そうに見せる
            b.Flipper(V(38, 31), V(44, 16), 2.5f, navyShade);
            b.Shaded(35f, 47f, 9f, 8.5f, navy);
            b.White(32f, 46f, 6f, 5f);
            if (b.Front)
            {
                b.Tilted(37f, 43f, 1.5f, 3.5f, 20f, yellow);
            }
            b.TurnedFace(32f, 48f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            // 冠は少し後ろへ傾け、上を向いた頭の角度に合わせる
            b.Polygon(V(37, 57), PenguinPalette.Gold, V(31, 54), V(31, 61), V(34, 58), V(37, 62), V(40, 59), V(43, 61), V(42, 54));
            b.Dot(37, 57, Rgb(230, 60, 90));
        }

        /// <summary>No.10 リュウヒョウペン: マイペースなのんびり屋。横に広い流氷の板のはしに腰かけて足をぶらぶらさせ、釣り竿を垂らして待つ。平らな板と座った低い体に、竿と糸の縦の線が立つシルエット</summary>
        private static void Ryuuhyou(PenguinBrush b)
        {
            var body = Rgb(70, 170, 200);
            var bodyShade = PenguinPalette.Shade(body);
            var sea = Rgb(70, 140, 220);
            var rod = Rgb(150, 100, 60);
            b.Ellipse(38f, 4f, 25f, 3f, sea);
            b.Ellipse(8f, 4f, 5f, 1.5f, sea);
            // 流氷は横に広く平らな板。角ばった輪郭で上面を明るく、側面を影にして、ただよう氷の台にする
            b.Polygon(V(41, 12), IceShade, V(20, 6), V(60, 5), V(62, 10), V(61, 17), V(22, 18), V(19, 12));
            b.Polygon(V(41, 18), IceLight, V(21, 16), V(60, 15), V(61, 18), V(57, 21), V(24, 21));
            b.Line(0.6f, IceShade, V(44, 12), V(47, 7));
            b.Line(0.6f, IceShade, V(54, 15), V(57, 10));
            // 足は板のはしから前へぶらぶらさせ、腰かけているのを見せる
            b.Foot(17f, 10f, -70f, 0.9f);
            b.Foot(22f, 9f, -55f, 0.9f);
            b.Flipper(V(36, 25), V(44, 21), 2.5f, bodyShade);
            b.ShadedTilted(28f, 27f, 10.5f, 9f, -10f, body);
            b.BellyTilted(25f, 26f, 7f, 6f, -10f);
            b.Shaded(31f, 41f, 10f, 9.5f, body);
            b.White(28f, 40f, 7f, 6f);
            // 釣り竿と糸。横に広い板に縦の線を足して、のんびり待つ姿を見せる
            b.Line(0.8f, rod, V(22, 26), V(5, 52));
            b.Line(0.6f, Rgb(200, 210, 225), V(5, 52), V(5, 6));
            b.Ellipse(6f, 5f, 1.5f, 1.5f, Rgb(230, 60, 60));
            b.Flipper(V(27, 28), V(20, 27), 2.5f, bodyShade);
            b.TurnedFace(28f, 41f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
        }

        /// <summary>No.25 ツララペン: クールで無口。細身で横を向き、手前のヒレを真横に突き出して冷気を放つ。後ろへ流れる氷の髪とヒレから、つららが垂れる</summary>
        private static void Tsurara(PenguinBrush b)
        {
            var body = Rgb(150, 205, 235);
            var bodyShade = PenguinPalette.Shade(body);
            // 氷の髪は後ろへ長く流し、下につららを並べて「くし」の形のシルエットにする
            b.Polygon(V(42, 46), IceShade, V(26, 46), V(32, 55), V(42, 54), V(61, 48), V(52, 43), V(38, 40));
            b.Polygon(V(40, 47), IceLight, V(28, 47), V(33, 53), V(42, 52), V(55, 48), V(40, 43));
            b.Triangle(V(53, 45), V(58, 47), V(57, 37), IceShade);
            b.Triangle(V(45, 42), V(50, 44), V(48, 33), IceLight);
            b.Foot(30f, 3f, -5f, 0.9f);
            b.Foot(39f, 4f, 15f, 0.9f);
            b.ShadedTilted(34f, 17f, 8f, 14f, -5f, body);
            b.BellyTilted(31f, 17f, 5f, 11f, -5f);
            b.Shaded(33f, 39f, 9f, 9f, body);
            b.White(30f, 38f, 6.5f, 5.5f);
            // 突き出したヒレの下に、長さの違うつららを並べる
            b.Flipper(V(30, 28), V(7, 34), 3f, bodyShade);
            b.Triangle(V(8, 33), V(12, 32), V(10, 23), IceLight);
            b.Triangle(V(14, 31), V(18, 30), V(16, 19), IceLight);
            b.Triangle(V(20, 30), V(23, 29), V(21.5f, 23), IceShade);
            b.Sparkle(4, 41, IceLight);
            b.TurnedFace(30f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
        }

        /// <summary>No.26 カマクラペン: 世話好きでおっとり。かまくらの体を斜めに傾け、左上の穴から顔を出してヒレで「おいで」と招く。大きな半球に小さな頭が横から生えたシルエット</summary>
        private static void Kamakura(PenguinBrush b)
        {
            var snow = Rgb(236, 242, 250);
            var snowShade = Rgb(180, 196, 220);
            var orange = Rgb(240, 150, 90);
            // かまくらは右へずらした大きな半球。顔を出す側を低くして、のぞき込む傾きを作る
            b.Tilted(36f, 8f, 26f, 28f, -8f, snowShade);
            b.Tilted(35f, 9f, 24.5f, 26.5f, -8f, snow);
            b.Erase(CenterX, -2f, 40f, 4f);
            // 足はかまくらのすそから少しだけのぞかせ、中にペンギンがいるのを見せる
            b.Foot(18f, 3f, -15f, 0.9f);
            b.Foot(56f, 3f, 15f, 0.9f);
            b.Line(0.6f, snowShade, V(16, 18), V(30, 22), V(46, 22), V(58, 18));
            b.Line(0.6f, snowShade, V(24, 32), V(38, 34), V(52, 31));
            b.Line(0.6f, snowShade, V(30, 22), V(32, 10));
            b.Line(0.6f, snowShade, V(46, 22), V(45, 34));
            // 入口の奥の火。かまくらの中のあたたかさを、暗い穴と炎の対比で見せる
            b.Ellipse(40f, 4f, 7f, 8f, Rgb(60, 50, 70));
            b.Ellipse(40f, 6f, 3f, 4f, FlameOuter);
            b.Ellipse(40f, 5f, 1.5f, 2f, FlameInner);
            // 顔を出す穴。体の中からのぞいているように、頭を穴のふちにかぶせる
            b.Ellipse(19f, 35f, 10f, 9f, snowShade);
            b.Shaded(17f, 38f, 9.5f, 9f, orange);
            b.White(14f, 37f, 7f, 6f);
            b.Flipper(V(11, 26), V(4, 36), 2.5f, PenguinPalette.Shade(orange));
            b.TurnedFace(14f, 38f, FaceLeft, EyeStyle.Smile, eyeGap: 6f, open: true);
            // 頭に積もった雪。かまくらと一体なのを示す
            b.Ellipse(19f, 46f, 6f, 2f, snowShade);
            b.Ellipse(18f, 47f, 4.5f, 1.5f, snow);
        }
    }
}
