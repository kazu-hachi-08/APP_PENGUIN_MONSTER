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
        // 150 体に増えたため、見やすい大きさ（マス 100）にして縦スクロールできるようにする
        private const int DexListColumns = 8;
        private static readonly Vector2 DexCellSize = new Vector2(100f, 100f);
        private const float DexCellSpacing = 6f;
        private const float DexCellIconPadding = 4f;
        // 捕まえた子の印（右上の小さなタマゴ）と、見ていない子の「？」
        private const float DexCaughtMarkSize = 26f;
        private const float DexCaughtMarkMargin = 3f;
        private const int DexUnknownFontSize = 48;
        private static readonly Color DexUnknownColor = new Color(0.55f, 0.6f, 0.72f);

        private const float DexScrollbarWidth = 14f;
        private const float DexScrollbarSpacing = 10f;
        private static readonly Color DexScrollbarTrackColor = new Color(0.06f, 0.08f, 0.14f, 0.6f);
        private static readonly Color DexScrollbarHandleColor = new Color(0.35f, 0.45f, 0.65f, 0.85f);

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
        private const int DexNameMinFontSize = 28;
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
            var back = CreateSubMenuFrame(parent, "DexPanel", "図鑑", out var content, out var backButton);
            var filters = CreateDexFilters(content);
            var cells = CreateDexList(content, out var cellIcons, out var caughtMarks, out var scrollRect);
            var detail = CreateDexDetail(content);

            return FinishSubMenu<DexPanel>(back, backButton, so =>
            {
                SetArray(so.FindProperty("_filterButtons"), filters);
                SetArray(so.FindProperty("_listButtons"), cells);
                SetArray(so.FindProperty("_listIcons"), cellIcons);
                SetArray(so.FindProperty("_listCaughtMarks"), caughtMarks);
                so.FindProperty("_scrollRect").objectReferenceValue = scrollRect;
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

        /// <summary>図鑑の体数ぶん（ストーリーの子も含む）のマス。スクロール枠に入れて一覧表示する</summary>
        private static Button[] CreateDexList(Transform parent, out Image[] icons, out Image[] caughtMarks, out ScrollRect scrollRect)
        {
            var scrollObj = UIDialogBuilder.CreateUIObject("List", parent);
            var rect = scrollObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.offsetMin = new Vector2(DexListLeft, DexBodyBottom);
            rect.offsetMax = new Vector2(DexListLeft + DexListWidth, -DexBodyTop);

            scrollRect = scrollObj.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 35f;

            // ビューポート（マスがはみ出さないようにマスクする）
            var viewport = UIDialogBuilder.CreateUIObject("Viewport", scrollObj.transform);
            var vRect = viewport.GetComponent<RectTransform>();
            vRect.anchorMin = Vector2.zero;
            vRect.anchorMax = Vector2.one;
            vRect.pivot = new Vector2(0f, 1f);
            vRect.offsetMin = Vector2.zero;
            vRect.offsetMax = new Vector2(-(DexScrollbarWidth + DexScrollbarSpacing), 0f);

            var vImg = viewport.AddComponent<Image>();
            vImg.color = Color.clear;
            viewport.AddComponent<RectMask2D>();

            // マスを並べるコンテナ
            var content = UIDialogBuilder.CreateUIObject("Content", viewport.transform);
            var cRect = content.GetComponent<RectTransform>();
            cRect.anchorMin = new Vector2(0f, 1f);
            cRect.anchorMax = new Vector2(1f, 1f);
            cRect.pivot = new Vector2(0f, 1f);
            cRect.offsetMin = Vector2.zero;
            cRect.offsetMax = Vector2.zero;

            var layout = content.AddComponent<GridLayoutGroup>();
            layout.cellSize = DexCellSize;
            layout.spacing = Vector2.one * DexCellSpacing;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = DexListColumns;
            layout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            layout.startAxis = GridLayoutGroup.Axis.Horizontal;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.padding = new RectOffset(2, 2, 2, 2);

            var fitter = content.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // スクロールバー
            var scrollbarObj = UIDialogBuilder.CreateUIObject("Scrollbar", scrollObj.transform);
            var sbRect = scrollbarObj.GetComponent<RectTransform>();
            sbRect.anchorMin = new Vector2(1f, 0f);
            sbRect.anchorMax = new Vector2(1f, 1f);
            sbRect.pivot = new Vector2(1f, 1f);
            sbRect.offsetMin = new Vector2(-DexScrollbarWidth, 0f);
            sbRect.offsetMax = Vector2.zero;

            var trackImg = scrollbarObj.AddComponent<Image>();
            trackImg.color = DexScrollbarTrackColor;

            var slidingArea = UIDialogBuilder.CreateUIObject("Sliding Area", scrollbarObj.transform);
            var saRect = slidingArea.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(saRect);

            var handle = UIDialogBuilder.CreateUIObject("Handle", slidingArea.transform);
            var hRect = handle.GetComponent<RectTransform>();
            hRect.anchorMin = Vector2.zero;
            hRect.anchorMax = Vector2.one;
            hRect.offsetMin = Vector2.zero;
            hRect.offsetMax = Vector2.zero;
            var handleImg = handle.AddComponent<Image>();
            handleImg.color = DexScrollbarHandleColor;

            var scrollbar = scrollbarObj.AddComponent<Scrollbar>();
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            scrollbar.targetGraphic = handleImg;
            scrollbar.handleRect = hRect;

            scrollRect.viewport = vRect;
            scrollRect.content = cRect;
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;

            int count = MonsterTable.Dex.Count;
            var buttons = new Button[count];
            icons = new Image[count];
            caughtMarks = new Image[count];
            for (int i = 0; i < count; i++)
            {
                var cell = CreateButton(content.transform, $"Btn_Cell{i}", "", DexCellSize, DexCellColor);
                buttons[i] = cell.GetComponent<Button>();
                icons[i] = CreateImage(cell.transform, "Icon", Color.white);
                icons[i].preserveAspect = true;
                icons[i].raycastTarget = false;
                UIDialogBuilder.SetStretchAll(icons[i].rectTransform);
                icons[i].rectTransform.offsetMin = Vector2.one * DexCellIconPadding;
                icons[i].rectTransform.offsetMax = -Vector2.one * DexCellIconPadding;
                caughtMarks[i] = CreateDexCaughtMark(cell.transform);
                var label = cell.GetComponentInChildren<Text>();
                label.fontSize = DexUnknownFontSize;
                label.color = DexUnknownColor;
            }
            return buttons;
        }

        /// <summary>マスの右上の、捕まえた印。タマゴの絵は実行時にはめる</summary>
        private static Image CreateDexCaughtMark(Transform cell)
        {
            var mark = CreateImage(cell, "Caught", Color.white);
            mark.preserveAspect = true;
            mark.raycastTarget = false;
            mark.enabled = false;
            var rect = mark.rectTransform;
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one;
            rect.anchoredPosition = -Vector2.one * DexCaughtMarkMargin;
            rect.sizeDelta = Vector2.one * DexCaughtMarkSize;
            return mark;
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
            // 長い名前（ギョウレツヒナペン など）は折り返した2行目が高さに収まらず消えるため、1行に収まるまで縮める
            detail.Name.resizeTextForBestFit = true;
            detail.Name.resizeTextMinSize = DexNameMinFontSize;
            detail.Name.resizeTextMaxSize = DexNameFontSize;
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
