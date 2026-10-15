using PenguinMonster.Common.Editor;
using PenguinMonster.Common.Scene;
using UnityEditor;
using UnityEngine;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 画面の暗転（共通基盤の FadeController）。ストーリーの出入口・タイトルとの行き来で使う。
    /// 透明な間はタップを通し、暗い間は FadeController がタップを止める
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static FadeController CreateFade(Transform canvas)
        {
            var image = CreateImage(canvas, "Fade", Color.black);
            UIDialogBuilder.SetStretchAll(image.rectTransform);
            var group = image.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;

            var fade = image.gameObject.AddComponent<FadeController>();
            var so = new SerializedObject(fade);
            so.FindProperty("_canvasGroup").objectReferenceValue = group;
            so.FindProperty("_fadeImage").objectReferenceValue = image;
            so.ApplyModifiedProperties();
            return fade;
        }
    }
}
