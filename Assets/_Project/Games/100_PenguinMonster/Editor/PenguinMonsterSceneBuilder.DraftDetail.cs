using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ドラフトの右の大きな詳細（DraftDetailView）。
    /// 上の紺の帯に 絵・名前・タイプ・合計、その下に「特性」の枠、下半分は左に「種族値」の枠（数字＋バー）・右に「技」の枠（タイプ色の2×2のマス）、
    /// 一番下に「これに 決めた！」。区切りを色の枠と縦の見出しで分け、空きを作らずに文字を大きく並べる
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string DraftDecideText = "これに 決めた！";
        private const int DraftStatCount = 6;
        private const int DraftMoveCount = 4;
        private const int DraftMoveColumns = 2;

        private const float DraftDetailLeft = DraftSideMargin + DraftCardWidth + DraftColumnGap;
        private const float DraftDetailPopScale = 0.94f;
        private const float DraftDetailPadding = 18f;
        private const float DraftSectionGap = 12f;

        // 上から 帯 → 特性 → 種族値と技 → 決定ボタン の順に、詳細の高さ（約 808）を使い切るよう割り振る
        private const float DraftHeaderHeight = 190f;
        private const float DraftTraitsTop = DraftHeaderHeight + DraftSectionGap;
        private const float DraftTraitsHeight = 160f;
        private const float DraftLowerTop = DraftTraitsTop + DraftTraitsHeight + DraftSectionGap;
        private const float DraftLowerHeight = 314f;

        private const float DraftDetailIconSize = 200f;
        private const float DraftDetailTextLeft = DraftDetailPadding + DraftDetailIconSize + 32f;
        private const float DraftDetailNameTop = 12f;
        private const float DraftDetailNameHeight = 100f;
        private const float DraftDetailBadgeTop = 118f;
        private static readonly Vector2 DraftDetailBadgeSize = new Vector2(210f, 62f);
        private const float DraftDetailBadgeSpacing = 16f;
        private static readonly Vector2 DraftDetailBadgeEdge = new Vector2(3f, -3f);
        private const float DraftTotalWidth = 230f;
        private static readonly Vector2 DraftTotalSize = new Vector2(DraftTotalWidth, 150f);
        private const float DraftTotalCaptionHeight = 44f;
        // 名前とタイプの札は、右の合計のバッジの手前で止める
        private const float DraftDetailTextRight = DraftDetailPadding * 2f + DraftTotalWidth;

        private const float DraftSectionTabWidth = 56f;
        private const float DraftSectionInnerPadding = 14f;
        private const float DraftStatsWidth = 590f;
        private const float DraftStatCaptionWidth = 120f;
        private const float DraftStatValueWidth = 80f;
        private const float DraftStatBarLeft = 215f;
        private const float DraftStatBarHeight = 26f;
        private const float DraftMoveGap = 10f;
        private const float DraftMoveNameTop = 12f;
        private const float DraftMoveNameHeight = 64f;
        private const float DraftMoveDetailHeight = 50f;
        private const float DraftMoveTextPadding = 12f;
        private static readonly Vector2 DraftMoveTextEdge = new Vector2(2f, -2f);
        private static readonly Vector2 DraftDecideSize = new Vector2(560f, 96f);
        private const float DraftDecideBottom = 12f;

        private const int DraftDetailNameFontSize = 88;
        private const int DraftDetailBadgeFontSize = 40;
        private const int DraftTotalCaptionFontSize = 30;
        private const int DraftTotalFontSize = 72;
        private const int DraftSectionCaptionFontSize = 30;
        private const int DraftTraitsFontSize = 34;
        private const int DraftStatsFontSize = 36;
        private const int DraftMoveNameFontSize = 36;
        private const int DraftMoveDetailFontSize = 28;
        // 長い技名（オーロラウェーブ など）はマスの幅に収まるまで縮める
        private const int DraftMoveMinFontSize = 20;
        private const int DraftDecideFontSize = 48;

        private static readonly Color DraftHeaderColor = new Color(0.16f, 0.22f, 0.42f);
        private static readonly Color DraftTotalFillColor = new Color(0.08f, 0.12f, 0.28f);
        private static readonly Color DraftTotalEdgeColor = new Color(1f, 0.8f, 0.25f);
        private static readonly Color DraftSectionFillColor = Color.white;
        private static readonly Color DraftTraitsColor = new Color(0.93f, 0.55f, 0.2f);
        private static readonly Color DraftStatsColor = new Color(0.3f, 0.62f, 0.38f);
        private static readonly Color DraftMovesColor = new Color(0.4f, 0.42f, 0.78f);
        private static readonly Color DraftStatBarBackColor = new Color(0.86f, 0.86f, 0.82f);
        private static readonly Color DraftMoveTextEdgeColor = new Color(0f, 0f, 0f, 0.45f);

        private static DraftDetailView CreateDraftDetail(Transform parent, out Button decide)
        {
            var frame = CreateFramedBox(parent, "Detail", DraftCardColor, out var inner);
            frame.raycastTarget = false;
            var rect = frame.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(DraftDetailLeft, DraftFooterHeight);
            rect.offsetMax = new Vector2(-DraftSideMargin, -DraftTitleHeight);
            var pop = AddPopIn(frame.gameObject, DraftDetailPopScale);

            var view = frame.gameObject.AddComponent<DraftDetailView>();
            var so = new SerializedObject(view);
            CreateDraftDetailHeader(inner, so);
            CreateDraftTraits(inner, so);
            CreateDraftStats(inner, so);
            CreateDraftMoves(inner, so);
            decide = CreateDraftDecide(inner);
            so.FindProperty("_pop").objectReferenceValue = pop;
            so.ApplyModifiedProperties();
            return view;
        }

        /// <summary>上の紺の帯。絵の右に大きな名前とタイプの札、右端に種族値の合計のバッジ</summary>
        private static void CreateDraftDetailHeader(Transform inner, SerializedObject so)
        {
            var header = CreateImage(inner, "Header", DraftHeaderColor);
            header.raycastTarget = false;
            PlaceTopBand(header.rectTransform, 0f, DraftHeaderHeight);

            var icon = CreateImage(header.transform, "Icon", Color.white);
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            Place(icon.rectTransform, new Vector2(0f, 0.5f), new Vector2(DraftDetailPadding + DraftDetailIconSize / 2f, 0f), Vector2.one * DraftDetailIconSize);

            var name = CreateText(header.transform, "Name", "", DraftDetailNameFontSize, Color.white, TextAnchor.MiddleLeft);
            StyleHeading(name);
            PlaceBand(name.rectTransform, DraftDetailTextLeft, DraftDetailNameTop, DraftDetailNameHeight, DraftDetailTextRight);

            var row = UIDialogBuilder.CreateUIObject("Types", header.transform);
            PlaceBand(row.GetComponent<RectTransform>(), DraftDetailTextLeft, DraftDetailBadgeTop, DraftDetailBadgeSize.y, DraftDetailTextRight);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = DraftDetailBadgeSpacing;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            var badges = new Image[TypesPerMonster];
            var labels = new Text[TypesPerMonster];
            for (int k = 0; k < TypesPerMonster; k++)
            {
                CreateTypeBadge(row.transform, k, DraftDetailBadgeSize, DraftDetailBadgeFontSize, out badges[k], out labels[k]);
                // やみ・うみのような暗いタイプの札も紺の帯に埋もれないよう、白い縁を付ける
                var edge = badges[k].gameObject.AddComponent<Outline>();
                edge.effectColor = Color.white;
                edge.effectDistance = DraftDetailBadgeEdge;
            }

            so.FindProperty("_icon").objectReferenceValue = icon;
            so.FindProperty("_name").objectReferenceValue = name;
            SetArray(so.FindProperty("_typeBadges"), badges);
            SetArray(so.FindProperty("_typeLabels"), labels);
            so.FindProperty("_statTotal").objectReferenceValue = CreateDraftTotal(header.transform);
        }

        /// <summary>合計のバッジ（金の縁の濃紺の札に「合計」と大きな数字）。数字を返す</summary>
        private static Text CreateDraftTotal(Transform header)
        {
            var badge = CreateFramedBox(header, "Total", DraftTotalFillColor, out var inner);
            badge.color = DraftTotalEdgeColor;
            badge.raycastTarget = false;
            Place(badge.rectTransform, new Vector2(1f, 0.5f), new Vector2(-DraftDetailPadding - DraftTotalSize.x / 2f, 0f), DraftTotalSize);

            var caption = CreateText(inner, "Caption", "合計", DraftTotalCaptionFontSize, Color.white, TextAnchor.MiddleCenter);
            caption.fontStyle = FontStyle.Bold;
            PlaceTopBand(caption.rectTransform, 0f, DraftTotalCaptionHeight);

            var value = CreateText(inner, "Value", "", DraftTotalFontSize, TitleLogoColor, TextAnchor.MiddleCenter);
            value.fontStyle = FontStyle.Bold;
            value.rectTransform.offsetMax = new Vector2(0f, -DraftTotalCaptionHeight);
            return value;
        }

        private static void CreateDraftTraits(Transform inner, SerializedObject so)
        {
            var section = CreateDraftSection(inner, "TraitsBox", "特性", DraftTraitsColor, out var content);
            PlaceBand(section, DraftDetailPadding, DraftTraitsTop, DraftTraitsHeight, DraftDetailPadding);
            var traits = CreateText(content, "Traits", "", DraftTraitsFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            // ヒナは綿毛の行が増えて4行になるので、枠から少しはみ出しても切らずに出す
            traits.verticalOverflow = VerticalWrapMode.Overflow;
            so.FindProperty("_traits").objectReferenceValue = traits;
        }

        /// <summary>
        /// 色の枠＋左の縦の見出し（「特\n性」のように1文字ずつ縦に積む）。中身を置く場所を content に返す。
        /// 何の欄かを文字を読む前に色で分かるようにする
        /// </summary>
        private static RectTransform CreateDraftSection(Transform parent, string name, string caption, Color color, out Transform content)
        {
            var frame = CreateFramedBox(parent, name, DraftSectionFillColor, out var inner);
            frame.color = color;
            frame.raycastTarget = false;

            var tab = CreateImage(inner, "Tab", color);
            tab.raycastTarget = false;
            tab.rectTransform.anchorMin = Vector2.zero;
            tab.rectTransform.anchorMax = new Vector2(0f, 1f);
            tab.rectTransform.offsetMin = Vector2.zero;
            tab.rectTransform.offsetMax = new Vector2(DraftSectionTabWidth, 0f);
            var label = CreateText(tab.transform, "Caption", string.Join("\n", caption.ToCharArray()), DraftSectionCaptionFontSize, Color.white, TextAnchor.MiddleCenter);
            label.fontStyle = FontStyle.Bold;
            AddOutline(label, DraftMoveTextEdgeColor, DraftMoveTextEdge);

            var area = UIDialogBuilder.CreateUIObject("Content", inner);
            var areaRect = area.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(areaRect);
            areaRect.offsetMin = new Vector2(DraftSectionTabWidth + DraftSectionInnerPadding, 0f);
            areaRect.offsetMax = new Vector2(-DraftSectionInnerPadding, 0f);
            content = area.transform;
            return frame.rectTransform;
        }

        /// <summary>種族値の枠。1行 = 見出し・数字・バー（合計は帯の右のバッジに出す）</summary>
        private static void CreateDraftStats(Transform inner, SerializedObject so)
        {
            var section = CreateDraftSection(inner, "StatsBox", "種族値", DraftStatsColor, out var content);
            section.anchorMin = section.anchorMax = new Vector2(0f, 1f);
            section.pivot = new Vector2(0f, 1f);
            section.anchoredPosition = new Vector2(DraftDetailPadding, -DraftLowerTop);
            section.sizeDelta = new Vector2(DraftStatsWidth, DraftLowerHeight);

            // 行の高さは枠の高さを6等分して決める（枠の高さを変えても行がはみ出さないように）
            var table = UIDialogBuilder.CreateUIObject("Rows", content);
            UIDialogBuilder.SetStretchAll(table.GetComponent<RectTransform>());
            var layout = table.AddComponent<VerticalLayoutGroup>();
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;
            layout.padding = new RectOffset(0, 0, (int)FrameThickness, (int)FrameThickness);

            string[] captions = StatCaptions.Split('\n');
            var values = new Text[DraftStatCount];
            var fills = new RectTransform[DraftStatCount];
            for (int i = 0; i < DraftStatCount; i++)
            {
                values[i] = CreateDraftStatRow(table.transform, i, captions[i]);
                fills[i] = CreateDraftStatBar(values[i].transform.parent);
            }
            SetArray(so.FindProperty("_statValues"), values);
            SetArray(so.FindProperty("_statFills"), fills);
        }

        private static Text CreateDraftStatRow(Transform table, int index, string caption)
        {
            var row = UIDialogBuilder.CreateUIObject($"Row{index}", table);
            var label = CreateText(row.transform, "Caption", caption, DraftStatsFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            PlaceLeftColumn(label.rectTransform, 0f, DraftStatCaptionWidth);
            var value = CreateText(row.transform, "Value", "", DraftStatsFontSize, BoxTextColor, TextAnchor.MiddleRight);
            value.fontStyle = FontStyle.Bold;
            PlaceLeftColumn(value.rectTransform, DraftStatCaptionWidth, DraftStatValueWidth);
            return value;
        }

        /// <summary>バーの下地と中身。中身の長さ（右端のアンカー）と色は DraftDetailView が種族値から決める</summary>
        private static RectTransform CreateDraftStatBar(Transform row)
        {
            var back = CreateImage(row, "Bar", DraftStatBarBackColor);
            back.raycastTarget = false;
            var rect = back.rectTransform;
            rect.anchorMin = new Vector2(0f, 0.5f);
            rect.anchorMax = new Vector2(1f, 0.5f);
            rect.offsetMin = new Vector2(DraftStatBarLeft, -DraftStatBarHeight / 2f);
            rect.offsetMax = new Vector2(0f, DraftStatBarHeight / 2f);

            var fill = CreateImage(back.transform, "Fill", Color.white);
            fill.raycastTarget = false;
            UIDialogBuilder.SetStretchAll(fill.rectTransform);
            return fill.rectTransform;
        }

        /// <summary>行の左から x の所に、幅 width・高さいっぱいで置く</summary>
        private static void PlaceLeftColumn(RectTransform rect, float x, float width)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(0f, 1f);
            rect.offsetMin = new Vector2(x, 0f);
            rect.offsetMax = new Vector2(x + width, 0f);
        }

        /// <summary>技の枠。技4つを、タイプの色で塗った2×2のマスに並べる（技名は白、下に タイプ・分類・威力）</summary>
        private static void CreateDraftMoves(Transform inner, SerializedObject so)
        {
            var section = CreateDraftSection(inner, "MovesBox", "技", DraftMovesColor, out var content);
            float left = DraftDetailPadding + DraftStatsWidth + DraftSectionGap;
            PlaceBand(section, left, DraftLowerTop, DraftLowerHeight, DraftDetailPadding);

            var tiles = new Image[DraftMoveCount];
            var names = new Text[DraftMoveCount];
            var details = new Text[DraftMoveCount];
            for (int i = 0; i < DraftMoveCount; i++)
            {
                CreateDraftMoveTile(content, i, out tiles[i], out names[i], out details[i]);
            }
            SetArray(so.FindProperty("_moveTiles"), tiles);
            SetArray(so.FindProperty("_moveNames"), names);
            SetArray(so.FindProperty("_moveDetails"), details);
        }

        private static void CreateDraftMoveTile(Transform content, int index, out Image tile, out Text name, out Text detail)
        {
            tile = CreateImage(content, $"Move{index}", Color.gray);
            tile.raycastTarget = false;
            // 枠の大きさに合わせて伸びるよう、2×2 の升目をアンカーの割合で切る
            int rows = DraftMoveCount / DraftMoveColumns;
            int column = index % DraftMoveColumns;
            int row = index / DraftMoveColumns;
            var rect = tile.rectTransform;
            rect.anchorMin = new Vector2((float)column / DraftMoveColumns, 1f - (float)(row + 1) / rows);
            rect.anchorMax = new Vector2((float)(column + 1) / DraftMoveColumns, 1f - (float)row / rows);
            float half = DraftMoveGap / 2f;
            rect.offsetMin = new Vector2(half, half + FrameThickness);
            rect.offsetMax = new Vector2(-half, -half - FrameThickness);

            name = CreateDraftMoveText(tile.transform, "Name", DraftMoveNameFontSize, DraftMoveNameTop, DraftMoveNameHeight);
            name.fontStyle = FontStyle.Bold;
            detail = CreateDraftMoveText(tile.transform, "Detail", DraftMoveDetailFontSize, DraftMoveNameTop + DraftMoveNameHeight, DraftMoveDetailHeight);
        }

        /// <summary>マスの上の文字。どのタイプの色の上でも読めるよう白＋暗い縁にし、長い文は縮めて1行に収める</summary>
        private static Text CreateDraftMoveText(Transform tile, string name, int fontSize, float top, float height)
        {
            var text = CreateText(tile, name, "", fontSize, Color.white, TextAnchor.MiddleLeft);
            AddOutline(text, DraftMoveTextEdgeColor, DraftMoveTextEdge);
            PlaceBand(text.rectTransform, DraftMoveTextPadding, top, height, DraftMoveTextPadding);
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = DraftMoveMinFontSize;
            text.resizeTextMaxSize = fontSize;
            return text;
        }

        private static Button CreateDraftDecide(Transform inner)
        {
            var obj = CreateButton(inner, "Btn_Decide", DraftDecideText, DraftDecideSize, SelectDecideColor);
            var rect = obj.GetComponent<RectTransform>();
            Place(rect, new Vector2(0.5f, 0f), new Vector2(0f, DraftDecideBottom + DraftDecideSize.y / 2f), DraftDecideSize);
            var label = obj.GetComponentInChildren<Text>();
            label.fontSize = DraftDecideFontSize;
            label.fontStyle = FontStyle.Bold;
            return obj.GetComponent<Button>();
        }
    }
}
