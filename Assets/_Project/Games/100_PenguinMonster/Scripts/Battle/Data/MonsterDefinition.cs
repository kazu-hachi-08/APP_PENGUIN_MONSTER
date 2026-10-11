using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモン1体の定義（仕様書 §3.1）。特性・持ち物は後のフェーズで足す
    /// </summary>
    public sealed class MonsterDefinition
    {
        public int No { get; }
        public string Name { get; }
        public IReadOnlyList<MonsterType> Types { get; }
        public Stats BaseStats { get; }
        public IReadOnlyList<MoveDefinition> Moves { get; }
        public MonsterLook Look { get; }

        public MonsterDefinition(int no, string name, MonsterType[] types, Stats baseStats, MoveDefinition[] moves, MonsterLook look = null)
        {
            No = no;
            Name = name;
            Types = types;
            BaseStats = baseStats;
            Moves = moves;
            // テスト用のペンモンは見た目を気にしないので省略できるようにする
            Look = look ?? MonsterLook.Default;
        }

        public bool HasType(MonsterType type)
        {
            foreach (var own in Types)
            {
                if (own == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
