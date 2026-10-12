using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技が当たったときのエフェクト。技ごとの型（MoveStyleTable）か特別な技の段（.Signatures.cs）から何を出すかを決め（.Stages.cs）、
    /// 粒の動き方は型ごとのレシピ（.Recipes.cs）で決め、動かし方はここで共通にする。
    /// ペンモンの絵は台座のマスクの中にあるので、エフェクトは場全体に重ねたこの層に出す
    /// </summary>
    public partial class MoveEffectView : MonoBehaviour
    {
        /// <summary>同時に出せる粒の数。SceneBuilder がこの数だけ Image を作る</summary>
        public const int ParticleCount = 10;

        // ペンモンの絵は 64 ドット。粒も同じドットの大きさで描き、絵と並べても浮かないようにする
        private const float MonsterPixels = 64f;
        // 寿命の最後のこの割合で薄くして消す
        private const float FadeRatio = 0.3f;

        [SerializeField] private Image[] _particles;

        private RectTransform Layer => (RectTransform)transform;

        private void Awake()
        {
            HideAll();
        }

        /// <param name="target">受けた側の絵。大きさ（シングル / ダブルス、手前 / 奥）に合わせて粒も大きくする</param>
        /// <param name="attacker">使った側の絵。風や結晶の飛んでくる向きに使う</param>
        public IEnumerator Play(MoveDefinition move, RectTransform target, RectTransform attacker)
        {
            var center = CenterOf(target);
            var toward = attacker == null ? Vector2.left : (center - CenterOf(attacker)).normalized;
            float unit = target.rect.height;
            foreach (var stage in StagesOf(move, toward))
            {
                yield return PlayStage(stage, center, unit);
            }
        }

        private IEnumerator PlayStage(Stage stage, Vector2 center, float unit)
        {
            float duration = Setup(stage, unit);
            for (float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
            {
                Step(stage.Particles, center, unit, elapsed);
                yield return null;
            }
            HideAll();
        }

        /// <summary>粒に絵と大きさを入れ、全部が消えるまでの秒数を返す</summary>
        private float Setup(Stage stage, float unit)
        {
            var particles = stage.Particles;
            var size = stage.Sprite.rect.size * (unit / MonsterPixels);
            float duration = 0f;
            for (int i = 0; i < _particles.Length; i++)
            {
                var image = _particles[i];
                image.enabled = false;
                if (i >= particles.Count) continue;

                image.sprite = stage.Sprite;
                image.color = particles[i].Tint;
                image.rectTransform.sizeDelta = size;
                duration = Mathf.Max(duration, particles[i].Delay + particles[i].Life);
            }
            return duration;
        }

        private void Step(List<Particle> particles, Vector2 center, float unit, float elapsed)
        {
            for (int i = 0; i < particles.Count && i < _particles.Length; i++)
            {
                var p = particles[i];
                var image = _particles[i];
                float t = (elapsed - p.Delay) / p.Life;
                image.enabled = t >= 0f && t <= 1f;
                if (!image.enabled) continue;

                var rect = image.rectTransform;
                rect.localPosition = center + p.PositionAt(t) * unit;
                rect.localScale = Vector3.one * p.ScaleAt(t);
                rect.localRotation = Quaternion.Euler(0f, 0f, p.Rotation + p.Spin * t);
                var color = p.Tint;
                color.a = t < 1f - FadeRatio ? 1f : (1f - t) / FadeRatio;
                image.color = color;
            }
        }

        /// <summary>絵の真ん中を、この層の座標で返す（絵の原点は足元なので、高さの半分だけ上）</summary>
        private Vector2 CenterOf(RectTransform body)
        {
            return Layer.InverseTransformPoint(body.TransformPoint(body.rect.center));
        }

        private void HideAll()
        {
            foreach (var image in _particles)
            {
                image.enabled = false;
            }
        }
    }
}
