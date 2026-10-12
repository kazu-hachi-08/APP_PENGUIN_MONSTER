using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 勝ち抜き戦の試合前の画面。夜空と雪の上に、上に「N戦目」（上から叩きつける）と連勝数のバッジ、
    /// ボスのときは左に先頭のペンモン・右に名前とセリフ。画面全体が1つのボタンで、どこを押しても次へ
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 IntroHeadlinePosition = new Vector2(0f, -190f);
        private static readonly Vector2 IntroHeadlineSize = new Vector2(1600f, 180f);
        private static readonly Vector2 IntroBadgePosition = new Vector2(0f, -330f);
        private static readonly Vector2 IntroBadgeSize = new Vector2(760f, 90f);
        private static readonly Vector2 IntroAcePosition = new Vector2(-480f, -110f);
        private const float IntroAceSize = 400f;
        private static readonly Vector2 IntroBossNamePosition = new Vector2(220f, -40f);
        private static readonly Vector2 IntroBossNameSize = new Vector2(1000f, 110f);
        private static readonly Vector2 IntroBossLinePosition = new Vector2(220f, -170f);
        private static readonly Vector2 IntroBossLineSize = new Vector2(1000f, 140f);
        private static readonly Vector2 IntroHintPosition = new Vector2(0f, 70f);
        private static readonly Vector2 IntroHintSize = new Vector2(1200f, 60f);
        // 叩きつける前の大きさ。画面からはみ出すくらい大きい所から落とす
        private const float IntroSlamScale = 3f;

        private const int IntroHeadlineFontSize = 130;
        private const int IntroSubFontSize = 44;
        private const int IntroBossNameFontSize = 76;
        private const int IntroBossLineFontSize = 44;
        private const int IntroHintFontSize = 36;

        private static readonly Color IntroHintColor = new Color(0.8f, 0.85f, 0.95f);
        // 連勝数は「燃えている」ように見せたいので、くちばしのオレンジ
        private static readonly Color IntroBadgeColor = new Color(0.85f, 0.4f, 0.12f);

        private static BattleIntroPanel CreateIntroPanel(Transform parent)
        {
            // 夜空はボスのとき外すので、NightSky は付けずに BattleIntroPanel がはめる
            var back = CreateImage(parent, "IntroPanel", Color.white);
            UIDialogBuilder.SetStretchAll(back.rectTransform);
            CreateTitleSnow(back.transform);
            var next = back.gameObject.AddComponent<Button>();
            next.targetGraphic = back;
            // 押したときに背景色が変わると色指定（ボスは赤）が崩れるので、色の遷移は切る
            next.transition = Selectable.Transition.None;

            var content = UIDialogBuilder.CreateUIObject("Content", back.transform);
            UIDialogBuilder.SetStretchAll(content.GetComponent<RectTransform>());

            var top = new Vector2(0.5f, 1f);
            var headline = CreateText(content.transform, "Headline", "", IntroHeadlineFontSize, Color.white, TextAnchor.MiddleCenter);
            StyleHeading(headline);
            Place(headline.rectTransform, top, IntroHeadlinePosition, IntroHeadlineSize);

            var badge = CreateFramedBox(content.transform, "Badge", IntroBadgeColor, out var badgeInner);
            badge.raycastTarget = false;
            Place(badge.rectTransform, top, IntroBadgePosition, IntroBadgeSize);
            var sub = CreateText(badgeInner, "Sub", "", IntroSubFontSize, Color.white, TextAnchor.MiddleCenter);
            sub.fontStyle = FontStyle.Bold;
            AddOutline(sub, ButtonTextEdgeColor, ButtonTextEdge);

            var ace = CreateImage(content.transform, "BossAce", Color.white);
            ace.preserveAspect = true;
            ace.raycastTarget = false;
            Place(ace.rectTransform, Center, IntroAcePosition, Vector2.one * IntroAceSize);
            var bossName = CreateText(content.transform, "BossName", "", IntroBossNameFontSize, Color.white, TextAnchor.MiddleLeft);
            StyleHeading(bossName);
            Place(bossName.rectTransform, Center, IntroBossNamePosition, IntroBossNameSize);
            var bossLine = CreateText(content.transform, "BossLine", "", IntroBossLineFontSize, Color.white, TextAnchor.UpperLeft);
            Place(bossLine.rectTransform, Center, IntroBossLinePosition, IntroBossLineSize);

            var hint = CreateText(content.transform, "Hint", "タップで 次へ", IntroHintFontSize, IntroHintColor, TextAnchor.MiddleCenter);
            Place(hint.rectTransform, new Vector2(0.5f, 0f), IntroHintPosition, IntroHintSize);
            hint.gameObject.AddComponent<Blink>();

            var panel = back.gameObject.AddComponent<BattleIntroPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_back").objectReferenceValue = back;
            so.FindProperty("_content").objectReferenceValue = content.GetComponent<RectTransform>();
            so.FindProperty("_headline").objectReferenceValue = headline;
            so.FindProperty("_headlinePop").objectReferenceValue = AddPopIn(headline.gameObject, IntroSlamScale);
            so.FindProperty("_subText").objectReferenceValue = sub;
            so.FindProperty("_badgePop").objectReferenceValue = AddPopIn(badge.gameObject);
            so.FindProperty("_bossName").objectReferenceValue = bossName;
            so.FindProperty("_bossLine").objectReferenceValue = bossLine;
            so.FindProperty("_bossAce").objectReferenceValue = ace;
            so.FindProperty("_acePop").objectReferenceValue = AddPopIn(ace.gameObject);
            so.FindProperty("_nextButton").objectReferenceValue = next;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }
    }
}
