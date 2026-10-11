using System;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// オンラインの制限時間（仕様書 §2.1 / §2.2 / §4.7）。残り秒数を出し、0 になったら一度だけ知らせる。
    /// オフラインでは使わないので、止めている間は自分ごと隠す
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private string _format = "残り {0}秒";

        private float _remaining;
        private Action _onTimeout;

        /// <param name="seconds">0 以下なら制限なし（隠したまま）</param>
        public void Begin(float seconds, Action onTimeout)
        {
            if (seconds <= 0f)
            {
                Stop();
                return;
            }
            _remaining = seconds;
            _onTimeout = onTimeout;
            Refresh();
            gameObject.SetActive(true);
        }

        public void Stop()
        {
            _onTimeout = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            // ポーズ等で時間が止まっても相手は待ってくれないので、実時間で数える
            _remaining -= Time.unscaledDeltaTime;
            Refresh();
            if (_remaining > 0f)
            {
                return;
            }
            var onTimeout = _onTimeout;
            Stop();
            onTimeout?.Invoke();
        }

        private void Refresh()
        {
            _text.text = string.Format(_format, Mathf.Max(0, Mathf.CeilToInt(_remaining)));
        }
    }
}
