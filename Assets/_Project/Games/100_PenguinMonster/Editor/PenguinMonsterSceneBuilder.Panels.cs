using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 下側の窓（メッセージ・コマンド・技選択・ダブルスの狙う相手）と、交代の画面・ランクの一覧
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 StageBoxSize = new Vector2(620f, 620f);
        private const int StageTitleFontSize = 40;
        private const int StageRowFontSize = 38;
        private const int StageHintFontSize = 26;
        // 箱の中を上から 見出し / 7行 / 「タップで 閉じる」 に分ける割合
        private const float StageRowsTop = 0.83f;
        private const float StageRowsBottom = 0.13f;
        // 行の中を 能力名 / ▲の並び / 数字 の列に分ける割合
        private const float StagePipsLeft = 0.36f;
        private const float StageValuesLeft = 0.8f;
        private static readonly Color StageHintColor = new Color(0.5f, 0.5f, 0.55f);

        private const float BottomAreaHeight = 300f;
        // 窓は画面の端から端まであるので、書き始めがスマホのカメラ穴に掛からないよう左右を広く空ける
        private const float MessagePaddingX = 100f;
        private const float MessagePaddingY = 36f;
        private static readonly Vector2 NextIndicatorSize = new Vector2(60f, 60f);

        private const float CommandPanelWidth = 760f;
        private static readonly Vector2 FightButtonSize = new Vector2(400f, 220f);
        private static readonly Vector2 PartyButtonSize = new Vector2(260f, 220f);
        private const float CommandSpacing = 24f;

        private static readonly Vector2 MoveButtonSize = new Vector2(660f, 118f);
        private static readonly Vector2 MoveButtonSpacing = new Vector2(20f, 16f);
        private const float MoveBackWidth = 260f;
        private static readonly Vector2 MoveBackSize = new Vector2(220f, 220f);
        private static readonly Vector2 MoveHintSize = new Vector2(1100f, 200f);
        private const float MoveHintGap = 12f;
        private const float MoveHintPadding = 28f;
        private const int MoveHintFontSize = 34;

        private static readonly Vector2 TargetButtonSize = new Vector2(620f, 220f);
        private const float TargetSpacing = 32f;

        // ダブルスの4体が入る大きさ（シングルは余った枠を隠す）
        private static readonly Vector2 PartyBoxSize = new Vector2(900f, 800f);
        private static readonly Vector2 PartySlotSize = new Vector2(760f, 130f);
        private const float PartySlotSpacing = 20f;
        private const float PartyIconSize = 112f;
        private static readonly Vector2 BackButtonSize = new Vector2(220f, 80f);

        private const int MessageFontSize = 46;
        private const int ButtonFontSize = 36;
        private const int CommandFontSize = 52;

        private static readonly Color FrameColor = new Color(0.2f, 0.24f, 0.36f);
        private static readonly Color InfoBoxColor = new Color(0.97f, 0.97f, 0.9f);
        private static readonly Color MessageBoxColor = new Color(0.98f, 0.98f, 0.98f);
        private static readonly Color BoxTextColor = new Color(0.2f, 0.2f, 0.25f);
        private static readonly Color NextIndicatorColor = new Color(0.85f, 0.3f, 0.25f);
        private static readonly Color FightColor = new Color(0.88f, 0.32f, 0.3f);
        private static readonly Color PartyColor = new Color(0.35f, 0.65f, 0.4f);
        private static readonly Color OverlayColor = new Color(0f, 0f, 0f, 0.6f);
        private static readonly Color SlotColor = new Color(0.3f, 0.5f, 0.8f);
        private static readonly Color BackColor = new Color(0.45f, 0.45f, 0.5f);
        /// <summary>技ボタンの統一背景色。タイプ色で塗らず、落ち着いた濃紺にする</summary>
        private static readonly Color MoveButtonColor = new Color(0.22f, 0.26f, 0.38f);

        /// <summary>
        /// 画面全体に透明なボタンを敷く。一番奥に置くので、窓やボタンのない所をタップしたときだけ反応する
        /// </summary>
        private static Button CreateFieldTapArea(Transform parent)
        {
            var image = CreateImage(parent, "FieldTapArea", Color.clear);
            UIDialogBuilder.SetStretchAll(image.rectTransform);
            return image.gameObject.AddComponent<Button>();
        }

        private static MessageWindow CreateMessageWindow(Transform parent, Button fieldTapArea)
        {
            var frame = CreateFramedBox(parent, "MessageWindow", MessageBoxColor, out var inner);
            SetBottomArea(frame.rectTransform, 0f, 1f);

            // 窓全体もボタンにして、どこをタップしても次の文へ送れるようにする
            var button = frame.gameObject.AddComponent<Button>();
            var text = CreateText(inner, "Text", "", MessageFontSize, BoxTextColor, TextAnchor.UpperLeft);
            text.rectTransform.offsetMin = new Vector2(MessagePaddingX, MessagePaddingY);
            text.rectTransform.offsetMax = new Vector2(-MessagePaddingX, -MessagePaddingY);

            var indicator = CreateText(inner, "Next", "▼", MessageFontSize, NextIndicatorColor, TextAnchor.MiddleCenter);
            var indicatorRect = indicator.rectTransform;
            indicatorRect.anchorMin = indicatorRect.anchorMax = new Vector2(1f, 0f);
            indicatorRect.pivot = new Vector2(1f, 0f);
            indicatorRect.sizeDelta = NextIndicatorSize;
            indicatorRect.anchoredPosition = new Vector2(-MessagePaddingX / 2f, MessagePaddingY / 2f);

            var window = frame.gameObject.AddComponent<MessageWindow>();
            var so = new SerializedObject(window);
            so.FindProperty("_text").objectReferenceValue = text;
            SetArray(so.FindProperty("_advanceButtons"), new Object[] { button, fieldTapArea });
            so.FindProperty("_nextIndicator").objectReferenceValue = indicator.gameObject;
            so.ApplyModifiedProperties();
            return window;
        }

        /// <summary>メッセージ窓の右側に重ねる。左には「〇〇は どうする？」が見えたまま残る</summary>
        private static CommandPanel CreateCommandPanel(Transform parent)
        {
            var frame = CreateFramedBox(parent, "CommandPanel", InfoBoxColor, out var inner);
            var rect = frame.rectTransform;
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(CommandPanelWidth, BottomAreaHeight);

            var layout = inner.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = CommandSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            var fight = CreateButton(inner, "Btn_Fight", "戦う", FightButtonSize, FightColor);
            var party = CreateButton(inner, "Btn_Party", "ペンモン", PartyButtonSize, PartyColor);
            fight.GetComponentInChildren<Text>().fontSize = CommandFontSize;

            var panel = frame.gameObject.AddComponent<CommandPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_fightButton").objectReferenceValue = fight.GetComponent<Button>();
            so.FindProperty("_partyButton").objectReferenceValue = party.GetComponent<Button>();
            so.ApplyModifiedProperties();
            return panel;
        }

        /// <summary>技選択は下の窓いっぱいに出す。左に技4つ（2x2）、右に「もどる」</summary>
        private static MovePanel CreateMovePanel(Transform parent)
        {
            var frame = CreateFramedBox(parent, "MovePanel", InfoBoxColor, out var inner);
            SetBottomArea(frame.rectTransform, 0f, 1f);

            var grid = UIDialogBuilder.CreateUIObject("Moves", inner);
            var gridRect = grid.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(gridRect);
            gridRect.offsetMax = new Vector2(-MoveBackWidth, 0f);
            var layout = grid.AddComponent<GridLayoutGroup>();
            layout.cellSize = MoveButtonSize;
            layout.spacing = MoveButtonSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = 2;

            var buttons = new Button[4];
            var labels = new Text[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                var buttonObj = CreateButton(grid.transform, $"Btn_Move{i}", "", MoveButtonSize, MoveButtonColor);
                buttons[i] = buttonObj.GetComponent<Button>();
                labels[i] = buttonObj.GetComponentInChildren<Text>();
                labels[i].color = Color.white;
                buttonObj.AddComponent<MoveHoldHint>();
            }

            var back = CreateButton(inner, "Btn_Back", "戻る", MoveBackSize, BackColor);
            var backRect = back.GetComponent<RectTransform>();
            backRect.anchorMin = backRect.anchorMax = new Vector2(1f, 0.5f);
            backRect.pivot = new Vector2(1f, 0.5f);
            backRect.anchoredPosition = new Vector2(-(MoveBackWidth - MoveBackSize.x) / 2f, 0f);

            var hintBox = CreateMoveHintBox(frame.transform, out var hintText);

            var panel = frame.gameObject.AddComponent<MovePanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_moveButtons"), buttons);
            SetArray(so.FindProperty("_moveLabels"), labels);
            so.FindProperty("_backButton").objectReferenceValue = back.GetComponent<Button>();
            so.FindProperty("_hintBox").objectReferenceValue = hintBox;
            so.FindProperty("_hintText").objectReferenceValue = hintText;
            so.ApplyModifiedProperties();
            return panel;
        }

        /// <summary>技の説明（長押しで出す）。指で隠れないよう、技選択の窓のすぐ上の左に置く</summary>
        private static GameObject CreateMoveHintBox(Transform panel, out Text text)
        {
            var frame = CreateFramedBox(panel, "MoveHint", MessageBoxColor, out var inner);
            frame.raycastTarget = false;
            var rect = frame.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 0f);
            rect.anchoredPosition = new Vector2(0f, MoveHintGap);
            rect.sizeDelta = MoveHintSize;

            text = CreateText(inner, "Text", "", MoveHintFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            text.rectTransform.offsetMin = new Vector2(MoveHintPadding, 0f);
            text.rectTransform.offsetMax = new Vector2(-MoveHintPadding, 0f);
            frame.gameObject.SetActive(false);
            return frame.gameObject;
        }

        private static PartyPanel CreatePartyPanel(Transform parent)
        {
            var overlay = CreateImage(parent, "PartyPanel", OverlayColor);
            UIDialogBuilder.SetStretchAll(overlay.rectTransform);

            var frame = CreateFramedBox(overlay.transform, "Box", InfoBoxColor, out var inner);
            Place(frame.rectTransform, Center, Vector2.zero, PartyBoxSize);
            var layout = inner.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = PartySlotSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            var buttons = new Button[BattleBalance.DoublesPartySize];
            var labels = new Text[buttons.Length];
            var icons = new Image[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                var slot = CreateButton(inner, $"Btn_Slot{i}", "", PartySlotSize, SlotColor);
                buttons[i] = slot.GetComponent<Button>();
                labels[i] = slot.GetComponentInChildren<Text>();
                // 左に絵を置くぶん、名前は右へずらす
                labels[i].rectTransform.offsetMin = new Vector2(PartyIconSize + 16f, 0f);
                // 長い名前＋HP＋状態異常は1行に入り切らないことがあるので、折り返さずに縮める（TextFit）
                labels[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                icons[i] = CreatePartyIcon(slot.transform);
            }
            var back = CreateButton(inner, "Btn_Back", "戻る", BackButtonSize, BackColor);

            var panel = overlay.gameObject.AddComponent<PartyPanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_slotButtons"), buttons);
            SetArray(so.FindProperty("_slotLabels"), labels);
            SetArray(so.FindProperty("_slotIcons"), icons);
            so.FindProperty("_backButton").objectReferenceValue = back.GetComponent<Button>();
            so.ApplyModifiedProperties();
            return panel;
        }

        /// <summary>ダブルスの狙う相手。下の窓いっぱいに、相手の場の左右と同じ並びでボタン2つと「戻る」</summary>
        private static TargetPanel CreateTargetPanel(Transform parent)
        {
            var frame = CreateFramedBox(parent, "TargetPanel", InfoBoxColor, out var inner);
            SetBottomArea(frame.rectTransform, 0f, 1f);
            var layout = inner.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = TargetSpacing;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            var buttons = new Button[BattleBalance.DoublesActiveCount];
            var labels = new Text[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                var buttonObj = CreateButton(inner, $"Btn_Target{i}", "", TargetButtonSize, SlotColor);
                buttons[i] = buttonObj.GetComponent<Button>();
                labels[i] = buttonObj.GetComponentInChildren<Text>();
                labels[i].supportRichText = true;
            }
            var back = CreateButton(inner, "Btn_Back", "戻る", MoveBackSize, BackColor);

            var panel = frame.gameObject.AddComponent<TargetPanel>();
            var so = new SerializedObject(panel);
            SetArray(so.FindProperty("_targetButtons"), buttons);
            SetArray(so.FindProperty("_targetLabels"), labels);
            so.FindProperty("_backButton").objectReferenceValue = back.GetComponent<Button>();
            so.ApplyModifiedProperties();
            frame.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>名前の枠をタップして開くランクの一覧。暗幕ごとボタンにして、どこをタップしても閉じる</summary>
        private static StatStagePanel CreateStatStagePanel(Transform parent)
        {
            var overlay = CreateImage(parent, "StatStagePanel", OverlayColor);
            UIDialogBuilder.SetStretchAll(overlay.rectTransform);
            var close = overlay.gameObject.AddComponent<Button>();
            close.transition = Selectable.Transition.None;

            var frame = CreateFramedBox(overlay.transform, "Box", InfoBoxColor, out var inner);
            Place(frame.rectTransform, Center, Vector2.zero, StageBoxSize);
            // 箱の上をタップしても閉じられるよう、箱はタップを受けない
            frame.raycastTarget = false;

            var title = CreateText(inner, "Title", "", StageTitleFontSize, BoxTextColor, TextAnchor.MiddleCenter);
            title.fontStyle = FontStyle.Bold;
            title.horizontalOverflow = HorizontalWrapMode.Overflow;
            SetBand(title.rectTransform, StageRowsTop, 1f);
            var names = CreateStageColumn(inner, "Names", 0f, StagePipsLeft, TextAnchor.MiddleLeft);
            var pips = CreateStageColumn(inner, "Pips", StagePipsLeft, StageValuesLeft, TextAnchor.MiddleLeft);
            var values = CreateStageColumn(inner, "Values", StageValuesLeft, 1f, TextAnchor.MiddleRight);
            var hint = CreateText(inner, "Hint", "タップで 閉じる", StageHintFontSize, StageHintColor, TextAnchor.MiddleCenter);
            SetBand(hint.rectTransform, 0f, StageRowsBottom);

            var panel = overlay.gameObject.AddComponent<StatStagePanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_closeButton").objectReferenceValue = close;
            so.FindProperty("_titleText").objectReferenceValue = title;
            so.FindProperty("_namesText").objectReferenceValue = names;
            so.FindProperty("_pipsText").objectReferenceValue = pips;
            so.FindProperty("_valuesText").objectReferenceValue = values;
            so.ApplyModifiedProperties();
            overlay.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>一覧の1列（7行ぶんを改行でつなげて入れる）。列どうしの行の高さがそろうよう、同じ大きさの字で上下中央に置く</summary>
        private static Text CreateStageColumn(RectTransform inner, string name, float left, float right, TextAnchor alignment)
        {
            var text = CreateText(inner, name, "", StageRowFontSize, BoxTextColor, alignment);
            text.supportRichText = true;
            var rect = text.rectTransform;
            rect.anchorMin = new Vector2(left, StageRowsBottom);
            rect.anchorMax = new Vector2(right, StageRowsTop);
            rect.offsetMin = new Vector2(left == 0f ? InfoPadding : 0f, 0f);
            rect.offsetMax = new Vector2(right == 1f ? -InfoPadding : 0f, 0f);
            return text;
        }

        /// <summary>全部の名前の枠から、タップで同じ一覧を開けるようにする</summary>
        private static void LinkStatStagePanel(StatStagePanel panel, params MonsterInfoBox[] infos)
        {
            foreach (var info in infos)
            {
                var so = new SerializedObject(info);
                so.FindProperty("_stagePanel").objectReferenceValue = panel;
                so.ApplyModifiedProperties();
            }
        }

        private static Image CreatePartyIcon(Transform slot)
        {
            var icon = CreateImage(slot, "Icon", Color.white);
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            var rect = icon.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0f, 0.5f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.anchoredPosition = new Vector2(16f, 0f);
            rect.sizeDelta = new Vector2(PartyIconSize, PartyIconSize);
            return icon;
        }

        /// <summary>画面下の帯（高さ BottomAreaHeight）の横 [min, max] に置く</summary>
        private static void SetBottomArea(RectTransform rect, float min, float max)
        {
            rect.anchorMin = new Vector2(min, 0f);
            rect.anchorMax = new Vector2(max, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0f, BottomAreaHeight);
        }
    }
}
