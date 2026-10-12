using System;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// バランス確認用の検証ツール。数値の良し悪しではなく、集計の数え方を確かめる
    /// </summary>
    public class BalanceSimulatorTests
    {
        private const int Battles = 50;

        [Test]
        public void 試合ごとに両チーム3体ずつ数える()
        {
            var rates = BalanceSimulator.Run(MonsterTable.All, Battles, 1);

            Assert.AreEqual(MonsterTable.All.Count, rates.Count);
            Assert.AreEqual(Battles * BattleBalance.PartySize * 2, rates.Sum(r => r.Battles));
            Assert.That(rates.Sum(r => r.Wins), Is.LessThanOrEqualTo(Battles * BattleBalance.PartySize));
        }

        [Test]
        public void 同じ種なら同じ結果()
        {
            var first = BalanceSimulator.Run(MonsterTable.All, Battles, 7).Select(r => r.Wins);
            var second = BalanceSimulator.Run(MonsterTable.All, Battles, 7).Select(r => r.Wins);

            CollectionAssert.AreEqual(first, second);
        }

        [Test]
        public void 強いほうが勝つ()
        {
            var strong = Party("強", 90, Finisher, attack: 150);
            var weak = Party("弱", 30, Strike);

            Assert.AreEqual(0, BalanceSimulator.Play(strong, weak, new Random(1)));
        }
    }
}
