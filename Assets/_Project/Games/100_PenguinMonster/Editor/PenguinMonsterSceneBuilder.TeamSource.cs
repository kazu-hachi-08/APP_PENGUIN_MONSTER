using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// オンラインでホストが「ドラフト / 自分のチーム」を選ぶ画面（PLAN_STORY_TOWN01.md フェーズ9）。
    /// シングル / ダブルスの画面の続きに見えるよう、並びと大きさはそちらと同じにし、下に選べない理由の文を置く
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string TeamSourceTitle = "チームを 選んでください";
        private const string DraftSourceLabel = "ドラフト\n<size=34>その場で 6体を\n取り合う</size>";
        private const string OwnTeamSourceLabel = "自分のチーム\n<size=34>ストーリーの 手持ち\n6体（Lv50）</size>";

        private static readonly Vector2 TeamSourceNoticePosition = new Vector2(0f, 200f);
        private static readonly Vector2 TeamSourceNoticeSize = new Vector2(1400f, 90f);
        private const int TeamSourceNoticeFontSize = 48;

        private static readonly Color DraftSourceColor = new Color(0.98f, 0.66f, 0.25f);
        private static readonly Color OwnTeamSourceColor = new Color(0.36f, 0.75f, 0.45f);
        private static readonly Color TeamSourceNoticeColor = new Color(1f, 0.85f, 0.4f);

        private static TeamSourcePanel CreateTeamSourcePanel(Transform parent)
        {
            var back = CreateNightBack(parent, "TeamSourcePanel");
            CreateSnowField(back.transform);
            CreateTitleWalkers(back.transform);
            var content = CreateSafeContent(back.transform);

            var title = CreateText(content, "Title", TeamSourceTitle, FormatTitleFontSize, Color.white, TextAnchor.MiddleCenter);
            StyleHeading(title);
            Place(title.rectTransform, new Vector2(0.5f, 1f), FormatTitlePosition, FormatTitleSize);

            var row = CreateCenteredRow(content, "Sources", Center, FormatRowPosition, FormatButtonSize.y, FormatSpacing);
            var draft = CreateFormatButton(row, "Btn_Draft", DraftSourceLabel, DraftSourceColor);
            var ownTeam = CreateFormatButton(row, "Btn_OwnTeam", OwnTeamSourceLabel, OwnTeamSourceColor);
            var draftIcon = CreateFormatIcon(draft.transform, "Icon", Vector2.zero, FormatSingleIconSize);
            var ownTeamIcon = CreateFormatIcon(ownTeam.transform, "Icon", Vector2.zero, FormatSingleIconSize);

            var notice = CreateText(content, "Notice", string.Empty, TeamSourceNoticeFontSize, TeamSourceNoticeColor, TextAnchor.MiddleCenter);
            StyleHeading(notice);
            Place(notice.rectTransform, new Vector2(0.5f, 0f), TeamSourceNoticePosition, TeamSourceNoticeSize);

            var backButton = CreateButton(content, "Btn_Back", "戻る", BackButtonSize, BackColor);
            Place(backButton.GetComponent<RectTransform>(), new Vector2(0f, 1f), FormatBackPosition, BackButtonSize);

            var panel = back.gameObject.AddComponent<TeamSourcePanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_draftButton").objectReferenceValue = draft;
            so.FindProperty("_ownTeamButton").objectReferenceValue = ownTeam;
            so.FindProperty("_backButton").objectReferenceValue = backButton.GetComponent<Button>();
            so.FindProperty("_notice").objectReferenceValue = notice;
            so.FindProperty("_draftIcon").objectReferenceValue = draftIcon;
            so.FindProperty("_ownTeamIcon").objectReferenceValue = ownTeamIcon;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }
    }
}
