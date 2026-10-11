using System.Linq;
using PenguinMonster.Common.Editor;
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
    /// 各パネルの組み立ては PenguinMonsterSceneBuilder.Panels.cs
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Games/100_PenguinMonster/Scenes/" + SceneNames.PenguinMonster + ".unity";

        // 横画面の基準解像度。共通基盤のダイアログと同じ値にそろえる
        private static readonly Vector2 ReferenceResolution = new Vector2(1920f, 1080f);
        private const float MatchWidthOrHeight = 0.5f;
        private static readonly Color BackgroundColor = new Color(0.80f, 0.90f, 0.95f);

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
            BuildBattleUI(canvas.transform);

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

        private static void BuildBattleUI(Transform canvas)
        {
            var root = UIDialogBuilder.CreateUIObject("Battle", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());

            var enemyPanel = CreateMonsterPanel(root.transform, "EnemyPanel", EnemyPanelPosition, isPlayer: false);
            var playerPanel = CreateMonsterPanel(root.transform, "PlayerPanel", PlayerPanelPosition, isPlayer: true);
            var messageWindow = CreateMessageWindow(root.transform);
            var commandPanel = CreateCommandPanel(root.transform);
            var partyPanel = CreatePartyPanel(root.transform);
            var retryButton = CreateRetryButton(root.transform);

            var controller = root.AddComponent<BattleController>();
            var so = new SerializedObject(controller);
            so.FindProperty("_playerPanel").objectReferenceValue = playerPanel;
            so.FindProperty("_enemyPanel").objectReferenceValue = enemyPanel;
            so.FindProperty("_messageWindow").objectReferenceValue = messageWindow;
            so.FindProperty("_commandPanel").objectReferenceValue = commandPanel;
            so.FindProperty("_partyPanel").objectReferenceValue = partyPanel;
            so.FindProperty("_retryButton").objectReferenceValue = retryButton;
            so.ApplyModifiedProperties();
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
