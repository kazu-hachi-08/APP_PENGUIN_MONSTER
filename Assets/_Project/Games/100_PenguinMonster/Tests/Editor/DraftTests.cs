using System;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// ドラフトの抽選（仕様書 §2.1）と、CPU のドラフト・選出。乱数しだいなので種を変えて何度も回す
    /// </summary>
    public class DraftTests
    {
        private const int Trials = 200;

        [Test]
        public void 提示は3体で重複しない()
        {
            for (int seed = 0; seed < Trials; seed++)
            {
                var draft = new Draft(MonsterTable.All, new Random(seed));
                while (!draft.IsDone)
                {
                    Assert.AreEqual(BattleBalance.DraftOfferCount, draft.Offer.Count);
                    Assert.AreEqual(draft.Offer.Count, draft.Offer.Distinct().Count());
                    draft.Pick(seed % draft.Offer.Count);
                }
            }
        }

        [Test]
        public void 取ったペンモンは提示に出ない()
        {
            for (int seed = 0; seed < Trials; seed++)
            {
                var draft = new Draft(MonsterTable.All, new Random(seed));
                while (!draft.IsDone)
                {
                    CollectionAssert.IsEmpty(draft.Offer.Intersect(draft.Picked));
                    draft.Pick(0);
                }
            }
        }

        [Test]
        public void 回数ぶん取ると終わり_取ったペンモンは重複しない()
        {
            var draft = new Draft(MonsterTable.All, new Random(1));
            for (int i = 0; i < BattleBalance.DraftRounds; i++)
            {
                Assert.IsFalse(draft.IsDone);
                draft.Pick(0);
            }

            Assert.IsTrue(draft.IsDone);
            Assert.AreEqual(BattleBalance.DraftRounds, draft.Picked.Distinct().Count());
            CollectionAssert.IsEmpty(draft.Offer);
            Assert.Throws<InvalidOperationException>(() => draft.Pick(0));
        }

        [Test]
        public void 取ったのは提示されていたペンモン()
        {
            var draft = new Draft(MonsterTable.All, new Random(2));
            while (!draft.IsDone)
            {
                var chosen = draft.Offer[1];
                draft.Pick(1);
                Assert.AreSame(chosen, draft.Picked.Last());
            }
        }

        [Test]
        public void 全ペンモンが提示されうる()
        {
            var offered = Enumerable.Range(0, Trials)
                .SelectMany(seed => new Draft(MonsterTable.All, new Random(seed)).Offer)
                .Distinct();

            Assert.AreEqual(MonsterTable.All.Count, offered.Count());
        }

        [Test]
        public void ペンモンが足りないと作れない()
        {
            var tooFew = MonsterTable.All.Take(BattleBalance.DraftRounds + BattleBalance.DraftOfferCount - 2).ToList();

            Assert.Throws<ArgumentException>(() => new Draft(tooFew, new Random(1)));
        }

        [Test]
        public void CPUはドラフトを最後まで進める()
        {
            var random = new Random(3);
            var draft = new Draft(MonsterTable.All, random);

            var six = new CpuPlayer(random).RunDraft(draft);

            Assert.IsTrue(draft.IsDone);
            Assert.AreEqual(BattleBalance.DraftRounds, six.Distinct().Count());
        }

        [Test]
        public void CPUの選出は候補から3体で重複しない()
        {
            var six = MonsterTable.All.Take(BattleBalance.DraftRounds).ToList();
            var cpu = new CpuPlayer(new Random(4));
            for (int i = 0; i < Trials; i++)
            {
                var team = cpu.ChooseTeam(six);

                Assert.AreEqual(BattleBalance.PartySize, team.Distinct().Count());
                CollectionAssert.IsSubsetOf(team, six);
            }
        }
    }
}
