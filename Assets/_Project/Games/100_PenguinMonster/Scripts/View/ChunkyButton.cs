using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ボタンを「ぷっくり」した見た目にする（絵は ButtonArt が実行時に作る）。
    /// 押している間は沈んだ絵に替え、文字も同じだけ下げて押し込んだように見せる。
    /// 押せないとき（「決定」など）に薄く見せたいので、遷移は色のまま残し、絵の差し替えはここで行う
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ChunkyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("押したときに一緒に沈める文字。中身の多いカードなどは未設定にして、面だけ沈める")]
        [SerializeField] private RectTransform _label;

        private Button _button;
        private Vector2 _labelPosition;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.image.sprite = ButtonArt.Normal;
            _button.image.type = Image.Type.Sliced;

            if (_label == null) return;
            _label.offsetMin += Vector2.up * ButtonArt.LabelLiftUnits;
            _labelPosition = _label.anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable) return;
            _button.image.sprite = ButtonArt.Pressed;
            if (_label != null) _label.anchoredPosition = _labelPosition + Vector2.down * ButtonArt.PressedDropUnits;
        }

        public void OnPointerUp(PointerEventData eventData) => Release();

        // 押した直後にパネルごと隠れることがあるので、次に出たときに沈んだままにならないよう戻す
        private void OnDisable() => Release();

        private void Release()
        {
            if (_button != null) _button.image.sprite = ButtonArt.Normal;
            if (_label != null) _label.anchoredPosition = _labelPosition;
        }
    }
}
