using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 持ち物の画面。夜空と雪の上に、上にもらった持ち物のカード（名前・説明。開くたびに「ぽよん」と出る）、
    /// 下に自分の6体のマス（絵・名前）。マスの大きさは見せ合い・選出と同じ
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string ItemTitleText = "持ち物を もらった！ 持たせる ペンモンを 選んでください";
        private const float ItemTitleHeight = 110f;
        private static readonly Vector2 ItemCardSize = new Vector2(900f, 300f);
        private const float ItemCardTop = 140f;
        private const float ItemCardPadding = 30f;
        private const float ItemNameHeight = 110f;
        private const float ItemRowBottom = 80f;
        private const float ItemRowTop = 480f;
        private const int ItemNameFontSize = 64;
        private const int ItemDescriptionFontSize = 40;

        private static readonly Color ItemCellColor = new Color(0.3f, 0.5f, 0.8f);

        private static ItemPanel CreateItemPanel(Transform parent)
        {
            var back = CreateNightBack(parent, "ItemPanel");

            CreateTopTitle(back.transform, ItemTitleText, SelectTitleFontSize, ItemTitleHeight);

            var card = CreateItemCard(back.transform, out var itemName, out var itemDescription);

            var row = CreateCellRow(back.transform, "Own", ItemRowBottom, ItemRowTop, SelectCellSpacing);
            int count = BattleBalance.DraftRounds;
            var buttons = new Button[count];
            var icons = new Image[count];
            var names = new Text[count];
            for (int i = 0; i < count; i++)
            {
                CreateItemCell(row, i, out buttons[i], out icons[i], out names[i]);
            }
            var timer = CreateCountdown(back.transform, TopRight, PanelCountdownPosition);

            var panel = back.gameObject.AddComponent<ItemPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_itemName").objectReferenceValue = itemName;
            so.FindProperty("_itemDescription").objectReferenceValue = itemDescription;
            so.FindProperty("_itemPop").objectReferenceValue = AddPopIn(card);
            SetArray(so.FindProperty("_cellButtons"), buttons);
            SetArray(so.FindProperty("_cellIcons"), icons);
            SetArray(so.FindProperty("_cellNames"), names);
            so.FindProperty("_timer").objectReferenceValue = timer;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>上のまん中のカード。名前（大きく）の下に説明</summary>
        private static GameObject CreateItemCard(Transform parent, out Text name, out Text description)
        {
            var card = CreateImage(parent, "ItemCard", DraftCardColor);
            card.raycastTarget = false;
            var rect = card.rectTransform;
            rect.anchorMin = rect.anchorMax = TopCenter;
            rect.pivot = TopCenter;
            rect.anchoredPosition = new Vector2(0f, -ItemCardTop);
            rect.sizeDelta = ItemCardSize;

            name = CreateText(card.transform, "Name", "", ItemNameFontSize, BoxTextColor, TextAnchor.MiddleCenter);
            name.fontStyle = FontStyle.Bold;
            PlaceTopBand(name.rectTransform, ItemCardPadding, ItemNameHeight);

            description = CreateText(card.transform, "Description", "", ItemDescriptionFontSize, BoxTextColor, TextAnchor.UpperCenter);
            PlaceTopBand(description.rectTransform, ItemCardPadding + ItemNameHeight, ItemCardSize.y - ItemNameHeight - ItemCardPadding * 2f);
            return card.gameObject;
        }

        /// <summary>1マス = 絵（上）＋名前（下）。マス全体が押せる</summary>
        private static void CreateItemCell(Transform row, int index, out Button button, out Image icon, out Text name)
        {
            var cell = CreateButton(row, $"Btn_Holder{index}", "", SelectCellSize, ItemCellColor, fontSize: SelectNameFontSize);
            button = cell.GetComponent<Button>();

            name = cell.GetComponentInChildren<Text>();
            name.alignment = TextAnchor.LowerCenter;
            // 長い名前は折り返すと上の絵に重なるので、1行のまま縮める（TextFit）
            name.horizontalOverflow = HorizontalWrapMode.Overflow;
            name.rectTransform.offsetMin = new Vector2(4f, 10f);

            icon = CreateTopIcon(cell.transform, SelectIconSize);
        }
    }
}
