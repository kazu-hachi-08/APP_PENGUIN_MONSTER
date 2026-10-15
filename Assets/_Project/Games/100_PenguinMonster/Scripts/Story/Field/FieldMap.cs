using System;
using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 1枚のマップ（STORY_ROADMAP.md §3.3）。文字の表（1文字＝1マス）から作る。
    /// 表のまま読めるよう、行は上から並べ、マスの位置も上の行から数える
    /// </summary>
    public sealed class FieldMap
    {
        /// <summary>マップの文字とマスの種類。表で使う文字はここに足す</summary>
        private static readonly Dictionary<char, TileKind> Legend = new Dictionary<char, TileKind>
        {
            { '.', TileKind.Snow },
            { ',', TileKind.Grass },
            { '=', TileKind.Path },
            { 'T', TileKind.Tree },
            { '~', TileKind.Water },
            { 'W', TileKind.HouseWall },
            { 'R', TileKind.Roof },
            { 'D', TileKind.Door },
            { '_', TileKind.Floor },
            { 'S', TileKind.Sign },
        };

        private readonly TileKind[,] _tiles;
        private readonly Dictionary<GridPos, Warp> _warps = new Dictionary<GridPos, Warp>();
        private readonly Dictionary<GridPos, string[]> _signs = new Dictionary<GridPos, string[]>();

        public string Id { get; }
        public int Width { get; }
        public int Height { get; }
        public IEnumerable<Warp> Warps => _warps.Values;

        public FieldMap(string id, string[] rows, params Warp[] warps)
        {
            Id = id;
            Height = rows.Length;
            Width = rows[0].Length;
            _tiles = new TileKind[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                ReadRow(rows[y], y);
            }
            foreach (var warp in warps)
            {
                _warps.Add(warp.From, warp);
            }
        }

        private void ReadRow(string row, int y)
        {
            // 表の打ち間違いは、歩いて気づくより先に、作った時点で止める
            if (row.Length != Width)
            {
                throw new ArgumentException($"{Id}: {y} 行目の長さが {row.Length}（1行目は {Width}）");
            }
            for (int x = 0; x < Width; x++)
            {
                if (!Legend.TryGetValue(row[x], out var kind))
                {
                    throw new ArgumentException($"{Id}: ({x}, {y}) の文字 '{row[x]}' はマスの種類に無い");
                }
                _tiles[x, y] = kind;
            }
        }

        public bool Contains(GridPos pos) => pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;

        public TileKind TileAt(GridPos pos) => _tiles[pos.X, pos.Y];

        /// <summary>マップの外は歩けない（端に木が無いマップでも外へはみ出さないように）</summary>
        public bool IsWalkable(GridPos pos) => Contains(pos) && TileAt(pos).IsWalkable();

        /// <summary>そのマスの出入口。無ければ null</summary>
        public Warp WarpAt(GridPos pos) => _warps.TryGetValue(pos, out var warp) ? warp : null;

        /// <summary>看板のマスに文を書く。表と続けて書けるよう、自分を返す</summary>
        public FieldMap WithSign(GridPos pos, params string[] lines)
        {
            // 看板の文字の無いマスに書くと読めないまま残るので、作った時点で止める
            if (!Contains(pos) || TileAt(pos) != TileKind.Sign)
            {
                throw new ArgumentException($"{Id}: {pos} は看板のマスではない");
            }
            _signs.Add(pos, lines);
            return this;
        }

        /// <summary>看板の文。そのマスに看板が無ければ null</summary>
        public IReadOnlyList<string> SignAt(GridPos pos) => _signs.TryGetValue(pos, out var lines) ? lines : null;
    }
}
