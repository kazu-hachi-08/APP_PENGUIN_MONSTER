using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// シングル / ダブルスを選ぶ画面。上に見出し、まん中に大きなボタン2つ（下に選出の数を添える）、左上に「戻る」
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string FormatTitle = "ルールを 選んでください";
        private const string SingleLabel = "シングル\n<size=34>3体を 選んで 1体ずつ</size>";
        private const string DoubleLabel = "ダブルス\n<size=34>4体を 選んで 2体ずつ</size>";

        private static readonly Vector2 FormatTitlePosition = new Vector2(0f, -180f);
        private static readonly Vector2 FormatTitleSize = new Vector2(1400f, 120f);
        private static readonly Vector2 FormatRowPosition = new Vector2(0f, -40f);
        private static readonly Vector2 FormatButtonSize = new Vector2(620f, 280f);
        private const float FormatSpacing = 80f;
        private static readonly Vector2 FormatBackPosition = new Vector2(150f, -55f);
        private const int FormatTitleFontSize = 64;
        private const int FormatButtonFontSize = 64;

        private static readonly Color FormatBackColor = new Color(0.16f, 0.2f, 0.32f);
        private static readonly Color SingleColor = new Color(1f, 0.42f, 0.38f);
        private static readonly Color DoubleColor = new Color(0.35f, 0.55f, 0.95f);

        private static FormatPanel CreateFormatPanel(Transform parent)
        {
            var back = CreateImage(parent, "FormatPanel", FormatBackColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);
            var content = CreateSafeContent(back.transform);

            var title = CreateText(content, "Title", FormatTitle, FormatTitleFontSize, Color.white, TextAnchor.MiddleCenter);
            title.fontStyle = FontStyle.Bold;
            Place(title.rectTransform, new Vector2(0.5f, 1f), FormatTitlePosition, FormatTitleSize);

            var row = CreateCenteredRow(content, "Formats", Center, FormatRowPosition, FormatButtonSize.y, FormatSpacing);
            var single = CreateFormatButton(row, "Btn_Single", SingleLabel, SingleColor);
            var doubles = CreateFormatButton(row, "Btn_Double", DoubleLabel, DoubleColor);

            var backButton = CreateButton(content, "Btn_Back", "戻る", BackButtonSize, BackColor);
            Place(backButton.GetComponent<RectTransform>(), new Vector2(0f, 1f), FormatBackPosition, BackButtonSize);

            var panel = back.gameObject.AddComponent<FormatPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_singleButton").objectReferenceValue = single;
            so.FindProperty("_doubleButton").objectReferenceValue = doubles;
            so.FindProperty("_backButton").objectReferenceValue = backButton.GetComponent<Button>();
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static Button CreateFormatButton(Transform row, string name, string label, Color color)
        {
            var obj = CreateButton(row, name, label, FormatButtonSize, color);
            var text = obj.GetComponentInChildren<Text>();
            text.fontSize = FormatButtonFontSize;
            text.fontStyle = FontStyle.Bold;
            text.supportRichText = true;
            return obj.GetComponent<Button>();
        }
    }
}
