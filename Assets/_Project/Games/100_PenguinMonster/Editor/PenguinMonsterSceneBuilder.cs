using System.Linq;
using PenguinMonster.Common.Editor;
using PenguinMonster.Common.Online;
using PenguinMonster.Common.Scene;
using PenguinMonster.View;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ペンモンのシーンをコードから生成する。シーンは生成物なので手で編集せず、ここを直して Rebuild する。
    /// 場（背景・ペンモン・HPの枠）は .Field.cs、下の窓と交代の画面は .Panels.cs、
    /// タイトルは .Title.cs、ドラフトは .Draft.cs、試合前の登場は .Intro.cs、見せ合い・選出は .Select.cs、
    /// タイトルから開く画面（タイプ相性表・あそびかた・設定）は .Menus.cs、ずかんは .Dex.cs
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Games/100_PenguinMonster/Scenes/" + SceneNames.PenguinMonster + ".unity";

        // 横画面の基準解像度。共通基盤のダイアログと同じ値にそろえる
        private static readonly Vector2 ReferenceResolution = new Vector2(1920f, 1080f);
        private const float MatchWidthOrHeight = 0.5f;
        private static readonly Color BackgroundColor = new Color(0.80f, 0.90f, 0.95f);
        private const string NextBattleLabel = "つぎの たたかいへ";
        private const string OfflineLabel = "CPUと対戦";
        private static readonly Vector2 BattleCountdownPosition = new Vector2(0f, -50f);
        private static readonly Vector2 TopCenter = new Vector2(0.5f, 1f);
        private static readonly Vector2 HostButtonPosition = new Vector2(0f, 80f);

        [MenuItem("Tools/PenguinMonster/Rebuild")]
        public static void Rebuild()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCamera();
            CreateEventSystem();
            var canvas = CreateCanvas();
            var battle = BuildBattleUI(canvas.transform);
            BuildFlow(canvas.transform, battle);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AddToBuildSettings();
            Debug.Log($"[PenguinMonsterSceneBuilder] {ScenePath} を生成しました");
        }

        private static void CreateCamera()
        {
            var cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            var camera = cameraObj.AddComponent<Camera>();
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = BackgroundColor;
        }

        private static void CreateEventSystem()
        {
            // Input System のみの設定なので、旧 StandaloneInputModule ではなくこちらを使う
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        }

        private static Canvas CreateCanvas()
        {
            var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = ReferenceResolution;
            scaler.matchWidthOrHeight = MatchWidthOrHeight;
            canvasObj.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static BattleController BuildBattleUI(Transform canvas)
        {
            var root = UIDialogBuilder.CreateUIObject("Battle", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());

            // 一番奥のタップ領域 → 場 → 下の窓 → 交代の画面、の順に重ねる
            var fieldTapArea = CreateFieldTapArea(root.transform);
            BuildField(root.transform, out var playerView, out var enemyView, out var playerInfo, out var enemyInfo);
            var messageWindow = CreateMessageWindow(root.transform, fieldTapArea);
            var commandPanel = CreateCommandPanel(root.transform);
            var movePanel = CreateMovePanel(root.transform);
            var partyPanel = CreatePartyPanel(root.transform);
            // 交代の画面の上にも見えるよう、最後に重ねる
            var turnTimer = CreateCountdown(root.transform, TopCenter, BattleCountdownPosition);

            var controller = root.AddComponent<BattleController>();
            var so = new SerializedObject(controller);
            so.FindProperty("_playerView").objectReferenceValue = playerView;
            so.FindProperty("_enemyView").objectReferenceValue = enemyView;
            so.FindProperty("_playerInfo").objectReferenceValue = playerInfo;
            so.FindProperty("_enemyInfo").objectReferenceValue = enemyInfo;
            so.FindProperty("_messageWindow").objectReferenceValue = messageWindow;
            so.FindProperty("_commandPanel").objectReferenceValue = commandPanel;
            so.FindProperty("_movePanel").objectReferenceValue = movePanel;
            so.FindProperty("_partyPanel").objectReferenceValue = partyPanel;
            so.FindProperty("_turnTimer").objectReferenceValue = turnTimer;
            so.ApplyModifiedProperties();
            return controller;
        }

        /// <summary>
        /// バトルの上に重ねる画面（結果 → 選出 → 登場 → ドラフト → タイトルから開く画面 → タイトルの順に手前）と、流れの進行役。
        /// 結果は共通基盤の ResultDialog を使う。「もう一度遊ぶ」は勝ち抜き戦の「つぎの たたかいへ」に使う
        /// </summary>
        private static void BuildFlow(Transform canvas, BattleController battle)
        {
            var root = UIDialogBuilder.CreateUIObject("Flow", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());

            var resultDialog = UIDialogBuilder.CreateResultDialog(root.transform);
            resultDialog.transform.Find("Panel/ButtonArea/Btn_Retry").GetComponentInChildren<Text>().text = NextBattleLabel;
            var selectPanel = CreateSelectPanel(root.transform);
            var introPanel = CreateIntroPanel(root.transform);
            var draftPanel = CreateDraftPanel(root.transform);
            var subMenus = CreateSubMenus(root.transform);
            var titlePanel = CreateTitlePanel(root.transform, subMenus);
            CreateOnline(canvas, out var session, out var modeSelect);

            var flow = root.AddComponent<GameFlow>();
            var so = new SerializedObject(flow);
            so.FindProperty("_titlePanel").objectReferenceValue = titlePanel;
            so.FindProperty("_draftPanel").objectReferenceValue = draftPanel;
            so.FindProperty("_introPanel").objectReferenceValue = introPanel;
            so.FindProperty("_selectPanel").objectReferenceValue = selectPanel;
            so.FindProperty("_battle").objectReferenceValue = battle;
            so.FindProperty("_resultDialog").objectReferenceValue = resultDialog;
            so.FindProperty("_session").objectReferenceValue = session;
            so.FindProperty("_modeSelect").objectReferenceValue = modeSelect;
            so.ApplyModifiedProperties();
        }

        /// <summary>
        /// 接続（共通基盤の OnlineSession）と、部屋を作る / コードで参加の画面（ModeSelectPanel）。
        /// CPU 戦はタイトルの「勝ち抜き戦」から入るので、ModeSelectPanel の CPU 戦ボタンは隠し、部屋を作るを中央に寄せる
        /// </summary>
        private static void CreateOnline(Transform canvas, out OnlineSession session, out ModeSelectPanel modeSelect)
        {
            session = new GameObject("OnlineSession").AddComponent<OnlineSession>();
            modeSelect = ModeSelectPanelBuilder.Create(canvas, session, OfflineLabel);
            var menu = modeSelect.transform.Find("Panel/Menu");
            menu.Find("Btn_Npc").gameObject.SetActive(false);
            ((RectTransform)menu.Find("Btn_Host")).anchoredPosition = HostButtonPosition;
        }

        private static void AddToBuildSettings()
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.Any(s => s.path == ScenePath))
            {
                return;
            }
            scenes.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
