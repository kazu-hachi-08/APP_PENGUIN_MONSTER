using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;
using static PenguinMonster.View.Art.PenguinBrush;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// ペンモン1体ごとの絵（仕様書 §6）。共通の胴体に小物を付けるだけだと全員同じ形に見えたので、
    /// No ごとにシルエットから描き分ける。座標は 64x64・Y 上向き。デザインは型ごとのファイルに分けてある
    /// </summary>
    public static partial class PenguinDesigns
    {
        public const int CanvasSize = 64;

        private static readonly Dictionary<int, Action<PenguinBrush>> ByNo = new Dictionary<int, Action<PenguinBrush>>
        {
            { 1, Yuki }, { 2, Nami }, { 3, Hino }, { 4, Ina }, { 5, Koke }, { 6, Iwa },
            { 7, Hyouga }, { 8, Hoshi }, { 9, Koutei }, { 10, Ryuuhyou }, { 11, Penta }, { 12, Mofu },
            { 13, Uzu }, { 14, Sango }, { 15, Kazan }, { 16, Takibi }, { 17, Rai }, { 18, Arashi },
            { 19, Nori }, { 20, Tsubasa }, { 21, Kaze }, { 22, Shachi }, { 23, Yoru }, { 24, Auro },
            { 25, Tsurara }, { 26, Kamakura }, { 27, Onsen }, { 28, Hanabi }, { 29, Kinoko }, { 30, Tsuta },
            { 31, Magician }, { 32, Boxer }, { 33, Shinju }, { 34, Iruka }, { 35, Koumori }, { 36, Hotaru },
            { 37, HinaPen }, { 38, YukiHina }, { 39, HaneHina }, { 40, Nakimushi }, { 41, KaraHina }, { 42, PiriHina },
            { 43, Same }, { 44, Hinotori }, { 45, YukiHyou }, { 46, Hikari }, { 47, Raigan },
            { 48, Nankyoku }, { 49, Raitei }, { 50, Kyokuya },
            { 51, Ganko }, { 52, Tsutsuki }, { 53, Binta }, { 54, Yukidaruma }, { 55, Sori }, { 56, Amagumo },
            { 57, Naminori }, { 58, Hideri }, { 59, Himawari }, { 60, Kokemakura }, { 61, Watayuki }, { 62, Kaen },
            { 63, Inazuma }, { 64, Yoiyami }, { 65, Hagoromo }, { 66, Kaigara }, { 67, Raiun },
            { 68, KotsubuHina }, { 69, NebosukeHina }, { 70, PokapokaHina },
            { 71, Fubukiou }, { 72, Umidori }, { 73, Jukai },
            { 74, Wadatsumi }, { 75, Amaterasu },
            { 76, SnowDome }, { 77, Kakigori }, { 78, Figure }, { 79, Hige }, { 80, Igloo }, { 81, Yukionna },
            { 82, Shirokuma }, { 83, Yukigassen }, { 84, Clione }, { 85, Ski }, { 86, Gentoo }, { 87, Shoubou },
            { 88, Sumi }, { 89, Kurage }, { 90, Kamenori }, { 91, Ningyo }, { 92, Galapagos }, { 93, Iwatobi },
            { 94, Yuubin }, { 95, Takoage }, { 96, Kazamidori }, { 97, Fukurou }, { 98, Tengu }, { 99, Kotatsu },
            { 100, Macaroni }, { 101, Sakadachi }, { 102, Yakiimo }, { 103, Hinomai }, { 104, Chouchin }, { 105, Robo },
            { 106, Antenna }, { 107, Rockn }, { 108, Taiko }, { 109, Pachipachi }, { 110, Taifuu }, { 111, Saboten },
            { 112, Marimo }, { 113, Matcha }, { 114, Bonsai }, { 115, Haetorisou }, { 116, Koishidorobou }, { 117, Tsuruhashi },
            { 118, Haniwa }, { 119, Dogu }, { 120, Uchuu }, { 121, Penki }, { 122, Yumekui }, { 123, Kaitou },
            { 124, Kyuuketsuki }, { 125, Saikoro }, { 126, Tamago }, { 127, GyouretsuHina }, { 128, UkiwaHina }, { 129, DenkyuuHina },
            { 130, MatchHina }, { 131, ObakeHina }, { 132, FutabaHina }, { 133, MuteppouHina }, { 134, YukidamaHina }, { 135, HoshikuzuHina },
            { 136, Raisetsu }, { 137, Kujiranori }, { 138, Suishou }, { 139, Plasma }, { 140, Oni }, { 141, Ninja },
            { 142, YamiInazuma }, { 143, Juhyou }, { 144, Yokozuna }, { 145, Ufo }, { 146, Kodaiou }, { 147, Sekaiju },
            { 148, Hajimari }, { 149, Nisshoku }, { 150, Amatsubasa },
        };

        public static PixelCanvas Paint(int no, bool front)
        {
            var canvas = new PixelCanvas(CanvasSize, CanvasSize);
            // テスト用や未登録の No でも絵が出るように、ふつうのペンギンで代用する
            var design = ByNo.TryGetValue(no, out var found) ? found : Penta;
            design(new PenguinBrush(canvas, front));
            canvas.Outline(PenguinPalette.Outline);
            if (IsLegend(no))
            {
                // 黒い輪郭の外にもう1周 金を回して、ひと目で伝説とわかるようにする（仕様書 §6）
                canvas.Outline(PenguinPalette.Gold);
            }
            if (!front)
            {
                canvas.FlipX();
            }
            return canvas;
        }

        /// <summary>MonsterTable.ByNo は無い No で例外を出すので、テスト用の No でも描けるように自前で探す</summary>
        private static bool IsLegend(int no)
        {
            foreach (var monster in MonsterTable.All)
            {
                if (monster.No == no) return monster.Rank == MonsterRank.Legend;
            }
            return false;
        }

        private static Color32 Rgb(byte r, byte g, byte bl) => PenguinPalette.Rgb(r, g, bl);
    }
}
