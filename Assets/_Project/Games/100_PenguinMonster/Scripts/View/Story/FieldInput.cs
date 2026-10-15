using PenguinMonster.Common.Input;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの操作（STORY_ROADMAP.md §2）。PC は矢印・WASD・Z（A）/ X（B）、タッチの端末は画面の十字キーと A / B。
    /// 画面のボタンはタッチの端末のときだけ出す（PC では景色を隠さないように）
    /// </summary>
    public class FieldInput : MonoBehaviour
    {
        [SerializeField] private GameObject _touchControls;
        [SerializeField] private VirtualButton _up;
        [SerializeField] private VirtualButton _down;
        [SerializeField] private VirtualButton _left;
        [SerializeField] private VirtualButton _right;
        [SerializeField] private VirtualButton _a;
        [SerializeField] private VirtualButton _b;

        private void OnEnable()
        {
            _touchControls.SetActive(Touchscreen.current != null);
        }

        /// <summary>押しっぱなしの向き。同時押しは上・下・左・右の順に優先する</summary>
        public Direction? HeldDirection
        {
            get
            {
                var keyboard = Keyboard.current;
                if (IsHeld(_up, keyboard?.upArrowKey, keyboard?.wKey)) return Direction.Up;
                if (IsHeld(_down, keyboard?.downArrowKey, keyboard?.sKey)) return Direction.Down;
                if (IsHeld(_left, keyboard?.leftArrowKey, keyboard?.aKey)) return Direction.Left;
                if (IsHeld(_right, keyboard?.rightArrowKey, keyboard?.dKey)) return Direction.Right;
                return null;
            }
        }

        public bool IsADown => _a.IsDown || (Keyboard.current?.zKey.wasPressedThisFrame ?? false);
        public bool IsBDown => _b.IsDown || (Keyboard.current?.xKey.wasPressedThisFrame ?? false);

        private static bool IsHeld(VirtualButton button, KeyControl arrow, KeyControl letter)
        {
            return button.IsHeld || (arrow?.isPressed ?? false) || (letter?.isPressed ?? false);
        }
    }
}
