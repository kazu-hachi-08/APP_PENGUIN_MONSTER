using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ボスの定義表（7戦目・14戦目・21戦目。その先は最初のボスからくり返す）。
    /// それぞれ得意なタイプでそろえ、種族値の合計が高めのペンモンを入れている
    /// </summary>
    public static class BossTable
    {
        public static readonly IReadOnlyList<BossDefinition> All = new[]
        {
            new BossDefinition("ひょうざんの ユキオ", "この こおりの かべを こえられるかな？",
                Team(1, 7, 8, 9, 10, 12)),
            new BossDefinition("あらしの ライカ", "かみなりと かぜで ふきとばしてあげる！",
                Team(4, 17, 18, 20, 21, 3)),
            new BossDefinition("きょくやの ヨミ", "ながい よるの はじまりだ……",
                Team(22, 23, 24, 8, 15, 9)),
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
                team[i] = MonsterTable.All[numbers[i] - 1];
            }
            return team;
        }
    }
}
