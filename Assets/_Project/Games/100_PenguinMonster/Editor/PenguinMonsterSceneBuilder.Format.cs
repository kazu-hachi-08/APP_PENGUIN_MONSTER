using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// シングル / ダブルスを選ぶ画面。タイトルの続きに見えるよう、夜空・雪・雪原を歩くペンモンはタイトルと同じ。
    /// 上に見出し、まん中に大きなボタン2つ（左に場に出る数だけペンモンの絵、右に名前と選出の数）、左上に「戻る」
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string FormatTitle = "ルールを 選んでください";
        private const string SingleLabel = "シングル\n<size=34>3体を 選んで\n1体ずつ 戦う</size>";
        private const string DoubleLabel = "ダブルス\n<size=34>4体を 選んで\n2体ずつ 戦う</size>";

        private static readonly Vector2 FormatTitlePosition = new Vector2(0f, -180f);
        private static readonly Vector2 FormatTitleSize = new Vector2(1400f, 120f);
        private static readonly Vector2 FormatRowPosition = new Vector2(0f, -20f);
        private static readonly Vector2 FormatButtonSize = new Vector2(720f, 320f);
        private const float FormatSpacing = 80f;
        private static readonly Vector2 FormatBackPosition = new Vector2(150f, -55f);
        private const int FormatTitleFontSize = 72;
        private const int FormatButtonFontSize = 64;

        // ボタンの左に絵、右に文字。ダブルスは2体を少しずらして重ね、並んで戦うことを見せる
        private const float FormatIconAreaWidth = 300f;
        private const float FormatSingleIconSize = 240f;
        private const float FormatDoubleIconSize = 190f;
        private static readonly Vector2[] FormatDoubleIconOffsets = { new Vector2(-50f, 30f), new Vector2(50f, -30f) };

        private static readonly Color SingleColor = new Color(1f, 0.42f, 0.38f);
        private static readonly Color DoubleColor = new Color(0.35f, 0.55f, 0.95f);

        private static FormatPanel CreateFormatPanel(Transform parent)
        {
            var content = CreateWalkerScreen(parent, "FormatPanel", FormatTitle, FormatTitleFontSize, FormatTitlePosition, FormatTitleSize, out var back);

            var row = CreateCenteredRow(content, "Formats", Center, FormatRowPosition, FormatButtonSize.y, FormatSpacing);
            var single = CreateFormatButton(row, "Btn_Single", SingleLabel, SingleColor);
            var doubles = CreateFormatButton(row, "Btn_Double", DoubleLabel, DoubleColor);
            var icons = new[]
            {
                CreateFormatIcon(single.transform, "Icon0", Vector2.zero, FormatSingleIconSize),
                CreateFormatIcon(doubles.transform, "Icon0", FormatDoubleIconOffsets[0], FormatDoubleIconSize),
                CreateFormatIcon(doubles.transform, "Icon1", FormatDoubleIconOffsets[1], FormatDoubleIconSize),
            };

            var backButton = CreateButton(content, "Btn_Back", "戻る", BackButtonSize, BackColor);
            Place(backButton.GetComponent<RectTransform>(), new Vector2(0f, 1f), FormatBackPosition, BackButtonSize);

            var panel = back.gameObject.AddComponent<FormatPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_singleButton").objectReferenceValue = single;
            so.FindProperty("_doubleButton").objectReferenceValue = doubles;
            so.FindProperty("_backButton").objectReferenceValue = backButton.GetComponent<Button>();
            SetArray(so.FindProperty("_icons"), icons);
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static Button CreateFormatButton(Transform row, string name, string label, Color color)
        {
            var obj = CreateButton(row, name, label, FormatButtonSize, color, fontSize: FormatButtonFontSize);
            var text = obj.GetComponentInChildren<Text>();
            text.supportRichText = true;
            text.rectTransform.offsetMin = new Vector2(FormatIconAreaWidth, 0f);
            return obj.GetComponent<Button>();
        }

        /// <summary>ボタンの左の絵の枠（絵は開くたびに FormatPanel がはめる）</summary>
        private static Image CreateFormatIcon(Transform button, string name, Vector2 offset, float size)
        {
            var icon = CreateImage(button, name, Color.white);
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            Place(icon.rectTransform, new Vector2(0f, 0.5f), new Vector2(FormatIconAreaWidth / 2f + offset.x, offset.y), Vector2.one * size);
            return icon;
        }
    }
}
