using System.Collections;
using System.Collections.Generic;
using PenguinMonster.View.Art;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 能力が上がる・下がるときの矢印。上がると青い ▲ が足元から立ちのぼり、下がると赤い ▼ が上から流れ落ちる。
    /// 色が光るだけだと上がったのか下がったのか分かりにくいので、動く向きでも見せる。技のエフェクトと同じ粒・同じ層を使う
    /// </summary>
    public partial class MoveEffectView
    {
        // 左から何列目に出すかを、出る順に並べたもの。隣り合わない順にして、列ではなく次々に湧いて見せる
        private static readonly int[] ArrowColumns = { 0, 3, 1, 4, 2, 5 };
        private const float ArrowSpread = 0.4f;
        private const float ArrowTravel = 0.75f;
        private const float ArrowLife = 0.45f;
        private const float ArrowStagger = 0.07f;
        private const float ArrowScale = 1.4f;

        public IEnumerator PlayStatArrows(bool up, RectTransform target)
        {
            var stage = new Stage { Particles = ArrowParticles(up), Sprite = MoveEffectSprites.Get(EffectShape.Arrow) };
            yield return PlayStage(stage, CenterOf(target), target.rect.height);
        }

        private static List<Particle> ArrowParticles(bool up)
        {
            var list = new List<Particle>();
            float direction = up ? 1f : -1f;
            var tint = up ? StatusColors.StatUp : StatusColors.StatDown;
            for (int i = 0; i < ArrowColumns.Length; i++)
            {
                float x = Mathf.Lerp(-ArrowSpread, ArrowSpread, ArrowColumns[i] / (ArrowColumns.Length - 1f));
                var from = new Vector2(x, -direction * ArrowTravel / 2f);
                var p = Move(from, from + new Vector2(0f, direction * ArrowTravel), i * ArrowStagger, ArrowLife, ArrowScale);
                p.Rotation = up ? 0f : 180f;
                p.Tint = tint;
                list.Add(p);
            }
            return list;
        }
    }
}
