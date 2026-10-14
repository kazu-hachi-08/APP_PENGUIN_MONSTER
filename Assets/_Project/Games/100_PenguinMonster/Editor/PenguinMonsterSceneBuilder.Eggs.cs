using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 繰り出すときにタマゴを投げる層（仕様書 §7）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        /// <summary>
        /// タマゴを投げる層。画面の外から飛んでくるので、台座のマスクの外（場全体）に重ねる
        /// </summary>
        private static EggThrowView CreateEggThrowLayer(Transform parent)
        {
            var layer = UIDialogBuilder.CreateUIObject("EggThrow", parent);
            UIDialogBuilder.SetStretchAll(layer.GetComponent<RectTransform>());
            var egg = CreateImage(layer.transform, "Egg", Color.white);
            var burst = CreateImage(layer.transform, "Burst", Color.white);
            foreach (var image in new[] { egg, burst })
            {
                image.raycastTarget = false;
                image.enabled = false;
            }

            var view = layer.AddComponent<EggThrowView>();
            var so = new SerializedObject(view);
            so.FindProperty("_egg").objectReferenceValue = egg;
            so.FindProperty("_burst").objectReferenceValue = burst;
            so.ApplyModifiedProperties();
            return view;
        }
    }
}
