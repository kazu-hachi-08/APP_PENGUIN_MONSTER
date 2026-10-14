using System;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// ドラフトの後に配る持ち物（仕様書 §4.6）
    /// </summary>
    public class ItemDrawTests
    {
        [Test]
        public void 候補になしとヒナの綿毛は出ず_それ以外は全部入る()
        {
            CollectionAssert.DoesNotContain(ItemDraw.Candidates, HeldItem.None);
            CollectionAssert.DoesNotContain(ItemDraw.Candidates, HeldItem.ChickDown);
            int all = Enum.GetValues(typeof(HeldItem)).Length;
            Assert.AreEqual(all - BattleBalance.UndrawableItems.Length, ItemDraw.Candidates.Count);
        }

        [Test]
        public void 引いた持ち物はいつも候補の中から()
        {
            var random = new Random(1);
            for (int i = 0; i < 200; i++)
            {
                CollectionAssert.Contains(ItemDraw.Candidates, ItemDraw.Draw(random));
            }
        }

        [Test]
        public void Giveは指定の1体だけに持たせ_Noと技は同じ()
        {
            var six = MonsterTable.All.Take(BattleBalance.DraftRounds).ToList();
            const int holder = 2;

            var given = ItemDraw.Give(six, holder, HeldItem.StarFish);

            Assert.AreEqual(HeldItem.StarFish, given[holder].Item);
            Assert.AreEqual(six[holder].No, given[holder].No);
            CollectionAssert.AreEqual(six[holder].Moves, given[holder].Moves);
            Assert.AreEqual(six[holder].Rank, given[holder].Rank);
            for (int i = 0; i < six.Count; i++)
            {
                if (i != holder)
                {
                    Assert.AreSame(six[i], given[i]);
                }
            }
        }

        [Test]
        public void Giveは元のリストと定義表を変えない()
        {
            var six = MonsterTable.All.Take(BattleBalance.DraftRounds).ToList();

            ItemDraw.Give(six, 0, HeldItem.StarFish);

            Assert.AreSame(MonsterTable.All[0], six[0]);
            Assert.AreEqual(HeldItem.None, MonsterTable.All[0].Item);
        }
    }
}
