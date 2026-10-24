using PenguinMonster.View.Art;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>8番道路 風花大橋のマス（橋・手すり・下り坂・展望マス）と、下り坂で出す風の線</summary>
    public static partial class FieldTileArt
    {
        // 海の上の木の橋。手すりは白く塗って、通れない縁だと遠目でも分かるようにする
        private static readonly Color32 PlankBase = new Color32(196, 150, 100, 255);
        private static readonly Color32 PlankLine = new Color32(150, 108, 70, 255);
        private static readonly Color32 RailWhite = new Color32(240, 240, 246, 255);
        private static readonly Color32 RailShade = new Color32(150, 156, 176, 255);
        private static readonly Color32 SlopeArrow = new Color32(232, 196, 148, 255);
        private static readonly Color32 LookoutGlow = new Color32(255, 236, 140, 255);
        private static readonly Color32 LookoutCore = new Color32(255, 250, 220, 255);
        private static readonly Color32 WindLine = new Color32(255, 255, 255, 200);

        private static Sprite _wind;

        /// <summary>橋の板。橋は東西に延びるので、板の継ぎ目は縦に並べる</summary>
        private static void PaintBridge(PixelCanvas canvas)
        {
            canvas.FillRect(0, 0, Size, Size, PlankBase);
            for (int x = 0; x < Size; x += 4)
            {
                canvas.FillRect(x, 0, 1, Size, PlankLine);
            }
        }

        /// <summary>手すり。下に海が見え、白い横木と柱が並ぶ</summary>
        private static void PaintRailing(PixelCanvas canvas)
        {
            PaintWater(canvas);
            canvas.FillRect(0, 9, Size, 3, RailWhite);
            canvas.FillRect(0, 8, Size, 1, RailShade);
            for (int x = 1; x < Size; x += 8)
            {
                canvas.FillRect(x, 0, 2, 9, RailWhite);
                canvas.FillRect(x + 2, 0, 1, 8, RailShade);
            }
        }

        /// <summary>下り坂。板の上に東向きの「＞」を2つ描いて、乗ると滑る向きを見せる</summary>
        private static void PaintDownhill(PixelCanvas canvas)
        {
            PaintBridge(canvas);
            for (int left = 2; left < Size; left += 7)
            {
                canvas.DrawLine(new Vector2(left, 12), new Vector2(left + 4, 8), 0.6f, SlopeArrow);
                canvas.DrawLine(new Vector2(left + 4, 8), new Vector2(left, 4), 0.6f, SlopeArrow);
            }
        }

        /// <summary>展望マス。板の上に光の輪を描いて、何かありそうな場所に見せる</summary>
        private static void PaintLookout(PixelCanvas canvas)
        {
            PaintBridge(canvas);
            canvas.FillEllipse(7.5f, 7.5f, 6f, 6f, LookoutGlow);
            canvas.FillEllipse(7.5f, 7.5f, 3.5f, 3.5f, LookoutCore);
        }

        /// <summary>下り坂で主人公の後ろに出す、白い横線3本（足元を基準に置く）</summary>
        public static Sprite Wind()
        {
            if (_wind != null)
            {
                return _wind;
            }
            var canvas = new PixelCanvas(Size, Size);
            canvas.FillRect(2, 11, 8, 1, WindLine);
            canvas.FillRect(0, 7, 10, 1, WindLine);
            canvas.FillRect(4, 3, 7, 1, WindLine);
            _wind = canvas.ToSprite("FieldWind", new Vector2(1f, 0f));
            return _wind;
        }
    }
}
