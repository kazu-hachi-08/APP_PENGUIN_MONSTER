using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 基本ペンギン（体・頭・ヒレ・足）を塗り、その上にパーツを重ねて 64x64 のドット絵を作る（仕様書 §6）。
    /// 正面は相手側、後ろ姿は自分側に使う。パーツの描き方は PenguinPainter.Parts.cs
    /// </summary>
    public static partial class PenguinPainter
    {
        public const int CanvasSize = 64;
        private const float CenterX = CanvasSize / 2f;

        /// <summary>体型ごとの寸法。頭のてっぺんは 53 以下にして、頭のパーツ（高さ8まで）と輪郭がはみ出さないようにする</summary>
        private readonly struct ShapeMetrics
        {
            public readonly float BodyRadiusX;
            public readonly float BodyRadiusY;
            public readonly float BodyCenterY;
            public readonly float HeadRadius;
            public readonly float HeadCenterY;

            public ShapeMetrics(float bodyRadiusX, float bodyRadiusY, float bodyCenterY, float headRadius, float headCenterY)
            {
                BodyRadiusX = bodyRadiusX;
                BodyRadiusY = bodyRadiusY;
                BodyCenterY = bodyCenterY;
                HeadRadius = headRadius;
                HeadCenterY = headCenterY;
            }

            public float HeadTop => HeadCenterY + HeadRadius;
            public float NeckY => HeadCenterY - HeadRadius + 2f;
        }

        private static readonly ShapeMetrics[] Shapes =
        {
            new ShapeMetrics(17f, 18f, 22f, 13f, 40f), // Normal
            new ShapeMetrics(21f, 17f, 21f, 13f, 38f), // Chubby
            new ShapeMetrics(14f, 19f, 23f, 12f, 41f), // Slim
        };

        private const float FootY = 3f;
        private const float FootOffsetX = 7f;
        private const float FootRadiusX = 5f;
        private const float FootRadiusY = 2.5f;
        private const float FlipperRadiusX = 4f;
        private const float FlipperRadiusY = 10f;
        private const float EyeOffsetX = 5f;
        // 明るい色の楕円を影よりどれだけ小さくするか。右下に残る影の太さになる
        private const float LightInset = 1.5f;

        public static PixelCanvas Paint(MonsterLook look, bool front)
        {
            var canvas = new PixelCanvas(CanvasSize, CanvasSize);
            var shape = Shapes[(int)look.Shape];
            var body = PenguinPalette.Body(look.Color);
            var shade = PenguinPalette.BodyShade(look.Color);

            PaintFeet(canvas);
            PaintFlippers(canvas, shape, shade);
            PaintBodyAndHead(canvas, shape, body, shade);
            if (front)
            {
                PaintFace(canvas, shape);
            }
            else
            {
                PaintTail(canvas, shape, shade);
            }
            PaintNeck(canvas, look.Neck, shape, front);
            PaintHead(canvas, look.Head, shape);

            canvas.Outline(PenguinPalette.Outline);
            return canvas;
        }

        /// <summary>体と頭の影を両方塗ってから明るい色を重ねる。1つずつ塗ると頭の影が体に輪のように残る</summary>
        private static void PaintBodyAndHead(PixelCanvas canvas, ShapeMetrics shape, Color32 light, Color32 shade)
        {
            canvas.FillEllipse(CenterX, shape.BodyCenterY, shape.BodyRadiusX, shape.BodyRadiusY, shade);
            canvas.FillEllipse(CenterX, shape.HeadCenterY, shape.HeadRadius, shape.HeadRadius, shade);
            canvas.FillEllipse(CenterX - 1f, shape.BodyCenterY + 1f, shape.BodyRadiusX - LightInset, shape.BodyRadiusY - LightInset, light);
            canvas.FillEllipse(CenterX - 1f, shape.HeadCenterY + 1f, shape.HeadRadius - LightInset, shape.HeadRadius - LightInset, light);
        }

        /// <summary>影色で塗ってから、左上にずらした少し小さい楕円を明るい色で重ねる。右下に影が残って丸く見える</summary>
        private static void PaintShaded(PixelCanvas canvas, float x, float y, float radiusX, float radiusY, Color32 light, Color32 shade)
        {
            canvas.FillEllipse(x, y, radiusX, radiusY, shade);
            canvas.FillEllipse(x - 1f, y + 1f, radiusX - LightInset, radiusY - LightInset, light);
        }

        private static void PaintFeet(PixelCanvas canvas)
        {
            canvas.FillEllipse(CenterX - FootOffsetX, FootY, FootRadiusX, FootRadiusY, PenguinPalette.Beak);
            canvas.FillEllipse(CenterX + FootOffsetX, FootY, FootRadiusX, FootRadiusY, PenguinPalette.Beak);
        }

        private static void PaintFlippers(PixelCanvas canvas, ShapeMetrics shape, Color32 shade)
        {
            float offsetX = shape.BodyRadiusX - 1f;
            float y = shape.BodyCenterY + 1f;
            canvas.FillEllipse(CenterX - offsetX, y, FlipperRadiusX, FlipperRadiusY, shade);
            canvas.FillEllipse(CenterX + offsetX, y, FlipperRadiusX, FlipperRadiusY, shade);
        }

        /// <summary>おなかと顔の白をつなげて、ペンギンらしい白黒の正面にする</summary>
        private static void PaintFace(PixelCanvas canvas, ShapeMetrics shape)
        {
            float bellyY = shape.BodyCenterY - 1f;
            PaintShaded(canvas, CenterX, bellyY, shape.BodyRadiusX - 5f, shape.BodyRadiusY - 3f, PenguinPalette.Belly, PenguinPalette.BellyShade);
            canvas.FillEllipse(CenterX, shape.HeadCenterY - 1f, shape.HeadRadius - 3f, shape.HeadRadius - 5f, PenguinPalette.Belly);

            int eyeY = Mathf.RoundToInt(shape.HeadCenterY) - 1;
            PaintEye(canvas, Mathf.RoundToInt(CenterX - EyeOffsetX) - 1, eyeY);
            PaintEye(canvas, Mathf.RoundToInt(CenterX + EyeOffsetX) - 2, eyeY);

            float beakY = shape.HeadCenterY - 5f;
            canvas.FillEllipse(CenterX, beakY, 3.5f, 2f, PenguinPalette.BeakShade);
            canvas.FillEllipse(CenterX, beakY + 0.5f, 3f, 1.5f, PenguinPalette.Beak);

            int cheekY = Mathf.RoundToInt(shape.HeadCenterY) - 4;
            canvas.FillRect(Mathf.RoundToInt(CenterX - EyeOffsetX - 4f), cheekY, 2, 1, PenguinPalette.Cheek);
            canvas.FillRect(Mathf.RoundToInt(CenterX + EyeOffsetX + 2f), cheekY, 2, 1, PenguinPalette.Cheek);
        }

        private static void PaintEye(PixelCanvas canvas, int left, int bottom)
        {
            canvas.FillRect(left, bottom, 3, 4, PenguinPalette.Eye);
            canvas.Set(left, bottom + 3, PenguinPalette.EyeLight);
        }

        private static void PaintTail(PixelCanvas canvas, ShapeMetrics shape, Color32 shade)
        {
            float bottom = shape.BodyCenterY - shape.BodyRadiusY;
            canvas.FillTriangle(new Vector2(CenterX - 4f, bottom + 4f), new Vector2(CenterX + 4f, bottom + 4f),
                new Vector2(CenterX, bottom - 1f), shade);
        }
    }
}
