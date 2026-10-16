using PenguinMonster.Common.Scene;
using PenguinMonster.View;
using PenguinMonster.View.Story;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ストーリーのバトル（PLAN_STORY_TOWN01.md フェーズ5）。バトルの画面は対戦と同じものを使い、
    /// コマンドの窓だけ「戦う / バッグ / ペンモン / 逃げる」の 2x2 を別に作る（対戦の窓と同じ場所・大きさ）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 StoryCommandButtonSize = new Vector2(330f, 115f);
        private static readonly Vector2 StoryCommandSpacing = new Vector2(20f, 20f);
        private const int StoryCommandColumns = 2;
        private static readonly Color BagColor = new Color(0.92f, 0.62f, 0.25f);
        private static readonly Color RunColor = new Color(0.35f, 0.5f, 0.85f);

        // レベルアップの能力の窓は、下の窓の右上に重ねる（はい / いいえ と同じ位置の決め方）
        private static readonly Vector2 LevelUpWindowSize = new Vector2(440f, 420f);
        private const float LevelUpRightMargin = 40f;
        private const float LevelUpGap = 12f;
        private const float LevelUpPadding = 28f;
        private const int LevelUpFontSize = 40;

        private static CommandPanel CreateStoryCommandPanel(Transform parent)
        {
            var frame = CreateFramedBox(parent, "StoryCommandPanel", InfoBoxColor, out var inner);
            var rect = frame.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(CommandPanelWidth, BottomAreaHeight);

            var layout = inner.gameObject.AddComponent<GridLayoutGroup>();
            layout.cellSize = StoryCommandButtonSize;
            layout.spacing = StoryCommandSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = StoryCommandColumns;

            // 本家と同じ並び（左上 戦う・右上 バッグ・左下 ペンモン・右下 逃げる）
            var fight = CreateStoryCommandButton(inner, "Btn_Fight", "戦う", FightColor);
            var bag = CreateStoryCommandButton(inner, "Btn_Bag", "バッグ", BagColor);
            var party = CreateStoryCommandButton(inner, "Btn_Party", "ペンモン", PartyColor);
            var run = CreateStoryCommandButton(inner, "Btn_Run", "逃げる", RunColor);

            var panel = frame.gameObject.AddComponent<CommandPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_fightButton").objectReferenceValue = fight;
            so.FindProperty("_partyButton").objectReferenceValue = party;
            so.FindProperty("_bagButton").objectReferenceValue = bag;
            so.FindProperty("_runButton").objectReferenceValue = run;
            so.ApplyModifiedProperties();

            frame.gameObject.SetActive(false);
            return panel;
        }

        private static Button CreateStoryCommandButton(Transform parent, string name, string label, Color color)
        {
            var button = CreateButton(parent, name, label, StoryCommandButtonSize, color);
            button.GetComponentInChildren<Text>().fontSize = CommandFontSize;
            return button.GetComponent<Button>();
        }

        /// <summary>左の列に能力の名前、右の列に数（右寄せ）</summary>
        private static LevelUpWindow CreateLevelUpWindow(Transform parent)
        {
            var safe = CreateSafeContent(parent);
            safe.name = "LevelUp";
            var frame = CreateFramedBox(safe, "LevelUpWindow", MessageBoxColor, out var inner);
            frame.raycastTarget = false;
            var rect = frame.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = new Vector2(-LevelUpRightMargin, BottomAreaHeight + LevelUpGap);
            rect.sizeDelta = LevelUpWindowSize;

            var names = CreateLevelUpColumn(inner, "Names", TextAnchor.MiddleLeft);
            var values = CreateLevelUpColumn(inner, "Values", TextAnchor.MiddleRight);

            var window = safe.gameObject.AddComponent<LevelUpWindow>();
            var so = new SerializedObject(window);
            so.FindProperty("_names").objectReferenceValue = names;
            so.FindProperty("_values").objectReferenceValue = values;
            so.ApplyModifiedProperties();

            safe.gameObject.SetActive(false);
            return window;
        }

        private static Text CreateLevelUpColumn(Transform inner, string name, TextAnchor alignment)
        {
            var text = CreateText(inner, name, "", LevelUpFontSize, BoxTextColor, alignment);
            text.raycastTarget = false;
            text.rectTransform.offsetMin = Vector2.one * LevelUpPadding;
            text.rectTransform.offsetMax = -Vector2.one * LevelUpPadding;
            return text;
        }

        /// <summary>フィールドからバトルへ移る役。フィールドの層（いつも出ている）に付け、フィールドと会話の両方から使う</summary>
        private static StoryBattleScreen CreateStoryBattleScreen(GameObject fieldRoot, BattleController battle, FadeController fade)
        {
            var screen = fieldRoot.AddComponent<StoryBattleScreen>();
            var so = new SerializedObject(screen);
            so.FindProperty("_battle").objectReferenceValue = battle;
            so.FindProperty("_fade").objectReferenceValue = fade;
            so.ApplyModifiedProperties();
            return screen;
        }
    }
}
