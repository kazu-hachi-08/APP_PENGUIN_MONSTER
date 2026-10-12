using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// バトル全体の状態。陣営 0 と 1 を持つ
    /// </summary>
    public sealed class BattleState
    {
        public const int SideCount = 2;

        public IReadOnlyList<BattleSide> Sides { get; }
        public bool IsOver { get; private set; }
        /// <summary>勝った陣営。決着前は -1</summary>
        public int Winner { get; private set; } = -1;

        public BattleState(IReadOnlyList<MonsterDefinition> party0, IReadOnlyList<MonsterDefinition> party1,
            int activeCount = BattleBalance.SingleActiveCount)
        {
            Sides = new[]
            {
                new BattleSide(0, party0, activeCount),
                new BattleSide(1, party1, activeCount),
            };
        }

        public BattleSide Opponent(int side) => Sides[1 - side];

        internal void Finish(int winner)
        {
            IsOver = true;
            Winner = winner;
        }
    }
}
