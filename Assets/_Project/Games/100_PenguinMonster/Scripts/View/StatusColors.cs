using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 状態異常の印の色と、能力変化の光り方の色（ダイパの印の色に寄せる）
    /// </summary>
    public static class StatusColors
    {
        private static readonly Color[] Colors =
        {
            Color.clear,                     // なし
            new Color(0.85f, 0.75f, 0.15f),  // まひ
            new Color(0.65f, 0.35f, 0.70f),  // どく
            new Color(0.50f, 0.20f, 0.55f),  // もうどく
            new Color(0.90f, 0.40f, 0.25f),  // やけど
            new Color(0.55f, 0.55f, 0.60f),  // ねむり
            new Color(0.45f, 0.75f, 0.90f),  // こおり
        };

        public static readonly Color StatUp = new Color(0.45f, 0.65f, 1f);
        public static readonly Color StatDown = new Color(1f, 0.45f, 0.45f);
        public static readonly Color Confusion = new Color(0.95f, 0.6f, 0.9f);

        public static Color Get(StatusCondition status) => Colors[(int)status];
    }
}
