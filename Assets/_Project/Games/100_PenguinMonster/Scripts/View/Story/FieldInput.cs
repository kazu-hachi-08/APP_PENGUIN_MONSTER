using PenguinMonster.Common.Input;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの操作。PC は矢印・WASD・Z（A）/ X（B）・Shift（走る）、タッチの端末は画面の十字キーと A / B・走る。
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
        [Tooltip("走るの切り替え。シューズを持つまでは隠す")]
        [SerializeField] private VirtualButton _run;
        [SerializeField] private Image _runImage;
        [SerializeField] private Color _runOffColor = new Color(0.45f, 0.6f, 0.85f, 0.8f);
        [SerializeField] private Color _runOnColor = new Color(1f, 0.6f, 0.2f, 0.9f);

        private bool _runToggleQueued;

        private void OnEnable()
        {
            _runToggleQueued = false;
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

        public bool IsADown => IsActiveDown(_a) || (Keyboard.current?.zKey.wasPressedThisFrame ?? false);
        public bool IsBDown => IsActiveDown(_b) || (Keyboard.current?.xKey.wasPressedThisFrame ?? false);

        /// <summary>前に見てから走るの切り替えが押されたか。見たら忘れる</summary>
        public bool ConsumeRunToggle()
        {
            bool pressed = _runToggleQueued;
            _runToggleQueued = false;
            return pressed;
        }

        private void Update()
        {
            // 1歩の途中（進行役が入力を見ていない間）に押しても取りこぼさないよう、ここで覚えておく
            bool shift = (Keyboard.current?.leftShiftKey.wasPressedThisFrame ?? false)
                || (Keyboard.current?.rightShiftKey.wasPressedThisFrame ?? false);
            if (shift || IsActiveDown(_run))
            {
                _runToggleQueued = true;
            }
        }

        /// <summary>走るボタンの出し入れと色。十字キーごと隠れている間（PC・会話中）は見えない</summary>
        public void ShowRun(bool hasShoes, bool isOn)
        {
            if (_run == null)
            {
                // シーンを作り直す前（ボタンがまだ無い）でも歩けるように
                return;
            }
            _run.gameObject.SetActive(hasShoes);
            _runImage.color = isOn ? _runOnColor : _runOffColor;
        }

        private static bool IsActiveDown(VirtualButton button)
        {
            return button != null && button.gameObject.activeInHierarchy && button.IsDown;
        }

        private static bool IsHeld(VirtualButton button, KeyControl arrow, KeyControl letter)
        {
            bool buttonHeld = button != null && button.gameObject.activeInHierarchy && button.IsHeld;
            return buttonHeld || (arrow?.isPressed ?? false) || (letter?.isPressed ?? false);
        }
    }
}
