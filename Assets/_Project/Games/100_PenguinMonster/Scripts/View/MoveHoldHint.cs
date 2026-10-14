using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技ボタン・交代画面の枠の説明を出す合図。タッチは長押し（離すと消える）、マウスは乗せている間。
    /// 長押しで出したときは、指を離しても技・交代を選ばないよう WasLongPress で知らせる
    /// </summary>
    public class MoveHoldHint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private const float NotPressed = -1f;

        [Tooltip("この秒数押し続けたら説明を出す")]
        [SerializeField] private float _holdSeconds = 0.4f;

        public event Action Shown;
        public event Action Hidden;

        public bool WasLongPress { get; private set; }

        private float _pressedAt = NotPressed;

        private void Update()
        {
            if (_pressedAt == NotPressed || Time.unscaledTime - _pressedAt < _holdSeconds)
            {
                return;
            }
            _pressedAt = NotPressed;
            WasLongPress = true;
            Shown?.Invoke();
        }

        private void OnDisable()
        {
            _pressedAt = NotPressed;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressedAt = Time.unscaledTime;
            WasLongPress = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressedAt = NotPressed;
            // マウスは乗せている間は出したままにする
            if (!IsMouse(eventData))
            {
                Hidden?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsMouse(eventData))
            {
                Shown?.Invoke();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 指を滑らせて外したら長押しも取りやめる
            _pressedAt = NotPressed;
            Hidden?.Invoke();
        }

        /// <summary>タッチも Enter が来るので、マウスかどうかで乗せただけの表示を分ける</summary>
        private static bool IsMouse(PointerEventData eventData)
        {
            return eventData is ExtendedPointerEventData extended && extended.pointerType == UIPointerType.MouseOrPen;
        }
    }
}
