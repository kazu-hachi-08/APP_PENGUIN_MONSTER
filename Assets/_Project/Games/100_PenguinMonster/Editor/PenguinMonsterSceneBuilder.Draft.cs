using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ドラフトの画面。夜空と雪の上に、左に提示の3枚の小さいカード（絵・名前・タイプ）、右に選んでいる1体の大きな詳細と
    /// 「これに 決めた！」、下の帯に取ったペンモンの6マス。文字とボタンはセーフエリアの内側に置く（スマホのカメラ穴で切れないように）。
    /// カードは配るたびに「ぽよん」と出し（PopIn）、取ったペンモンは一番手前の絵（Flyer）が下のマスへ運ぶ
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string StatCaptions = "HP\n攻撃\n防御\n特攻\n特防\n素早さ\n合計";
        private const int TypesPerMonster = 2;

        private const float DraftTitleHeight = 110f;
        private const float DraftFooterHeight = 140f;
        private const float DraftSideMargin = 40f;
        private const float DraftColumnGap = 30f;
        // 見出しは左に寄せ、右上の残り秒数と「やめる」に被らない所で止める
        private const float DraftTitleRightMargin = 640f;
        private const float DraftCardWidth = 500f;
        private static readonly Vector2 DraftCardSize = new Vector2(DraftCardWidth, 250f);
        private const float DraftCardSpacing = 25f;
        private const float DraftCardIconSize = 210f;
        private const float DraftCardPadding = 20f;
        private const float DraftCardTextLeft = DraftCardPadding + DraftCardIconSize + 15f;
        private const float DraftCardNameTop = 40f;
        private const float DraftCardNameHeight = 80f;
        private const float DraftCardBadgeTop = 140f;
        private static readonly Vector2 DraftCardBadgeSize = new Vector2(112f, 52f);
        private const float DraftCardBadgeSpacing = 12f;
        private static readonly Vector2 DraftSelectedEdge = new Vector2(8f, -8f);
        private const float DraftFooterCaptionWidth = 300f;
        private const float DraftPickedSlotSize = 110f;
        private const float DraftFooterSpacing = 20f;

        private const int DraftTitleFontSize = 46;
        private const int DraftCardNameFontSize = 44;
        private const int DraftCardBadgeFontSize = 28;
        private const int DraftCaptionFontSize = 34;

        private static readonly Vector2 PanelCountdownPosition = new Vector2(-480f, -55f);
        private static readonly Vector2 TopRight = new Vector2(1f, 1f);

        private static readonly Color DraftCardColor = new Color(0.97f, 0.97f, 0.9f);
        private static readonly Color DraftSelectedColor = new Color(1f, 0.85f, 0.2f);
        private static readonly Color DraftSlotColor = new Color(0.1f, 0.14f, 0.22f);

        private static DraftPanel CreateDraftPanel(Transform parent)
        {
            var back = CreateNightBack(parent, "DraftPanel");
            var safe = CreateSafeContent(back.transform);
            var title = CreateDraftTitle(safe);

            var column = CreateDraftCardColumn(safe);
            int cards = BattleBalance.DraftOfferCount;
            var buttons = new Button[cards];
            var icons = new Image[cards];
            var names = new Text[cards];
            var outlines = new Outline[cards];
            var badges = new Image[cards * TypesPerMonster];
            var badgeLabels = new Text[badges.Length];
            for (int i = 0; i < cards; i++)
            {
                CreateDraftCard(column, i, out buttons[i], out icons[i], out names[i], out outlines[i], badges, badgeLabels);
            }

            var detail = CreateDraftDetail(safe, out var decide);
            var picked = CreatePickedSlots(safe);
            var timer = CreateCountdown(safe, TopRight, PanelCountdownPosition);
            var flyer = CreateDraftFlyer(back.transform);

            var panel = back.gameObject.AddComponent<DraftPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_titleText").objectReferenceValue = title;
            SetArray(so.FindProperty("_cardButtons"), buttons);
            SetArray(so.FindProperty("_cardIcons"), icons);
            SetArray(so.FindProperty("_cardNames"), names);
            SetArray(so.FindProperty("_cardOutlines"), outlines);
            SetArray(so.FindProperty("_typeBadges"), badges);
            SetArray(so.FindProperty("_typeLabels"), badgeLabels);
            so.FindProperty("_detail").objectReferenceValue = detail;
            so.FindProperty("_decideButton").objectReferenceValue = decide;
            SetArray(so.FindProperty("_pickedIcons"), picked);
            so.FindProperty("_timer").objectReferenceValue = timer;
            so.FindProperty("_flyer").objectReferenceValue = flyer;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static Text CreateDraftTitle(Transform parent)
        {
            var title = CreateTopTitle(parent, "", DraftTitleFontSize, DraftTitleHeight, TextAnchor.MiddleLeft, DraftSideMargin);
            title.rectTransform.offsetMax = new Vector2(-DraftTitleRightMargin, 0f);
            return title;
        }

        /// <summary>取ったペンモンを運ぶ絵。位置と大きさは飛ぶたびに DraftPanel が決めるので、ここでは隠しておくだけ</summary>
        private static Image CreateDraftFlyer(Transform parent)
        {
            var flyer = CreateImage(parent, "Flyer", Color.white);
            flyer.preserveAspect = true;
            flyer.raycastTarget = false;
            Place(flyer.rectTransform, Center, Vector2.zero, Vector2.one * DraftCardIconSize);
            flyer.enabled = false;
            return flyer;
        }

        /// <summary>左の列。見出しと下の帯の間に、カード3枚を縦に並べる</summary>
        private static Transform CreateDraftCardColumn(Transform parent)
        {
            var column = UIDialogBuilder.CreateUIObject("Cards", parent);
            var rect = column.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(0f, 1f);
            rect.offsetMin = new Vector2(DraftSideMargin, DraftFooterHeight);
            rect.offsetMax = new Vector2(DraftSideMargin + DraftCardWidth, -DraftTitleHeight);
            AddChildLayout<VerticalLayoutGroup>(column, DraftCardSpacing, TextAnchor.MiddleCenter);
            return column.transform;
        }

        /// <summary>カード1枚 = 左に絵、右に名前とタイプの札。押すと右の詳細に出る（決定はしない）</summary>
        private static void CreateDraftCard(Transform column, int index, out Button button, out Image icon, out Text name, out Outline outline,
            Image[] badges, Text[] badgeLabels)
        {
            var card = CreateButton(column, $"Btn_Card{index}", "", DraftCardSize, DraftCardColor, sinkLabel: false);
            button = card.GetComponent<Button>();
            AddPopIn(card);

            // 選んでいる1枚だけ太い黄色の縁で囲む（DraftPanel が付け外しする）
            outline = card.AddComponent<Outline>();
            outline.effectColor = DraftSelectedColor;
            outline.effectDistance = DraftSelectedEdge;
            outline.enabled = false;

            icon = CreateImage(card.transform, "Icon", Color.white);
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            Place(icon.rectTransform, new Vector2(0f, 0.5f), new Vector2(DraftCardPadding + DraftCardIconSize / 2f, 0f), Vector2.one * DraftCardIconSize);

            name = card.GetComponentInChildren<Text>();
            name.fontSize = DraftCardNameFontSize;
            name.fontStyle = FontStyle.Bold;
            name.color = BoxTextColor;
            name.alignment = TextAnchor.MiddleLeft;
            // 長い名前は折り返すと下のタイプの札に重なるので、1行のまま縮める（TextFit）
            name.horizontalOverflow = HorizontalWrapMode.Overflow;
            PlaceBand(name.rectTransform, DraftCardTextLeft, DraftCardNameTop, DraftCardNameHeight);

            var badgeRow = UIDialogBuilder.CreateUIObject("Types", card.transform);
            PlaceBand(badgeRow.GetComponent<RectTransform>(), DraftCardTextLeft, DraftCardBadgeTop, DraftCardBadgeSize.y);
            AddChildLayout<HorizontalLayoutGroup>(badgeRow, DraftCardBadgeSpacing, TextAnchor.MiddleLeft);
            for (int k = 0; k < TypesPerMonster; k++)
            {
                int badge = index * TypesPerMonster + k;
                CreateTypeBadge(badgeRow.transform, k, DraftCardBadgeSize, DraftCardBadgeFontSize, out badges[badge], out badgeLabels[badge]);
            }
        }

        private static void CreateTypeBadge(Transform row, int index, Vector2 size, int fontSize, out Image badge, out Text label)
        {
            badge = CreateImage(row, $"Type{index}", Color.gray);
            badge.raycastTarget = false;
            badge.rectTransform.sizeDelta = size;
            label = CreateText(badge.transform, "Label", "", fontSize, Color.white, TextAnchor.MiddleCenter);
            label.fontStyle = FontStyle.Bold;
        }

        /// <summary>親の上端から top 下がった所に、左から left・右から right 空けて、高さ height で置く</summary>
        private static void PlaceBand(RectTransform rect, float left, float top, float height, float right = DraftCardPadding)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.offsetMin = new Vector2(left, -top - height);
            rect.offsetMax = new Vector2(-right, -top);
        }

        /// <summary>親の上端から top 下がった所に、横いっぱい・高さ height で置く</summary>
        private static void PlaceTopBand(RectTransform rect, float top, float height)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -top);
            rect.sizeDelta = new Vector2(0f, height);
        }

        /// <summary>下の帯に「取った ペンモン」の見出しと6マス。マスの中の絵を返す</summary>
        private static Image[] CreatePickedSlots(Transform parent)
        {
            var footer = CreateFooter(parent, DraftFooterHeight, DraftFooterSpacing);
            var caption = CreateText(footer, "Caption", "取った ペンモン", DraftCaptionFontSize, Color.white, TextAnchor.MiddleCenter);
            StyleHeading(caption);
            caption.rectTransform.sizeDelta = new Vector2(DraftFooterCaptionWidth, DraftPickedSlotSize);

            var icons = new Image[BattleBalance.DraftRounds];
            for (int i = 0; i < icons.Length; i++)
            {
                var slot = CreateImage(footer, $"Slot{i}", DraftSlotColor);
                slot.rectTransform.sizeDelta = new Vector2(DraftPickedSlotSize, DraftPickedSlotSize);
                icons[i] = CreateImage(slot.transform, "Icon", Color.white);
                icons[i].preserveAspect = true;
                icons[i].raycastTarget = false;
                UIDialogBuilder.SetStretchAll(icons[i].rectTransform);
            }
            return icons;
        }
    }
}
