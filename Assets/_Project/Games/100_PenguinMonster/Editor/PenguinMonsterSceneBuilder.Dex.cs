using System;
using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ずかん。上の帯にタイプのしぼりこみ（すべて + 10タイプ）、左に一覧のマス、右に選んだペンモンの詳しい枠
    /// （左の列: 絵・種族値 / 右の列: 名前・タイプ・特性と持ち物・技）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string DexAllLabel = "すべて";
        private static readonly Vector2 DexFilterPosition = new Vector2(0f, -170f);
        private static readonly Vector2 DexFilterSize = new Vector2(150f, 66f);
        private const float DexFilterSpacing = 10f;
        private const int DexFilterFontSize = 28;

        private const float DexBodyTop = 230f;
        private const float DexBodyBottom = 40f;
        private const float DexListLeft = 40f;
        private const float DexListWidth = 870f;
        private const int DexListColumns = 6;
        private static readonly Vector2 DexCellSize = new Vector2(132f, 132f);
        private const float DexCellSpacing = 12f;
        private const float DexCellIconPadding = 6f;

        private const float DexDetailLeft = 940f;
        private const float DexDetailRight = 40f;
        private const float DexDetailPadding = 24f;
        private const float DexLeftColumnWidth = 300f;
        private const float DexRightColumnLeft = 340f;
        private const float DexIconSize = 220f;
        private const float DexStatsTop = 250f;
        private const float DexStatsHeight = 300f;
        private const float DexNameHeight = 70f;
        private const float DexBadgeTop = 80f;
        private static readonly Vector2 DexBadgeSize = new Vector2(170f, 52f);
        private const float DexBadgeSpacing = 16f;
        private const float DexTraitsTop = 150f;
        private const float DexTraitsHeight = 170f;
        private const float DexMovesTop = 330f;

        private const int DexNameFontSize = 48;
        private const int DexBadgeFontSize = 30;
        private const int DexStatsFontSize = 32;
        private const int DexTraitsFontSize = 30;
        private const int DexMovesFontSize = 30;

        private static readonly Color DexAllColor = new Color(0.45f, 0.45f, 0.5f);
        private static readonly Color DexCellColor = new Color(0.1f, 0.14f, 0.22f);
        private static readonly Color DexDetailColor = new Color(0.1f, 0.14f, 0.22f);
        private static readonly Color DexTextColor = Color.white;

        private static DexPanel CreateDexPanel(Transform parent)
        {
            var back = CreateSubMenuFrame(parent, "DexPanel", "ずかん", out var backButton);
            var filters = CreateDexFilters(back.transform);
            var cells = CreateDexList(back.transform, out var cellIcons);
            var detail = CreateDexDetail(back.transform);

            return FinishSubMenu<DexPanel>(back, backButton, so =>
            {
                SetArray(so.FindProperty("_filterButtons"), filters);
                SetArray(so.FindProperty("_listButtons"), cells);
                SetArray(so.FindProperty("_listIcons"), cellIcons);
                so.FindProperty("_cellColor").colorValue = DexCellColor;
                detail.Apply(so);
            });
        }

        /// <summary>「すべて」とタイプの数だけボタンを並べる。タイプのボタンは札と同じ色</summary>
        private static Button[] CreateDexFilters(Transform parent)
        {
            var row = CreateCenteredRow(parent, "Filters", new Vector2(0.5f, 1f), DexFilterPosition, DexFilterSize.y, DexFilterSpacing);
            var types = (MonsterType[])Enum.GetValues(typeof(MonsterType));
            var buttons = new Button[types.Length + 1];
            buttons[0] = CreateDexFilterButton(row, DexAllLabel, DexAllColor);
            for (int i = 0; i < types.Length; i++)
            {
                buttons[i + 1] = CreateDexFilterButton(row, types[i].ToJapanese(), Color.white);
                TypeColors.PaintBadge(buttons[i + 1].image, buttons[i + 1].GetComponentInChildren<Text>(), types[i]);
            }
            return buttons;
        }

        private static Button CreateDexFilterButton(Transform row, string label, Color color)
        {
            var obj = CreateButton(row, $"Btn_{label}", label, DexFilterSize, color);
            obj.GetComponentInChildren<Text>().fontSize = DexFilterFontSize;
            return obj.GetComponent<Button>();
        }

        /// <summary>定義表の体数ぶんのマス。しぼりこみで使わないマスは実行時に隠す</summary>
        private static Button[] CreateDexList(Transform parent, out Image[] icons)
        {
            var list = UIDialogBuilder.CreateUIObject("List", parent);
            var rect = list.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.offsetMin = new Vector2(DexListLeft, DexBodyBottom);
            rect.offsetMax = new Vector2(DexListLeft + DexListWidth, -DexBodyTop);
            var layout = list.AddComponent<GridLayoutGroup>();
            layout.cellSize = DexCellSize;
            layout.spacing = Vector2.one * DexCellSpacing;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = DexListColumns;

            int count = MonsterTable.All.Count;
            var buttons = new Button[count];
            icons = new Image[count];
            for (int i = 0; i < count; i++)
            {
                var cell = CreateButton(list.transform, $"Btn_Cell{i}", "", DexCellSize, DexCellColor);
                buttons[i] = cell.GetComponent<Button>();
                icons[i] = CreateImage(cell.transform, "Icon", Color.white);
                icons[i].preserveAspect = true;
                icons[i].raycastTarget = false;
                UIDialogBuilder.SetStretchAll(icons[i].rectTransform);
                icons[i].rectTransform.offsetMin = Vector2.one * DexCellIconPadding;
                icons[i].rectTransform.offsetMax = -Vector2.one * DexCellIconPadding;
            }
            return buttons;
        }

        /// <summary>右の詳しい枠の部品。SerializedObject への登録をまとめるために束ねる</summary>
        private struct DexDetail
        {
            public Image Icon;
            public Text Name;
            public Image[] Badges;
            public Text[] BadgeLabels;
            public Text StatValues;
            public Text Moves;
            public Text Traits;

            public void Apply(SerializedObject so)
            {
                so.FindProperty("_icon").objectReferenceValue = Icon;
                so.FindProperty("_nameText").objectReferenceValue = Name;
                SetArray(so.FindProperty("_typeBadges"), Badges);
                SetArray(so.FindProperty("_typeLabels"), BadgeLabels);
                so.FindProperty("_statValues").objectReferenceValue = StatValues;
                so.FindProperty("_movesText").objectReferenceValue = Moves;
                so.FindProperty("_traitsText").objectReferenceValue = Traits;
            }
        }

        private static DexDetail CreateDexDetail(Transform parent)
        {
            var detail = new DexDetail();
            var box = CreateFramedBox(parent, "Detail", DexDetailColor, out var inner);
            UIDialogBuilder.SetStretchAll(box.rectTransform);
            box.rectTransform.offsetMin = new Vector2(DexDetailLeft, DexBodyBottom);
            box.rectTransform.offsetMax = new Vector2(-DexDetailRight, -DexBodyTop);

            var left = CreateDexColumn(inner, "Left", 0f, DexLeftColumnWidth);
            var right = CreateDexColumn(inner, "Right", DexRightColumnLeft, 0f);

            detail.Icon = CreateTopIcon(left, DexIconSize, 0f);
            var captions = CreateText(left, "StatCaptions", StatCaptions, DexStatsFontSize, DexTextColor, TextAnchor.UpperLeft);
            PlaceTopBand(captions.rectTransform, DexStatsTop, DexStatsHeight);
            detail.StatValues = CreateText(left, "StatValues", "", DexStatsFontSize, DexTextColor, TextAnchor.UpperRight);
            detail.StatValues.fontStyle = FontStyle.Bold;
            PlaceTopBand(detail.StatValues.rectTransform, DexStatsTop, DexStatsHeight);

            detail.Name = CreateText(right, "Name", "", DexNameFontSize, DexTextColor, TextAnchor.MiddleLeft);
            detail.Name.fontStyle = FontStyle.Bold;
            PlaceTopBand(detail.Name.rectTransform, 0f, DexNameHeight);
            CreateDexBadges(right, out detail.Badges, out detail.BadgeLabels);

            detail.Traits = CreateText(right, "Traits", "", DexTraitsFontSize, DexTextColor, TextAnchor.UpperLeft);
            PlaceTopBand(detail.Traits.rectTransform, DexTraitsTop, DexTraitsHeight);
            detail.Moves = CreateText(right, "Moves", "", DexMovesFontSize, DexTextColor, TextAnchor.UpperLeft);
            var moves = detail.Moves.rectTransform;
            UIDialogBuilder.SetStretchAll(moves);
            moves.offsetMax = new Vector2(0f, -DexMovesTop);
            return detail;
        }

        /// <summary>枠の内側に、左端から left の所に幅 width の縦長の列を作る（余白つき）。width が 0 なら右端まで</summary>
        private static Transform CreateDexColumn(Transform inner, string name, float left, float width)
        {
            var column = UIDialogBuilder.CreateUIObject(name, inner);
            var rect = column.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rect);
            if (width > 0f)
            {
                rect.anchorMax = new Vector2(0f, 1f);
                rect.offsetMin = new Vector2(left + DexDetailPadding, DexDetailPadding);
                rect.offsetMax = new Vector2(left + DexDetailPadding + width, -DexDetailPadding);
            }
            else
            {
                rect.offsetMin = new Vector2(left + DexDetailPadding, DexDetailPadding);
                rect.offsetMax = new Vector2(-DexDetailPadding, -DexDetailPadding);
            }
            return column.transform;
        }

        private static void CreateDexBadges(Transform parent, out Image[] badges, out Text[] labels)
        {
            var row = UIDialogBuilder.CreateUIObject("Types", parent);
            PlaceTopBand(row.GetComponent<RectTransform>(), DexBadgeTop, DexBadgeSize.y);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = DexBadgeSpacing;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            badges = new Image[TypeColors.BadgesPerMonster];
            labels = new Text[badges.Length];
            for (int k = 0; k < badges.Length; k++)
            {
                badges[k] = CreateImage(row.transform, $"Type{k}", Color.gray);
                badges[k].raycastTarget = false;
                badges[k].rectTransform.sizeDelta = DexBadgeSize;
                labels[k] = CreateText(badges[k].transform, "Label", "", DexBadgeFontSize, Color.white, TextAnchor.MiddleCenter);
                labels[k].fontStyle = FontStyle.Bold;
            }
        }
    }
}
