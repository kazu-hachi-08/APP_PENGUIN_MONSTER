using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    public static partial class PenguinMonsterSceneBuilder
    {
        // 配置（基準解像度 1920x1080、画面中央が原点）。奥右に相手、手前左に自分
        private static readonly Vector2 EnemyPanelPosition = new Vector2(380f, 260f);
        private static readonly Vector2 PlayerPanelPosition = new Vector2(-380f, -60f);
        private static readonly Vector2 MonsterPanelSize = new Vector2(820f, 300f);
        private const float BodySize = 260f;
        private static readonly Vector2 InfoBoxSize = new Vector2(500f, 170f);
        private const float HpBarHeight = 24f;

        private const float BottomAreaHeight = 300f;
        private const float MessageAreaWidthRatio = 0.5f;
        private static readonly Vector2 MoveButtonSize = new Vector2(440f, 110f);
        private static readonly Vector2 MoveButtonSpacing = new Vector2(16f, 16f);
        private static readonly Vector2 SwitchButtonSize = new Vector2(220f, 80f);

        private static readonly Vector2 PartyBoxSize = new Vector2(900f, 640f);
        private static readonly Vector2 PartySlotSize = new Vector2(760f, 120f);
        private const float PartySlotSpacing = 24f;
        private static readonly Vector2 RetryButtonSize = new Vector2(360f, 110f);

        private const int NameFontSize = 40;
        private const int HpFontSize = 30;
        private const int MessageFontSize = 44;
        private const int ButtonFontSize = 34;

        private static readonly Color BoxColor = new Color(0.97f, 0.97f, 0.92f);
        private static readonly Color BoxTextColor = new Color(0.15f, 0.15f, 0.2f);
        private static readonly Color HpBackColor = new Color(0.3f, 0.3f, 0.3f);
        private static readonly Color MessageBoxColor = new Color(0.18f, 0.22f, 0.32f);
        private static readonly Color CommandBoxColor = new Color(0.92f, 0.92f, 0.88f);
        private static readonly Color SwitchColor = new Color(0.35f, 0.55f, 0.35f);
        private static readonly Color OverlayColor = new Color(0f, 0f, 0f, 0.6f);
        private static readonly Color SlotColor = new Color(0.25f, 0.45f, 0.75f);
        private static readonly Color BackColor = new Color(0.45f, 0.45f, 0.5f);
        private static readonly Color RetryColor = new Color(0.85f, 0.45f, 0.2f);

        private static MonsterPanel CreateMonsterPanel(Transform parent, string name, Vector2 position, bool isPlayer)
        {
            var root = UIDialogBuilder.CreateUIObject(name, parent);
            Place(root.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), position, MonsterPanelSize);

            // 自分は絵が左・情報が右、相手は情報が左・絵が右（ダイパ風の向き）
            float bodyX = (MonsterPanelSize.x - BodySize) / 2f * (isPlayer ? -1f : 1f);
            float infoX = (MonsterPanelSize.x - InfoBoxSize.x) / 2f * (isPlayer ? 1f : -1f);

            var body = CreateImage(root.transform, "Body", Color.white);
            Place(body.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(bodyX, 0f), new Vector2(BodySize, BodySize));

            var info = CreateImage(root.transform, "Info", BoxColor);
            Place(info.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(infoX, 0f), InfoBoxSize);
            var nameText = CreateText(info.transform, "Name", "", NameFontSize, BoxTextColor, TextAnchor.UpperLeft);
            SetBand(nameText.rectTransform, 0.6f, 1f);
            var hpText = CreateText(info.transform, "HpText", "", HpFontSize, BoxTextColor, TextAnchor.LowerRight);
            SetBand(hpText.rectTransform, 0f, 0.35f);

            var hpBack = CreateImage(info.transform, "HpBar", HpBackColor);
            var hpBackRect = hpBack.rectTransform;
            hpBackRect.anchorMin = new Vector2(0.05f, 0.45f);
            hpBackRect.anchorMax = new Vector2(0.95f, 0.45f);
            hpBackRect.sizeDelta = new Vector2(0f, HpBarHeight);
            var hpFill = CreateImage(hpBack.transform, "Fill", Color.green);
            UIDialogBuilder.SetStretchAll(hpFill.rectTransform);

            var panel = root.AddComponent<MonsterPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_body").objectReferenceValue = body;
            so.FindProperty("_nameText").objectReferenceValue = nameText;
            so.FindProperty("_hpText").objectReferenceValue = hpText;
            so.FindProperty("_hpFill").objectReferenceValue = hpFill.rectTransform;
            so.FindProperty("_hpFillImage").objectReferenceValue = hpFill;
            so.ApplyModifiedProperties();
            return panel;
        }

        private static MessageWindow CreateMessageWindow(Transform parent)
        {
            var box = CreateImage(parent, "MessageWindow", MessageBoxColor);
            var rect = box.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(MessageAreaWidthRatio, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.sizeDelta = new Vector2(0f, BottomAreaHeight);

            // 窓全体をボタンにして、どこをタップしても次の文へ送れるようにする
            var button = box.gameObject.AddComponent<Button>();
            var text = CreateText(box.transform, "Text", "", MessageFontSize, Color.white, TextAnchor.MiddleLeft);
            text.rectTransform.offsetMin = new Vector2(48f, 24f);
            text.rectTransform.offsetMax = new Vector2(-48f, -24f);

            var window = box.gameObject.AddComponent<MessageWindow>();
            var so = new SerializedObject(window);
            so.FindProperty("_text").objectReferenceValue = text;
            so.FindProperty("_advanceButton").objectReferenceValue = button;
            so.ApplyModifiedProperties();
            return window;
        }

        private static CommandPanel CreateCommandPanel(Transform parent)
        {
            var box = CreateImage(parent, "CommandPanel", CommandBoxColor);
            var rect = box.rectTransform;
            rect.anchorMin = new Vector2(MessageAreaWidthRatio, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.sizeDelta = new Vector2(0f, BottomAreaHeight);

            var grid = UIDialogBuilder.CreateUIObject("Moves", box.transform);
            UIDialogBuilder.SetStretchAll(grid.GetComponent<RectTransform>());
            var layout = grid.AddComponent<GridLayoutGroup>();
            layout.cellSize = MoveButtonSize;
            layout.spacing = MoveButtonSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = 2;

            var buttons = new Button[4];
            var labels = new Text[4];
            for (int i = 0; i < buttons.Length; i++)
            {
                var buttonObj = CreateButton(grid.transform, $"Btn_Move{i}", "", MoveButtonSize, Color.gray);
                buttons[i] = buttonObj.GetComponent<Button>();
                labels[i] = buttonObj.GetComponentInChildren<Text>();
                labels[i].color = BoxTextColor;
            }

            // 交代ボタンはコマンド窓の右上に乗せる
            var switchObj = CreateButton(box.transform, "Btn_Switch", "こうたい", SwitchButtonSize, SwitchColor);
            var switchRect = switchObj.GetComponent<RectTransform>();
            switchRect.anchorMin = switchRect.anchorMax = new Vector2(1f, 1f);
            switchRect.pivot = new Vector2(1f, 0f);
            switchRect.anchoredPosition = new Vector2(-16f, 16f);

            var panel = box.gameObject.AddComponent<CommandPanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_moveButtons"), buttons);
            SetArray(so.FindProperty("_moveLabels"), labels);
            so.FindProperty("_switchButton").objectReferenceValue = switchObj.GetComponent<Button>();
            so.ApplyModifiedProperties();
            return panel;
        }

        private static PartyPanel CreatePartyPanel(Transform parent)
        {
            var overlay = CreateImage(parent, "PartyPanel", OverlayColor);
            UIDialogBuilder.SetStretchAll(overlay.rectTransform);

            var box = CreateImage(overlay.transform, "Box", MessageBoxColor);
            Place(box.rectTransform, new Vector2(0.5f, 0.5f), Vector2.zero, PartyBoxSize);
            var layout = box.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = PartySlotSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            var buttons = new Button[BattleBalance.PartySize];
            var labels = new Text[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                var slot = CreateButton(box.transform, $"Btn_Slot{i}", "", PartySlotSize, SlotColor);
                buttons[i] = slot.GetComponent<Button>();
                labels[i] = slot.GetComponentInChildren<Text>();
            }
            var back = CreateButton(box.transform, "Btn_Back", "もどる", SwitchButtonSize, BackColor);

            var panel = overlay.gameObject.AddComponent<PartyPanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_slotButtons"), buttons);
            SetArray(so.FindProperty("_slotLabels"), labels);
            so.FindProperty("_backButton").objectReferenceValue = back.GetComponent<Button>();
            so.ApplyModifiedProperties();
            return panel;
        }

        private static Button CreateRetryButton(Transform parent)
        {
            var obj = CreateButton(parent, "Btn_Retry", "もういちど", RetryButtonSize, RetryColor);
            Place(obj.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0f, 120f), RetryButtonSize);
            return obj.GetComponent<Button>();
        }
    }
}
