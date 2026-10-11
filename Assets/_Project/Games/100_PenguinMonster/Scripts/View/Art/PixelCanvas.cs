using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// ドット絵を塗るための小さなキャンバス（Y は上向き、左下が原点）。
    /// 64x64 を文字の絵で書くのは大変なので、楕円や三角の組み合わせで描く
    /// </summary>
    public sealed class PixelCanvas
    {
        public int Width { get; }
        public int Height { get; }
        public Color32[] Pixels { get; }

        public PixelCanvas(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Color32[width * height];
        }

        public void Set(int x, int y, Color32 color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            Pixels[y * Width + x] = color;
        }

        public bool IsOpaque(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return false;
            return Pixels[y * Width + x].a > 0;
        }

        public void FillEllipse(float centerX, float centerY, float radiusX, float radiusY, Color32 color)
        {
            for (int y = Mathf.FloorToInt(centerY - radiusY); y <= Mathf.CeilToInt(centerY + radiusY); y++)
            {
                for (int x = Mathf.FloorToInt(centerX - radiusX); x <= Mathf.CeilToInt(centerX + radiusX); x++)
                {
                    // ドットの中心で判定すると、左右対称のきれいな楕円になる
                    float dx = (x + 0.5f - centerX) / radiusX;
                    float dy = (y + 0.5f - centerY) / radiusY;
                    if (dx * dx + dy * dy <= 1f) Set(x, y, color);
                }
            }
        }

        public void FillRect(int left, int bottom, int width, int height, Color32 color)
        {
            for (int y = bottom; y < bottom + height; y++)
            {
                for (int x = left; x < left + width; x++)
                {
                    Set(x, y, color);
                }
            }
        }

        /// <summary>角を半径 radius で丸めた四角。ドットの中心で判定する（FillEllipse と同じ考え方）</summary>
        public void FillRoundedRect(int left, int bottom, int width, int height, float radius, Color32 color)
        {
            for (int y = bottom; y < bottom + height; y++)
            {
                for (int x = left; x < left + width; x++)
                {
                    // 一番近い「角の円の中心」からの距離で判定する。角以外では中心が自分と重なるので必ず塗られる
                    float centerX = Mathf.Clamp(x + 0.5f, left + radius, left + width - radius);
                    float centerY = Mathf.Clamp(y + 0.5f, bottom + radius, bottom + height - radius);
                    float dx = x + 0.5f - centerX;
                    float dy = y + 0.5f - centerY;
                    if (dx * dx + dy * dy <= radius * radius) Set(x, y, color);
                }
            }
        }

        public void FillTriangle(Vector2 a, Vector2 b, Vector2 c, Color32 color)
        {
            int minX = Mathf.FloorToInt(Mathf.Min(a.x, Mathf.Min(b.x, c.x)));
            int maxX = Mathf.CeilToInt(Mathf.Max(a.x, Mathf.Max(b.x, c.x)));
            int minY = Mathf.FloorToInt(Mathf.Min(a.y, Mathf.Min(b.y, c.y)));
            int maxY = Mathf.CeilToInt(Mathf.Max(a.y, Mathf.Max(b.y, c.y)));
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (Contains(a, b, c, new Vector2(x + 0.5f, y + 0.5f))) Set(x, y, color);
                }
            }
        }

        /// <summary>小さな円を並べて太い線を引く</summary>
        public void DrawLine(Vector2 from, Vector2 to, float radius, Color32 color)
        {
            int steps = Mathf.CeilToInt(Vector2.Distance(from, to)) * 2;
            for (int i = 0; i <= steps; i++)
            {
                var point = Vector2.Lerp(from, to, steps == 0 ? 0f : i / (float)steps);
                FillEllipse(point.x, point.y, radius, radius, color);
            }
        }

        /// <summary>不透明なドットの外側1ドットを縁取る。ドット絵らしさの大半はこの輪郭で出る</summary>
        public void Outline(Color32 color)
        {
            var source = (Color32[])Pixels.Clone();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (source[y * Width + x].a > 0) continue;
                    if (IsOpaqueIn(source, x - 1, y) || IsOpaqueIn(source, x + 1, y) ||
                        IsOpaqueIn(source, x, y - 1) || IsOpaqueIn(source, x, y + 1))
                    {
                        Pixels[y * Width + x] = color;
                    }
                }
            }
        }

        /// <summary>左右反転。後ろ姿は正面と逆を向かせる（自分側は右の相手のほうを向く）</summary>
        public void FlipX()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width / 2; x++)
                {
                    int left = y * Width + x;
                    int right = y * Width + (Width - 1 - x);
                    (Pixels[left], Pixels[right]) = (Pixels[right], Pixels[left]);
                }
            }
        }

        /// <param name="border">9スライスの縁（左・下・右・上のドット数）。伸ばしても角を崩したくない絵だけ指定する</param>
        public Sprite ToSprite(string name, Vector2 pivot, Vector4 border = default)
        {
            var texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point, // ドット絵をぼかさない
                wrapMode = TextureWrapMode.Clamp,
                name = name,
            };
            texture.SetPixels32(Pixels);
            texture.Apply(false, true); // CPU 側のコピーは要らないので捨ててメモリを減らす

            var sprite = Sprite.Create(texture, new Rect(0, 0, Width, Height), pivot, Width, 0, SpriteMeshType.FullRect, border);
            sprite.name = name;
            return sprite;
        }

        private bool IsOpaqueIn(Color32[] source, int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return false;
            return source[y * Width + x].a > 0;
        }

        private static bool Contains(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
        {
            float d1 = Cross(a, b, p);
            float d2 = Cross(b, c, p);
            float d3 = Cross(c, a, p);
            bool hasNegative = d1 < 0 || d2 < 0 || d3 < 0;
            bool hasPositive = d1 > 0 || d2 > 0 || d3 > 0;
            return !(hasNegative && hasPositive);
        }

        private static float Cross(Vector2 a, Vector2 b, Vector2 p) => (b.x - a.x) * (p.y - a.y) - (b.y - a.y) * (p.x - a.x);
    }
}
