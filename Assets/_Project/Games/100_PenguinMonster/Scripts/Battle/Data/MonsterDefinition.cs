using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモン1体の定義。見た目は View 側が No から描き分ける
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
        /// <summary>バトルでのレベル。定義表では全員 Lv50 で、ストーリーの子だけ写し（WithLoadout）で変える</summary>
        public int Level { get; }

        public MonsterDefinition(int no, string name, MonsterType[] types, Stats baseStats, MoveDefinition[] moves,
            Ability ability = Ability.None, HeldItem item = HeldItem.None, MonsterRank rank = MonsterRank.Normal,
            int level = BattleBalance.Level)
        {
            No = no;
            Name = name;
            Types = types;
            BaseStats = baseStats;
            Moves = moves;
            Ability = ability;
            Item = item;
            Rank = rank;
            Level = level;
        }

        /// <summary>
        /// 持ち物だけ違う写し。ドラフトの後に配った持ち物をここで持たせる。
        /// 持ち物を定義に持たせておけば、バトル側（HasItem など）は今のまま動く
        /// </summary>
        public MonsterDefinition WithItem(HeldItem item)
        {
            return WithLoadout(Level, (MoveDefinition[])Moves, item);
        }

        /// <summary>
        /// レベル・技・持ち物を差し替えた写し。ストーリーで育てた1体をバトルに出すときに使う。
        /// WithItem と同じく定義の側を差し替えるので、バトルの中身は変えずに済む
        /// </summary>
        public MonsterDefinition WithLoadout(int level, MoveDefinition[] moves, HeldItem item)
        {
            return new MonsterDefinition(No, Name, (MonsterType[])Types, BaseStats, moves, Ability, item, Rank, level);
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
