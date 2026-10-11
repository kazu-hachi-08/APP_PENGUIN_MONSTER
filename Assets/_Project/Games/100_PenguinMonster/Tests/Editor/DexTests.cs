using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// ずかんで使う しぼりこみ と 技の説明（Phase 8）
    /// </summary>
    public class DexTests
    {
        [Test]
        public void しぼりこみ無しなら全員()
        {
            Assert.AreSame(MonsterTable.All, MonsterTable.WithType(null));
        }

        [Test]
        public void タイプでしぼると_そのタイプを持つペンモンだけがNo順に並ぶ()
        {
            var ice = MonsterTable.WithType(MonsterType.Ice);

            Assert.That(ice.All(m => m.HasType(MonsterType.Ice)));
            Assert.AreEqual(MonsterTable.All.Count(m => m.HasType(MonsterType.Ice)), ice.Count);
            CollectionAssert.IsOrdered(ice.Select(m => m.No));
        }

        [Test]
        public void タイプが2つのペンモンはどちらのタイプでも出る()
        {
            var dual = MonsterTable.All.First(m => m.Types.Count == 2);

            CollectionAssert.Contains(MonsterTable.WithType(dual.Types[0]), dual);
            CollectionAssert.Contains(MonsterTable.WithType(dual.Types[1]), dual);
        }

        [Test]
        public void 技の数字は分類と威力と命中_変化技の威力は棒()
        {
            Assert.AreEqual("ぶつり　いりょく 50　めいちゅう 100", MoveTable.Tackle.Numbers());
            Assert.AreEqual("へんか　いりょく -　めいちゅう 90", MoveTable.NumbWave.Numbers());
        }

        [Test]
        public void 技の効果は先制と確率と中身を出す()
        {
            Assert.AreEqual("", MoveTable.Tackle.Effects());
            Assert.AreEqual("せんせい", MoveTable.BellySlide.Effects());
            Assert.AreEqual("30%で あいてを まひに する", MoveTable.PenguinPress.Effects());
            Assert.AreEqual("10%で あいてを こんらんさせる", MoveTable.Shout.Effects());
            Assert.AreEqual("じぶんの こうげきを 2だん あげる", MoveTable.FlapDance.Effects());
            Assert.AreEqual("あいての すばやさを 1だん さげる", MoveTable.ColdBreath.Effects());
        }

        [Test]
        public void 技ボタンの相性は相手の2タイプを掛け合わせる()
        {
            Assert.AreEqual(4f, TestMonsters.FireStrike.Effectiveness(new[] { MonsterType.Moss, MonsterType.Ice }));
            Assert.AreEqual(0f, MoveTable.Tackle.Effectiveness(new[] { MonsterType.Aurora }));
        }

        [Test]
        public void 変化技の相性はいつも等倍()
        {
            Assert.AreEqual(1f, MoveTable.FlapDance.Effectiveness(new[] { MonsterType.Aurora }));
        }
    }
}
