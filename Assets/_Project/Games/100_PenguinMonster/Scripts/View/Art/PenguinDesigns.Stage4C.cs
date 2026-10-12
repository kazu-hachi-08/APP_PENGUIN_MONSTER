using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>段階4のヒナ・エース・伝説（No.126〜150）のデザイン</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 HinaGrayFluff = Rgb(170, 172, 186);

        /// <summary>No.126 タマゴペン: のんびりした生まれたて。下半分の割れた殻に座ったまま顔を出し、殻のかけらを帽子にかぶる。ギザギザの殻のふちが横一文字に走るシルエット</summary>
        private static void Tamago(PenguinBrush b)
        {
            var shell = Rgb(245, 235, 205);
            var shellShade = PenguinPalette.Darken(shell, 0.82f);
            var fluffShade = PenguinPalette.Shade(HinaGrayFluff);
            b.Shaded(32f, 16f, 8.5f, 7f, HinaGrayFluff);
            // 小さなヒレは殻のふちの外へ出し、殻の中に体があるとわかるようにする
            b.Flipper(V(24, 15), V(16, 19), 2f, fluffShade);
            b.Flipper(V(40, 15), V(48, 19), 2f, fluffShade);
            // 殻のふちはギザギザにして、割れたばかりの卵だとひと目でわかるようにする
            var rim = new[] { V(44, 9), V(41, 3), V(32, 1), V(23, 3), V(20, 9), V(20, 14), V(23, 18), V(26, 14), V(29, 18), V(32, 14), V(35, 18), V(38, 14), V(41, 18), V(44, 14) };
            b.Polygon(V(32, 9), shellShade, rim);
            b.Polygon(V(31, 10), shell, V(41, 9), V(39, 4), V(32, 3), V(24, 4), V(22, 9), V(22, 13), V(23, 16), V(26, 12), V(29, 16), V(32, 12), V(35, 16), V(38, 12), V(41, 15));
            b.Ellipse(38f, 6f, 1.5f, 1.2f, shellShade);
            b.Ellipse(26f, 8f, 1.2f, 1f, shellShade);
            b.Shaded(31f, 25f, 8.5f, 7.5f, HinaGrayFluff);
            b.White(29f, 24f, 6f, 4.5f);
            b.TurnedFace(29f, 25f, FaceLeft, EyeStyle.Sparkle, eyeGap: 5f);
            // 殻のかけらの帽子は少し傾け、すき間からアホ毛を出す
            b.Polygon(V(31, 34), shellShade, V(23, 31), V(26, 29), V(29, 31), V(32, 29), V(35, 31), V(38, 30), V(37, 35), V(32, 38), V(26, 36));
            b.Polygon(V(31, 34), shell, V(24, 32), V(26, 30), V(29, 32), V(32, 30), V(35, 32), V(36, 32), V(35, 35), V(31, 37), V(26, 35));
            b.Line(0.9f, HinaGrayFluff, V(38, 34), V(41, 37), V(44, 36));
        }

        /// <summary>No.127 ギョウレツヒナペン: まじめで仲よし。4羽のヒナが斜めに一列で行進し、先頭が小さな旗を掲げる。左下から右上へ小さくなる階段状のシルエット</summary>
        private static void GyouretsuHina(PenguinBrush b)
        {
            var flag = Rgb(230, 60, 60);
            // 奥（右上）から順に描き、手前のヒナが奥のヒナに重なるようにする
            GyouretsuChick(b, 54f, 33f, 0.55f);
            GyouretsuChick(b, 45f, 25f, 0.68f);
            GyouretsuChick(b, 34f, 16f, 0.82f);
            b.Line(0.8f, Rgb(120, 90, 60), V(13, 14), V(11, 40));
            b.Triangle(V(11, 40), V(11, 33), V(3, 37), flag);
            b.Flipper(V(16, 12), V(12, 16), 2f, PenguinPalette.Shade(HinaGrayFluff));
            GyouretsuChick(b, 22f, 10f, 1f);
        }

        /// <summary>行列の1羽。scale で手前と奥の大きさを変える。目は小さくしても形が崩れないよう点目にする</summary>
        private static void GyouretsuChick(PenguinBrush b, float x, float y, float scale)
        {
            b.Foot(x - 2f * scale, y - 6f * scale, 0f, 0.5f * scale + 0.1f);
            b.Foot(x + 3f * scale, y - 6f * scale, 0f, 0.5f * scale + 0.1f);
            b.Shaded(x, y, 7f * scale, 6f * scale, HinaGrayFluff);
            b.Belly(x - 1.5f * scale, y - 0.5f * scale, 4.5f * scale, 4f * scale);
            float headY = y + 10f * scale;
            b.Shaded(x - 1f, headY, 6.5f * scale, 6f * scale, HinaGrayFluff);
            b.White(x - 2.5f * scale, headY - 0.5f, 4.5f * scale, 3.5f * scale);
            b.Line(0.8f, HinaGrayFluff, V(x, headY + 5f * scale), V(x + 2f * scale, headY + 9f * scale));
            b.TurnedFace(x - 2.5f * scale, headY, FaceLeft, EyeStyle.Dot, eyeGap: 4f);
        }

        /// <summary>No.128 ウキワヒナペン: のんきでおっとり。しましまの浮き輪に入ってぷかぷか浮かぶ。水の波紋と浮き輪で横に広い低いシルエット</summary>
        private static void UkiwaHina(PenguinBrush b)
        {
            var pink = Rgb(250, 130, 170);
            var water = Rgb(150, 210, 245);
            var fluffShade = PenguinPalette.Shade(HinaGrayFluff);
            b.Ellipse(32f, 8f, 28f, 6f, PenguinPalette.Shade(water));
            b.Ellipse(31f, 8.5f, 26f, 5f, water);
            b.Line(0.5f, PenguinPalette.Belly, V(6, 9), V(10, 10), V(14, 9));
            b.Line(0.5f, PenguinPalette.Belly, V(50, 9), V(54, 10), V(58, 9));
            // 浮き輪の奥半分 → 体 → 手前半分の順に塗り、輪の中に入っている奥行きを出す
            UkiwaRing(b, 0f, 180f, pink, true);
            b.Shaded(32f, 18f, 8f, 6.5f, HinaGrayFluff);
            b.Belly(31f, 17f, 5f, 4f);
            UkiwaRing(b, 180f, 360f, pink, false);
            b.Flipper(V(25, 20), V(17, 18), 2f, fluffShade);
            b.Flipper(V(39, 20), V(47, 18), 2f, fluffShade);
            b.Shaded(31f, 28f, 8.5f, 7.5f, HinaGrayFluff);
            b.White(29f, 27f, 6f, 4.5f);
            b.TurnedFace(29f, 28f, FaceLeft, EyeStyle.Sparkle, eyeGap: 5f);
            b.Line(0.9f, HinaGrayFluff, V(33, 35), V(31, 39), V(34, 41));
        }

        /// <summary>浮き輪の帯。ピンクと白を交互に塗ってしましまにする。back は輪の奥側（上半分）</summary>
        private static void UkiwaRing(PenguinBrush b, float fromDeg, float toDeg, Color32 pink, bool back)
        {
            const int Segments = 12;
            const float RadiusX = 15f;
            const float RadiusY = 6f;
            for (int i = 0; i < Segments; i++)
            {
                float from = Mathf.Lerp(fromDeg, toDeg, (float)i / Segments) * Mathf.Deg2Rad;
                float to = Mathf.Lerp(fromDeg, toDeg, (float)(i + 1) / Segments) * Mathf.Deg2Rad;
                var color = i % 2 == 0 ? pink : PenguinPalette.Belly;
                b.Line(back ? 2.5f : 3.5f, PenguinPalette.Darken(color, 0.85f), OnEllipse(32f, 14f, RadiusX, RadiusY, from), OnEllipse(32f, 14f, RadiusX, RadiusY, to));
                b.Line(back ? 1.8f : 2.6f, color, OnEllipse(31.5f, 14.5f, RadiusX, RadiusY, from), OnEllipse(31.5f, 14.5f, RadiusX, RadiusY, to));
            }
        }

        /// <summary>No.129 デンキュウヒナペン: 気まぐれ。電球の殻をヘルメットのようにかぶり、ガラスの中で頭とフィラメントが光る。片側だけチカチカ光が飛ぶ。上が大きく丸い電球のシルエット</summary>
        private static void DenkyuuHina(PenguinBrush b)
        {
            var glass = Rgb(255, 240, 150);
            var glassShade = Rgb(230, 200, 90);
            var metal = Rgb(170, 172, 180);
            var metalShade = PenguinPalette.Shade(metal);
            var filament = Rgb(255, 140, 40);
            var fluffShade = PenguinPalette.Shade(HinaGrayFluff);
            b.Foot(27f, 3f, -10f, 0.6f);
            b.Foot(37f, 3f, 10f, 0.6f);
            b.Shaded(32f, 10f, 8f, 7f, HinaGrayFluff);
            b.Belly(31f, 9f, 5f, 4.5f);
            b.Flipper(V(25, 11), V(19, 6), 2f, fluffShade);
            b.Flipper(V(39, 11), V(45, 6), 2f, fluffShade);
            // 口金は首輪のように巻き、ねじの段を交互の色で見せる
            b.Rect(25, 15, 14, 5, metalShade);
            b.Rect(25, 16, 14, 1, metal);
            b.Rect(25, 18, 14, 1, metal);
            b.Ellipse(32f, 32f, 12f, 12f, glassShade);
            b.Ellipse(31f, 33f, 10.5f, 10.5f, glass);
            b.Shaded(31f, 27f, 7.5f, 6.5f, HinaGrayFluff);
            b.White(29f, 26f, 5.5f, 4f);
            b.TurnedFace(29f, 27f, FaceLeft, EyeStyle.Sparkle, eyeGap: 5f);
            // アホ毛の先をフィラメントにつないで、頭の毛が光っているように見せる
            b.Line(0.8f, HinaGrayFluff, V(33, 33), V(32, 36));
            b.Line(0.6f, filament, V(27, 37), V(29, 40), V(31, 37), V(33, 40), V(35, 37));
            b.Rect(25, 39, 3, 1, PenguinPalette.Belly);
            b.Rect(24, 36, 1, 3, PenguinPalette.Belly);
            // 光は片側にだけ飛ばして、点いたり消えたりする気まぐれさを出す
            b.Line(0.6f, glass, V(14, 36), V(9, 38));
            b.Line(0.6f, glass, V(17, 44), V(13, 49));
            b.Line(0.6f, glass, V(26, 48), V(25, 54));
        }

        /// <summary>No.130 マッチヒナペン: びくびくした怖がり。マッチ棒のように細長い体で、赤い頭薬の頭のてっぺんに小さな炎が灯る。横のマッチ箱に寄りかかる。細い縦長のシルエット</summary>
        private static void MatchHina(PenguinBrush b)
        {
            var wood = Rgb(225, 185, 125);
            var head = Rgb(210, 60, 50);
            var box = Rgb(80, 120, 190);
            var strip = Rgb(120, 80, 60);
            b.Rect(39, 2, 18, 14, PenguinPalette.Shade(box));
            b.Rect(39, 3, 17, 12, box);
            b.Rect(39, 2, 18, 3, strip);
            b.Star(47f, 10f, 3.5f, 1.5f, Rgb(250, 220, 120));
            b.Foot(27f, 3f, -10f, 0.55f);
            b.Foot(34f, 3f, 10f, 0.55f);
            // 体はマッチの軸のように細く長くする
            b.ShadedTilted(31f, 14f, 4.5f, 11f, -4f, wood);
            b.Flipper(V(34, 18), V(40, 13), 1.6f, PenguinPalette.Shade(wood));
            b.Flipper(V(28, 18), V(24, 13), 1.6f, PenguinPalette.Shade(wood));
            b.Shaded(31f, 30f, 7f, 7.5f, head);
            b.White(29f, 29f, 5f, 4f);
            b.TurnedFace(29f, 30f, FaceLeft, EyeStyle.Worried, eyeGap: 5f);
            b.Line(0.8f, HinaGrayFluff, V(35, 36), V(38, 39), V(41, 38));
            // 炎はしずく形にして、とんがり帽子ではなく火だとわかるようにする
            b.Ellipse(31f, 39f, 3f, 3f, FlameOuter);
            b.Triangle(V(28, 39), V(34, 39), V(32, 48), FlameOuter);
            b.Ellipse(31f, 39f, 1.5f, 2f, FlameInner);
            // 汗をとばして、自分の炎におびえている顔にする
            if (!b.Front) return;
            b.Ellipse(21f, 33f, 1f, 1.6f, Rgb(120, 200, 250));
        }

        /// <summary>No.131 オバケヒナペン: いたずら好き。シーツのおばけの仮装でふわりと浮き、両そでを広げておどかす。すそが波打つ釣鐘形のシルエットで、顔はシーツの穴からのぞく目だけ</summary>
        private static void ObakeHina(PenguinBrush b)
        {
            var sheet = Rgb(248, 248, 252);
            var sheetShade = Rgb(200, 188, 230);
            var hole = Rgb(70, 60, 100);
            b.Foot(29f, 9f, -20f, 0.55f);
            b.Foot(36f, 9f, 20f, 0.55f);
            // そでを横へ広げ、おどかすポーズにする
            b.Polygon(V(22, 22), sheetShade, V(24, 28), V(14, 24), V(11, 18), V(16, 19), V(24, 16));
            b.Polygon(V(42, 22), sheetShade, V(40, 28), V(50, 24), V(53, 18), V(48, 19), V(40, 16));
            b.Ellipse(32f, 26f, 11f, 11f, sheetShade);
            b.Rect(21, 14, 23, 13, sheetShade);
            for (int x = 22; x <= 42; x += 5)
            {
                b.Ellipse(x, 13f, 2.6f, 2.6f, sheetShade);
            }
            b.Ellipse(31f, 27f, 9.5f, 9.5f, sheet);
            b.Rect(22, 15, 19, 12, sheet);
            for (int x = 22; x <= 37; x += 5)
            {
                b.Ellipse(x + 1f, 14f, 1.6f, 1.6f, sheet);
            }
            b.Line(0.9f, HinaGrayFluff, V(33, 36), V(35, 40), V(38, 39));
            ObakeHinaHoles(b, hole);
        }

        /// <summary>シーツの目の穴。顔はシーツの下なので、穴の中の点目とほっぺの染みだけで表情を出す</summary>
        private static void ObakeHinaHoles(PenguinBrush b, Color32 hole)
        {
            if (!b.Front) return;
            b.Ellipse(25f, 27f, 2.5f, 3f, hole);
            b.Ellipse(33f, 27f, 2.5f, 3f, hole);
            b.Eye(25f, 27f, EyeStyle.Dot, false);
            b.Eye(33f, 27f, EyeStyle.Dot, true);
            b.Dot(24, 28, PenguinPalette.EyeLight);
            b.Dot(32, 28, PenguinPalette.EyeLight);
            b.Cheek(22f, 22f);
            b.Cheek(36f, 22f);
        }

        /// <summary>No.132 フタバヒナペン: すなおで素朴。土の小山にちょこんと座り、頭から双葉の芽を伸ばして上からのしずくを受ける。頭の上に双葉が開くシルエット</summary>
        private static void FutabaHina(PenguinBrush b)
        {
            var soil = Rgb(140, 100, 70);
            var leaf = Rgb(120, 200, 90);
            var drop = Rgb(120, 200, 250);
            var fluffShade = PenguinPalette.Shade(HinaGrayFluff);
            b.Shaded(32f, 4f, 20f, 7f, soil);
            b.Dot(16, 6, PenguinPalette.Darken(soil, 0.6f));
            b.Dot(44, 4, PenguinPalette.Darken(soil, 0.6f));
            b.Shaded(32f, 15f, 8.5f, 6.5f, HinaGrayFluff);
            b.Belly(30f, 14f, 5.5f, 4.5f);
            // 足を前へ投げ出して、小山に腰を下ろしているのを見せる
            b.Foot(22f, 10f, 30f, 0.6f);
            b.Foot(27f, 9f, 15f, 0.6f);
            b.Flipper(V(38, 15), V(44, 11), 2f, fluffShade);
            b.Shaded(31f, 26f, 8.5f, 7.5f, HinaGrayFluff);
            b.White(29f, 25f, 6f, 4.5f);
            b.TurnedFace(29f, 26f, FaceLeft, EyeStyle.Sparkle, eyeGap: 5f);
            b.Line(0.9f, HinaGrayFluff, V(36, 32), V(39, 35), V(41, 34));
            // 双葉は綿毛より高く大きく開かせ、苔タイプの目印にする
            b.Line(0.8f, PenguinPalette.Shade(leaf), V(31, 32), V(31, 40));
            b.Tilted(26f, 41f, 5f, 2.5f, 25f, PenguinPalette.Shade(leaf));
            b.Tilted(25.5f, 41.5f, 4f, 1.6f, 25f, leaf);
            b.Tilted(36f, 42f, 5f, 2.5f, -25f, PenguinPalette.Shade(leaf));
            b.Tilted(35.5f, 42.5f, 4f, 1.6f, -25f, leaf);
            foreach (var p in new[] { V(30, 50), V(34, 56), V(26, 58) })
            {
                b.Ellipse(p.x, p.y, 1.2f, 1.6f, drop);
                b.Dot((int)p.x, (int)p.y + 2, drop);
            }
        }

        /// <summary>No.133 ムテッポウヒナペン: 向こう見ずな無鉄砲。段ボールの手作りの翼を背中にくくり、ゴーグルを額に上げて崖のふちから前へ飛び出す瞬間。前のめりの体と後ろへ立つ四角い翼のシルエット</summary>
        private static void MuteppouHina(PenguinBrush b)
        {
            var cardboard = Rgb(200, 150, 95);
            var cardboardShade = PenguinPalette.Shade(cardboard);
            var string_ = Rgb(240, 235, 220);
            var lens = Rgb(150, 220, 250);
            var strap = Rgb(80, 70, 70);
            var fluffShade = PenguinPalette.Shade(HinaGrayFluff);
            // 崖は右下の角を埋める岩にし、左の面を切り立たせて「ふち」に見せる
            b.Polygon(V(52, 6), PenguinPalette.Shade(Stone), V(39, 14), V(46, 17), V(54, 15), V(62, 18), V(62, 0), V(41, 0), V(42, 7));
            b.Polygon(V(52, 8), Stone, V(41, 13), V(46, 16), V(54, 14), V(61, 16), V(61, 4), V(44, 4), V(43, 8));
            b.Line(0.5f, PenguinPalette.Shade(Stone), V(48, 11), V(51, 7), V(50, 3));
            b.Line(0.5f, PenguinPalette.Shade(Stone), V(56, 12), V(58, 6));
            // 翼は羽の形に切り抜いた段ボールの板を2枚、上と後ろへ開く。下のふちのギザギザは はさみで切った羽の先
            b.Polygon(V(44, 36), PenguinPalette.Darken(cardboardShade, 0.85f), V(36, 26), V(40, 40), V(46, 52), V(50, 50), V(48, 44), V(52, 44), V(48, 36), V(52, 34), V(44, 24));
            b.Polygon(V(48, 28), cardboardShade, V(38, 30), V(52, 34), V(62, 36), V(61, 31), V(57, 32), V(57, 27), V(52, 28), V(51, 23), V(46, 24), V(40, 22));
            b.Polygon(V(48, 29), cardboard, V(40, 30), V(52, 33), V(60, 34), V(59, 33), V(55, 33), V(55, 29), V(50, 29), V(49, 25), V(45, 25), V(41, 24));
            b.Line(0.5f, cardboardShade, V(42, 28), V(56, 32));
            b.Foot(41f, 16f, -30f, 0.6f);
            b.Line(1.5f, fluffShade, V(37, 15), V(41, 16));
            b.Foot(28f, 9f, 20f, 0.6f);
            // 体を左へ大きく傾け、崖から身を投げ出した瞬間の重心にする
            b.ShadedTilted(32f, 18f, 8f, 7f, 30f, HinaGrayFluff);
            b.BellyTilted(30f, 17f, 5f, 4.5f, 30f);
            // 胴に巻いたひもで、背中にくくりつけてあるのを見せる
            b.Line(0.6f, string_, V(27, 22), V(38, 18));
            b.Line(0.6f, string_, V(28, 14), V(38, 22));
            b.Flipper(V(26, 20), V(15, 26), 2f, fluffShade);
            b.Shaded(24f, 28f, 8f, 7.5f, HinaGrayFluff);
            b.White(22f, 27f, 5.5f, 4.5f);
            b.TurnedFace(22f, 28f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f, open: true);
            b.Line(1f, strap, V(17, 33), V(31, 33));
            b.Ellipse(20f, 34f, 2.5f, 2f, strap);
            b.Ellipse(26f, 34f, 2.5f, 2f, strap);
            b.Ellipse(20f, 34.5f, 1.5f, 1.2f, lens);
            b.Ellipse(26f, 34.5f, 1.5f, 1.2f, lens);
            b.Line(0.9f, HinaGrayFluff, V(27, 36), V(30, 40), V(33, 39));
        }

        /// <summary>No.134 ユキダマヒナペン: うっかり屋。坂を転げ落ちて雪玉に丸ごと包まれ、頭と足だけ出して転がる。まん丸の大きな玉のシルエット</summary>
        private static void YukidamaHina(PenguinBrush b)
        {
            var snow = Rgb(245, 248, 255);
            var snowShade = Rgb(185, 205, 228);
            b.Line(0.6f, snowShade, V(52, 8), V(60, 8));
            b.Line(0.6f, snowShade, V(54, 14), V(62, 14));
            b.Line(0.6f, snowShade, V(53, 20), V(59, 20));
            // 足は玉の上から空へ突き出し、転がっている途中だとわかるようにする
            b.Foot(40f, 33f, 60f, 0.6f);
            b.Foot(45f, 30f, 30f, 0.6f);
            b.Ellipse(33f, 17f, 15f, 15f, snowShade);
            b.Ellipse(31.5f, 18.5f, 13f, 13f, snow);
            b.Shaded(42f, 10f, 2.2f, 1.8f, Stone);
            b.Shaded(36f, 26f, 1.8f, 1.5f, Stone);
            b.Shaded(26f, 9f, 2f, 1.6f, Stone);
            b.Shaded(44f, 21f, 1.5f, 1.3f, Stone);
            b.Shaded(20f, 26f, 7.5f, 7f, HinaGrayFluff);
            b.White(18f, 25f, 5.5f, 4.5f);
            b.TurnedFace(18f, 26f, FaceLeft, EyeStyle.Worried, eyeGap: 5f);
            b.Line(0.9f, HinaGrayFluff, V(21, 32), V(19, 36), V(22, 38));
            b.Ellipse(14f, 31f, 2.5f, 1.5f, snow);
        }

        /// <summary>No.135 ホシクズヒナペン: 夢見がち。濃紺の綿毛に星くずがきらめき、流れ星のように左下へ飛んでくる。後ろへ長く伸びる尾で斜めのシルエット</summary>
        private static void HoshikuzuHina(PenguinBrush b)
        {
            var fluff = Rgb(50, 58, 120);
            var fluffShade = PenguinPalette.Shade(fluff);
            var star = Rgb(255, 240, 170);
            var tailPurple = Rgb(170, 150, 230);
            // 尾は体から右上の角へ細くなる帯にして、流れ星の動きを形で見せる
            b.Triangle(V(30, 30), V(40, 14), V(60, 56), tailPurple);
            b.Triangle(V(33, 26), V(38, 18), V(56, 50), star);
            b.Line(0.5f, PenguinPalette.Belly, V(37, 22), V(52, 44));
            b.Foot(38f, 9f, -40f, 0.6f);
            b.Foot(42f, 13f, -60f, 0.6f);
            b.ShadedTilted(33f, 18f, 8f, 7f, 30f, fluff);
            b.BellyTilted(31f, 17f, 5f, 4.5f, 30f);
            b.Flipper(V(37, 22), V(44, 27), 2f, fluffShade);
            b.Shaded(26f, 27f, 8.5f, 7.5f, fluff);
            b.White(24f, 26f, 6f, 4.5f);
            b.TurnedFace(24f, 27f, FaceLeft, EyeStyle.Cross, eyeGap: 5f);
            // アホ毛の先を星にして、星から生まれた子に見せる
            b.Line(0.9f, fluff, V(28, 34), V(26, 38), V(29, 41));
            b.Star(29f, 42f, 3.5f, 1.5f, star);
            foreach (var p in new[] { V(36, 18), V(31, 13), V(38, 13), V(33, 31), V(19, 31) })
            {
                b.Dot((int)p.x, (int)p.y, star);
            }
            b.Sparkle(10, 40, star);
            b.Sparkle(14, 10, star);
        }

        /// <summary>No.136 ライセツペン: 冷たく荒々しい。宙に浮き、肩から背中へ雷雲と雪のマントを大きく広げ、その中で稲妻が走る。右上へふくらむ雲のかたまりのシルエット</summary>
        private static void Raisetsu(PenguinBrush b)
        {
            var body = Rgb(150, 200, 235);
            var bodyShade = PenguinPalette.Shade(body);
            var cloud = Rgb(130, 135, 165);
            // 雲のマントは背中側（右上）に積み上げ、体より大きなかたまりにする
            var puffs = new[] { V(36, 32), V(34, 46), V(42, 40), V(46, 52), V(55, 47), V(57, 37), V(52, 27), V(56, 17), V(46, 22), V(40, 56) };
            foreach (var p in puffs)
            {
                b.Ellipse(p.x, p.y, 7f, 6.5f, PenguinPalette.Shade(cloud));
            }
            foreach (var p in puffs)
            {
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 5.5f, 5f, cloud);
            }
            var bolt = new[] { V(48, 58), V(52, 46), V(46, 40), V(54, 30), V(50, 20) };
            b.Line(1.4f, BoltShade, bolt);
            b.Line(0.7f, Bolt, bolt);
            foreach (var p in new[] { V(40, 50), V(58, 42), V(44, 28), V(58, 24), V(36, 58), V(54, 54) })
            {
                b.Sparkle((int)p.x, (int)p.y, PenguinPalette.Belly);
            }
            // 足は下へだらりと垂らし、風に乗って浮いているのを見せる
            b.Foot(24f, 7f, 80f, 0.8f);
            b.Foot(31f, 6f, 100f, 0.8f);
            b.ShadedTilted(29f, 21f, 11f, 12f, 8f, body);
            b.BellyTilted(26f, 20f, 7f, 8.5f, 8f);
            b.Flipper(V(18, 24), V(6, 30), 3f, bodyShade);
            b.Shaded(26f, 38f, 10f, 9.5f, body);
            b.White(24f, 37f, 7.5f, 6f);
            b.TurnedFace(24f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            // 体のまわりに吹雪の筋を流し、雪も運ぶ嵐だとわかるようにする
            b.Line(0.5f, IceLight, V(4, 16), V(12, 14));
            b.Line(0.5f, IceLight, V(8, 44), V(14, 46));
            b.Line(0.5f, IceLight, V(40, 8), V(48, 6));
        }

        /// <summary>No.137 クジラノリペン: 陽気な冒険家。大きな青いクジラの背中に乗ってヒレを掲げる。クジラが下半分を占め、右に尾びれが跳ね上がり、潮吹きが上がる横長のシルエット</summary>
        private static void Kujiranori(PenguinBrush b)
        {
            var whale = Rgb(70, 120, 200);
            var whaleShade = PenguinPalette.Shade(whale);
            var whaleBelly = Rgb(225, 235, 245);
            var body = Rgb(40, 55, 110);
            var bodyShade = PenguinPalette.Shade(body);
            // 尾びれは体の後ろから上へ跳ね上げ、クジラが泳いでいる勢いを出す
            b.Line(4f, whaleShade, V(46, 10), V(54, 18), V(57, 26));
            b.Polygon(V(57, 30), whaleShade, V(55, 26), V(48, 36), V(52, 37), V(57, 33), V(62, 38), V(63, 34), V(60, 26));
            b.Polygon(V(57, 30), whale, V(56, 27), V(50, 35), V(57, 31), V(61, 34), V(59, 27));
            // クジラのおなかの白は後ろ姿でも見えるので、White ではなく普通の色で塗る
            b.Ellipse(28f, 12f, 25f, 10f, whaleShade);
            b.Ellipse(27f, 13f, 23.5f, 8.5f, whale);
            b.Ellipse(24f, 6f, 20f, 4f, whaleBelly);
            for (int x = 10; x <= 36; x += 6)
            {
                b.Line(0.5f, PenguinPalette.BellyShade, V(x, 4), V(x + 3, 4));
            }
            b.Line(0.6f, whaleShade, V(4, 10), V(14, 9));
            b.Rect(10, 13, 2, 2, PenguinPalette.Eye);
            b.Shaded(16f, 9f, 3f, 2f, whale);
            // 潮吹きは頭の上へ扇のように広げ、背の高いシルエットのアクセントにする
            var spout = Rgb(170, 220, 250);
            b.Line(1.2f, spout, V(14, 22), V(13, 38));
            b.Line(0.8f, spout, V(13, 36), V(6, 44), V(4, 40));
            b.Line(0.8f, spout, V(13, 36), V(19, 45), V(22, 42));
            b.Ellipse(13f, 40f, 3f, 2.5f, spout);
            b.Ellipse(5f, 38f, 1f, 1.2f, spout);
            b.Ellipse(22f, 40f, 1f, 1.2f, spout);
            b.Foot(30f, 22f, 0f, 0.7f);
            b.Foot(37f, 22f, 0f, 0.7f);
            b.Shaded(34f, 29f, 8f, 7f, body);
            b.Belly(32f, 28f, 5f, 4.5f);
            b.Flipper(V(40, 32), V(47, 46), 2.5f, bodyShade);
            b.Shaded(32f, 40f, 8.5f, 8f, body);
            b.White(30f, 39f, 6f, 5f);
            b.TurnedFace(30f, 40f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
        }

        /// <summary>No.138 スイショウペン: どっしり構えた石頭。背中と頭から六角柱の水晶がいくつも突き出し、体にもきらめく面がある。右上へとげが放射するシルエット</summary>
        private static void Suishou(PenguinBrush b)
        {
            var body = Rgb(110, 100, 150);
            var bodyShade = PenguinPalette.Shade(body);
            var purple = Rgb(190, 140, 240);
            var cyan = Rgb(140, 220, 245);
            // 背中の結晶は扇形に並べ、どの向きから見ても鉱石の山に見えるようにする
            SuishouCrystal(b, V(44, 30), V(61, 50), 4.5f, purple);
            SuishouCrystal(b, V(46, 22), V(62, 28), 3.5f, cyan);
            SuishouCrystal(b, V(40, 38), V(46, 61), 4f, cyan);
            SuishouCrystal(b, V(46, 14), V(60, 9), 3f, purple);
            b.Foot(20f, 4f, 0f, 1.1f);
            b.Foot(38f, 4f, -10f, 1.1f);
            b.Shaded(29f, 18f, 17f, 14f, body);
            b.Belly(25f, 17f, 11f, 10f);
            b.Flipper(V(13, 22), V(5, 10), 3.5f, bodyShade);
            b.Shaded(27f, 37f, 11f, 10.5f, body);
            b.White(24f, 36f, 8f, 6.5f);
            SuishouCrystal(b, V(28, 44), V(24, 58), 3f, purple);
            SuishouCrystal(b, V(33, 44), V(36, 55), 2.5f, cyan);
            b.TurnedFace(24f, 37f, FaceLeft, EyeStyle.Cross, eyeGap: 6f);
            // 体の面のきらめきで、肌まで鉱物になりかけているのを見せる
            b.Triangle(V(40, 16), V(44, 22), V(44, 12), cyan);
            b.Triangle(V(14, 14), V(18, 18), V(14, 20), purple);
            b.Sparkle(8, 40, PenguinPalette.Belly);
            b.Sparkle(54, 60, PenguinPalette.Belly);
        }

        /// <summary>六角柱の結晶。根元から先へ伸ばし、先をとがらせる。片側を明るく塗って柱の面を見せる</summary>
        private static void SuishouCrystal(PenguinBrush b, Vector2 root, Vector2 tip, float width, Color32 color)
        {
            var along = (tip - root).normalized;
            var side = new Vector2(-along.y, along.x) * width;
            var shoulder = tip - along * width * 1.5f;
            var dark = PenguinPalette.Shade(color);
            b.Triangle(root + side, root - side, shoulder - side, dark);
            b.Triangle(root + side, shoulder + side, shoulder - side, dark);
            b.Triangle(shoulder + side, shoulder - side, tip, dark);
            b.Triangle(root + side, root, shoulder, color);
            b.Triangle(root + side, shoulder + side, shoulder, color);
            b.Triangle(shoulder + side, shoulder, tip, color);
            b.Line(0.5f, PenguinPalette.Belly, root + side * 0.5f, shoulder + side * 0.5f);
        }

        /// <summary>No.139 プラズマペン: 激しやすい。オレンジから紫に光るプラズマの体で、ガラスのプラズマボールを両ヒレで頭上に掲げる。周りを火と火花の輪が回る。上に玉が乗った縦長のシルエット</summary>
        private static void Plasma(PenguinBrush b)
        {
            var body = Rgb(245, 140, 60);
            var glow = Rgb(150, 70, 190);
            var glass = Rgb(210, 180, 255);
            NightArc(b, FlameOuter, 32f, 20f, 28f, 8f, 0f, 180f);
            // 玉は頭の上へ掲げ、ガラスの中で稲妻が放射するのを見せる
            b.Ellipse(32f, 51f, 10f, 10f, glow);
            b.Ellipse(31f, 52f, 8.5f, 8.5f, glass);
            foreach (var end in new[] { V(25, 56), V(37, 58), V(39, 48), V(25, 46), V(31, 60) })
            {
                b.Line(0.5f, PenguinPalette.Belly, V(31, 52), V(31 + (end.x - 31) * 0.5f + 1, 52 + (end.y - 52) * 0.5f), end);
            }
            b.Ellipse(31f, 52f, 2f, 2f, PenguinPalette.Belly);
            b.Foot(22f, 4f, 0f, 1f);
            b.Foot(40f, 4f, -10f, 1f);
            // 影色を紫にして、オレンジの体の中で別の色の光がゆらいでいるように見せる
            b.Ellipse(31f, 17f, 14f, 13f, glow);
            b.Ellipse(30f, 18f, 12.5f, 11.5f, body);
            b.Belly(28f, 16f, 9f, 8f);
            b.Flipper(V(20, 26), V(24, 44), 3f, glow);
            b.Flipper(V(42, 26), V(39, 44), 3f, glow);
            b.Ellipse(30f, 33f, 10f, 9.5f, glow);
            b.Ellipse(29f, 34f, 8.5f, 8f, body);
            b.White(27f, 33f, 7f, 5.5f);
            b.TurnedFace(27f, 34f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
            NightArc(b, FlameMiddle, 32f, 20f, 28f, 8f, 185f, 355f);
            b.Sparkle(6, 30, FlameInner);
            b.Sparkle(58, 34, FlameInner);
            b.Sparkle(12, 48, PenguinPalette.Belly);
            b.Sparkle(52, 52, PenguinPalette.Belly);
        }

        /// <summary>No.140 オニペン: 乱暴で豪快。赤い体に2本の角、虎じまのパンツで、トゲの金棒を頭の上へ振りかぶって吠える。右上へ長く突き出た金棒のシルエット</summary>
        private static void Oni(PenguinBrush b)
        {
            var body = Rgb(210, 60, 50);
            var bodyShade = PenguinPalette.Shade(body);
            var tiger = Rgb(250, 205, 60);
            var horn = Rgb(245, 230, 190);
            var iron = Rgb(140, 145, 160);
            // 金棒は体の後ろから右上の角まで伸ばし、先ほど太くして重さを出す
            b.Line(2f, PenguinPalette.Shade(iron), V(38, 30), V(48, 46));
            b.Line(4.5f, PenguinPalette.Shade(iron), V(46, 43), V(58, 60));
            b.Line(3.5f, iron, V(46, 44), V(57, 59));
            foreach (var p in new[] { V(48, 50), V(52, 54), V(55, 58), V(51, 47), V(54, 51) })
            {
                b.Rect((int)p.x - 2, (int)p.y, 1, 1, PenguinPalette.Belly);
                b.Rect((int)p.x + 2, (int)p.y - 1, 1, 1, PenguinPalette.Belly);
            }
            b.Foot(18f, 4f, 10f, 1.1f);
            b.Foot(38f, 4f, -10f, 1.1f);
            b.Shaded(29f, 17f, 15f, 13f, body);
            b.Belly(26f, 18f, 9f, 8f);
            // 虎じまのパンツは腰回りに巻き、黒い斜めのしまで鬼だとわかるようにする
            b.Ellipse(29f, 9f, 14.5f, 6f, PenguinPalette.Shade(tiger));
            b.Ellipse(28.5f, 9.5f, 13.5f, 5f, tiger);
            foreach (var x in new[] { 18, 24, 30, 36 })
            {
                b.Line(0.7f, PenguinPalette.Eye, V(x, 5), V(x + 3, 13));
            }
            b.Flipper(V(38, 24), V(41, 33), 3f, bodyShade);
            b.Flipper(V(16, 22), V(6, 16), 3f, bodyShade);
            b.Triangle(V(19, 44), V(24, 45), V(15, 56), PenguinPalette.Shade(horn));
            b.Triangle(V(20, 44), V(23, 45), V(16, 54), horn);
            b.Triangle(V(30, 45), V(35, 44), V(36, 57), PenguinPalette.Shade(horn));
            b.Triangle(V(31, 45), V(34, 44), V(35, 55), horn);
            b.Shaded(27f, 35f, 11f, 10.5f, body);
            b.White(24f, 34f, 8f, 6.5f);
            b.TurnedFace(24f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
        }

        /// <summary>No.141 ニンジャペン: 寡黙で素早い。黒い頭巾で目元だけ出し、低く前傾して駆けながら手裏剣を構える。赤いマフラーが後ろへ長くなびく、低く横長のシルエット</summary>
        private static void Ninja(PenguinBrush b)
        {
            var body = Rgb(45, 45, 62);
            var bodyShade = PenguinPalette.Shade(body);
            var scarf = Rgb(220, 50, 50);
            var smoke = Rgb(200, 200, 210);
            b.Ellipse(55f, 5f, 4f, 3f, smoke);
            b.Ellipse(60f, 10f, 2.5f, 2f, smoke);
            b.Ellipse(49f, 3f, 2.5f, 2f, smoke);
            // マフラーは首から右の端まで波打たせて流し、走る速さを形で見せる
            var tail = new[] { V(30, 31), V(40, 36), V(48, 33), V(56, 38), V(62, 34) };
            b.Line(2.2f, PenguinPalette.Shade(scarf), tail);
            b.Line(1.4f, scarf, tail);
            b.Line(1.4f, scarf, V(40, 33), V(50, 28), V(58, 28));
            b.Foot(44f, 9f, -40f, 1f);
            b.Foot(22f, 4f, 10f, 1f);
            // 体を大きく前へ倒し、頭が足より前に出る低い駆け足にする
            b.ShadedTilted(34f, 17f, 10f, 13f, 55f, body);
            b.BellyTilted(31f, 14f, 5.5f, 9f, 55f);
            b.Flipper(V(38, 22), V(50, 18), 3f, bodyShade);
            b.Shaded(20f, 26f, 10f, 9f, body);
            b.Line(3f, scarf, V(17, 20), V(29, 23));
            // 頭巾から出るのは目元の細い帯だけ。くちばしとほっぺはそのすき間からのぞかせる
            if (b.Front) b.Ellipse(17f, 27f, 6.5f, 3.5f, PenguinPalette.Belly);
            b.TurnedFace(17f, 28f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            b.Line(1.2f, bodyShade, V(27, 33), V(32, 35), V(35, 32));
            b.Flipper(V(16, 18), V(7, 16), 2.5f, bodyShade);
            NinjaShuriken(b, 5f, 17f);
        }

        /// <summary>4枚刃の手裏剣。真ん中に穴を残して、ただの星ではなく刃物に見せる</summary>
        private static void NinjaShuriken(PenguinBrush b, float x, float y)
        {
            var steel = Rgb(180, 185, 200);
            b.Triangle(V(x - 1.5f, y), V(x + 1.5f, y), V(x, y + 4.5f), steel);
            b.Triangle(V(x - 1.5f, y), V(x + 1.5f, y), V(x, y - 4.5f), steel);
            b.Triangle(V(x, y - 1.5f), V(x, y + 1.5f), V(x - 4.5f, y), steel);
            b.Triangle(V(x, y - 1.5f), V(x, y + 1.5f), V(x + 4.5f, y), steel);
            b.Dot((int)x, (int)y, PenguinPalette.Shade(steel));
        }

        /// <summary>No.142 ヤミイナズマペン: 荒々しい暴れ者。黒い雷雲の体に紫の稲妻が走り、頭に稲妻の角。前かがみで両ヒレを広げてほえる。もくもくのふちと角のとがったシルエット</summary>
        private static void YamiInazuma(PenguinBrush b)
        {
            var cloud = Rgb(50, 46, 70);
            var cloudShade = PenguinPalette.Shade(cloud);
            var purple = Rgb(190, 110, 250);
            var purpleShade = PenguinPalette.Shade(purple);
            // 角は稲妻の形にして、頭の後ろへ大きく反らせる
            YamiBolt(b, purpleShade, purple, V(26, 42), V(20, 50), V(26, 51), V(16, 62));
            YamiBolt(b, purpleShade, purple, V(34, 42), V(38, 50), V(34, 51), V(44, 61));
            b.Foot(18f, 4f, 10f, 1.1f);
            b.Foot(40f, 4f, -10f, 1.1f);
            // 体のふちに雲のふくらみを並べ、輪郭そのものを雷雲にする
            foreach (var p in new[] { V(14, 18), V(16, 28), V(44, 14), V(46, 26), V(40, 34), V(22, 8), V(38, 7) })
            {
                b.Ellipse(p.x, p.y, 6f, 5.5f, cloudShade);
            }
            b.Ellipse(30f, 19f, 16f, 14f, cloudShade);
            b.Ellipse(29f, 20f, 14.5f, 12.5f, cloud);
            b.Belly(26f, 18f, 9f, 9f);
            YamiBolt(b, purpleShade, purple, V(40, 30), V(36, 22), V(42, 18), V(37, 8));
            b.Flipper(V(16, 24), V(3, 32), 3.5f, cloudShade);
            b.Flipper(V(44, 24), V(57, 34), 3.5f, cloudShade);
            b.Line(0.6f, purple, V(3, 34), V(1, 38));
            b.Line(0.6f, purple, V(58, 36), V(61, 40));
            b.Shaded(27f, 35f, 11f, 10f, cloud);
            b.White(24f, 34f, 8f, 6.5f);
            // シャチのような白い模様は目の後ろに付け、後ろ姿でも黒白の荒々しい柄が見えるようにする
            b.Tilted(34f, 38f, 3f, 1.8f, 20f, PenguinPalette.Belly);
            b.TurnedFace(24f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
        }

        /// <summary>稲妻の筋。影色の太線に明るい細線を重ねて光らせる</summary>
        private static void YamiBolt(PenguinBrush b, Color32 shade, Color32 light, params Vector2[] points)
        {
            b.Line(1.6f, shade, points);
            b.Line(0.8f, light, points);
        }

        /// <summary>No.143 ジュヒョウペン: 眠たげで動じない。雪に覆われたモミの木（樹氷）の姿で、凸凹の白い段からところどころ緑がのぞき、風下へ氷のトゲが伸びる。中のうろから顔がのぞく三角のシルエット</summary>
        private static void Juhyou(PenguinBrush b)
        {
            var snow = Rgb(240, 245, 252);
            var snowShade = Rgb(175, 200, 225);
            var fir = Rgb(60, 120, 80);
            // 氷のトゲは風下（右）へ流し、吹きつける吹雪の向きをシルエットで見せる
            foreach (var p in new[] { V(50, 10), V(46, 24), V(42, 38), V(38, 50) })
            {
                b.Triangle(V(p.x - 2, p.y + 2), V(p.x - 2, p.y - 2), V(p.x + 12, p.y + 1), IceShade);
                b.Triangle(V(p.x - 2, p.y + 2), V(p.x - 2, p.y), V(p.x + 10, p.y + 1), IceLight);
            }
            b.Foot(24f, 3f, 0f, 1f);
            b.Foot(38f, 3f, -10f, 1f);
            JuhyouTier(b, 32f, 6f, 25f, 14f, fir, snow, snowShade);
            JuhyouTier(b, 32f, 19f, 21f, 14f, fir, snow, snowShade);
            JuhyouTier(b, 32f, 32f, 16f, 15f, fir, snow, snowShade);
            JuhyouTier(b, 32f, 45f, 10f, 15f, fir, snow, snowShade);
            // 雪のすき間から枝の緑をのぞかせ、ただの雪山ではなく木だとわかるようにする
            foreach (var p in new[] { V(42, 14), V(16, 13), V(38, 28), V(36, 41), V(26, 39), V(30, 52), V(46, 26) })
            {
                b.Tilted(p.x, p.y, 2.2f, 1.2f, -20f, fir);
            }
            // 顔は白い雪の中でも見えるよう、木の暗いうろの中に置く
            if (b.Front) b.Ellipse(28f, 22f, 9f, 7f, PenguinPalette.Darken(fir, 0.6f));
            b.White(27f, 22f, 7f, 5f);
            b.TurnedFace(27f, 23f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
        }

        /// <summary>樹氷の1段。垂れた枝の緑の上に雪の帽子をかぶせ、すそのこぶの間から緑をのぞかせる</summary>
        private static void JuhyouTier(PenguinBrush b, float x, float bottom, float halfWidth, float height, Color32 fir, Color32 snow, Color32 snowShade)
        {
            b.Polygon(V(x, bottom + height * 0.4f), fir, V(x - halfWidth, bottom - 1f), V(x - halfWidth * 0.5f, bottom + 1f), V(x, bottom - 1f),
                V(x + halfWidth * 0.5f, bottom + 1f), V(x + halfWidth, bottom - 1f), V(x, bottom + height));
            b.Triangle(V(x - halfWidth + 1f, bottom + 3f), V(x + halfWidth - 1f, bottom + 3f), V(x, bottom + height + 1f), snowShade);
            b.Triangle(V(x - halfWidth + 2f, bottom + 4f), V(x + halfWidth - 4f, bottom + 4f), V(x - 1f, bottom + height), snow);
            for (float px = x - halfWidth + 3f; px < x + halfWidth - 1f; px += 5f)
            {
                b.Ellipse(px, bottom + 3.5f, 2.6f, 2.2f, snowShade);
                b.Ellipse(px - 0.5f, bottom + 4f, 1.8f, 1.5f, snow);
            }
        }

        /// <summary>No.144 ヨコヅナペン: 堂々とした大関の上。まん丸の巨体に黒いまわしと白い綱（紙垂付き）、頭にちょんまげ。前の足を横へ高く上げる四股で、丸に太い脚が突き出すシルエット</summary>
        private static void Yokozuna(PenguinBrush b)
        {
            var body = Rgb(55, 65, 120);
            var bodyShade = PenguinPalette.Shade(body);
            var mawashi = Rgb(35, 35, 45);
            var rope = Rgb(245, 245, 240);
            // 上げる足は前（左）。ライガンペン（後ろ足を上げる）と向きを逆にして見分けやすくする
            b.Foot(44f, 4f, -10f, 1.2f);
            b.Line(4f, bodyShade, V(42, 10), V(44, 5));
            b.Shaded(34f, 24f, 17f, 17f, body);
            b.Belly(31f, 25f, 11.5f, 11f);
            // 足は胴より外まで高く上げ、四股の踏み込み前の溜めを見せる
            b.Line(4.5f, bodyShade, V(24, 12), V(12, 20), V(6, 30));
            b.Foot(5f, 34f, 80f, 1.1f);
            b.Ellipse(34f, 11f, 16f, 5f, mawashi);
            b.Rect(19, 7, 31, 5, mawashi);
            b.Line(1.6f, rope, V(18, 16), V(26, 14), V(34, 14), V(43, 15), V(50, 17));
            foreach (var x in new[] { 22, 30, 38, 46 })
            {
                b.Line(0.6f, rope, V(x, 14), V(x + 2, 11), V(x, 9), V(x + 2, 6));
            }
            b.Flipper(V(48, 26), V(57, 18), 3.5f, bodyShade);
            b.Flipper(V(20, 26), V(13, 22), 3f, bodyShade);
            b.Shaded(30f, 45f, 10.5f, 10f, body);
            b.White(27f, 44f, 7.5f, 6.5f);
            b.TurnedFace(27f, 45f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            // ちょんまげは頭のてっぺんから前へ倒し、力士だとひと目でわかるようにする
            b.Ellipse(34f, 55f, 4f, 2.5f, mawashi);
            b.Line(1.5f, mawashi, V(34, 56), V(27, 58));
            b.Rect(32, 55, 2, 1, rope);
        }

        /// <summary>No.145 ユーフォーペン: ひょうきんな宇宙好き。空飛ぶ円盤のガラスのドームに乗り、ふちのライトを光らせて光線を下へ放つ。横に平たい円盤と三角の光線のシルエット</summary>
        private static void Ufo(PenguinBrush b)
        {
            var silver = Rgb(195, 200, 215);
            var silverShade = PenguinPalette.Shade(silver);
            var glass = Rgb(200, 235, 250);
            var body = Rgb(120, 90, 180);
            var beam = Rgb(255, 245, 180);
            // 光線は円盤より細い根元から広げ、地面を照らすスポットライトにする
            b.Polygon(V(32, 10), beam, V(26, 20), V(38, 20), V(48, 2), V(16, 2));
            b.Line(0.5f, PenguinPalette.Belly, V(28, 16), V(24, 4));
            b.Line(0.5f, PenguinPalette.Belly, V(37, 16), V(41, 6));
            b.Ellipse(32f, 32f, 12f, 12f, PenguinPalette.Shade(glass));
            b.Ellipse(31f, 33f, 11f, 11f, glass);
            b.Shaded(32f, 30f, 7.5f, 7f, body);
            b.Belly(31f, 28f, 4.5f, 3f);
            b.Flipper(V(37, 32), V(42, 38), 2f, PenguinPalette.Shade(body));
            b.Shaded(31f, 37f, 7.5f, 6.5f, body);
            b.White(29f, 36f, 5.5f, 4.5f);
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Cross, eyeGap: 5f, open: true);
            b.Line(0.6f, PenguinPalette.Belly, V(36, 42), V(39, 39));
            b.Ellipse(32f, 21f, 29f, 7f, silverShade);
            b.Ellipse(31f, 22f, 27.5f, 5.5f, silver);
            b.Ellipse(32f, 17f, 16f, 3f, silverShade);
            var lights = new[] { Rgb(255, 90, 90), Rgb(255, 220, 80), Rgb(90, 220, 120), Rgb(90, 170, 255), Rgb(240, 120, 240) };
            for (int i = 0; i < 7; i++)
            {
                float x = 8f + i * 8f;
                float y = 21f - Mathf.Abs(x - 32f) * 0.08f;
                b.Ellipse(x, y, 1.6f, 1.4f, lights[i % lights.Length]);
            }
        }

        /// <summary>No.146 コダイオウペン: 太古から生きる寡黙な王。化石の石のような背の高い巨体で前へ踏み出す。ひび割れと琥珀の体、肩に氷のかけら、胸に金の古代の首飾り。縦に高いシルエット</summary>
        private static void Kodaiou(PenguinBrush b)
        {
            var stone = Rgb(160, 120, 85);
            var stoneShade = PenguinPalette.Shade(stone);
            var amber = Rgb(235, 165, 60);
            var crack = PenguinPalette.Darken(stone, 0.5f);
            // 後ろ足は残し、前足を大きく踏み出して、太古の巨体がのしのし歩く重さを出す
            b.Foot(44f, 5f, -15f, 1.2f);
            b.Foot(14f, 6f, 15f, 1.2f);
            b.Line(4f, stoneShade, V(18, 10), V(14, 6));
            // ほかの伝説より縦に長い体にして、ひときわ背が高い古代種に見せる
            b.ShadedTilted(31f, 22f, 16f, 20f, -6f, stone);
            b.BellyTilted(27f, 21f, 10f, 15f, -6f);
            KodaiouCracks(b, crack, amber);
            // 肩の氷は左右で大きさを変え、氷河を抜けてきた古さを見せる
            b.Triangle(V(40, 38), V(48, 40), V(56, 56), IceShade);
            b.Triangle(V(41, 38), V(46, 39), V(52, 52), IceLight);
            b.Triangle(V(46, 36), V(52, 36), V(60, 44), IceLight);
            b.Triangle(V(16, 38), V(22, 40), V(10, 52), IceLight);
            b.Flipper(V(16, 30), V(5, 18), 4f, stoneShade);
            b.Shaded(28f, 47f, 12f, 11.5f, stone);
            b.White(25f, 46f, 9f, 7.5f);
            b.TurnedFace(25f, 47f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Line(0.6f, crack, V(34, 56), V(36, 52), V(39, 51));
            // 首飾りは大きな金の玉を連ねて、王の宝を素朴な古代の形で見せる
            for (int i = 0; i <= 6; i++)
            {
                var p = OnEllipse(29f, 39f, 13f, 5f, Mathf.PI * (1.1f + i * 0.8f / 6f));
                b.Shaded(p.x, p.y, 1.8f, 1.8f, PenguinPalette.Gold);
            }
            // 胸飾りは胸の前に下がるので、後ろ姿では描かない
            if (!b.Front) return;
            b.Ellipse(27f, 30f, 3.5f, 3.5f, PenguinPalette.GoldShade);
            b.Ellipse(26.5f, 30.5f, 2.5f, 2.5f, PenguinPalette.Gold);
            b.Ellipse(26.5f, 30.5f, 1f, 1f, amber);
        }

        /// <summary>化石のひび割れと、石に閉じこめられた琥珀。後ろ姿にも見えるよう普通の色で描く</summary>
        private static void KodaiouCracks(PenguinBrush b, Color32 crack, Color32 amber)
        {
            b.Line(0.6f, crack, V(42, 30), V(39, 24), V(43, 18), V(40, 12));
            b.Line(0.6f, crack, V(18, 14), V(22, 9));
            b.Line(0.6f, crack, V(38, 6), V(42, 4));
            b.Shaded(43f, 25f, 2.5f, 2f, amber);
            b.Shaded(17f, 22f, 2f, 1.6f, amber);
            b.Shaded(35f, 6f, 1.8f, 1.5f, amber);
        }

        /// <summary>No.147 セカイジュペン: すべてを見守る賢者。背中から大樹が生え、広がる枝葉の樹冠がシルエットの上半分をおおう。足元に根が広がり、木に金の実が光る。きのこ雲のような大きな傘のシルエット</summary>
        private static void Sekaiju(PenguinBrush b)
        {
            var bark = Rgb(120, 85, 55);
            var barkShade = PenguinPalette.Shade(bark);
            var leaf = Rgb(80, 165, 85);
            var leafShade = PenguinPalette.Shade(leaf);
            var body = Rgb(95, 120, 85);
            var bodyShade = PenguinPalette.Shade(body);
            // 根は地面を這うように左右へ広げ、大地に根を張った長老に見せる
            b.Line(1.5f, barkShade, V(32, 6), V(20, 3), V(6, 4));
            b.Line(1.5f, barkShade, V(36, 6), V(50, 3), V(60, 5));
            b.Line(1f, barkShade, V(26, 5), V(14, 7), V(4, 9));
            b.Line(4f, barkShade, V(42, 18), V(44, 30), V(41, 42));
            b.Line(3f, bark, V(41, 18), V(43, 30), V(40, 42));
            b.Line(2f, barkShade, V(43, 34), V(54, 42));
            b.Line(2f, barkShade, V(42, 38), V(28, 46));
            // 樹冠はふくらみを全部影色で塗ってから明るい色を重ね、1つの大きな森のかたまりにする
            var puffs = new[] { V(10, 46), V(18, 53), V(29, 56), V(41, 56), V(52, 52), V(56, 43), V(46, 45), V(33, 48), V(21, 45) };
            foreach (var p in puffs)
            {
                b.Ellipse(p.x, p.y, 7.5f, 6f, leafShade);
            }
            foreach (var p in puffs)
            {
                b.Ellipse(p.x - 0.8f, p.y + 0.8f, 6f, 4.5f, leaf);
            }
            foreach (var p in new[] { V(12, 47), V(24, 55), V(37, 52), V(48, 57), V(55, 45), V(30, 47), V(42, 46) })
            {
                b.Shaded(p.x, p.y, 1.6f, 1.6f, PenguinPalette.Gold);
            }
            b.Foot(20f, 4f, 0f, 1.1f);
            b.Foot(36f, 4f, -10f, 1.1f);
            b.Shaded(28f, 17f, 15f, 13f, body);
            b.Belly(25f, 16f, 10f, 9f);
            b.Flipper(V(14, 20), V(8, 10), 3.5f, bodyShade);
            b.Shaded(26f, 32f, 10.5f, 10f, body);
            b.White(23f, 31f, 8f, 6.5f);
            b.TurnedFace(23f, 32f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            // 賢者らしい白い眉。目の上に垂らして、年を重ねた落ち着きを出す
            if (b.Front) b.Line(0.6f, PenguinPalette.BellyShade, V(17, 35), V(21, 36), V(24, 35));
        }

        /// <summary>No.148 ハジマリペン: すべての始まりの、おだやかな母。真珠のような白い体で静かに立ち、背後に卵の殻のかけらが後光の輪のように浮かぶ。体に金の渦巻き模様。丸い輪に囲まれたシルエット</summary>
        private static void Hajimari(PenguinBrush b)
        {
            var pearl = Rgb(222, 218, 236);
            var pearlShade = Rgb(180, 172, 205);
            var shell = Rgb(250, 242, 220);
            var shellShade = PenguinPalette.Darken(shell, 0.8f);
            var rainbow = new[] { Rgb(255, 170, 200), Rgb(150, 230, 250), Rgb(250, 240, 150), Rgb(190, 250, 190) };
            // 殻のかけらを等間隔の輪に並べ、生まれた瞬間の後光に見せる
            const int Shards = 10;
            for (int i = 0; i < Shards; i++)
            {
                var p = OnEllipse(32f, 34f, 26f, 25f, i * Mathf.PI * 2f / Shards + 0.3f);
                if (p.y < 12f) continue;
                // 丸い殻の上側をえぐって器の形にし、割れた卵の殻のかけらとわかるようにする
                b.Ellipse(p.x, p.y, 5f, 4f, shellShade);
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 4f, 3f, shell);
                b.Erase(p.x, p.y + 3.5f, 4f, 2.5f);
                b.Dot((int)p.x, (int)p.y + 2, shell);
            }
            b.Foot(22f, 4f, 0f, 1.1f);
            b.Foot(40f, 4f, -10f, 1.1f);
            b.Ellipse(32f, 20f, 18f, 16f, pearlShade);
            b.Ellipse(31f, 21f, 16.5f, 14.5f, pearl);
            b.Belly(29f, 19f, 11f, 10f);
            HajimariSwirl(b, 44f, 22f);
            HajimariSwirl(b, 18f, 12f);
            b.Flipper(V(16, 24), V(6, 18), 3.5f, pearlShade);
            b.Flipper(V(48, 24), V(57, 18), 3.5f, pearlShade);
            b.Ellipse(30f, 39f, 12f, 11.5f, pearlShade);
            b.Ellipse(29f, 40f, 10.5f, 10f, pearl);
            b.White(27f, 39f, 8.5f, 7f);
            b.TurnedFace(27f, 40f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            HajimariSwirl(b, 32f, 49f);
            for (int i = 0; i < rainbow.Length; i++)
            {
                var p = OnEllipse(32f, 34f, 20f, 20f, i * 1.6f + 0.8f);
                b.Sparkle((int)p.x, (int)p.y, rainbow[i]);
            }
        }

        /// <summary>金の小さな渦巻き。古い模様らしく、線を角ばらせて巻く</summary>
        private static void HajimariSwirl(PenguinBrush b, float x, float y)
        {
            b.Line(0.6f, PenguinPalette.GoldShade, V(x, y), V(x + 1, y + 1), V(x + 2, y), V(x + 1, y - 2), V(x - 2, y - 2), V(x - 3, y + 1), V(x - 1, y + 3), V(x + 3, y + 3));
        }

        /// <summary>No.149 ニッショクペン: 冷酷で威圧的。背後に黒い太陽の円盤を負い、そのふちで炎のコロナが燃える。暗い体のふちだけが火の色に光り、頭に金の冠。炎のぎざぎざに縁取られた丸いシルエット</summary>
        private static void Nisshoku(PenguinBrush b)
        {
            var disc = Rgb(28, 22, 40);
            var body = Rgb(55, 45, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var red = Rgb(230, 60, 40);
            const float DiscX = 40f;
            const float DiscY = 38f;
            // コロナは長短の炎を交互に出し、静かな日輪ではなく燃えさかる日食にする
            const int Flames = 20;
            for (int i = 0; i < Flames; i++)
            {
                float angle = i * Mathf.PI * 2f / Flames;
                float reach = i % 2 == 0 ? 21f : 19f;
                b.Triangle(OnEllipse(DiscX, DiscY, 15f, 15f, angle - 0.18f), OnEllipse(DiscX, DiscY, 15f, 15f, angle + 0.18f),
                    OnEllipse(DiscX, DiscY, reach, reach, angle), i % 2 == 0 ? red : FlameOuter);
            }
            b.Ellipse(DiscX, DiscY, 16.5f, 16.5f, FlameMiddle);
            b.Ellipse(DiscX, DiscY, 15f, 15f, disc);
            // 右上のふちに光の弧を残し、太陽が欠けた瞬間（ダイヤモンドリング）に見せる
            NightArc(b, FlameInner, DiscX, DiscY, 15f, 15f, 10f, 80f);
            b.Foot(18f, 4f, 0f, 1.1f);
            b.Foot(36f, 4f, -10f, 1.1f);
            // 体は火の色で塗ってから暗い色を左下へずらして重ね、背後の炎に照らされた右上のふちだけ光らせる
            b.Ellipse(29f, 20f, 16f, 15f, FlameOuter);
            b.Ellipse(28f, 19f, 15f, 14f, body);
            b.Belly(25f, 18f, 10f, 9.5f);
            b.Flipper(V(14, 24), V(4, 14), 3.5f, bodyShade);
            b.Ellipse(27f, 39f, 12f, 11.5f, FlameOuter);
            b.Ellipse(26f, 38f, 11f, 10.5f, body);
            b.White(24f, 38f, 8f, 6.5f);
            b.TurnedFace(24f, 39f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            b.Rect(17, 48, 18, 3, PenguinPalette.Gold);
            b.Triangle(V(17, 50), V(22, 50), V(18, 57), PenguinPalette.Gold);
            b.Triangle(V(23, 50), V(29, 50), V(26, 59), PenguinPalette.Gold);
            b.Triangle(V(30, 50), V(35, 50), V(34, 57), PenguinPalette.Gold);
            b.Rect(25, 49, 2, 1, red);
        }

        /// <summary>No.150 アマツバサペン: 全ペンギンの夢をかなえた、晴れやかな勇者。本物の羽根の翼を左右いっぱいに広げて空を飛び、後ろにオーロラの光の帯を引く。キャンバスの横幅いっぱいに翼が広がるシルエット</summary>
        private static void Amatsubasa(PenguinBrush b)
        {
            var body = Rgb(80, 100, 165);
            var bodyShade = PenguinPalette.Shade(body);
            var feather = Rgb(250, 250, 245);
            var featherShade = Rgb(205, 210, 225);
            // オーロラは体から右下の角へうねらせて流し、左上へ飛んできた軌跡に見せる
            Ribbon(b, Rgb(120, 240, 180), V(32, 18), V(38, 10), V(46, 12), V(54, 6), V(61, 4));
            Ribbon(b, Rgb(190, 140, 250), V(30, 16), V(32, 8), V(40, 4), V(48, 6), V(56, 2));
            // 翼は付け根から外へ羽根の先を並べ、ふちを金で縁取って本物の羽に見せる
            Wing(b, V(38, 32), new[] { V(38, 28), V(46, 26), V(56, 30), V(61, 40), V(61, 50), V(56, 60), V(48, 56), V(44, 46) }, feather, featherShade, PenguinPalette.Gold);
            Wing(b, V(26, 32), new[] { V(26, 28), V(18, 26), V(8, 30), V(3, 40), V(3, 50), V(8, 60), V(16, 56), V(20, 46) }, feather, featherShade, PenguinPalette.Gold);
            // 足は後ろへそろえて伸ばし、地面を離れて飛んでいるのを見せる
            b.Foot(29f, 9f, 80f, 0.8f);
            b.Foot(35f, 10f, 100f, 0.8f);
            b.ShadedTilted(32f, 24f, 10f, 13f, 5f, body);
            b.BellyTilted(30f, 23f, 6.5f, 9f, 5f);
            b.Shaded(31f, 40f, 10.5f, 10f, body);
            b.White(29f, 39f, 8f, 6.5f);
            b.TurnedFace(29f, 40f, FaceLeft, EyeStyle.Round, eyeGap: 6f, open: true);
            // 金の羽飾りを額に立て、空の勇者の冠にする
            b.Triangle(V(30, 48), V(34, 48), V(28, 58), PenguinPalette.Gold);
            b.Triangle(V(32, 48), V(36, 48), V(38, 57), PenguinPalette.GoldShade);
            b.Rect(26, 47, 12, 2, PenguinPalette.Gold);
        }
    }
}
