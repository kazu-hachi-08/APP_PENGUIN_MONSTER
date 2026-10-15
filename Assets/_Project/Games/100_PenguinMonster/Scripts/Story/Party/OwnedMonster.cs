using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 捕まえた（もらった）ペンモン1体（STORY_ROADMAP.md §3.2）。
    /// 種の定義とは別に、レベル・技・HP・状態異常を戦いの後も持ち越すために持つ
    /// </summary>
    public sealed class OwnedMonster
    {
        private readonly List<MoveDefinition> _moves;

        public StorySpecies Species { get; private set; }
        public int Level { get; private set; }
        public int CurrentHp { get; private set; }
        public StatusCondition Status { get; private set; }
        public HeldItem Item { get; set; }

        public int No => Species.No;
        public string Name => Species.Name;
        public IReadOnlyList<MoveDefinition> Moves => _moves;
        public Stats Stats => Stats.FromBase(Species.Definition.BaseStats, Level);
        public int MaxHp => Stats.Hp;
        public bool IsFainted => CurrentHp <= 0;

        /// <summary>HP 満タン・状態異常なしで作る</summary>
        public OwnedMonster(StorySpecies species, int level, IEnumerable<MoveDefinition> moves, HeldItem item = HeldItem.None)
        {
            Species = species;
            Level = level;
            _moves = new List<MoveDefinition>(moves);
            Item = item;
            CurrentHp = MaxHp;
        }

        /// <summary>
        /// バトルに出す写し。定義の側をレベル・技・持ち物で差し替えるので、バトルの中身はドラフトと同じまま動く
        /// </summary>
        public BattleMonster ToBattle()
        {
            var definition = Species.Definition.WithLoadout(Level, _moves.ToArray(), Item);
            return new BattleMonster(definition, CurrentHp, Status);
        }

        /// <summary>戦いの後の HP・状態異常を持ち帰る（ストーリーでは傷が残る）</summary>
        public void ApplyBattleResult(BattleMonster fought)
        {
            CurrentHp = fought.CurrentHp;
            Status = fought.Status;
        }
    }
}
