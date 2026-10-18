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

        // 捕獲タマゴの種類ごと。タイプ特化のタマゴは効くタイプの色に寄せて見分けやすくする
        private static readonly EggStyle Great = new EggStyle(
            new Color32(150, 205, 245, 255), new Color32(255, 255, 255, 255), new Color32(30, 70, 130, 255),
            new Color(0.8f, 0.95f, 1f));
        private static readonly EggStyle Ultra = new EggStyle(
            new Color32(245, 205, 70, 255), new Color32(110, 70, 30, 255), new Color32(110, 70, 20, 255),
            new Color(1f, 0.95f, 0.6f));
        private static readonly EggStyle Net = new EggStyle(
            new Color32(120, 200, 120, 255), new Color32(110, 200, 235, 255), new Color32(30, 90, 50, 255),
            new Color(0.8f, 1f, 0.85f));
        private static readonly EggStyle Night = new EggStyle(
            new Color32(40, 50, 100, 255), new Color32(250, 220, 90, 255), new Color32(15, 20, 50, 255),
            new Color(0.75f, 0.7f, 1f));
        private static readonly EggStyle Heavy = new EggStyle(
            new Color32(160, 160, 165, 255), new Color32(120, 85, 55, 255), new Color32(60, 60, 70, 255),
            new Color(0.9f, 0.9f, 0.9f));
        private static readonly EggStyle Sky = new EggStyle(
            new Color32(250, 225, 100, 255), new Color32(120, 190, 240, 255), new Color32(130, 100, 30, 255),
            new Color(1f, 1f, 0.75f));

        public static EggStyle Get(EggKind kind)
        {
            switch (kind)
            {
                case EggKind.Great: return Great;
                case EggKind.Ultra: return Ultra;
                case EggKind.Net: return Net;
                case EggKind.Night: return Night;
                case EggKind.Heavy: return Heavy;
                case EggKind.Sky: return Sky;
                default: return Normal;
            }
        }
    }

    /// <summary>
    /// タマゴの絵（16x16 のドット絵）を種類ごとに作って使い回す。繰り出すときに投げる絵に使う
    /// </summary>
    public static class EggSprites
    {
        public const int Size = 16;

        private static readonly Vector2 MiddlePivot = new Vector2(0.5f, 0.5f);
        private static readonly Dictionary<EggKind, Sprite> Cache = new Dictionary<EggKind, Sprite>();

        public static Sprite Get(EggKind kind)
        {
            return PixelCanvas.GetOrCache(Cache, kind, () => Paint(EggStyleTable.Get(kind)).ToSprite($"Egg_{kind}", MiddlePivot));
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
