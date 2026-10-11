using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 見せ合い・選出の画面。上の帯に相手の6体、まん中に自分の6体のマス、下に「おまかせ」「けってい」
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 SelectCellSize = new Vector2(250f, 290f);
        private const float SelectCellSpacing = 16f;
        private const float SelectIconSize = 180f;
        private static readonly Vector2 SelectOrderSize = new Vector2(56f, 56f);
        private const float SelectTitleHeight = 110f;
        private const float SelectFooterHeight = 150f;
        private static readonly Vector2 SelectButtonSize = new Vector2(320f, 100f);
        private const float SelectButtonSpacing = 60f;
        private const int SelectTitleFontSize = 48;
        private const int SelectNameFontSize = 30;
        private const int SelectOrderFontSize = 44;

        private const float OpponentBandHeight = 300f;
        private const float OpponentCaptionWidth = 220f;
        private static readonly Vector2 OpponentEntrySize = new Vector2(220f, 260f);
        private const float OpponentIconSize = 160f;
        private const int OpponentCaptionFontSize = 36;

        private static readonly Color SelectBackColor = new Color(0.16f, 0.2f, 0.32f);
        private static readonly Color OpponentBandColor = new Color(0.32f, 0.16f, 0.2f);
        private static readonly Color SelectTitleColor = Color.white;
        private static readonly Color SelectOrderColor = new Color(0.85f, 0.2f, 0.2f);
        private static readonly Color SelectRandomColor = new Color(0.45f, 0.45f, 0.5f);
        private static readonly Color SelectDecideColor = new Color(0.88f, 0.32f, 0.3f);

        private static SelectPanel CreateSelectPanel(Transform parent)
        {
            var back = CreateImage(parent, "SelectPanel", SelectBackColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);

            var title = CreateText(back.transform, "Title", "", SelectTitleFontSize, SelectTitleColor, TextAnchor.MiddleCenter);
            title.rectTransform.anchorMin = new Vector2(0f, 1f);
            title.rectTransform.offsetMin = new Vector2(0f, -SelectTitleHeight);

            CreateOpponentBand(back.transform, out var opponentIcons, out var opponentLabels);

            int count = BattleBalance.DraftRounds;
            var row = CreateSelectRow(back.transform);
            var buttons = new Button[count];
            var icons = new Image[count];
            var labels = new Text[count];
            var orders = new Text[count];
            for (int i = 0; i < count; i++)
            {
                CreateSelectCell(row, i, out buttons[i], out icons[i], out labels[i], out orders[i]);
            }

            var footer = CreateFooter(back.transform, SelectFooterHeight, SelectButtonSpacing);
            var random = CreateButton(footer, "Btn_Random", "おまかせ", SelectButtonSize, SelectRandomColor);
            var decide = CreateButton(footer, "Btn_Decide", "けってい", SelectButtonSize, SelectDecideColor);
            var timer = CreateCountdown(back.transform, TopRight, PanelCountdownPosition);

            var panel = back.gameObject.AddComponent<SelectPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_titleText").objectReferenceValue = title;
            SetArray(so.FindProperty("_cellButtons"), buttons);
            SetArray(so.FindProperty("_cellIcons"), icons);
            SetArray(so.FindProperty("_cellLabels"), labels);
            SetArray(so.FindProperty("_cellOrders"), orders);
            SetArray(so.FindProperty("_opponentIcons"), opponentIcons);
            SetArray(so.FindProperty("_opponentLabels"), opponentLabels);
            so.FindProperty("_randomButton").objectReferenceValue = random.GetComponent<Button>();
            so.FindProperty("_decideButton").objectReferenceValue = decide.GetComponent<Button>();
            so.FindProperty("_timer").objectReferenceValue = timer;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>タイトルのすぐ下の帯。左に「あいて」の見出し、右に相手の6体（押せない）</summary>
        private static void CreateOpponentBand(Transform parent, out Image[] icons, out Text[] labels)
        {
            var band = CreateImage(parent, "OpponentBand", OpponentBandColor);
            var rect = band.rectTransform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -SelectTitleHeight);
            rect.sizeDelta = new Vector2(0f, OpponentBandHeight);

            var caption = CreateText(band.transform, "Caption", "あいての\nペンモン", OpponentCaptionFontSize, SelectTitleColor, TextAnchor.MiddleCenter);
            caption.rectTransform.anchorMax = new Vector2(0f, 1f);
            caption.rectTransform.offsetMax = new Vector2(OpponentCaptionWidth, 0f);

            var row = UIDialogBuilder.CreateUIObject("Row", band.transform);
            var rowRect = row.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rowRect);
            rowRect.offsetMin = new Vector2(OpponentCaptionWidth, 0f);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = SelectCellSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            int count = BattleBalance.DraftRounds;
            icons = new Image[count];
            labels = new Text[count];
            for (int i = 0; i < count; i++)
            {
                var entry = UIDialogBuilder.CreateUIObject($"Opponent{i}", row.transform);
                entry.GetComponent<RectTransform>().sizeDelta = OpponentEntrySize;
                icons[i] = CreateTopIcon(entry.transform, OpponentIconSize);
                labels[i] = CreateText(entry.transform, "Label", "", SelectNameFontSize, SelectTitleColor, TextAnchor.LowerCenter);
                labels[i].supportRichText = true;
            }
        }

        private static Transform CreateSelectRow(Transform parent)
        {
            var row = UIDialogBuilder.CreateUIObject("Own", parent);
            var rect = row.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rect);
            rect.offsetMin = new Vector2(0f, SelectFooterHeight);
            rect.offsetMax = new Vector2(0f, -(SelectTitleHeight + OpponentBandHeight));
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = SelectCellSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            return row.transform;
        }

        /// <summary>1マス = 絵（上）＋名前とタイプ（下）＋選んだ順の数字（左上）</summary>
        private static void CreateSelectCell(Transform row, int index, out Button button, out Image icon, out Text label, out Text order)
        {
            var cell = CreateButton(row, $"Btn_Select{index}", "", SelectCellSize, Color.white);
            button = cell.GetComponent<Button>();

            label = cell.GetComponentInChildren<Text>();
            label.fontSize = SelectNameFontSize;
            label.supportRichText = true;
            label.alignment = TextAnchor.LowerCenter;
            label.rectTransform.offsetMin = new Vector2(4f, 10f);

            icon = CreateTopIcon(cell.transform, SelectIconSize);

            order = CreateText(cell.transform, "Order", "", SelectOrderFontSize, SelectOrderColor, TextAnchor.UpperLeft);
            order.fontStyle = FontStyle.Bold;
            var orderRect = order.rectTransform;
            orderRect.anchorMin = orderRect.anchorMax = new Vector2(0f, 1f);
            orderRect.pivot = new Vector2(0f, 1f);
            orderRect.anchoredPosition = new Vector2(10f, -4f);
            orderRect.sizeDelta = SelectOrderSize;
        }
    }
}
