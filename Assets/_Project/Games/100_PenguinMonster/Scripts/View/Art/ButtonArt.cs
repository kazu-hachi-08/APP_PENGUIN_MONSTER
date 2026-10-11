using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 角丸で厚みのある「ぷっくり」ボタンの絵（ふつう / 押したとき）。
    /// 灰色の濃淡で描き、Image の色で塗り分けて全ボタンで使い回す。9スライスで伸ばすので角は崩れない
    /// </summary>
    public static class ButtonArt
    {
        private const int Size = 16;
        private const int SliceBorder = 5;
        private const float OuterRadius = 4f;
        private const float InnerRadius = 3f;
        private const int LedgeHeight = 3;
        private const int PressedDrop = 2;
        // Canvas の referencePixelsPerUnit（既定 100）÷ スプライトの ppu（= Size）。UI 上で1ドットが何単位になるか
        private const float DotSize = 100f / Size;

        // Image の色を掛けるので、面は白より少し暗くしてハイライトだけ白で明るく見せる
        private static readonly Color32 Edge = new Color32(48, 48, 64, 255);
        private static readonly Color32 Ledge = new Color32(140, 140, 150, 255);
        private static readonly Color32 Face = new Color32(220, 220, 220, 255);
        private static readonly Color32 Highlight = new Color32(255, 255, 255, 255);

        private static Sprite _normal;
        private static Sprite _pressed;

        public static Sprite Normal => _normal ??= Create("Button_Normal", 0);
        public static Sprite Pressed => _pressed ??= Create("Button_Pressed", PressedDrop);

        /// <summary>押したときに面が沈む量（UI の単位）。文字も同じだけ下げる</summary>
        public const float PressedDropUnits = PressedDrop * DotSize;

        /// <summary>
        /// 文字の枠の下端を上げる量（UI の単位）。下の段のぶん面の中心が上にあるので、文字を面のまん中に合わせる
        /// </summary>
        public const float LabelLiftUnits = LedgeHeight * DotSize;

        /// <param name="drop">上から何ドット沈めるか。押したときは下の段が薄くなり、面が下がって見える</param>
        private static Sprite Create(string name, int drop)
        {
            var canvas = new PixelCanvas(Size, Size);
            int top = Size - drop;
            int ledge = LedgeHeight - drop;
            canvas.FillRoundedRect(0, 0, Size, top, OuterRadius, Edge);
            canvas.FillRoundedRect(1, 1, Size - 2, top - 2, InnerRadius, Ledge);
            canvas.FillRoundedRect(1, 1 + ledge, Size - 2, top - 2 - ledge, InnerRadius, Face);
            int highlightLeft = Mathf.CeilToInt(InnerRadius) + 1;
            canvas.FillRect(highlightLeft, top - 3, Size - highlightLeft * 2, 1, Highlight);
            return canvas.ToSprite(name, new Vector2(0.5f, 0.5f), Vector4.one * SliceBorder);
        }
    }
}
