using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 場の天候の層（仕様書 §4.10）。場全体に重ねる色と、降らせる粒。色・速さは WeatherView の SerializeField で調整する
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private const int WeatherParticleCount = 60;

        private static WeatherView CreateWeatherLayer(Transform parent)
        {
            var layer = UIDialogBuilder.CreateUIObject("Weather", parent);
            UIDialogBuilder.SetStretchAll(layer.GetComponent<RectTransform>());
            // 粒が毎フレーム動くので、別の Canvas に分けて、場の他の部品まで作り直しにならないようにする
            layer.AddComponent<Canvas>();

            var tint = CreateImage(layer.transform, "Tint", Color.clear);
            UIDialogBuilder.SetStretchAll(tint.rectTransform);
            tint.raycastTarget = false;

            var particles = new Image[WeatherParticleCount];
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = CreateImage(layer.transform, $"Particle{i}", Color.white);
                particles[i].raycastTarget = false;
                // 大きさと位置は天候ごとに実行時に決める
                Place(particles[i].rectTransform, Vector2.zero, Vector2.zero, Vector2.one);
            }

            var view = layer.AddComponent<WeatherView>();
            var so = new SerializedObject(view);
            so.FindProperty("_tint").objectReferenceValue = tint;
            SetArray(so.FindProperty("_particles"), particles);
            so.ApplyModifiedProperties();
            return view;
        }
    }
}
