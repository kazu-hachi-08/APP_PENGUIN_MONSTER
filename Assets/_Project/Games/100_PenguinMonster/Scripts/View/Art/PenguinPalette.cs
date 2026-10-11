using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// ペンモンのドット絵の色。体の色は「影・ふつう」の2段で塗り、立体感を出す
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

        // 頭・首のパーツの色
        public static readonly Color32 CrestYellow = new Color32(255, 214, 60, 255);
        public static readonly Color32 HornIce = new Color32(200, 240, 255, 255);
        public static readonly Color32 HornIceShade = new Color32(130, 196, 230, 255);
        public static readonly Color32 FlameOuter = new Color32(255, 110, 40, 255);
        public static readonly Color32 FlameInner = new Color32(255, 220, 80, 255);
        public static readonly Color32 Leaf = new Color32(90, 190, 80, 255);
        public static readonly Color32 LeafShade = new Color32(50, 130, 60, 255);
        public static readonly Color32 Bolt = new Color32(255, 236, 70, 255);
        public static readonly Color32 Gold = new Color32(250, 200, 50, 255);
        public static readonly Color32 GoldShade = new Color32(200, 140, 30, 255);
        public static readonly Color32 Gem = new Color32(230, 60, 90, 255);
        public static readonly Color32 Scarf = new Color32(220, 50, 60, 255);
        public static readonly Color32 ScarfShade = new Color32(160, 30, 45, 255);

        private static readonly Color32[] BodyColors =
        {
            new Color32(52, 64, 110, 255),   // Navy
            new Color32(56, 56, 64, 255),    // Black
            new Color32(110, 180, 230, 255), // Sky
            new Color32(50, 100, 200, 255),  // Blue
            new Color32(210, 70, 50, 255),   // Red
            new Color32(230, 190, 40, 255),  // Yellow
            new Color32(70, 150, 70, 255),   // Green
            new Color32(140, 100, 70, 255),  // Brown
            new Color32(110, 70, 150, 255),  // Purple
            new Color32(230, 130, 170, 255), // Pink
        };

        private const float ShadeRate = 0.7f;

        public static Color32 Body(BodyColor color) => BodyColors[(int)color];

        public static Color32 BodyShade(BodyColor color) => Darken(Body(color), ShadeRate);

        private static Color32 Darken(Color32 color, float rate)
        {
            return new Color32((byte)(color.r * rate), (byte)(color.g * rate), (byte)(color.b * rate), color.a);
        }
    }
}
