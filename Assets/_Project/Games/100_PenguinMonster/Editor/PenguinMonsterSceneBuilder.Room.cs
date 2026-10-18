using PenguinMonster.Common.Editor;
using PenguinMonster.Common.Online;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// オンライン対戦の部屋を作る / コードで参加の画面。
    /// 接続の流れは共通基盤の ModeSelectPanel をそのまま使い、見た目だけゲーム側で作る（共通の箱だとタイトルとつながらないため）。
    /// タイトルと同じ夜空・雪原・歩くペンモンの上に、左に「部屋を 作る」、右に「コードで 参加」の枠。
    /// 接続を待つ間は大きな枠に参加コードを出し、下の雪原ではペンモンが歩き続ける
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // CPU 戦はタイトルの「勝ち抜き戦」から入るので、ModeSelectPanel の CPU 戦ボタンは作って隠す（参照が無いと動かないため）
        private const string OfflineLabel = "CPUと対戦";
        private const string RoomTitle = "オンライン対戦";
        private const string RoomHostLabel = "部屋を 作る\n<size=34>参加コードを 相手に 送る</size>";
        private const string RoomJoinCaption = "コードで 参加";
        private const string RoomJoinLabel = "参加する";
        private const string RoomCodePlaceholder = "コードを入力";
        private const string RoomCopyLabel = "コードをコピー";
        private const string RoomCancelLabel = "キャンセル";
        // Relay の参加コードは英数字で、これより長くはならない
        private const int RoomCodeLimit = 12;

        private static readonly Vector2 RoomTitlePosition = new Vector2(0f, -150f);
        private static readonly Vector2 RoomTitleSize = new Vector2(1400f, 160f);
        private static readonly Vector2 RoomCardSize = new Vector2(640f, 340f);
        private static readonly Vector2 RoomHostPosition = new Vector2(-370f, -10f);
        private static readonly Vector2 RoomJoinPosition = new Vector2(370f, -10f);
        private static readonly Vector2 RoomJoinCaptionPosition = new Vector2(0f, -55f);
        private static readonly Vector2 RoomJoinCaptionSize = new Vector2(600f, 80f);
        // 枠の中は上から 見出し → 入力欄 → 「参加する」。重ならないよう入力欄は少し上に寄せる
        private static readonly Vector2 RoomCodeInputPosition = new Vector2(0f, 15f);
        private static readonly Vector2 RoomCodeInputSize = new Vector2(520f, 100f);
        private const float RoomCodeInputPadding = 16f;
        private static readonly Vector2 RoomJoinButtonPosition = new Vector2(0f, 65f);
        private static readonly Vector2 RoomJoinButtonSize = new Vector2(360f, 90f);
        private static readonly Vector2 RoomStatusPosition = new Vector2(0f, 30f);
        private static readonly Vector2 RoomStatusSize = new Vector2(1100f, 400f);
        private static readonly Vector2 RoomWaitButtonsPosition = new Vector2(0f, -270f);
        private static readonly Vector2 RoomCopySize = new Vector2(420f, 110f);
        private static readonly Vector2 RoomCancelSize = new Vector2(360f, 110f);
        private const float RoomWaitButtonsSpacing = 60f;

        private const int RoomTitleFontSize = 100;
        private const int RoomHostFontSize = 64;
        private const int RoomJoinCaptionFontSize = 52;
        private const int RoomCodeFontSize = 52;
        private const int RoomStatusFontSize = 48;

        private static readonly Color RoomJoinColor = new Color(1f, 0.75f, 0.3f);
        private static readonly Color RoomPlaceholderColor = new Color(0.55f, 0.55f, 0.6f);

        private static ModeSelectPanel CreateRoomPanel(Transform canvas, OnlineSession session)
        {
            var content = CreateWalkerScreen(canvas, "ModeSelectPanel", RoomTitle, RoomTitleFontSize, RoomTitlePosition, RoomTitleSize, out var back);

            var menu = CreateRoomMenu(content, out var host, out var join, out var codeInput, out var toTitle, out var npc);
            var waiting = CreateRoomWaiting(content, out var status, out var copy, out var cancel);

            var panel = back.gameObject.AddComponent<ModeSelectPanel>();
            var so = new SerializedObject(panel);
            so.FindProperty("_session").objectReferenceValue = session;
            so.FindProperty("_menuGroup").objectReferenceValue = menu;
            so.FindProperty("_npcButton").objectReferenceValue = npc;
            so.FindProperty("_hostButton").objectReferenceValue = host;
            so.FindProperty("_joinButton").objectReferenceValue = join;
            so.FindProperty("_codeInput").objectReferenceValue = codeInput;
            so.FindProperty("_titleButton").objectReferenceValue = toTitle;
            so.FindProperty("_waitingGroup").objectReferenceValue = waiting;
            so.FindProperty("_statusText").objectReferenceValue = status;
            so.FindProperty("_cancelButton").objectReferenceValue = cancel;
            so.FindProperty("_copyButton").objectReferenceValue = copy;
            so.FindProperty("_copyButtonLabel").objectReferenceValue = copy.GetComponentInChildren<Text>();
            so.ApplyModifiedProperties();

            back.gameObject.SetActive(false);
            return panel;
        }

        /// <summary>左に「部屋を 作る」の大きなボタン、右に「コードで 参加」の枠（入力欄と「参加する」）、左上に「戻る」</summary>
        private static GameObject CreateRoomMenu(Transform parent, out Button host, out Button join, out InputField codeInput,
            out Button toTitle, out Button npc)
        {
            var menu = UIDialogBuilder.CreateUIObject("Menu", parent);
            UIDialogBuilder.SetStretchAll(menu.GetComponent<RectTransform>());

            var hostObj = CreateButton(menu.transform, "Btn_Host", RoomHostLabel, RoomCardSize, OnlineColor, fontSize: RoomHostFontSize);
            Place(hostObj.GetComponent<RectTransform>(), Center, RoomHostPosition, RoomCardSize);
            host = hostObj.GetComponent<Button>();

            var box = CreateFramedBox(menu.transform, "JoinBox", TitleBestFillColor, out var inner);
            Place(box.rectTransform, Center, RoomJoinPosition, RoomCardSize);
            var caption = CreateText(inner, "Caption", RoomJoinCaption, RoomJoinCaptionFontSize, Color.white, TextAnchor.MiddleCenter);
            caption.fontStyle = FontStyle.Bold;
            Place(caption.rectTransform, new Vector2(0.5f, 1f), RoomJoinCaptionPosition, RoomJoinCaptionSize);
            codeInput = CreateRoomCodeInput(inner);
            var joinObj = CreateButton(inner, "Btn_Join", RoomJoinLabel, RoomJoinButtonSize, RoomJoinColor);
            Place(joinObj.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), RoomJoinButtonPosition, RoomJoinButtonSize);
            join = joinObj.GetComponent<Button>();

            var titleObj = CreateButton(menu.transform, "Btn_Title", "戻る", BackButtonSize, BackColor);
            Place(titleObj.GetComponent<RectTransform>(), new Vector2(0f, 1f), FormatBackPosition, BackButtonSize);
            toTitle = titleObj.GetComponent<Button>();

            var npcObj = CreateButton(menu.transform, "Btn_Npc", OfflineLabel, BackButtonSize, BackColor);
            npcObj.SetActive(false);
            npc = npcObj.GetComponent<Button>();
            return menu;
        }

        /// <summary>接続待ち。まん中の枠に状態と参加コード（文面は ModeSelectPanel が入れる）、下に「コードをコピー」「キャンセル」</summary>
        private static GameObject CreateRoomWaiting(Transform parent, out Text status, out Button copy, out Button cancel)
        {
            var waiting = UIDialogBuilder.CreateUIObject("Waiting", parent);
            UIDialogBuilder.SetStretchAll(waiting.GetComponent<RectTransform>());

            var box = CreateFramedBox(waiting.transform, "StatusBox", TitleBestFillColor, out var inner);
            box.raycastTarget = false;
            Place(box.rectTransform, Center, RoomStatusPosition, RoomStatusSize);
            status = CreateText(inner, "StatusText", "", RoomStatusFontSize, TitleLogoColor, TextAnchor.MiddleCenter);
            status.fontStyle = FontStyle.Bold;

            var row = CreateCenteredRow(waiting.transform, "Buttons", Center, RoomWaitButtonsPosition, RoomCopySize.y, RoomWaitButtonsSpacing);
            var copyObj = CreateButton(row, "Btn_Copy", RoomCopyLabel, RoomCopySize, OnlineColor);
            copyObj.SetActive(false);
            copy = copyObj.GetComponent<Button>();
            cancel = CreateButton(row, "Btn_Cancel", RoomCancelLabel, RoomCancelSize, BackColor).GetComponent<Button>();

            waiting.SetActive(false);
            return waiting;
        }

        /// <summary>参加コードの入力欄。白い枠に大きな文字で、スマホでも押しやすくする</summary>
        private static InputField CreateRoomCodeInput(Transform parent)
        {
            var frame = CreateFramedBox(parent, "CodeInput", Color.white, out var inner);
            Place(frame.rectTransform, Center, RoomCodeInputPosition, RoomCodeInputSize);

            var placeholder = CreateText(inner, "Placeholder", RoomCodePlaceholder, RoomCodeFontSize, RoomPlaceholderColor, TextAnchor.MiddleCenter);
            var text = CreateText(inner, "Text", "", RoomCodeFontSize, BoxTextColor, TextAnchor.MiddleCenter);
            text.fontStyle = FontStyle.Bold;
            text.supportRichText = false;
            foreach (var label in new[] { placeholder, text })
            {
                label.rectTransform.offsetMin = new Vector2(RoomCodeInputPadding, 0f);
                label.rectTransform.offsetMax = new Vector2(-RoomCodeInputPadding, 0f);
            }

            var input = frame.gameObject.AddComponent<InputField>();
            input.targetGraphic = frame;
            input.textComponent = text;
            input.placeholder = placeholder;
            input.characterValidation = InputField.CharacterValidation.Alphanumeric;
            input.characterLimit = RoomCodeLimit;
            return input;
        }
    }
}
