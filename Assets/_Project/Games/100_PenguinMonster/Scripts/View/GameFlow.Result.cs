using System.Collections;
using PenguinMonster.Battle;
using PenguinMonster.Common.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PenguinMonster.View
{
    /// <summary>
    /// 1試合ごとの結果。勝ったら「つぎの たたかいへ」で続け、負けたら連勝数を出してタイトルへ
    /// </summary>
    public partial class GameFlow
    {
        private const string WinTitle = "かち！";
        private const string LoseTitle = "まけ…";
        private const string WinDetail = "あいての ペンモンを ぜんぶ たおした！";
        private const string BossWinDetail = "ボスに かった！";
        private const string LoseDetail = "じぶんの ペンモンが ぜんぶ たおれた…";
        private const string NewRecordText = "しんきろく！";

        private IEnumerator ShowWin(SurvivalRun run, SurvivalOpponent opponent)
        {
            bool next = false;
            _resultDialog.Show(WinTitle, $"{run.Wins} れんしょう", opponent.IsBoss ? BossWinDetail : WinDetail,
                onRetry: () => next = true, onTitle: BackToTitle);
            yield return new WaitUntil(() => next);
        }

        private void ShowLose(SurvivalRun run, int bestAtStart)
        {
            string record = run.Wins > bestAtStart ? NewRecordText : $"さいこう きろく: {Mathf.Max(run.Wins, bestAtStart)} れんしょう";
            _resultDialog.Show(LoseTitle, $"{run.Wins} れんしょう", $"{LoseDetail}\n{record}", onTitle: BackToTitle);
        }

        /// <summary>タイトル専用のシーンは無いので、シーンを読み直して最初（タイトル）から始め直す</summary>
        private static void BackToTitle()
        {
            SceneManager.LoadScene(SceneNames.Title);
        }
    }
}
