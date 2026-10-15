using System.Collections.Generic;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドのタイル（16x16 のドット絵）をマスの種類ごとにコードで描き、使い回す。
    /// 1マス＝1ワールド単位になるよう、絵の幅をそのまま pixelsPerUnit にする（PixelCanvas.ToSprite）
    /// </summary>
    public static class FieldTileArt
    {
        public const int Size = 16;

        private static readonly Vector2 MiddlePivot = new Vector2(0.5f, 0.5f);
        private static readonly Dictionary<TileKind, Tile> Cache = new Dictionary<TileKind, Tile>();

        // 雪国の村。地面は青みの白、建物は木の茶と赤い屋根
        private static readonly Color32 SnowBase = new Color32(236, 243, 250, 255);
        private static readonly Color32 SnowShade = new Color32(205, 220, 240, 255);
        private static readonly Color32 GrassDark = new Color32(52, 120, 84, 255);
        private static readonly Color32 GrassLight = new Color32(96, 168, 108, 255);
        private static readonly Color32 PathBase = new Color32(204, 194, 178, 255);
        private static readonly Color32 PathPebble = new Color32(170, 160, 146, 255);
        private static readonly Color32 Trunk = new Color32(110, 74, 50, 255);
        private static readonly Color32 Leaves = new Color32(38, 98, 72, 255);
        private static readonly Color32 LeavesShade = new Color32(26, 72, 54, 255);
        private static readonly Color32 SnowCap = new Color32(250, 252, 255, 255);
        private static readonly Color32 WaterBase = new Color32(78, 138, 208, 255);
        private static readonly Color32 WaterWave = new Color32(150, 198, 240, 255);
        private static readonly Color32 WallBase = new Color32(192, 152, 112, 255);
        private static readonly Color32 WallLine = new Color32(150, 110, 78, 255);
        private static readonly Color32 RoofBase = new Color32(184, 62, 60, 255);
        private static readonly Color32 RoofLine = new Color32(136, 40, 46, 255);
        private static readonly Color32 DoorBase = new Color32(98, 64, 42, 255);
        private static readonly Color32 DoorKnob = new Color32(240, 200, 80, 255);
        private static readonly Color32 FloorBase = new Color32(216, 186, 142, 255);
        private static readonly Color32 FloorLine = new Color32(186, 150, 110, 255);

        public static Tile Get(TileKind kind)
        {
            if (Cache.TryGetValue(kind, out var tile) && tile != null)
            {
                return tile;
            }

            tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = Paint(kind).ToSprite($"Tile_{kind}", MiddlePivot);
            tile.name = $"Tile_{kind}";
            Cache[kind] = tile;
            return tile;
        }

        private static PixelCanvas Paint(TileKind kind)
        {
            var canvas = new PixelCanvas(Size, Size);
            switch (kind)
            {
                case TileKind.Grass: PaintGrass(canvas); break;
                case TileKind.Path: PaintPath(canvas); break;
                case TileKind.Tree: PaintTree(canvas); break;
                case TileKind.Water: PaintWater(canvas); break;
                case TileKind.HouseWall: PaintWall(canvas); break;
                case TileKind.Roof: PaintRoof(canvas); break;
                case TileKind.Door: PaintDoor(canvas); break;
                case TileKind.Floor: PaintFloor(canvas); break;
                default: PaintSnow(canvas); break;
            }
            return canvas;
        }

        private static void PaintSnow(PixelCanvas canvas)
        {
            canvas.FillRect(0, 0, Size, Size, SnowBase);
            // 一面の白だと歩いても動いて見えないので、まばらに影の点を打つ
            canvas.FillRect(3, 11, 2, 1, SnowShade);
            canvas.FillRect(11, 5, 2, 1, SnowShade);
            canvas.FillRect(7, 2, 1, 1, SnowShade);
        }

        private static void PaintGrass(PixelCanvas canvas)
        {
            PaintSnow(canvas);
            // 草むらは2列×2段の草の束。下の段を濃くして、背の高い草に見せる
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    int left = 1 + col * 8;
                    int bottom = 1 + row * 7;
                    canvas.FillTriangle(new Vector2(left, bottom), new Vector2(left + 3, bottom + 6), new Vector2(left + 6, bottom), GrassDark);
                    canvas.FillTriangle(new Vector2(left + 1, bottom + 1), new Vector2(left + 3, bottom + 5), new Vector2(left + 5, bottom + 1), GrassLight);
                }
            }
        }

        private static void PaintPath(PixelCanvas canvas)
        {
            canvas.FillRect(0, 0, Size, Size, PathBase);
            canvas.FillRect(2, 12, 2, 1, PathPebble);
            canvas.FillRect(10, 9, 1, 1, PathPebble);
            canvas.FillRect(6, 3, 2, 1, PathPebble);
            canvas.FillRect(13, 1, 1, 1, PathPebble);
        }

        private static void PaintTree(PixelCanvas canvas)
        {
            PaintSnow(canvas);
            canvas.FillRect(7, 0, 2, 4, Trunk);
            // 針葉樹の3段の三角に、雪をかぶせる
            canvas.FillTriangle(new Vector2(1, 3), new Vector2(8, 10), new Vector2(15, 3), LeavesShade);
            canvas.FillTriangle(new Vector2(2, 7), new Vector2(8, 13), new Vector2(14, 7), Leaves);
            canvas.FillTriangle(new Vector2(4, 11), new Vector2(8, 16), new Vector2(12, 11), Leaves);
            canvas.FillRect(7, 14, 2, 2, SnowCap);
            canvas.FillRect(5, 11, 6, 1, SnowCap);
            canvas.FillRect(3, 7, 3, 1, SnowCap);
            canvas.FillRect(10, 7, 3, 1, SnowCap);
        }

        private static void PaintWater(PixelCanvas canvas)
        {
            canvas.FillRect(0, 0, Size, Size, WaterBase);
            canvas.FillRect(2, 11, 4, 1, WaterWave);
            canvas.FillRect(9, 6, 4, 1, WaterWave);
            canvas.FillRect(3, 2, 3, 1, WaterWave);
        }

        private static void PaintWall(PixelCanvas canvas)
        {
            canvas.FillRect(0, 0, Size, Size, WallBase);
            // 横板の継ぎ目
            for (int y = 3; y < Size; y += 4)
            {
                canvas.FillRect(0, y, Size, 1, WallLine);
            }
        }

        private static void PaintRoof(PixelCanvas canvas)
        {
            canvas.FillRect(0, 0, Size, Size, RoofBase);
            // 瓦の段。段ごとに継ぎ目をずらして、ならべた瓦に見せる
            for (int y = 0; y < Size; y += 4)
            {
                canvas.FillRect(0, y, Size, 1, RoofLine);
                int offset = (y / 4) % 2 == 0 ? 0 : 4;
                for (int x = offset; x < Size; x += 8)
                {
                    canvas.FillRect(x, y, 1, 4, RoofLine);
                }
            }
        }

        private static void PaintDoor(PixelCanvas canvas)
        {
            PaintWall(canvas);
            canvas.FillRect(3, 0, 10, 14, DoorBase);
            canvas.FillRect(10, 6, 2, 2, DoorKnob);
        }

        private static void PaintFloor(PixelCanvas canvas)
        {
            canvas.FillRect(0, 0, Size, Size, FloorBase);
            // 縦の床板。継ぎ目の高さを板ごとにずらす
            for (int x = 0; x < Size; x += 4)
            {
                canvas.FillRect(x, 0, 1, Size, FloorLine);
            }
            canvas.FillRect(1, 10, 3, 1, FloorLine);
            canvas.FillRect(9, 4, 3, 1, FloorLine);
        }
    }
}
