using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ボスの定義表（7戦目・14戦目…70戦目。その先は最初のボスからくり返す）。
    /// それぞれ得意なタイプ（4・5人目は天候、6人目からは題材）でそろえ、エースと切り札の伝説を1体ずつ入れている
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
            new BossDefinition("大雨の シズク", "この 雨は 止まないよ。ずっとね。",
                Team(56, 67, 57, 34, 72, 74), Monster(74)),
            new BossDefinition("灼熱の ヒナタ", "お日さまの 力、見せてあげる！",
                Team(58, 62, 59, 16, 73, 75), Monster(75)),
            // 42戦目〜70戦目。新しい伝説を切り札にし、題材（太古・森・始まり・日食・空）でそろえる
            new BossDefinition("太古の ガンテツ", "氷河の 時代から 待っていたぞ。",
                Team(80, 117, 118, 119, 140, 146), Monster(146)),
            new BossDefinition("森の コダマ", "森は ゆっくり、でも 必ず 育つの。",
                Team(111, 112, 114, 113, 143, 147), Monster(147)),
            new BossDefinition("始まりの ウイ", "ぜんぶの ペンギンは ここから 始まったんだ。",
                Team(125, 105, 82, 123, 144, 148), Monster(148)),
            new BossDefinition("日食の クロエ", "太陽が 欠ける とき、わたしは 強くなる。",
                Team(104, 101, 124, 81, 141, 149), Monster(149)),
            new BossDefinition("天空の ソラ", "ペンギンだって 空を 飛べる。見せてあげる！",
                Team(95, 93, 85, 94, 145, 150), Monster(150)),
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
