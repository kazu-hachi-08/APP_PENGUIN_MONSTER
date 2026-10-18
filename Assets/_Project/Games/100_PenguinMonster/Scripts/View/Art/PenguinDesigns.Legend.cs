using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 伝説ランクのデザイン。キャンバスいっぱいに描き、金の飾りを付ける。
    /// Paint が外側に黒と金の輪郭を2周回すので、外周 2 ドットは空けておく
    /// </summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.48 ナンキョクペン: 威厳ある氷の王。巨体で胸を張ってあごを上げ、ヒレで前を指して命じる。背中に氷山、頭に氷の王冠（ユキオの切り札）</summary>
        private static void Nankyoku(PenguinBrush b)
        {
            var body = Rgb(70, 120, 190);
            var bodyShade = PenguinPalette.Shade(body);
            // 氷山は背中側（右）に高くそびえさせ、体と合わせて右肩上がりの大きな山のシルエットにする
            b.Polygon(V(46, 30), IceShade, V(30, 6), V(34, 36), V(40, 50), V(46, 44), V(52, 61), V(58, 48), V(61, 36), V(61, 6));
            b.Polygon(V(44, 32), IceLight, V(34, 8), V(37, 36), V(41, 46), V(46, 40), V(51, 56), V(52, 30), V(50, 8));
            b.Foot(18f, 4.5f, 0f, 1.2f);
            b.Foot(42f, 4.5f, -10f, 1.2f);
            // 体はキャンバスの幅いっぱい。少し反らせて胸を張る
            b.ShadedTilted(30f, 22f, 21f, 18f, -10f, body);
            b.BellyTilted(26f, 21f, 15f, 14f, -10f);
            b.Flipper(V(14, 28), V(3, 20), 4f, bodyShade);
            b.Shaded(27f, 43f, 13f, 12.5f, body);
            b.White(24f, 42f, 10f, 8f);
            b.TurnedFace(24f, 43f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            // 王冠は後ろへ少し傾け、あごを上げた尊大さを出す
            b.Rect(18, 53, 18, 3, PenguinPalette.Gold);
            b.Triangle(V(18, 55), V(23, 55), V(19, 61), IceLight);
            b.Triangle(V(23, 55), V(31, 55), V(28, 62), IceLight);
            b.Triangle(V(27, 55), V(31, 55), V(28, 62), IceShade);
            b.Triangle(V(31, 55), V(36, 55), V(37, 60), IceLight);
            b.Rect(26, 54, 2, 1, Rgb(90, 200, 255));
            b.Sparkle(10, 50, PenguinPalette.Belly);
            b.Sparkle(56, 24, PenguinPalette.Belly);
        }

        /// <summary>No.49 ライテイペン: 気まぐれな雷の帝。空に浮かび、雷雲の翼を高低ちがいに広げて口を開け、稲妻を呼ぶ。背中に金の輪（ライカの切り札）</summary>
        private static void Raitei(PenguinBrush b)
        {
            var body = Rgb(250, 228, 130);
            var cloud = Rgb(110, 115, 145);
            b.Ellipse(36f, 38f, 19f, 19f, PenguinPalette.GoldShade);
            b.Ellipse(35.5f, 38.5f, 18f, 18f, PenguinPalette.Gold);
            b.Erase(36f, 38f, 16f, 16f);
            for (int i = 0; i < 4; i++)
            {
                var p = OnEllipse(36f, 38f, 17f, 17f, Mathf.PI / 4f + i * Mathf.PI / 2f);
                b.Shaded(p.x, p.y, 2.5f, 2.5f, PenguinPalette.Gold);
            }
            // 稲妻は雲の翼から地面まで落として、浮いた体と地面をつなぐ縦の線にする
            b.Line(1.6f, BoltShade, V(8, 34), V(12, 24), V(5, 18), V(8, 4));
            b.Line(0.8f, Bolt, V(8, 34), V(12, 24), V(5, 18), V(8, 4));
            b.Line(1.6f, BoltShade, V(56, 18), V(52, 11), V(58, 7), V(56, 3));
            b.Line(0.8f, Bolt, V(56, 18), V(52, 11), V(58, 7), V(56, 3));
            // 翼は前を高く、奥を低く。左右で高さを変えて左右対称の威嚇ポーズを崩す。ヒレで雲をつかんでいるように見せる
            b.Flipper(V(42, 28), V(52, 26), 3.5f, PenguinPalette.Shade(body));
            StormWing(b, cloud, 53f, 26f);
            b.Foot(30f, 7f, 80f, 0.8f);
            b.Foot(38f, 8f, 110f, 0.8f);
            b.ShadedTilted(34f, 20f, 14f, 14f, 12f, body);
            b.BellyTilted(31f, 19f, 9f, 10f, 12f);
            b.Flipper(V(22, 32), V(12, 42), 3.5f, PenguinPalette.Shade(body));
            StormWing(b, cloud, 12f, 47f);
            b.Shaded(30f, 37f, 12.5f, 12f, body);
            b.White(27f, 36f, 9.5f, 7.5f);
            var crest = new[] { V(32, 48), V(37, 53), V(32, 56), V(38, 61) };
            b.Line(1.4f, BoltShade, crest);
            b.Line(0.7f, Bolt, crest);
            b.TurnedFace(27f, 37f, FaceLeft, EyeStyle.Cross, eyeGap: 7f, open: true);
        }

        /// <summary>雷雲の翼。ふくらみを全部影色で塗ってから明るい色を重ねる（1つずつ影を付けると中に線が残るため）</summary>
        private static void StormWing(PenguinBrush b, Color32 cloud, float x, float y)
        {
            var puffs = new[] { V(x - 6f, y + 1f), V(x + 1f, y + 5f), V(x + 6f, y - 1f), V(x - 1f, y - 4f), V(x + 2f, y) };
            foreach (var p in puffs)
            {
                b.Ellipse(p.x, p.y, 5.5f, 5f, PenguinPalette.Shade(cloud));
            }
            foreach (var p in puffs)
            {
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 4.3f, 3.8f, cloud);
            }
        }

        /// <summary>No.50 キョクヤペン: 静かで底知れない夜の主。星空のマントを後ろへ大きくひるがえし、前に出したヒレの上に星を浮かべる。すそにオーロラ、頭に三日月の冠（ヨミの切り札）</summary>
        private static void Kyokuya(PenguinBrush b)
        {
            var cape = Rgb(28, 30, 72);
            var body = Rgb(80, 64, 130);
            // マントは背中側（右上）へ風を受けたように広げ、体の倍の大きさの斜めのシルエットにする。ふちは波打たせて布らしく
            b.Polygon(V(42, 28), cape, V(24, 46), V(36, 50), V(48, 58), V(61, 61), V(57, 50), V(61, 40), V(55, 30), V(60, 20), V(54, 12),
                V(60, 3), V(30, 3), V(20, 12));
            // オーロラはひるがえった裏地（上のふち）とすそに通して、マントの流れを強調する
            Ribbon(b, Rgb(120, 240, 180), V(34, 49), V(46, 56), V(58, 59));
            Ribbon(b, Rgb(240, 140, 220), V(38, 6), V(46, 8), V(54, 5), V(59, 9));
            foreach (var p in new[] { V(44, 48), V(50, 38), V(42, 16), V(52, 22), V(46, 28) })
            {
                b.Dot((int)p.x, (int)p.y, PenguinPalette.Gold);
            }
            b.Sparkle(40, 42, PenguinPalette.Gold);
            b.Sparkle(52, 14, PenguinPalette.Gold);
            b.Foot(20f, 4.5f);
            b.Foot(33f, 4.5f, -10f);
            b.ShadedTilted(27f, 19f, 13f, 15f, 6f, body);
            b.BellyTilted(24f, 18f, 8f, 11f, 6f);
            b.Shaded(26f, 39f, 12.5f, 12f, body);
            b.White(23f, 38f, 9.5f, 7.5f);
            b.TurnedFace(23f, 39f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
            // 前に差し出したヒレの上に星を浮かべ、力を静かに見せつける
            b.Flipper(V(16, 26), V(6, 28), 3f, PenguinPalette.Shade(body));
            b.Star(7f, 37f, 4.5f, 2f, PenguinPalette.Gold);
            b.Rect(18, 50, 16, 3, PenguinPalette.Gold);
            b.Triangle(V(18, 52), V(22, 52), V(19, 56), PenguinPalette.Gold);
            b.Ellipse(26f, 57f, 5f, 5f, Rgb(255, 230, 120));
            b.Erase(28.5f, 59f, 4f, 4f);
        }

        /// <summary>No.74 ワダツミペン: 海を統べる寡黙な神。背中に大渦を巡らせ、金の三叉の矛を立てて静かに見下ろす。頭に波の冠（シズクの切り札）</summary>
        private static void Wadatsumi(PenguinBrush b)
        {
            var body = Rgb(40, 80, 150);
            var bodyShade = PenguinPalette.Shade(body);
            NightArc(b, Water, 33f, 34f, 27f, 26f, 20f, 330f);
            NightArc(b, DeepWater, 33f, 34f, 20f, 19f, 60f, 380f);
            b.Line(1f, PenguinPalette.GoldShade, V(53, 3), V(53, 50));
            b.Line(0.6f, PenguinPalette.Gold, V(53, 3), V(53, 50));
            b.Line(1f, PenguinPalette.Gold, V(47, 46), V(59, 46));
            foreach (var x in new[] { 47, 53, 59 })
            {
                b.Triangle(V(x - 1.5f, 46), V(x + 1.5f, 46), V(x, x == 53 ? 60 : 56), PenguinPalette.Gold);
            }
            b.Foot(20f, 4f, 0f, 1.1f);
            b.Foot(38f, 4f, -10f, 1.1f);
            b.ShadedTilted(29f, 21f, 18f, 16f, -8f, body);
            b.BellyTilted(26f, 20f, 13f, 12f, -8f);
            b.Flipper(V(42, 25), V(52, 27), 3.5f, bodyShade);
            b.Flipper(V(14, 26), V(5, 16), 3.5f, bodyShade);
            b.Shaded(27f, 41f, 12f, 11.5f, body);
            b.White(25f, 40f, 9f, 7.5f);
            b.TurnedFace(25f, 41f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Rect(17, 50, 19, 3, PenguinPalette.Gold);
            b.Triangle(V(17, 52), V(23, 52), V(16, 60), Water);
            b.Triangle(V(23, 52), V(30, 52), V(24, 61), PenguinPalette.Belly);
            b.Triangle(V(29, 52), V(36, 52), V(31, 59), Water);
        }

        /// <summary>No.75 アマテラスペン: 明るく気高い日輪の女王。背に燃える日輪を負い、オーロラの帯をまとって両ヒレを広げる（ヒナタの切り札）</summary>
        private static void Amaterasu(PenguinBrush b)
        {
            var body = Rgb(235, 160, 80);
            var bodyShade = PenguinPalette.Shade(body);
            var teal = Rgb(110, 220, 200);
            var pink = Rgb(240, 150, 200);
            const int Rays = 16;
            for (int i = 0; i < Rays; i++)
            {
                float angle = i * Mathf.PI * 2f / Rays;
                b.Triangle(OnEllipse(32f, 37f, 16f, 16f, angle - 0.14f), OnEllipse(32f, 37f, 16f, 16f, angle + 0.14f),
                    OnEllipse(32f, 37f, 25f, 24f, angle), i % 2 == 0 ? FlameOuter : PenguinPalette.Gold);
            }
            b.Ellipse(32f, 37f, 17f, 17f, PenguinPalette.GoldShade);
            b.Ellipse(31.5f, 37.5f, 15.5f, 15.5f, PenguinPalette.Gold);
            b.Foot(24f, 4f, 10f, 1.1f);
            b.Foot(40f, 4f, -10f, 1.1f);
            b.Shaded(32f, 19f, 15f, 14f, body);
            b.Belly(31f, 18f, 10f, 10f);
            NightArc(b, teal, 32f, 18f, 27f, 6f, 180f, 360f);
            b.Flipper2(V(19, 24), V(6, 31), 3.5f, bodyShade);
            b.Shaded(31f, 37f, 11f, 10.5f, body);
            b.White(29f, 36f, 8f, 6.5f);
            b.TurnedFace(29f, 37f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
            NightArc(b, pink, 32f, 26f, 25f, 5f, 200f, 340f);
            b.Star(31f, 51f, 4.5f, 2f, FlameOuter);
            b.Star(31f, 51.5f, 3f, 1.3f, FlameInner);
        }
    }
}
