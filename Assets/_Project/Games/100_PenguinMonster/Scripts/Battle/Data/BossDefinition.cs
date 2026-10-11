using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 勝ち抜き戦のボス1人（仕様書 §2.3）。チームは固定の6体で、選出は毎回ランダム
    /// </summary>
    public sealed class BossDefinition
    {
        public string Name { get; }
        /// <summary>登場のときのセリフ</summary>
        public string Line { get; }
        public IReadOnlyList<MonsterDefinition> Team { get; }

        public BossDefinition(string name, string line, MonsterDefinition[] team)
        {
            Name = name;
            Line = line;
            Team = team;
        }
    }
}
