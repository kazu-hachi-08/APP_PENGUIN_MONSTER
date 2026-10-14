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
            Assert.AreEqual("物理　威力 50　命中 100", MoveTable.Tackle.Numbers());
            Assert.AreEqual("変化　威力 -　命中 90", MoveTable.NumbWave.Numbers());
        }

        [Test]
        public void 技の効果は先制と確率と中身を出す()
        {
            Assert.AreEqual("", MoveTable.Tackle.Effects());
            Assert.AreEqual("先制", MoveTable.BellySlide.Effects());
            Assert.AreEqual("30%で 相手を 麻痺に する", MoveTable.PenguinPress.Effects());
            Assert.AreEqual("相手 全員に 当たる・10%で 相手を 混乱させる", MoveTable.Shout.Effects());
            Assert.AreEqual("自分の 攻撃を 2段階 上げる", MoveTable.FlapDance.Effects());
            Assert.AreEqual("相手 全員に 当たる・相手の 素早さを 1段階 下げる", MoveTable.ColdBreath.Effects());
            Assert.AreEqual("能力の 変化を 引き継いで 控えの 先頭と 交代する", MoveTable.FeatherRelay.Effects());
            Assert.AreEqual("先制・その ターン 相手の 技を 防ぐ（続けて 使うと 失敗する）", MoveTable.FeatherWrap.Effects());
            Assert.AreEqual("4ターン 味方の 素早さを 2倍に する", MoveTable.NorthWindCall.Effects());
            Assert.AreEqual("先制・ダブルスで 味方の 技の ダメージを 1.5倍に する", MoveTable.Cheer.Effects());
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
