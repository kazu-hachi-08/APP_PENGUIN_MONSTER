using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ボタンを「ぷっくり」した見た目にする（絵は ButtonArt が実行時に作る）。
    /// 押している間は沈んだ絵に替え、文字も同じだけ下げて押し込んだように見せる
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ChunkyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _label;

        private Button _button;
        private Vector2 _labelPosition;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.image.sprite = ButtonArt.Normal;
            _button.image.type = Image.Type.Sliced;
            _button.transition = Selectable.Transition.SpriteSwap;
            _button.spriteState = new SpriteState { pressedSprite = ButtonArt.Pressed };

            _label.offsetMin += Vector2.up * ButtonArt.LabelLiftUnits;
            _labelPosition = _label.anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable) return;
            _label.anchoredPosition = _labelPosition + Vector2.down * ButtonArt.PressedDropUnits;
        }

        public void OnPointerUp(PointerEventData eventData) => ResetLabel();

        // 押した直後にパネルごと隠れることがあるので、次に出たときに沈んだままにならないよう戻す
        private void OnDisable() => ResetLabel();

        private void ResetLabel()
        {
            if (_label != null) _label.anchoredPosition = _labelPosition;
        }
    }
}
