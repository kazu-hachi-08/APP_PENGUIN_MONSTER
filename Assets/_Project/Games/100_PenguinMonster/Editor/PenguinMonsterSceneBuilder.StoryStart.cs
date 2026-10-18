using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 記録があるときの「続きから / 最初から」。シングル / ダブルスを選ぶ画面と同じ並び
    /// （タイトルと同じ夜空・雪原、上に見出し、まん中に大きなボタン2つ、左上に「戻る」）。最初からの確かめは手前に重ねる
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string StoryStartTitle = "ストーリー";
        private static readonly Vector2 StoryStartButtonSize = new Vector2(620f, 220f);
        private static readonly Color ContinueColor = new Color(0.35f, 0.55f, 0.95f);
        private static readonly Color NewGameColor = new Color(0.55f, 0.75f, 0.45f);

        private static StoryStartPanel CreateStoryStartPanel(Transform parent)
        {
            var content = CreateWalkerScreen(parent, "StoryStartPanel", StoryStartTitle, FormatTitleFontSize, FormatTitlePosition, FormatTitleSize, out var back);

            var row = CreateCenteredRow(content, "Choices", Center, FormatRowPosition, StoryStartButtonSize.y, FormatSpacing);
            var continueButton = CreateStoryStartButton(row, "Btn_Continue", "続きから", ContinueColor);
            var newGameButton = CreateStoryStartButton(row, "Btn_NewGame", "最初から", NewGameColor);

            var backButton = CreateButton(content, "Btn_Back", "戻る", BackButtonSize, BackColor);
            Place(backButton.GetComponent<RectTransform>(), new Vector2(0f, 1f), FormatBackPosition, BackButtonSize);

            // 確かめは画面いっぱいに暗幕を敷くので、セーフエリアの外（夜空の上）に置く
            var dialog = CreateConfirmDialog(back.transform);

            var panel = back.gameObject.AddComponent<StoryStartPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_continueButton").objectReferenceValue = continueButton;
            so.FindProperty("_newGameButton").objectReferenceValue = newGameButton;
            so.FindProperty("_backButton").objectReferenceValue = backButton.GetComponent<Button>();
            so.FindProperty("_dialog").objectReferenceValue = dialog;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static Button CreateStoryStartButton(Transform row, string name, string label, Color color)
        {
            var obj = CreateButton(row, name, label, StoryStartButtonSize, color, fontSize: FormatButtonFontSize);
            return obj.GetComponent<Button>();
        }
    }
}
