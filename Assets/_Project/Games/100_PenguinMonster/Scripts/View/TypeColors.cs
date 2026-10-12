using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技ボタン・タイプの札に使うタイプごとの色
    /// </summary>
    public static class TypeColors
    {
        private static readonly Color[] Colors =
        {
            new Color(0.63f, 0.62f, 0.57f), // ノーマル — 落ち着いたグレーベージュ
            new Color(0.45f, 0.72f, 0.80f), // こおり  — 少し暗めの氷青
            new Color(0.25f, 0.45f, 0.75f), // うみ    — 深めの青
            new Color(0.80f, 0.35f, 0.20f), // ひ      — 落ち着いた赤橙
            new Color(0.78f, 0.68f, 0.20f), // かみなり — 暗めの黄
            new Color(0.35f, 0.62f, 0.30f), // こけ    — 深めの緑
            new Color(0.60f, 0.50f, 0.33f), // いわ    — 暗めの土色
            new Color(0.48f, 0.57f, 0.78f), // とり    — 落ち着いた空色
            new Color(0.32f, 0.28f, 0.40f), // やみ    — 濃い紫
            new Color(0.68f, 0.42f, 0.72f), // オーロラ — 落ち着いた紫ピンク
        };

        /// <summary>タイプの色がこれより明るければ、文字を暗い色にして読めるようにする</summary>
        private const float BrightThreshold = 0.6f;
        public const int BadgesPerMonster = 2;
        private static readonly Color DarkLabel = new Color(0.2f, 0.2f, 0.25f);

        public static Color Get(MonsterType type) => Colors[(int)type];

        /// <summary>タイプの札（色の面＋タイプ名）を塗る。ドラフト・ずかん・相性表で同じ見た目にする</summary>
        public static void PaintBadge(Image badge, Text label, MonsterType type)
        {
            var color = Get(type);
            badge.color = color;
            label.color = color.grayscale > BrightThreshold ? DarkLabel : Color.white;
            label.text = type.ToJapanese();
        }

        /// <summary>1体ぶんの札2つ（badges[start] と [start + 1]）を塗る。2タイプ目が無ければ隠す</summary>
        public static void PaintBadges(MonsterDefinition monster, Image[] badges, Text[] labels, int start = 0)
        {
            for (int k = 0; k < BadgesPerMonster; k++)
            {
                bool exists = k < monster.Types.Count;
                badges[start + k].gameObject.SetActive(exists);
                if (exists)
                {
                    PaintBadge(badges[start + k], labels[start + k], monster.Types[k]);
                }
            }
        }
    }
}
