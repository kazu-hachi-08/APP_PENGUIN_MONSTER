using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Common.Scene;
using PenguinMonster.Common.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PenguinMonster.View
{
    /// <summary>
    /// 画面の流れ（仕様書 §2）: タイトル → ドラフト → 見せ合い・選出 → バトル → 結果 → タイトル。
    /// 各画面は自分の表示と入力だけを持ち、次へ進む順番はここで決める
    /// </summary>
    public class GameFlow : MonoBehaviour
    {
        private const string ResultTitle = "けっか";
        private const string WinText = "かち！";
        private const string LoseText = "まけ…";
        private const string WinDetail = "あいての ペンモンを ぜんぶ たおした！";
        private const string LoseDetail = "じぶんの ペンモンが ぜんぶ たおれた…";

        [SerializeField] private TitlePanel _titlePanel;
        [SerializeField] private DraftPanel _draftPanel;
        [SerializeField] private SelectPanel _selectPanel;
        [SerializeField] private BattleController _battle;
        [SerializeField] private ResultDialog _resultDialog;

        private void Start()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            bool started = false;
            _titlePanel.Show(() => started = true);
            yield return new WaitUntil(() => started);

            var random = new System.Random();
            var cpu = new CpuPlayer(random);

            IReadOnlyList<MonsterDefinition> playerSix = null;
            _draftPanel.Show(new Draft(MonsterTable.All, random), six => playerSix = six);
            yield return new WaitUntil(() => playerSix != null);
            var cpuSix = cpu.RunDraft(new Draft(MonsterTable.All, random));

            List<MonsterDefinition> playerTeam = null;
            _selectPanel.Show(playerSix, cpuSix, team => playerTeam = team);
            yield return new WaitUntil(() => playerTeam != null);

            yield return _battle.Run(playerTeam, cpu.ChooseTeam(cpuSix), random, cpu);
            ShowResult(_battle.PlayerWon);
        }

        private void ShowResult(bool won)
        {
            // タイトル専用のシーンは無いので、シーンを読み直して最初（タイトル）から始め直す
            _resultDialog.Show(ResultTitle, won ? WinText : LoseText, won ? WinDetail : LoseDetail,
                onTitle: () => SceneManager.LoadScene(SceneNames.Title));
        }
    }
}
