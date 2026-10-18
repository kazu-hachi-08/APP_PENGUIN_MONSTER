using PenguinMonster.Common.Editor;
using PenguinMonster.Story;
using PenguinMonster.View;
using PenguinMonster.View.Story;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// フィールドのメニュー。奥から 項目の窓（右上）→ 手持ちの画面 → バッグ。
    /// 十字キーより手前、会話の層より奥に置く（「やめる」の はい / いいえ を会話窓で聞くため）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // 項目の窓は本家と同じく右上に縦に並べる
        private static readonly Vector2 FieldMenuButtonSize = new Vector2(360f, 96f);
        private const float FieldMenuSpacing = 12f;
        private const float FieldMenuMargin = 40f;
        private const int FieldMenuPadding = 24;

        // 手持ちの画面: 左に一覧、右に選択肢か詳しく、下に案内の1行
        private const string PartyScreenTitleText = "手持ち";
        // 枠の文は2行（36 + 28）で、丸ゴシックは行の高さが字の約1.45倍なので約93要る。
        // ぷっくりボタンの段差ぶん（ButtonArt.LabelLiftUnits ≒ 19）文字が上がるので、枠は 93 + 19 より高くする
        private static readonly Vector2 PartyListSize = new Vector2(600f, 776f);
        private static readonly Vector2 PartyListSlotSize = new Vector2(540f, 120f);
        private const float PartyListSpacing = 6f;
        // 文が枠の右の角丸に触れないよう空ける（あずかり箱も同じ）
        private const float SlotTextRightPadding = 16f;
        private static readonly Vector2 PartyActionsSize = new Vector2(440f, 500f);
        private static readonly Vector2 PartyActionButtonSize = new Vector2(380f, 100f);
        private const float PartyHintHeight = 110f;
        private const float PartyHintBottom = 16f;
        private const int PartyHintFontSize = 40;
        private const float PartyHintPadding = 24f;

        private static float PartyRightColumnX => StoryScreenSideMargin + PartyListSize.x + StoryScreenGap;

        private static FieldMenu CreateFieldMenu(Transform canvas, FieldInput input)
        {
            var root = UIDialogBuilder.CreateUIObject("FieldMenu", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());

            var list = CreateFieldMenuList(root.transform, out var buttons);
            var bag = CreateBagPanel(root.transform);
            var party = CreatePartyScreen(root.transform, bag);
            // バッグは手持ちの画面の上でも開くので、一番手前に並べ直す
            bag.transform.SetAsLastSibling();

            var menu = root.AddComponent<FieldMenu>();
            var so = new SerializedObject(menu);
            so.FindProperty("_partyButton").objectReferenceValue = buttons[0];
            so.FindProperty("_bagButton").objectReferenceValue = buttons[1];
            so.FindProperty("_dexButton").objectReferenceValue = buttons[2];
            so.FindProperty("_saveButton").objectReferenceValue = buttons[3];
            so.FindProperty("_quitButton").objectReferenceValue = buttons[4];
            so.FindProperty("_closeButton").objectReferenceValue = buttons[5];
            so.FindProperty("_list").objectReferenceValue = list;
            so.FindProperty("_party").objectReferenceValue = party;
            so.FindProperty("_bag").objectReferenceValue = bag;
            so.FindProperty("_input").objectReferenceValue = input;
            so.ApplyModifiedProperties();

            root.SetActive(false);
            return menu;
        }

        /// <summary>会話の層・図鑑はメニューより後に作るので、できてからつなぐ</summary>
        private static void LinkFieldMenu(FieldMenu menu, FieldEventPlayer events, DexPanel dex, FieldController field)
        {
            var so = new SerializedObject(menu);
            so.FindProperty("_events").objectReferenceValue = events;
            so.FindProperty("_dex").objectReferenceValue = dex;
            so.ApplyModifiedProperties();
            var fieldSo = new SerializedObject(field);
            fieldSo.FindProperty("_menu").objectReferenceValue = menu;
            fieldSo.ApplyModifiedProperties();
        }

        /// <summary>手持ち / バッグ / 図鑑 / 記録する / やめる / 閉じる の順</summary>
        private static GameObject CreateFieldMenuList(Transform parent, out Button[] buttons)
        {
            string[] labels = { "手持ち", "バッグ", "図鑑", "記録する", "やめる", "閉じる" };
            var safe = CreateSafeContent(parent);
            safe.name = "MenuList";
            float height = labels.Length * FieldMenuButtonSize.y + (labels.Length - 1) * FieldMenuSpacing + FieldMenuPadding * 2;
            var frame = CreateFramedBox(safe, "Box", InfoBoxColor, out var inner);
            var rect = frame.rectTransform;
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one;
            rect.anchoredPosition = new Vector2(-FieldMenuMargin, -FieldMenuMargin);
            rect.sizeDelta = new Vector2(FieldMenuButtonSize.x + FieldMenuPadding * 2, height);
            var layout = inner.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = FieldMenuSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            buttons = new Button[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                var color = i == labels.Length - 1 ? BackColor : SlotColor;
                buttons[i] = CreateButton(inner, $"Btn_{i}", labels[i], FieldMenuButtonSize, color).GetComponent<Button>();
            }
            return safe.gameObject;
        }

        private static PartyScreen CreatePartyScreen(Transform parent, BagPanel bag)
        {
            var back = CreateNightBack(parent, "PartyScreen");
            var safe = CreateSafeContent(back.transform);
            CreateDraftTitle(safe).text = PartyScreenTitleText;
            var close = CreateTopRightClose(safe, "閉じる");

            var list = CreateStoryScreenColumn(safe, "List", new Vector2(StoryScreenSideMargin, StoryScreenTop), PartyListSize, PartyListSpacing);
            var buttons = new Button[StoryBalance.MaxPartySize];
            var labels = new Text[buttons.Length];
            var icons = new Image[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                var slot = CreateButton(list, $"Btn_Slot{i}", "", PartyListSlotSize, SlotColor);
                buttons[i] = slot.GetComponent<Button>();
                labels[i] = slot.GetComponentInChildren<Text>();
                labels[i].rectTransform.offsetMin = new Vector2(BoxIconSize + 24f, 0f);
                labels[i].rectTransform.offsetMax = new Vector2(-SlotTextRightPadding, 0f);
                labels[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                icons[i] = CreatePartyIcon(slot.transform);
                icons[i].rectTransform.sizeDelta = Vector2.one * BoxIconSize;
            }

            var actionsColumn = CreateStoryScreenColumn(safe, "Actions", new Vector2(PartyRightColumnX, StoryScreenTop), PartyActionsSize, FieldMenuSpacing);
            var detailButton = CreateButton(actionsColumn, "Btn_Detail", "詳しく 見る", PartyActionButtonSize, SlotColor);
            var swapButton = CreateButton(actionsColumn, "Btn_Swap", "並べ替え", PartyActionButtonSize, SlotColor);
            var itemButton = CreateButton(actionsColumn, "Btn_Item", "道具を 使う", PartyActionButtonSize, BagColor);
            var actionsBack = CreateButton(actionsColumn, "Btn_Back", "戻る", PartyActionButtonSize, BackColor);

            var detail = CreateDraftDetail(safe, out var detailClose);
            // ドラフトより一覧が広いので、詳しくは一覧の右から始める
            detail.GetComponent<RectTransform>().offsetMin = new Vector2(PartyRightColumnX, DraftFooterHeight);
            detailClose.GetComponentInChildren<Text>().text = "閉じる";
            var hint = CreatePartyHint(safe);

            var screen = back.gameObject.AddComponent<PartyScreen>();
            var so = new SerializedObject(screen);
            SetArray(so.FindProperty("_slotButtons"), buttons);
            SetArray(so.FindProperty("_slotLabels"), labels);
            SetArray(so.FindProperty("_slotIcons"), icons);
            so.FindProperty("_hint").objectReferenceValue = hint;
            so.FindProperty("_closeButton").objectReferenceValue = close;
            so.FindProperty("_actions").objectReferenceValue = actionsColumn.parent.gameObject;
            so.FindProperty("_detailButton").objectReferenceValue = detailButton.GetComponent<Button>();
            so.FindProperty("_swapButton").objectReferenceValue = swapButton.GetComponent<Button>();
            so.FindProperty("_itemButton").objectReferenceValue = itemButton.GetComponent<Button>();
            so.FindProperty("_actionsBackButton").objectReferenceValue = actionsBack.GetComponent<Button>();
            so.FindProperty("_detail").objectReferenceValue = detail;
            so.FindProperty("_detailCloseButton").objectReferenceValue = detailClose;
            so.FindProperty("_bag").objectReferenceValue = bag;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return screen;
        }

        /// <summary>見出しの右端の「閉じる」（あずかり箱の「やめる」と同じ置き方）</summary>
        private static Button CreateTopRightClose(Transform safe, string label)
        {
            var close = CreateButton(safe, "Btn_Close", label, StoryCloseButtonSize, BackColor);
            var rect = close.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one;
            rect.anchoredPosition = new Vector2(-StoryScreenSideMargin, -(DraftTitleHeight - StoryCloseButtonSize.y) / 2f);
            rect.sizeDelta = StoryCloseButtonSize;
            return close.GetComponent<Button>();
        }

        /// <summary>下の帯いっぱいの案内（会話窓と同じ色の枠）</summary>
        private static Text CreatePartyHint(Transform safe)
        {
            var frame = CreateFramedBox(safe, "Hint", MessageBoxColor, out var inner);
            frame.raycastTarget = false;
            var rect = frame.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.offsetMin = new Vector2(StoryScreenSideMargin, PartyHintBottom);
            rect.offsetMax = new Vector2(-StoryScreenSideMargin, PartyHintBottom + PartyHintHeight);
            var text = CreateText(inner, "Text", "", PartyHintFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            text.raycastTarget = false;
            text.rectTransform.offsetMin = new Vector2(PartyHintPadding, 0f);
            text.rectTransform.offsetMax = new Vector2(-PartyHintPadding, 0f);
            return text;
        }
    }
}
