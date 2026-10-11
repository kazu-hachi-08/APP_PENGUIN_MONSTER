using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View.Art
{
    public static partial class PenguinPainter
    {
        private const float BoltThickness = 1.6f;

        /// <summary>頭のパーツ。頭のてっぺん（top）から上へ高さ8ドットまでに収める</summary>
        private static void PaintHead(PixelCanvas canvas, HeadPart part, ShapeMetrics shape)
        {
            float x = CenterX;
            float top = shape.HeadTop;
            switch (part)
            {
                case HeadPart.Crest:
                    canvas.FillTriangle(V(x - 5, top - 2), V(x - 1, top - 1), V(x - 8, top + 6), PenguinPalette.CrestYellow);
                    canvas.FillTriangle(V(x - 2, top - 1), V(x + 2, top - 1), V(x, top + 8), PenguinPalette.CrestYellow);
                    canvas.FillTriangle(V(x + 1, top - 1), V(x + 5, top - 2), V(x + 8, top + 6), PenguinPalette.CrestYellow);
                    break;
                case HeadPart.Horn:
                    canvas.FillTriangle(V(x - 4, top - 2), V(x + 4, top - 2), V(x, top + 8), PenguinPalette.HornIce);
                    canvas.FillTriangle(V(x, top - 2), V(x + 4, top - 2), V(x, top + 8), PenguinPalette.HornIceShade);
                    break;
                case HeadPart.Flame:
                    canvas.FillTriangle(V(x - 7, top - 3), V(x - 1, top - 3), V(x - 6, top + 6), PenguinPalette.FlameOuter);
                    canvas.FillTriangle(V(x - 3, top - 3), V(x + 3, top - 3), V(x, top + 8), PenguinPalette.FlameOuter);
                    canvas.FillTriangle(V(x + 1, top - 3), V(x + 7, top - 3), V(x + 6, top + 6), PenguinPalette.FlameOuter);
                    canvas.FillTriangle(V(x - 2, top - 2), V(x + 2, top - 2), V(x, top + 4), PenguinPalette.FlameInner);
                    break;
                case HeadPart.Leaf:
                    canvas.FillRect(Mathf.RoundToInt(x), Mathf.RoundToInt(top) - 1, 1, 4, PenguinPalette.LeafShade);
                    canvas.FillEllipse(x + 4.5f, top + 4f, 5f, 2.5f, PenguinPalette.Leaf);
                    canvas.FillRect(Mathf.RoundToInt(x) + 1, Mathf.RoundToInt(top) + 3, 7, 1, PenguinPalette.LeafShade);
                    break;
                case HeadPart.Bolt:
                    // 太い線3本で稲妻のジグザグにする
                    canvas.DrawLine(V(x + 4, top + 7), V(x - 2, top + 3), BoltThickness, PenguinPalette.Bolt);
                    canvas.DrawLine(V(x - 2, top + 3), V(x + 2, top + 3), BoltThickness, PenguinPalette.Bolt);
                    canvas.DrawLine(V(x + 2, top + 3), V(x - 3, top - 2), BoltThickness, PenguinPalette.Bolt);
                    break;
                case HeadPart.Crown:
                    PaintCrown(canvas, x, top);
                    break;
                case HeadPart.Fin:
                    // シャチの背びれ。先を後ろへ倒して、泳いでいる向きを感じさせる
                    canvas.FillTriangle(V(x - 4, top - 2), V(x + 3, top - 2), V(x + 5, top + 8), PenguinPalette.FinShade);
                    canvas.FillTriangle(V(x - 3, top - 1), V(x + 1, top - 1), V(x + 4, top + 6), PenguinPalette.Fin);
                    break;
            }
        }

        private static void PaintCrown(PixelCanvas canvas, float x, float top)
        {
            int left = Mathf.RoundToInt(x) - 6;
            int bottom = Mathf.RoundToInt(top) - 2;
            canvas.FillRect(left, bottom, 13, 4, PenguinPalette.Gold);
            canvas.FillRect(left, bottom, 13, 1, PenguinPalette.GoldShade);
            for (int i = -1; i <= 1; i++)
            {
                float spikeX = x + i * 5f;
                canvas.FillTriangle(V(spikeX - 2, top + 2), V(spikeX + 2, top + 2), V(spikeX, top + 6), PenguinPalette.Gold);
            }
            canvas.FillRect(Mathf.RoundToInt(x) - 1, bottom + 1, 2, 2, PenguinPalette.Gem);
        }

        /// <summary>首のパーツ。後ろ姿ではマフラーの端と鈴が見えない位置になる</summary>
        private static void PaintNeck(PixelCanvas canvas, NeckPart part, ShapeMetrics shape, bool front)
        {
            float y = shape.NeckY;
            float halfWidth = shape.BodyRadiusX - 2f;
            switch (part)
            {
                case NeckPart.Scarf:
                    canvas.FillEllipse(CenterX, y, halfWidth, 2.5f, PenguinPalette.ScarfShade);
                    canvas.FillEllipse(CenterX, y + 0.5f, halfWidth - 1f, 1.5f, PenguinPalette.Scarf);
                    int tailX = Mathf.RoundToInt(front ? CenterX + 5f : CenterX - 2f);
                    canvas.FillRect(tailX, Mathf.RoundToInt(y) - 8, 4, 7, PenguinPalette.Scarf);
                    canvas.FillRect(tailX + 3, Mathf.RoundToInt(y) - 8, 1, 7, PenguinPalette.ScarfShade);
                    break;
                case NeckPart.Bell:
                    canvas.FillEllipse(CenterX, y, halfWidth - 2f, 1.2f, PenguinPalette.Scarf);
                    if (front)
                    {
                        canvas.FillEllipse(CenterX, y - 3f, 3f, 3f, PenguinPalette.GoldShade);
                        canvas.FillEllipse(CenterX - 0.5f, y - 2.5f, 2.2f, 2.2f, PenguinPalette.Gold);
                    }
                    break;
            }
        }

        private static Vector2 V(float x, float y) => new Vector2(x, y);
    }
}
