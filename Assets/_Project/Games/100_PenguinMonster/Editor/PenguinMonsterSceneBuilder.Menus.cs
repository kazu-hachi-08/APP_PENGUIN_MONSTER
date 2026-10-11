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
    /// タイトルから開く画面（Phase 8）。共通の枠（見出し・もどる）と、タイプ相性表・あそびかた・設定。ずかんは .Dex.cs
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // タイトルのメニューに並べる順。CreateSubMenus の戻り値もこの順にそろえる
        private static readonly string[] SubMenuLabels = { "ずかん", "タイプ相性表", "あそびかた", "設定" };

        private const float SubMenuTitleHeight = 110f;
        private static readonly Vector2 SubMenuBackPosition = new Vector2(170f, -55f);
        private static readonly Vector2 SubMenuBackSize = new Vector2(260f, 86f);
        private const int SubMenuTitleFontSize = 52;
        private static readonly Color SubMenuBackColor = new Color(0.14f, 0.2f, 0.32f);
        private static readonly Color SubMenuButtonColor = new Color(0.45f, 0.45f, 0.5f);

        // タイプ相性表: 見出し1行 + 10タイプ、見出し1列 + 10タイプ
        private static readonly Vector2 ChartCellSize = new Vector2(150f, 78f);
        private const float ChartCellSpacing = 4f;
        private static readonly Vector2 ChartPosition = new Vector2(0f, -50f);
        private const int ChartCellFontSize = 40;
        private const int ChartHeaderFontSize = 28;
        private const int ChartCornerFontSize = 22;
        private const string ChartCorner = "わざ＼うける";
        /// <summary>マスの色がこれより暗ければ白い文字にする</summary>
        private const float ChartDarkThreshold = 0.5f;
        private static readonly Color ChartPlainColor = new Color(0.9f, 0.9f, 0.88f);
        private static readonly Color ChartSuperColor = new Color(0.9f, 0.35f, 0.3f);
        private static readonly Color ChartHalfColor = new Color(0.35f, 0.55f, 0.85f);
        private static readonly Color ChartNoneColor = new Color(0.3f, 0.3f, 0.32f);
        private static readonly Color ChartCornerColor = new Color(0.25f, 0.3f, 0.42f);

        // あそびかた
        private static readonly Vector2 HowToTabPosition = new Vector2(0f, -175f);
        private static readonly Vector2 HowToTabSize = new Vector2(320f, 90f);
        private const float HowToTabSpacing = 24f;
        private const float HowToBodyTop = 240f;
        private const float HowToBodyMarginX = 160f;
        private const float HowToBodyMarginBottom = 60f;
        private const float HowToBodyPadding = 40f;
        private const int HowToBodyFontSize = 40;
        private const float HowToLineSpacing = 1.3f;
        private static readonly Color HowToTabColor = new Color(0.35f, 0.45f, 0.65f);
        private static readonly Color HowToBodyColor = new Color(0.97f, 0.97f, 0.9f);

        // 設定
        private static readonly Vector2 SettingsSliderSize = new Vector2(900f, 140f);
        private static readonly Vector2 SettingsBgmPosition = new Vector2(0f, 230f);
        private static readonly Vector2 SettingsSePosition = new Vector2(0f, 60f);
        private static readonly Vector2 SettingsSpeedCaptionPosition = new Vector2(0f, -100f);
        private static readonly Vector2 SettingsSpeedCaptionSize = new Vector2(900f, 60f);
        private static readonly Vector2 SettingsSpeedPosition = new Vector2(0f, -200f);
        private static readonly Vector2 SettingsSpeedButtonSize = new Vector2(280f, 100f);
        private const float SettingsSpeedSpacing = 30f;
        private const int SettingsLabelFontSize = 40;
        private static readonly Color SettingsSpeedColor = new Color(0.88f, 0.32f, 0.3f);

        private static SubMenuPanel[] CreateSubMenus(Transform parent)
        {
            return new SubMenuPanel[]
            {
                CreateDexPanel(parent),
                CreateTypeChartPanel(parent),
                CreateHowToPanel(parent),
                CreateSettingsPanel(parent),
            };
        }

        /// <summary>画面いっぱいの背景・上の見出し・左上の「もどる」。中身は content（セーフエリアの内側）に置く</summary>
        private static Image CreateSubMenuFrame(Transform parent, string name, string title, out Transform content, out Button backButton)
        {
            var back = CreateImage(parent, name, SubMenuBackColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);
            content = CreateSafeContent(back.transform);

            var titleText = CreateText(content, "Title", title, SubMenuTitleFontSize, Color.white, TextAnchor.MiddleCenter);
            titleText.fontStyle = FontStyle.Bold;
            titleText.rectTransform.anchorMin = new Vector2(0f, 1f);
            titleText.rectTransform.offsetMin = new Vector2(0f, -SubMenuTitleHeight);

            var backObj = CreateButton(content, "Btn_Back", "もどる", SubMenuBackSize, SubMenuButtonColor);
            Place(backObj.GetComponent<RectTransform>(), new Vector2(0f, 1f), SubMenuBackPosition, SubMenuBackSize);
            backButton = backObj.GetComponent<Button>();
            return back;
        }

        /// <summary>シーンの参照を入れて、最初は隠しておく。共通の「もどる」もここで渡す</summary>
        private static T FinishSubMenu<T>(Image back, Button backButton, Action<SerializedObject> setFields) where T : SubMenuPanel
        {
            var panel = back.gameObject.AddComponent<T>();
            var so = new SerializedObject(panel);
            so.FindProperty("_backButton").objectReferenceValue = backButton;
            setFields?.Invoke(so);
            so.ApplyModifiedProperties();
            back.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>
        /// 中身は TypeChart から生成時に焼き込む（実行時に変わらないため）。相性を変えたら Rebuild する
        /// </summary>
        private static SubMenuPanel CreateTypeChartPanel(Transform parent)
        {
            var back = CreateSubMenuFrame(parent, "TypeChartPanel", "タイプ相性表（たて: わざ　よこ: うける ペンモン）", out var content, out var backButton);

            var types = (MonsterType[])Enum.GetValues(typeof(MonsterType));
            int side = types.Length + 1;
            var grid = UIDialogBuilder.CreateUIObject("Chart", content);
            var size = new Vector2(side * ChartCellSize.x + (side - 1) * ChartCellSpacing, side * ChartCellSize.y + (side - 1) * ChartCellSpacing);
            Place(grid.GetComponent<RectTransform>(), Center, ChartPosition, size);
            var layout = grid.AddComponent<GridLayoutGroup>();
            layout.cellSize = ChartCellSize;
            layout.spacing = Vector2.one * ChartCellSpacing;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = side;

            CreateChartCell(grid.transform, "Corner", ChartCorner, ChartCornerColor, ChartCornerFontSize);
            foreach (var defense in types)
            {
                CreateChartTypeHeader(grid.transform, defense);
            }
            foreach (var attack in types)
            {
                CreateChartTypeHeader(grid.transform, attack);
                foreach (var defense in types)
                {
                    float multiplier = TypeChart.GetMultiplier(attack, defense);
                    CreateChartCell(grid.transform, $"{attack}_{defense}", ChartLabel(multiplier), ChartColor(multiplier), ChartCellFontSize);
                }
            }

            return FinishSubMenu<SubMenuPanel>(back, backButton, null);
        }

        private static void CreateChartTypeHeader(Transform grid, MonsterType type)
        {
            var label = CreateChartCell(grid, $"Header_{type}", "", Color.white, ChartHeaderFontSize);
            TypeColors.PaintBadge(label.transform.parent.GetComponent<Image>(), label, type);
        }

        private static Text CreateChartCell(Transform grid, string name, string label, Color color, int fontSize)
        {
            var cell = CreateImage(grid, name, color);
            cell.raycastTarget = false;
            bool dark = color.grayscale < ChartDarkThreshold;
            var text = CreateText(cell.transform, "Label", label, fontSize, dark ? Color.white : BoxTextColor, TextAnchor.MiddleCenter);
            text.fontStyle = FontStyle.Bold;
            return text;
        }

        // 仕様書 §3.4 と同じ書き方（等倍は空欄）
        private static string ChartLabel(float multiplier)
        {
            return multiplier == 0f ? "0" : multiplier < 1f ? "½" : multiplier > 1f ? "2" : "";
        }

        private static Color ChartColor(float multiplier)
        {
            return multiplier == 0f ? ChartNoneColor : multiplier < 1f ? ChartHalfColor : multiplier > 1f ? ChartSuperColor : ChartPlainColor;
        }

        /// <summary>上にタブ、下に説明の枠。文面は HowToPanel が持つ</summary>
        private static HowToPanel CreateHowToPanel(Transform parent)
        {
            var back = CreateSubMenuFrame(parent, "HowToPanel", "あそびかた", out var content, out var backButton);

            var tabRow = CreateCenteredRow(content, "Tabs", new Vector2(0.5f, 1f), HowToTabPosition, HowToTabSize.y, HowToTabSpacing);
            var tabs = new Button[HowToPanel.PageTitles.Length];
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i] = CreateButton(tabRow, $"Btn_Tab{i}", HowToPanel.PageTitles[i], HowToTabSize, HowToTabColor).GetComponent<Button>();
            }

            var box = CreateFramedBox(content, "Body", HowToBodyColor, out var inner);
            UIDialogBuilder.SetStretchAll(box.rectTransform);
            box.rectTransform.offsetMin = new Vector2(HowToBodyMarginX, HowToBodyMarginBottom);
            box.rectTransform.offsetMax = new Vector2(-HowToBodyMarginX, -HowToBodyTop);
            var body = CreateText(inner, "Text", "", HowToBodyFontSize, BoxTextColor, TextAnchor.UpperLeft);
            body.lineSpacing = HowToLineSpacing;
            body.rectTransform.offsetMin = Vector2.one * HowToBodyPadding;
            body.rectTransform.offsetMax = -Vector2.one * HowToBodyPadding;

            return FinishSubMenu<HowToPanel>(back, backButton, so =>
            {
                SetArray(so.FindProperty("_tabButtons"), tabs);
                so.FindProperty("_bodyText").objectReferenceValue = body;
            });
        }

        /// <summary>音量のスライダーは共通基盤の部品をそのまま使い、見出しだけ大きくする</summary>
        private static SettingsPanel CreateSettingsPanel(Transform parent)
        {
            var back = CreateSubMenuFrame(parent, "SettingsPanel", "設定", out var content, out var backButton);

            var bgm = CreateSettingsSlider(content, "BgmSlider", "BGM の おおきさ", SettingsBgmPosition);
            var se = CreateSettingsSlider(content, "SeSlider", "こうかおん の おおきさ", SettingsSePosition);

            var caption = CreateText(content, "SpeedCaption", "メッセージの はやさ", SettingsLabelFontSize, Color.white, TextAnchor.MiddleLeft);
            Place(caption.rectTransform, Center, SettingsSpeedCaptionPosition, SettingsSpeedCaptionSize);
            var speedRow = CreateCenteredRow(content, "Speeds", Center, SettingsSpeedPosition, SettingsSpeedButtonSize.y, SettingsSpeedSpacing);
            var speeds = new Button[GameSettings.SpeedLabels.Length];
            for (int i = 0; i < speeds.Length; i++)
            {
                speeds[i] = CreateButton(speedRow, $"Btn_Speed{i}", GameSettings.SpeedLabels[i], SettingsSpeedButtonSize, SettingsSpeedColor)
                    .GetComponent<Button>();
            }

            return FinishSubMenu<SettingsPanel>(back, backButton, so =>
            {
                so.FindProperty("_bgmSlider").objectReferenceValue = bgm;
                so.FindProperty("_seSlider").objectReferenceValue = se;
                SetArray(so.FindProperty("_speedButtons"), speeds);
            });
        }

        private static Slider CreateSettingsSlider(Transform parent, string name, string label, Vector2 position)
        {
            var slider = UIDialogBuilder.CreateVolumeSlider(name, parent, label);
            var row = (RectTransform)slider.transform.parent;
            Place(row, Center, position, SettingsSliderSize);
            row.Find("Label").GetComponent<Text>().fontSize = SettingsLabelFontSize;
            return slider;
        }

        /// <summary>中身を横に中央寄せで並べる行。幅は親いっぱい</summary>
        private static Transform CreateCenteredRow(Transform parent, string name, Vector2 anchor, Vector2 position, float height, float spacing)
        {
            var row = UIDialogBuilder.CreateUIObject(name, parent);
            var rect = row.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, anchor.y);
            rect.anchorMax = new Vector2(1f, anchor.y);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(0f, height);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            return row.transform;
        }
    }
}
