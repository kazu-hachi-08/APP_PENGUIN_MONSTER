using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Common.Profile;
using PenguinMonster.Common.UI;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 画面の流れ（仕様書 §2 / §2.3）: タイトル → ドラフト →
    /// [何戦目か・ボス登場 → 見せ合い・選出 → バトル → 結果] を負けるまでくり返す → タイトル。
    /// 各画面は自分の表示と入力だけを持ち、次へ進む順番はここで決める。
    /// 結果の文面は GameFlow.Result.cs、オンライン対戦の流れは GameFlow.Online.cs
    /// </summary>
    public partial class GameFlow : MonoBehaviour
    {
        /// <summary>最高連勝数の保存キー（共通基盤の BestScore）</summary>
        private const string BestWinsKey = "PenguinMonster.SurvivalWins";

        [SerializeField] private TitlePanel _titlePanel;
        [SerializeField] private DraftPanel _draftPanel;
        [SerializeField] private BattleIntroPanel _introPanel;
        [SerializeField] private SelectPanel _selectPanel;
        [SerializeField] private BattleController _battle;
        [SerializeField] private ResultDialog _resultDialog;

        private void Start()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            int bestAtStart = BestScore.Get(BestWinsKey);
            bool survival = false;
            bool online = false;
            _titlePanel.Show(bestAtStart, onSurvival: () => survival = true, onOnline: () => online = true);
            yield return new WaitUntil(() => survival || online);

            yield return online ? RunOnline() : RunSurvival(bestAtStart);
        }

        private IEnumerator RunSurvival(int bestAtStart)
        {
            var random = new System.Random();
            IReadOnlyList<MonsterDefinition> playerSix = null;
            _draftPanel.Show(new Draft(MonsterTable.All, random), six => playerSix = six);
            yield return new WaitUntil(() => playerSix != null);

            // 途中経過は保存しない（仕様書 §9）。アプリを閉じたらその回はおしまい
            var run = new SurvivalRun(playerSix, random);
            while (true)
            {
                var opponent = run.NextOpponent();
                yield return PlayOneBattle(run, opponent, random);
                if (!_battle.PlayerWon)
                {
                    break;
                }
                run.RecordWin();
                // 勝つたびに保存しておけば、途中でタイトルへ戻っても記録は残る
                BestScore.Submit(BestWinsKey, run.Wins);
                yield return ShowWin(run, opponent);
            }
            ShowLose(run, bestAtStart);
        }

        private IEnumerator PlayOneBattle(SurvivalRun run, SurvivalOpponent opponent, System.Random random)
        {
            bool introDone = false;
            _introPanel.Show(run, opponent, () => introDone = true);
            yield return new WaitUntil(() => introDone);

            List<MonsterDefinition> playerTeam = null;
            _selectPanel.Show(run.PlayerSix, opponent.Six, team => playerTeam = team);
            yield return new WaitUntil(() => playerTeam != null);

            yield return _battle.Run(playerTeam, opponent.Cpu.ChooseTeam(opponent.Six), random, opponent.Cpu);
        }
    }
}
