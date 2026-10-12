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

        /// <summary>
        /// 基本のちびペンギン（足・ヒレ・体・頭・おなかと顔の白）。目と頭の飾りは各デザインが足す。
        /// 頭を体と同じくらい大きくして、かわいさを出す
        /// </summary>
        private static void StandardBody(PenguinBrush b, Color32 color, float bodyY, float bodyRadiusX, float bodyRadiusY,
            float headY, float headRadius)
        {
            b.Feet(CenterX, bodyRadiusX * 0.5f);
            b.Shaded2(CenterX - bodyRadiusX + 1f, bodyY, 3.5f, 7f, PenguinPalette.Shade(color));
            b.Shaded(CenterX, bodyY, bodyRadiusX, bodyRadiusY, color);
            b.Belly(CenterX, bodyY - 1f, bodyRadiusX - 4f, bodyRadiusY - 2.5f);
            b.Shaded(CenterX, headY, headRadius, headRadius, color);
            b.White(CenterX, headY - 1f, headRadius - 3f, headRadius - 4.5f);
        }

        private static Color32 Rgb(byte r, byte g, byte bl) => PenguinPalette.Rgb(r, g, bl);
    }
}
