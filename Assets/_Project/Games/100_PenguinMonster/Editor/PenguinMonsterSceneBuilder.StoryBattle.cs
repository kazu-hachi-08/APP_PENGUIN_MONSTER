using PenguinMonster.Common.Editor;
using PenguinMonster.Common.Scene;
using PenguinMonster.View;
using PenguinMonster.View.Story;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ストーリーのバトル。バトルの画面は対戦と同じものを使い、
    /// コマンドの窓だけ「戦う / バッグ / ペンモン / 逃げる」の 2x2 を別に作る（対戦の窓と同じ場所・大きさ）。
    /// バッグの一覧は交代の画面と同じ形（暗幕の上の縦の枠）にする
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

        // 覚える技の説明は、技の長押しの説明（下の窓の上の左・幅 1100）と並ぶよう右に置く
        private static readonly Vector2 NewMoveWindowSize = new Vector2(680f, 240f);

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
            var button = CreateButton(parent, name, label, StoryCommandButtonSize, color, fontSize: CommandFontSize);
            return button.GetComponent<Button>();
        }

        private const int BagEmptyFontSize = 40;
        // バッグ: 上に「タマゴ / 薬 / 持ち物」のタブ、6枠ずつのページ、下に ← 戻る →。
        // 窓は交代の画面と同じ大きさのまま、タブの1行ぶん枠を低くして収める
        private const int BagPageSize = 6;
        private static readonly string[] BagPocketLabels = { "タマゴ", "薬", "持ち物" };
        private static readonly Vector2 BagSlotSize = new Vector2(760f, 96f);
        // 3つのタブが間を空けて枠の幅（BagSlotSize.x）に収まる幅
        private static readonly Vector2 BagTabSize = new Vector2(240f, 80f);
        private static readonly Vector2 BagPageButtonSize = new Vector2(120f, 80f);
        private const float BagFooterHeight = 110f;

        private static BagPanel CreateBagPanel(Transform parent)
        {
            var overlay = CreateImage(parent, "BagPanel", OverlayColor);
            UIDialogBuilder.SetStretchAll(overlay.rectTransform);

            var frame = CreateFramedBox(overlay.transform, "Box", InfoBoxColor, out var inner);
            Place(frame.rectTransform, Center, Vector2.zero, PartyBoxSize);
            var layout = AddChildLayout<VerticalLayoutGroup>(inner.gameObject, PartySlotSpacing, TextAnchor.UpperCenter);
            layout.padding = new RectOffset(0, 0, (int)PartySlotSpacing, 0);

            var tabs = CreateBagPocketTabs(inner);
            var empty = CreateText(inner, "Empty", "", BagEmptyFontSize, BoxTextColor, TextAnchor.MiddleCenter);
            empty.rectTransform.sizeDelta = BagSlotSize;
            var buttons = new Button[BagPageSize];
            var labels = new Text[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                var slot = CreateButton(inner, $"Btn_Item{i}", "", BagSlotSize, BagColor);
                buttons[i] = slot.GetComponent<Button>();
                labels[i] = slot.GetComponentInChildren<Text>();
                labels[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                UseVerticalNavigation(buttons[i]);
            }

            // 下の帯は枠の数で上下しないよう、並びから外して窓の下に留める
            var footer = CreateFooter(inner, BagFooterHeight, PartySlotSpacing);
            footer.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
            var prev = CreateButton(footer, "Btn_Prev", "◀", BagPageButtonSize, SlotColor);
            var back = CreateButton(footer, "Btn_Back", "戻る", BackButtonSize, BackColor);
            var next = CreateButton(footer, "Btn_Next", "▶", BagPageButtonSize, SlotColor);

            var panel = overlay.gameObject.AddComponent<BagPanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_pocketTabs"), tabs);
            SetArray(so.FindProperty("_slotButtons"), buttons);
            SetArray(so.FindProperty("_slotLabels"), labels);
            so.FindProperty("_prevButton").objectReferenceValue = prev.GetComponent<Button>();
            so.FindProperty("_nextButton").objectReferenceValue = next.GetComponent<Button>();
            so.FindProperty("_backButton").objectReferenceValue = back.GetComponent<Button>();
            so.FindProperty("_emptyLabel").objectReferenceValue = empty;
            so.ApplyModifiedProperties();

            overlay.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>BagPocket の順にタブを横に並べる</summary>
        private static Button[] CreateBagPocketTabs(Transform parent)
        {
            var row = UIDialogBuilder.CreateUIObject("Tabs", parent);
            row.GetComponent<RectTransform>().sizeDelta = new Vector2(BagSlotSize.x, BagTabSize.y);
            AddChildLayout<HorizontalLayoutGroup>(row, PartySlotSpacing, TextAnchor.MiddleCenter);

            var tabs = new Button[BagPocketLabels.Length];
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i] = CreateButton(row.transform, $"Btn_Tab{i}", BagPocketLabels[i], BagTabSize, YesColor).GetComponent<Button>();
            }
            return tabs;
        }

        /// <summary>← → はページ送りに使うので、一覧の枠はキーの上下だけで選ぶ</summary>
        private static void UseVerticalNavigation(Selectable selectable)
        {
            var navigation = selectable.navigation;
            navigation.mode = Navigation.Mode.Vertical;
            selectable.navigation = navigation;
        }

        /// <summary>左の列に能力の名前、右の列に数（右寄せ）</summary>
        private static LevelUpWindow CreateLevelUpWindow(Transform parent)
        {
            var frame = CreateBottomRightWindow(parent, "LevelUp", "LevelUpWindow", new Vector2(-LevelUpRightMargin, BottomAreaHeight + LevelUpGap),
                LevelUpWindowSize, out var safe, out var inner);
            frame.raycastTarget = false;

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

        private static NewMoveWindow CreateNewMoveWindow(Transform parent)
        {
            var frame = CreateBottomRightWindow(parent, "NewMove", "NewMoveWindow", new Vector2(-LevelUpRightMargin, BottomAreaHeight + LevelUpGap),
                NewMoveWindowSize, out var safe, out var inner);
            frame.raycastTarget = false;

            var text = CreateText(inner, "Text", "", MoveHintFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            text.raycastTarget = false;
            text.rectTransform.offsetMin = new Vector2(MoveHintPadding, 0f);
            text.rectTransform.offsetMax = new Vector2(-MoveHintPadding, 0f);

            var window = safe.gameObject.AddComponent<NewMoveWindow>();
            var so = new SerializedObject(window);
            so.FindProperty("_text").objectReferenceValue = text;
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
