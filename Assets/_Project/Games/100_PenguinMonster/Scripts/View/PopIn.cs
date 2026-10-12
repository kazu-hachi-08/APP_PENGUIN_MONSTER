using System.Collections;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 「ぽよん」と出す演出。開始の大きさから等倍へ、少し行き過ぎてから戻る。
    /// 開始が 0 なら小さい所から弾んで出て、1 より大きければ上から叩きつけたように縮んで止まる
    /// </summary>
    public class PopIn : MonoBehaviour
    {
        [SerializeField] private float _startScale;
        [SerializeField] private float _seconds = 0.35f;

        // 行き過ぎる強さ（MonsterView の「ぽよん」と同じ式）
        private const float Overshoot = 1.7f;

        /// <param name="delay">並んだものを1つずつ出すときの待ち時間</param>
        public void Play(float delay = 0f)
        {
            StopAllCoroutines();
            transform.localScale = Vector3.one * _startScale;
            StartCoroutine(Animate(delay));
        }

        private IEnumerator Animate(float delay)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            for (float elapsed = 0f; elapsed < _seconds; elapsed += Time.deltaTime)
            {
                transform.localScale = Vector3.one * Mathf.LerpUnclamped(_startScale, 1f, BackOut(elapsed / _seconds));
                yield return null;
            }
            transform.localScale = Vector3.one;
        }

        private static float BackOut(float t)
        {
            float p = t - 1f;
            return 1f + p * p * ((Overshoot + 1f) * p + Overshoot);
        }

        // 途中で画面ごと隠れても、次に出たときに縮んだままにならないよう戻す
        private void OnDisable() => transform.localScale = Vector3.one;
    }
}
