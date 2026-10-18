using System;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// ストーリーの記録の置き場所。記録は1つだけで、StorySaveData を JSON にして PlayerPrefs に置く。
    /// 中身の行き来は StorySave（純C#）が受け持つ
    /// </summary>
    public static class StorySaveStore
    {
        private const string Key = "PenguinMonster.Story.Save";
        // 読めなかった記録の退避先。自動で記録するので、退避しないと最初からの記録ですぐ上書きされて消える
        private const string UnreadableKey = "PenguinMonster.Story.Save.Unreadable";

        public static void Save(StorySaveData data)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 記録を戻す。無い・壊れている・今は読めない（StorySave.Restore を参照）なら null（記録が無いのと同じに扱う）
        /// </summary>
        public static RestoredStory Load()
        {
            string json = PlayerPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            // JSON に無い項目を初期値のまま残すため、作っておいた中身に上書きで読む
            var data = new StorySaveData();
            try
            {
                JsonUtility.FromJsonOverwrite(json, data);
            }
            catch (ArgumentException e)
            {
                Debug.LogWarning($"[StorySaveStore] 記録を読めませんでした: {e.Message}");
                KeepUnreadable(json);
                return null;
            }
            var restored = StorySave.Restore(data, StoryWorld.Create(), Town01Maps.Start);
            if (restored == null)
            {
                Debug.LogWarning("[StorySaveStore] 今は無い種・技が入った記録か、新しい版の記録なので読みません");
                KeepUnreadable(json);
            }
            return restored;
        }

        /// <summary>
        /// 読めなかった記録を別のキーに残す（後の版で読み替えて戻せるように）。
        /// すでに残してあるものは上書きしない（最初に読めなくなった記録が、一番進んでいた本来の記録のはずなので）
        /// </summary>
        private static void KeepUnreadable(string json)
        {
            if (PlayerPrefs.HasKey(UnreadableKey))
            {
                return;
            }
            PlayerPrefs.SetString(UnreadableKey, json);
            PlayerPrefs.Save();
        }
    }
}
