using PenguinMonster.Common.Editor;
using PenguinMonster.Common.UI;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 1試合ごとの結果の画面。文面とボタンの動きは共通基盤の ResultDialog をそのまま使い、見た目だけゲーム側で作る
    /// （共通の箱だとタイトルからの見た目とつながらないため）。
    /// バトルの上に暗幕を敷き、ロゴ風の「勝ち！」「負け…」・連勝数のバッジ・説明・ぷっくりボタン、勝ったときは紙吹雪（ResultBanner）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // ResultDialog の「もう一度遊ぶ」は、勝ち抜き戦で次の試合へ進むのに使う
        private const string NextBattleLabel = "次の 戦いへ";
        private const string ResultTitleLabel = "タイトルへ";
        private const int ConfettiCount = 60;

        private static readonly Vector2 ResultTitlePosition = new Vector2(0f, 270f);
        private static readonly Vector2 ResultTitleSize = new Vector2(1400f, 240f);
        private static readonly Vector2 ResultBadgePosition = new Vector2(0f, 100f);
        private static readonly Vector2 ResultBadgeSize = new Vector2(520f, 100f);
        private static readonly Vector2 ResultDetailPosition = new Vector2(0f, -70f);
        private static readonly Vector2 ResultDetailSize = new Vector2(1400f, 170f);
        private static readonly Vector2 ResultButtonRowPosition = new Vector2(0f, -280f);
        private static readonly Vector2 ResultRetrySize = new Vector2(460f, 130f);
        private static readonly Vector2 ResultBackSize = new Vector2(400f, 130f);
        private const float ResultButtonSpacing = 60f;
        private static readonly Vector2 ConfettiMinSize = new Vector2(12f, 18f);
        private static readonly Vector2 ConfettiMaxSize = new Vector2(20f, 32f);

        private const int ResultTitleFontSize = 180;
        private const int ResultScoreFontSize = 60;
        private const int ResultDetailFontSize = 48;
        private const int ResultButtonFontSize = 52;

        private static readonly Color ResultOverlayColor = new Color(0f, 0f, 0.05f, 0.65f);

        private static ResultDialog CreateResultPanel(Transform parent, out ResultBanner banner)
        {
            var back = CreateImage(parent, "ResultDialog", ResultOverlayColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);
            var confetti = CreateConfetti(back.transform);
            var content = CreateSafeContent(back.transform);

            var title = CreateText(content, "TitleText", "", ResultTitleFontSize, Color.white, TextAnchor.MiddleCenter);
            StyleHeading(title);
            Place(title.rectTransform, Center, ResultTitlePosition, ResultTitleSize);

            var badge = CreateFramedBox(content, "ScoreBadge", IntroBadgeColor, out var badgeInner);
            badge.raycastTarget = false;
            Place(badge.rectTransform, Center, ResultBadgePosition, ResultBadgeSize);
            var score = CreateText(badgeInner, "ScoreText", "", ResultScoreFontSize, Color.white, TextAnchor.MiddleCenter);
            score.fontStyle = FontStyle.Bold;
            AddOutline(score, ButtonTextEdgeColor, ButtonTextEdge);

            var detail = CreateText(content, "DetailText", "", ResultDetailFontSize, Color.white, TextAnchor.MiddleCenter);
            AddOutline(detail, ButtonTextEdgeColor, ButtonTextEdge);
            Place(detail.rectTransform, Center, ResultDetailPosition, ResultDetailSize);

            var row = CreateCenteredRow(content, "Buttons", Center, ResultButtonRowPosition, ResultRetrySize.y, ResultButtonSpacing);
            var retry = CreateButton(row, "Btn_Retry", NextBattleLabel, ResultRetrySize, SurvivalColor);
            retry.GetComponentInChildren<Text>().fontSize = ResultButtonFontSize;
            var toTitle = CreateButton(row, "Btn_Title", ResultTitleLabel, ResultBackSize, MenuColor);
            toTitle.GetComponentInChildren<Text>().fontSize = ResultButtonFontSize;

            var dialog = back.gameObject.AddComponent<ResultDialog>();
            var so = new SerializedObject(dialog);
            so.FindProperty("_resultTitleText").objectReferenceValue = title;
            so.FindProperty("_scoreText").objectReferenceValue = score;
            so.FindProperty("_detailText").objectReferenceValue = detail;
            so.FindProperty("_retryButton").objectReferenceValue = retry.GetComponent<Button>();
            so.FindProperty("_titleButton").objectReferenceValue = toTitle.GetComponent<Button>();
            so.ApplyModifiedProperties();

            banner = back.gameObject.AddComponent<ResultBanner>();
            var bannerSo = new SerializedObject(banner);
            bannerSo.FindProperty("_title").objectReferenceValue = title;
            bannerSo.FindProperty("_titlePop").objectReferenceValue = AddPopIn(title.gameObject);
            bannerSo.FindProperty("_scoreText").objectReferenceValue = score;
            bannerSo.FindProperty("_scoreBadge").objectReferenceValue = badge.gameObject;
            bannerSo.FindProperty("_scorePop").objectReferenceValue = AddPopIn(badge.gameObject);
            bannerSo.FindProperty("_confetti").objectReferenceValue = confetti;
            bannerSo.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return dialog;
        }

        /// <summary>紙吹雪の粒。色と動きは Confetti が実行時に決める。勝ったときだけ ResultBanner が出す</summary>
        private static GameObject CreateConfetti(Transform parent)
        {
            var area = UIDialogBuilder.CreateUIObject("Confetti", parent);
            UIDialogBuilder.SetStretchAll(area.GetComponent<RectTransform>());
            // 粒が毎フレーム動くので、別の Canvas に分けて、文字やボタンまで作り直しにならないようにする
            area.AddComponent<Canvas>();

            var pieces = new RectTransform[ConfettiCount];
            for (int i = 0; i < pieces.Length; i++)
            {
                var piece = CreateImage(area.transform, $"Piece{i}", Color.white);
                piece.raycastTarget = false;
                var size = Vector2.Lerp(ConfettiMinSize, ConfettiMaxSize, i / (float)(pieces.Length - 1));
                Place(piece.rectTransform, Vector2.zero, Vector2.zero, size);
                pieces[i] = piece.rectTransform;
            }

            var confetti = area.AddComponent<Confetti>();
            var so = new SerializedObject(confetti);
            SetArray(so.FindProperty("_pieces"), pieces);
            so.ApplyModifiedProperties();
            area.SetActive(false);
            return area;
        }
    }
}
