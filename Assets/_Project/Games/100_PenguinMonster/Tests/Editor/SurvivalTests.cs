using System;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 勝ち抜き戦（仕様書 §2.3）。連勝数での CPU の賢さ・ボスの順番・相手の作り方
    /// </summary>
    public class SurvivalTests
    {
        [TestCase(0, CpuLevel.Weak)]
        [TestCase(2, CpuLevel.Weak)]
        [TestCase(3, CpuLevel.Normal)]
        [TestCase(5, CpuLevel.Normal)]
        [TestCase(6, CpuLevel.Strong)]
        [TestCase(20, CpuLevel.Strong)]
        public void 連勝数で賢さが上がる(int wins, CpuLevel expected)
        {
            Assert.AreEqual(expected, SurvivalRun.LevelFor(wins, false));
        }

        [Test]
        public void ボスはいつもつよい()
        {
            Assert.AreEqual(CpuLevel.Strong, SurvivalRun.LevelFor(0, true));
        }

        [TestCase(7, true)]
        [TestCase(14, true)]
        [TestCase(21, true)]
        [TestCase(1, false)]
        [TestCase(6, false)]
        [TestCase(8, false)]
        public void ボスは7戦ごと(int battleNumber, bool expected)
        {
            Assert.AreEqual(expected, SurvivalRun.IsBoss(battleNumber));
        }

        [Test]
        public void ボスは順番に出て_最後の次は最初に戻る()
        {
            int interval = BattleBalance.BossInterval;
            for (int i = 0; i < BossTable.All.Count * 2; i++)
            {
                Assert.AreSame(BossTable.All[i % BossTable.All.Count], BossTable.ForBattle(interval * (i + 1)));
            }
        }

        [Test]
        public void ボスのチームは重複のない6体()
        {
            foreach (var boss in BossTable.All)
            {
                Assert.AreEqual(BattleBalance.DraftRounds, boss.Team.Distinct().Count(), boss.Name);
                CollectionAssert.IsSubsetOf(boss.Team, MonsterTable.All, boss.Name);
            }
        }

        [Test]
        public void ボスはチームに伝説の切り札を1体持つ()
        {
            foreach (var boss in BossTable.All)
            {
                CollectionAssert.Contains(boss.Team, boss.TrumpCard, boss.Name);
                Assert.AreEqual(MonsterRank.Legend, boss.TrumpCard.Rank, boss.Name);
                Assert.AreEqual(1, boss.Team.Count(m => m.Rank == MonsterRank.Legend), boss.Name);
            }
        }

        [Test]
        public void 切り札は必ず選出されて最後に出る()
        {
            var boss = BossTable.All[0];
            var cpu = new CpuPlayer(new Random(5));
            for (int i = 0; i < 100; i++)
            {
                var team = cpu.ChooseTeam(boss.Team, boss.TrumpCard);

                Assert.AreEqual(BattleBalance.PartySize, team.Distinct().Count());
                Assert.AreSame(boss.TrumpCard, team.Last());
                CollectionAssert.IsSubsetOf(team, boss.Team);
            }
        }

        [TestCase(0, 0)]
        [TestCase(5, 50)]
        [TestCase(30, 100)]
        public void 強く取る確率は連勝数で上がり100で止まる(int wins, int expected)
        {
            Assert.AreEqual(expected, SurvivalRun.StrongPickPercent(wins));
        }

        [Test]
        public void ふつうの相手はドラフトした6体_7戦目はボスのチーム()
        {
            var run = new SurvivalRun(MonsterTable.All.Take(BattleBalance.DraftRounds).ToList(), new Random(1));

            var first = run.NextOpponent();
            Assert.IsFalse(first.IsBoss);
            Assert.AreEqual(BattleBalance.DraftRounds, first.Six.Distinct().Count());
            Assert.AreEqual(CpuLevel.Weak, first.Cpu.Level);

            for (int i = 1; i < BattleBalance.BossInterval; i++)
            {
                run.RecordWin();
            }
            var boss = run.NextOpponent();
            Assert.AreEqual(BattleBalance.BossInterval, run.BattleNumber);
            Assert.IsTrue(boss.IsBoss);
            Assert.AreSame(BossTable.All[0].Team, boss.Six);
            Assert.AreEqual(CpuLevel.Strong, boss.Cpu.Level);
        }
    }
}
