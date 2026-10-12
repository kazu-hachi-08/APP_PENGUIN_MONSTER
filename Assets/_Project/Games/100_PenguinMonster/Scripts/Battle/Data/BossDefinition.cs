using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 勝ち抜き戦のボス1人（仕様書 §2.3）。チームは固定の6体。選出は切り札（伝説）を必ず最後に入れ、残りはランダム
    /// </summary>
    public sealed class BossDefinition
    {
        public string Name { get; }
        /// <summary>登場のときのセリフ</summary>
        public string Line { get; }
        public IReadOnlyList<MonsterDefinition> Team { get; }
        /// <summary>切り札の伝説。Team の中の1体</summary>
        public MonsterDefinition TrumpCard { get; }

        public BossDefinition(string name, string line, MonsterDefinition[] team, MonsterDefinition trumpCard)
        {
            Name = name;
            Line = line;
            Team = team;
            TrumpCard = trumpCard;
        }
    }
}
