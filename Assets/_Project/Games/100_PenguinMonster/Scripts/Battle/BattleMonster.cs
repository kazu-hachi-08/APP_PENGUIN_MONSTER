using System;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// バトル中のペンモン1体（定義＋現在のHP）
    /// </summary>
    public sealed class BattleMonster
    {
        public MonsterDefinition Definition { get; }
        public Stats Stats { get; }
        public int CurrentHp { get; private set; }

        public string Name => Definition.Name;
        public int MaxHp => Stats.Hp;
        public bool IsFainted => CurrentHp <= 0;

        public BattleMonster(MonsterDefinition definition)
        {
            Definition = definition;
            Stats = Stats.FromBase(definition.BaseStats);
            CurrentHp = Stats.Hp;
        }

        public void TakeDamage(int damage)
        {
            CurrentHp = Math.Max(0, CurrentHp - damage);
        }
    }
}
