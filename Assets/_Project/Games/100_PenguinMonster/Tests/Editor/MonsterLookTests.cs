using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    public class MonsterLookTests
    {
        [Test]
        public void 見た目を省略するとDefaultになる()
        {
            var monster = TestMonsters.Create("テスト", 50, TestMonsters.Strike);
            Assert.AreSame(MonsterLook.Default, monster.Look);
        }

        [Test]
        public void 定義表のペンモンは見た目の組み合わせがすべて違う()
        {
            var looks = MonsterTable.All.Select(m => (m.Look.Shape, m.Look.Color, m.Look.Head, m.Look.Neck)).ToList();
            Assert.AreEqual(looks.Count, looks.Distinct().Count());
        }
    }
}
