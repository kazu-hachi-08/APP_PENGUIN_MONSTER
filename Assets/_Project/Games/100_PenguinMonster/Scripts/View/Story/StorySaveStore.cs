using System;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// ストーリーの記録の置き場所（PLAN_STORY_TOWN01.md フェーズ7）。記録は1つだけで、StorySaveData を JSON にして PlayerPrefs に置く。
    /// 中身の行き来は StorySave（純C#）が受け持つ
    /// </summary>
    public static class StorySaveStore
    {
        private const string Key = "PenguinMonster.Story.Save";

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
                return null;
            }
            var restored = StorySave.Restore(data, Town01Maps.CreateWorld(), Town01Maps.Start);
            if (restored == null)
            {
                Debug.LogWarning("[StorySaveStore] 今は無い種・技が入った記録か、新しい版の記録なので読みません");
            }
            return restored;
        }
    }
}
