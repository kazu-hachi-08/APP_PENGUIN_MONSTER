using System;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 定義表の形のチェック（仕様書 §3.5 の段階2）。ランクの帯は見るが、細かいバランスは検証ツールと遊んで決める
    /// </summary>
    public class MonsterTableTests
    {
        private const int Stage2Count = 50;

        [Test]
        public void 段階2は50体で_Noは1から順に並ぶ()
        {
            Assert.AreEqual(Stage2Count, MonsterTable.All.Count);
            CollectionAssert.AreEqual(Enumerable.Range(1, Stage2Count), MonsterTable.All.Select(m => m.No));
        }

        [TestCase(MonsterRank.Hina, 6, 360, 400)]
        [TestCase(MonsterRank.Normal, 36, 425, 475)]
        [TestCase(MonsterRank.Ace, 5, 520, 540)]
        [TestCase(MonsterRank.Legend, 3, 580, 600)]
        public void ランクごとの数と種族値の合計の帯(MonsterRank rank, int count, int minTotal, int maxTotal)
        {
            var ranked = MonsterTable.All.Where(m => m.Rank == rank).ToList();

            Assert.AreEqual(count, ranked.Count);
            foreach (var monster in ranked)
            {
                Assert.That(monster.BaseStats.Total, Is.InRange(minTotal, maxTotal), monster.Name);
            }
        }

        [Test]
        public void ヒナの綿毛はヒナだけが持つ()
        {
            foreach (var monster in MonsterTable.All.Where(m => m.Item == HeldItem.ChickDown))
            {
                Assert.AreEqual(MonsterRank.Hina, monster.Rank, monster.Name);
            }
        }

        [Test]
        public void 名前が重複しない()
        {
            var names = MonsterTable.All.Select(m => m.Name).ToList();
            Assert.AreEqual(names.Count, names.Distinct().Count());
        }

        [Test]
        public void 技は4つで重複しない()
        {
            foreach (var monster in MonsterTable.All)
            {
                Assert.AreEqual(4, monster.Moves.Count, monster.Name);
                Assert.AreEqual(4, monster.Moves.Distinct().Count(), monster.Name);
            }
        }

        [Test]
        public void どのタイプも6体以上いて_こおりが一番多い()
        {
            var counts = Enum.GetValues(typeof(MonsterType)).Cast<MonsterType>()
                .ToDictionary(t => t, t => MonsterTable.All.Count(m => m.HasType(t)));

            foreach (var pair in counts)
            {
                Assert.That(pair.Value, Is.GreaterThanOrEqualTo(6), pair.Key.ToString());
            }
            int mostOfOthers = counts.Where(p => p.Key != MonsterType.Ice).Max(p => p.Value);
            Assert.That(counts[MonsterType.Ice], Is.GreaterThan(mostOfOthers));
        }

        [Test]
        public void 使われている技は50から60個()
        {
            int moveCount = MonsterTable.All.SelectMany(m => m.Moves).Distinct().Count();
            Assert.That(moveCount, Is.InRange(50, 60));
        }

        [Test]
        public void 変化技は威力0で効果を持つ()
        {
            foreach (var move in MonsterTable.All.SelectMany(m => m.Moves).Where(m => m.IsStatus).Distinct())
            {
                Assert.AreEqual(0, move.Power, move.Name);
                Assert.That(move.Effects.Count, Is.GreaterThan(0), move.Name);
            }
        }
    }
}
