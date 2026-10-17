using PenguinMonster.Common.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 設定。BGM・SE の音量とメッセージの速さ。
    /// 音量は共通基盤の AudioManager に渡し、閉じるときに保存する。速さは押したらすぐ GameSettings に保存する
    /// </summary>
    public class SettingsPanel : SubMenuPanel
    {
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _seSlider;
        [Tooltip("MessageSpeed の並び順（遅い / 普通 / 速い）")]
        [SerializeField] private Button[] _speedButtons;

        private Color[] _speedColors;

        protected override void Awake()
        {
            base.Awake();
            _bgmSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetBgmVolume(v));
            _seSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetSeVolume(v));
            _speedColors = ButtonHighlight.CaptureColors(_speedButtons);
            for (int i = 0; i < _speedButtons.Length; i++)
            {
                var speed = (MessageSpeed)i;
                _speedButtons[i].onClick.AddListener(() => ChooseSpeed(speed));
            }
        }

        protected override void OnShow()
        {
            // 値を入れると onValueChanged が呼ばれるが、同じ値を入れ直すだけなので問題ない
            _bgmSlider.value = AudioManager.Instance.BgmVolume;
            _seSlider.value = AudioManager.Instance.SeVolume;
            PaintSpeedButtons();
        }

        protected override void OnHide()
        {
            AudioManager.Instance.SaveAudioSettings();
        }

        private void ChooseSpeed(MessageSpeed speed)
        {
            BattleSounds.Play(BattleSound.Cursor);
            GameSettings.Speed = speed;
            PaintSpeedButtons();
        }

        private void PaintSpeedButtons()
        {
            ButtonHighlight.Paint(_speedButtons, _speedColors, (int)GameSettings.Speed);
        }
    }
}
