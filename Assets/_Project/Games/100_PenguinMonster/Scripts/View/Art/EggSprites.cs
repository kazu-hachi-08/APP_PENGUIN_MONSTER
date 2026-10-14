using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>タマゴ1種類ぶんの色。殻・模様・縁と、割れたときの光</summary>
    public readonly struct EggStyle
    {
        public readonly Color32 Shell;
        public readonly Color32 Spot;
        public readonly Color32 Outline;
        public readonly Color Flash;

        public EggStyle(Color32 shell, Color32 spot, Color32 outline, Color flash)
        {
            Shell = shell;
            Spot = spot;
            Outline = outline;
            Flash = flash;
        }
    }

    /// <summary>タマゴの種類ごとの色の表。種類を足すときはここに1行足す</summary>
    public static class EggStyleTable
    {
        // ふつうのタマゴ: クリーム色の殻に水色の斑点（ペンギンの卵のイメージ）
        private static readonly EggStyle Normal = new EggStyle(
            new Color32(250, 245, 225, 255), new Color32(120, 190, 235, 255), new Color32(40, 60, 110, 255),
            new Color(0.85f, 0.95f, 1f));

        public static EggStyle Get(EggKind kind)
        {
            switch (kind)
            {
                default: return Normal;
            }
        }
    }

    /// <summary>
    /// タマゴの絵（16x16 のドット絵）を種類ごとに作って使い回す。残りの数の表示と、繰り出すときに投げる絵の両方で使う
    /// </summary>
    public static class EggSprites
    {
        public const int Size = 16;

        private static readonly Vector2 MiddlePivot = new Vector2(0.5f, 0.5f);
        private static readonly Dictionary<EggKind, Sprite> Cache = new Dictionary<EggKind, Sprite>();

        public static Sprite Get(EggKind kind)
        {
            if (Cache.TryGetValue(kind, out var sprite) && sprite != null)
            {
                return sprite;
            }

            sprite = Paint(EggStyleTable.Get(kind)).ToSprite($"Egg_{kind}", MiddlePivot);
            Cache[kind] = sprite;
            return sprite;
        }

        /// <summary>下がふくらんだ卵形（下の丸＋上の細い楕円）に斑点と光</summary>
        private static PixelCanvas Paint(EggStyle style)
        {
            var c = new PixelCanvas(Size, Size);
            c.FillEllipse(8f, 6f, 5.5f, 5.5f, style.Shell);
            c.FillEllipse(8f, 9f, 4.5f, 6f, style.Shell);
            c.FillEllipse(10f, 5f, 1.5f, 1.2f, style.Spot);
            c.FillEllipse(6f, 8f, 1.2f, 1.2f, style.Spot);
            c.FillEllipse(9.5f, 11f, 1f, 1f, style.Spot);
            c.FillEllipse(5.5f, 11f, 0.8f, 1.5f, new Color32(255, 255, 255, 255));
            c.Outline(style.Outline);
            return c;
        }
    }
}
