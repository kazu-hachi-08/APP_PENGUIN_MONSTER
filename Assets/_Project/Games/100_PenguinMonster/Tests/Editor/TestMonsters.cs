namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// テスト用のペンモン。本番の定義表は数値が変わるので、テストはここの値で確かめる
    /// </summary>
    internal static class TestMonsters
    {
        public static readonly MoveDefinition Strike = new MoveDefinition("テストうち", MonsterType.Normal, MoveCategory.Physical, 50, 100);
        public static readonly MoveDefinition QuickStrike = new MoveDefinition("テストはやうち", MonsterType.Normal, MoveCategory.Physical, 40, 100, priority: 1);
        public static readonly MoveDefinition Finisher = new MoveDefinition("テストひっさつ", MonsterType.Normal, MoveCategory.Physical, 250, 100);

        public static MonsterDefinition Create(string name, int speed, MoveDefinition move, int attack = 50, int defense = 50)
        {
            return new MonsterDefinition(0, name, new[] { MonsterType.Normal },
                new Stats(50, attack, defense, 50, 50, speed),
                new[] { move, move, move, move });
        }

        public static MonsterDefinition[] Party(string prefix, int speed, MoveDefinition move, int attack = 50, int defense = 50)
        {
            return new[]
            {
                Create(prefix + "1", speed, move, attack, defense),
                Create(prefix + "2", speed, move, attack, defense),
                Create(prefix + "3", speed, move, attack, defense),
            };
        }
    }
}
