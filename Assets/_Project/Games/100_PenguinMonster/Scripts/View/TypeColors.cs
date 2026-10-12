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
            new Color(0.75f, 0.75f, 0.70f), // ノーマル
            new Color(0.55f, 0.85f, 0.95f), // こおり
            new Color(0.25f, 0.50f, 0.90f), // うみ
            new Color(0.95f, 0.45f, 0.25f), // ひ
            new Color(0.95f, 0.85f, 0.25f), // かみなり
            new Color(0.40f, 0.75f, 0.35f), // こけ
            new Color(0.65f, 0.55f, 0.40f), // いわ
            new Color(0.60f, 0.70f, 0.95f), // とり
            new Color(0.35f, 0.30f, 0.45f), // やみ
            new Color(0.85f, 0.55f, 0.90f), // オーロラ
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
