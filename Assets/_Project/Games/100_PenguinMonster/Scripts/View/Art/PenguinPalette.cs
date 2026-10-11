using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 全員に共通するドット絵の色（輪郭・おなかの白・くちばし・目・ほっぺ）。体の色は各デザインが持つ
    /// </summary>
    public static class PenguinPalette
    {
        public static readonly Color32 Outline = new Color32(28, 30, 48, 255);
        public static readonly Color32 Belly = new Color32(250, 250, 244, 255);
        public static readonly Color32 BellyShade = new Color32(214, 220, 228, 255);
        public static readonly Color32 Beak = new Color32(250, 168, 40, 255);
        public static readonly Color32 BeakShade = new Color32(210, 116, 24, 255);
        public static readonly Color32 Eye = new Color32(24, 24, 32, 255);
        public static readonly Color32 EyeLight = new Color32(255, 255, 255, 255);
        public static readonly Color32 Cheek = new Color32(255, 150, 160, 255);
        public static readonly Color32 Gold = new Color32(250, 200, 50, 255);
        public static readonly Color32 GoldShade = new Color32(200, 140, 30, 255);
        public static readonly Color32 Clear = new Color32(0, 0, 0, 0);

        private const float ShadeRate = 0.68f;

        /// <summary>影の色。どの色も同じ割合で暗くして、全員の陰影の付き方をそろえる</summary>
        public static Color32 Shade(Color32 color) => Darken(color, ShadeRate);

        public static Color32 Darken(Color32 color, float rate)
        {
            return new Color32((byte)(color.r * rate), (byte)(color.g * rate), (byte)(color.b * rate), color.a);
        }

        public static Color32 Rgb(byte r, byte g, byte b) => new Color32(r, g, b, 255);
    }
}
