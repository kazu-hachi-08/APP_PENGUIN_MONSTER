using System.Collections.Generic;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 型ごとの粒の動き方。位置は「受けた側の絵の高さを 1」とした、絵の真ん中からの距離で書く
    /// （シングル / ダブルス、手前 / 奥で絵の大きさが違っても同じ見た目にするため）。
    /// 乱数は見た目のゆらぎだけに使うので、オンラインで2台の見た目がずれても勝敗には関係しない
    /// </summary>
    public partial class MoveEffectView
    {
        private struct Particle
        {
            public Vector2 From;
            public Vector2 To;
            public float Delay;
            public float Life;
            /// <summary>途中で上へ膨らむ高さ。水しぶきを山なりに飛ばす</summary>
            public float Arc;
            /// <summary>だんだん速くする。落ちてくる岩・稲妻</summary>
            public bool Accelerate;
            /// <summary>寿命のあいだに中心のまわりを回る角度。苔の葉を渦に巻かせる</summary>
            public float Orbit;
            public float StartScale;
            public float EndScale;
            /// <summary>大きくなってから小さくなる。オーロラのきらめき</summary>
            public bool Pulse;
            public float Rotation;
            public float Spin;
            public Color Tint;

            public Vector2 PositionAt(float t)
            {
                var position = Vector2.Lerp(From, To, Accelerate ? t * t : t);
                if (Orbit != 0f)
                {
                    position = Rotate(position, Orbit * t);
                }
                position.y += Arc * 4f * t * (1f - t);
                return position;
            }

            public float ScaleAt(float t)
            {
                float scale = Mathf.Lerp(StartScale, EndScale, t);
                return Pulse ? scale * Mathf.Sin(t * Mathf.PI) : scale;
            }
        }

        /// <param name="toward">使った側から受けた側への向き</param>
        private static List<Particle> Recipe(MoveStyle style, Vector2 toward)
        {
            var list = new List<Particle>();
            switch (style)
            {
                case MoveStyle.Pierce: Pierce(list, toward); break;
                case MoveStyle.Slash: Claws(list); break;
                case MoveStyle.Bite: Bite(list); break;
                case MoveStyle.Converge: Shards(list); break;
                case MoveStyle.Blow: Gust(list, toward); break;
                case MoveStyle.Shot: Shot(list, toward); break;
                case MoveStyle.Fall: Rockfall(list); break;
                case MoveStyle.Plunge: Strike(list); break;
                case MoveStyle.Rise: Flames(list); break;
                case MoveStyle.Splash: Splash(list); break;
                case MoveStyle.Swirl: LeafSwirl(list); break;
                case MoveStyle.Twinkle: Sparkles(list); break;
                default: Impact(list); break;
            }
            return list;
        }

        private static void Impact(List<Particle> list)
        {
            for (int i = 0; i < 3; i++)
            {
                var at = Random.insideUnitCircle * 0.2f;
                var p = Move(at, at, i * 0.08f, 0.22f, 0.6f);
                p.EndScale = 2.4f;
                list.Add(p);
            }
        }

        private static void Shards(List<Particle> list)
        {
            const int count = 6;
            for (int i = 0; i < count; i++)
            {
                float angle = i * 360f / count + Random.Range(-15f, 15f);
                var dir = Direction(angle);
                var p = Move(dir * 0.65f, dir * 0.05f, i * 0.04f, 0.25f, 1.3f);
                // 結晶などは先が上（90度）を向いた絵なので、先を中心（angle + 180度）へ向ける
                p.Rotation = angle + 90f;
                list.Add(p);
            }
        }

        private static void Splash(List<Particle> list)
        {
            for (int i = 0; i < 8; i++)
            {
                var to = new Vector2(Random.Range(-0.55f, 0.55f), -0.35f);
                var p = Move(new Vector2(0f, -0.1f), to, Random.Range(0f, 0.1f), 0.45f, 1f);
                p.EndScale = 0.7f;
                p.Arc = Random.Range(0.35f, 0.55f);
                list.Add(p);
            }
        }

        private static void Flames(List<Particle> list)
        {
            const int count = 7;
            for (int i = 0; i < count; i++)
            {
                float x = Mathf.Lerp(-0.4f, 0.4f, i / (count - 1f));
                var to = new Vector2(x + Random.Range(-0.08f, 0.08f), 0.35f);
                var p = Move(new Vector2(x, -0.4f), to, Random.Range(0f, 0.15f), 0.4f, 1.6f);
                p.EndScale = 0.6f;
                list.Add(p);
            }
        }

        private static void Strike(List<Particle> list)
        {
            const float landing = 0.12f;
            var hitPoint = new Vector2(0f, 0.1f);
            var fall = Move(new Vector2(0f, 1.1f), hitPoint, 0f, landing, 3f);
            fall.Accelerate = true;
            list.Add(fall);

            var stay = Move(hitPoint, hitPoint, landing, 0.25f, 3f);
            stay.EndScale = 3.4f;
            list.Add(stay);

            for (int i = 0; i < 4; i++)
            {
                var spark = Move(Vector2.zero, Direction(45f + i * 90f) * 0.45f, landing, 0.25f, 0.8f);
                spark.EndScale = 0.4f;
                list.Add(spark);
            }
        }

        private static void LeafSwirl(List<Particle> list)
        {
            const int count = 6;
            for (int i = 0; i < count; i++)
            {
                var dir = Direction(i * 360f / count);
                var p = Move(dir * 0.6f, dir * 0.05f, i * 0.03f, 0.5f, 1.2f);
                p.Orbit = 270f;
                p.Spin = 360f;
                list.Add(p);
            }
        }

        private static void Rockfall(List<Particle> list)
        {
            for (int i = 0; i < 4; i++)
            {
                float x = Random.Range(-0.3f, 0.3f);
                var p = Move(new Vector2(x, 1.2f), new Vector2(x * 0.5f, 0f), i * 0.08f, 0.3f, Random.Range(1.4f, 2f));
                p.Accelerate = true;
                p.Spin = Random.Range(-90f, 90f);
                list.Add(p);
            }
        }

        private static void Gust(List<Particle> list, Vector2 toward)
        {
            const int count = 6;
            var across = new Vector2(-toward.y, toward.x);
            for (int i = 0; i < count; i++)
            {
                var offset = across * Mathf.Lerp(-0.35f, 0.35f, i / (count - 1f));
                var p = Move(-toward * 0.8f + offset, toward * 0.8f + offset, Random.Range(0f, 0.15f), 0.35f, 1.2f);
                p.Arc = 0.1f;
                p.Rotation = Random.Range(0f, 360f);
                p.Spin = 180f;
                list.Add(p);
            }
        }

        private static void Claws(List<Particle> list)
        {
            var stroke = new Vector2(0.15f, -0.15f);
            for (int i = 0; i < 3; i++)
            {
                var at = new Vector2(0.15f, 0.15f) * (i - 1);
                var p = Move(at - stroke, at + stroke, i * 0.07f, 0.25f, 1.6f);
                p.EndScale = 2f;
                list.Add(p);
            }
        }

        private static void Sparkles(List<Particle> list)
        {
            for (int i = 0; i < 8; i++)
            {
                var at = Random.insideUnitCircle * 0.45f;
                var p = Move(at, at, Random.Range(0f, 0.3f), 0.3f, 1.4f);
                p.Pulse = true;
                list.Add(p);
            }
        }

        private static void Pierce(List<Particle> list, Vector2 toward)
        {
            var across = new Vector2(-toward.y, toward.x);
            for (int i = 0; i < 3; i++)
            {
                var offset = across * ((i - 1) * 0.12f);
                var p = Move(-toward * 0.7f + offset, toward * 0.05f + offset, i * 0.09f, 0.16f, 1.4f);
                p.Accelerate = true;
                // 先が上を向いた絵なので、先を飛ぶ向きへ向ける
                p.Rotation = AngleOf(toward) - 90f;
                list.Add(p);
            }
        }

        private static void Bite(List<Particle> list)
        {
            const float gap = 0.06f;
            for (int i = 0; i < 2; i++)
            {
                float delay = i * 0.22f;
                var upper = Move(new Vector2(0f, 0.45f), new Vector2(0f, gap), delay, 0.2f, 2.2f);
                upper.Accelerate = true;
                list.Add(upper);

                var lower = Move(new Vector2(0f, -0.45f), new Vector2(0f, -gap), delay, 0.2f, 2.2f);
                lower.Accelerate = true;
                lower.Rotation = 180f;
                list.Add(lower);
            }
        }

        private static void Shot(List<Particle> list, Vector2 toward)
        {
            const int stream = 6;
            const float travel = 0.18f;
            for (int i = 0; i < stream; i++)
            {
                var p = Move(-toward * 1.1f, Vector2.zero, i * 0.04f, travel, 1.1f);
                p.Spin = 180f;
                list.Add(p);
            }

            float burst = (stream - 1) * 0.04f + travel;
            for (int i = 0; i < 3; i++)
            {
                var p = Move(Vector2.zero, Direction(Random.Range(0f, 360f)) * 0.35f, burst, 0.22f, 1f);
                p.EndScale = 0.4f;
                list.Add(p);
            }
        }

        private static Particle Move(Vector2 from, Vector2 to, float delay, float life, float scale)
        {
            return new Particle
            {
                From = from, To = to, Delay = delay, Life = life, StartScale = scale, EndScale = scale, Tint = Color.white,
            };
        }

        private static Vector2 Direction(float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        private static float AngleOf(Vector2 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        private static Vector2 Rotate(Vector2 v, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }
    }
}
