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
        private const string WinTitle = "勝ち！";
        private const string LoseTitle = "負け…";
        private const string WinDetail = "相手の ペンモンを 全部 倒した！";
        private const string BossWinDetail = "ボスに 勝った！";
        private const string LoseDetail = "自分の ペンモンが 全部 倒れた…";
        private const string NewRecordText = "新記録！";

        private IEnumerator ShowWin(SurvivalRun run, SurvivalOpponent opponent)
        {
            bool next = false;
            _quitButton.Hide();
            ShowResult(true, $"{run.Wins} 連勝", opponent.IsBoss ? BossWinDetail : WinDetail, onRetry: () => next = true);
            yield return new WaitUntil(() => next);
            _quitButton.Show(SurvivalQuitMessage, BackToTitle);
        }

        private void ShowLose(SurvivalRun run, int bestAtStart)
        {
            _quitButton.Hide();
            string record = run.Wins > bestAtStart ? NewRecordText : $"最高記録: {Mathf.Max(run.Wins, bestAtStart)} 連勝";
            ShowResult(false, $"{run.Wins} 連勝", $"{LoseDetail}\n{record}");
        }

        /// <param name="score">連勝数。空ならバッジごと隠す（オンライン）</param>
        /// <param name="onRetry">「次の 戦いへ」。null ならボタンを出さない</param>
        private void ShowResult(bool won, string score, string detail, System.Action onRetry = null)
        {
            _resultDialog.Show(won ? WinTitle : LoseTitle, score, detail, onRetry, onTitle: BackToTitle);
            _resultBanner.Play(won);
        }

        /// <summary>タイトル専用のシーンは無いので、シーンを読み直して最初（タイトル）から始め直す</summary>
        private static void BackToTitle()
        {
            SceneManager.LoadScene(SceneNames.Title);
        }
    }
}
