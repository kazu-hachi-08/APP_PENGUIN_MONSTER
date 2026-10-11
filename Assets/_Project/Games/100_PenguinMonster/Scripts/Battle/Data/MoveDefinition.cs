namespace PenguinMonster.Battle
{
    public enum MoveCategory
    {
        Physical,
        Special,
    }

    /// <summary>
    /// 技の定義（仕様書 §4.2）。変化技・追加効果は Phase 3 で足す
    /// </summary>
    public sealed class MoveDefinition
    {
        public string Name { get; }
        public MonsterType Type { get; }
        public MoveCategory Category { get; }
        public int Power { get; }
        /// <summary>命中率（%）</summary>
        public int Accuracy { get; }
        public int Priority { get; }

        public MoveDefinition(string name, MonsterType type, MoveCategory category, int power, int accuracy, int priority = 0)
        {
            Name = name;
            Type = type;
            Category = category;
            Power = power;
            Accuracy = accuracy;
            Priority = priority;
        }
    }
}
