using System;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// タブ・しぼりこみ・速さのように「並んだボタンから1つを選ぶ」ときの見た目。
    /// 選んだボタンは元の色のまま、ほかを暗くする（タイプの色を残したまま、どれを選んだか分かるように）
    /// </summary>
    public static class ButtonHighlight
    {
        private const float DimRate = 0.55f;

        public static Color[] CaptureColors(Button[] buttons)
        {
            var colors = new Color[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                colors[i] = buttons[i].image.color;
            }
            return colors;
        }

        public static void Paint(Button[] buttons, Color[] baseColors, int selected)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].image.color = i == selected ? baseColors[i] : Color.Lerp(baseColors[i], Color.black, DimRate);
            }
        }

        /// <summary>並んだボタンのそれぞれに、押されたら自分の番号で onClick を呼ぶ処理をつける。番号は押したときの値を使う（ループ変数を閉じ込めないため）</summary>
        public static void AddIndexedClick(Button[] buttons, Action<int> onClick)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => onClick(index));
            }
        }
    }
}
