using PenguinMonster.Common.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    public static partial class PenguinMonsterSceneBuilder
    {
        private static void Place(RectTransform rect, Vector2 anchor, Vector2 position, Vector2 size)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        /// <summary>親の上下 [min, max] の帯に横いっぱいで置く（左右に少し余白）</summary>
        private static void SetBand(RectTransform rect, float min, float max)
        {
            const float Padding = 20f;
            rect.anchorMin = new Vector2(0f, min);
            rect.anchorMax = new Vector2(1f, max);
            rect.offsetMin = new Vector2(Padding, 0f);
            rect.offsetMax = new Vector2(-Padding, 0f);
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
