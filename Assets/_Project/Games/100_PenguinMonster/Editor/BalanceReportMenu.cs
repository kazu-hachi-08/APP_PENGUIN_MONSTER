using System.Linq;
using System.Text;
using PenguinMonster.Battle;
using UnityEditor;
using UnityEngine;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// バランス確認用。CPU どうしを大量に戦わせ、ペンモンごとの勝率をランク別に Console へ出す。
    /// 同じランクの平均から大きく外れた子に印を付けて、種族値・技を見直す目安にする
    /// </summary>
    public static class BalanceReportMenu
    {
        private const int Battles = 20000;
        private const int Seed = 1;
        /// <summary>ランクの平均からこれ以上離れたら印を付ける（勝率の差）</summary>
        private const float OutlierGap = 0.1f;

        [MenuItem("Tools/PenguinMonster/Balance Report")]
        public static void Report()
        {
            EditorUtility.DisplayProgressBar("Balance Report", $"{Battles} 試合 戦わせています…", 0f);
            try
            {
                var rates = BalanceSimulator.Run(MonsterTable.All, Battles, Seed);
                Debug.Log(Format(rates));
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static string Format(System.Collections.Generic.IReadOnlyList<MonsterWinRate> rates)
        {
            var text = new StringBuilder($"ペンモンの勝率（ランダムな3体どうし・CPU つよい・{Battles} 試合）\n");
            foreach (var group in rates.GroupBy(r => r.Monster.Rank).OrderBy(g => g.Key))
            {
                float average = group.Average(r => r.Rate);
                text.AppendLine($"== {group.Key}  平均 {average:P1}");
                foreach (var rate in group.OrderByDescending(r => r.Rate))
                {
                    string mark = Mathf.Abs(rate.Rate - average) >= OutlierGap ? "  ←" : "";
                    text.AppendLine($"No.{rate.Monster.No,2} {rate.Monster.Name}  合計{rate.Monster.BaseStats.Total}  {rate.Rate:P1}{mark}");
                }
            }
            return text.ToString();
        }
    }
}
