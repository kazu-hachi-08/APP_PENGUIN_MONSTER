using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技ボタンに使うタイプごとの色
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

        public static Color Get(MonsterType type) => Colors[(int)type];
    }
}
