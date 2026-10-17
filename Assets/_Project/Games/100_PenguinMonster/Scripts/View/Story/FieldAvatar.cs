using System.Collections;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの主人公の絵。向きを変える・1マス歩く動きだけを持つ（どこへ行けるかは FieldPlayer が決める）
    /// </summary>
    public class FieldAvatar : MonoBehaviour
    {
        // 1マスの前半は足を出したコマ、ここから後は立ちのコマ（歩いてくるトレーナーも同じ）
        private const float StandFromProgress = 0.5f;

        [SerializeField] private SpriteRenderer _renderer;

        // 上下に歩くときは、歩きのコマを1歩ごとに反転して左右の足を交互に出す
        private bool _mirrorNextStep;

        public void Place(Vector3 feet, Direction facing)
        {
            transform.position = feet;
            Face(facing);
        }

        public void Face(Direction facing)
        {
            _renderer.sprite = HeroArt.Get(facing, 0);
        }

        /// <summary>前半は足を出したコマ、後半は立ちのコマで1マス進む</summary>
        public IEnumerator Walk(Vector3 to, Direction facing, float seconds)
        {
            _renderer.sprite = HeroArt.Get(facing, 1, _mirrorNextStep);
            _mirrorNextStep = !_mirrorNextStep;
            yield return Slide(transform, to, seconds, () => Face(facing));
            Place(to, facing);
        }

        /// <summary>1マス分を滑らかに動かし、半分まで来たら onHalfway を1回呼ぶ（立ちのコマへ替える）。最後は to にぴったり置く</summary>
        public static IEnumerator Slide(Transform mover, Vector3 to, float seconds, System.Action onHalfway)
        {
            var from = mover.position;
            bool passedHalf = false;
            for (float t = 0f; t < seconds; t += Time.deltaTime)
            {
                float progress = t / seconds;
                mover.position = Vector3.Lerp(from, to, progress);
                if (!passedHalf && progress >= StandFromProgress)
                {
                    onHalfway();
                    passedHalf = true;
                }
                yield return null;
            }
            mover.position = to;
        }
    }
}
