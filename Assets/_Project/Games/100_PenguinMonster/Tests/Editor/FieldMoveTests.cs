using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 戦況を変える技（仕様書 §4.2）。守る・追い風・手助け
    /// </summary>
    public class FieldMoveTests
    {
        private static readonly MoveDefinition Guard = MoveTable.FeatherWrap;
        private static readonly MoveDefinition Tailwind = MoveTable.NorthWindCall;
        private static readonly MoveDefinition Help = MoveTable.Cheer;
        private static readonly MoveDefinition AttackDown = StatusMove("テストよわく", MoveEffect.FoeStat(StatKind.Attack, -1));
        private static readonly MoveDefinition Spread = new MoveDefinition("テストぜんたい", MonsterType.Normal, MoveCategory.Special, 50, 100,
            hitsAllFoes: true);

        private static BattleEngine Single(MonsterDefinition mon0, MonsterDefinition mon1)
        {
            return new BattleEngine(new BattleState(Party(mon0), Party(mon1)), new Random(1));
        }

        private static BattleEngine Double(MonsterDefinition mon0, MonsterDefinition mon1, int seed = 1)
        {
            var party0 = Enumerable.Repeat(mon0, BattleFormat.Double.PartySize()).ToArray();
            var party1 = Enumerable.Repeat(mon1, BattleFormat.Double.PartySize()).ToArray();
            return new BattleEngine(new BattleState(party0, party1, BattleFormat.Double.ActiveCount()), new Random(seed));
        }

        private static MonsterDefinition Mon(int speed, params MoveDefinition[] moves)
        {
            var four = Enumerable.Range(0, 4).Select(i => moves[Math.Min(i, moves.Length - 1)]).ToArray();
            return Create("テスト" + speed, speed, four, MonsterType.Normal);
        }

        private static List<BattleEvent> Turn(BattleEngine engine, int move0, int move1 = 0)
        {
            return engine.ResolveTurn(BattleAction.UseMove(move0), BattleAction.UseMove(move1));
        }

        private static bool Has(List<BattleEvent> events, BattleEventKind kind) => events.Any(e => e.Kind == kind);

        private static int FirstMover(List<BattleEvent> events) => events.First(e => e.Kind == BattleEventKind.MoveUsed).Side;

        [Test]
        public void 守ると相手の攻撃を受けない()
        {
            var engine = Single(Mon(10, Guard, Strike), Mon(90, Strike));

            var events = Turn(engine, 0);

            Assert.IsTrue(Has(events, BattleEventKind.ProtectStarted));
            Assert.IsTrue(Has(events, BattleEventKind.Protected));
            Assert.IsTrue(engine.State.Sides[0].Active.IsFullHp);
        }

        [Test]
        public void 守るは相手にかける変化技も防ぐ()
        {
            var engine = Single(Mon(10, Guard), Mon(90, AttackDown));

            Turn(engine, 0);

            Assert.AreEqual(0, engine.State.Sides[0].Active.GetStage(StatKind.Attack));
        }

        [Test]
        public void 守りが決まった次のターンに続けて使うと失敗する()
        {
            var engine = Single(Mon(10, Guard), Mon(90, Strike));
            Turn(engine, 0);

            var events = Turn(engine, 0);

            Assert.IsTrue(Has(events, BattleEventKind.Failed));
            Assert.IsFalse(engine.State.Sides[0].Active.IsFullHp);
        }

        [Test]
        public void 間に別の技を挟めばまた守れる()
        {
            var engine = Single(Mon(10, Guard, Wait), Mon(90, Wait));
            Turn(engine, 0);
            Turn(engine, 1);

            var events = Turn(engine, 0);

            Assert.IsTrue(Has(events, BattleEventKind.ProtectStarted));
        }

        [Test]
        public void ダブルスの全体技は守っていない相手にだけ当たる()
        {
            var engine = Double(Mon(10, Guard, Wait), Mon(90, Spread));

            var events = engine.ResolveTurn(new[] { BattleAction.UseMove(0), BattleAction.UseMove(1) },
                new[] { BattleAction.UseMove(0), BattleAction.UseMove(0) });

            var side0 = engine.State.Sides[0];
            Assert.IsTrue(side0.ActiveAt(0).IsFullHp);
            Assert.IsFalse(side0.ActiveAt(1).IsFullHp);
            Assert.AreEqual(2, events.Count(e => e.Kind == BattleEventKind.Protected));
        }

        [Test]
        public void 追い風で遅い陣営が先に動けるようになる()
        {
            var engine = Single(Mon(30, Tailwind, Wait), Mon(50, Wait));

            Assert.AreEqual(1, FirstMover(Turn(engine, 0)));
            Assert.IsTrue(engine.State.Sides[0].HasTailwind);
            Assert.AreEqual(0, FirstMover(Turn(engine, 1)));
        }

        [Test]
        public void 追い風は始めたターンを含めて4ターン目の終わりに止む()
        {
            var engine = Single(Mon(30, Tailwind, Wait), Mon(50, Wait));
            Turn(engine, 0);
            for (int turn = 2; turn < BattleBalance.TailwindTurns; turn++)
            {
                Assert.IsFalse(Has(Turn(engine, 1), BattleEventKind.TailwindEnded));
            }

            var last = Turn(engine, 1);

            Assert.AreEqual(0, last.Single(e => e.Kind == BattleEventKind.TailwindEnded).Side);
            Assert.IsFalse(engine.State.Sides[0].HasTailwind);
        }

        [Test]
        public void 追い風が吹いている間は追い風の技が失敗する()
        {
            var engine = Single(Mon(30, Tailwind), Mon(50, Wait));
            Turn(engine, 0);

            Assert.IsTrue(Has(Turn(engine, 0), BattleEventKind.Failed));
        }

        [Test]
        public void 手助けを受けた味方の技のダメージが上がる()
        {
            int helped = DamageWithPartnerMove(Help);
            int plain = DamageWithPartnerMove(Wait);

            Assert.AreEqual((int)(plain * BattleBalance.HelpingHandMultiplier), helped);
        }

        [Test]
        public void 手助けはシングルでは失敗する()
        {
            var engine = Single(Mon(30, Help), Mon(50, Wait));

            var events = Turn(engine, 0);

            Assert.IsTrue(Has(events, BattleEventKind.Failed));
            Assert.IsFalse(Has(events, BattleEventKind.Helped));
        }

        [Test]
        public void 手助けと守るはターンが終わると消える()
        {
            var engine = Double(Mon(30, Help, Guard), Mon(50, Wait));

            engine.ResolveTurn(new[] { BattleAction.UseMove(0), BattleAction.UseMove(1) },
                new[] { BattleAction.UseMove(0), BattleAction.UseMove(0) });

            var side0 = engine.State.Sides[0];
            Assert.IsFalse(side0.ActiveAt(1).IsHelped);
            Assert.IsFalse(side0.ActiveAt(1).IsProtecting);
            Assert.IsTrue(side0.ActiveAt(1).ProtectedLastTurn);
        }

        /// <summary>
        /// 場の左が partnerMove、右が攻撃したときに相手の左が受けたダメージ。
        /// 手助けも比べる変化技も乱数を引かないので、同じ種なら攻撃の乱数がそろう
        /// </summary>
        private static int DamageWithPartnerMove(MoveDefinition partnerMove)
        {
            var engine = Double(Mon(30, partnerMove, Strike), Mon(50, Wait));
            var target = engine.State.Sides[1].ActiveAt(0);

            engine.ResolveTurn(new[] { BattleAction.UseMove(0), BattleAction.UseMove(1, 0) },
                new[] { BattleAction.UseMove(0), BattleAction.UseMove(0) });

            return target.MaxHp - target.CurrentHp;
        }
    }
}
