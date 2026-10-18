using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// タイトル画面。夜空に雪が降り、下の雪原をペンモンが歩く。
    /// 上にロゴと飾りのペンモン、まん中に大きなメニューボタン3つ（ストーリー・勝ち抜き戦・オンライン対戦）と最高記録、下に小さなメニューの列
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // メニュー。ストーリー・勝ち抜き戦・オンライン対戦を大きく出し、SubMenuLabels（ずかん〜設定）は下の列に並べる
        private const string StoryMenu = "ストーリー";
        private const string SurvivalMenu = "勝ち抜き戦";
        private const string OnlineMenu = "オンライン対戦";
        private const int TitleMascotCount = 2;
        private const int TitleWalkerCount = 2;
        private const int TitleSnowCount = 40;

        private static readonly Vector2 TitleLogoPosition = new Vector2(0f, -190f);
        private static readonly Vector2 TitleLogoSize = new Vector2(1200f, 240f);
        private static readonly string[] TitleLogoLetters = { "ペ", "ン", "モ", "ン" };
        private const float TitleLetterSpacing = 200f;
        private static readonly Vector2 TitleLetterSize = new Vector2(220f, 260f);
        private const float TitleLetterTilt = 6f;
        private static readonly Vector2 TitleLetterInnerEdge = new Vector2(4f, -4f);
        private static readonly Vector2 TitleLetterOuterEdge = new Vector2(8f, -8f);
        private static readonly Vector2 TitleLogoShadow = new Vector2(10f, -14f);
        private static readonly Vector2 TitleSubPosition = new Vector2(0f, -330f);
        private static readonly Vector2 TitleSubSize = new Vector2(1200f, 70f);
        private const float TitleMascotX = 620f;
        private const float TitleMascotY = -200f;
        private const float TitleMascotSize = 240f;

        private static readonly Vector2 TitleMainMenuPosition = new Vector2(0f, 20f);
        // 3つ並べても基準の横幅 1920 に収まる幅（「オンライン対戦」の7文字が入る）
        private static readonly Vector2 TitleMainButtonSize = new Vector2(560f, 200f);
        private const float TitleMainSpacing = 40f;
        private static readonly Vector2 TitleBestPosition = new Vector2(0f, -150f);
        // シングルとダブルスの2つの記録が1行に入る幅
        private static readonly Vector2 TitleBestSize = new Vector2(1100f, 70f);
        private static readonly Vector2 TitleSubMenuPosition = new Vector2(0f, 90f);
        private static readonly Vector2 TitleSubButtonSize = new Vector2(340f, 100f);
        private const float TitleSubSpacing = 30f;

        private const float TitleSnowFieldHeight = 200f;
        private const float TitleWalkerY = -270f;
        private const float TitleWalkerSize = 170f;
        private const float TitleMinFlakeSize = 6f;
        private const float TitleMaxFlakeSize = 14f;

        private const int TitleLogoFontSize = 180;
        private const int TitleSubFontSize = 40;
        private const int TitleMainFontSize = 72;
        private const int TitleSubMenuFontSize = 40;
        private const int TitleBestFontSize = 36;

        private static readonly Color TitleBackColor = new Color(0.14f, 0.26f, 0.48f);
        private static readonly Color TitleLogoColor = new Color(1f, 0.95f, 0.75f);
        private static readonly Color TitleLogoEdgeColor = new Color(0.08f, 0.12f, 0.28f);
        // 黄・氷の水色・くちばしのオレンジ・雪の白。ペンギンと雪の色から取る
        private static readonly Color[] TitleLetterColors =
        {
            new Color(1f, 0.85f, 0.3f),
            new Color(0.55f, 0.85f, 1f),
            new Color(1f, 0.6f, 0.25f),
            new Color(0.95f, 0.98f, 1f),
        };
        private static readonly Color TitleSubColor = new Color(0.8f, 0.88f, 1f);
        private static readonly Color TitleBestFillColor = new Color(0.1f, 0.16f, 0.32f);
        private static readonly Color TitleSnowFieldColor = new Color(0.86f, 0.92f, 1f);
        private static readonly Color TitleFlakeColor = new Color(1f, 1f, 1f, 0.85f);
        // ボタンの絵は面が白より少し暗い（ButtonArt）ので、そのぶん明るめの色を掛ける
        private static readonly Color StoryColor = new Color(1f, 0.72f, 0.25f);
        private static readonly Color SurvivalColor = new Color(1f, 0.42f, 0.38f);
        private static readonly Color OnlineColor = new Color(0.3f, 0.72f, 0.5f);
        private static readonly Color MenuColor = new Color(0.45f, 0.6f, 0.85f);

        private static TitlePanel CreateTitlePanel(Transform parent, SubMenuPanel[] subMenus)
        {
            var back = CreateImage(parent, "TitlePanel", TitleBackColor);
            UIDialogBuilder.SetStretchAll(back.rectTransform);

            // 後ろから順に重なる: 雪原 → 雪 → 歩くペンモン → ロゴ・メニュー
            // 雪景色は飾りなので画面の端まで、ロゴとメニューはセーフエリアの内側に置く
            CreateSnowField(back.transform);
            CreateTitleSnow(back.transform);
            CreateTitleWalkers(back.transform);
            var content = CreateSafeContent(back.transform);
            CreateTitleLogo(content);
            var mascots = CreateTitleMascots(content);

            var mainRow = CreateCenteredRow(content, "MainMenu", Center, TitleMainMenuPosition, TitleMainButtonSize.y, TitleMainSpacing);
            var story = CreateTitleMenuButton(mainRow, StoryMenu, StoryColor, TitleMainButtonSize, TitleMainFontSize);
            var survival = CreateTitleMenuButton(mainRow, SurvivalMenu, SurvivalColor, TitleMainButtonSize, TitleMainFontSize);
            var online = CreateTitleMenuButton(mainRow, OnlineMenu, OnlineColor, TitleMainButtonSize, TitleMainFontSize);
            var best = CreateTitleBest(content);

            var subRow = CreateCenteredRow(content, "SubMenu", new Vector2(0.5f, 0f), TitleSubMenuPosition, TitleSubButtonSize.y, TitleSubSpacing);
            var subMenuButtons = new Button[SubMenuLabels.Length];
            for (int i = 0; i < subMenuButtons.Length; i++)
            {
                subMenuButtons[i] = CreateTitleMenuButton(subRow, SubMenuLabels[i], MenuColor, TitleSubButtonSize, TitleSubMenuFontSize);
            }

            var panel = back.gameObject.AddComponent<TitlePanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_storyButton").objectReferenceValue = story;
            so.FindProperty("_survivalButton").objectReferenceValue = survival;
            so.FindProperty("_onlineButton").objectReferenceValue = online;
            SetArray(so.FindProperty("_subMenuButtons"), subMenuButtons);
            SetArray(so.FindProperty("_subMenus"), subMenus);
            SetArray(so.FindProperty("_mascots"), mascots);
            so.FindProperty("_bestText").objectReferenceValue = best;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        private static void CreateTitleLogo(Transform parent)
        {
            var top = new Vector2(0.5f, 1f);
            var logo = UIDialogBuilder.CreateUIObject("Logo", parent);
            Place(logo.GetComponent<RectTransform>(), top, TitleLogoPosition, TitleLogoSize);

            // 1文字ずつ別の Text にして、色・傾き・動きを文字ごとに変える（1つの Text だと全体でしか動かせない）
            var letters = new RectTransform[TitleLogoLetters.Length];
            float firstX = -(letters.Length - 1) / 2f * TitleLetterSpacing;
            for (int i = 0; i < letters.Length; i++)
            {
                var letter = CreateTitleLetter(logo.transform, i);
                Place(letter.rectTransform, Center, new Vector2(firstX + i * TitleLetterSpacing, 0f), TitleLetterSize);
                // 交互に左右へ傾けて、手書きの看板のようなはずみを出す
                letter.rectTransform.localEulerAngles = new Vector3(0f, 0f, i % 2 == 0 ? TitleLetterTilt : -TitleLetterTilt);
                letters[i] = letter.rectTransform;
            }

            var animation = logo.AddComponent<TitleLogo>();
            var so = new SerializedObject(animation);
            SetArray(so.FindProperty("_letters"), letters);
            so.ApplyModifiedProperties();

            var sub = CreateText(parent, "Sub", "ペンギンだけの モンスターバトル", TitleSubFontSize, TitleSubColor, TextAnchor.MiddleCenter);
            Place(sub.rectTransform, top, TitleSubPosition, TitleSubSize);
        }

        private static Text CreateTitleLetter(Transform logo, int index)
        {
            var letter = CreateText(logo, $"Letter{index}", TitleLogoLetters[index], TitleLogoFontSize, TitleLetterColors[index], TextAnchor.MiddleCenter);
            letter.fontStyle = FontStyle.Bold;
            // 白の縁 → 紺の太い縁 → 下に影、の順に重ねる。後の効果は前の縁ごと縁取るので、外側ほど太くなる
            AddOutline(letter, Color.white, TitleLetterInnerEdge);
            AddOutline(letter, TitleLogoEdgeColor, TitleLetterOuterEdge);
            var shadow = letter.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TitleLogoEdgeColor;
            shadow.effectDistance = TitleLogoShadow;
            return letter;
        }

        private static void AddOutline(Text text, Color color, Vector2 distance)
        {
            var outline = text.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = distance;
        }

        /// <summary>ロゴの左右に立つ飾りのペンモン（絵は実行時にはめる）</summary>
        private static Image[] CreateTitleMascots(Transform parent)
        {
            var mascots = new Image[TitleMascotCount];
            for (int i = 0; i < mascots.Length; i++)
            {
                float side = i == 0 ? -1f : 1f;
                mascots[i] = CreateImage(parent, $"Mascot{i}", Color.white);
                mascots[i].preserveAspect = true;
                mascots[i].raycastTarget = false;
                Place(mascots[i].rectTransform, new Vector2(0.5f, 1f), new Vector2(side * TitleMascotX, TitleMascotY), Vector2.one * TitleMascotSize);
            }
            return mascots;
        }

        /// <summary>最高連勝数のバッジ。文面は実行時に入れる</summary>
        private static Text CreateTitleBest(Transform parent)
        {
            var badge = CreateFramedBox(parent, "BestBadge", TitleBestFillColor, out var inner);
            badge.raycastTarget = false;
            Place(badge.rectTransform, Center, TitleBestPosition, TitleBestSize);
            var best = CreateText(inner, "Best", "", TitleBestFontSize, TitleLogoColor, TextAnchor.MiddleCenter);
            // 連勝数の桁が増えても折り返さず、1行のまま縮める（TextFit）
            best.horizontalOverflow = HorizontalWrapMode.Overflow;
            return best;
        }

        private static void CreateSnowField(Transform parent)
        {
            var field = CreateImage(parent, "SnowField", TitleSnowFieldColor);
            field.raycastTarget = false;
            UIDialogBuilder.SetStretchAll(field.rectTransform);
            field.rectTransform.anchorMax = new Vector2(1f, 0f);
            field.rectTransform.offsetMax = new Vector2(0f, TitleSnowFieldHeight);
        }

        private static void CreateTitleSnow(Transform parent)
        {
            var area = UIDialogBuilder.CreateUIObject("Snow", parent);
            UIDialogBuilder.SetStretchAll(area.GetComponent<RectTransform>());
            // 粒が毎フレーム動くので、別の Canvas に分けて、メニューまで作り直しにならないようにする
            area.AddComponent<Canvas>();

            var flakes = new RectTransform[TitleSnowCount];
            for (int i = 0; i < flakes.Length; i++)
            {
                var flake = CreateImage(area.transform, $"Flake{i}", TitleFlakeColor);
                flake.raycastTarget = false;
                // 大きさは生成時に決めて焼き込む（位置は実行時にばらまく）
                float size = Mathf.Lerp(TitleMinFlakeSize, TitleMaxFlakeSize, i / (float)(flakes.Length - 1));
                Place(flake.rectTransform, Vector2.zero, Vector2.zero, Vector2.one * size);
                flakes[i] = flake.rectTransform;
            }

            var snow = area.AddComponent<TitleSnow>();
            var so = new SerializedObject(snow);
            SetArray(so.FindProperty("_flakes"), flakes);
            so.ApplyModifiedProperties();
        }

        private static void CreateTitleWalkers(Transform parent)
        {
            for (int i = 0; i < TitleWalkerCount; i++)
            {
                var image = CreateImage(parent, $"Walker{i}", Color.white);
                image.preserveAspect = true;
                image.raycastTarget = false;
                Place(image.rectTransform, Center, new Vector2(0f, TitleWalkerY), Vector2.one * TitleWalkerSize);

                var walker = image.gameObject.AddComponent<TitleWalker>();
                var so = new SerializedObject(walker);
                so.FindProperty("_image").objectReferenceValue = image;
                so.ApplyModifiedProperties();
            }
        }

        private static Button CreateTitleMenuButton(Transform menu, string label, Color color, Vector2 size, int fontSize)
        {
            var obj = CreateButton(menu, $"Btn_{label}", label, size, color, fontSize: fontSize);
            return obj.GetComponent<Button>();
        }
    }
}
