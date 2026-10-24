using System.Collections;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの主人公の絵。向きを変える・1マス歩く（こぐ）動きだけを持つ（どこへ行けるかは FieldPlayer が決める）
    /// </summary>
    public class FieldAvatar : MonoBehaviour
    {
        // 1マスの前半は足を出したコマ、ここから後は立ちのコマ（歩いてくるトレーナーも同じ）
        private const float StandFromProgress = 0.5f;

        [SerializeField] private SpriteRenderer _renderer;
        [Tooltip("下り坂で後ろへ流れる風の線。シーンを作り直す前は無くても歩ける")]
        [SerializeField] private SpriteRenderer _wind;
        [Tooltip("風の線が1マスの間に後ろへ流れる幅（マス）")]
        [SerializeField] private float _windDrift = 0.5f;

        // 上下に歩くときは、歩きのコマを1歩ごとに反転して左右の足を交互に出す
        private bool _mirrorNextStep;
        // 自転車は1歩ごとにこぐ足を替える
        private int _pedalFrame;
        private Direction _facing;

        /// <summary>自転車に乗っているか。替えたらすぐ絵を替える</summary>
        public bool Riding { get; private set; }

        private void Awake()
        {
            // 絵はコードで描くので、シーンには入れずに起きたときに付ける
            if (_wind != null)
            {
                _wind.sprite = FieldTileArt.Wind();
                _wind.enabled = false;
            }
        }

        public void SetRiding(bool riding)
        {
            if (Riding == riding)
            {
                return;
            }
            Riding = riding;
            Face(_facing);
        }

        public void Place(Vector3 feet, Direction facing)
        {
            transform.position = feet;
            Face(facing);
        }

        public void Face(Direction facing)
        {
            _facing = facing;
            _renderer.sprite = Riding ? HeroArt.GetBicycle(facing, _pedalFrame) : HeroArt.Get(facing, 0);
        }

        /// <summary>前半は足を出したコマ、後半は立ちのコマで1マス進む。自転車はこぐ足を替えて進む</summary>
        /// <param name="windy">下り坂を滑る1歩。後ろへ流れる風の線を出す</param>
        public IEnumerator Walk(Vector3 to, Direction facing, float seconds, bool windy = false)
        {
            _facing = facing;
            if (Riding)
            {
                _pedalFrame = 1 - _pedalFrame;
                _renderer.sprite = HeroArt.GetBicycle(facing, _pedalFrame);
            }
            else
            {
                _renderer.sprite = HeroArt.Get(facing, 1, _mirrorNextStep);
                _mirrorNextStep = !_mirrorNextStep;
            }
            ShowWind(windy);
            yield return Slide(transform, to, seconds, () => Face(facing), DriftWind);
            ShowWind(false);
            Place(to, facing);
        }

        private void ShowWind(bool visible)
        {
            if (_wind != null)
            {
                _wind.enabled = visible;
            }
        }

        /// <summary>風の線を、進む向きと逆へ少しずつずらす（主人公の後ろへ流れて見える）</summary>
        private void DriftWind(float progress)
        {
            if (_wind != null && _wind.enabled)
            {
                _wind.transform.localPosition = new Vector3(-_windDrift * progress, _wind.transform.localPosition.y, 0f);
            }
        }

        /// <summary>1マス分を滑らかに動かし、半分まで来たら onHalfway を1回呼ぶ（立ちのコマへ替える）。最後は to にぴったり置く</summary>
        /// <param name="onProgress">毎フレーム、進んだ割合（0〜1）を受け取る。無ければ省く</param>
        public static IEnumerator Slide(Transform mover, Vector3 to, float seconds, System.Action onHalfway,
            System.Action<float> onProgress = null)
        {
            var from = mover.position;
            bool passedHalf = false;
            for (float t = 0f; t < seconds; t += Time.deltaTime)
            {
                float progress = t / seconds;
                mover.position = Vector3.Lerp(from, to, progress);
                onProgress?.Invoke(progress);
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
