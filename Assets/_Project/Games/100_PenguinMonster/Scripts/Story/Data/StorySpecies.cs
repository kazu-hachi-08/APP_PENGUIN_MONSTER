using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリーに出る種（No.151〜）の、バトルの定義に足す育成の情報（STORY_ROADMAP.md §3.3）。
    /// 種族値・タイプ・特性は今までどおり MonsterDefinition に持ち、ここでは包むだけにする
    /// </summary>
    public sealed class StorySpecies
    {
        public MonsterDefinition Definition { get; }

        public int No => Definition.No;
        public string Name => Definition.Name;

        public StorySpecies(MonsterDefinition definition)
        {
            Definition = definition;
        }
    }
}
