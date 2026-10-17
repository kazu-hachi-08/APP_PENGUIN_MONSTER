using System;
using System.Linq;
using NUnit.Framework;
using PenguinMonster.Battle;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    /// <summary>ストーリーの1戦（PLAN_STORY_TOWN01.md フェーズ5）: 手持ち全員で戦い、傷を持ち帰り、出た子だけで経験値を分ける</summary>
    public class StoryBattleTests
    {
        private static Party PartyOf(params OwnedMonster[] members)
        {
            var party = new Party();
            foreach (var member in members)
            {
                party.Add(member);
            }
            return party;
        }

        private static OwnedMonster Fainted(StorySpecies species, int level)
        {
            var owned = Owned(species, level);
            Hurt(owned, owned.MaxHp);
            return owned;
        }

        [Test]
        public void Start_SkipsFaintedMemberAndKeepsHp()
        {
            var hurt = Owned(Chick, 10);
            Hurt(hurt, 5);
            var battle = new StoryBattle(PartyOf(Fainted(Chick, 10), hurt), new[] { Owned(Chick, 3) }, isTrainer: false);

            var player = battle.State.Sides[StoryBattle.PlayerSide];
            Assert.AreEqual(1, player.ActivePartyIndex);
            Assert.AreEqual(hurt.MaxHp - 5, player.Active.CurrentHp);
            Assert.AreEqual(2, player.Party.Count);
        }

        [Test]
        public void ApplyResults_BringsBackDamageAndStatus()
        {
            var owned = Owned(Chick, 10);
            var battle = new StoryBattle(PartyOf(owned), new[] { Owned(Chick, 3) }, isTrainer: false);
            var fighter = battle.State.Sides[StoryBattle.PlayerSide].Active;
            fighter.TakeDamage(7);
            fighter.SetStatus(StatusCondition.Burn, 0);

            battle.ApplyResults();

            Assert.AreEqual(owned.MaxHp - 7, owned.CurrentHp);
            Assert.AreEqual(StatusCondition.Burn, owned.Status);
        }

        [Test]
        public void ExpRewards_GoOnlyToMembersWhoFacedTheFoe()
        {
            var first = Owned(Chick, 10);
            var bench = Owned(Chick, 10);
            var wild = Owned(Chick, 5);
            var battle = new StoryBattle(PartyOf(first, bench), new[] { wild }, isTrainer: false);
            battle.State.Sides[StoryBattle.FoeSide].Active.TakeDamage(wild.MaxHp);

            var rewards = battle.ExpRewards();

            Assert.AreEqual(1, rewards.Count);
            Assert.AreEqual(first, rewards[0].Member);
            Assert.AreEqual(Experience.Defeated(Chick.BaseExp, 5, isTrainer: false), rewards[0].Exp);
        }

        [Test]
        public void ExpRewards_SplitBetweenSurvivorsAndSkipFainted()
        {
            var first = Owned(Chick, 10);
            var second = Owned(Chick, 10);
            var third = Owned(Chick, 10);
            var battle = new StoryBattle(PartyOf(first, second, third), new[] { Owned(Junior, 20) }, isTrainer: true);
            var engine = new BattleEngine(battle.State, new Random(1));
            var player = battle.State.Sides[StoryBattle.PlayerSide];
            // 1体目が倒れ、2体目・3体目と順に向き合ってから相手を倒す
            player.Active.TakeDamage(first.MaxHp);
            engine.Replace(StoryBattle.PlayerSide, 1);
            battle.RecordFacing();
            engine.Replace(StoryBattle.PlayerSide, 2);
            battle.RecordFacing();
            battle.State.Sides[StoryBattle.FoeSide].Active.TakeDamage(999);

            var rewards = battle.ExpRewards();

            int share = Experience.Defeated(Junior.BaseExp, 20, isTrainer: true) / 2;
            CollectionAssert.AreEqual(new[] { second, third }, rewards.Select(r => r.Member).ToArray());
            Assert.IsTrue(rewards.All(r => r.Exp == share));
        }

        [Test]
        public void ExpRewards_NoneWhileFoeStands()
        {
            var battle = new StoryBattle(PartyOf(Owned(Chick, 10)), new[] { Owned(Chick, 5) }, isTrainer: false);

            Assert.AreEqual(0, battle.ExpRewards().Count);
        }

        [Test]
        public void TryFlee_FasterAlwaysEscapes()
        {
            var battle = new StoryBattle(PartyOf(Owned(Adult, 50)), new[] { Owned(Chick, 2) }, isTrainer: false);

            Assert.IsTrue(battle.TryFlee(new Random(1)));
        }

        [Test]
        public void TryFlee_FromTrainerFails()
        {
            var battle = new StoryBattle(PartyOf(Owned(Adult, 50)), new[] { Owned(Chick, 2) }, isTrainer: true);

            Assert.IsFalse(battle.TryFlee(new Random(1)));
        }

        [Test]
        public void TryFlee_SlowerEscapesAfterEnoughTries()
        {
            var battle = new StoryBattle(PartyOf(Owned(Chick, 2)), new[] { Owned(Adult, 50) }, isTrainer: false);
            var random = new Random(1);

            // 試すたびに 30 ずつ足されるので、9回目には 256 を超えて必ず逃げられる
            bool escaped = false;
            for (int i = 0; i < 9 && !escaped; i++)
            {
                escaped = battle.TryFlee(random);
            }
            Assert.IsTrue(escaped);
        }

        [Test]
        public void SkipAction_OnlyFoeActs()
        {
            var owned = Owned(Chick, 10);
            var battle = new StoryBattle(PartyOf(owned), new[] { Owned(Chick, 10) }, isTrainer: false);
            var engine = new BattleEngine(battle.State, new Random(1));

            var events = engine.ResolveTurn(BattleAction.Skip(), BattleAction.UseMove(0));

            var used = events.Where(e => e.Kind == BattleEventKind.MoveUsed).ToList();
            Assert.AreEqual(1, used.Count);
            Assert.AreEqual(StoryBattle.FoeSide, used[0].Side);
        }

        [Test]
        public void SeenNos_OnlyFoesThatCameOut()
        {
            var battle = new StoryBattle(PartyOf(Owned(Chick, 10)), new[] { Owned(Chick, 3), Owned(Junior, 3) }, isTrainer: true);

            CollectionAssert.AreEquivalent(new[] { Chick.No }, battle.SeenNos);
        }

        [Test]
        public void Receive_MarksCaughtAndSeen()
        {
            var state = new StoryState(Town01Maps.Start);

            state.Receive(Owned(Chick, 5));
            state.MarkSeen(Junior.No);

            CollectionAssert.AreEquivalent(new[] { Chick.No, Junior.No }, state.SeenNos);
            CollectionAssert.AreEquivalent(new[] { Chick.No }, state.CaughtNos);
        }

        [Test]
        public void BlackOut_HalvesMoneyHealsAndReturnsHome()
        {
            var state = new StoryState(Town01Maps.Start);
            var owned = Owned(Chick, 10);
            state.Party.Add(owned);
            Hurt(owned, owned.MaxHp);
            state.EarnMoney(1);

            var spot = state.BlackOut();

            Assert.AreEqual((StoryBalance.StartMoney + 1) / 2, state.Money);
            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(Town01Maps.Start.MapId, spot.MapId);
            Assert.AreEqual(Town01Maps.Start.Position, spot.Position);
        }
    }
}
