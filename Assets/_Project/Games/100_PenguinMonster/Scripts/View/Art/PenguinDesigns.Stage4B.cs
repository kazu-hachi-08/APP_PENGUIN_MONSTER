using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>ふつう後半（No.101〜125）のデザイン</summary>
    public static partial class PenguinDesigns
    {
        /// <summary>No.101 サカダチペン: ひねくれ者。両ヒレで地面を突いて逆立ちし、頭が下・足が上。上の足先にろうそくの炎がともる</summary>
        private static void Sakadachi(PenguinBrush b)
        {
            var body = Rgb(225, 95, 60);
            var bodyShade = PenguinPalette.Shade(body);
            // 足の裏を上に向け、炎を足先に立てて「上下が逆」をシルエットのてっぺんで見せる
            SakadachiFlame(b, 24f, 55f);
            SakadachiFlame(b, 40f, 56f);
            b.Foot(24f, 52f, 10f);
            b.Foot(40f, 53f, -10f);
            b.ShadedTilted(32f, 36f, 11f, 14f, -6f, body);
            b.BellyTilted(30f, 36f, 7f, 10f, -6f);
            // ヒレは付け根を太くして地面へ突っ張らせ、体を支えているのが分かるようにする
            b.Flipper(V(22, 30), V(11, 4), 4f, bodyShade);
            b.Flipper(V(42, 30), V(53, 4), 4f, bodyShade);
            b.Shaded(31f, 15f, 10.5f, 10f, body);
            b.White(29f, 14f, 7.5f, 6f);
            SakadachiFace(b, 29f, 12f);
        }

        /// <summary>ろうそくの炎。しずく形を上へとがらせる</summary>
        private static void SakadachiFlame(PenguinBrush b, float x, float y)
        {
            b.Ellipse(x, y, 2.5f, 2.5f, FlameOuter);
            b.Triangle(V(x - 2.5f, y), V(x + 2.5f, y), V(x, y + 7f), FlameOuter);
            b.Ellipse(x, y, 1.2f, 1.5f, FlameInner);
        }

        /// <summary>逆さまの顔。くちばしとほっぺを目の上に置き、にっこり目も上下を返して v の形にする</summary>
        private static void SakadachiFace(PenguinBrush b, float x, float eyeY)
        {
            if (!b.Front) return;
            const float Gap = 6f;
            float farX = x - Gap * 0.35f;
            float nearX = x + Gap * 0.65f;
            b.Line(0.6f, PenguinPalette.Eye, V(farX - 2f, eyeY + 1f), V(farX, eyeY - 1f), V(farX + 2f, eyeY + 1f));
            b.Line(0.6f, PenguinPalette.Eye, V(nearX - 2f, eyeY + 1f), V(nearX, eyeY - 1f), V(nearX + 2f, eyeY + 1f));
            b.Cheek(nearX + 1f, eyeY + 2f);
            b.SideBeak(x - Gap * 0.6f, eyeY + 3.5f, FaceLeft);
        }

        /// <summary>No.102 ヤキイモペン: 食いしん坊の職人肌。ねじりはちまきで、自分ほどもある焼き芋を斜めに抱える。足元に炭火</summary>
        private static void Yakiimo(PenguinBrush b)
        {
            var body = Rgb(130, 85, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var skin = Rgb(150, 70, 140);
            var flesh = Rgb(255, 210, 90);
            var coal = Rgb(70, 64, 70);
            var steam = Rgb(235, 240, 248);
            // 炭火は体の前に小さく置き、芋が焼きたてだと分かるようにする
            b.Ellipse(9f, 4f, 4f, 2.5f, coal);
            b.Ellipse(15f, 3f, 3.5f, 2.2f, PenguinPalette.Darken(coal, 0.8f));
            b.Dot(8, 5, FlameOuter);
            b.Dot(14, 4, FlameMiddle);
            b.Dot(11, 3, FlameOuter);
            b.Foot(30f, 3f);
            b.Foot(44f, 3f, -10f);
            b.Shaded(37f, 20f, 13f, 14f, body);
            b.Flipper(V(48, 24), V(50, 14), 3f, bodyShade);
            b.Shaded(33f, 41f, 10.5f, 10f, body);
            b.White(30f, 40f, 7.5f, 6f);
            b.TurnedFace(30f, 41f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            YakiimoBand(b, 33f, 47f);
            // 芋は体の前を斜めに横切らせ、抱えた大きさで丸い体の輪郭を崩す
            b.ShadedTilted(28f, 21f, 22f, 8.5f, 28f, skin);
            // 割れ目は芋の長さに沿ったギザギザの裂け目にする。丸い塊だとチーズに見えるので
            b.Line(1.4f, flesh, V(14, 15), V(18, 19), V(21, 17), V(25, 22), V(28, 20), V(32, 25), V(35, 23), V(39, 27));
            b.Line(0.5f, Rgb(255, 245, 190), V(18, 19), V(21, 17), V(25, 22), V(28, 20), V(32, 25));
            b.Line(0.6f, PenguinPalette.Darken(skin, 0.8f), V(42, 30), V(46, 28));
            b.Line(0.6f, PenguinPalette.Darken(skin, 0.8f), V(10, 14), V(13, 11));
            b.Flipper(V(42, 24), V(30, 17), 3f, bodyShade);
            b.Line(0.6f, steam, V(17, 27), V(15, 31), V(17, 35), V(15, 39));
            b.Line(0.6f, steam, V(10, 23), V(8, 27), V(10, 31));
        }

        /// <summary>ねじりはちまき。白い帯にねじれの影を入れ、結び目を後ろへ跳ねさせる</summary>
        private static void YakiimoBand(PenguinBrush b, float x, float y)
        {
            var band = PenguinPalette.Belly;
            var twist = PenguinPalette.BellyShade;
            b.Line(1.3f, band, V(x - 10f, y - 1f), V(x + 9f, y + 1f));
            for (int i = 0; i < 6; i++)
            {
                float tx = x - 8f + i * 3f;
                b.Line(0.5f, twist, V(tx, y - 1.5f), V(tx + 1f, y + 0.5f));
            }
            b.Triangle(V(x + 8f, y + 1f), V(x + 15f, y + 6f), V(x + 13f, y), band);
            b.Triangle(V(x + 8f, y), V(x + 16f, y - 2f), V(x + 13f, y + 2f), twist);
        }

        /// <summary>No.103 ヒノマイペン: 華やかな舞い手。片足立ちでくるりと回り、両ヒレに赤い扇子を広げる。炎とオーロラの帯が体に渦を巻く</summary>
        private static void Hinomai(PenguinBrush b)
        {
            var body = Rgb(235, 120, 70);
            var bodyShade = PenguinPalette.Shade(body);
            var fan = Rgb(220, 40, 60);
            var green = Rgb(120, 240, 180);
            var violet = Rgb(190, 140, 250);
            // 帯は傾いた輪の奥の半分を先に、手前の半分を体の後に塗る。輪を傾けて両端を切り、回る勢いのらせんに見せる
            Ribbon(b, violet, HinomaiSwirl(20f, 165f));
            HinomaiFan(b, fan, V(50, 31), -50f, 40f);
            b.Flipper(V(42, 28), V(49, 31), 2.5f, bodyShade);
            // 上げた足は後ろへ高く蹴り上げ、片足立ちの回転を形で見せる
            b.Line(2f, bodyShade, V(38, 14), V(48, 12));
            b.Foot(51f, 12f, 40f);
            b.Foot(30f, 3f);
            b.ShadedTilted(32f, 20f, 10f, 13f, -10f, body);
            b.BellyTilted(29f, 19f, 6f, 9.5f, -10f);
            b.Shaded(29f, 37f, 10f, 9.5f, body);
            b.White(26f, 36f, 7f, 6f);
            b.TurnedFace(26f, 37f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            b.Flipper(V(22, 28), V(14, 44), 2.5f, bodyShade);
            HinomaiFan(b, fan, V(14, 45), 70f, 170f);
            Ribbon(b, green, HinomaiSwirl(195f, 335f));
            // 帯の切れ目から炎が燃え上がり、火の舞だと分かるようにする
            HinomaiFlame(b, HinomaiSwirl(335f, 335f)[0]);
            HinomaiFlame(b, HinomaiSwirl(180f, 180f)[0]);
        }

        /// <summary>体のまわりを回る帯の通り道。右上がりに傾けた楕円の一部（角度は度）</summary>
        private static Vector2[] HinomaiSwirl(float fromDeg, float toDeg)
        {
            const int Steps = 10;
            const float Tilt = 20f * Mathf.Deg2Rad;
            var points = new Vector2[Steps + 1];
            for (int i = 0; i <= Steps; i++)
            {
                float angle = Mathf.Lerp(fromDeg, toDeg, i / (float)Steps) * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * 27f;
                float y = Mathf.Sin(angle) * 7f;
                points[i] = V(32f + x * Mathf.Cos(Tilt) - y * Mathf.Sin(Tilt), 24f + x * Mathf.Sin(Tilt) + y * Mathf.Cos(Tilt));
            }
            return points;
        }

        private static void HinomaiFlame(PenguinBrush b, Vector2 p)
        {
            b.Triangle(V(p.x - 3.5f, p.y), V(p.x + 3.5f, p.y), V(p.x, p.y + 10f), FlameOuter);
            b.Triangle(V(p.x - 1.5f, p.y), V(p.x + 1.5f, p.y), V(p.x, p.y + 6f), FlameInner);
        }

        /// <summary>開いた扇子。要（かなめ）から扇形に広げ、骨の線と明るい縁で紙の扇だと分かるようにする</summary>
        private static void HinomaiFan(PenguinBrush b, Color32 color, Vector2 pivot, float fromDeg, float toDeg)
        {
            const float Radius = 11f;
            var arc = ArcPoints(pivot.x, pivot.y, Radius, Radius, fromDeg * Mathf.Deg2Rad, toDeg * Mathf.Deg2Rad);
            var outer = new Vector2[arc.Length + 1];
            outer[0] = pivot;
            arc.CopyTo(outer, 1);
            b.Polygon(pivot, color, outer);
            b.Line(0.8f, FlameInner, arc);
            for (int i = 2; i < arc.Length; i += 3)
            {
                b.Line(0.4f, PenguinPalette.Shade(color), pivot, arc[i]);
            }
            b.Ellipse(pivot.x, pivot.y, 1.2f, 1.2f, PenguinPalette.Shade(color));
        }

        /// <summary>No.104 チョウチンペン: のんびりしたおばけ。体そのものが赤い提灯で、上下に黒い枠。中の灯りがにじみ、ふわりと浮く</summary>
        private static void Chouchin(PenguinBrush b)
        {
            var paper = Rgb(215, 60, 55);
            var paperShade = PenguinPalette.Shade(paper);
            var cream = Rgb(255, 230, 190);
            var glow = Rgb(255, 210, 110);
            var frame = Rgb(45, 40, 48);
            var wisp = Rgb(170, 200, 255);
            b.Line(1f, frame, V(27, 52), V(29, 59), V(35, 59), V(37, 52));
            b.Ellipse(32f, 29f, 17f, 20f, paperShade);
            b.Ellipse(31f, 30f, 15.5f, 18.5f, paper);
            // 灯りは下の方からにじませ、中に火が入っている提灯に見せる
            b.Ellipse(33f, 22f, 10f, 9f, PenguinPalette.Darken(glow, 0.9f));
            b.Ellipse(33f, 21f, 6f, 5f, glow);
            b.Ellipse(19f, 31f, 2f, 12f, cream);
            for (int y = 14; y <= 46; y += 4)
            {
                float half = 16f * Mathf.Sqrt(Mathf.Max(0f, 1f - ((y - 29f) / 20f) * ((y - 29f) / 20f)));
                b.Line(0.4f, paperShade, V(32f - half, y), V(32f + half, y + 0.5f));
            }
            b.Rect(22, 47, 20, 5, frame);
            b.Rect(24, 7, 16, 4, frame);
            b.Line(0.8f, frame, V(32, 7), V(31, 2));
            b.Ellipse(31f, 2f, 1.5f, 1.5f, paper);
            b.Flipper(V(16, 28), V(8, 21), 2.5f, paperShade);
            b.Flipper(V(48, 28), V(56, 21), 2.5f, paperShade);
            b.White(27f, 34f, 7.5f, 6f);
            b.TurnedFace(27f, 35f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            b.Ellipse(55f, 46f, 2.5f, 2.5f, wisp);
            b.Triangle(V(53, 47), V(57, 47), V(58, 53), wisp);
            b.Ellipse(8f, 50f, 1.8f, 1.8f, wisp);
            b.Triangle(V(6.5f, 51), V(9.5f, 51), V(10, 55), wisp);
        }

        /// <summary>No.105 ロボペン: まじめな働き者。角ばった四角い金属の頭と胴、アンテナの先で玉が点滅し、ヒレはアームのはさみ</summary>
        private static void Robo(PenguinBrush b)
        {
            var metal = Rgb(160, 165, 178);
            var metalShade = PenguinPalette.Shade(metal);
            var metalLight = Rgb(205, 210, 220);
            var joint = Rgb(90, 92, 105);
            // 丸い子が多いので、頭も胴も四角で描いて角ばったシルエットにする
            b.Rect(24, 4, 5, 6, joint);
            b.Rect(36, 4, 5, 6, joint);
            b.Foot(26f, 3f, 0f, 0.8f);
            b.Foot(39f, 3f, 0f, 0.8f);
            RoboArm(b, joint, metalShade, V(46, 22), V(54, 14));
            RoboBox(b, 19, 9, 26, 20, metal, metalShade, metalLight);
            RoboBox(b, 15, 29, 32, 21, metal, metalShade, metalLight);
            b.Line(0.6f, joint, V(33, 50), V(35, 57));
            b.Ellipse(35.5f, 58.5f, 2.5f, 2.5f, Rgb(255, 80, 80));
            b.Dot(35, 59, PenguinPalette.EyeLight);
            b.Sparkle(41, 60, Bolt);
            b.White(28f, 39f, 9f, 7f);
            b.TurnedFace(28f, 40f, FaceLeft, EyeStyle.Dot, eyeGap: 7f);
            if (b.Front)
            {
                b.Polygon(V(31, 19), Bolt, V(32, 26), V(27, 19), V(31, 19), V(29, 12), V(35, 20), V(31, 20));
            }
            else
            {
                // 後ろ姿はのっぺりしないよう、背中にねじ止めのふたを付ける
                b.Rect(25, 13, 14, 11, metalShade);
                b.Rect(26, 14, 12, 9, metal);
                b.Line(0.4f, metalShade, V(28, 20), V(36, 20));
                b.Line(0.4f, metalShade, V(28, 17), V(36, 17));
            }
            RoboArm(b, joint, metalShade, V(18, 22), V(8, 28));
        }

        /// <summary>金属の箱。明るい面を左上、影を右下に残し、四隅にリベットを打つ</summary>
        private static void RoboBox(PenguinBrush b, int left, int bottom, int width, int height, Color32 metal, Color32 shade, Color32 light)
        {
            b.Rect(left, bottom, width, height, shade);
            b.Rect(left, bottom + 1, width - 1, height - 1, metal);
            b.Rect(left, bottom + height - 2, width - 1, 1, light);
            b.Dot(left + 1, bottom + 2, shade);
            b.Dot(left + width - 3, bottom + 2, shade);
            b.Dot(left + 1, bottom + height - 4, shade);
            b.Dot(left + width - 3, bottom + height - 4, shade);
        }

        /// <summary>アームの先に開いたはさみを付ける</summary>
        private static void RoboArm(PenguinBrush b, Color32 joint, Color32 claw, Vector2 root, Vector2 tip)
        {
            b.Line(1.2f, joint, root, tip);
            var dir = (tip - root).normalized;
            var side = new Vector2(-dir.y, dir.x);
            b.Line(1f, claw, tip, tip + dir * 4f + side * 3f);
            b.Line(1f, claw, tip, tip + dir * 4f - side * 3f);
            b.Ellipse(tip.x, tip.y, 1.8f, 1.8f, claw);
        }

        /// <summary>No.106 アンテナペン: 好奇心の強い発明好き。頭のパラボラアンテナからオーロラ色の電波をジグザグに飛ばし、ヒレのリモコンで操る</summary>
        private static void Antenna(PenguinBrush b)
        {
            var body = Rgb(130, 105, 185);
            var bodyShade = PenguinPalette.Shade(body);
            var dish = Rgb(235, 238, 245);
            var dishShade = Rgb(170, 175, 195);
            var remote = Rgb(60, 62, 75);
            var waves = new[] { Rgb(120, 240, 180), Rgb(120, 200, 250), Rgb(240, 140, 220) };
            // 電波は皿の先から同心の弧で広げ、弧をジグザグに折って「電気の波」に見せる
            for (int i = 0; i < waves.Length; i++)
            {
                AntennaWave(b, waves[i], V(45, 50), 5f + i * 4f);
            }
            b.Line(1f, dishShade, V(33, 46), V(36, 52));
            // 皿は上向きに傾け、くぼみを影色で抜いてパラボラだと分かるようにする
            b.Tilted(38f, 53f, 9f, 4f, -35f, dish);
            b.Tilted(39f, 54f, 7f, 2.2f, -35f, dishShade);
            b.Line(0.5f, dishShade, V(38, 53), V(43, 58));
            b.Ellipse(43.5f, 58.5f, 1.2f, 1.2f, Rgb(255, 80, 80));
            b.Foot(26f, 3f);
            b.Foot(40f, 3f, -15f);
            b.Flipper(V(42, 26), V(50, 16), 3f, bodyShade);
            b.ShadedTilted(33f, 19f, 12f, 14f, -8f, body);
            b.BellyTilted(30f, 18f, 8f, 10f, -8f);
            b.Shaded(30f, 37f, 10.5f, 10f, body);
            b.White(27f, 36f, 7.5f, 6f);
            b.TurnedFace(27f, 37f, FaceLeft, EyeStyle.Cross, eyeGap: 6f);
            b.Flipper(V(23, 24), V(13, 26), 3f, bodyShade);
            b.Rect(5, 24, 8, 5, remote);
            b.Dot(6, 26, Rgb(255, 80, 80));
            b.Dot(8, 26, Rgb(120, 220, 140));
            b.Line(0.5f, remote, V(6, 29), V(4, 33));
        }

        /// <summary>半径を交互に出し入れしながら弧をたどり、ジグザグの電波にする</summary>
        private static void AntennaWave(PenguinBrush b, Color32 color, Vector2 center, float radius)
        {
            const int Steps = 8;
            var points = new Vector2[Steps + 1];
            for (int i = 0; i <= Steps; i++)
            {
                float angle = Mathf.Lerp(-40f, 60f, i / (float)Steps) * Mathf.Deg2Rad;
                float r = radius + (i % 2 == 0 ? 1.2f : -1.2f);
                points[i] = OnEllipse(center.x, center.y, r, r, angle);
            }
            b.Line(0.6f, color, points);
        }

        /// <summary>No.107 ロックンペン: 目立ちたがりのロックスター。足を大きく開いて踏ん張り、稲妻形の赤いギターをかき鳴らす。紫のトゲトゲ頭とサングラス</summary>
        private static void Rockn(PenguinBrush b)
        {
            var body = Rgb(62, 62, 80);
            var bodyShade = PenguinPalette.Shade(body);
            var hair = Rgb(160, 80, 220);
            var guitar = Rgb(220, 40, 50);
            var neck = Rgb(120, 80, 50);
            var lens = Rgb(25, 25, 35);
            // 髪は後ろ上へ何本も逆立て、頭のシルエットをギザギザにする
            b.Triangle(V(24, 44), V(32, 46), V(22, 60), hair);
            b.Triangle(V(28, 45), V(36, 44), V(36, 61), hair);
            b.Triangle(V(32, 44), V(40, 40), V(47, 56), hair);
            b.Triangle(V(34, 40), V(40, 34), V(52, 44), hair);
            b.Foot(13f, 4f, 25f);
            b.Foot(51f, 4f, -25f);
            b.Line(2.5f, bodyShade, V(22, 12), V(15, 5));
            b.Line(2.5f, bodyShade, V(42, 12), V(49, 5));
            b.Shaded(32f, 20f, 12f, 12f, body);
            b.Belly(30f, 19f, 8f, 8.5f);
            b.Shaded(30f, 37f, 10.5f, 10f, body);
            b.White(27f, 36f, 7.5f, 6f);
            b.TurnedFace(27f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f, open: true);
            if (b.Front)
            {
                b.Rect(22, 37, 5, 3, lens);
                b.Rect(29, 37, 5, 3, lens);
                b.Rect(27, 38, 2, 1, lens);
                b.Dot(23, 39, PenguinPalette.EyeLight);
                b.Dot(30, 39, PenguinPalette.EyeLight);
            }
            // ギターは体の前に斜めに抱え、ネックを左上へ突き出して構えの形を作る
            b.Line(1.2f, neck, V(36, 18), V(8, 34));
            b.Rect(5, 33, 4, 4, neck);
            // ギターの胴は太い稲妻の形にして、ふつうのギターではなく雷のギターだと分かるようにする
            RocknBolt(b, PenguinPalette.Shade(guitar), 1f, -1f);
            RocknBolt(b, guitar, 0f, 0f);
            b.Line(0.5f, PenguinPalette.Belly, V(30, 22), V(38, 22));
            b.Flipper(V(40, 26), V(40, 16), 3f, bodyShade);
            b.Flipper(V(22, 26), V(16, 30), 3f, bodyShade);
            b.Sparkle(56, 28, Bolt);
            b.Sparkle(58, 18, Bolt);
            b.Line(0.6f, Bolt, V(52, 22), V(55, 24), V(53, 26));
        }

        /// <summary>稲妻の形のギターの胴。2つの三角をずらして重ね、折れ曲がった雷にする</summary>
        private static void RocknBolt(PenguinBrush b, Color32 color, float dx, float dy)
        {
            b.Triangle(V(26 + dx, 25 + dy), V(41 + dx, 25 + dy), V(38 + dx, 12 + dy), color);
            b.Triangle(V(33 + dx, 17 + dy), V(47 + dx, 17 + dy), V(54 + dx, 3 + dy), color);
        }

        /// <summary>No.108 タイコペン: 豪快な雷さま。小さな雲にあぐらをかいて浮かび、背中の太鼓の輪をばちで打つ。輪は閉じた丸で、腰に虎じまの腰巻</summary>
        private static void Taiko(PenguinBrush b)
        {
            var body = Rgb(165, 195, 85);
            var bodyShade = PenguinPalette.Shade(body);
            var drum = Rgb(160, 100, 60);
            var drumFace = Rgb(235, 215, 170);
            var ring = Rgb(110, 70, 45);
            var cloud = Rgb(225, 228, 240);
            var stick = Rgb(200, 150, 90);
            var tiger = Rgb(250, 210, 60);
            const float RingX = 32f;
            const float RingY = 34f;
            const float RingRadius = 24f;
            const int Drums = 8;
            // ライペン（No.17）の半円と違い、輪を一周閉じて丸い枠をシルエットの主役にする
            b.Line(1.2f, ring, ArcPoints(RingX, RingY, RingRadius, RingRadius, 0f, Mathf.PI * 2f));
            for (int i = 0; i < Drums; i++)
            {
                var p = OnEllipse(RingX, RingY, RingRadius, RingRadius, Mathf.PI / 2f + i * Mathf.PI * 2f / Drums);
                b.Ellipse(p.x, p.y, 4.5f, 4.5f, drum);
                b.Ellipse(p.x - 0.5f, p.y + 0.5f, 3f, 3f, drumFace);
            }
            b.Shaded(22f, 9f, 8f, 5f, cloud);
            b.Shaded(42f, 9f, 8f, 5f, cloud);
            b.Shaded(32f, 10f, 10f, 6f, cloud);
            b.Shaded(32f, 22f, 12f, 10f, body);
            b.Belly(30f, 22f, 8f, 7f);
            // あぐらの足を前へ出し、雲に座っていることを見せる
            b.Foot(24f, 14f, -15f);
            b.Foot(39f, 14f, 15f);
            b.Rect(20, 15, 24, 5, tiger);
            for (int x = 22; x < 44; x += 4)
            {
                b.Line(0.6f, PenguinPalette.Eye, V(x, 15), V(x + 1, 19));
            }
            b.Line(0.8f, stick, V(46, 32), V(52, 42));
            b.Ellipse(52.5f, 42.5f, 1.5f, 1.5f, stick);
            b.Flipper(V(42, 26), V(46, 32), 3f, bodyShade);
            b.Shaded(30f, 37f, 10.5f, 10f, body);
            b.White(27f, 36f, 7.5f, 6f);
            b.TurnedFace(27f, 37f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            b.Line(0.8f, stick, V(17, 30), V(11, 40));
            b.Ellipse(10.5f, 40.5f, 1.5f, 1.5f, stick);
            b.Flipper(V(22, 25), V(17, 30), 3f, bodyShade);
        }

        /// <summary>No.109 パチパチペン: 気が小さい静電気の毛玉。毛が逆立ってまん丸にふくらみ、輪郭がギザギザ。まわりで火花がはじけて困り顔</summary>
        private static void Pachipachi(PenguinBrush b)
        {
            var fluff = Rgb(250, 238, 160);
            var fluffShade = PenguinPalette.Shade(fluff);
            var spark = Rgb(255, 255, 255);
            b.Foot(25f, 4f, 0f, 0.8f);
            b.Foot(39f, 4f, 0f, 0.8f);
            // 毛の先を三角で外へとがらせ、ふわふわではなく静電気で逆立った毛にする
            const int Spikes = 22;
            for (int i = 0; i < Spikes; i++)
            {
                float angle = i * Mathf.PI * 2f / Spikes;
                var tip = OnEllipse(32f, 31f, 27f, 26f, angle);
                b.Triangle(OnEllipse(32f, 30f, 19f, 18f, angle - 0.16f), OnEllipse(32f, 30f, 19f, 18f, angle + 0.16f), tip, fluffShade);
            }
            b.Ellipse(32f, 30f, 20f, 19f, fluffShade);
            for (int i = 0; i < Spikes; i++)
            {
                float angle = i * Mathf.PI * 2f / Spikes;
                if (Mathf.Sin(angle) < -0.3f || Mathf.Cos(angle) > 0.6f) continue;
                b.Triangle(OnEllipse(31f, 31f, 17f, 16f, angle - 0.14f), OnEllipse(31f, 31f, 17f, 16f, angle + 0.14f), OnEllipse(31f, 31f, 22f, 21f, angle), fluff);
            }
            b.Ellipse(31f, 31f, 18f, 17f, fluff);
            b.Flipper(V(14, 26), V(8, 20), 2.5f, fluffShade);
            b.Flipper(V(50, 26), V(56, 20), 2.5f, fluffShade);
            b.White(28f, 33f, 9f, 7f);
            b.TurnedFace(28f, 34f, FaceLeft, EyeStyle.Worried, eyeGap: 7f);
            PachipachiSpark(b, spark, V(6, 46));
            PachipachiSpark(b, Bolt, V(54, 54));
            PachipachiSpark(b, spark, V(58, 12));
            PachipachiSpark(b, Bolt, V(4, 12));
        }

        /// <summary>はじける小さな火花。短い稲妻ときらめきを並べる</summary>
        private static void PachipachiSpark(PenguinBrush b, Color32 color, Vector2 p)
        {
            b.Line(0.5f, color, p + V(-3, 3), p + V(0, 1), p + V(-1, -1), p + V(2, -3));
            b.Sparkle((int)p.x + 3, (int)p.y + 3, color);
        }

        /// <summary>No.110 タイフウペン: 嵐の中でも動じない落ち着き者。雲と風のうずまきが体のまわりをぐるっと囲み、台風の目の中で静かに立つ。雨粒と稲妻</summary>
        private static void Taifuu(PenguinBrush b)
        {
            var body = Rgb(70, 95, 150);
            var bodyShade = PenguinPalette.Shade(body);
            var cloud = Rgb(175, 180, 200);
            var cloudShade = PenguinPalette.Shade(cloud);
            // うずは2本の腕が中心へ巻き込む形にし、弧の中ほどを太くして台風の雲に見せる
            TaifuuArm(b, cloudShade, cloud, 0f);
            TaifuuArm(b, cloudShade, cloud, Mathf.PI);
            b.Line(0.6f, Water, V(6, 30), V(5, 26));
            b.Line(0.6f, Water, V(58, 36), V(57, 32));
            b.Line(0.6f, Water, V(50, 4), V(49, 1));
            b.Line(0.6f, Water, V(12, 58), V(11, 55));
            b.Line(1.2f, BoltShade, V(56, 58), V(52, 52), V(56, 50), V(52, 44));
            b.Line(0.6f, Bolt, V(56, 58), V(52, 52), V(56, 50), V(52, 44));
            b.Feet(32f, 4f, 19f);
            b.Shaded(32f, 27f, 7f, 8f, body);
            b.Belly(31f, 26f, 4.5f, 5.5f);
            b.Shaded(31f, 38f, 8f, 7.5f, body);
            b.White(29f, 37f, 6f, 4.5f);
            b.TurnedFace(29f, 38f, FaceLeft, EyeStyle.Line, eyeGap: 5f);
            b.Flipper(V(25, 28), V(21, 22), 2f, bodyShade);
            b.Flipper(V(39, 28), V(43, 22), 2f, bodyShade);
        }

        /// <summary>うずまきの腕1本。外から中心へ半径を縮めながら、雲の玉を並べる</summary>
        private static void TaifuuArm(PenguinBrush b, Color32 shade, Color32 cloud, float start)
        {
            const int Puffs = 14;
            for (int pass = 0; pass < 2; pass++)
            {
                for (int i = 0; i < Puffs; i++)
                {
                    float t = i / (float)(Puffs - 1);
                    float angle = start + t * Mathf.PI * 1.5f;
                    float radius = Mathf.Lerp(30f, 13f, t);
                    float size = Mathf.Lerp(2f, 5.5f, Mathf.Sin(t * Mathf.PI));
                    var p = OnEllipse(32f, 32f, radius, radius * 0.95f, angle);
                    if (pass == 0) b.Ellipse(p.x, p.y, size, size, shade);
                    else b.Ellipse(p.x - 0.7f, p.y + 0.7f, size - 1.2f, size - 1.2f, cloud);
                }
            }
        }

        /// <summary>No.111 サボテンペン: 我慢強い。サボテンの着ぐるみで植木鉢に立ち、両ヒレをサボテンの腕のように上へ曲げる。頭にピンクの花</summary>
        private static void Saboten(PenguinBrush b)
        {
            var green = Rgb(90, 165, 85);
            var greenShade = PenguinPalette.Shade(green);
            var spine = Rgb(245, 245, 230);
            var pot = Rgb(200, 110, 70);
            var potShade = PenguinPalette.Shade(pot);
            var flower = Rgb(255, 130, 180);
            // 腕は横へ出してから直角に上へ曲げ、サワロ形のサボテンのシルエットにする
            b.Line(3f, greenShade, V(44, 26), V(53, 26), V(53, 38));
            b.Line(2.2f, green, V(44, 27), V(52, 27), V(52, 38));
            b.Shaded(32f, 33f, 12f, 21f, green);
            for (int x = 24; x <= 40; x += 5)
            {
                b.Line(0.4f, greenShade, V(x, 16), V(x, 48 - Mathf.Abs(x - 32) * 0.6f));
            }
            b.Line(3f, greenShade, V(21, 30), V(11, 30), V(11, 44));
            b.Line(2.2f, green, V(21, 31), V(12, 31), V(12, 44));
            SabotenSpines(b, spine);
            b.White(29f, 38f, 7.5f, 6f);
            b.TurnedFace(29f, 39f, FaceLeft, EyeStyle.Dot, eyeGap: 6f);
            b.Ellipse(33f, 55f, 3.5f, 3f, flower);
            b.Ellipse(29f, 54f, 2.5f, 2.5f, flower);
            b.Ellipse(37f, 54f, 2.5f, 2.5f, flower);
            b.Ellipse(33f, 54.5f, 1.2f, 1.2f, Rgb(255, 230, 120));
            // 鉢はすそすぼまりの台形にして、足が鉢に入っていることを見せる
            b.Polygon(V(32, 8), potShade, V(18, 16), V(46, 16), V(42, 1), V(22, 1));
            b.Polygon(V(31, 8), pot, V(19, 15), V(44, 15), V(40, 2), V(23, 2));
            b.Rect(16, 13, 32, 4, potShade);
            b.Rect(16, 14, 31, 3, pot);
        }

        /// <summary>白いトゲの点。顔にかからない所だけに散らす</summary>
        private static void SabotenSpines(PenguinBrush b, Color32 spine)
        {
            int[] xs = { 24, 40, 22, 42, 26, 38, 24, 40, 30, 35, 11, 53, 9, 55 };
            int[] ys = { 46, 46, 28, 30, 20, 22, 36, 38, 18, 24, 38, 33, 34, 30 };
            for (int i = 0; i < xs.Length; i++)
            {
                b.Dot(xs[i], ys[i], spine);
                b.Dot(xs[i] - 1, ys[i] + 1, spine);
            }
        }

        /// <summary>No.112 マリモペン: 無口でのんびり。体そのものがまん丸のマリモの毛玉で、顔と小さな足だけ出る。水の泡がのぼる</summary>
        private static void Marimo(PenguinBrush b)
        {
            var moss = Rgb(55, 115, 60);
            var mossShade = PenguinPalette.Darken(moss, 0.72f);
            var bubble = Rgb(170, 225, 250);
            b.Foot(26f, 4f, 0f, 0.7f);
            b.Foot(38f, 4f, 0f, 0.7f);
            // 毛玉は手足を埋めるほど丸くして、ほかの子より丸だけのシルエットにする
            FluffBall(b, 32f, 27f, 18f, 18f, moss, mossShade);
            for (int i = 0; i < 10; i++)
            {
                float angle = i * 2.4f;
                var p = OnEllipse(32f, 27f, 13f - i, 12f - i, angle);
                b.Dot((int)p.x, (int)p.y, Rgb(90, 155, 90));
            }
            b.White(29f, 29f, 8f, 6f);
            b.TurnedFace(29f, 30f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
            MarimoBubble(b, bubble, 52f, 46f, 3f);
            MarimoBubble(b, bubble, 46f, 56f, 2f);
            MarimoBubble(b, bubble, 55f, 60f, 1.5f);
            MarimoBubble(b, bubble, 10f, 52f, 2f);
        }

        /// <summary>水の泡。輪だけ塗って中を抜き、左上に光を入れる</summary>
        private static void MarimoBubble(PenguinBrush b, Color32 color, float x, float y, float r)
        {
            b.Ellipse(x, y, r, r, color);
            if (r > 2f) b.Ellipse(x, y, r - 1f, r - 1f, PenguinPalette.Darken(color, 0.85f));
            b.Dot((int)(x - r * 0.5f), (int)(y + r * 0.3f), PenguinPalette.EyeLight);
        }

        /// <summary>No.113 マッチャペン: 落ち着いた茶人。緑の羽織で正座し、前に置いた茶碗の抹茶を茶せんで点てる。低く横に広いシルエット</summary>
        private static void Matcha(PenguinBrush b)
        {
            var haori = Rgb(90, 140, 80);
            var haoriShade = PenguinPalette.Shade(haori);
            var head = Rgb(60, 70, 90);
            var bowl = Rgb(70, 50, 45);
            var tea = Rgb(150, 200, 90);
            var bamboo = Rgb(220, 200, 140);
            // 正座の体は横に広く低く置き、頭も低くして静かに座る形にする
            b.Shaded(37f, 14f, 17f, 12f, haori);
            b.BellyTilted(31f, 15f, 6f, 8f, 10f);
            if (b.Front)
            {
                b.Line(0.8f, haoriShade, V(26, 22), V(33, 6));
                b.Line(0.8f, haoriShade, V(37, 22), V(36, 6));
            }
            b.Shaded(32f, 31f, 10.5f, 10f, head);
            b.White(29f, 30f, 7.5f, 6f);
            b.TurnedFace(29f, 31f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            b.Ellipse(12f, 7f, 8f, 6f, bowl);
            b.Rect(3, 7, 17, 7, PenguinPalette.Clear);
            b.Ellipse(12f, 7f, 8f, 2.5f, PenguinPalette.Shade(tea));
            b.Ellipse(11.5f, 7.5f, 6.5f, 1.8f, tea);
            b.Rect(9, 1, 6, 2, bowl);
            // 茶せんは穂先を茶碗に入れ、ヒレで柄を持たせる
            b.Rect(13, 8, 4, 5, bamboo);
            for (int x = 13; x <= 16; x++) b.Dot(x, 9 + (x % 2), PenguinPalette.Shade(bamboo));
            b.Line(0.8f, bamboo, V(15, 13), V(17, 18));
            b.Flipper(V(26, 16), V(18, 17), 3f, haoriShade);
        }

        /// <summary>No.114 ボンサイペン: 物知りの老師。四角い青い鉢に座り、頭からくねった松の盆栽が生える。白い長い眉毛</summary>
        private static void Bonsai(PenguinBrush b)
        {
            var body = Rgb(95, 110, 95);
            var bodyShade = PenguinPalette.Shade(body);
            var trunk = Rgb(120, 80, 55);
            var pine = Rgb(70, 140, 70);
            var pot = Rgb(70, 110, 180);
            var potShade = PenguinPalette.Shade(pot);
            // 幹は左右に折れ曲がりながら伸ばし、葉は平たい雲の固まりにして盆栽の松らしくする
            b.Line(1.8f, trunk, V(32, 40), V(36, 46), V(31, 51), V(40, 56), V(48, 54));
            b.Line(1.2f, trunk, V(34, 50), V(22, 53));
            BonsaiCloud(b, pine, 48f, 56f, 9f, 3.5f);
            BonsaiCloud(b, pine, 20f, 54f, 7f, 3f);
            BonsaiCloud(b, pine, 40f, 47f, 5f, 2.5f);
            b.Flipper(V(42, 20), V(48, 18), 3f, bodyShade);
            b.Shaded(32f, 21f, 11f, 9f, body);
            b.Belly(30f, 20f, 7f, 6f);
            b.Shaded(31f, 34f, 10.5f, 10f, body);
            b.White(28f, 33f, 7.5f, 6f);
            b.TurnedFace(28f, 34f, FaceLeft, EyeStyle.Line, eyeGap: 6f);
            if (b.Front)
            {
                b.Line(0.7f, PenguinPalette.Belly, V(22, 37), V(25, 37), V(20, 33));
                b.Line(0.7f, PenguinPalette.Belly, V(29, 37), V(33, 37), V(34, 34));
            }
            b.Flipper(V(22, 20), V(16, 16), 3f, bodyShade);
            // 鉢は横長の四角にして、サボテンペンのすぼまった鉢と形で分ける
            b.Rect(8, 2, 48, 13, potShade);
            b.Rect(8, 4, 46, 11, pot);
            b.Rect(6, 13, 52, 3, potShade);
            b.Rect(6, 14, 51, 2, pot);
            b.Rect(10, 1, 5, 2, potShade);
            b.Rect(49, 1, 5, 2, potShade);
        }

        /// <summary>雲のような葉の固まり。平たい楕円を3つ並べ、下側に影を残す</summary>
        private static void BonsaiCloud(PenguinBrush b, Color32 color, float x, float y, float radiusX, float radiusY)
        {
            var shade = PenguinPalette.Shade(color);
            b.Ellipse(x, y, radiusX, radiusY, shade);
            b.Ellipse(x - radiusX * 0.5f, y + 1f, radiusX * 0.6f, radiusY, shade);
            b.Ellipse(x - 0.5f, y + 1f, radiusX - 1.5f, radiusY - 1.2f, color);
            b.Ellipse(x - radiusX * 0.5f, y + 1.8f, radiusX * 0.5f, radiusY - 1f, color);
        }

        /// <summary>No.115 ハエトリソウペン: にやりと笑う策士。ハエトリソウのフードをかぶり、上下2枚の大きな葉が口のように開いて頭をはさむ。ふちにトゲ</summary>
        private static void Haetorisou(PenguinBrush b)
        {
            var body = Rgb(55, 60, 95);
            var bodyShade = PenguinPalette.Shade(body);
            var leaf = Rgb(110, 185, 70);
            var leafShade = PenguinPalette.Shade(leaf);
            var inside = Rgb(215, 60, 70);
            b.Feet(32f, 6f);
            b.Shaded(33f, 17f, 11f, 12f, body);
            b.Belly(31f, 16f, 7f, 8.5f);
            // 2枚の葉は頭の後ろのちょうつがいでつながり、左（前）へ口を開ける。別々の円盤に見えないようにするため
            HaetorisouLeaf(b, leaf, leafShade, inside, V(31, 30f), 32f, false);
            b.Flipper(V(42, 20), V(48, 12), 3f, bodyShade);
            b.Shaded(28f, 38f, 10f, 9.5f, body);
            b.White(25f, 37f, 7f, 6f);
            b.TurnedFace(25f, 38f, FaceLeft, EyeStyle.Sharp, eyeGap: 6f);
            HaetorisouLeaf(b, leaf, leafShade, inside, V(31, 46f), -32f, true);
            b.Ellipse(45f, 38f, 2.5f, 2.5f, leafShade);
            b.Flipper(V(23, 20), V(17, 13), 3f, bodyShade);
        }

        /// <summary>ハエトリソウの葉1枚。内側の赤を開いた口の側に見せ、ふちに細いトゲを並べる</summary>
        private static void HaetorisouLeaf(PenguinBrush b, Color32 leaf, Color32 shade, Color32 inside, Vector2 c, float angle, bool upper)
        {
            const int Teeth = 9;
            float rad = angle * Mathf.Deg2Rad;
            float side = upper ? 1f : -1f;
            for (int i = 0; i < Teeth; i++)
            {
                float t = Mathf.Lerp(-0.9f, 0.9f, i / (float)(Teeth - 1));
                float lx = t * 16f;
                float ly = side * 5.5f * Mathf.Sqrt(1f - t * t);
                var root = V(c.x + lx * Mathf.Cos(rad) - ly * Mathf.Sin(rad), c.y + lx * Mathf.Sin(rad) + ly * Mathf.Cos(rad));
                var tip = root + V(-Mathf.Sin(rad), Mathf.Cos(rad)) * side * 3.5f;
                b.Line(0.5f, leaf, root, tip);
            }
            b.Tilted(c.x, c.y, 15.5f, 6f, angle, shade);
            b.Tilted(c.x - 0.5f, c.y + 0.5f, 14f, 4.5f, angle, leaf);
            var inner = V(c.x + Mathf.Sin(rad) * side * 2f, c.y - Mathf.Cos(rad) * side * 2f);
            b.Tilted(inner.x, inner.y, 12f, 3f, angle, inside);
        }

        /// <summary>No.116 コイシドロボウペン: ずる賢いこそ泥（アデリーペンギン）。体を低くかがめて抜き足差し足、口に小石をくわえ、背中に石の袋。目のまわりに泥棒のマスク</summary>
        private static void Koishidorobou(PenguinBrush b)
        {
            var body = Rgb(45, 48, 62);
            var bodyShade = PenguinPalette.Shade(body);
            var sack = Rgb(170, 125, 80);
            var sackShade = PenguinPalette.Shade(sack);
            var mask = Rgb(25, 25, 35);
            // 袋は背中の上へ大きく背負わせ、かがんだ体の上にこぶのような山を作る
            b.Shaded(45f, 33f, 11f, 11f, sack);
            b.Ellipse(41f, 43f, 4f, 2f, sackShade);
            b.Shaded(39f, 45f, 2.5f, 2f, Stone);
            b.Shaded(44f, 45f, 2.5f, 2.2f, Stone);
            b.Line(0.6f, sackShade, V(48, 38), V(52, 30));
            // 抜き足: 後ろの足はつま先立ちで浮かせ、前の足だけそっと地面に付ける
            b.Foot(46f, 7f, 35f, 0.9f);
            b.Foot(22f, 3f, -10f, 0.9f);
            b.ShadedTilted(33f, 17f, 12f, 10f, 25f, body);
            b.BellyTilted(29f, 15f, 7f, 6.5f, 25f);
            b.Flipper(V(38, 24), V(42, 34), 2.5f, bodyShade);
            b.Shaded(20f, 26f, 9.5f, 9f, body);
            b.White(18f, 25f, 7f, 5.5f);
            if (b.Front)
            {
                // マスクの中に白い輪を残し、アデリーペンギンの白いアイリングにする
                b.Rect(11, 25, 16, 5, mask);
                b.Ellipse(16f, 27.5f, 2.3f, 2.3f, PenguinPalette.Belly);
                b.Ellipse(22f, 27.5f, 2.3f, 2.3f, PenguinPalette.Belly);
            }
            b.TurnedFace(18f, 27f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
            b.Shaded(9f, 23f, 2.2f, 2f, Stone);
            b.Flipper(V(24, 18), V(30, 10), 2.5f, bodyShade);
        }

        /// <summary>No.117 ツルハシペン: 働き者の炭鉱夫。ライト付きのヘルメットで前を照らし、肩にツルハシを担いで歩く。足元に光る水晶</summary>
        private static void Tsuruhashi(PenguinBrush b)
        {
            var body = Rgb(85, 85, 100);
            var bodyShade = PenguinPalette.Shade(body);
            var helmet = Rgb(245, 200, 60);
            var helmetShade = PenguinPalette.Shade(helmet);
            var overall = Rgb(140, 95, 60);
            var handle = Rgb(160, 110, 70);
            var iron = Rgb(150, 155, 165);
            var beam = Rgb(255, 245, 190);
            var crystal = Rgb(140, 220, 245);
            // ライトの光は前へ長く伸ばし、暗い坑道を照らす向きをシルエットに出す
            b.Triangle(V(18, 47), V(1, 54), V(1, 40), beam);
            // ツルハシは肩から後ろ上へ担ぎ、先の鉄を頭の後ろで大きく張り出させる
            b.Line(1f, handle, V(22, 24), V(48, 52));
            b.Polygon(V(47, 51), iron, V(36, 58), V(42, 56), V(48, 53), V(54, 46), V(59, 38), V(56, 46), V(50, 52), V(44, 58));
            b.Foot(28f, 3f, 10f);
            b.Foot(42f, 4f, -20f);
            b.Shaded(34f, 19f, 12f, 14f, body);
            b.Belly(31f, 19f, 7.5f, 9f);
            b.Rect(22, 6, 23, 10, overall);
            b.Rect(25, 15, 4, 8, overall);
            b.Rect(37, 15, 4, 8, overall);
            b.Dot(27, 21, helmet);
            b.Dot(39, 21, helmet);
            b.Shaded(31f, 36f, 10.5f, 10f, body);
            b.White(28f, 35f, 7.5f, 6f);
            b.TurnedFace(28f, 36f, FaceLeft, EyeStyle.Dot, eyeGap: 6f);
            b.Ellipse(31f, 44f, 11f, 6f, helmetShade);
            b.Ellipse(30f, 45f, 10f, 5f, helmet);
            b.Rect(18, 42, 26, 2, helmetShade);
            b.Ellipse(19f, 46f, 2.5f, 2.5f, PenguinPalette.Belly);
            b.Flipper(V(24, 24), V(22, 30), 3f, bodyShade);
            TsuruhashiCrystal(b, crystal, 10f, 2f, 0f);
            TsuruhashiCrystal(b, Rgb(200, 160, 245), 15f, 2f, -20f);
            b.Sparkle(8, 13, PenguinPalette.Belly);
        }

        /// <summary>水晶のかけら。細長い六角柱を傾けて立て、明るい面を左に入れる</summary>
        private static void TsuruhashiCrystal(PenguinBrush b, Color32 color, float x, float y, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            var up = V(-Mathf.Sin(rad), Mathf.Cos(rad));
            var side = V(Mathf.Cos(rad), Mathf.Sin(rad));
            var root = V(x, y);
            b.Polygon(root + up * 4f, PenguinPalette.Shade(color), root - side * 2f, root + side * 2f, root + side * 2f + up * 6f, root + up * 9f, root - side * 2f + up * 6f);
            b.Triangle(root - side * 2f, root, root + up * 9f, color);
            b.Line(0.4f, color, root - side * 2f, root - side * 2f + up * 6f);
        }

        /// <summary>No.118 ハニワペン: のんきな踊り好き。素焼きの円筒の体で、片ヒレを上げ片ヒレを下げる踊る埴輪のポーズ。目は黒い穴</summary>
        private static void Haniwa(PenguinBrush b)
        {
            var clay = Rgb(205, 125, 80);
            var clayShade = PenguinPalette.Shade(clay);
            var hole = Rgb(40, 25, 20);
            // 腕は細い筒を折り曲げ、上げた腕と下げた腕で埴輪の踊りの形をそのまま見せる
            b.Line(2.5f, clayShade, V(22, 36), V(13, 37), V(11, 49));
            b.Line(1.8f, clay, V(22, 37), V(13, 38), V(12, 49));
            b.Line(2.5f, clayShade, V(42, 30), V(51, 28), V(52, 18));
            // 体は下へ少し広がる円筒。上は平らな頭にして、丸いペンギンと違う素焼きの筒らしさを出す
            b.Polygon(V(32, 20), clayShade, V(20, 2), V(45, 2), V(43, 40), V(43, 52), V(39, 56), V(25, 56), V(21, 52), V(21, 40));
            b.Polygon(V(31, 21), clay, V(21, 3), V(43, 3), V(41, 40), V(41, 52), V(38, 55), V(25, 55), V(22, 52), V(22, 40));
            b.Rect(20, 2, 25, 3, clayShade);
            b.Line(0.6f, clayShade, V(23, 33), V(40, 33));
            b.Line(1.8f, clay, V(42, 31), V(50, 29), V(51, 18));
            if (b.Front)
            {
                // 普通の目の代わりに縦長の黒い穴を2つあけ、くちばしだけを残す                b.Ellipse(26f, 46f, 1.8f, 2.8f, hole);
                b.Ellipse(34f, 46f, 1.8f, 2.8f, hole);
                b.SideBeak(29f, 40f, FaceLeft, false, 3f);
            }
            else
            {
                b.Line(0.5f, clayShade, V(28, 48), V(30, 42));
                b.Line(0.5f, clayShade, V(34, 20), V(36, 12));
            }
        }

        /// <summary>No.119 ドグウペン: 謎めいた古代の守り手。遮光器土偶。横長のゴーグルの目にオーロラの光、ずんぐりした体に縄文の渦巻き、頭に冠、太い足</summary>
        private static void Dogu(PenguinBrush b)
        {
            var clay = Rgb(160, 140, 115);
            var clayShade = PenguinPalette.Shade(clay);
            var goggle = Rgb(80, 65, 58);
            var glow = Rgb(130, 240, 200);
            // 足は太い丸太のように2本立て、肩を横に張って土偶のずんぐりした形にする
            b.Shaded(24f, 9f, 6f, 8f, clay);
            b.Shaded(41f, 9f, 6f, 8f, clay);
            b.Shaded2(13f, 26f, 5f, 6f, clay);
            b.Shaded(32f, 23f, 15f, 12f, clay);
            DoguSpiral(b, clayShade, 25f, 22f);
            DoguSpiral(b, clayShade, 39f, 22f);
            b.Line(0.5f, clayShade, V(22, 14), V(42, 14));
            // 冠は台形の上に平たいまげを乗せる。頭の両脇に丸いこぶを置くとクマの耳に見えるので
            b.Polygon(V(32, 50), clayShade, V(19, 46), V(45, 46), V(40, 56), V(24, 56));
            b.Polygon(V(31, 50), clay, V(20, 47), V(43, 47), V(39, 55), V(25, 55));
            b.Line(0.5f, clayShade, V(23, 51), V(26, 54), V(29, 51), V(32, 54), V(35, 51), V(38, 54), V(41, 51));
            b.Shaded(32f, 58f, 6f, 3f, clay);
            b.Shaded(32f, 40f, 13f, 10f, clay);
            b.White(30f, 37f, 8f, 5f);
            if (b.Front)
            {
                DoguGoggle(b, goggle, glow, 25f, 41f);
                DoguGoggle(b, goggle, glow, 37f, 41f);
                b.SideBeak(29f, 35f, FaceLeft);
                b.Cheek(36f, 34f);
                b.Cheek(23f, 34f);
            }
            else
            {
                DoguSpiral(b, clayShade, 32f, 40f);
            }
        }

        /// <summary>遮光器の目。横長の楕円に横線の切れ目を入れ、中にオーロラ色の光を通す</summary>
        private static void DoguGoggle(PenguinBrush b, Color32 goggle, Color32 glow, float x, float y)
        {
            b.Ellipse(x, y, 6f, 3.5f, goggle);
            b.Line(0.5f, glow, V(x - 3.5f, y), V(x + 3.5f, y));
        }

        /// <summary>縄文の渦巻き。半径を縮めながら1周半まわす</summary>
        private static void DoguSpiral(PenguinBrush b, Color32 color, float x, float y)
        {
            const int Steps = 12;
            var points = new Vector2[Steps + 1];
            for (int i = 0; i <= Steps; i++)
            {
                float t = i / (float)Steps;
                points[i] = OnEllipse(x, y, 4f * (1f - t * 0.8f), 4f * (1f - t * 0.8f), t * Mathf.PI * 3f);
            }
            b.Line(0.4f, color, points);
        }

        /// <summary>No.120 ウチュウペン: 夢を叶えた冒険家。白い宇宙服で斜めに浮かび、丸い金魚鉢のヘルメットと背中の酸素ボンベ。命綱がうねり、隅に輪のある惑星</summary>
        private static void Uchuu(PenguinBrush b)
        {
            var suit = Rgb(235, 238, 245);
            var suitShade = Rgb(170, 175, 195);
            var head = Rgb(55, 65, 110);
            var glass = Rgb(190, 225, 245);
            var tank = Rgb(150, 155, 170);
            var planet = Rgb(200, 170, 240);
            var tether = Rgb(240, 200, 120);
            b.Ellipse(53f, 53f, 5f, 5f, PenguinPalette.Shade(planet));
            b.Ellipse(52.5f, 53.5f, 4f, 4f, planet);
            b.Tilted(53f, 53f, 9f, 2f, 20f, Rgb(240, 220, 160));
            b.Ellipse(54f, 55f, 3f, 2f, planet);
            // 命綱はボンベから右下へ大きくうねらせ、無重力で漂っている感じを出す
            b.Line(0.7f, tether, V(44, 20), V(50, 14), V(46, 8), V(54, 4), V(62, 6));
            b.Rect(38, 14, 9, 16, PenguinPalette.Shade(tank));
            b.Rect(38, 15, 8, 15, tank);
            b.Foot(38f, 8f, 60f, 0.8f);
            b.Foot(46f, 10f, 70f, 0.8f);
            b.Tilted(36f, 13f, 4f, 5f, 30f, suitShade);
            b.Tilted(43f, 15f, 4f, 5f, 40f, suitShade);
            b.Tilted(33f, 24f, 11f, 13f, 35f, suitShade);
            b.Tilted(32f, 25f, 9.5f, 11.5f, 35f, suit);
            b.Rect(27, 22, 6, 4, Rgb(220, 70, 70));
            b.Flipper(V(26, 22), V(14, 14), 3.5f, suitShade);
            b.Flipper(V(26, 22), V(14, 14), 2.5f, suit);
            // ヘルメットは顔より大きなガラス玉にして、頭のまわりに丸い大きな形を作る
            b.Ellipse(23f, 40f, 13f, 13f, suitShade);
            b.Ellipse(23f, 40f, 12f, 12f, glass);
            b.Shaded(22f, 39f, 9f, 8.5f, head);
            b.White(20f, 38f, 6.5f, 5.5f);
            b.TurnedFace(20f, 39f, FaceLeft, EyeStyle.Round, eyeGap: 6f);
            b.Line(0.6f, PenguinPalette.Belly, V(15, 48), V(19, 50), V(24, 51));
            b.Dot(13, 45, PenguinPalette.Belly);
            b.Sparkle(6, 58, PenguinPalette.Belly);
            b.Sparkle(60, 30, PenguinPalette.Belly);
            b.Dot(4, 28, PenguinPalette.Belly);
        }

        /// <summary>No.121 ペンキペン: 自由な芸術家。ベレー帽で、虹色のペンキがしたたる大きな絵筆を振り上げ、反対のヒレにパレット。体にもペンキが飛び散る</summary>
        private static void Penki(PenguinBrush b)
        {
            var body = Rgb(50, 62, 105);
            var bodyShade = PenguinPalette.Shade(body);
            var beret = Rgb(215, 50, 60);
            var palette = Rgb(225, 190, 140);
            var handle = Rgb(170, 120, 75);
            var paints = new[] { Rgb(240, 70, 70), Rgb(255, 200, 60), Rgb(90, 200, 110), Rgb(80, 150, 240), Rgb(180, 100, 230) };
            // パレットは体の後ろ右に大きく構え、親指の穴を抜いて画材だと分かる形にする
            b.Tilted(51f, 24f, 9f, 6.5f, 20f, PenguinPalette.Shade(palette));
            b.Tilted(50.5f, 24.5f, 8f, 5.5f, 20f, palette);
            b.Erase(55f, 22f, 1.5f, 1.5f);
            for (int i = 0; i < 4; i++)
            {
                var p = OnEllipse(50f, 25f, 5.5f, 3.5f, 1.2f + i * 0.9f);
                b.Ellipse(p.x, p.y, 1.3f, 1.3f, paints[i]);
            }
            b.Flipper(V(42, 24), V(48, 22), 3f, bodyShade);
            b.Foot(26f, 3f);
            b.Foot(40f, 3f, -15f);
            b.Shaded(33f, 19f, 12f, 14f, body);
            b.Belly(30f, 18f, 8f, 10f);
            PenkiSplat(b, paints[0], 38f, 12f);
            PenkiSplat(b, paints[2], 27f, 24f);
            PenkiSplat(b, paints[3], 33f, 8f);
            PenkiSplat(b, paints[4], 41f, 26f);
            b.Shaded(31f, 37f, 10.5f, 10f, body);
            b.White(28f, 36f, 7.5f, 6f);
            b.TurnedFace(28f, 37f, FaceLeft, EyeStyle.Smile, eyeGap: 6f);
            // ベレー帽は横へつぶして少し傾け、てっぺんに小さな突起を付ける
            b.Tilted(33f, 46f, 10f, 3.5f, -10f, PenguinPalette.Shade(beret));
            b.Tilted(32.5f, 46.5f, 9f, 2.8f, -10f, beret);
            b.Line(0.6f, beret, V(34, 49), V(35, 51));
            // 絵筆は体より長くして左上へ振り上げ、穂先から虹色のしずくを垂らす
            b.Line(1f, handle, V(24, 22), V(10, 50));
            b.Rect(8, 49, 5, 2, Rgb(190, 190, 200));
            for (int i = 0; i < paints.Length; i++)
            {
                b.Line(0.9f, paints[i], V(7.5f + i, 51), V(9f + i * 0.5f, 57));
            }
            b.Line(0.6f, paints[0], V(6, 46), V(6, 42));
            b.Ellipse(6f, 41f, 1.2f, 1.4f, paints[0]);
            b.Line(0.6f, paints[3], V(14, 44), V(14, 40));
            b.Ellipse(14f, 39f, 1.2f, 1.4f, paints[3]);
            b.Flipper(V(23, 26), V(21, 21), 3f, bodyShade);
            PenkiSplat(b, paints[1], 6f, 6f);
            PenkiSplat(b, paints[4], 54f, 6f);
        }

        /// <summary>ペンキの飛び散り。大きな点のまわりに小さな点を散らす</summary>
        private static void PenkiSplat(PenguinBrush b, Color32 color, float x, float y)
        {
            b.Ellipse(x, y, 1.6f, 1.4f, color);
            b.Dot((int)x + 2, (int)y + 2, color);
            b.Dot((int)x - 2, (int)y - 1, color);
        }

        /// <summary>No.122 ユメクイペン: 夢見がちなねぼすけ（獏）。くちばしがバクの鼻のように長く垂れ、体は黒・白・黒の模様。ナイトキャップで、星入りの夢の雲を吸い込む</summary>
        private static void Yumekui(PenguinBrush b)
        {
            var black = Rgb(50, 52, 68);
            var blackShade = PenguinPalette.Shade(black);
            var band = Rgb(235, 235, 240);
            var cap = Rgb(130, 120, 200);
            var dream = Rgb(215, 200, 250);
            var star = Rgb(255, 240, 150);
            // 夢の雲は鼻の先の足元に浮かべ、吸い込まれる筋で鼻とつなぐ
            b.Shaded(9f, 9f, 7f, 5f, dream);
            b.Shaded(17f, 7f, 6f, 4.5f, dream);
            b.Shaded(13f, 13f, 5f, 4f, dream);
            b.Star(11f, 9f, 2.5f, 1.1f, star);
            b.Sparkle(18, 7, star);
            b.Foot(30f, 3f);
            b.Foot(44f, 3f, -15f);
            // バクの模様は背中からも見えるので、白い帯は White ではなく常に塗る
            b.ShadedTilted(37f, 19f, 15f, 12f, 10f, black);
            YumekuiBand(b, band);
            b.Flipper(V(44, 22), V(52, 14), 3f, blackShade);
            b.Shaded(26f, 34f, 10.5f, 10f, black);
            b.White(23f, 33f, 7.5f, 6f);
            b.TurnedFace(23f, 34f, FaceLeft, EyeStyle.Sleepy, eyeGap: 6f);
            if (b.Front)
            {
                // くちばしを下へ長く垂らし、バクの鼻のように見せる
                b.Line(1.5f, PenguinPalette.BeakShade, V(18, 31), V(13, 28), V(11, 22), V(12, 18));
                b.Line(0.8f, PenguinPalette.Beak, V(18, 31.5f), V(13.5f, 28.5f), V(11.5f, 22), V(12, 19));
            }
            b.Line(0.5f, dream, V(12, 16), V(13, 14));
            b.Line(0.5f, dream, V(15, 17), V(16, 15));
            // ナイトキャップは後ろへ長く垂らし、先のぼんぼんで頭の形に変化を付ける
            b.Polygon(V(30, 44), PenguinPalette.Shade(cap), V(17, 40), V(34, 38), V(46, 44), V(52, 34), V(42, 50), V(26, 48));
            b.Polygon(V(30, 45), cap, V(18, 41), V(33, 40), V(44, 45), V(50, 36), V(41, 49), V(26, 47));
            b.Line(1f, band, V(17, 41), V(35, 39));
            b.Ellipse(52f, 33f, 3f, 3f, band);
        }

        /// <summary>獏の白い帯。体の真ん中を縦に一周する</summary>
        private static void YumekuiBand(PenguinBrush b, Color32 band)
        {
            b.Polygon(V(38, 19), band, V(32, 30), V(40, 31), V(44, 19), V(41, 8), V(33, 7), V(30, 19));
            b.Polygon(V(41, 19), PenguinPalette.BellyShade, V(40, 31), V(42, 30), V(45, 19), V(42, 8), V(41, 8), V(44, 19));
        }

        /// <summary>No.123 カイトウペン: 気取った怪盗。シルクハットに片眼鏡、白い手袋で盗んだ宝石を高く掲げ、黒いマントを後ろへ大きく翻す</summary>
        private static void Kaitou(PenguinBrush b)
        {
            var body = Rgb(40, 40, 55);
            var bodyShade = PenguinPalette.Shade(body);
            var cape = Rgb(30, 30, 42);
            var lining = Rgb(210, 40, 60);
            var gem = Rgb(120, 220, 250);
            var glove = PenguinPalette.Belly;
            // マントは肩から右上へ風になびかせ、先を3つに裂いて翻る勢いを出す。マジシャンペンの横に張るマントと向きで分ける
            b.Polygon(V(44, 32), cape, V(36, 40), V(48, 50), V(56, 60), V(56, 50), V(63, 52), V(58, 40), V(63, 34), V(54, 28), V(56, 14), V(44, 20));
            b.Polygon(V(47, 32), lining, V(42, 36), V(50, 46), V(54, 54), V(54, 47), V(59, 46), V(55, 39), V(58, 34), V(51, 30), V(52, 20), V(45, 24));
            b.Foot(27f, 3f);
            b.Foot(38f, 3f, -10f);
            b.Shaded(33f, 18f, 10f, 13f, body);
            b.Belly(31f, 17f, 6f, 10f);
            if (b.Front)
            {
                b.Triangle(V(29, 27), V(33, 27), V(31, 24), lining);
            }
            b.Flipper(V(40, 22), V(44, 14), 2.5f, bodyShade);
            b.Ellipse(44f, 13f, 2f, 2f, glove);
            b.Shaded(30f, 35f, 10f, 9.5f, body);
            b.White(27f, 34f, 7f, 6f);
            b.TurnedFace(27f, 35f, FaceLeft, EyeStyle.Sharp, EyeStyle.Dot, eyeGap: 6f);
            if (b.Front)
            {
                // 片眼鏡は手前の目だけを輪で囲み、鎖をほおへ垂らす
                b.Line(0.5f, Rgb(200, 200, 215), OnEllipse(30.9f, 35f, 3f, 3f, 0f), OnEllipse(30.9f, 35f, 3f, 3f, 1.5f), OnEllipse(30.9f, 35f, 3f, 3f, 3.1f), OnEllipse(30.9f, 35f, 3f, 3f, 4.7f), OnEllipse(30.9f, 35f, 3f, 3f, 6.3f));
                b.Line(0.3f, Rgb(200, 200, 215), V(33.5f, 33), V(35, 28), V(34, 26));
            }
            b.Rect(19, 42, 22, 2, cape);
            b.Rect(22, 43, 16, 12, cape);
            b.Rect(22, 45, 16, 2, lining);
            b.Flipper(V(22, 28), V(14, 44), 2.5f, bodyShade);
            b.Ellipse(14f, 45f, 2.2f, 2.2f, glove);
            // 宝石はヒレの先で高く掲げ、ひし形にきらめきを添える
            b.Polygon(V(12, 52), PenguinPalette.Shade(gem), V(8, 52), V(12, 48), V(16, 52), V(14, 55), V(10, 55));
            b.Polygon(V(12, 52), gem, V(9, 52), V(12, 49), V(15, 52), V(13.5f, 54), V(10.5f, 54));
            b.Sparkle(6, 58, PenguinPalette.Belly);
            b.Sparkle(18, 57, gem);
        }

        /// <summary>No.124 キュウケツキペン: 高貴で気位が高い。正面を向き、立ち襟の黒いマントをコウモリの翼のように左右いっぱいに広げる。オールバックの羽と小さな牙</summary>
        private static void Kyuuketsuki(PenguinBrush b)
        {
            var body = Rgb(95, 70, 120);
            var bodyShade = PenguinPalette.Shade(body);
            var cape = Rgb(30, 25, 40);
            var lining = Rgb(180, 30, 50);
            // 翼のすそはぎざぎざの弧にして、正面の左右対称でコウモリの翼に見せる
            b.Polygon(V(32, 30), cape, V(32, 44), V(16, 46), V(4, 40), V(1, 28), V(4, 14), V(8, 18), V(13, 10), V(18, 16), V(24, 6), V(32, 10));
            b.Polygon(V(32, 30), cape, V(32, 44), V(48, 46), V(60, 40), V(63, 28), V(60, 14), V(56, 18), V(51, 10), V(46, 16), V(40, 6), V(32, 10));
            b.Polygon(V(22, 30), lining, V(28, 40), V(16, 42), V(7, 37), V(5, 28), V(7, 18), V(10, 21), V(13, 14), V(18, 20), V(24, 12), V(28, 16));
            b.Polygon(V(42, 30), lining, V(36, 40), V(48, 42), V(57, 37), V(59, 28), V(57, 18), V(54, 21), V(51, 14), V(46, 20), V(40, 12), V(36, 16));
            b.Feet(32f, 5f);
            b.Shaded(32f, 19f, 10f, 13f, body);
            b.Belly(31f, 18f, 6f, 9.5f);
            // 立ち襟は頭の後ろで左右にとがらせ、吸血鬼の襟のシルエットを作る
            b.Triangle2(V(28, 30), V(12, 55), V(25, 44), cape);
            b.Triangle2(V(26, 33), V(15, 51), V(24, 43), lining);
            b.Shaded(32f, 38f, 10f, 9.5f, body);
            b.White(31f, 36f, 7f, 5.5f);
            // オールバックは額の真ん中を V 字にとがらせ、白い顔に髪の生え際を見せる
            if (b.Front)
            {
                b.Triangle(V(26, 43), V(36, 43), V(31, 39), body);
            }
            else
            {
                b.Line(0.5f, bodyShade, V(28, 46), V(27, 32));
                b.Line(0.5f, bodyShade, V(34, 46), V(35, 32));
            }
            b.Face(31f, 37f, 3.5f, EyeStyle.Sharp);
            if (b.Front)
            {
                b.Triangle(V(29, 32), V(30.5f, 32), V(29.5f, 29.5f), PenguinPalette.Belly);
                b.Triangle(V(32, 32), V(33.5f, 32), V(32.5f, 29.5f), PenguinPalette.Belly);
            }
        }

        /// <summary>No.125 サイコロペン: 気まぐれな運まかせ。白いサイコロの体を傾けて転がる途中で、上の面から頭を出す。四角いシルエット</summary>
        private static void Saikoro(PenguinBrush b)
        {
            var body = Rgb(60, 70, 110);
            var bodyShade = PenguinPalette.Shade(body);
            var white = Rgb(245, 245, 240);
            var top = Rgb(225, 228, 235);
            var side = Rgb(185, 188, 200);
            var pip = Rgb(30, 30, 40);
            var red = Rgb(215, 40, 50);
            // 立方体を傾けて描き、転がっている途中の動きと角ばったシルエットを両方出す
            const float Tilt = 18f * Mathf.Deg2Rad;
            const float Half = 12f;
            var c = V(27, 19);
            var ax = V(Mathf.Cos(Tilt), Mathf.Sin(Tilt)) * Half;
            var ay = V(-Mathf.Sin(Tilt), Mathf.Cos(Tilt)) * Half;
            var depth = V(9, 8);
            b.Flipper(c + ay + depth * 0.9f + ax * 0.4f, c + ay + depth + ax * 0.9f + V(2, 6), 2.5f, bodyShade);
            SaikoroQuad(b, side, c + ax - ay, c + ax + ay, c + ax + ay + depth, c + ax - ay + depth);
            SaikoroQuad(b, top, c - ax + ay, c + ax + ay, c + ax + ay + depth, c - ax + ay + depth);
            SaikoroQuad(b, white, c - ax - ay, c + ax - ay, c + ax + ay, c - ax + ay);
            // 面の境目に影の線を入れ、紙を重ねたのではなく立方体の角だと分かるようにする
            b.Line(0.5f, side, c - ax + ay, c + ax + ay, c + ax - ay);
            var corner = c - ax + ay + depth * 0.5f + ax * 0.25f;
            b.Ellipse(corner.x, corner.y, 1.3f, 1.3f, pip);
            b.Ellipse(c.x, c.y, 3.2f, 3.2f, red);
            for (int i = -1; i <= 1; i++)
            {
                var p = c + ax + depth * 0.5f + (ay * 0.6f + depth * 0.3f) * i;
                b.Ellipse(p.x, p.y, 1.4f, 1.4f, pip);
            }
            // 頭は上の面の真ん中から出す。くちばしを左へ向け、転がる向きを見せる
            var head = c + ay + depth * 0.5f + ax * 0.35f + V(0, 9);
            b.Shaded(head.x, head.y, 9f, 8.5f, body);
            b.White(head.x - 3f, head.y - 1f, 7f, 6f);
            b.TurnedFace(head.x - 3f, head.y, FaceLeft, EyeStyle.Cross, eyeGap: 6f);
            b.Flipper(c - ax * 0.4f + ay + depth * 0.3f, c - ax + ay + V(-6, 4), 2.5f, bodyShade);
            b.Foot(c.x - 6f, c.y - Half - 3f, -20f, 0.8f);
        }

        /// <summary>四角い面を2つの三角で塗る。点は外周を順に回る</summary>
        private static void SaikoroQuad(PenguinBrush b, Color32 color, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            b.Triangle(p0, p1, p2, color);
            b.Triangle(p0, p2, p3, color);
        }
    }
}
