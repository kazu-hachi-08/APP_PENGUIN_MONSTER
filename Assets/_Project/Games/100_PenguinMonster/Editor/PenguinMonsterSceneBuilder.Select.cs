using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 見せ合い・選出の画面。夜空と雪の上に、上の帯に相手の6体、まん中に自分の6体のマス、下に「おまかせ」「けってい」。
    /// 開いたら相手の帯は右から・自分の列は左から滑り込み（SlideIn）、境目の左に「VS」が弾んで出る
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

        // 帯と自分の列は画面の幅ぶん外から入れる
        private static readonly Vector2 OpponentSlideFrom = new Vector2(1920f, 0f);
        private static readonly Vector2 OwnSlideFrom = new Vector2(-1920f, 0f);
        // 相手の見出しの真下・帯と自分の列の境目。自分のマスの上の余白に収まる
        private static readonly Vector2 VsSize = new Vector2(240f, 140f);
        private const int VsFontSize = 110;
        private const float VsTilt = 8f;

        // 夜空が少し透けるよう、帯は半透明にする
        private static readonly Color OpponentBandColor = new Color(0.45f, 0.12f, 0.18f, 0.85f);
        private static readonly Color SelectTitleColor = Color.white;
        private static readonly Color SelectOrderColor = new Color(0.85f, 0.2f, 0.2f);
        private static readonly Color SelectRandomColor = new Color(0.45f, 0.45f, 0.5f);
        private static readonly Color SelectDecideColor = new Color(0.88f, 0.32f, 0.3f);

        private static SelectPanel CreateSelectPanel(Transform parent)
        {
            var back = CreateNightBack(parent, "SelectPanel");

            var title = CreateTopTitle(back.transform, "", SelectTitleFontSize, SelectTitleHeight);

            var band = CreateOpponentBand(back.transform, out var opponentIcons, out var opponentLabels);

            int count = BattleBalance.DraftRounds;
            var row = CreateCellRow(back.transform, "Own", SelectFooterHeight, SelectTitleHeight + OpponentBandHeight, SelectCellSpacing);
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
            var decide = CreateButton(footer, "Btn_Decide", "決定", SelectButtonSize, SelectDecideColor);
            var timer = CreateCountdown(back.transform, TopRight, PanelCountdownPosition);
            var vs = CreateVsBadge(back.transform);

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
            so.FindProperty("_opponentSlide").objectReferenceValue = AddSlideIn(band, OpponentSlideFrom);
            so.FindProperty("_ownSlide").objectReferenceValue = AddSlideIn(row.gameObject, OwnSlideFrom);
            so.FindProperty("_vsPop").objectReferenceValue = vs;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>相手の帯と自分の列の境目に置く「VS」。ロゴと同じ縁取りで、少し傾けて勢いを出す</summary>
        private static PopIn CreateVsBadge(Transform parent)
        {
            var vs = CreateText(parent, "Vs", "VS", VsFontSize, Color.white, TextAnchor.MiddleCenter);
            StyleHeading(vs);
            vs.color = TitleLetterColors[0];
            Place(vs.rectTransform, new Vector2(0f, 1f), new Vector2(OpponentCaptionWidth / 2f, -(SelectTitleHeight + OpponentBandHeight)), VsSize);
            vs.rectTransform.localEulerAngles = new Vector3(0f, 0f, VsTilt);
            return AddPopIn(vs.gameObject);
        }

        /// <summary>タイトルのすぐ下の帯。左に「あいて」の見出し、右に相手の6体（押せない）</summary>
        private static GameObject CreateOpponentBand(Transform parent, out Image[] icons, out Text[] labels)
        {
            var band = CreateImage(parent, "OpponentBand", OpponentBandColor);
            var rect = band.rectTransform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -SelectTitleHeight);
            rect.sizeDelta = new Vector2(0f, OpponentBandHeight);

            var caption = CreateText(band.transform, "Caption", "相手の\nペンモン", OpponentCaptionFontSize, SelectTitleColor, TextAnchor.MiddleCenter);
            caption.rectTransform.anchorMax = new Vector2(0f, 1f);
            caption.rectTransform.offsetMax = new Vector2(OpponentCaptionWidth, 0f);

            var row = UIDialogBuilder.CreateUIObject("Row", band.transform);
            var rowRect = row.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rowRect);
            rowRect.offsetMin = new Vector2(OpponentCaptionWidth, 0f);
            AddChildLayout<HorizontalLayoutGroup>(row, SelectCellSpacing, TextAnchor.MiddleCenter);

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
                labels[i].horizontalOverflow = HorizontalWrapMode.Overflow;
            }
            return band.gameObject;
        }

        /// <summary>1マス = 絵（上）＋名前とタイプ（下）＋選んだ順の数字（左上）</summary>
        private static void CreateSelectCell(Transform row, int index, out Button button, out Image icon, out Text label, out Text order)
        {
            var cell = CreateButton(row, $"Btn_Select{index}", "", SelectCellSize, Color.white, fontSize: SelectNameFontSize);
            button = cell.GetComponent<Button>();

            label = cell.GetComponentInChildren<Text>();
            label.supportRichText = true;
            label.alignment = TextAnchor.LowerCenter;
            // 長い名前は折り返すと上の絵に重なるので、1行のまま縮める（TextFit）
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
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
