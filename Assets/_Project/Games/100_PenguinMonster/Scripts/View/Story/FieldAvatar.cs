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
            var from = transform.position;
            _renderer.sprite = HeroArt.Get(facing, 1, _mirrorNextStep);
            _mirrorNextStep = !_mirrorNextStep;
            bool standing = false;
            for (float t = 0f; t < seconds; t += Time.deltaTime)
            {
                float progress = t / seconds;
                transform.position = Vector3.Lerp(from, to, progress);
                if (!standing && progress >= 0.5f)
                {
                    _renderer.sprite = HeroArt.Get(facing, 0);
                    standing = true;
                }
                yield return null;
            }
            Place(to, facing);
        }
    }
}
