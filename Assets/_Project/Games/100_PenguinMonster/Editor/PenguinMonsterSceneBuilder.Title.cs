using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// タイトル画面。上にロゴ、まん中にメニュー（2列×3段）、左右に飾りのペンモン
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // 仕様書 §2 のメニュー。勝ち抜き戦とオンライン対戦が押せる。他は作ったフェーズで押せるようにする
        private static readonly string[] ComingSoonMenus = { "ずかん", "タイプ相性表", "あそびかた", "設定" };
        private const string SurvivalMenu = "勝ち抜き戦";
        private const string OnlineMenu = "オンライン対戦";
        private const int PlayableMenuCount = 2;
        private const int TitleMenuColumns = 2;
        private const int TitleMascotCount = 2;

        private static readonly Vector2 TitleLogoPosition = new Vector2(0f, -230f);
        private static readonly Vector2 TitleLogoSize = new Vector2(1200f, 240f);
        private static readonly Vector2 TitleSubPosition = new Vector2(0f, -380f);
        private static readonly Vector2 TitleSubSize = new Vector2(1200f, 70f);
        private static readonly Vector2 TitleMenuPosition = new Vector2(0f, -190f);
        private static readonly Vector2 TitleMenuButtonSize = new Vector2(480f, 110f);
        private static readonly Vector2 TitleMenuSpacing = new Vector2(30f, 30f);
        private static readonly Vector2 TitleBestPosition = new Vector2(0f, 80f);
        private static readonly Vector2 TitleBestSize = new Vector2(1200f, 60f);
        private const float TitleMascotX = 720f;
        private const float TitleMascotY = -60f;
        private const float TitleMascotSize = 340f;

        private const int TitleLogoFontSize = 180;
        private const int TitleSubFontSize = 40;
        private const int TitleMenuFontSize = 44;
        private const int TitleBestFontSize = 40;

        private static readonly Color TitleBackColor = new Color(0.14f, 0.26f, 0.48f);
        private static readonly Color TitleLogoColor = new Color(1f, 0.95f, 0.75f);
        private static readonly Color TitleSubColor = new Color(0.8f, 0.88f, 1f);
        private static readonly Color SurvivalColor = new Color(0.88f, 0.32f, 0.3f);
        private static readonly Color OnlineColor = new Color(0.2f, 0.55f, 0.4f);
        private static readonly Color MenuColor = new Color(0.35f, 0.45f, 0.65f);

        private static TitlePanel CreateTitlePanel(Transform parent)
        {
            var back = CreateImage(parent, "TitlePanel", TitleBackColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);

            var top = new Vector2(0.5f, 1f);
            var logo = CreateText(back.transform, "Logo", "ペンモン", TitleLogoFontSize, TitleLogoColor, TextAnchor.MiddleCenter);
            logo.fontStyle = FontStyle.Bold;
            Place(logo.rectTransform, top, TitleLogoPosition, TitleLogoSize);
            var sub = CreateText(back.transform, "Sub", "ペンギンだけの モンスターバトル", TitleSubFontSize, TitleSubColor, TextAnchor.MiddleCenter);
            Place(sub.rectTransform, top, TitleSubPosition, TitleSubSize);
            // 最高連勝数はメニューの下。文面は実行時に入れる
            var best = CreateText(back.transform, "Best", "", TitleSubFontSize, TitleLogoColor, TextAnchor.MiddleCenter);
            Place(best.rectTransform, new Vector2(0.5f, 0f), TitleBestPosition, TitleBestSize);

            var mascots = new Image[TitleMascotCount];
            for (int i = 0; i < mascots.Length; i++)
            {
                float side = i == 0 ? -1f : 1f;
                mascots[i] = CreateImage(back.transform, $"Mascot{i}", Color.white);
                mascots[i].preserveAspect = true;
                mascots[i].raycastTarget = false;
                Place(mascots[i].rectTransform, Center, new Vector2(side * TitleMascotX, TitleMascotY), Vector2.one * TitleMascotSize);
            }

            var menu = CreateTitleMenu(back.transform);
            var survival = CreateTitleMenuButton(menu, SurvivalMenu, SurvivalColor);
            var online = CreateTitleMenuButton(menu, OnlineMenu, OnlineColor);
            var comingSoon = new Button[ComingSoonMenus.Length];
            for (int i = 0; i < comingSoon.Length; i++)
            {
                comingSoon[i] = CreateTitleMenuButton(menu, ComingSoonMenus[i], MenuColor);
            }

            var panel = back.gameObject.AddComponent<TitlePanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_survivalButton").objectReferenceValue = survival;
            so.FindProperty("_onlineButton").objectReferenceValue = online;
            SetArray(so.FindProperty("_comingSoonButtons"), comingSoon);
            SetArray(so.FindProperty("_mascots"), mascots);
            so.FindProperty("_bestText").objectReferenceValue = best;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static Transform CreateTitleMenu(Transform parent)
        {
            int rows = (ComingSoonMenus.Length + PlayableMenuCount + TitleMenuColumns - 1) / TitleMenuColumns;
            var size = new Vector2(
                TitleMenuColumns * TitleMenuButtonSize.x + (TitleMenuColumns - 1) * TitleMenuSpacing.x,
                rows * TitleMenuButtonSize.y + (rows - 1) * TitleMenuSpacing.y);

            var menu = UIDialogBuilder.CreateUIObject("Menu", parent);
            Place(menu.GetComponent<RectTransform>(), Center, TitleMenuPosition, size);
            var layout = menu.AddComponent<GridLayoutGroup>();
            layout.cellSize = TitleMenuButtonSize;
            layout.spacing = TitleMenuSpacing;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = TitleMenuColumns;
            return menu.transform;
        }

        private static Button CreateTitleMenuButton(Transform menu, string label, Color color)
        {
            var obj = CreateButton(menu, $"Btn_{label}", label, TitleMenuButtonSize, color);
            obj.GetComponentInChildren<Text>().fontSize = TitleMenuFontSize;
            return obj.GetComponent<Button>();
        }
    }
}
