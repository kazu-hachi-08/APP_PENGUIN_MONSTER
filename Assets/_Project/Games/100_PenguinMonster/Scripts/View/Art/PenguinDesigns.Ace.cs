using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>エースランクのデザイン（仕様書 §6）。ふつうより大きな武器（ひれ・翼・しっぽ・岩）を付けて強そうに見せる</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.43 サメペン: どう猛。サメの着ぐるみで下から跳び上がって噛みつく。背中の大きな背びれが横へ突き出し、足元に尾びれ</summary>
        private static void Same(PenguinBrush b)
        {
            var shark = Rgb(90, 110, 150);
            var sharkShade = PenguinPalette.Shade(shark);
            // 背びれは頭（着ぐるみ）のてっぺんから後ろへ反らせ、ひと目でサメとわかる形にする
            b.Triangle(V(22, 50), V(36, 44), V(50, 63), sharkShade);
            b.Triangle(V(25, 50), V(35, 46), V(46, 60), shark);
            // 尾びれは体の後ろから三日月形に開き、水から跳ね上がった勢いを出す
            b.Line(2.5f, sharkShade, V(38, 9), V(48, 10));
            b.Polygon(V(50, 11), sharkShade, V(46, 10), V(56, 24), V(62, 26), V(56, 12), V(62, 1), V(54, 3));
            b.Polygon(V(52, 12), shark, V(48, 11), V(56, 21), V(59, 22), V(54, 12));
            b.Foot(26f, 3f, 10f);
            b.ShadedTilted(34f, 20f, 12f, 16f, 25f, shark);
            b.BellyTilted(30f, 19f, 7f, 12f, 25f);
            b.Flipper(V(24, 24), V(10, 18), 3.5f, sharkShade);
            // 頭を大きく前上へ突き出し、噛みつく口（着ぐるみの口）をシルエットの主役にする
            b.Shaded(23f, 40f, 15f, 14f, shark);
            if (b.Front)
            {
                b.Ellipse(19f, 39f, 10.5f, 9.5f, Rgb(160, 40, 60));
                SharkTeeth(b, 19f, 39f, 10.5f, 9.5f);
                b.Ellipse(19f, 38.5f, 7f, 6.5f, PenguinPalette.Belly);
            }
            b.TurnedFace(19f, 40f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
        }

        /// <summary>口の輪のふちから内側へ向けて三角の歯を並べる</summary>
        private static void SharkTeeth(PenguinBrush b, float x, float y, float radiusX, float radiusY)
        {
            const int Teeth = 12;
            const float Half = 0.18f;
            const float Depth = 3f;
            for (int i = 0; i < Teeth; i++)
            {
                float angle = i * Mathf.PI * 2f / Teeth;
                b.Triangle(OnEllipse(x, y, radiusX, radiusY, angle - Half), OnEllipse(x, y, radiusX, radiusY, angle + Half),
                    OnEllipse(x, y, radiusX - Depth, radiusY - Depth, angle), PenguinPalette.Belly);
            }
        }

        private static Vector2 OnEllipse(float x, float y, float radiusX, float radiusY, float angle)
            => V(x + Mathf.Cos(angle) * radiusX, y + Mathf.Sin(angle) * radiusY);

        /// <summary>No.44 ヒノトリペン: 熱血で目立ちたがり。炎の翼を上へ大きく広げて舞い上がる。足は浮き、炎の尾羽が下へ長く垂れる</summary>
        private static void Hinotori(PenguinBrush b)
        {
            var red = Rgb(220, 70, 50);
            // 尾羽は下へ流して、地面に立たず空にいることを足元の形で見せる
            b.Triangle(V(30, 16), V(38, 16), V(44, 1), FlameOuter);
            b.Triangle(V(28, 16), V(34, 16), V(26, 2), FlameMiddle);
            b.Triangle(V(32, 16), V(37, 16), V(36, 4), FlameInner);
            // 翼は不死鳥の紋章のように両上の角まで届かせ、体より翼が大きいシルエットにする
            Wing(b, V(38, 30), new[] { V(40, 26), V(54, 30), V(62, 42), V(61, 54), V(54, 62), V(44, 44) }, FlameMiddle, FlameOuter, FlameInner);
            Wing(b, V(26, 30), new[] { V(28, 24), V(12, 26), V(2, 36), V(3, 50), V(10, 60), V(22, 44) }, FlameMiddle, FlameOuter, FlameInner);
            b.Foot(30f, 11f, 70f, 0.7f);
            b.Foot(37f, 12f, 100f, 0.7f);
            b.ShadedTilted(33f, 24f, 9f, 12f, 10f, red);
            b.BellyTilted(31f, 23f, 5.5f, 8.5f, 10f);
            // とさかは上へ燃え上がらせ、翼の V の間を埋める
            b.Triangle(V(29, 46), V(35, 46), V(33, 62), FlameOuter);
            b.Triangle(V(32, 45), V(38, 46), V(42, 58), FlameMiddle);
            b.Triangle(V(31, 46), V(34, 46), V(33, 57), FlameInner);
            b.Shaded(31f, 39f, 10f, 9.5f, red);
            b.White(28.5f, 38f, 7.5f, 6f);
            b.TurnedFace(29f, 39f, FaceLeft, EyeStyle.Sparkle, eyeGap: 6f, open: true);
        }

        /// <summary>No.45 ユキヒョウペン: クールな狩人。低く身をかがめて獲物を狙う。長いしっぽを背中の上へ大きく巻き上げ、前のヒレに氷のつめ</summary>
        private static void YukiHyou(PenguinBrush b)
        {
            var fur = Rgb(220, 224, 234);
            var furShade = PenguinPalette.Shade(fur);
            var spot = Rgb(70, 72, 92);
            // しっぽは後ろから頭の上まで弧を描き、低い体とのコントラストでシルエットを作る
            var tail = new[] { V(48, 14), V(56, 20), V(59, 32), V(56, 44), V(48, 50), V(40, 48) };
            b.Line(3.2f, furShade, tail);
            b.Line(2.4f, fur, tail);
            b.Ellipse(40f, 48f, 3f, 3f, spot);
            b.Ellipse(58f, 32f, 1.3f, 1.8f, spot);
            b.Ellipse(53f, 46f, 1.6f, 1.3f, spot);
            b.Foot(48f, 3f, -10f);
            b.Foot(38f, 3f);
            // 腰を高く、頭を低く。飛びかかる直前の重心
            b.ShadedTilted(36f, 15f, 15f, 10f, 12f, fur);
            b.BellyTilted(33f, 11f, 10f, 5f, 12f);
            foreach (var p in new[] { V(38, 20), V(45, 18), V(42, 13), V(32, 19) })
            {
                b.Ellipse(p.x, p.y, 1.5f, 1.2f, spot);
            }
            // 耳は後ろへ倒して、身をひそめている感じを出す
            b.Triangle(V(18, 26), V(24, 25), V(28, 33), furShade);
            b.Triangle(V(19, 25), V(23, 25), V(26, 30), fur);
            b.Shaded(18f, 17f, 10f, 9.5f, fur);
            b.White(15f, 15.5f, 7f, 5.5f);
            b.Ellipse(22f, 23f, 1.4f, 1.1f, spot);
            b.Ellipse(25f, 18f, 1.4f, 1.1f, spot);
            b.Flipper(V(20, 9), V(8, 3), 2.5f, furShade);
            b.Triangle(V(8, 4), V(4, 3), V(1, 8), IceLight);
            b.Triangle(V(8, 3), V(4, 1), V(2, 2), IceShade);
            b.TurnedFace(15f, 17f, FaceLeft, EyeStyle.Dot, eyeGap: 6f);
        }

        /// <summary>No.46 ヒカリペン: 神秘的でやさしい。大きな光の花を背負い、目を閉じてヒレを合わせ、葉の上で浮かんで祈る</summary>
        private static void Hikari(PenguinBrush b)
        {
            var green = Rgb(160, 215, 160);
            var petal = Rgb(255, 190, 225);
            var petalShade = PenguinPalette.Darken(petal, 0.8f);
            var leaf = Rgb(90, 170, 100);
            const int Petals = 7;
            const float FlowerX = 36f;
            const float FlowerY = 44f;
            // 花は頭の後ろに高く置き、縦長のシルエットにする（ほかのエースは横へ広がるため）
            for (int i = 0; i < Petals; i++)
            {
                float angle = (i + 0.25f) * Mathf.PI * 2f / Petals;
                var p = OnEllipse(FlowerX, FlowerY, 14f, 13f, angle);
                b.Ellipse(p.x, p.y, 6f, 6f, petalShade);
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 4.8f, 4.8f, petal);
            }
            b.Ellipse(FlowerX, FlowerY, 12f, 12f, Rgb(255, 240, 160));
            // 葉の台座。足元を細くとがらせて、地面から浮いているように見せる
            b.Polygon(V(32, 9), PenguinPalette.Shade(leaf), V(10, 12), V(22, 6), V(32, 3), V(44, 6), V(56, 12), V(44, 10), V(32, 12), V(20, 10));
            b.Polygon(V(30, 9), leaf, V(12, 12), V(22, 8), V(30, 6), V(30, 11), V(20, 10));
            // 座って祈るので、下の体は横に広く低くする
            b.Shaded(32f, 19f, 14f, 9f, green);
            b.Belly(30f, 18f, 9.5f, 6f);
            b.Shaded(30f, 32f, 10.5f, 10f, green);
            b.White(28f, 31f, 7.5f, 6.5f);
            // ヒレを胸の前で合わせて祈る
            b.Flipper(V(21, 19), V(27, 24), 3f, PenguinPalette.Shade(green));
            b.TurnedFace(28f, 32f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
            b.Sparkle(5, 24, PenguinPalette.Gold);
            b.Sparkle(9, 44, PenguinPalette.Gold);
            b.Sparkle(58, 22, PenguinPalette.Gold);
            b.Sparkle(15, 58, PenguinPalette.Gold);
        }

        /// <summary>No.47 ライガンペン: 無口な力持ち。岩のよろいの巨体で、片足を高く上げて四股を踏む。背中に岩のとげ、頭に稲妻の角</summary>
        private static void Raigan(PenguinBrush b)
        {
            var rock = Rgb(130, 118, 108);
            var rockShade = PenguinPalette.Shade(rock);
            // 背中の岩のとげ。肩の後ろ（右上）へ張り出して、横にも縦にも厚いシルエットにする
            b.Polygon(V(42, 36), rockShade, V(34, 46), V(42, 52), V(46, 46), V(56, 48), V(54, 40), V(62, 36), V(54, 30), V(44, 26));
            b.Polygon(V(43, 37), Stone, V(37, 45), V(42, 49), V(45, 44), V(53, 45), V(52, 39), V(57, 36), V(52, 32), V(44, 29));
            b.Triangle(V(21, 46), V(27, 47), V(16, 62), BoltShade);
            b.Triangle(V(22, 46), V(26, 47), V(18, 58), Bolt);
            // 上げた足。太い脚で高く持ち上げ、四股の重さを出す
            b.Line(3.5f, rockShade, V(38, 14), V(48, 18), V(54, 22));
            b.Foot(56f, 23f, 25f, 1.1f);
            b.Foot(18f, 3f, 0f, 1.2f);
            b.ShadedTilted(28f, 17f, 18f, 14f, -12f, rock);
            b.BellyTilted(25f, 16f, 12f, 10f, -12f);
            b.Line(1.2f, BoltShade, V(20, 24), V(28, 17), V(22, 13), V(31, 6));
            b.Line(0.6f, Bolt, V(20, 24), V(28, 17), V(22, 13), V(31, 6));
            b.Shaded(26f, 36f, 12f, 11f, rock);
            b.White(23f, 35f, 9f, 7f);
            // ヒレは太ももに置いて踏ん張る
            b.Flipper(V(16, 26), V(9, 14), 3.5f, rockShade);
            b.TurnedFace(23f, 36f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            // 踏みしめた足元に火花を散らす
            b.Line(0.5f, Bolt, V(6, 4), V(3, 8));
            b.Line(0.5f, Bolt, V(30, 4), V(33, 8));
        }

        /// <summary>No.71 フブキオウペン: 誇り高い吹雪の王者。氷の大きな翼を前後に広げて急降下し、つららの冠と尾羽が吹雪に光る</summary>
        private static void Fubukiou(PenguinBrush b)
        {
            var body = Rgb(110, 160, 215);
            var bodyShade = PenguinPalette.Shade(body);
            Wing(b, V(38, 32), new[] { V(40, 28), V(52, 33), V(61, 44), V(60, 56), V(51, 61), V(43, 44) }, IceLight, IceShade, PenguinPalette.Belly);
            b.Triangle(V(27, 14), V(33, 14), V(28, 2), IceShade);
            b.Triangle(V(31, 14), V(37, 14), V(38, 3), IceLight);
            b.Foot(28f, 11f, 70f, 0.7f);
            b.ShadedTilted(32f, 23f, 9f, 12f, 15f, body);
            b.BellyTilted(30f, 22f, 5.5f, 8.5f, 15f);
            b.Triangle(V(22, 45), V(27, 45), V(22, 56), IceLight);
            b.Triangle(V(26, 46), V(31, 46), V(30, 58), IceShade);
            b.Triangle(V(30, 45), V(34, 44), V(37, 55), IceLight);
            b.Shaded(28f, 38f, 9.5f, 9f, body);
            b.White(26f, 37f, 7f, 5.5f);
            Wing(b, V(26, 28), new[] { V(28, 24), V(14, 20), V(3, 25), V(2, 37), V(7, 47), V(22, 38) }, IceLight, IceShade, PenguinPalette.Belly);
            b.TurnedFace(26f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
            b.Sparkle(10, 58, IceLight);
            b.Sparkle(52, 10, IceLight);
        }

        /// <summary>No.72 ウミドリペン: 自由気ままな渡り鳥。細長い翼をまっすぐ広げて大波の上すれすれを滑空する</summary>
        private static void Umidori(PenguinBrush b)
        {
            var gray = Rgb(165, 175, 192);
            var wing = Rgb(110, 120, 142);
            var wingShade = PenguinPalette.Shade(wing);
            var tip = Rgb(50, 55, 70);
            b.Polygon(V(32, 4), Water, V(2, 1), V(62, 1), V(62, 9), V(52, 13), V(42, 9), V(30, 13), V(18, 9), V(2, 12));
            b.Line(0.6f, PenguinPalette.Belly, V(18, 9), V(24, 12), V(30, 13));
            b.Line(0.6f, PenguinPalette.Belly, V(42, 9), V(47, 12), V(52, 13));
            Wing(b, V(38, 32), new[] { V(36, 34), V(46, 40), V(56, 48), V(62, 54), V(58, 48), V(42, 30) }, wing, wingShade, tip);
            b.Foot(52f, 25f, 20f, 0.7f);
            b.ShadedTilted(36f, 29f, 7.5f, 14f, 72f, gray);
            b.BellyTilted(35f, 27f, 4.5f, 10f, 72f);
            b.Shaded(20f, 33f, 9f, 8.5f, gray);
            b.White(18f, 32f, 6.5f, 5.5f);
            Wing(b, V(30, 34), new[] { V(32, 36), V(22, 44), V(10, 52), V(2, 56), V(8, 48), V(26, 32) }, wing, wingShade, tip);
            b.TurnedFace(18f, 33f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f);
        }

        /// <summary>No.73 ジュカイペン: 森を守る用心棒。葉っぱの大きな翼を上へ広げて威嚇し、つるの尾が後ろへうねる。頭に若葉の冠羽</summary>
        private static void Jukai(PenguinBrush b)
        {
            var green = Rgb(60, 130, 80);
            var greenShade = PenguinPalette.Shade(green);
            var leaf = Rgb(110, 190, 90);
            var leafShade = PenguinPalette.Shade(leaf);
            var leafTip = Rgb(200, 240, 150);
            b.Line(1.2f, leafShade, V(36, 10), V(44, 5), V(52, 8), V(60, 4));
            b.Tilted(53f, 11f, 3f, 1.5f, 30f, leaf);
            b.Tilted(45f, 2f, 3f, 1.5f, -20f, leaf);
            Wing(b, V(38, 30), new[] { V(40, 26), V(53, 29), V(61, 41), V(60, 53), V(51, 58), V(44, 42) }, leaf, leafShade, leafTip);
            b.Feet(32f, 6f);
            b.Shaded(32f, 20f, 11f, 12f, green);
            b.Belly(31f, 19f, 7f, 8.5f);
            b.Tilted(28f, 51f, 2.5f, 6f, 25f, leaf);
            b.Tilted(33f, 52f, 2.5f, 6f, -10f, leafShade);
            b.Shaded(31f, 38f, 9.5f, 9f, green);
            b.White(29f, 37f, 7f, 5.5f);
            Wing(b, V(25, 30), new[] { V(23, 26), V(11, 29), V(3, 41), V(4, 53), V(13, 58), V(20, 42) }, leaf, leafShade, leafTip);
            b.Flipper(V(40, 22), V(44, 14), 2f, greenShade);
            b.TurnedFace(29f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
        }
    }
}
