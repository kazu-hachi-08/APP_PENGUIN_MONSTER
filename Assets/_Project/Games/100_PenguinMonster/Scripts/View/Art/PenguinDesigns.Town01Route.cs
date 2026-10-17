using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>区切り1の道路の子（No.160〜171。PLAN_STORY_TOWN01.md フェーズ8a）。鳥・お餅・雪・土筆・川魚の系統</summary>
    public static partial class PenguinDesigns
    {
        private static readonly Color32 BirdCap = Rgb(45, 42, 52);
        private static readonly Color32 Mochi = Rgb(245, 238, 222);
        private static readonly Color32 ParrMark = Rgb(70, 80, 95);

        /// <summary>No.160 コガラペン: ちょこまか落ち着かない子。黒い帽子と よだれかけ模様で、雪の積もった小枝の上を羽ばたきながら跳ねる</summary>
        private static void Kogara(PenguinBrush b)
        {
            var body = Rgb(160, 148, 135);
            var bodyShade = PenguinPalette.Shade(body);
            b.Line(1.3f, Wood, V(4, 7), V(60, 11));
            b.Line(1.3f, Wood, V(46, 10), V(54, 16));
            b.Line(0.7f, SnowWhite, V(6, 9), V(26, 10.5f));
            b.Line(0.7f, SnowWhite, V(40, 12), V(58, 13));
            b.Foot(29f, 10f, 0f, 0.55f);
            b.Foot(36f, 10.5f, 0f, 0.55f);
            b.Flipper(V(38, 21), V(50, 30), 2.5f, bodyShade);
            b.Shaded(32f, 18f, 8.5f, 7.5f, body);
            b.Belly(30f, 17f, 5.5f, 5f);
            b.Shaded(31f, 28f, 8f, 7.5f, body);
            b.Ellipse(31f, 33f, 8f, 3.5f, BirdCap);
            b.White(29f, 27f, 6f, 4f);
            if (b.Front)
            {
                b.Ellipse(29f, 22.5f, 2.5f, 1.5f, BirdCap);
            }
            b.Flipper(V(25, 21), V(15, 26), 2.5f, bodyShade);
            b.TurnedFace(29f, 28f, FaceLeft, EyeStyle.Dot, eyeGap: 5f, open: true);
        }

        /// <summary>No.161 ツグミペン: 胸を張る見栄っ張り。雪の上で背すじを伸ばし、まだら模様の胸を突き出して空を見上げる</summary>
        private static void Tsugumi(PenguinBrush b)
        {
            var body = Rgb(125, 95, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var brow = Rgb(235, 220, 190);
            b.Foot(36f, 3f);
            b.Foot(26f, 3f, 10f);
            b.Flipper(V(41, 22), V(52, 12), 2.5f, bodyShade);
            b.ShadedTilted(32f, 17f, 11f, 12f, 8f, body);
            b.BellyTilted(29f, 18f, 7.5f, 9f, 8f);
            if (b.Front)
            {
                foreach (var spot in new[] { V(26, 22), V(30, 24), V(24, 17), V(29, 19), V(33, 21), V(27, 13), V(31, 15) })
                {
                    b.Ellipse(spot.x, spot.y, 1.2f, 0.9f, bodyShade);
                }
            }
            b.Shaded(27f, 34f, 10f, 9.5f, body);
            b.White(24f, 34f, 7.5f, 6f);
            b.Line(0.7f, brow, V(18, 39), V(30, 40));
            b.Flipper(V(22, 22), V(12, 14), 2.5f, bodyShade);
            b.TurnedFace(23f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
        }

        /// <summary>No.162 オオワシペン: 空の王を気取る誇り高い子。白い肩の羽を大きく広げて威嚇し、鋭い目でにらみつける</summary>
        private static void Oowashi(PenguinBrush b)
        {
            var body = Rgb(65, 48, 42);
            var bodyShade = PenguinPalette.Shade(body);
            var wingWhite = Rgb(240, 240, 235);
            // 翼は体より先に塗り、肩から外へ大きく張り出すシルエットにする
            b.Polygon(V(12, 36), bodyShade, V(22, 40), V(2, 54), V(1, 40), V(4, 28), V(20, 26));
            b.Polygon(V(52, 36), bodyShade, V(42, 40), V(62, 54), V(63, 40), V(60, 28), V(44, 26));
            b.Line(1f, wingWhite, V(20, 38), V(4, 50));
            b.Line(1f, wingWhite, V(44, 38), V(60, 50));
            b.Triangle(V(26, 6), V(38, 6), V(32, 0), wingWhite);
            b.Foot(38f, 3f);
            b.Foot(26f, 3f);
            b.Shaded(32f, 19f, 14f, 15f, body);
            b.Belly(30f, 18f, 9f, 11f);
            b.Shaded2(22f, 32f, 5f, 4f, wingWhite);
            b.Shaded(31f, 40f, 12f, 11f, body);
            b.White(28f, 39f, 9f, 7.5f);
            b.TurnedFace(28f, 40f, FaceLeft, EyeStyle.Sharp, eyeGap: 7f);
            if (b.Front)
            {
                b.SideBeak(26f, 36f, FaceLeft, length: 7f);
            }
        }

        /// <summary>No.163 オモチペン: ふにゃっと柔らかい のんき者。つきたてのお餅の体でぺたんと広がり、頭の焦げ目がぷくっと膨らんでいる</summary>
        private static void Omochi(PenguinBrush b)
        {
            var mochiShade = PenguinPalette.Darken(Mochi, 0.85f);
            var toast = Rgb(200, 150, 90);
            b.Foot(22f, 2f, 0f, 0.6f);
            b.Foot(42f, 2f, 0f, 0.6f);
            b.Shaded(32f, 11f, 14f, 9.5f, Mochi);
            b.Shaded(39f, 21f, 4f, 3.5f, Mochi);
            b.Ellipse(39f, 22f, 2.5f, 1.8f, toast);
            b.Flipper(V(20, 10), V(14, 6), 2f, mochiShade);
            b.Flipper(V(44, 10), V(50, 6), 2f, mochiShade);
            b.TurnedFace(28f, 13f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
        }

        /// <summary>No.164 カガミモチペン: どっしり構えた縁起者。三方の台にお餅を2段重ね、てっぺんに橙をのせて、新年のように堂々と座る</summary>
        private static void Kagamimochi(PenguinBrush b)
        {
            var mochiShade = PenguinPalette.Darken(Mochi, 0.85f);
            var orange = Rgb(250, 150, 40);
            var leaf = Rgb(60, 150, 70);
            var paperRed = Rgb(210, 50, 50);
            b.Rect(10, 0, 44, 4, Wood);
            b.Rect(14, 4, 36, 4, WoodLight);
            b.Rect(28, 5, 8, 2, Wood);
            b.Triangle(V(8, 9), V(56, 9), V(32, 4), SnowWhite);
            b.Line(0.6f, paperRed, V(8, 9), V(56, 9));
            b.Shaded(32f, 17f, 18f, 9f, Mochi);
            b.Flipper(V(16, 17), V(6, 22), 2.5f, mochiShade);
            b.Flipper(V(48, 17), V(58, 22), 2.5f, mochiShade);
            b.Shaded(32f, 32f, 13f, 8.5f, Mochi);
            b.Shaded(33f, 44f, 5f, 4.5f, orange);
            b.Tilted(37f, 49f, 3f, 1.5f, -20f, leaf);
            b.TurnedFace(29f, 33f, FaceLeft, EyeStyle.Line, eyeGap: 7f);
        }

        /// <summary>No.165 コナユキペン: ふわふわ漂う夢見がち。粉雪と一緒に宙に浮かび、眠そうな目で風にまかせて流れていく</summary>
        private static void Konayuki(PenguinBrush b)
        {
            var body = Rgb(205, 228, 248);
            var bodyShade = PenguinPalette.Shade(body);
            b.Foot(29f, 8f, 20f, 0.55f);
            b.Foot(36f, 9f, -10f, 0.55f);
            b.Shaded(33f, 18f, 8.5f, 7.5f, body);
            b.Belly(31f, 17f, 5.5f, 5f);
            b.Flipper(V(26, 20), V(17, 25), 2f, bodyShade);
            b.Flipper(V(40, 20), V(49, 25), 2f, bodyShade);
            b.Shaded(32f, 29f, 8f, 7.5f, body);
            b.White(30f, 28.5f, 6f, 4.5f);
            b.TurnedFace(30f, 30f, FaceLeft, EyeStyle.Sleepy, eyeGap: 5f);
            foreach (var flake in new[] { V(10, 40), V(16, 14), V(52, 44), V(56, 16), V(44, 52), V(22, 48), V(8, 26) })
            {
                b.Sparkle((int)flake.x, (int)flake.y, IceLight);
            }
        }

        /// <summary>No.166 ボタンユキペン: くるくる踊るおしゃれさん。大きな雪の結晶の冠をかぶり、片足でつま先立ちして回る</summary>
        private static void Botanyuki(PenguinBrush b)
        {
            var body = Rgb(150, 195, 235);
            var bodyShade = PenguinPalette.Shade(body);
            b.Foot(32f, 3f, 70f, 0.8f);
            b.Foot(42f, 10f, -40f, 0.8f);
            b.Flipper(V(41, 25), V(54, 34), 2.5f, bodyShade);
            b.ShadedTilted(33f, 17f, 10f, 11f, -6f, body);
            b.BellyTilted(31f, 16f, 6.5f, 8f, -6f);
            b.Shaded(31f, 33f, 10f, 9.5f, body);
            b.White(28f, 32f, 7.5f, 6.5f);
            Snowflake(b, 33f, 46f, 7f, SnowWhite);
            b.Flipper(V(23, 25), V(10, 34), 2.5f, bodyShade);
            b.TurnedFace(28f, 33f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
            Snowflake(b, 9f, 12f, 4f, IceLight);
            Snowflake(b, 55f, 50f, 4f, IceLight);
            Snowflake(b, 54f, 14f, 3f, IceLight);
        }

        /// <summary>No.167 ゴウセツペン: 静かで底知れない雪の主。頭と肩に雪を厚く積もらせ、両ヒレを掲げて頭上の雪雲から大雪を呼ぶ</summary>
        private static void Gousetsu(PenguinBrush b)
        {
            var body = Rgb(85, 135, 200);
            var bodyShade = PenguinPalette.Shade(body);
            var cloud = Rgb(170, 180, 200);
            b.Ellipse(18f, 57f, 8f, 4.5f, cloud);
            b.Ellipse(30f, 60f, 9f, 5f, cloud);
            b.Ellipse(42f, 59f, 8f, 5f, cloud);
            b.Ellipse(51f, 56f, 6f, 4f, cloud);
            b.Ellipse(33f, 55f, 18f, 3f, PenguinPalette.Shade(cloud));
            b.Flipper(V(44, 34), V(54, 50), 3f, bodyShade);
            b.Flipper(V(20, 34), V(10, 50), 3f, bodyShade);
            b.Foot(38f, 3f);
            b.Foot(26f, 3f);
            b.Shaded(32f, 19f, 14f, 15f, body);
            b.Belly(30f, 18f, 9.5f, 11f);
            b.Shaded(32f, 38f, 11.5f, 10.5f, body);
            b.White(29f, 37f, 8.5f, 7f);
            // 積もった雪は頭の上に厚く盛り、輪っかではなく雪の山に見せる
            b.Shaded(32f, 46f, 11f, 5f, SnowWhite);
            b.Shaded(34f, 50f, 6f, 3f, SnowWhite);
            b.Shaded2(19f, 31f, 4f, 2.5f, SnowWhite);
            b.TurnedFace(29f, 38f, FaceLeft, EyeStyle.Sleepy, eyeGap: 7f);
            Snowflake(b, 8f, 30f, 3.5f, SnowWhite);
            Snowflake(b, 57f, 24f, 3.5f, SnowWhite);
            Snowflake(b, 6f, 10f, 3f, IceLight);
            Snowflake(b, 58f, 8f, 3f, IceLight);
        }

        /// <summary>No.168 ツクシペン: 春を待ちきれない せっかち。土筆のように袴の節が付いた体で地面から伸び、胞子の穂を頭にのせる</summary>
        private static void Tsukushi(PenguinBrush b)
        {
            var body = Rgb(195, 165, 105);
            var bodyShade = PenguinPalette.Shade(body);
            var cone = Rgb(150, 120, 75);
            b.Ellipse(32f, 2f, 12f, 2.5f, Rgb(120, 90, 60));
            b.Foot(29f, 4f, 0f, 0.55f);
            b.Foot(36f, 4f, 0f, 0.55f);
            b.Shaded(32f, 12f, 7.5f, 8f, body);
            b.Belly(30f, 11f, 4.5f, 5f);
            b.Line(1f, bodyShade, V(25, 15), V(39, 15));
            b.Line(1f, bodyShade, V(25, 9), V(39, 9));
            b.Flipper(V(26, 13), V(21, 8), 2f, bodyShade);
            b.Flipper(V(38, 13), V(43, 18), 2f, bodyShade);
            b.Shaded(32f, 23f, 7f, 6.5f, body);
            b.Shaded(32f, 32f, 4.5f, 5.5f, cone);
            for (int y = 29; y <= 35; y += 2)
            {
                b.Dot(30, y, PenguinPalette.Shade(cone));
                b.Dot(33, y + 1, PenguinPalette.Shade(cone));
            }
            b.White(30f, 22.5f, 5f, 4f);
            b.TurnedFace(30f, 23f, FaceLeft, EyeStyle.Dot, eyeGap: 5f, open: true);
        }

        /// <summary>No.169 スギナペン: 切れ味鋭い武闘家。体から針のような葉が輪になって何段も生え、低く構えて蹴りを放つ</summary>
        private static void Sugina(PenguinBrush b)
        {
            var body = Rgb(80, 160, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var needle = Rgb(120, 200, 90);
            // 針の葉は体の両側へ斜め上に何段も伸ばし、とげとげのシルエットにする
            for (int i = 0; i < 3; i++)
            {
                float y = 14f + i * 9f;
                b.Line(0.7f, needle, V(36, y), V(56 - i * 3, y + 7));
                b.Line(0.7f, needle, V(36, y - 2), V(58 - i * 3, y + 1));
                b.Line(0.7f, needle, V(28, y), V(9 + i * 3, y + 7));
                b.Line(0.7f, needle, V(28, y - 2), V(7 + i * 3, y + 1));
            }
            b.Foot(40f, 3f, -10f);
            b.Line(2.5f, body, V(26, 14), V(12, 20));
            b.Foot(10f, 20f, 60f);
            b.ShadedTilted(33f, 17f, 10.5f, 12f, 15f, body);
            b.BellyTilted(31f, 17f, 6.5f, 8.5f, 15f);
            b.Shaded(31f, 35f, 10f, 9.5f, body);
            b.White(28f, 34f, 7.5f, 6.5f);
            b.Line(1f, needle, V(31, 44), V(29, 52));
            b.Line(1f, needle, V(34, 44), V(38, 51));
            b.Flipper(V(40, 26), V(50, 34), 2.5f, bodyShade);
            b.Flipper(V(23, 27), V(16, 33), 2.5f, bodyShade);
            b.TurnedFace(28f, 35f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
        }

        /// <summary>No.170 ヤマメペン: 元気な川の子。横腹に丸い斑の模様があり、川面から跳ね上がって水しぶきを散らす</summary>
        private static void Yamame(PenguinBrush b)
        {
            var body = Rgb(150, 172, 165);
            var bodyShade = PenguinPalette.Shade(body);
            var pink = Rgb(235, 150, 160);
            b.Ellipse(36f, 4f, 16f, 3f, Water);
            b.Line(0.7f, DeepWater, V(22, 4), V(50, 4));
            b.Triangle(V(43, 13), V(52, 7), V(51, 17), bodyShade);
            b.ShadedTilted(36f, 16f, 8f, 7f, -30f, body);
            b.BellyTilted(34f, 15f, 5f, 4.5f, -30f);
            if (b.Front)
            {
                b.Ellipse(39f, 15f, 1.5f, 2.2f, ParrMark);
                b.Ellipse(42f, 13f, 1.3f, 2f, ParrMark);
                b.Line(0.4f, pink, V(33, 18), V(43, 12));
            }
            b.Shaded(28f, 25f, 8f, 7.5f, body);
            b.White(26f, 24.5f, 6f, 4.5f);
            b.Flipper(V(31, 18), V(24, 13), 2f, bodyShade);
            b.TurnedFace(26f, 26f, FaceLeft, EyeStyle.Round, eyeGap: 5f, open: true);
            b.Ellipse(16f, 8f, 1.5f, 1.5f, Water);
            b.Ellipse(54f, 10f, 1.5f, 1.5f, Water);
            b.Ellipse(20f, 13f, 1f, 1f, IceLight);
        }

        /// <summary>No.171 サクラマスペン: 大海を旅して戻る ひたむきな子。銀の背に桜色の帯を光らせ、桜の花びらの中を矢のように跳ぶ</summary>
        private static void Sakuramasu(PenguinBrush b)
        {
            var body = Rgb(165, 180, 195);
            var bodyShade = PenguinPalette.Shade(body);
            var sakura = Rgb(240, 140, 165);
            var petal = Rgb(255, 200, 215);
            b.Triangle(V(46, 18), V(60, 8), V(60, 26), bodyShade);
            b.Triangle(V(50, 17), V(58, 12), V(58, 22), body);
            b.ShadedTilted(36f, 22f, 15f, 9f, -20f, body);
            b.BellyTilted(33f, 19f, 10f, 5f, -20f);
            b.Line(1.5f, sakura, V(24, 27), V(48, 18));
            b.Shaded(20f, 32f, 10f, 9.5f, body);
            b.White(17f, 31f, 7.5f, 6.5f);
            b.Line(1.5f, sakura, V(24, 28), V(28, 30));
            b.Flipper(V(34, 18), V(28, 8), 2.5f, bodyShade);
            b.Flipper(V(30, 28), V(42, 36), 2.5f, bodyShade);
            b.TurnedFace(17f, 33f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            foreach (var spot in new[] { V(6, 50), V(44, 48), V(56, 40), V(10, 12), V(30, 50), V(52, 54) })
            {
                b.Ellipse(spot.x, spot.y, 1.8f, 1.2f, petal);
            }
            b.Line(0.6f, Water, V(8, 4), V(20, 7), V(34, 4));
        }

        /// <summary>雪の結晶。中心を通る3本の線と、先の小枝</summary>
        private static void Snowflake(PenguinBrush b, float x, float y, float radius, Color32 color)
        {
            for (int i = 0; i < 3; i++)
            {
                float angle = Mathf.PI / 2f + i * Mathf.PI / 3f;
                var dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                var center = V(x, y);
                b.Line(0.6f, color, center - dir, center + dir);
            }
            b.Ellipse(x, y, radius * 0.25f, radius * 0.25f, color);
        }
    }
}
