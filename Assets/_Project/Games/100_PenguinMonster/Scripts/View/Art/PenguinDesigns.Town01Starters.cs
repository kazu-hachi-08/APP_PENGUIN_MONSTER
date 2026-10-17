using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// ストーリーの最初の3体（No.151〜159）。
    /// 進化で育つのが見て分かるよう、体を 進化前 ヒナくらい → 途中 ひと回り大きく → 最後 ふつうより大きく、と段ごとに大きくする
    /// </summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 Wood = Rgb(170, 115, 70);
        private static readonly Color32 WoodLight = Rgb(205, 155, 100);
        private static readonly Color32 Iron = Rgb(90, 92, 108);
        private static readonly Color32 IronLight = Rgb(150, 152, 170);
        private static readonly Color32 SnowShade = Rgb(196, 212, 230);

        /// <summary>No.151 コブネペン: 好奇心いっぱいの見習い船乗り。小さな木の小舟に乗り込み、ヒレで櫂を握って漕ぎ出す</summary>
        private static void Kobune(PenguinBrush b)
        {
            var body = Rgb(70, 140, 215);
            var bodyShade = PenguinPalette.Shade(body);
            b.Line(1f, WoodLight, V(46, 26), V(56, 3));
            b.Tilted(56f, 4f, 1.8f, 4.5f, -22f, Wood);
            b.Shaded(32f, 16f, 8.5f, 7.5f, body);
            b.Belly(30f, 15f, 5.5f, 5f);
            b.Shaded(31f, 27f, 8f, 7.5f, body);
            b.White(29f, 26.5f, 6f, 4.5f);
            // 舟は体の下半分に重ねて、乗り込んでいるように見せる
            b.Polygon(V(32, 9), PenguinPalette.Shade(Wood), V(12, 15), V(52, 15), V(45, 4), V(19, 4));
            b.Polygon(V(32, 10), Wood, V(14, 15), V(50, 15), V(44, 6), V(20, 6));
            b.Line(1f, WoodLight, V(13, 15), V(51, 15));
            b.Line(0.5f, PenguinPalette.Shade(Wood), V(20, 10), V(46, 10));
            b.Flipper(V(37, 18), V(47, 21), 2f, bodyShade);
            b.TurnedFace(29f, 28f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
            b.Line(0.6f, Water, V(4, 4), V(9, 6), V(14, 4));
            b.Line(0.6f, Water, V(50, 3), V(55, 5), V(60, 3));
        }

        /// <summary>No.152 ホバシラペン: 頼れる甲板長。帆をいっぱいに張った帆柱の前に立ち、バンダナをなびかせて行く先をヒレで指す</summary>
        private static void Hobashira(PenguinBrush b)
        {
            var body = Rgb(55, 120, 200);
            var bodyShade = PenguinPalette.Shade(body);
            var bandana = Rgb(220, 60, 60);
            b.Line(1.2f, Wood, V(47, 6), V(47, 61));
            b.Triangle(V(47, 61), V(47, 57), V(40, 59), bandana);
            // 帆は風をはらんだふくらみを出すため、外の辺に楕円を重ねる
            b.Triangle(V(48, 56), V(48, 20), V(61, 24), SnowShade);
            b.Ellipse(55f, 38f, 5f, 14f, SnowShade);
            b.Triangle(V(48, 55), V(48, 22), V(59, 25), SnowWhite);
            b.Ellipse(54f, 38f, 4f, 13f, SnowWhite);
            b.Line(1f, DeepWater, V(48, 36), V(58, 37));
            b.Foot(36f, 3f);
            b.Foot(24f, 3f, 10f);
            b.ShadedTilted(31f, 16f, 11f, 12f, -5f, body);
            b.BellyTilted(28f, 15f, 7.5f, 9f, -5f);
            b.Flipper(V(38, 22), V(44, 14), 2.5f, bodyShade);
            b.Shaded(30f, 33f, 10.5f, 10f, body);
            b.White(27f, 32f, 8f, 6.5f);
            b.Line(2f, bandana, V(20, 40), V(39, 41));
            b.Line(1.2f, bandana, V(39, 40), V(44, 36), V(46, 38));
            b.Flipper(V(21, 22), V(6, 30), 2.5f, bodyShade);
            b.TurnedFace(27f, 33f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
        }

        /// <summary>No.153 サイヒョウセンペン: 何があっても進む船長。体の前が砕氷船の赤い舳先になっていて、氷を割り砕きながら胸を張る</summary>
        private static void Saihyousen(PenguinBrush b)
        {
            var body = Rgb(40, 80, 160);
            var bodyShade = PenguinPalette.Shade(body);
            var hull = Rgb(205, 60, 50);
            var cap = Rgb(245, 245, 250);
            b.Flipper(V(47, 28), V(59, 38), 3f, bodyShade);
            b.Shaded(34f, 21f, 16f, 15f, body);
            b.Belly(31f, 22f, 10f, 9f);
            // 舳先は左にとがらせ、体ごと船になって進んでいるシルエットにする
            b.Polygon(V(32, 10), PenguinPalette.Shade(hull), V(4, 15), V(52, 23), V(54, 4), V(14, 1));
            b.Polygon(V(32, 11), hull, V(6, 15), V(51, 21), V(52, 6), V(16, 3));
            b.Line(1f, SnowWhite, V(7, 14), V(51, 19));
            b.Dot(36, 12, SnowWhite);
            b.Dot(42, 13, SnowWhite);
            b.Shaded(32f, 41f, 12f, 11f, body);
            b.White(29f, 40f, 9f, 7.5f);
            b.Ellipse(32f, 51f, 10f, 3.5f, cap);
            b.Ellipse(26f, 48f, 8f, 1.6f, PenguinPalette.Outline);
            b.Ellipse(32f, 51f, 1.5f, 1.5f, PenguinPalette.Gold);
            b.Flipper(V(20, 30), V(9, 25), 3f, bodyShade);
            b.TurnedFace(29f, 41f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Polygon(V(6, 7), IceLight, V(1, 5), V(7, 2), V(11, 7), V(5, 11));
            b.Polygon(V(4, 20), IceShade, V(1, 18), V(6, 17), V(8, 21), V(3, 23));
            b.Polygon(V(58, 4), IceLight, V(55, 2), V(62, 2), V(61, 7), V(56, 7));
            b.Sparkle(12, 24, IceLight);
        }

        /// <summary>No.154 ヒダネペン: 寒がりの甘えん坊。頭のてっぺんに小さな火をともし、両ヒレで大事そうに火種を包んで温まる</summary>
        private static void Hidane(PenguinBrush b)
        {
            var body = Rgb(225, 95, 60);
            var bodyShade = PenguinPalette.Shade(body);
            b.Foot(28f, 3f, 0f, 0.6f);
            b.Foot(37f, 3f, 0f, 0.6f);
            b.Shaded(33f, 12f, 8.5f, 8f, body);
            b.Belly(31f, 11f, 5.5f, 5.5f);
            b.Shaded(32f, 23f, 8f, 7.5f, body);
            b.White(30f, 22.5f, 6f, 4.5f);
            b.Tilted(34f, 33f, 3f, 5f, -15f, FlameOuter);
            b.Tilted(34f, 32f, 2f, 3.5f, -15f, FlameMiddle);
            b.Ellipse(34f, 31f, 1f, 1.5f, FlameInner);
            b.Ellipse(23f, 12f, 3f, 3f, FlameOuter);
            b.Ellipse(23f, 12.5f, 2f, 2f, FlameMiddle);
            b.Dot(23, 13, FlameInner);
            b.Flipper(V(27, 15), V(21, 9), 2f, bodyShade);
            b.Flipper(V(39, 15), V(26, 13), 2f, bodyShade);
            b.TurnedFace(30f, 24f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
        }

        /// <summary>No.155 フイゴペン: 働き者の鍛冶の弟子。大きな鞴をヒレで押しこみ、先の口から火を吹き出させる。頭の火も勢いを増した</summary>
        private static void Fuigo(PenguinBrush b)
        {
            var body = Rgb(215, 85, 50);
            var bodyShade = PenguinPalette.Shade(body);
            var leather = Rgb(130, 85, 55);
            b.Foot(41f, 3f);
            b.Foot(31f, 3f, 15f);
            b.ShadedTilted(37f, 16f, 11f, 12f, -8f, body);
            b.BellyTilted(34f, 15f, 7.5f, 9f, -8f);
            b.Tilted(39f, 45f, 4f, 7f, -10f, FlameOuter);
            b.Tilted(39f, 44f, 3f, 5f, -10f, FlameMiddle);
            b.Ellipse(39f, 43f, 1.5f, 2f, FlameInner);
            b.Shaded(35f, 33f, 10f, 9.5f, body);
            b.White(32f, 32f, 7.5f, 6.5f);
            // 鞴は革の袋を板で挟んだ形。押しこんでいる最中なので、上の板を斜めにする
            b.Tilted(21f, 13f, 9f, 5f, 0f, leather);
            b.Line(0.5f, PenguinPalette.Shade(leather), V(15, 13), V(27, 13));
            b.Line(1.5f, Wood, V(12, 17), V(30, 20));
            b.Line(1.5f, Wood, V(12, 9), V(30, 8));
            b.Triangle(V(13, 15), V(13, 11), V(5, 13), Iron);
            b.Ellipse(3f, 13f, 3f, 2.5f, FlameOuter);
            b.Ellipse(3f, 13f, 2f, 1.5f, FlameMiddle);
            b.Flipper(V(30, 24), V(27, 19), 2.5f, bodyShade);
            b.TurnedFace(32f, 33f, FaceLeft, EyeStyle.Round, eyeGap: 6f, open: true);
            b.Sparkle(6, 21, FlameInner);
        }

        /// <summary>No.156 カタナカジペン: 頑固一徹の刀鍛冶。鉢巻を締め、金床の上の真っ赤な刃へ金槌を振り上げる。火花が散る</summary>
        private static void Katanakaji(PenguinBrush b)
        {
            var body = Rgb(165, 70, 50);
            var bodyShade = PenguinPalette.Shade(body);
            var apron = Rgb(110, 80, 60);
            var band = Rgb(240, 240, 235);
            var hot = Rgb(255, 120, 60);
            b.Line(1.3f, Wood, V(45, 30), V(52, 52));
            b.Rect(47, 50, 11, 6, Iron);
            b.Rect(47, 54, 11, 2, IronLight);
            b.Foot(42f, 3f);
            b.Foot(30f, 3f, 10f);
            b.ShadedTilted(37f, 19f, 14f, 15f, -6f, body);
            b.BellyTilted(34f, 19f, 9.5f, 11f, -6f);
            // 前掛けはおなかの下半分だけにして、白いおなかを残す（全部覆うと樽のように見えたため）
            if (b.Front)
            {
                b.Polygon(V(34, 12), apron, V(25, 17), V(43, 18), V(42, 7), V(27, 6));
                b.Line(0.6f, apron, V(26, 17), V(28, 28));
                b.Line(0.6f, apron, V(42, 18), V(40, 29));
            }
            b.Flipper(V(45, 31), V(50, 42), 3f, bodyShade);
            b.Shaded(35f, 41f, 12f, 11.5f, body);
            b.White(32f, 40f, 9f, 7.5f);
            b.Line(2f, band, V(23, 47), V(47, 48));
            b.Line(1.2f, band, V(47, 47), V(53, 43), V(56, 45));
            // 金床は台の上だけ広い形。凹んだ形は Polygon で塗れないので四角を重ねる
            b.Rect(3, 13, 22, 4, Iron);
            b.Rect(3, 16, 22, 1, IronLight);
            b.Rect(9, 4, 10, 9, Iron);
            b.Rect(6, 1, 16, 3, Iron);
            b.Line(1.2f, hot, V(4, 18.5f), V(24, 18.5f));
            b.Line(0.4f, FlameInner, V(6, 19), V(20, 19));
            b.Flipper(V(25, 27), V(20, 21), 3f, bodyShade);
            b.TurnedFace(32f, 41f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Sparkle(9, 25, FlameInner);
            b.Sparkle(17, 29, FlameInner);
            b.Sparkle(27, 22, FlameMiddle);
        }

        /// <summary>No.157 メブキペン: のんびりしたお日さま好き。雪の上にちょこんと立ち、頭に双葉の芽を出している</summary>
        private static void Mebuki(PenguinBrush b)
        {
            var body = Rgb(120, 180, 90);
            var bodyShade = PenguinPalette.Shade(body);
            var leaf = Rgb(90, 200, 80);
            b.Ellipse(32f, 3f, 16f, 3f, SnowShade);
            b.Ellipse(31f, 4f, 14f, 2f, SnowWhite);
            b.Foot(28f, 4f, 0f, 0.6f);
            b.Foot(37f, 4f, 0f, 0.6f);
            b.Shaded(32f, 12f, 8.5f, 7.5f, body);
            b.Belly(30f, 11f, 5.5f, 5f);
            b.Flipper(V(25, 13), V(20, 8), 2f, bodyShade);
            b.Flipper(V(39, 13), V(44, 8), 2f, bodyShade);
            b.Shaded(32f, 23f, 8f, 7.5f, body);
            b.White(30f, 22.5f, 6f, 4.5f);
            b.Line(0.8f, PenguinPalette.Shade(leaf), V(32, 30), V(32, 35));
            b.Tilted(28.5f, 36f, 3.5f, 1.8f, 25f, leaf);
            b.Tilted(35.5f, 37f, 3.5f, 1.8f, -25f, leaf);
            b.TurnedFace(30f, 24f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
        }

        /// <summary>No.158 フキノトウペン: はにかみ屋。雪の山から顔を出し、蕗の薹のつぼみの葉を頭巾のようにかぶって、すき間からのぞく</summary>
        private static void Fukinotou(PenguinBrush b)
        {
            var body = Rgb(110, 170, 80);
            var bud = Rgb(175, 215, 110);
            var bract = Rgb(130, 185, 85);
            var bractTip = Rgb(200, 230, 140);
            b.Shaded(32f, 19f, 10.5f, 10f, body);
            b.Belly(30f, 18f, 7f, 6.5f);
            b.Shaded(32f, 34f, 10f, 9.5f, bud);
            // つぼみの外の葉を左右から重ね、顔だけ のぞかせる
            b.ShadedTilted(22f, 33f, 5f, 10f, 18f, bract);
            b.ShadedTilted(42f, 33f, 5f, 10f, -18f, bract);
            b.ShadedTilted(32f, 44f, 6f, 5f, 0f, bractTip);
            b.Ellipse(32f, 49f, 2f, 2.5f, bract);
            b.White(30f, 32f, 6.5f, 5f);
            b.TurnedFace(30f, 33f, FaceLeft, EyeStyle.Worried, eyeGap: 5f);
            // 雪の山は最後に重ねて、下から生えてきたように見せる
            b.Ellipse(32f, 6f, 25f, 8f, SnowShade);
            b.Ellipse(30f, 7f, 23f, 6.5f, SnowWhite);
            b.Sparkle(10, 12, IceLight);
        }

        /// <summary>No.159 ユキツバキペン: 凛とした雪国の姫。深い緑の葉の衣をまとい、頭に雪をのせた真っ赤な椿を咲かせる。まわりにオーロラの光</summary>
        private static void Yukitsubaki(PenguinBrush b)
        {
            var body = Rgb(40, 110, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var leaf = Rgb(60, 150, 80);
            var petal = Rgb(215, 40, 60);
            var petalShade = PenguinPalette.Shade(petal);
            var stamen = Rgb(255, 220, 80);
            b.Foot(36f, 3f);
            b.Foot(27f, 3f);
            b.ShadedTilted(32f, 19f, 13f, 15f, -4f, body);
            b.BellyTilted(29f, 18f, 8.5f, 11f, -4f);
            b.ShadedTilted(18f, 24f, 4f, 8f, 30f, leaf);
            b.ShadedTilted(46f, 24f, 4f, 8f, -30f, leaf);
            b.Flipper(V(42, 30), V(52, 40), 2.5f, bodyShade);
            b.Shaded(31f, 39f, 11f, 10.5f, body);
            b.White(28f, 38f, 8f, 7f);
            for (int i = 0; i < 5; i++)
            {
                float angle = Mathf.PI / 2f + i * Mathf.PI * 2f / 5f;
                b.Ellipse(41f + Mathf.Cos(angle) * 4f, 50f + Mathf.Sin(angle) * 4f, 3.5f, 3.5f, petalShade);
                b.Ellipse(40.5f + Mathf.Cos(angle) * 4f, 50.5f + Mathf.Sin(angle) * 4f, 2.8f, 2.8f, petal);
            }
            b.Ellipse(41f, 50f, 2f, 2f, stamen);
            b.Ellipse(40f, 55f, 4f, 1.5f, SnowWhite);
            b.Flipper(V(21, 30), V(14, 22), 2.5f, bodyShade);
            b.TurnedFace(28f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
            b.Sparkle(8, 44, Rgb(140, 240, 220));
            b.Sparkle(56, 28, Rgb(240, 160, 230));
            b.Sparkle(12, 10, Rgb(140, 240, 220));
        }
    }
}
