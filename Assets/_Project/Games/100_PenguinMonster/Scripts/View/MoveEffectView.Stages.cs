using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技から「何をどの順に出すか」を決める。ふつうの技は型1つぶん（1段）、特別な技（.Signatures.cs）は何段でも続けて出せる
    /// </summary>
    public partial class MoveEffectView
    {
        // タイプの色は札用に落ち着かせてあるので、白い粒に付けると暗すぎる。少し白に寄せて場の上で見えるようにする
        private const float ShapeTintLighten = 0.35f;

        private static readonly Color[] AuroraTints =
        {
            new Color(1f, 0.6f, 0.85f),
            new Color(0.55f, 0.95f, 1f),
            new Color(0.6f, 1f, 0.65f),
            new Color(0.8f, 0.65f, 1f),
        };

        /// <summary>1段ぶんのエフェクト。1段の粒はすべて同じ絵で出す</summary>
        private struct Stage
        {
            public List<Particle> Particles;
            public Sprite Sprite;
        }

        private static List<Stage> StagesOf(MoveDefinition move, Vector2 toward)
        {
            if (Signatures.TryGetValue(move, out var signature))
            {
                return signature(toward);
            }
            return new List<Stage> { StageOf(MoveStyleTable.Get(move), move.Type, toward) };
        }

        /// <summary>型の動き方に、粒の絵と色を付ける</summary>
        private static Stage StageOf(MoveStyle style, MonsterType type, Vector2 toward)
        {
            var particles = Recipe(style, toward);
            var shape = ShapeOf(style);
            if (shape == null)
            {
                if (type == MonsterType.Aurora)
                {
                    // オーロラの粒は白で描いてあり、ここで1つずつ色を変えて虹色に見せる
                    Tint(particles, i => AuroraTints[i % AuroraTints.Length]);
                }
                return new Stage { Particles = particles, Sprite = MoveEffectSprites.Get(type) };
            }

            if (shape != EffectShape.Star)
            {
                var color = Color.Lerp(TypeColors.Get(type), Color.white, ShapeTintLighten);
                Tint(particles, _ => color);
            }
            return new Stage { Particles = particles, Sprite = MoveEffectSprites.Get(shape.Value) };
        }

        /// <summary>
        /// 形が大事な型だけ型の絵を使う（null ならタイプの絵）。星は「当たった」印として全タイプ同じ色にし、
        /// 切り傷と牙はタイプの色を付けて何の技か分かるようにする
        /// </summary>
        private static EffectShape? ShapeOf(MoveStyle style)
        {
            switch (style)
            {
                case MoveStyle.Hit: return EffectShape.Star;
                case MoveStyle.Slash: return EffectShape.Slash;
                case MoveStyle.Bite: return EffectShape.Fang;
                default: return null;
            }
        }

        private static void Tint(List<Particle> particles, System.Func<int, Color> colorAt)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                var p = particles[i];
                p.Tint = colorAt(i);
                particles[i] = p;
            }
        }

        /// <summary>段の粒をまとめて大きくする。特別な技を「いつもより派手」にするのに使う</summary>
        private static Stage Bigger(Stage stage, float factor)
        {
            for (int i = 0; i < stage.Particles.Count; i++)
            {
                var p = stage.Particles[i];
                p.StartScale *= factor;
                p.EndScale *= factor;
                stage.Particles[i] = p;
            }
            return stage;
        }
    }
}
