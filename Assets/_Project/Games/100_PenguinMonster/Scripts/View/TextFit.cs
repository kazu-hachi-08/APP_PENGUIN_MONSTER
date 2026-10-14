using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 文を枠に入り切る大きさまで縮めて入れる。丸ゴシックは字の幅も行の高さも大きく、長い名前（ムテッポウヒナペン など）や
    /// 2タイプ（ノーマル/オーロラ）が入ると、折り返した行が縦にはみ出したり（GameFontApplier が Overflow にしている）
    /// 隣の文字の下に潜ったりして重なるため。
    /// Unity の Best Fit は &lt;size=N&gt; の指定を縮めてくれないので、地の文字と &lt;size&gt; を同じ割合で縮める。
    /// 横が Overflow の Text は「1行の長さが枠の幅に入るか」、Wrap の Text は「折り返した高さが枠の高さに入るか」で決める
    /// </summary>
    public static class TextFit
    {
        private const float MinScale = 0.5f;
        private const float ScaleStep = 0.05f;
        private static readonly Regex SizeTag = new Regex(@"<size=(\d+)>");

        // 縮める前の大きさ（SceneBuilder が決めた大きさ）。縮めたあとに短い文を入れたら元に戻すため
        private static readonly Dictionary<Text, int> BaseSizes = new Dictionary<Text, int>();

        public static void SetFitted(this Text text, string content)
        {
            int baseSize = BaseSize(text);
            Vector2 area = text.rectTransform.rect.size;
            for (float scale = 1f; ; scale -= ScaleStep)
            {
                text.fontSize = Mathf.RoundToInt(baseSize * scale);
                text.text = scale < 1f ? ScaleSizeTags(content, scale) : content;
                if (scale - ScaleStep < MinScale || Fits(text, area))
                {
                    return;
                }
            }
        }

        private static int BaseSize(Text text)
        {
            if (!BaseSizes.TryGetValue(text, out int size))
            {
                size = text.fontSize;
                BaseSizes[text] = size;
            }
            return size;
        }

        private static bool Fits(Text text, Vector2 area)
        {
            var settings = text.GetGenerationSettings(new Vector2(area.x, 0f));
            var generator = text.cachedTextGeneratorForLayout;
            float unit = text.pixelsPerUnit;
            if (text.horizontalOverflow == HorizontalWrapMode.Overflow)
            {
                return generator.GetPreferredWidth(text.text, settings) / unit <= area.x;
            }
            return generator.GetPreferredHeight(text.text, settings) / unit <= area.y;
        }

        private static string ScaleSizeTags(string content, float scale)
        {
            return SizeTag.Replace(content, m => $"<size={Mathf.RoundToInt(int.Parse(m.Groups[1].Value) * scale)}>");
        }
    }
}
