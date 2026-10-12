using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ボスの定義表（7戦目・14戦目・21戦目。その先は最初のボスからくり返す）。
    /// それぞれ得意なタイプでそろえ、エースと切り札の伝説を1体ずつ入れている
    /// </summary>
    public static class BossTable
    {
        public static readonly IReadOnlyList<BossDefinition> All = new[]
        {
            new BossDefinition("氷山の ユキオ", "この 氷の 壁を 越えられるかな？",
                Team(7, 9, 10, 26, 45, 48), Monster(48)),
            new BossDefinition("嵐の ライカ", "雷と 風で 吹き飛ばしてあげる！",
                Team(4, 17, 20, 44, 47, 49), Monster(49)),
            new BossDefinition("極夜の ヨミ", "長い 夜の 始まりだ……",
                Team(22, 23, 33, 35, 46, 50), Monster(50)),
        };

        /// <summary>何戦目のボスか。ボス戦でない戦数を渡したときは呼ばないこと</summary>
        public static BossDefinition ForBattle(int battleNumber)
        {
            int round = battleNumber / BattleBalance.BossInterval - 1;
            return All[round % All.Count];
        }

        /// <summary>定義表の No で並べる（No は 1 から）</summary>
        private static MonsterDefinition[] Team(params int[] numbers)
        {
            var team = new MonsterDefinition[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                team[i] = Monster(numbers[i]);
            }
            return team;
        }

        private static MonsterDefinition Monster(int no) => MonsterTable.All[no - 1];
    }
}
