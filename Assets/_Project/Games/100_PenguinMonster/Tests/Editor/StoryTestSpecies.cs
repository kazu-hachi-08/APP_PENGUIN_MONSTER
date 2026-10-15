using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>
    /// ストーリーのテスト用の仮の3体（3段進化の1系統）。本番の定義表はフェーズ8で埋まるので、テストはここの値で確かめる
    /// </summary>
    internal static class StoryTestSpecies
    {
        public static readonly MoveDefinition Peck = new MoveDefinition("テストつつく", MonsterType.Normal, MoveCategory.Physical, 40, 100);

        public static readonly StorySpecies Chick = new StorySpecies(Definition(901, "テストヒナ", new Stats(45, 49, 49, 65, 65, 45)));
        public static readonly StorySpecies Junior = new StorySpecies(Definition(902, "テストワカ", new Stats(60, 62, 63, 80, 80, 60)));
        public static readonly StorySpecies Adult = new StorySpecies(Definition(903, "テストオトナ", new Stats(80, 82, 83, 100, 100, 80)));

        private static MonsterDefinition Definition(int no, string name, Stats baseStats)
        {
            var moves = new[] { Peck, Peck, Peck, Peck };
            return new MonsterDefinition(no, name, new[] { MonsterType.Normal }, baseStats, moves, rank: MonsterRank.Young);
        }

        /// <summary>技を1つだけ覚えた子</summary>
        public static OwnedMonster Owned(StorySpecies species, int level)
        {
            return new OwnedMonster(species, level, new[] { Peck });
        }
    }
}
