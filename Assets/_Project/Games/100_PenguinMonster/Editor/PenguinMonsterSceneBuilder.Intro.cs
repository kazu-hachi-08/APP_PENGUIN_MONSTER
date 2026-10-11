using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 勝ち抜き戦の試合前の画面。上に「Nせんめ」と連勝数、ボスのときは左に先頭のペンモン・右に名前とセリフ。
    /// 画面全体が1つのボタンで、どこを押しても次へ
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 IntroHeadlinePosition = new Vector2(0f, -190f);
        private static readonly Vector2 IntroHeadlineSize = new Vector2(1600f, 160f);
        private static readonly Vector2 IntroSubPosition = new Vector2(0f, -320f);
        private static readonly Vector2 IntroSubSize = new Vector2(1200f, 70f);
        private static readonly Vector2 IntroAcePosition = new Vector2(-480f, -110f);
        private const float IntroAceSize = 400f;
        private static readonly Vector2 IntroBossNamePosition = new Vector2(220f, -40f);
        private static readonly Vector2 IntroBossNameSize = new Vector2(1000f, 110f);
        private static readonly Vector2 IntroBossLinePosition = new Vector2(220f, -170f);
        private static readonly Vector2 IntroBossLineSize = new Vector2(1000f, 140f);
        private static readonly Vector2 IntroHintPosition = new Vector2(0f, 70f);
        private static readonly Vector2 IntroHintSize = new Vector2(1200f, 60f);

        private const int IntroHeadlineFontSize = 110;
        private const int IntroSubFontSize = 44;
        private const int IntroBossNameFontSize = 76;
        private const int IntroBossLineFontSize = 44;
        private const int IntroHintFontSize = 36;

        private static readonly Color IntroHeadlineColor = new Color(1f, 0.95f, 0.75f);
        private static readonly Color IntroHintColor = new Color(0.8f, 0.85f, 0.95f);

        private static BattleIntroPanel CreateIntroPanel(Transform parent)
        {
            var back = CreateImage(parent, "IntroPanel", Color.white);
            UIDialogBuilder.SetStretchAll(back.rectTransform);
            var next = back.gameObject.AddComponent<Button>();
            next.targetGraphic = back;
            // 押したときに背景色が変わると色指定（ボスは赤）が崩れるので、色の遷移は切る
            next.transition = Selectable.Transition.None;

            var top = new Vector2(0.5f, 1f);
            var headline = CreateText(back.transform, "Headline", "", IntroHeadlineFontSize, IntroHeadlineColor, TextAnchor.MiddleCenter);
            headline.fontStyle = FontStyle.Bold;
            Place(headline.rectTransform, top, IntroHeadlinePosition, IntroHeadlineSize);
            var sub = CreateText(back.transform, "Sub", "", IntroSubFontSize, Color.white, TextAnchor.MiddleCenter);
            Place(sub.rectTransform, top, IntroSubPosition, IntroSubSize);

            var ace = CreateImage(back.transform, "BossAce", Color.white);
            ace.preserveAspect = true;
            ace.raycastTarget = false;
            Place(ace.rectTransform, Center, IntroAcePosition, Vector2.one * IntroAceSize);
            var bossName = CreateText(back.transform, "BossName", "", IntroBossNameFontSize, IntroHeadlineColor, TextAnchor.MiddleLeft);
            bossName.fontStyle = FontStyle.Bold;
            Place(bossName.rectTransform, Center, IntroBossNamePosition, IntroBossNameSize);
            var bossLine = CreateText(back.transform, "BossLine", "", IntroBossLineFontSize, Color.white, TextAnchor.UpperLeft);
            Place(bossLine.rectTransform, Center, IntroBossLinePosition, IntroBossLineSize);

            var hint = CreateText(back.transform, "Hint", "タップで つぎへ", IntroHintFontSize, IntroHintColor, TextAnchor.MiddleCenter);
            Place(hint.rectTransform, new Vector2(0.5f, 0f), IntroHintPosition, IntroHintSize);

            var panel = back.gameObject.AddComponent<BattleIntroPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_back").objectReferenceValue = back;
            so.FindProperty("_headline").objectReferenceValue = headline;
            so.FindProperty("_subText").objectReferenceValue = sub;
            so.FindProperty("_bossName").objectReferenceValue = bossName;
            so.FindProperty("_bossLine").objectReferenceValue = bossLine;
            so.FindProperty("_bossAce").objectReferenceValue = ace;
            so.FindProperty("_nextButton").objectReferenceValue = next;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }
    }
}
