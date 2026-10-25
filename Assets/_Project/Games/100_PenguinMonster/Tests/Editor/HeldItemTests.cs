using NUnit.Framework;
using PenguinMonster.Battle;
using static PenguinMonster.Story.Tests.StoryTestSpecies;

namespace PenguinMonster.Story.Tests
{
    /// <summary>ストーリーの持たせる道具（PLAN_STORY_TOWN04.md フェーズ2）</summary>
    public class HeldItemTests
    {
        private StoryState _state;
        private OwnedMonster _member;

        [SetUp]
        public void SetUp()
        {
            _state = new StoryState(Town01Maps.Start);
            _member = Owned(Chick, 10);
            _state.Party.Add(_member);
        }

        [Test]
        public void Give_TakesOneFromBagAndHolds()
        {
            _state.Bag.Add(StoryItem.HeldDriedFish, 2);

            Assert.IsTrue(_state.Party.GiveItem(0, StoryItem.HeldDriedFish, _state.Bag));

            Assert.AreEqual(HeldItem.StarFish, _member.Item);
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldDriedFish));
        }

        [Test]
        public void Give_WhenHolding_SwapsAndReturnsPreviousToBag()
        {
            _state.Bag.Add(StoryItem.HeldDriedFish);
            _state.Bag.Add(StoryItem.HeldBraveFeather);
            _state.Party.GiveItem(0, StoryItem.HeldDriedFish, _state.Bag);

            Assert.IsTrue(_state.Party.GiveItem(0, StoryItem.HeldBraveFeather, _state.Bag));

            Assert.AreEqual(HeldItem.BraveFeather, _member.Item);
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldDriedFish));
            Assert.AreEqual(0, _state.Bag.CountOf(StoryItem.HeldBraveFeather));
        }

        [Test]
        public void Give_NotInBagOrNotHeldItem_IsRefused()
        {
            _state.Bag.Add(StoryItem.Potion);

            Assert.IsFalse(_state.Party.GiveItem(0, StoryItem.HeldWiseFeather, _state.Bag));
            Assert.IsFalse(_state.Party.GiveItem(0, StoryItem.Potion, _state.Bag));

            Assert.AreEqual(HeldItem.None, _member.Item);
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.Potion));
        }

        [Test]
        public void Take_ReturnsItemToBag()
        {
            _state.Bag.Add(StoryItem.HeldFreshKelp);
            _state.Party.GiveItem(0, StoryItem.HeldFreshKelp, _state.Bag);

            Assert.IsTrue(_state.Party.TakeItem(0, _state.Bag));

            Assert.AreEqual(HeldItem.None, _member.Item);
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldFreshKelp));
            Assert.IsFalse(_state.Party.TakeItem(0, _state.Bag), "持っていなければ外せない");
        }

        [Test]
        public void Take_ItemNotInStory_IsKeptOnMonster()
        {
            // ストーリーの道具に無い持ち物は、外すとバッグに戻せず消えてしまうので外さない
            _member.Item = HeldItem.LifeIcicle;

            Assert.IsFalse(_state.Party.TakeItem(0, _state.Bag));

            Assert.AreEqual(HeldItem.LifeIcicle, _member.Item);
        }

        [Test]
        public void BattleCopy_HoldsTheItem()
        {
            _state.Bag.Add(StoryItem.HeldWiseFeather);
            _state.Party.GiveItem(0, StoryItem.HeldWiseFeather, _state.Bag);

            var battle = new StoryBattle(_state.Party, new[] { Owned(Chick, 3) }, isTrainer: false);

            Assert.IsTrue(battle.State.Sides[StoryBattle.PlayerSide].Active.HasItem(HeldItem.WiseFeather));
        }

        [Test]
        public void OneTimeItem_UsedInBattle_StaysOnMonsterAfterBattle()
        {
            _state.Bag.Add(StoryItem.HeldSquidSnack);
            _state.Party.GiveItem(0, StoryItem.HeldSquidSnack, _state.Bag);
            var battle = new StoryBattle(_state.Party, new[] { Owned(Chick, 3) }, isTrainer: false);
            var fighter = battle.State.Sides[StoryBattle.PlayerSide].Active;

            fighter.UseItem();
            battle.ApplyResults();

            Assert.IsFalse(fighter.HasItem(HeldItem.SquidSnack));
            Assert.AreEqual(HeldItem.SquidSnack, _member.Item);
            var next = new StoryBattle(_state.Party, new[] { Owned(Chick, 3) }, isTrainer: false);
            Assert.IsTrue(next.State.Sides[StoryBattle.PlayerSide].Active.HasItem(HeldItem.SquidSnack), "次のバトルではまた使える");
        }

        [Test]
        public void TrainerMon_ItemIsGivenOnlyWhenWritten()
        {
            var trainer = new TrainerDefinition("test.trainer", "テスト", new[]
            {
                new TrainerMon(Chick, 5),
                new TrainerMon(Chick, 6, HeldItem.WiseFeather),
            }, prize: 100, sightRange: 0);

            var party = trainer.CreateParty();

            Assert.AreEqual(HeldItem.None, party[0].Item);
            Assert.AreEqual(HeldItem.WiseFeather, party[1].Item);
        }

        [Test]
        public void HeldItems_CannotBeSoldOrBoughtAndMapBackFromHeldItem()
        {
            foreach (var definition in StoryItems.Definitions)
            {
                if (!definition.IsHeldItem)
                {
                    continue;
                }
                Assert.IsFalse(definition.CanSell, definition.Name);
                Assert.IsTrue(StoryItems.TryFromHeld(definition.Held, out var item), definition.Name);
                Assert.AreEqual(definition.Item, item);
                Assert.AreEqual(definition.Held.ToJapanese(), definition.Name);
            }
        }

        [Test]
        public void Sell_HeldItem_IsRefused()
        {
            _state.Bag.Add(StoryItem.HeldBraveFeather);
            int before = _state.Money;

            Assert.IsFalse(Shop.Sell(_state, StoryItem.HeldBraveFeather));

            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldBraveFeather));
            Assert.AreEqual(before, _state.Money);
        }

        [Test]
        public void HeldItem_CannotBeUsedAsMedicine()
        {
            Hurt(_member, 5);
            _state.Bag.Add(StoryItem.HeldDriedFish);

            Assert.IsFalse(_state.UseItem(StoryItem.HeldDriedFish, _member));
            Assert.AreEqual(1, _state.Bag.CountOf(StoryItem.HeldDriedFish));
        }
    }
}
