using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモン1体の定義（仕様書 §3.1）。見た目は View 側が No から描き分ける
    /// </summary>
    public sealed class MonsterDefinition
    {
        public int No { get; }
        public string Name { get; }
        public IReadOnlyList<MonsterType> Types { get; }
        public Stats BaseStats { get; }
        public IReadOnlyList<MoveDefinition> Moves { get; }
        public Ability Ability { get; }
        public HeldItem Item { get; }
        public MonsterRank Rank { get; }

        public MonsterDefinition(int no, string name, MonsterType[] types, Stats baseStats, MoveDefinition[] moves,
            Ability ability = Ability.None, HeldItem item = HeldItem.None, MonsterRank rank = MonsterRank.Normal)
        {
            No = no;
            Name = name;
            Types = types;
            BaseStats = baseStats;
            Moves = moves;
            Ability = ability;
            Item = item;
            Rank = rank;
        }

        /// <summary>
        /// 持ち物だけ違う写し。ドラフトの後に配った持ち物をここで持たせる。
        /// 持ち物を定義に持たせておけば、バトル側（HasItem など）は今のまま動く
        /// </summary>
        public MonsterDefinition WithItem(HeldItem item)
        {
            return new MonsterDefinition(No, Name, (MonsterType[])Types, BaseStats, (MoveDefinition[])Moves, Ability, item, Rank);
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
