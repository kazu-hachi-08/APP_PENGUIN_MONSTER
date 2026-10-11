using UnityEngine;

namespace PenguinMonster.View
{
    public enum MessageSpeed
    {
        Slow,
        Normal,
        Fast,
    }

    /// <summary>
    /// ペンモン固有の設定（仕様書 §9）。音量は共通基盤の AudioManager が保存するので、ここはメッセージの速さだけ
    /// </summary>
    public static class GameSettings
    {
        private const string MessageSpeedKey = "PenguinMonster.MessageSpeed";

        public static readonly string[] SpeedLabels = { "遅い", "普通", "速い" };
        /// <summary>MessageWindow の文字送りの速さに掛ける倍率</summary>
        private static readonly float[] SpeedRates = { 0.5f, 1f, 2f };

        public static MessageSpeed Speed
        {
            get => (MessageSpeed)PlayerPrefs.GetInt(MessageSpeedKey, (int)MessageSpeed.Normal);
            set
            {
                PlayerPrefs.SetInt(MessageSpeedKey, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static float SpeedRate => SpeedRates[(int)Speed];
    }
}
