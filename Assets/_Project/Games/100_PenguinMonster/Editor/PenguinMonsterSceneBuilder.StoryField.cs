using PenguinMonster.Common.Editor;
using PenguinMonster.Common.Input;
using PenguinMonster.Common.Scene;
using PenguinMonster.View;
using PenguinMonster.View.Story;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ストーリーのフィールド。マップと主人公は UI ではなくカメラに映す（Tilemap と SpriteRenderer）。
    /// フィールドに居る間はバトルの UI を隠すので、Canvas（Overlay）の後ろのカメラの絵が見える。
    /// 十字キーと A / B は Canvas に置く（タッチの端末だけ出す）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // 主人公と村の人はマップのタイルより手前に描く。タイル（0）の上に、下り坂の風の線（1）、その上に主人公と村の人（2）
        private const int WindSortingOrder = 1;
        private const int HeroSortingOrder = 2;

        // 十字キーは左下、A / B は右下（ダイパの手の位置。右手の親指で A を押しやすいよう A を外側の上に置く）
        private static readonly Vector2 PadCenter = new Vector2(300f, 300f);
        private const float PadSpacing = 160f;
        private static readonly Vector2 PadButtonSize = new Vector2(150f, 150f);
        private static readonly Vector2 AButtonPosition = new Vector2(-200f, 320f);
        private static readonly Vector2 BButtonPosition = new Vector2(-420f, 200f);
        private static readonly Vector2 ActionButtonSize = new Vector2(180f, 180f);
        private const int PadFontSize = 56;
        // 景色が透けて見えるよう少し薄くする
        private static readonly Color PadColor = new Color(0.45f, 0.6f, 0.85f, 0.8f);
        private static readonly Color AButtonColor = new Color(1f, 0.42f, 0.38f, 0.8f);
        private static readonly Color BButtonColor = new Color(1f, 0.72f, 0.25f, 0.8f);
        // 走るの切り替えは十字キーの右上の空き（▲と▶の間の外側）。親指を少しずらせば押せて、歩くボタンと間違えない小ささにする
        private static readonly Vector2 RunButtonPosition = PadCenter + new Vector2(PadSpacing, PadSpacing);
        private static readonly Vector2 RunButtonSize = new Vector2(120f, 120f);
        private const int RunFontSize = 36;
        // ON は十字キーと違う色にして、走っているのがひと目でわかるようにする
        private static readonly Color RunOnColor = new Color(1f, 0.6f, 0.2f, 0.9f);
        // 自転車の切り替えは「走る」と左右対称の、十字キーの左上の空き。乗っている間は緑にする
        private static readonly Vector2 BikeButtonPosition = PadCenter + new Vector2(-PadSpacing, PadSpacing);
        private static readonly Color BikeOnColor = new Color(0.3f, 0.8f, 0.4f, 0.9f);

        private static FieldController BuildStoryField(Camera camera, FieldInput input, FadeController fade, FieldEventPlayer events,
            BattleController battle)
        {
            var root = new GameObject("StoryField", typeof(Grid));
            var mapView = CreateFieldMap(root.transform);
            var people = CreatePeople(root.transform);
            var avatar = CreateHero(root.transform);
            var fieldCamera = CreateFieldCamera(camera);
            var battles = CreateStoryBattleScreen(root, battle, fade);
            var eventsSo = new SerializedObject(events);
            eventsSo.FindProperty("_battles").objectReferenceValue = battles;
            eventsSo.ApplyModifiedProperties();

            var controller = root.AddComponent<FieldController>();
            var so = new SerializedObject(controller);
            so.FindProperty("_mapView").objectReferenceValue = mapView;
            so.FindProperty("_avatar").objectReferenceValue = avatar;
            so.FindProperty("_peopleView").objectReferenceValue = people;
            so.FindProperty("_events").objectReferenceValue = events;
            so.FindProperty("_camera").objectReferenceValue = fieldCamera;
            so.FindProperty("_input").objectReferenceValue = input;
            so.FindProperty("_fade").objectReferenceValue = fade;
            so.FindProperty("_battles").objectReferenceValue = battles;
            so.ApplyModifiedProperties();

            root.SetActive(false);
            return controller;
        }

        private static void LinkStory(GameFlow flow, FieldController field, FadeController fade)
        {
            var so = new SerializedObject(flow);
            so.FindProperty("_field").objectReferenceValue = field;
            so.FindProperty("_fade").objectReferenceValue = fade;
            so.ApplyModifiedProperties();
        }

        private static FieldMapView CreateFieldMap(Transform parent)
        {
            var tiles = new GameObject("Tiles", typeof(Tilemap), typeof(TilemapRenderer));
            tiles.transform.SetParent(parent, false);
            var view = tiles.AddComponent<FieldMapView>();
            var so = new SerializedObject(view);
            so.FindProperty("_tilemap").objectReferenceValue = tiles.GetComponent<Tilemap>();
            so.ApplyModifiedProperties();
            return view;
        }

        /// <summary>村の人の絵の置き場。絵（SpriteRenderer）はマップに立つ人数ぶん FieldPeopleView が実行時に作る</summary>
        private static FieldPeopleView CreatePeople(Transform parent)
        {
            var people = new GameObject("People");
            people.transform.SetParent(parent, false);
            var view = people.AddComponent<FieldPeopleView>();
            var so = new SerializedObject(view);
            so.FindProperty("_sortingOrder").intValue = HeroSortingOrder;
            so.ApplyModifiedProperties();
            return view;
        }

        private static FieldAvatar CreateHero(Transform parent)
        {
            var hero = new GameObject("Hero");
            hero.transform.SetParent(parent, false);
            var renderer = hero.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = HeroSortingOrder;
            var wind = new GameObject("Wind");
            wind.transform.SetParent(hero.transform, false);
            var windRenderer = wind.AddComponent<SpriteRenderer>();
            windRenderer.sortingOrder = WindSortingOrder;
            var avatar = hero.AddComponent<FieldAvatar>();
            var so = new SerializedObject(avatar);
            so.FindProperty("_renderer").objectReferenceValue = renderer;
            so.FindProperty("_wind").objectReferenceValue = windRenderer;
            so.ApplyModifiedProperties();
            return avatar;
        }

        /// <summary>フィールドに居る間だけ動かす（バトルなどの UI は Overlay なのでカメラの位置に左右されない）</summary>
        private static FieldCamera CreateFieldCamera(Camera camera)
        {
            var fieldCamera = camera.gameObject.AddComponent<FieldCamera>();
            var so = new SerializedObject(fieldCamera);
            so.FindProperty("_camera").objectReferenceValue = camera;
            so.ApplyModifiedProperties();
            fieldCamera.enabled = false;
            return fieldCamera;
        }

        private static FieldInput CreateFieldControls(Transform canvas)
        {
            var root = UIDialogBuilder.CreateUIObject("FieldControls", canvas);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());
            var touch = CreateSafeContent(root.transform);
            touch.name = "TouchControls";

            var input = root.AddComponent<FieldInput>();
            var so = new SerializedObject(input);
            so.FindProperty("_touchControls").objectReferenceValue = touch.gameObject;
            so.FindProperty("_up").objectReferenceValue = CreatePadButton(touch, "Up", "▲", Vector2.zero, PadCenter + Vector2.up * PadSpacing, PadButtonSize, PadColor);
            so.FindProperty("_down").objectReferenceValue = CreatePadButton(touch, "Down", "▼", Vector2.zero, PadCenter + Vector2.down * PadSpacing, PadButtonSize, PadColor);
            so.FindProperty("_left").objectReferenceValue = CreatePadButton(touch, "Left", "◀", Vector2.zero, PadCenter + Vector2.left * PadSpacing, PadButtonSize, PadColor);
            so.FindProperty("_right").objectReferenceValue = CreatePadButton(touch, "Right", "▶", Vector2.zero, PadCenter + Vector2.right * PadSpacing, PadButtonSize, PadColor);
            so.FindProperty("_a").objectReferenceValue = CreatePadButton(touch, "A", "A", Vector2.right, AButtonPosition, ActionButtonSize, AButtonColor);
            so.FindProperty("_b").objectReferenceValue = CreatePadButton(touch, "B", "B", Vector2.right, BButtonPosition, ActionButtonSize, BButtonColor);
            var run = CreatePadButton(touch, "Run", "走る", Vector2.zero, RunButtonPosition, RunButtonSize, PadColor);
            run.GetComponentInChildren<UnityEngine.UI.Text>().fontSize = RunFontSize;
            so.FindProperty("_run").objectReferenceValue = run;
            so.FindProperty("_runImage").objectReferenceValue = run.GetComponent<UnityEngine.UI.Image>();
            so.FindProperty("_runOffColor").colorValue = PadColor;
            so.FindProperty("_runOnColor").colorValue = RunOnColor;
            run.gameObject.SetActive(false);
            var bike = CreatePadButton(touch, "Bike", "自転車", Vector2.zero, BikeButtonPosition, RunButtonSize, PadColor);
            bike.GetComponentInChildren<UnityEngine.UI.Text>().fontSize = RunFontSize;
            so.FindProperty("_bike").objectReferenceValue = bike;
            so.FindProperty("_bikeImage").objectReferenceValue = bike.GetComponent<UnityEngine.UI.Image>();
            so.FindProperty("_bikeOffColor").colorValue = PadColor;
            so.FindProperty("_bikeOnColor").colorValue = BikeOnColor;
            bike.gameObject.SetActive(false);
            so.ApplyModifiedProperties();

            root.SetActive(false);
            return input;
        }

        /// <summary>
        /// 見た目と押し込みはほかの画面と同じ「ぷっくり」ボタン。押しっぱなしで歩けるよう、入力は VirtualButton で受ける
        /// </summary>
        private static VirtualButton CreatePadButton(Transform parent, string name, string label, Vector2 anchor, Vector2 position, Vector2 size, Color color)
        {
            var obj = CreateButton(parent, $"Pad_{name}", label, size, color, fontSize: PadFontSize);
            Place(obj.GetComponent<RectTransform>(), anchor, position, size);
            return obj.AddComponent<VirtualButton>();
        }
    }
}
