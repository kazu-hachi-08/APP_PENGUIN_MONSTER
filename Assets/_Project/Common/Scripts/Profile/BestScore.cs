using UnityEngine;

namespace PenguinMonster.Common.Profile
{
    /// <summary>
    /// 端末に保存する最高記録。ゲームごとにキーを分けて使う（例: 勝ち抜き戦の最高連勝数）
    /// </summary>
    public static class BestScore
    {
        private const string KeyPrefix = "BestScore.";

        public static int Get(string key) => PlayerPrefs.GetInt(KeyPrefix + key, 0);

        /// <summary>記録を更新したら保存して true。届かなければ何もしない</summary>
        public static bool Submit(string key, int score)
        {
            if (score <= Get(key)) return false;

            PlayerPrefs.SetInt(KeyPrefix + key, score);
            // ブラウザ版は Save しないとタブを閉じたときに消えることがあるため即書き込む
            PlayerPrefs.Save();
            return true;
        }
    }
}
