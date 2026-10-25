using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 7番道路の奥・大橋・道場の子（No.256〜270）。夜風・夜嵐・火打ち石・狼煙・ウミネコ・大カモメ・シャボン玉・風鈴・
    /// 風車・風力発電・つむじ風・竜巻・避雷針・池のぬし・虹の羽
    /// </summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 NightWind = Rgb(150, 140, 200);
        private static readonly Color32 Smoke = Rgb(190, 190, 195);
        private static readonly Color32 GullGray = Rgb(150, 160, 175);
        private static readonly Color32 BubbleFilm = Rgb(200, 230, 250);
        // 虹は外から 赤・橙・黄・緑・青・紫
        private static readonly Color32[] Rainbow =
        {
            Rgb(235, 80, 80), Rgb(245, 160, 60), Rgb(250, 220, 80), Rgb(110, 200, 110), Rgb(90, 150, 235), Rgb(160, 100, 210),
        };

        /// <summary>No.256 ヨカゼペン: 夜の道を こっそり吹き抜ける 夜風の子。月の明るい晩は 背中に うず巻きの風をまとう</summary>
        private static void Yokaze(PenguinBrush b)
        {
            var body = Rgb(80, 70, 120);
            var bodyShade = PenguinPalette.Shade(body);
            // 背中の三日月。丸を塗ってから少しずらして消し、細い月にする
            b.Ellipse(46f, 44f, 6f, 6f, MoonYellow);
            b.Erase(49f, 46f, 5f, 5f);
            b.Line(0.6f, NightWind, V(12, 30), V(8, 26), V(12, 22), V(16, 25));
            b.Line(0.6f, NightWind, V(50, 20), V(56, 18), V(54, 13));
            b.Feet(32f, 5f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(18, 13), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Sleepy, eyeGap: 5f);
        }

        /// <summary>No.257 ヨアラシペン: 夜の嵐を 連れて歩く 黒い風。ぼろぼろのマントのような翼を ひるがえすと 灯りが次々に消える</summary>
        private static void Yoarashi(PenguinBrush b)
        {
            var body = Rgb(60, 50, 95);
            var bodyShade = PenguinPalette.Shade(body);
            var cape = Rgb(40, 35, 65);
            // マントの翼は裾をぎざぎざに切り、後ろへ大きくはためかせる
            b.Polygon(V(32, 32), cape, V(18, 46), V(46, 46), V(60, 34), V(58, 20), V(62, 8), V(52, 12), V(48, 4), V(40, 10),
                V(24, 10), V(16, 4), V(12, 12), V(2, 8), V(6, 20), V(4, 34));
            b.Ellipse(52f, 54f, 6f, 6f, MoonYellow);
            b.Erase(55f, 56f, 5f, 5f);
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 13f, 12f, body);
            b.Belly(30f, 18f, 8f, 8f);
            b.Shaded(31f, 37f, 11f, 9.5f, body);
            b.White(28f, 36f, 8f, 6.5f);
            WindLines(b, NightWind, (2f, 52f, 10f), (6f, 58f, 8f), (44f, 2f, 12f));
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 8f);
        }

        /// <summary>No.258 ヒウチペン: 両手の火打ち石を かちかち鳴らす 小さな火の番。火花が飛ぶと 得意げに胸を張る</summary>
        private static void Hiuchi(PenguinBrush b)
        {
            var body = Rgb(200, 110, 70);
            var bodyShade = PenguinPalette.Shade(body);
            b.Feet(32f, 5f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.White(30f, 25.5f, 6f, 4.5f);
            // 両手を胸の前で打ち合わせ、間に火花を散らす
            b.Flipper(V(25, 18), V(22, 30), 2f, bodyShade);
            b.Flipper(V(39, 18), V(42, 30), 2f, bodyShade);
            b.Shaded(21f, 32f, 3f, 2.5f, Stone);
            b.Shaded(43f, 32f, 3f, 2.5f, Stone);
            b.Sparkles(FlameInner, V(32, 38), V(27, 42), V(37, 42));
            b.Dot(30, 35, FlameOuter);
            b.Dot(34, 35, FlameOuter);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
        }

        /// <summary>No.259 ノロシペン: 頭の火から 高く煙をのぼらせ、遠くの仲間に合図を送る子。風の向きは 煙を見れば ひと目でわかる</summary>
        private static void Noroshi(PenguinBrush b)
        {
            var body = Rgb(160, 75, 55);
            var bodyShade = PenguinPalette.Shade(body);
            // 煙は頭から上へのぼり、風下（右）へ流れて広がる。小さい玉から大きい玉へ
            foreach (var (x, y, r) in new[] { (34f, 50f, 3f), (37f, 54f, 4f), (42f, 57f, 4.5f), (49f, 59f, 5f), (56f, 60f, 4.5f) })
            {
                b.Shaded(x, y, r, r * 0.8f, Smoke);
            }
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(29f, 18f, 9f, 9f);
            b.Shaded(31f, 37f, 11f, 9.5f, body);
            b.Ellipse(32f, 46f, 4.5f, 4f, FlameOuter);
            b.Ellipse(32f, 46f, 2.8f, 2.5f, FlameMiddle);
            b.Ellipse(32f, 45.5f, 1.4f, 1.3f, FlameInner);
            b.White(28f, 36f, 8f, 6.5f);
            b.Flipper(V(18, 24), V(10, 14), 3.5f, bodyShade);
            b.Flipper(V(45, 26), V(53, 36), 3.5f, bodyShade);
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
        }

        /// <summary>No.260 ウミネコペン: 大橋の休憩所で 弁当を ねらっている ちゃっかり屋。猫のような声で みゃあと鳴く</summary>
        private static void Umineko(PenguinBrush b)
        {
            var bodyShade = PenguinPalette.Shade(GullGray);
            b.Feet(32f, 5f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, GullGray);
            b.Belly(30f, 14f, 5.5f, 5.5f);
            // 頭は白く、くちばしの先に黒と赤の印を付ける（ウミネコの目印）
            b.Shaded(32f, 26f, 8f, 7.5f, SnowWhite);
            b.Triangle(V(38, 16), V(44, 14), V(42, 8), PenguinPalette.Outline);
            b.Flipper2(V(25, 17), V(17, 15), 2.5f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
            if (b.Front)
            {
                b.Dot(23, 24, PenguinPalette.Outline);
                b.Dot(23, 23, Rgb(220, 60, 60));
            }
        }

        /// <summary>No.261 オオカモメペン: 大橋の上を 翼を広げたまま 何時間も 滑るように飛ぶ。風を読むのは 誰よりもうまい</summary>
        private static void Ookamome(PenguinBrush b)
        {
            var wing = GullGray;
            var wingShade = PenguinPalette.Shade(wing);
            // 翼は水平にまっすぐ広げ、先だけ黒くする。風に乗って滑る姿
            b.Polygon(V(14, 34), wingShade, V(22, 38), V(1, 40), V(1, 34), V(22, 28));
            b.Polygon(V(50, 34), wingShade, V(42, 38), V(63, 40), V(63, 34), V(42, 28));
            b.Polygon(V(14, 35), wing, V(22, 37), V(4, 39), V(4, 35), V(22, 30));
            b.Polygon(V(50, 35), wing, V(42, 37), V(60, 39), V(60, 35), V(42, 30));
            b.Triangle(V(1, 40), V(1, 33), V(7, 37), PenguinPalette.Outline);
            b.Triangle(V(63, 40), V(63, 33), V(57, 37), PenguinPalette.Outline);
            b.Feet(32f, 7f);
            b.Shaded(32f, 19f, 12f, 12f, wing);
            b.Belly(30f, 18f, 8.5f, 9f);
            b.Shaded(31f, 38f, 10f, 9f, SnowWhite);
            b.TurnedFace(29f, 39f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            if (b.Front)
            {
                b.Dot(20, 36, PenguinPalette.Outline);
                b.Dot(20, 35, Rgb(220, 60, 60));
            }
        }

        /// <summary>シャボン玉を1つ描く。薄い膜の輪と、光の点だけで透けて見せる</summary>
        private static void Bubble(PenguinBrush b, float x, float y, float radius)
        {
            b.Ellipse(x, y, radius, radius, BubbleFilm);
            b.Erase(x, y, radius - 1f, radius - 1f);
            b.Dot((int)(x - radius * 0.4f), (int)(y + radius * 0.4f), PenguinPalette.EyeLight);
        }

        /// <summary>No.262 シャボンペン: くちばしから シャボン玉を ふうっと吹く子。風に乗った玉が 虹色に光るのを 見るのが好き</summary>
        private static void Shabon(PenguinBrush b)
        {
            var body = Rgb(190, 175, 230);
            var bodyShade = PenguinPalette.Shade(body);
            b.Feet(32f, 5f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(19, 12), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Smile, eyeGap: 5f);
            // シャボン玉は体に重ならない所へ。大きさを変えて、吹いた順に遠くへ流れて見せる
            Bubble(b, 14f, 34f, 4f);
            Bubble(b, 10f, 46f, 3f);
            Bubble(b, 20f, 52f, 5f);
            Bubble(b, 46f, 42f, 3f);
            b.Sparkles(AuroraPink, V(40, 52), V(8, 22));
        }

        /// <summary>No.263 フウリンペン: ガラスの風鈴を まとった子。風が吹くたび ちりんと鳴り、聞いた者の 暑さも疲れも 消えていく</summary>
        private static void Fuurin(PenguinBrush b)
        {
            var glass = Rgb(185, 225, 240);
            var glassShade = PenguinPalette.Shade(glass);
            var paint = Rgb(220, 70, 90);
            var paper = Rgb(250, 240, 200);
            // 短冊は風に揺れて斜めに垂れる。風鈴の内側から糸でつなぐ
            b.Feet(32f, 6f, 12f);
            // 風鈴のガラスはお椀を伏せた形。縁を厚くして、金魚の絵（赤）を入れる
            b.Shaded(32f, 30f, 16f, 18f, glass);
            b.Rect(14, 12, 37, 5, glassShade);
            b.Erase(32f, 9f, 18f, 4f);
            b.Tilted(44f, 24f, 3f, 1.8f, 20f, paint);
            b.Triangle(V(46, 25), V(50, 28), V(50, 22), paint);
            b.Line(0.6f, paint, V(18, 18), V(24, 21), V(30, 18), V(36, 21), V(42, 18), V(46, 20));
            b.White(29f, 31f, 8f, 6f);
            b.TurnedFace(29f, 33f, FaceLeft, EyeStyle.Cross, eyeGap: 7f);
            // 短冊は風に揺れて斜めに垂れる。ガラスの縁の下へ糸でつなぐ
            b.Line(0.4f, Wood, V(34, 12), V(38, 8));
            b.Polygon(V(41, 5), paper, V(37, 9), V(41, 10), V(46, 1), V(42, 0));
            b.Rect(30, 48, 4, 4, Iron);
            b.Sparkles(PenguinPalette.EyeLight, V(22, 40), V(52, 44), V(8, 26));
        }

        /// <summary>No.264 カザグルマペン: 頭の風車が くるくる回ると 体にびりっと電気がたまる。風の強い日ほど 元気</summary>
        private static void Kazaguruma(PenguinBrush b)
        {
            var body = Rgb(240, 210, 90);
            var bodyShade = PenguinPalette.Shade(body);
            b.Feet(32f, 5f);
            b.Shaded(32f, 15f, 8.5f, 7.5f, body);
            b.Belly(30f, 14f, 5f, 5f);
            b.Line(0.6f, Wood, V(32, 32), V(32, 40));
            // 風車の羽は4色の三角。中心の周りを回る向きにそろえる
            var center = V(32, 42);
            b.Triangle(center, V(32, 50), V(38, 47), Rainbow[0]);
            b.Triangle(center, V(40, 42), V(37, 36), Rainbow[4]);
            b.Triangle(center, V(32, 34), V(26, 37), Rainbow[3]);
            b.Triangle(center, V(24, 42), V(27, 48), Rainbow[1]);
            b.Ellipse(32f, 42f, 1.2f, 1.2f, PenguinPalette.EyeLight);
            b.Shaded(32f, 26f, 8f, 7.5f, body);
            b.White(30f, 25.5f, 6f, 4.5f);
            b.Flipper2(V(25, 17), V(18, 12), 2f, bodyShade);
            b.TurnedFace(30f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 5f);
            b.Line(0.5f, Bolt, V(16, 30), V(13, 27), V(15, 25));
            b.Line(0.5f, Bolt, V(48, 30), V(51, 27), V(49, 25));
        }

        /// <summary>No.265 フウシャペン: 背中の大きな風車で 風を 電気に変える子。町じゅうの灯りを ひとりで ともしたことがある</summary>
        private static void Fuusha(PenguinBrush b)
        {
            var body = Rgb(210, 215, 225);
            var bodyShade = PenguinPalette.Shade(body);
            var blade = Rgb(245, 248, 250);
            // 3枚羽の風車を頭の後ろに。羽は付け根が太く、先が細い
            var hub = V(32, 46);
            b.Flipper(hub, V(32, 63), 2.5f, PenguinPalette.Shade(blade));
            b.Flipper(hub, V(17, 37), 2.5f, PenguinPalette.Shade(blade));
            b.Flipper(hub, V(47, 37), 2.5f, PenguinPalette.Shade(blade));
            b.Flipper(hub, V(32, 62), 1.7f, blade);
            b.Flipper(hub, V(18, 38), 1.7f, blade);
            b.Flipper(hub, V(46, 38), 1.7f, blade);
            b.Feet(32f, 8f);
            b.Shaded(32f, 19f, 14f, 13f, body);
            b.Belly(29f, 18f, 9f, 9f);
            b.Shaded(31f, 36f, 11f, 9.5f, body);
            b.Ellipse(32f, 46f, 3f, 3f, IronLight);
            b.White(28f, 35f, 8f, 6.5f);
            b.Flipper2(V(18, 22), V(10, 12), 3.5f, bodyShade);
            b.TurnedFace(28f, 37f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
            b.Line(0.8f, Bolt, V(6, 30), V(3, 26), V(7, 24), V(4, 20));
            b.Line(0.8f, Bolt, V(58, 30), V(61, 26), V(57, 24), V(60, 20));
        }

        /// <summary>No.266 ツムジペン: 足元に いつも小さな つむじ風を 巻いている子。気に入った相手の周りを ぐるぐる回って 離れない</summary>
        private static void Tsumuji(PenguinBrush b)
        {
            var body = Rgb(70, 160, 175);
            var bodyShade = PenguinPalette.Shade(body);
            // 足元のつむじ風は、下ほど小さくなる楕円の輪を重ねる
            foreach (var (y, rx) in new[] { (10f, 14f), (6f, 10f), (3f, 6f) })
            {
                b.Ellipse(32f, y, rx, 2f, WindStreak);
                b.Ellipse(32f, y + 0.6f, rx - 1.5f, 1f, PenguinPalette.Clear);
            }
            b.Shaded(32f, 19f, 9f, 8f, body);
            b.Belly(30f, 18f, 5.5f, 5.5f);
            b.Shaded(32f, 31f, 8.5f, 8f, body);
            // 頭のてっぺんの毛を うず巻きにする
            b.Line(0.8f, bodyShade, V(32, 39), V(35, 42), V(32, 45), V(28, 42), V(31, 38));
            b.White(30f, 30.5f, 6f, 4.5f);
            b.Flipper(V(25, 22), V(16, 27), 2.5f, bodyShade);
            b.Flipper(V(39, 22), V(48, 17), 2.5f, bodyShade);
            b.TurnedFace(30f, 32f, FaceLeft, EyeStyle.Sharp, eyeGap: 5f);
        }

        /// <summary>No.267 タツマキペン: 体そのものが 渦を巻く竜巻になった 風の王者。通ったあとには 雲ひとつ残らない</summary>
        private static void Tatsumaki(PenguinBrush b)
        {
            var body = Rgb(60, 130, 150);
            var bodyShade = PenguinPalette.Shade(body);
            var funnel = Rgb(170, 200, 210);
            // 竜巻の胴は上ほど太い楕円を左右にずらしながら積み、ねじれて見せる
            foreach (var (x, y, rx) in new[] { (33f, 4f, 4f), (30f, 9f, 7f), (34f, 14f, 10f), (30f, 20f, 13f), (33f, 26f, 15f) })
            {
                b.Shaded(x, y, rx, 3.5f, funnel);
                b.Line(0.5f, PenguinPalette.Shade(funnel), V(x - rx + 2f, y - 1f), V(x + rx - 3f, y + 1f));
            }
            b.Shaded(32f, 38f, 12f, 10f, body);
            b.White(29f, 37f, 8.5f, 6.5f);
            b.Flipper(V(20, 38), V(6, 46), 3.5f, bodyShade);
            b.Flipper(V(44, 38), V(58, 46), 3.5f, bodyShade);
            b.Triangle(V(28, 46), V(34, 47), V(36, 58), bodyShade);
            b.TurnedFace(29f, 39f, FaceLeft, EyeStyle.Sharp, eyeGap: 8f, open: true);
            // 巻き上げた木の葉と小石
            b.Tilted(8f, 22f, 2.5f, 1.4f, 40f, DryLeaf);
            b.Tilted(56f, 12f, 2.5f, 1.4f, -30f, BambooLeaf);
            b.Shaded(54f, 28f, 1.8f, 1.5f, Gravel);
            WindLines(b, WindStreak, (2f, 32f, 8f), (52f, 54f, 9f));
        }

        /// <summary>No.268 ヒライシンペン: 大橋の塔の てっぺんの避雷針に とまって 雷を待つ鳥。落ちた雷を 羽にためて 一気に放つ</summary>
        private static void Hiraishin(PenguinBrush b)
        {
            var body = Rgb(250, 210, 60);
            var bodyShade = PenguinPalette.Shade(body);
            var back = Rgb(50, 55, 75);
            // 避雷針の柱。足元から下へ伸ばし、とまっている所を見せる
            b.Rect(30, 0, 4, 10, Iron);
            b.Rect(31, 0, 1, 10, IronLight);
            b.Rect(24, 9, 16, 2, Iron);
            b.Feet(32f, 4f, 11f);
            b.Polygon(V(46, 24), back, V(40, 30), V(60, 14), V(56, 12), V(40, 18));
            b.Shaded(32f, 22f, 11f, 10f, body);
            b.Belly(30f, 21f, 6.5f, 6.5f);
            b.Shaded(31f, 37f, 9f, 8.5f, body);
            // 頭の冠羽は稲妻の形に
            b.Polygon(V(33, 48), back, V(30, 44), V(35, 44), V(34, 49), V(38, 50), V(30, 58), V(32, 51), V(28, 51));
            b.White(28f, 36f, 6.5f, 5f);
            b.Flipper(V(21, 26), V(12, 34), 3f, back);
            b.TurnedFace(28f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            b.Line(1f, Bolt, V(6, 60), V(10, 52), V(6, 50), V(12, 42));
            b.Line(0.6f, BoltShade, V(50, 58), V(54, 52), V(51, 50), V(56, 44));
        }

        /// <summary>No.269 オオナマズペン: 池の底に 何十年も住む「ぬし」。長いひげで 水の揺れを感じ、めったに 釣り糸には かからない</summary>
        private static void Oonamazu(PenguinBrush b)
        {
            var body = Rgb(85, 95, 70);
            var bodyShade = PenguinPalette.Shade(body);
            b.Ellipse(32f, 5f, 26f, 4f, DeepWater);
            b.Ellipse(32f, 6f, 22f, 2f, Water);
            // 横に平たく大きな体。頭も体と同じくらい幅を広げる
            b.Shaded(32f, 17f, 22f, 12f, body);
            b.Belly(30f, 14f, 15f, 7f);
            b.Shaded(30f, 34f, 17f, 10f, body);
            b.White(28f, 34f, 10f, 5f);
            b.Flipper2(V(12, 16), V(6, 12), 3f, bodyShade);
            b.TurnedFace(28f, 37f, FaceLeft, EyeStyle.Dot, eyeGap: 12f);
            // 長いひげはくちばしの付け根から前へ垂らす。顔より後に描いて、ひげの根元を顔の上に出す
            b.Line(0.6f, bodyShade, V(20, 32), V(10, 34), V(3, 30), V(2, 24));
            b.Line(0.6f, bodyShade, V(20, 30), V(12, 27), V(9, 21));
            b.Line(0.5f, WindStreak, V(52, 44), V(56, 46));
        }

        /// <summary>No.270 ニジバネペン: 大橋の空に まれに現れる 虹の羽の子。背中の虹は 見た者の願いを 1つだけ 空へ届けるという</summary>
        private static void Nijibane(PenguinBrush b)
        {
            var body = Rgb(245, 245, 255);
            var bodyShade = PenguinPalette.Shade(body);
            // 背中に虹の弧。外の色から順に半径を小さくして重ね、最後に内側を抜く
            for (int i = 0; i < Rainbow.Length; i++)
            {
                float radius = 30f - i * 2.2f;
                b.Ellipse(32f, 22f, radius, radius, Rainbow[i]);
            }
            b.Erase(32f, 22f, 16.5f, 16.5f);
            b.Erase(32f, 2f, 32f, 10f);
            // 翼も虹色の帯。付け根から先へ色を変えた羽を並べる
            for (int i = 0; i < Rainbow.Length; i++)
            {
                float t = i / (Rainbow.Length - 1f);
                b.Flipper(V(22, 26), V(4 + t * 4f, 40 - t * 20f), 2.2f, Rainbow[i]);
                b.Flipper(V(42, 26), V(60 - t * 4f, 40 - t * 20f), 2.2f, Rainbow[i]);
            }
            b.Feet(32f, 6f, 13f);
            b.Shaded(32f, 24f, 10f, 9.5f, body);
            b.Belly(30f, 23f, 6.5f, 6.5f);
            b.Shaded(31f, 39f, 9f, 8.5f, body);
            b.White(29f, 38.5f, 6.5f, 5f);
            // 冠羽は3色の長い羽
            b.Line(1f, Rainbow[0], V(30, 46), V(26, 56));
            b.Line(1f, Rainbow[2], V(32, 47), V(32, 58));
            b.Line(1f, Rainbow[4], V(34, 46), V(38, 56));
            b.Line(0.5f, bodyShade, V(26, 18), V(30, 20));
            b.TurnedFace(29f, 40f, FaceLeft, EyeStyle.Cross, eyeGap: 7f);
            b.Sparkles(PenguinPalette.Gold, V(6, 56), V(58, 58), V(10, 8), V(54, 6));
        }
    }
}
