using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// バトルの背景（空・地面・ペンモンが立つ楕円の台座）をドット絵で作る。
    /// 大きく引き伸ばして使うので小さく描き、Point フィルタでドットのまま拡大する
    /// </summary>
    public static class BackdropArt
    {
        private const int SkyHeight = 12;
        private const int GroundHeight = 10;
        private const int PlatformWidth = 96;
        private const int PlatformHeight = 24;
        private const float PlatformRimRate = 0.82f;

        private static readonly Color32 SkyTop = new Color32(120, 190, 240, 255);
        private static readonly Color32 SkyBottom = new Color32(220, 244, 255, 255);
        private static readonly Color32 GroundFar = new Color32(176, 222, 160, 255);
        private static readonly Color32 GroundNear = new Color32(132, 196, 118, 255);
        private static readonly Color32 PlatformRim = new Color32(96, 160, 90, 255);
        private static readonly Color32 PlatformTop = new Color32(196, 236, 170, 255);
        private static readonly Color32 PlatformHighlight = new Color32(222, 248, 200, 255);

        private static readonly Vector2 CenterPivot = new Vector2(0.5f, 0.5f);

        /// <summary>上から下へ明るくなる空。段ごとに色を分けてドット絵らしい帯にする</summary>
        public static Sprite CreateSky() => CreateBands("Backdrop_Sky", SkyHeight, SkyBottom, SkyTop);

        /// <summary>奥ほど明るい地面</summary>
        public static Sprite CreateGround() => CreateBands("Backdrop_Ground", GroundHeight, GroundNear, GroundFar);

        public static Sprite CreatePlatform()
        {
            var canvas = new PixelCanvas(PlatformWidth, PlatformHeight);
            float centerX = PlatformWidth / 2f;
            float centerY = PlatformHeight / 2f;
            canvas.FillEllipse(centerX, centerY, centerX, centerY, PlatformRim);
            canvas.FillEllipse(centerX, centerY + 1f, centerX * PlatformRimRate, centerY * PlatformRimRate, PlatformTop);
            canvas.FillEllipse(centerX - 8f, centerY + 3f, centerX * 0.4f, centerY * 0.3f, PlatformHighlight);
            return canvas.ToSprite("Backdrop_Platform", CenterPivot);
        }

        /// <param name="bottom">いちばん下の段の色</param>
        /// <param name="top">いちばん上の段の色</param>
        private static Sprite CreateBands(string name, int height, Color32 bottom, Color32 top)
        {
            var canvas = new PixelCanvas(1, height);
            for (int y = 0; y < height; y++)
            {
                canvas.Set(0, y, Color32.Lerp(bottom, top, y / (float)(height - 1)));
            }
            return canvas.ToSprite(name, CenterPivot);
        }
    }
}
