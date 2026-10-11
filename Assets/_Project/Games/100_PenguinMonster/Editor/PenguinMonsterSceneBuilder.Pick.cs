using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 仮の選択画面（Phase 3）。全ペンモンを 8列のマスに並べ、下に「おまかせ」「けってい」。Phase 4 のドラフトで置き換える
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const int PickColumns = 8;
        private static readonly Vector2 PickCellSize = new Vector2(220f, 250f);
        private static readonly Vector2 PickCellSpacing = new Vector2(12f, 12f);
        private const float PickIconSize = 150f;
        private static readonly Vector2 PickOrderSize = new Vector2(56f, 56f);
        private const float PickTitleHeight = 110f;
        private const float PickFooterHeight = 150f;
        private static readonly Vector2 PickButtonSize = new Vector2(320f, 100f);
        private const float PickButtonSpacing = 60f;
        private const int PickTitleFontSize = 48;
        private const int PickNameFontSize = 30;
        private const int PickOrderFontSize = 44;

        private static readonly Color PickBackColor = new Color(0.16f, 0.2f, 0.32f);
        private static readonly Color PickTitleColor = Color.white;
        private static readonly Color PickOrderColor = new Color(0.85f, 0.2f, 0.2f);
        private static readonly Color PickRandomColor = new Color(0.45f, 0.45f, 0.5f);
        private static readonly Color PickDecideColor = new Color(0.88f, 0.32f, 0.3f);

        private static PickPanel CreatePickPanel(Transform parent)
        {
            var back = CreateImage(parent, "PickPanel", PickBackColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);

            var title = CreateText(back.transform, "Title", "", PickTitleFontSize, PickTitleColor, TextAnchor.MiddleCenter);
            title.rectTransform.anchorMin = new Vector2(0f, 1f);
            title.rectTransform.offsetMin = new Vector2(0f, -PickTitleHeight);

            int count = MonsterTable.All.Count;
            var grid = CreatePickGrid(back.transform);
            var buttons = new Button[count];
            var icons = new Image[count];
            var labels = new Text[count];
            var orders = new Text[count];
            for (int i = 0; i < count; i++)
            {
                CreatePickCell(grid, i, out buttons[i], out icons[i], out labels[i], out orders[i]);
            }

            var footer = CreatePickFooter(back.transform);
            var random = CreateButton(footer, "Btn_Random", "おまかせ", PickButtonSize, PickRandomColor);
            var decide = CreateButton(footer, "Btn_Decide", "けってい", PickButtonSize, PickDecideColor);

            var panel = back.gameObject.AddComponent<PickPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_titleText").objectReferenceValue = title;
            SetArray(so.FindProperty("_cellButtons"), buttons);
            SetArray(so.FindProperty("_cellIcons"), icons);
            SetArray(so.FindProperty("_cellLabels"), labels);
            SetArray(so.FindProperty("_cellOrders"), orders);
            so.FindProperty("_randomButton").objectReferenceValue = random.GetComponent<Button>();
            so.FindProperty("_decideButton").objectReferenceValue = decide.GetComponent<Button>();
            so.ApplyModifiedProperties();
            return panel;
        }

        private static Transform CreatePickGrid(Transform parent)
        {
            var grid = UIDialogBuilder.CreateUIObject("Grid", parent);
            var rect = grid.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rect);
            rect.offsetMin = new Vector2(0f, PickFooterHeight);
            rect.offsetMax = new Vector2(0f, -PickTitleHeight);
            var layout = grid.AddComponent<GridLayoutGroup>();
            layout.cellSize = PickCellSize;
            layout.spacing = PickCellSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = PickColumns;
            return grid.transform;
        }

        /// <summary>1マス = 絵（上）＋名前とタイプ（下）＋選んだ順の数字（左上）</summary>
        private static void CreatePickCell(Transform grid, int index, out Button button, out Image icon, out Text label, out Text order)
        {
            var cell = CreateButton(grid, $"Btn_Pick{index}", "", PickCellSize, Color.white);
            button = cell.GetComponent<Button>();

            label = cell.GetComponentInChildren<Text>();
            label.fontSize = PickNameFontSize;
            label.supportRichText = true;
            label.alignment = TextAnchor.LowerCenter;
            label.rectTransform.offsetMin = new Vector2(4f, 10f);

            icon = CreateImage(cell.transform, "Icon", Color.white);
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            var iconRect = icon.rectTransform;
            iconRect.anchorMin = iconRect.anchorMax = new Vector2(0.5f, 1f);
            iconRect.pivot = new Vector2(0.5f, 1f);
            iconRect.anchoredPosition = new Vector2(0f, -8f);
            iconRect.sizeDelta = new Vector2(PickIconSize, PickIconSize);

            order = CreateText(cell.transform, "Order", "", PickOrderFontSize, PickOrderColor, TextAnchor.UpperLeft);
            order.fontStyle = FontStyle.Bold;
            var orderRect = order.rectTransform;
            orderRect.anchorMin = orderRect.anchorMax = new Vector2(0f, 1f);
            orderRect.pivot = new Vector2(0f, 1f);
            orderRect.anchoredPosition = new Vector2(10f, -4f);
            orderRect.sizeDelta = PickOrderSize;
        }

        private static Transform CreatePickFooter(Transform parent)
        {
            var footer = UIDialogBuilder.CreateUIObject("Footer", parent);
            var rect = footer.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rect);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.offsetMax = new Vector2(0f, PickFooterHeight);
            var layout = footer.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = PickButtonSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            return footer.transform;
        }
    }
}
