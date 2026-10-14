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
        /// <summary>
        /// Six の中の切り札（ボスでなければ null）。持ち物を持たせた写しなので Boss.TrumpCard（定義表の元）とは別のインスタンス。
        /// 選出は Six の中から参照で選ぶので、こちらを使う
        /// </summary>
        public MonsterDefinition TrumpCard { get; }

        public bool IsBoss => Boss != null;

        public SurvivalOpponent(IReadOnlyList<MonsterDefinition> six, CpuPlayer cpu, BossDefinition boss = null,
            MonsterDefinition trumpCard = null)
        {
            Six = six;
            Cpu = cpu;
            Boss = boss;
            TrumpCard = trumpCard;
        }
    }
}
