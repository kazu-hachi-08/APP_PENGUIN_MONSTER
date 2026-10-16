using System;
using NUnit.Framework;
using PenguinMonster.Battle;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    /// <summary>バッグ・道具・捕獲・あずかり箱・よろず屋（PLAN_STORY_TOWN01.md フェーズ6）</summary>
    public class ItemCaptureTests
    {
        private const int CatchRate = 45;

        private StoryState _state;

        [SetUp]
        public void SetUp()
        {
            _state = new StoryState(Town01Maps.Start);
        }

        private static OwnedMonster Fainted(StorySpecies species, int level)
        {
            var owned = Owned(species, level);
            Hurt(owned, owned.MaxHp);
            return owned;
        }

        private static OwnedMonster Poisoned(StorySpecies species, int level)
        {
            var owned = Owned(species, level);
            var fighter = owned.ToBattle();
            fighter.SetStatus(StatusCondition.Poison, 0);
            owned.ApplyBattleResult(fighter);
            return owned;
        }

        private StoryBattle WildBattle(OwnedMonster wild)
        {
            return new StoryBattle(_state.Party, new[] { wild }, isTrainer: false);
        }

        // --- バッグ ---

        [Test]
        public void Bag_AddRemoveAndListsOnlyHeldItemsInOrder()
        {
            var bag = new Bag();
            bag.Add(StoryItem.Antidote);
            bag.Add(StoryItem.CaptureEgg, 2);

            Assert.AreEqual(new[] { StoryItem.CaptureEgg, StoryItem.Antidote }, bag.Items());
            Assert.IsTrue(bag.Remove(StoryItem.Antidote));
            Assert.IsFalse(bag.Remove(StoryItem.Antidote));
            Assert.AreEqual(new[] { StoryItem.CaptureEgg }, bag.Items());
        }

        // --- よろず屋 ---

        [Test]
        public void Buy_WithEnoughMoney_PaysAndAddsToBag()
        {
            int before = _state.Money;

            Assert.IsTrue(Shop.Buy(_state, StoryItem.Potion));

            Assert.AreEqual(before - StoryItems.Get(StoryItem.Potion).Price, _state.Money);
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.Potion));
        }

        [Test]
        public void Buy_WithoutEnoughMoney_DoesNothing()
        {
            _state.SpendMoney(_state.Money - 1);
            _state.EarnMoney(StoryItems.Get(StoryItem.Potion).Price - 2);
            int before = _state.Money;

            Assert.IsFalse(Shop.Buy(_state, StoryItem.Potion));

            Assert.AreEqual(before, _state.Money);
            Assert.AreEqual(0, _state.Bag.CountOf(StoryItem.Potion));
        }

        [Test]
        public void Sell_GivesHalfPrice_AndNeedsTheItem()
        {
            int before = _state.Money;
            Assert.IsFalse(Shop.Sell(_state, StoryItem.Potion));

            _state.Bag.Add(StoryItem.Potion);
            Assert.IsTrue(Shop.Sell(_state, StoryItem.Potion));

            Assert.AreEqual(before + StoryItems.Get(StoryItem.Potion).Price / 2, _state.Money);
            Assert.AreEqual(0, _state.Bag.CountOf(StoryItem.Potion));
        }

        // --- 道具を使う ---

        [Test]
        public void Potion_HealsUpToMaxHp()
        {
            var owned = Owned(Chick, 10);
            _state.Party.Add(owned);
            Hurt(owned, 5);
            _state.Bag.Add(StoryItem.Potion);

            Assert.IsTrue(_state.UseItem(StoryItem.Potion, owned));

            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
            Assert.AreEqual(0, _state.Bag.CountOf(StoryItem.Potion));
        }

        [Test]
        public void Potion_OnFullHpOrFainted_IsNotUsed()
        {
            var full = Owned(Chick, 10);
            var fainted = Fainted(Chick, 10);
            _state.Bag.Add(StoryItem.Potion);

            Assert.IsFalse(_state.UseItem(StoryItem.Potion, full));
            Assert.IsFalse(_state.UseItem(StoryItem.Potion, fainted));

            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.Potion));
            Assert.AreEqual(0, fainted.CurrentHp);
        }

        [Test]
        public void Antidote_CuresPoisonButNotOtherStatus()
        {
            var poisoned = Poisoned(Chick, 10);
            _state.Bag.Add(StoryItem.Antidote);
            _state.Bag.Add(StoryItem.ParalyzeHeal);

            Assert.IsFalse(_state.UseItem(StoryItem.ParalyzeHeal, poisoned));
            Assert.IsTrue(_state.UseItem(StoryItem.Antidote, poisoned));

            Assert.AreEqual(StatusCondition.None, poisoned.Status);
        }

        [Test]
        public void CaptureEgg_CannotBeUsedOnPartyMember()
        {
            var owned = Owned(Chick, 10);
            Hurt(owned, 5);
            _state.Bag.Add(StoryItem.CaptureEgg);

            Assert.IsFalse(_state.UseItem(StoryItem.CaptureEgg, owned));
        }

        [Test]
        public void UseMedicineInBattle_HealsFighterAndIsBroughtBack()
        {
            var owned = Owned(Chick, 10);
            _state.Party.Add(owned);
            Hurt(owned, 5);
            _state.Bag.Add(StoryItem.Potion);
            var battle = WildBattle(Owned(Chick, 3));

            Assert.IsTrue(battle.UseMedicine(_state.Bag, StoryItem.Potion, 0));
            Assert.IsFalse(battle.CanTreat(StoryItem.Potion, 0));
            battle.ApplyResults();

            Assert.AreEqual(owned.MaxHp, owned.CurrentHp);
        }

        // --- 捕獲 ---

        [Test]
        public void CaptureOdds_RiseAsHpFalls()
        {
            int full = Capture.Odds(30, 30, CatchRate, StatusCondition.None);
            int half = Capture.Odds(30, 15, CatchRate, StatusCondition.None);
            int low = Capture.Odds(30, 1, CatchRate, StatusCondition.None);

            Assert.Less(full, half);
            Assert.Less(half, low);
            // 満タンなら 捕まえやすさの 1/3（本家と同じ）
            Assert.AreEqual(CatchRate / 3, full);
        }

        [Test]
        public void CaptureOdds_SleepBeatsParalysisBeatsNone()
        {
            int none = Capture.Odds(30, 15, CatchRate, StatusCondition.None);
            int paralysis = Capture.Odds(30, 15, CatchRate, StatusCondition.Paralysis);
            int sleep = Capture.Odds(30, 15, CatchRate, StatusCondition.Sleep);

            Assert.AreEqual(none * 3 / 2, paralysis);
            Assert.AreEqual(none * 2, sleep);
        }

        [Test]
        public void CaptureRoll_MaxOddsAlwaysCatchesWithThreeShakes()
        {
            var result = Capture.Roll(StoryBalance.CaptureMaxOdds, new Random(1));

            Assert.IsTrue(result.Caught);
            Assert.AreEqual(3, result.Shakes);
        }

        [Test]
        public void CaptureRoll_ZeroOddsNeverShakes()
        {
            var result = Capture.Roll(0, new Random(1));

            Assert.IsFalse(result.Caught);
            Assert.AreEqual(0, result.Shakes);
        }

        [Test]
        public void CaptureRoll_LowerOddsCatchLessOften()
        {
            Assert.Less(CatchCount(Capture.Odds(30, 30, CatchRate, StatusCondition.None)),
                CatchCount(Capture.Odds(30, 1, CatchRate, StatusCondition.Sleep)));
        }

        private static int CatchCount(int odds)
        {
            var random = new Random(7);
            int caught = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (Capture.Roll(odds, random).Caught)
                {
                    caught++;
                }
            }
            return caught;
        }

        [Test]
        public void ThrowEgg_InTrainerBattle_IsRefused()
        {
            _state.Party.Add(Owned(Chick, 10));
            _state.Bag.Add(StoryItem.CaptureEgg);
            var battle = new StoryBattle(_state.Party, new[] { Owned(Chick, 3) }, isTrainer: true);

            Assert.IsFalse(battle.CanThrowEgg);
            Assert.Throws<InvalidOperationException>(() => battle.ThrowEgg(_state.Bag, new Random(1)));
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.CaptureEgg));
        }

        [Test]
        public void ThrowEgg_Caught_KeepsBattleHpAndGoesToParty()
        {
            _state.Party.Add(Owned(Chick, 10));
            _state.Bag.Add(StoryItem.CaptureEgg);
            // テストの種は捕まえやすさ 255（既定）なので、HP1 で眠らせれば必ず捕まえる
            var wild = Owned(Chick, 3);
            var battle = WildBattle(wild);
            var fighter = battle.State.Sides[StoryBattle.FoeSide].Active;
            fighter.TakeDamage(wild.MaxHp - 1);
            fighter.SetStatus(StatusCondition.Sleep, 2);

            var result = battle.ThrowEgg(_state.Bag, new Random(1));

            Assert.IsTrue(result.Caught);
            Assert.AreEqual(wild, battle.Caught);
            Assert.AreEqual(1, wild.CurrentHp);
            Assert.AreEqual(StatusCondition.Sleep, wild.Status);
            Assert.AreEqual(0, _state.Bag.CountOf(StoryItem.CaptureEgg));
            Assert.IsTrue(_state.Receive(battle.Caught));
            Assert.AreEqual(2, _state.Party.Count);
        }

        // --- 手持ち・あずかり箱 ---

        [Test]
        public void Receive_WhenPartyFull_GoesToBox()
        {
            for (int i = 0; i < StoryBalance.MaxPartySize; i++)
            {
                _state.Receive(Owned(Chick, 5));
            }
            var seventh = Owned(Junior, 20);

            Assert.IsFalse(_state.Receive(seventh));

            Assert.AreEqual(StoryBalance.MaxPartySize, _state.Party.Count);
            Assert.AreEqual(seventh, _state.Box.Members[0]);
        }

        [Test]
        public void Deposit_LastFighter_IsRefused()
        {
            _state.Receive(Owned(Chick, 5));
            _state.Receive(Fainted(Chick, 5));

            Assert.IsFalse(_state.Deposit(0));
            Assert.IsTrue(_state.Deposit(1));

            Assert.AreEqual(1, _state.Party.Count);
            Assert.AreEqual(1, _state.Box.Count);
        }

        [Test]
        public void Withdraw_WhenPartyFull_IsRefused()
        {
            _state.Box.Add(Owned(Junior, 20));
            Assert.IsTrue(_state.Withdraw(0));
            _state.Box.Add(Owned(Junior, 20));
            for (int i = _state.Party.Count; i < StoryBalance.MaxPartySize; i++)
            {
                _state.Receive(Owned(Chick, 5));
            }

            Assert.IsFalse(_state.Withdraw(0));
            Assert.AreEqual(1, _state.Box.Count);
        }

        [Test]
        public void SwapWithBox_ExchangesMembers_ButKeepsAFighter()
        {
            var lead = Owned(Chick, 5);
            var boxed = Owned(Junior, 20);
            var faintedInBox = Fainted(Adult, 40);
            _state.Receive(lead);
            _state.Box.Add(boxed);
            _state.Box.Add(faintedInBox);

            Assert.IsFalse(_state.SwapWithBox(0, 1));
            Assert.IsTrue(_state.SwapWithBox(0, 0));

            Assert.AreEqual(boxed, _state.Party.Members[0]);
            Assert.AreEqual(lead, _state.Box.Members[0]);
        }

        [Test]
        public void PartySwap_ReordersMembers()
        {
            var first = Owned(Chick, 5);
            var second = Owned(Junior, 20);
            _state.Receive(first);
            _state.Receive(second);

            _state.Party.Swap(0, 1);

            Assert.AreEqual(second, _state.Party.Members[0]);
            Assert.AreEqual(first, _state.Party.Members[1]);
        }
    }
}
