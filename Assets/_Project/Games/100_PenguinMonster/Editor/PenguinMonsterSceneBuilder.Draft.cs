using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ドラフトの画面。まん中に提示の3枚のカード（絵・名前・タイプ・特性と持ち物・種族値）、下の帯に取ったペンモンの6マス
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string StatCaptions = "HP\n攻撃\n防御\n特攻\n特防\n素早さ\n合計";
        private const int TypesPerMonster = 2;

        // カード（高さ 780）の上下に 25 ずつ余白が残るよう、タイトルと下の帯は細めにする
        private const float DraftTitleHeight = 100f;
        private const float DraftFooterHeight = 150f;
        private static readonly Vector2 DraftCardSize = new Vector2(500f, 780f);
        private const float DraftCardSpacing = 60f;
        private const float DraftCardIconSize = 180f;
        private const float DraftNameTop = 192f;
        private const float DraftNameHeight = 60f;
        private const float DraftBadgeTop = 256f;
        private static readonly Vector2 DraftBadgeSize = new Vector2(170f, 52f);
        private const float DraftBadgeSpacing = 16f;
        private const float DraftTraitsTop = 318f;
        private const float DraftTraitsHeight = 140f;
        private const float DraftTraitsPaddingX = 20f;
        private const float DraftStatsHeight = 290f;
        private const float DraftStatsPaddingX = 70f;
        private const float DraftStatsPaddingBottom = 24f;
        private const float DraftFooterCaptionWidth = 300f;
        private const float DraftPickedSlotSize = 120f;
        private const float DraftFooterSpacing = 20f;

        private const int DraftTitleFontSize = 48;
        private const int DraftNameFontSize = 46;
        private const int DraftBadgeFontSize = 30;
        private const int DraftStatsFontSize = 30;
        private const int DraftTraitsFontSize = 26;
        private const int DraftCaptionFontSize = 36;

        private static readonly Vector2 PanelCountdownPosition = new Vector2(-480f, -55f);
        private static readonly Vector2 TopRight = new Vector2(1f, 1f);

        private static readonly Color DraftBackColor = new Color(0.16f, 0.24f, 0.36f);
        private static readonly Color DraftCardColor = new Color(0.97f, 0.97f, 0.9f);
        private static readonly Color DraftSlotColor = new Color(0.1f, 0.14f, 0.22f);

        private static DraftPanel CreateDraftPanel(Transform parent)
        {
            var back = CreateImage(parent, "DraftPanel", DraftBackColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);

            var title = CreateText(back.transform, "Title", "", DraftTitleFontSize, Color.white, TextAnchor.MiddleCenter);
            title.rectTransform.anchorMin = new Vector2(0f, 1f);
            title.rectTransform.offsetMin = new Vector2(0f, -DraftTitleHeight);

            var row = CreateDraftRow(back.transform);
            int cards = BattleBalance.DraftOfferCount;
            var buttons = new Button[cards];
            var icons = new Image[cards];
            var names = new Text[cards];
            var stats = new Text[cards];
            var traits = new Text[cards];
            var badges = new Image[cards * TypesPerMonster];
            var badgeLabels = new Text[badges.Length];
            for (int i = 0; i < cards; i++)
            {
                CreateDraftCard(row, i, out buttons[i], out icons[i], out names[i], out stats[i], out traits[i], badges, badgeLabels);
            }

            var picked = CreatePickedSlots(back.transform);
            var timer = CreateCountdown(back.transform, TopRight, PanelCountdownPosition);

            var panel = back.gameObject.AddComponent<DraftPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_titleText").objectReferenceValue = title;
            SetArray(so.FindProperty("_cardButtons"), buttons);
            SetArray(so.FindProperty("_cardIcons"), icons);
            SetArray(so.FindProperty("_cardNames"), names);
            SetArray(so.FindProperty("_cardStats"), stats);
            SetArray(so.FindProperty("_cardTraits"), traits);
            SetArray(so.FindProperty("_typeBadges"), badges);
            SetArray(so.FindProperty("_typeLabels"), badgeLabels);
            SetArray(so.FindProperty("_pickedIcons"), picked);
            so.FindProperty("_timer").objectReferenceValue = timer;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static Transform CreateDraftRow(Transform parent)
        {
            var row = UIDialogBuilder.CreateUIObject("Cards", parent);
            var rect = row.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rect);
            rect.offsetMin = new Vector2(0f, DraftFooterHeight);
            rect.offsetMax = new Vector2(0f, -DraftTitleHeight);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = DraftCardSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            return row.transform;
        }

        /// <summary>カード1枚 = 絵 → 名前 → タイプの札 → 特性と持ち物 → 種族値（見出しは左、数字は右）。カード全体が押せる</summary>
        private static void CreateDraftCard(Transform row, int index, out Button button, out Image icon, out Text name, out Text stats,
            out Text traits, Image[] badges, Text[] badgeLabels)
        {
            var card = CreateButton(row, $"Btn_Card{index}", "", DraftCardSize, DraftCardColor);
            button = card.GetComponent<Button>();

            icon = CreateTopIcon(card.transform, DraftCardIconSize);

            name = card.GetComponentInChildren<Text>();
            name.fontSize = DraftNameFontSize;
            name.color = BoxTextColor;
            PlaceTopBand(name.rectTransform, DraftNameTop, DraftNameHeight);

            var badgeRow = UIDialogBuilder.CreateUIObject("Types", card.transform);
            PlaceTopBand(badgeRow.GetComponent<RectTransform>(), DraftBadgeTop, DraftBadgeSize.y);
            var layout = badgeRow.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = DraftBadgeSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            for (int k = 0; k < TypesPerMonster; k++)
            {
                int badge = index * TypesPerMonster + k;
                badges[badge] = CreateImage(badgeRow.transform, $"Type{k}", Color.gray);
                badges[badge].raycastTarget = false;
                badges[badge].rectTransform.sizeDelta = DraftBadgeSize;
                badgeLabels[badge] = CreateText(badges[badge].transform, "Label", "", DraftBadgeFontSize, Color.white, TextAnchor.MiddleCenter);
                badgeLabels[badge].fontStyle = FontStyle.Bold;
            }

            traits = CreateText(card.transform, "Traits", "", DraftTraitsFontSize, BoxTextColor, TextAnchor.UpperLeft);
            PlaceTopBand(traits.rectTransform, DraftTraitsTop, DraftTraitsHeight);
            traits.rectTransform.sizeDelta = new Vector2(-DraftTraitsPaddingX * 2f, DraftTraitsHeight);

            var captions = CreateText(card.transform, "StatCaptions", StatCaptions, DraftStatsFontSize, BoxTextColor, TextAnchor.LowerLeft);
            PlaceStatsArea(captions.rectTransform);
            stats = CreateText(card.transform, "StatValues", "", DraftStatsFontSize, BoxTextColor, TextAnchor.LowerRight);
            stats.fontStyle = FontStyle.Bold;
            PlaceStatsArea(stats.rectTransform);
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

        /// <summary>種族値の欄はカードの下側。見出しと数字の Text を同じ場所に重ね、左寄せ / 右寄せで2列に見せる</summary>
        private static void PlaceStatsArea(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = new Vector2(0f, DraftStatsPaddingBottom);
            rect.sizeDelta = new Vector2(-DraftStatsPaddingX * 2f, DraftStatsHeight);
        }

        /// <summary>下の帯に「とった ペンモン」の見出しと6マス。マスの中の絵を返す</summary>
        private static Image[] CreatePickedSlots(Transform parent)
        {
            var footer = CreateFooter(parent, DraftFooterHeight, DraftFooterSpacing);
            var caption = CreateText(footer, "Caption", "取った ペンモン", DraftCaptionFontSize, Color.white, TextAnchor.MiddleCenter);
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
