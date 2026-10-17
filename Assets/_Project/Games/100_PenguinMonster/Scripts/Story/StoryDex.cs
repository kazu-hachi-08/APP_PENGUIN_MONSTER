using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 図鑑の見せ方（PLAN_STORY_TOWN01.md フェーズ8）。ドラフトの子はいつも見せ、ストーリーの子は見た子だけ見せる
    /// </summary>
    public static class StoryDex
    {
        /// <summary>名前・絵・詳細を見せてよい（見ていないストーリーの子は「？」のマスにする）</summary>
        public static bool IsRevealed(MonsterDefinition monster, ICollection<int> seen)
        {
            return !MonsterTable.IsStory(monster.No) || seen.Contains(monster.No);
        }

        /// <summary>
        /// しぼりこんだ一覧。「すべて」は見ていない子も「？」のマスとして並べる（あと何体いるか分かるように）。
        /// タイプでしぼったときは、見ていない子を並べるとタイプが分かってしまうので出さない
        /// </summary>
        public static List<MonsterDefinition> Filter(MonsterType? type, ICollection<int> seen)
        {
            var shown = new List<MonsterDefinition>();
            foreach (var monster in MonsterTable.WithType(type))
            {
                if (type == null || IsRevealed(monster, seen))
                {
                    shown.Add(monster);
                }
            }
            return shown;
        }
    }
}
