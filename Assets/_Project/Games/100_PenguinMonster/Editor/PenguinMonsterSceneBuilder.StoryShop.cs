using PenguinMonster.Story;
using PenguinMonster.View;
using PenguinMonster.View.Story;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// よろず屋・あずかり箱の画面（PLAN_STORY_TOWN01.md フェーズ6）。最初の3体を選ぶ画面と同じく夜空の上に置き、
    /// 画面の下は会話窓（結果の文を出す）のために空ける
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string ShopTitleText = "よろず屋";
        private const string BoxTitleText = "あずかり箱";
        private const float StoryScreenSideMargin = 40f;
        private const float StoryScreenGap = 30f;
        private static readonly Vector2 StoryCloseButtonSize = new Vector2(260f, 90f);

        // よろず屋: 左に品の一覧、右にお金と「買う / 売る / やめる」
        private static readonly Vector2 ShopListSize = new Vector2(1100f, 640f);
        private static readonly Vector2 ShopSlotSize = new Vector2(1040f, 104f);
        private const float ShopSlotSpacing = 14f;
        private static readonly Vector2 ShopSideSize = new Vector2(360f, 640f);
        private static readonly Vector2 ShopTabSize = new Vector2(300f, 110f);
        private const int ShopMoneyFontSize = 44;

        // あずかり箱: 左に手持ち、右に箱。どちらも 6枠
        private const int BoxPageSize = 6;
        private static readonly Vector2 BoxColumnSize = new Vector2(880f, 640f);
        private static readonly Vector2 BoxSlotSize = new Vector2(820f, 84f);
        private const float BoxSlotSpacing = 8f;
        private const float BoxHeaderHeight = 70f;
        private static readonly Vector2 BoxPageButtonSize = new Vector2(90f, 60f);
        private const int BoxHeaderFontSize = 40;
        private const float BoxIconSize = 72f;

        /// <summary>中身を置く帯（見出しの下 〜 会話窓の上）の、上の端から y 下がった所</summary>
        private static float StoryScreenTop => -DraftTitleHeight - StoryScreenGap;

        private static ShopPanel CreateShopPanel(Transform parent)
        {
            var back = CreateNightBack(parent, "ShopPanel");
            var safe = CreateSafeContent(back.transform);
            CreateDraftTitle(safe).text = ShopTitleText;

            var list = CreateStoryScreenColumn(safe, "List", new Vector2(StoryScreenSideMargin, StoryScreenTop), ShopListSize, ShopSlotSpacing);
            var buttons = new Button[StoryItems.Count];
            var labels = new Text[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                var slot = CreateButton(list, $"Btn_Item{i}", "", ShopSlotSize, SlotColor);
                buttons[i] = slot.GetComponent<Button>();
                labels[i] = slot.GetComponentInChildren<Text>();
                labels[i].horizontalOverflow = HorizontalWrapMode.Overflow;
            }

            var sidePosition = new Vector2(StoryScreenSideMargin + ShopListSize.x + StoryScreenGap, StoryScreenTop);
            var side = CreateStoryScreenColumn(safe, "Side", sidePosition, ShopSideSize, ShopSlotSpacing);
            var money = CreateText(side, "Money", "", ShopMoneyFontSize, BoxTextColor, TextAnchor.MiddleCenter);
            money.rectTransform.sizeDelta = ShopTabSize;
            var buy = CreateButton(side, "Btn_Buy", "買う", ShopTabSize, YesColor);
            var sell = CreateButton(side, "Btn_Sell", "売る", ShopTabSize, BagColor);
            var close = CreateButton(side, "Btn_Close", "やめる", ShopTabSize, BackColor);

            var panel = back.gameObject.AddComponent<ShopPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_buyTab").objectReferenceValue = buy.GetComponent<Button>();
            so.FindProperty("_sellTab").objectReferenceValue = sell.GetComponent<Button>();
            so.FindProperty("_closeButton").objectReferenceValue = close.GetComponent<Button>();
            SetArray(so.FindProperty("_slotButtons"), buttons);
            SetArray(so.FindProperty("_slotLabels"), labels);
            so.FindProperty("_moneyText").objectReferenceValue = money;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static BoxPanel CreateBoxPanel(Transform parent)
        {
            var back = CreateNightBack(parent, "BoxPanel");
            var safe = CreateSafeContent(back.transform);
            CreateDraftTitle(safe).text = BoxTitleText;
            var close = CreateTopRightClose(safe, "やめる");

            var party = CreateBoxColumn(safe, "Party", StoryScreenSideMargin, out var partyButtons, out var partyLabels, out var partyIcons);
            CreateBoxHeader(party, "手持ち");
            float boxX = StoryScreenSideMargin + BoxColumnSize.x + StoryScreenGap;
            var box = CreateBoxColumn(safe, "Box", boxX, out var boxButtons, out var boxLabels, out var boxIcons);
            var boxTitle = CreateBoxHeader(box, "");
            var prev = CreateBoxPageButton(box, "Btn_Prev", "◀", -1f);
            var next = CreateBoxPageButton(box, "Btn_Next", "▶", 1f);

            var panel = back.gameObject.AddComponent<BoxPanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_partyButtons"), partyButtons);
            SetArray(so.FindProperty("_partyLabels"), partyLabels);
            SetArray(so.FindProperty("_partyIcons"), partyIcons);
            SetArray(so.FindProperty("_boxButtons"), boxButtons);
            SetArray(so.FindProperty("_boxLabels"), boxLabels);
            SetArray(so.FindProperty("_boxIcons"), boxIcons);
            so.FindProperty("_boxTitle").objectReferenceValue = boxTitle;
            so.FindProperty("_prevButton").objectReferenceValue = prev;
            so.FindProperty("_nextButton").objectReferenceValue = next;
            so.FindProperty("_closeButton").objectReferenceValue = close;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>結果の文を出す会話窓を、2つの画面につなぐ（会話窓は画面より後に作るため）</summary>
        private static void LinkStoryScreenWindow(MessageWindow window, params Component[] screens)
        {
            foreach (var screen in screens)
            {
                var so = new SerializedObject(screen);
                so.FindProperty("_window").objectReferenceValue = window;
                so.ApplyModifiedProperties();
            }
        }

        /// <summary>左上を基準に置いた枠の中に、上から縦に並べる列</summary>
        private static RectTransform CreateStoryScreenColumn(Transform parent, string name, Vector2 topLeft, Vector2 size, float spacing)
        {
            var frame = CreateFramedBox(parent, name, InfoBoxColor, out var inner);
            var rect = frame.rectTransform;
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(topLeft.x, topLeft.y);
            rect.sizeDelta = size;
            var layout = inner.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = new RectOffset(0, 0, (int)spacing, (int)spacing);
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            return inner;
        }

        /// <summary>見出しの1行ぶん空けて、絵つきの枠を6つ並べる</summary>
        private static RectTransform CreateBoxColumn(Transform parent, string name, float x, out Button[] buttons, out Text[] labels, out Image[] icons)
        {
            var column = CreateStoryScreenColumn(parent, name, new Vector2(x, StoryScreenTop), BoxColumnSize, BoxSlotSpacing);
            column.GetComponent<VerticalLayoutGroup>().padding.top = (int)BoxHeaderHeight;
            buttons = new Button[BoxPageSize];
            labels = new Text[BoxPageSize];
            icons = new Image[BoxPageSize];
            for (int i = 0; i < BoxPageSize; i++)
            {
                var slot = CreateButton(column, $"Btn_Slot{i}", "", BoxSlotSize, SlotColor);
                buttons[i] = slot.GetComponent<Button>();
                labels[i] = slot.GetComponentInChildren<Text>();
                labels[i].rectTransform.offsetMin = new Vector2(BoxIconSize + 24f, 0f);
                labels[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                icons[i] = CreatePartyIcon(slot.transform);
                icons[i].rectTransform.sizeDelta = Vector2.one * BoxIconSize;
            }
            return column;
        }

        /// <summary>列の上の見出し。並べる枠の流れに入れないよう、レイアウトから外して上端に置く</summary>
        private static Text CreateBoxHeader(RectTransform column, string label)
        {
            var text = CreateText(column, "Header", label, BoxHeaderFontSize, BoxTextColor, TextAnchor.MiddleCenter);
            text.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
            var rect = text.rectTransform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0f, BoxHeaderHeight);
            return text;
        }

        /// <param name="side">-1 で見出しの左端、1 で右端</param>
        private static Button CreateBoxPageButton(RectTransform column, string name, string label, float side)
        {
            var button = CreateButton(column, name, label, BoxPageButtonSize, SlotColor);
            button.AddComponent<LayoutElement>().ignoreLayout = true;
            var rect = button.GetComponent<RectTransform>();
            var anchor = new Vector2(side < 0f ? 0f : 1f, 1f);
            rect.anchorMin = rect.anchorMax = rect.pivot = anchor;
            rect.anchoredPosition = new Vector2(-side * BoxSlotSpacing * 2f, -(BoxHeaderHeight - BoxPageButtonSize.y) / 2f);
            rect.sizeDelta = BoxPageButtonSize;
            return button.GetComponent<Button>();
        }
    }
}
