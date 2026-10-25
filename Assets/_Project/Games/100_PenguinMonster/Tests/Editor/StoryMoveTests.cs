using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestBattle;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 区切り4で足した、相手の特攻・特防を下げる技（PLAN_STORY_TOWN04 §7）
    /// </summary>
    public class StoryMoveTests
    {
        // 確率の技を何回試すか。多いほどぶれないが、テストが遅くなる
        private const int Trials = 400;

        [Test]
        public void ぽかん顔は相手の特攻を1段下げる()
        {
            var engine = CreateEngine(Fast(MoveTable.BlankStare), Slow());

            Turn(engine);

            Assert.AreEqual(-1, Active(engine, 1).GetStage(StatKind.SpAttack));
        }

        [Test]
        public void キンキン声は当たれば相手の特防を2段下げる()
        {
            var engine = CreateEngine(Fast(MoveTable.ShrillVoice), Slow());

            // 命中 85 なので、当たるまで繰り返す
            var changed = Enumerable.Range(0, 10)
                .SelectMany(_ => Turn(engine))
                .First(IsFoeDown(StatKind.SpDefense));

            Assert.AreEqual(-2, changed.Amount);
        }

        [Test]
        public void キンキン声はたまに外れる()
        {
            float rate = HitRate(MoveTable.ShrillVoice, e => e.Kind == BattleEventKind.Missed && e.Side == 1);

            Assert.That(rate, Is.InRange(0.08f, 0.22f));
        }

        // 追加効果 30% × 命中（向かい風は 95）
        [TestCase(StatKind.SpAttack, 0.22f, 0.36f)]
        [TestCase(StatKind.SpDefense, 0.23f, 0.37f)]
        public void 追加効果の技は30パーセントで相手の能力を下げる(StatKind stat, float min, float max)
        {
            var move = stat == StatKind.SpAttack ? MoveTable.Headwind : MoveTable.SeeThrough;

            float rate = HitRate(move, IsFoeDown(stat));

            Assert.That(rate, Is.InRange(min, max));
        }

        [Test]
        public void 追加効果の技はダメージを与える()
        {
            foreach (var move in new[] { MoveTable.Headwind, MoveTable.SeeThrough })
            {
                Assert.AreEqual(MoveCategory.Special, move.Category, move.Name);
                Assert.That(move.Power, Is.GreaterThan(0), move.Name);
            }
        }

        [Test]
        public void ドラフトの技表には入らない()
        {
            var draftMoves = MonsterTable.All.SelectMany(m => m.Moves).ToList();

            foreach (var move in NewMoves)
            {
                CollectionAssert.DoesNotContain(draftMoves, move, move.Name);
            }
        }

        [Test]
        public void 記録とオンラインの両方から引ける()
        {
            foreach (var move in NewMoves)
            {
                Assert.AreSame(move, MoveTable.ByName(move.Name), move.Name);
                Assert.That(MoveTable.IndexOf(move), Is.GreaterThanOrEqualTo(0), move.Name);
            }
        }

        private static MoveDefinition[] NewMoves => new[]
        {
            MoveTable.BlankStare, MoveTable.ShrillVoice, MoveTable.Headwind, MoveTable.SeeThrough,
        };

        /// <summary>相手（陣営1）の stat が下がった、の文か</summary>
        private static System.Func<BattleEvent, bool> IsFoeDown(StatKind stat)
        {
            return e => e.Kind == BattleEventKind.StatChanged && e.Side == 1 && e.Stat == stat && e.Amount < 0;
        }

        /// <summary>種を変えて1ターンずつ試し、match の文が出た割合</summary>
        private static float HitRate(MoveDefinition move, System.Func<BattleEvent, bool> match)
        {
            int hits = 0;
            for (int seed = 0; seed < Trials; seed++)
            {
                var engine = CreateEngine(Fast(move), Slow(), seed);
                if (Turn(engine).Any(match))
                {
                    hits++;
                }
            }
            return (float)hits / Trials;
        }
    }
}
