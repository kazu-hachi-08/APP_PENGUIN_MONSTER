using PenguinMonster.Common.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 Center = new Vector2(0.5f, 0.5f);
        private const float FrameThickness = 6f;
        private const float DefaultBandPadding = 20f;

        private static void Place(RectTransform rect, Vector2 anchor, Vector2 position, Vector2 size)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        /// <summary>親の上下 [min, max] の帯に横いっぱいで置く（左右に少し余白）</summary>
        private static void SetBand(RectTransform rect, float min, float max, float padding = DefaultBandPadding)
        {
            rect.anchorMin = new Vector2(0f, min);
            rect.anchorMax = new Vector2(1f, max);
            rect.offsetMin = new Vector2(padding, 0f);
            rect.offsetMax = new Vector2(-padding, 0f);
        }

        /// <summary>濃い色の縁の内側に、指定の色の面を重ねた枠（ダイパ風の窓）。中身は inner に置く</summary>
        private static Image CreateFramedBox(Transform parent, string name, Color fillColor, out RectTransform inner)
        {
            var frame = CreateImage(parent, name, FrameColor);
            var fill = CreateImage(frame.transform, "Fill", fillColor);
            fill.raycastTarget = false;
            inner = fill.rectTransform;
            UIDialogBuilder.SetStretchAll(inner);
            inner.offsetMin = Vector2.one * FrameThickness;
            inner.offsetMax = -Vector2.one * FrameThickness;
            return frame;
        }

        private static Image CreateImage(Transform parent, string name, Color color)
        {
            var obj = UIDialogBuilder.CreateUIObject(name, parent);
            var image = obj.AddComponent<Image>();
            image.color = color;
            return image;
        }

        private static Text CreateText(Transform parent, string name, string content, int fontSize, Color color, TextAnchor alignment)
        {
            var obj = UIDialogBuilder.CreateUIObject(name, parent);
            UIDialogBuilder.SetStretchAll(obj.GetComponent<RectTransform>());
            var text = obj.AddComponent<Text>();
            text.text = content;
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = alignment;
            // 表示用の Text がタップを奪わないよう、レイキャストを切る
            text.raycastTarget = false;
            return text;
        }

        private static GameObject CreateButton(Transform parent, string name, string label, Vector2 size, Color color)
        {
            var obj = UIDialogBuilder.CreateButton(parent, name, label, size.x, size.y, color);
            var text = obj.GetComponentInChildren<Text>();
            text.fontSize = ButtonFontSize;
            text.raycastTarget = false;
            return obj;
        }

        private static void SetArray(SerializedProperty property, Object[] values)
        {
            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            }
        }
    }
}
