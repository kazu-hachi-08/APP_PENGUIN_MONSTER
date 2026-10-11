using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// デザインを短く書くための筆。左右対称の形は「2」の付くメソッドで片側だけ書けば反対側も塗る。
    /// 顔とおなかの白は正面だけ塗るので、各デザインは正面・後ろ姿を同じコードで描ける
    /// </summary>
    public sealed class PenguinBrush
    {
        public const float CenterX = 32f;

        private const float EyeRadiusX = 2.1f;
        private const float EyeRadiusY = 2.6f;
        private const float SmileThickness = 0.6f;
        private const float BeakDrop = 3.5f;
        private const float CheekDrop = 3f;
        private const int CheekWidth = 3;
        private const int CheekHeight = 2;
        // 明るい色の楕円を影よりどれだけ小さくするか。右下に残る影の太さになる
        private const float LightInset = 1.5f;

        private readonly PixelCanvas _canvas;

        public bool Front { get; }

        public PenguinBrush(PixelCanvas canvas, bool front)
        {
            _canvas = canvas;
            Front = front;
        }

        public static Vector2 V(float x, float y) => new Vector2(x, y);

        private static Vector2 Mirror(Vector2 p) => new Vector2(CenterX * 2f - p.x, p.y);

        public void Ellipse(float x, float y, float radiusX, float radiusY, Color32 color)
            => _canvas.FillEllipse(x, y, radiusX, radiusY, color);

        public void Ellipse2(float x, float y, float radiusX, float radiusY, Color32 color)
        {
            Ellipse(x, y, radiusX, radiusY, color);
            Ellipse(CenterX * 2f - x, y, radiusX, radiusY, color);
        }

        /// <summary>影色で塗ってから、左上にずらした少し小さい楕円を明るい色で重ねる。右下に影が残って丸く見える</summary>
        public void Shaded(float x, float y, float radiusX, float radiusY, Color32 light)
        {
            Ellipse(x, y, radiusX, radiusY, PenguinPalette.Shade(light));
            Ellipse(x - 1f, y + 1f, radiusX - LightInset, radiusY - LightInset, light);
        }

        public void Shaded2(float x, float y, float radiusX, float radiusY, Color32 light)
        {
            Shaded(x, y, radiusX, radiusY, light);
            Shaded(CenterX * 2f - x, y, radiusX, radiusY, light);
        }

        public void Triangle(Vector2 a, Vector2 b, Vector2 c, Color32 color) => _canvas.FillTriangle(a, b, c, color);

        public void Triangle2(Vector2 a, Vector2 b, Vector2 c, Color32 color)
        {
            Triangle(a, b, c, color);
            Triangle(Mirror(a), Mirror(b), Mirror(c), color);
        }

        /// <summary>中心から扇形に三角を並べて多角形を塗る。点は外周を順に回る</summary>
        public void Polygon(Vector2 center, Color32 color, params Vector2[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Triangle(center, points[i], points[(i + 1) % points.Length], color);
            }
        }

        public void Line(float thickness, Color32 color, params Vector2[] points)
        {
            for (int i = 0; i + 1 < points.Length; i++)
            {
                _canvas.DrawLine(points[i], points[i + 1], thickness, color);
            }
        }

        public void Line2(float thickness, Color32 color, params Vector2[] points)
        {
            Line(thickness, color, points);
            var mirrored = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                mirrored[i] = Mirror(points[i]);
            }
            Line(thickness, color, mirrored);
        }

        public void Rect(int left, int bottom, int width, int height, Color32 color)
            => _canvas.FillRect(left, bottom, width, height, color);

        public void Dot(int x, int y, Color32 color) => _canvas.Set(x, y, color);

        /// <summary>塗った所を透明に戻す。三日月や輪っかの内側を抜くのに使う</summary>
        public void Erase(float x, float y, float radiusX, float radiusY)
            => _canvas.FillEllipse(x, y, radiusX, radiusY, PenguinPalette.Clear);

        /// <summary>星形。外と内の半径を交互に結ぶ</summary>
        public void Star(float x, float y, float outer, float inner, Color32 color)
        {
            const int Tips = 5;
            var points = new Vector2[Tips * 2];
            for (int i = 0; i < points.Length; i++)
            {
                float angle = Mathf.PI / 2f + i * Mathf.PI / Tips;
                float radius = i % 2 == 0 ? outer : inner;
                points[i] = V(x + Mathf.Cos(angle) * radius, y + Mathf.Sin(angle) * radius);
            }
            Polygon(V(x, y), color, points);
        }

        /// <summary>十字のきらめき</summary>
        public void Sparkle(int x, int y, Color32 color)
        {
            Rect(x - 1, y, 3, 1, color);
            Rect(x, y - 1, 1, 3, color);
        }

        public void Feet(float x, float offsetX, float y = 3f)
        {
            Ellipse(x - offsetX, y, 5f, 2.5f, PenguinPalette.Beak);
            Ellipse(x + offsetX, y, 5f, 2.5f, PenguinPalette.Beak);
        }

        /// <summary>おなかや顔の白。後ろ姿では見えないので塗らない</summary>
        public void White(float x, float y, float radiusX, float radiusY)
        {
            if (!Front) return;
            Ellipse(x, y, radiusX, radiusY, PenguinPalette.Belly);
        }

        /// <summary>白いおなか。下側に薄い影を残して丸みを出す</summary>
        public void Belly(float x, float y, float radiusX, float radiusY)
        {
            if (!Front) return;
            Ellipse(x, y, radiusX, radiusY, PenguinPalette.BellyShade);
            Ellipse(x - 1f, y + 1f, radiusX - LightInset, radiusY - LightInset, PenguinPalette.Belly);
        }

        /// <summary>
        /// 目・くちばし・ほっぺ。目は小さめの丸に縦2ドットの光を入れる（大きな黒目は見つめられているようで怖く見えたため）
        /// </summary>
        public void Face(float x, float eyeY, float eyeOffsetX, bool smile = false)
        {
            if (!Front) return;
            if (smile)
            {
                SmileEye(x - eyeOffsetX, eyeY);
                SmileEye(x + eyeOffsetX, eyeY);
            }
            else
            {
                Eye(x - eyeOffsetX, eyeY);
                Eye(x + eyeOffsetX, eyeY);
            }
            Beak(x, eyeY - BeakDrop);
            Cheeks(x, eyeY - CheekDrop, eyeOffsetX);
        }

        private void Eye(float x, float y)
        {
            Ellipse(x, y - 0.5f, EyeRadiusX, EyeRadiusY, PenguinPalette.Eye);
            int ix = Mathf.RoundToInt(x) - 1;
            int iy = Mathf.RoundToInt(y);
            Dot(ix, iy, PenguinPalette.EyeLight);
            Dot(ix, iy - 1, PenguinPalette.EyeLight);
        }

        private void SmileEye(float x, float y)
        {
            var top = V(x, y + 1f);
            Line(SmileThickness, PenguinPalette.Eye, V(x - 2f, y - 1f), top, V(x + 2f, y - 1f));
        }

        private void Beak(float x, float y)
        {
            Ellipse(x, y, 2.5f, 1.5f, PenguinPalette.BeakShade);
            Ellipse(x, y + 0.5f, 2f, 1f, PenguinPalette.Beak);
        }

        /// <summary>右のほっぺは左を顔の中心で折り返した位置にして、左右で同じ形にする</summary>
        private void Cheeks(float x, float y, float eyeOffsetX)
        {
            int left = Mathf.RoundToInt(x - eyeOffsetX - 4f);
            int right = Mathf.RoundToInt(x * 2f) - left - CheekWidth;
            int bottom = Mathf.RoundToInt(y);
            Rect(left, bottom, CheekWidth, CheekHeight, PenguinPalette.Cheek);
            Rect(right, bottom, CheekWidth, CheekHeight, PenguinPalette.Cheek);
        }
    }
}
