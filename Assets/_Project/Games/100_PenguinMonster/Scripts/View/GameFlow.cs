using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Common.Profile;
using PenguinMonster.Common.UI;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// 画面の流れ: タイトル → シングル / ダブルス → ドラフト → 持ち物 →
    /// [何戦目か・ボス登場 → 見せ合い・選出 → バトル → 結果] を負けるまでくり返す → タイトル。
    /// 各画面は自分の表示と入力だけを持ち、次へ進む順番はここで決める。
    /// 結果の文面は GameFlow.Result.cs、オンライン対戦の流れは GameFlow.Online.cs、ストーリーの流れは GameFlow.Story.cs
    /// </summary>
    public partial class GameFlow : MonoBehaviour
    {
        /// <summary>最高連勝数の保存キー（共通基盤の BestScore）。シングルは従来と同じキーのまま（記録を引き継ぐため）、ダブルスは別に数える</summary>
        private const string BestWinsKey = "PenguinMonster.SurvivalWins";
        private const string BestDoubleWinsKey = "PenguinMonster.SurvivalWins.Double";
        private const string SurvivalQuitMessage = "勝ち抜き戦は ここで 終わります\n（最高記録は 残ります）";

        [SerializeField] private TitlePanel _titlePanel;
        [SerializeField] private FormatPanel _formatPanel;
        [SerializeField] private DraftPanel _draftPanel;
        [SerializeField] private ItemPanel _itemPanel;
        [SerializeField] private BattleIntroPanel _introPanel;
        [SerializeField] private SelectPanel _selectPanel;
        [SerializeField] private BattleController _battle;
        [SerializeField] private ResultDialog _resultDialog;
        [SerializeField] private ResultBanner _resultBanner;
        [SerializeField] private QuitButton _quitButton;

        private void Start()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            bool story = false;
            bool survival = false;
            bool online = false;
            // 図鑑・設定などタイトルから開く画面と、シングル / ダブルスを選ぶまではタイトルの曲のまま
            GameMusic.Play(MusicTrack.Title);
            _titlePanel.Show(BestScore.Get(BestWinsKey), BestScore.Get(BestDoubleWinsKey),
                onStory: () => story = true, onSurvival: () => survival = true, onOnline: () => online = true);
            yield return new WaitUntil(() => story || survival || online);

            if (story)
            {
                yield return RunStory();
            }
            else
            {
                yield return online ? RunOnline() : RunSurvival();
            }
        }

        private static string BestKey(BattleFormat format) => format == BattleFormat.Double ? BestDoubleWinsKey : BestWinsKey;

        private IEnumerator RunSurvival()
        {
            BattleFormat? format = null;
            _formatPanel.Show(chosen => format = chosen, onBack: BackToTitle);
            yield return new WaitUntil(() => format.HasValue);
            int bestAtStart = BestScore.Get(BestKey(format.Value));

            var random = new System.Random();
            // 最高連勝数は勝つたびに保存済みなので、やめるときはそのままタイトルへ戻すだけでよい
            _quitButton.Show(SurvivalQuitMessage, BackToTitle);
            IReadOnlyList<MonsterDefinition> playerSix = null;
            GameMusic.Play(MusicTrack.Draft);
            _draftPanel.Show(new Draft(MonsterTable.All, random), six => playerSix = six);
            yield return new WaitUntil(() => playerSix != null);
            // 持ち物は最初の1回だけ。チームを途中で変えないので、持ち主も最後まで同じ
            List<MonsterDefinition> withItem = null;
            _itemPanel.Show(playerSix, ItemDraw.Draw(random), six => withItem = six);
            yield return new WaitUntil(() => withItem != null);

            // 途中経過は保存しない。アプリを閉じたらその回はおしまい
            var run = new SurvivalRun(withItem, random, format.Value);
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
                BestScore.Submit(BestKey(run.Format), run.Wins);
                yield return ShowWin(run, opponent);
            }
            ShowLose(run, bestAtStart);
        }

        private IEnumerator PlayOneBattle(SurvivalRun run, SurvivalOpponent opponent, System.Random random)
        {
            bool introDone = false;
            // 2戦目からは決着で止めた曲を、何戦目かの画面から考える曲で流し直す
            GameMusic.Play(MusicTrack.Draft);
            _introPanel.Show(run, opponent, () => introDone = true);
            yield return new WaitUntil(() => introDone);

            int partySize = run.Format.PartySize();
            List<MonsterDefinition> playerTeam = null;
            _selectPanel.Show(run.PlayerSix, opponent.Six, partySize, team => playerTeam = team);
            yield return new WaitUntil(() => playerTeam != null);

            var cpuTeam = opponent.Cpu.ChooseTeam(opponent.Six, opponent.TrumpCard, partySize);
            GameMusic.Play(MusicTrack.Battle);
            yield return _battle.Run(playerTeam, cpuTeam, random, opponent.Cpu, run.Format);
        }
    }
}
