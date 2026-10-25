using System;
using System.Collections.Generic;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 主人公（雪国の子ども。赤い毛糸の帽子に青い上着）の絵。4方向 × 3コマ（立ち・歩き・歩きの反転）。
    /// 16x16 は図形を組み合わせるより1ドットずつ書くほうが形を決めやすいので、文字の絵で描く。
    /// 右向きは左向きの左右反転。歩きのコマも反転すると、もう片方の足を出した絵になる。
    /// 村の人も同じ形の色違いで描く（NpcArt）
    /// </summary>
    public static partial class HeroArt
    {
        public const int Size = 16;

        // 足元がマスの下端に来るよう、下の中央を基準にする
        private static readonly Vector2 FeetPivot = new Vector2(0.5f, 0f);

        // 文字ごとの色: o 縁 / r 帽子 / w 帽子の縁取り / h 髪 / s 肌 / e 目 / b 上着 / B 上着の裾 / k 靴
        private static readonly Dictionary<char, Color32> Palette = new Dictionary<char, Color32>
        {
            { 'o', new Color32(36, 40, 72, 255) },
            { 'r', new Color32(214, 58, 58, 255) },
            { 'w', new Color32(248, 248, 252, 255) },
            { 'h', new Color32(84, 54, 40, 255) },
            { 's', new Color32(250, 210, 176, 255) },
            { 'e', new Color32(36, 40, 72, 255) },
            { 'b', new Color32(64, 120, 210, 255) },
            { 'B', new Color32(40, 80, 160, 255) },
            { 'k', new Color32(70, 60, 60, 255) },
            // 自転車（HeroArt.Bicycle）: g ハンドル / f 車体
            { 'g', new Color32(170, 176, 188, 255) },
            { 'f', new Color32(60, 176, 96, 255) },
        };

        private static readonly string[] Head =
        {
            "......oooo......",
            ".....owwwwo.....",
            "....orrrrrro....",
            "...orrrrrrrro...",
            "...owwwwwwwwo...",
            "...ohhhhhhhho...",
        };
        private static readonly string[] FrontFace =
        {
            "...osssssssso...",
            "...osesssseso...",
            "...osssssssso...",
        };
        private static readonly string[] BackHair =
        {
            "...ohhhhhhhho...",
            "...ohhhhhhhho...",
            "...ohhhhhhhho...",
        };
        private static readonly string[] FrontBody =
        {
            "....obbbbbbo....",
            "...obbbwwbbbo...",
            "..osbbbwwbbbso..",
            "...obbbwwbbbo...",
            "...oBBBBBBBBo...",
        };
        private static readonly string[] BackBody =
        {
            "....obbbbbbo....",
            "...obbbbbbbbo...",
            "..osbbbbbbbbso..",
            "...obbbbbbbbo...",
            "...oBBBBBBBBo...",
        };
        private static readonly string[] FrontLegs = { "....okkookko....", "....oooooooo...." };
        private static readonly string[] FrontStep = { "....okko.oo.....", "....oooo........" };

        // 横向き。帽子のぼんぼりを後ろへ倒し、顔は進む側（左）に寄せる
        private static readonly string[] Side =
        {
            ".......oooo.....",
            "......owwwwo....",
            "....orrrrrro....",
            "...orrrrrrrro...",
            "...owwwwwwwwo...",
            "...ohhhhhhhho...",
            "...osshhhhhho...",
            "...oesshhhhho...",
            "...osssshhhho...",
            "....obbbbbbo....",
            "....obbbbbbo....",
            "....osbbbbbo....",
            "....obbbbbbo....",
            "....oBBBBBBo....",
        };
        private static readonly string[] SideLegs = { ".....okkkko.....", ".....oooooo....." };
        private static readonly string[] SideStep = { "....okko.oko....", "....oooo.ooo...." };

        private static Sprite[,] _sprites;

        /// <param name="frame">0 が立ち、1 が歩き</param>
        /// <param name="mirrorStep">
        /// 歩きのコマを反転して、もう片方の足を出す（上下向きだけ。横向きは反転すると逆を向いてしまうので無視する）
        /// </param>
        public static Sprite Get(Direction facing, int frame, bool mirrorStep = false)
        {
            _sprites ??= CreateAll("Hero", Palette);
            bool canMirror = facing == Direction.Down || facing == Direction.Up;
            int column = frame == 0 ? 0 : (mirrorStep && canMirror ? 2 : 1);
            return _sprites[(int)facing, column];
        }

        /// <summary>主人公の色の一部を差し替えた色の表（村の人の色違い）</summary>
        public static Dictionary<char, Color32> Recolor(IReadOnlyDictionary<char, Color32> changes)
        {
            var palette = new Dictionary<char, Color32>(Palette);
            foreach (var change in changes)
            {
                palette[change.Key] = change.Value;
            }
            return palette;
        }

        /// <summary>[向き, コマ（0 立ち / 1 歩き / 2 歩きの反転）] の絵を全部作る</summary>
        public static Sprite[,] CreateAll(string name, IReadOnlyDictionary<char, Color32> palette)
        {
            var front = new[] { Head, FrontFace, FrontBody };
            var back = new[] { Head, BackHair, BackBody };
            var sprites = new Sprite[4, 3];
            Fill(sprites, name, palette, Direction.Down, Join(front, FrontLegs), Join(front, FrontStep), flip: false);
            Fill(sprites, name, palette, Direction.Up, Join(back, FrontLegs), Join(back, FrontStep), flip: false);
            Fill(sprites, name, palette, Direction.Left, Join(new[] { Side }, SideLegs), Join(new[] { Side }, SideStep), flip: false);
            Fill(sprites, name, palette, Direction.Right, Join(new[] { Side }, SideLegs), Join(new[] { Side }, SideStep), flip: true);
            return sprites;
        }

        private static string[] Join(string[][] parts, string[] legs)
        {
            var rows = new List<string>();
            foreach (var part in parts)
            {
                rows.AddRange(part);
            }
            rows.AddRange(legs);
            return rows.ToArray();
        }

        /// <param name="flip">横向きの絵は左向きで書いてあるので、右向きは反転する</param>
        private static void Fill(Sprite[,] sprites, string name, IReadOnlyDictionary<char, Color32> palette, Direction facing,
            string[] stand, string[] step, bool flip)
        {
            sprites[(int)facing, 0] = Paint(stand, palette, flip).ToSprite($"{name}_{facing}_Stand", FeetPivot);
            sprites[(int)facing, 1] = Paint(step, palette, flip).ToSprite($"{name}_{facing}_Step", FeetPivot);
            sprites[(int)facing, 2] = Paint(step, palette, !flip).ToSprite($"{name}_{facing}_StepMirror", FeetPivot);
        }

        /// <summary>文字の絵を塗る。1行目が一番上（PixelCanvas は下が y=0 なので上下を入れ替える）</summary>
        private static PixelCanvas Paint(string[] rows, IReadOnlyDictionary<char, Color32> palette, bool flip)
        {
            if (rows.Length != Size)
            {
                throw new ArgumentException($"人の絵の行数が {rows.Length}（{Size} のはず）");
            }
            var canvas = new PixelCanvas(Size, Size);
            for (int row = 0; row < Size; row++)
            {
                if (rows[row].Length != Size)
                {
                    throw new ArgumentException($"人の絵の {row} 行目の長さが {rows[row].Length}");
                }
                for (int x = 0; x < Size; x++)
                {
                    if (palette.TryGetValue(rows[row][x], out var color))
                    {
                        canvas.Set(x, Size - 1 - row, color);
                    }
                }
            }
            if (flip)
            {
                canvas.FlipX();
            }
            return canvas;
        }
    }
}
