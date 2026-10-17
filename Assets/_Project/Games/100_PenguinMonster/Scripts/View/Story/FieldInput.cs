using PenguinMonster.Common.Input;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの操作。PC は矢印・WASD・Z（A）/ X（B）、タッチの端末は画面の十字キーと A / B。
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
            SetPadVisible(true);
        }

        /// <summary>会話の間は十字キーを隠す（下の会話窓と重なるのと、窓をタップすれば送れるため）</summary>
        public void SetPadVisible(bool visible)
        {
            _touchControls.SetActive(visible && Touchscreen.current != null);
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

        /// <summary>
        /// 縦に並んだ選択肢の送り（-1 で上、1 で下、0 は動かさない）。押し続けても1回だけ動かすよう、
        /// 前のフレームの向き（lastHeld）から変わった瞬間だけ見る
        /// </summary>
        public static int VerticalStep(Direction? held, Direction? lastHeld)
        {
            if (held == lastHeld) return 0;
            if (held == Direction.Up) return -1;
            if (held == Direction.Down) return 1;
            return 0;
        }

        public bool IsADown => _a.IsDown || (Keyboard.current?.zKey.wasPressedThisFrame ?? false);
        public bool IsBDown => _b.IsDown || (Keyboard.current?.xKey.wasPressedThisFrame ?? false);

        private static bool IsHeld(VirtualButton button, KeyControl arrow, KeyControl letter)
        {
            return button.IsHeld || (arrow?.isPressed ?? false) || (letter?.isPressed ?? false);
        }
    }
}
