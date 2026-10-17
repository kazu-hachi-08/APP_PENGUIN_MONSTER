using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>オンラインの「自分のチーム」（PLAN_STORY_TOWN01.md フェーズ9）</summary>
    public class OnlineTeamTests
    {
        private static Party SixOf(params StorySpecies[] species)
        {
            var party = new Party();
            foreach (var one in species)
            {
                party.Add(OwnedMonster.Create(one, 7));
            }
            return party;
        }

        private static Party FullParty() => SixOf(Town01Species.Kobune, Town01Species.Kogara, Town01Species.Kogara,
            Town01Species.Hidane, Town01Species.Mebuki, Town01Species.Tsugumi);

        [Test]
        public void FullParty_CanBeUsed()
        {
            Assert.AreEqual(OnlineTeamProblem.None, OnlineTeam.Check(FullParty()));
        }

        [Test]
        public void FewerThanSix_CannotBeUsed()
        {
            var party = SixOf(Town01Species.Kobune, Town01Species.Kogara);

            Assert.AreEqual(OnlineTeamProblem.NotEnough, OnlineTeam.Check(party));
            Assert.AreEqual(OnlineTeamProblem.NotEnough, OnlineTeam.Check(null));
        }

        [Test]
        public void TwoLegends_CannotBeUsed()
        {
            var legends = MonsterTable.All.Where(m => m.Rank == MonsterRank.Legend).Take(2)
                .Select(m => new StorySpecies(m, GrowthRate.MediumSlow, 300, new[] { new LevelMove(1, m.Moves[0]) }))
                .ToArray();
            var party = SixOf(legends[0], legends[1], Town01Species.Kogara, Town01Species.Kogara,
                Town01Species.Kogara, Town01Species.Kogara);

            Assert.AreEqual(OnlineTeamProblem.TooManyLegends, OnlineTeam.Check(party));
        }

        [Test]
        public void Team_IsLevel50WithLearnedMoves()
        {
            var party = FullParty();

            var team = OnlineTeam.From(party);

            Assert.AreEqual(6, team.Count);
            for (int i = 0; i < team.Count; i++)
            {
                Assert.AreEqual(BattleBalance.Level, team[i].Level);
                Assert.AreEqual(party.Members[i].No, team[i].No);
                CollectionAssert.AreEqual(party.Members[i].Moves, team[i].Moves);
            }
        }

        /// <summary>覚えた技は MoveTable.All の番号で送るので、覚える技の表の技も全部その表に無いといけない</summary>
        [Test]
        public void EveryLearnableMoveHasAnIndex()
        {
            foreach (var species in Town01Species.All)
            {
                foreach (var entry in species.Learnset)
                {
                    Assert.GreaterOrEqual(MoveTable.IndexOf(entry.Move), 0, $"{species.Name} の {entry.Move.Name}");
                }
            }
        }

        [Test]
        public void Team_StartsFullyHealedAndLeavesThePartyAsIs()
        {
            var party = FullParty();
            var hurt = party.Members[0];
            StoryTestSpecies.Hurt(hurt, 5);
            int hpBefore = hurt.CurrentHp;

            var fighter = new BattleMonster(OnlineTeam.From(party)[0]);

            Assert.AreEqual(fighter.MaxHp, fighter.CurrentHp);
            Assert.AreEqual(StatusCondition.None, fighter.Status);
            Assert.AreEqual(7, hurt.Level);
            Assert.AreEqual(hpBefore, hurt.CurrentHp);
        }
    }
}
