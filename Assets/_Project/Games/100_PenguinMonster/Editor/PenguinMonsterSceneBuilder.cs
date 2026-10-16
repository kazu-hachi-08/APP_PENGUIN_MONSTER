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
    /// タイトルは .Title.cs、ドラフトは .Draft.cs、持ち物は .Item.cs、「やめる」は .Quit.cs、試合前の登場は .Intro.cs、見せ合い・選出は .Select.cs、
    /// タイトルから開く画面（タイプ相性表・あそびかた・設定）は .Menus.cs、ずかんは .Dex.cs、
    /// 結果は .Result.cs、オンラインの部屋は .Room.cs、画面をまたいでそろえる見た目（夜空・見出し・ボタン）は .Style.cs、
    /// ストーリーのフィールド（マップ・主人公・十字キー）は .StoryField.cs、会話窓・はい / いいえ・最初の3体を選ぶ画面は .StoryEvents.cs、
    /// ストーリーのバトルのコマンドは .StoryBattle.cs、
    /// 画面の暗転は .Fade.cs
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Games/100_PenguinMonster/Scenes/" + SceneNames.PenguinMonster + ".unity";

        // 横画面の基準解像度。共通基盤のダイアログと同じ値にそろえる
        private static readonly Vector2 ReferenceResolution = new Vector2(1920f, 1080f);
        private static readonly Color BackgroundColor = new Color(0.80f, 0.90f, 0.95f);
        private static readonly Vector2 BattleCountdownPosition = new Vector2(0f, -50f);
        private static readonly Vector2 TopCenter = new Vector2(0.5f, 1f);

        [MenuItem("Tools/PenguinMonster/Rebuild")]
        public static void Rebuild()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var camera = CreateCamera();
            CreateEventSystem();
            var canvas = CreateCanvas();
            var battle = BuildBattleUI(canvas.transform);
            var fieldInput = CreateFieldControls(canvas.transform);
            // メニューは十字キーより手前、会話の層より奥（「やめる」の はい / いいえ を会話窓で聞くため）
            var fieldMenu = CreateFieldMenu(canvas.transform, fieldInput);
            // 会話の層は十字キーより手前（会話の間は十字キーを隠すが、窓のタップを取られないように）
            var fieldEvents = CreateFieldEvents(canvas.transform, fieldInput);
            var flow = BuildFlow(canvas.transform, battle);
            // 暗転は全部の画面より手前に重ねる
            var fade = CreateFade(canvas.transform);
            var field = BuildStoryField(camera, fieldInput, fade, fieldEvents, battle);
            LinkStory(flow, field, fade);
            // 図鑑はタイトルのもの（Flow の層）をそのまま開く
            LinkFieldMenu(fieldMenu, fieldEvents, flow.GetComponentInChildren<DexPanel>(true), field);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AddToBuildSettings();
            Debug.Log($"[PenguinMonsterSceneBuilder] {ScenePath} を生成しました");
        }

        private static Camera CreateCamera()
        {
            var cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            var camera = cameraObj.AddComponent<Camera>();
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = BackgroundColor;
            cameraObj.AddComponent<AudioListener>();
            return camera;
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
            // 比率が違う画面（20:9 のスマホ・4:3 のタブレット）でも基準の 1920x1080 が必ず全部見えるよう、足りない側を広げる。
            // 縦横の中間で合わせると、横長のスマホで高さが 966 まで縮み、上下の枠がはみ出したため
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasObj.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static BattleController BuildBattleUI(Transform canvas)
        {
            var root = UIDialogBuilder.CreateUIObject("Battle", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());

            // 一番奥のタップ領域 → 場 → 下の窓 → 交代の画面 → ランクの一覧、の順に重ねる
            var fieldTapArea = CreateFieldTapArea(root.transform);
            BuildField(root.transform, out var playerView, out var enemyView, out var playerInfo, out var enemyInfo);
            BuildDoubleField(root.transform, out var playerViews, out var enemyViews, out var playerInfos, out var enemyInfos);
            // 天候とエフェクトはペンモンと名前の枠より手前、下の窓より奥
            var weather = CreateWeatherLayer(root.transform);
            var moveEffect = CreateMoveEffectLayer(root.transform);
            var eggThrow = CreateEggThrowLayer(root.transform);
            // 特性の帯はエフェクトより手前（矢印の粒に隠れないように）、下の窓より奥
            var playerBanner = CreateAbilityBanner(root.transform, "PlayerAbilityBanner", PlayerBannerPosition, fromLeft: true);
            var enemyBanner = CreateAbilityBanner(root.transform, "EnemyAbilityBanner", EnemyBannerPosition, fromLeft: false);
            var messageWindow = CreateMessageWindow(root.transform, fieldTapArea);
            var commandPanel = CreateCommandPanel(root.transform);
            var storyCommandPanel = CreateStoryCommandPanel(root.transform);
            var movePanel = CreateMovePanel(root.transform);
            var targetPanel = CreateTargetPanel(root.transform);
            var bagPanel = CreateBagPanel(root.transform);
            var partyPanel = CreatePartyPanel(root.transform);
            var levelUpWindow = CreateLevelUpWindow(root.transform);
            // 一覧は枠をタップしたときだけ出る。暗幕で場と窓を覆うよう、窓より手前に置く
            var stagePanel = CreateStatStagePanel(root.transform);
            LinkStatStagePanel(stagePanel, playerInfos.Concat(enemyInfos).Append(playerInfo).Append(enemyInfo).ToArray());
            // 交代の画面の上にも見えるよう、最後に重ねる
            var turnTimer = CreateCountdown(root.transform, TopCenter, BattleCountdownPosition);

            var controller = root.AddComponent<BattleController>();
            var so = new SerializedObject(controller);
            so.FindProperty("_playerView").objectReferenceValue = playerView;
            so.FindProperty("_enemyView").objectReferenceValue = enemyView;
            so.FindProperty("_playerInfo").objectReferenceValue = playerInfo;
            so.FindProperty("_enemyInfo").objectReferenceValue = enemyInfo;
            SetArray(so.FindProperty("_playerDoubleViews"), playerViews);
            SetArray(so.FindProperty("_enemyDoubleViews"), enemyViews);
            SetArray(so.FindProperty("_playerDoubleInfos"), playerInfos);
            SetArray(so.FindProperty("_enemyDoubleInfos"), enemyInfos);
            so.FindProperty("_moveEffect").objectReferenceValue = moveEffect;
            so.FindProperty("_weather").objectReferenceValue = weather;
            so.FindProperty("_playerBanner").objectReferenceValue = playerBanner;
            so.FindProperty("_enemyBanner").objectReferenceValue = enemyBanner;
            so.FindProperty("_eggThrow").objectReferenceValue = eggThrow;
            so.FindProperty("_messageWindow").objectReferenceValue = messageWindow;
            so.FindProperty("_commandPanel").objectReferenceValue = commandPanel;
            so.FindProperty("_storyCommandPanel").objectReferenceValue = storyCommandPanel;
            so.FindProperty("_movePanel").objectReferenceValue = movePanel;
            so.FindProperty("_targetPanel").objectReferenceValue = targetPanel;
            so.FindProperty("_partyPanel").objectReferenceValue = partyPanel;
            so.FindProperty("_bagPanel").objectReferenceValue = bagPanel;
            so.FindProperty("_levelUpWindow").objectReferenceValue = levelUpWindow;
            so.FindProperty("_turnTimer").objectReferenceValue = turnTimer;
            so.ApplyModifiedProperties();
            return controller;
        }

        /// <summary>
        /// バトルの上に重ねる画面（結果 → 選出 → 登場 → 持ち物 → ドラフト → シングル / ダブルス → やめる → タイトルから開く画面 → タイトルの順に手前）と、流れの進行役。
        /// 結果は共通基盤の ResultDialog を見た目だけ作り直して使う。「もう一度遊ぶ」は勝ち抜き戦の「次の 戦いへ」に使う
        /// </summary>
        private static GameFlow BuildFlow(Transform canvas, BattleController battle)
        {
            var root = UIDialogBuilder.CreateUIObject("Flow", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());

            var resultDialog = CreateResultPanel(root.transform, out var resultBanner);
            var selectPanel = CreateSelectPanel(root.transform);
            var introPanel = CreateIntroPanel(root.transform);
            var itemPanel = CreateItemPanel(root.transform);
            var draftPanel = CreateDraftPanel(root.transform);
            var formatPanel = CreateFormatPanel(root.transform);
            var quitButton = CreateQuitButton(root.transform);
            var subMenus = CreateSubMenus(root.transform);
            var titlePanel = CreateTitlePanel(root.transform, subMenus);
            var session = new GameObject("OnlineSession").AddComponent<OnlineSession>();
            var modeSelect = CreateRoomPanel(canvas, session);

            var flow = root.AddComponent<GameFlow>();
            var so = new SerializedObject(flow);
            so.FindProperty("_titlePanel").objectReferenceValue = titlePanel;
            so.FindProperty("_formatPanel").objectReferenceValue = formatPanel;
            so.FindProperty("_draftPanel").objectReferenceValue = draftPanel;
            so.FindProperty("_itemPanel").objectReferenceValue = itemPanel;
            so.FindProperty("_introPanel").objectReferenceValue = introPanel;
            so.FindProperty("_selectPanel").objectReferenceValue = selectPanel;
            so.FindProperty("_battle").objectReferenceValue = battle;
            so.FindProperty("_resultDialog").objectReferenceValue = resultDialog;
            so.FindProperty("_resultBanner").objectReferenceValue = resultBanner;
            so.FindProperty("_quitButton").objectReferenceValue = quitButton;
            so.FindProperty("_session").objectReferenceValue = session;
            so.FindProperty("_modeSelect").objectReferenceValue = modeSelect;
            so.ApplyModifiedProperties();
            return flow;
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
