using System;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 定義表の形のチェック（仕様書 §3.5 の段階1）。数値のバランスは遊んで決めるので見ない
    /// </summary>
    public class MonsterTableTests
    {
        private const int Stage1Count = 24;

        [Test]
        public void 段階1は24体で_Noは1から順に並ぶ()
        {
            Assert.AreEqual(Stage1Count, MonsterTable.All.Count);
            CollectionAssert.AreEqual(Enumerable.Range(1, Stage1Count), MonsterTable.All.Select(m => m.No));
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
        public void どのタイプも2体以上いて_こおりが一番多い()
        {
            var counts = Enum.GetValues(typeof(MonsterType)).Cast<MonsterType>()
                .ToDictionary(t => t, t => MonsterTable.All.Count(m => m.HasType(t)));

            foreach (var pair in counts)
            {
                Assert.That(pair.Value, Is.GreaterThanOrEqualTo(2), pair.Key.ToString());
            }
            int mostOfOthers = counts.Where(p => p.Key != MonsterType.Ice).Max(p => p.Value);
            Assert.That(counts[MonsterType.Ice], Is.GreaterThan(mostOfOthers));
        }

        [Test]
        public void 使われている技は40から50個()
        {
            int moveCount = MonsterTable.All.SelectMany(m => m.Moves).Distinct().Count();
            Assert.That(moveCount, Is.InRange(40, 50));
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
