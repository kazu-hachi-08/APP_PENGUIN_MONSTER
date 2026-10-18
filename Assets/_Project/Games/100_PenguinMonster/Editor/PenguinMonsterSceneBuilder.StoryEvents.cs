using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View.Story;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// フィールドの出来事の層。奥から タップ領域 → 最初の3体を選ぶ画面・よろず屋・あずかり箱 → 会話窓 → はい / いいえ。
    /// 会話窓はバトルと同じ作り（CreateMessageWindow）。選ぶ画面の上に会話窓が重なるよう、選ぶ画面を奥に置く（「〇〇に しますか？」を画面の上で聞くため）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string StarterTitleText = "タマゴを 1つ 選んでください";
        private const string StarterDecideText = "この子に 決めた！";

        // はい / いいえ は会話窓の右上に重ねる（ダイパと同じ）
        private static readonly Vector2 YesNoSize = new Vector2(380f, 280f);
        private const float YesNoRightMargin = 40f;
        private const float YesNoGap = 12f;
        private static readonly Vector2 YesNoButtonSize = new Vector2(240f, 100f);
        private const float YesNoRowOffset = 60f;
        private const float YesNoButtonX = 40f;
        private const float YesNoCursorX = -125f;
        private static readonly Vector2 YesNoCursorSize = new Vector2(60f, 100f);
        private static readonly Color YesColor = new Color(0.35f, 0.65f, 0.4f);

        private static FieldEventPlayer CreateFieldEvents(Transform canvas, FieldInput input)
        {
            var root = UIDialogBuilder.CreateUIObject("FieldEvents", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());

            // 窓の外をタップしても送れるよう、一番奥に敷く（バトルと同じ）
            var tapArea = CreateFieldTapArea(root.transform);
            var starters = CreateStarterPanel(root.transform, input);
            var shop = CreateShopPanel(root.transform);
            var box = CreateBoxPanel(root.transform);
            // 会話の文字の音はフィールドの窓だけ（お店・あずかり箱もこの窓を使う）
            var window = CreateMessageWindow(root.transform, tapArea, typingSound: true);
            LinkStoryScreenWindow(window, shop, box);
            var yesNo = CreateYesNoWindow(root.transform, input);

            var player = root.AddComponent<FieldEventPlayer>();
            var so = new SerializedObject(player);
            so.FindProperty("_window").objectReferenceValue = window;
            so.FindProperty("_yesNo").objectReferenceValue = yesNo;
            so.FindProperty("_starters").objectReferenceValue = starters;
            so.FindProperty("_shop").objectReferenceValue = shop;
            so.FindProperty("_box").objectReferenceValue = box;
            so.FindProperty("_input").objectReferenceValue = input;
            so.ApplyModifiedProperties();

            root.SetActive(false);
            return player;
        }

        private static YesNoWindow CreateYesNoWindow(Transform parent, FieldInput input)
        {
            CreateBottomRightWindow(parent, "YesNo", "YesNoWindow", new Vector2(-YesNoRightMargin, BottomAreaHeight + YesNoGap), YesNoSize,
                out var safe, out var inner);

            var yes = CreateYesNoRow(inner, "Yes", "はい", YesNoRowOffset, YesColor, out var yesCursor);
            var no = CreateYesNoRow(inner, "No", "いいえ", -YesNoRowOffset, BackColor, out var noCursor);

            var window = safe.gameObject.AddComponent<YesNoWindow>();
            var so = new SerializedObject(window);
            so.FindProperty("_yes").objectReferenceValue = yes;
            so.FindProperty("_no").objectReferenceValue = no;
            SetArray(so.FindProperty("_cursors"), new Object[] { yesCursor, noCursor });
            so.FindProperty("_input").objectReferenceValue = input;
            so.ApplyModifiedProperties();

            safe.gameObject.SetActive(false);
            return window;
        }

        /// <summary>ボタン1つと、選んでいるときに左に出す ▶</summary>
        private static Button CreateYesNoRow(Transform inner, string name, string label, float y, Color color, out GameObject cursor)
        {
            var button = CreateButton(inner, $"Btn_{name}", label, YesNoButtonSize, color);
            Place(button.GetComponent<RectTransform>(), Center, new Vector2(YesNoButtonX, y), YesNoButtonSize);
            var mark = CreateText(inner, $"Cursor_{name}", "▶", MessageFontSize, BoxTextColor, TextAnchor.MiddleCenter);
            Place(mark.rectTransform, Center, new Vector2(YesNoCursorX, y), YesNoCursorSize);
            cursor = mark.gameObject;
            return button.GetComponent<Button>();
        }

        /// <summary>ドラフトの画面と同じ形。左にタマゴのカード3枚、右に中身の詳細と「この子に 決めた！」</summary>
        private static StarterPanel CreateStarterPanel(Transform parent, FieldInput input)
        {
            var back = CreateNightBack(parent, "StarterPanel");
            var safe = CreateSafeContent(back.transform);
            var title = CreateDraftTitle(safe);
            title.text = StarterTitleText;

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
            decide.GetComponentInChildren<Text>().text = StarterDecideText;

            var panel = back.gameObject.AddComponent<StarterPanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_cardButtons"), buttons);
            SetArray(so.FindProperty("_cardIcons"), icons);
            SetArray(so.FindProperty("_cardNames"), names);
            SetArray(so.FindProperty("_cardOutlines"), outlines);
            SetArray(so.FindProperty("_typeBadges"), badges);
            SetArray(so.FindProperty("_typeLabels"), badgeLabels);
            so.FindProperty("_detail").objectReferenceValue = detail;
            so.FindProperty("_decideButton").objectReferenceValue = decide;
            so.FindProperty("_input").objectReferenceValue = input;
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }
    }
}
