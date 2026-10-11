using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 勝ち抜き戦の1戦ぶんの相手。ボスのときだけ名前とセリフがある
    /// </summary>
    public sealed class SurvivalOpponent
    {
        public IReadOnlyList<MonsterDefinition> Six { get; }
        public CpuPlayer Cpu { get; }
        /// <summary>ボスでなければ null</summary>
        public BossDefinition Boss { get; }

        public bool IsBoss => Boss != null;

        public SurvivalOpponent(IReadOnlyList<MonsterDefinition> six, CpuPlayer cpu, BossDefinition boss = null)
        {
            Six = six;
            Cpu = cpu;
            Boss = boss;
        }
    }
}
