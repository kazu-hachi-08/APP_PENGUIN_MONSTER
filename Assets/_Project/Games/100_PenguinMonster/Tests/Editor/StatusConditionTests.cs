using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 状態異常のダメージ・行動不能（仕様書 §4.4）。状態は直接セットして、乱数に頼らず状況を作る
    /// </summary>
    public class StatusConditionTests
    {
        // テスト用ペンモンの HP は 種族値50 + 75
        private const int MaxHp = 125;

        private static BattleEngine CreateEngine(MonsterDefinition mon0, MonsterDefinition mon1, int seed = 1)
        {
            return new BattleEngine(new BattleState(Party(mon0), Party(mon1)), new System.Random(seed));
        }

        /// <summary>陣営0（速い）と陣営1（遅い）が、どちらも何もしない技だけを持つ</summary>
        private static BattleEngine CreateIdleEngine(int seed = 1, int defense1 = 50)
        {
            return CreateEngine(Create("速", 90, Wait), Create("遅", 30, Wait, defense: defense1), seed);
        }

        private static List<BattleEvent> Idle(BattleEngine engine)
        {
            return engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
        }

        private static BattleMonster Active(BattleEngine engine, int side) => engine.State.Sides[side].Active;

        private static bool Has(List<BattleEvent> events, BattleEventKind kind, int side)
        {
            return events.Any(e => e.Kind == kind && e.Side == side);
        }

        [Test]
        public void どくはターン終わりに最大HPの8分の1()
        {
            var engine = CreateIdleEngine();
            Active(engine, 1).SetStatus(StatusCondition.Poison, 0);

            var events = Idle(engine);

            Assert.IsTrue(Has(events, BattleEventKind.StatusDamage, 1));
            Assert.AreEqual(MaxHp - MaxHp / 8, Active(engine, 1).CurrentHp);
        }

        [Test]
        public void もうどくは16分の1ずつ増える()
        {
            var engine = CreateIdleEngine();
            var target = Active(engine, 1);
            target.SetStatus(StatusCondition.BadPoison, 0);

            Idle(engine);
            int afterFirst = target.CurrentHp;
            Idle(engine);

            Assert.AreEqual(MaxHp - MaxHp * 1 / 16, afterFirst);
            Assert.AreEqual(afterFirst - MaxHp * 2 / 16, target.CurrentHp);
        }

        [Test]
        public void もうどくの段階は交代で戻る()
        {
            var engine = CreateIdleEngine();
            var target = Active(engine, 1);
            target.SetStatus(StatusCondition.BadPoison, 0);
            Idle(engine);

            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.SwitchTo(1));

            Assert.AreEqual(StatusCondition.BadPoison, target.Status);
            Assert.AreEqual(0, target.BadPoisonCount);
        }

        [Test]
        public void やけどはターン終わりに16分の1()
        {
            var engine = CreateIdleEngine();
            Active(engine, 1).SetStatus(StatusCondition.Burn, 0);

            Idle(engine);

            Assert.AreEqual(MaxHp - MaxHp / 16, Active(engine, 1).CurrentHp);
        }

        [Test]
        public void 状態異常のダメージでたおれたら勝敗が決まる()
        {
            var engine = CreateIdleEngine();
            var side1 = engine.State.Sides[1];
            foreach (var monster in side1.Party)
            {
                monster.SetStatus(StatusCondition.Poison, 0);
                monster.TakeDamage(MaxHp - 1);
            }

            var events = Idle(engine);
            for (int i = 1; i < side1.Party.Count && !engine.State.IsOver; i++)
            {
                engine.Replace(1, i);
                events = Idle(engine);
            }

            Assert.IsTrue(engine.State.IsOver);
            Assert.AreEqual(0, engine.State.Winner);
            Assert.AreEqual(BattleEventKind.BattleEnd, events.Last().Kind);
        }

        [Test]
        public void ねむりは指定ターン動けず_その次のターンに起きて動く()
        {
            var engine = CreateIdleEngine();
            Active(engine, 1).SetStatus(StatusCondition.Sleep, 2);

            for (int turn = 0; turn < 2; turn++)
            {
                var events = Idle(engine);
                Assert.IsTrue(Has(events, BattleEventKind.Immobilized, 1));
                Assert.IsFalse(Has(events, BattleEventKind.MoveUsed, 1));
            }
            var wakeTurn = Idle(engine);

            Assert.IsTrue(Has(wakeTurn, BattleEventKind.WokeUp, 1));
            Assert.IsTrue(Has(wakeTurn, BattleEventKind.MoveUsed, 1));
            Assert.AreEqual(StatusCondition.None, Active(engine, 1).Status);
        }

        [Test]
        public void ねむりの技は1から3ターン眠らせる()
        {
            var sleepMove = StatusMove("テストねむり", MoveEffect.Ailment(StatusCondition.Sleep));
            for (int seed = 0; seed < 50; seed++)
            {
                // 眠らせた同じターンに相手が動こうとすると残りが減るので、相手が先に動き終わるようにする
                var engine = CreateEngine(Create("遅", 30, sleepMove), Create("速", 90, Wait), seed);
                engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

                Assert.That(Active(engine, 1).SleepTurns, Is.InRange(1, 3));
            }
        }

        [Test]
        public void こおりは解けるまで動けない()
        {
            var engine = CreateIdleEngine();
            Active(engine, 1).SetStatus(StatusCondition.Freeze, 0);
            for (int turn = 0; turn < 40 && Active(engine, 1).Status == StatusCondition.Freeze; turn++)
            {
                var events = Idle(engine);
                bool thawed = Has(events, BattleEventKind.Thawed, 1);
                // 解けたターンはそのまま動き、解けなかったターンは動かない
                Assert.AreEqual(thawed, Has(events, BattleEventKind.MoveUsed, 1));
                Assert.AreEqual(!thawed, Has(events, BattleEventKind.Immobilized, 1));
            }

            Assert.AreEqual(StatusCondition.None, Active(engine, 1).Status);
        }

        [Test]
        public void こおりはひの技を受けると解ける()
        {
            var engine = CreateEngine(Create("ひ", 90, FireStrike), Create("こおった", 30, Wait));
            Active(engine, 1).SetStatus(StatusCondition.Freeze, 0);

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            var damagedIndex = events.FindIndex(e => e.Kind == BattleEventKind.Damaged && e.Side == 1);
            var thawedIndex = events.FindIndex(e => e.Kind == BattleEventKind.Thawed && e.Side == 1);
            Assert.That(thawedIndex, Is.GreaterThan(damagedIndex));
            Assert.That(damagedIndex, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void まひはすばやさが半分になり行動順が変わる()
        {
            var engine = CreateEngine(Create("まひ", 90, Strike), Create("ふつう", 70, Strike));
            Active(engine, 0).SetStatus(StatusCondition.Paralysis, 0);

            // 90+20=110 → 55 なので、70+20=90 の相手が先に動く
            Assert.AreEqual(55, Active(engine, 0).EffectiveSpeed);
            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
            Assert.AreEqual(1, events.First(e => e.Kind == BattleEventKind.MoveUsed).Side);
        }

        [Test]
        public void まひはおよそ4回に1回動けない()
        {
            const int turns = 400;
            var engine = CreateIdleEngine(seed: 7);
            Active(engine, 1).SetStatus(StatusCondition.Paralysis, 0);
            int immobilized = 0;

            for (int turn = 0; turn < turns; turn++)
            {
                var events = Idle(engine);
                bool stopped = Has(events, BattleEventKind.Immobilized, 1);
                Assert.AreNotEqual(stopped, Has(events, BattleEventKind.MoveUsed, 1));
                immobilized += stopped ? 1 : 0;
            }

            Assert.That(immobilized / (float)turns, Is.InRange(0.18f, 0.32f));
        }

        [Test]
        public void こんらん中は自分を攻撃することがあり_交代で治る()
        {
            // 自傷ダメージで先にたおれないよう、ぼうぎょを高くしておく
            var engine = CreateIdleEngine(seed: 3, defense1: 200);
            var target = Active(engine, 1);
            target.Confuse(20);
            bool hurt = false;

            for (int turn = 0; turn < 15; turn++)
            {
                var events = Idle(engine);
                Assert.IsTrue(Has(events, BattleEventKind.ConfusionCheck, 1));
                bool hurtThisTurn = Has(events, BattleEventKind.HurtItself, 1);
                Assert.AreNotEqual(hurtThisTurn, Has(events, BattleEventKind.MoveUsed, 1));
                hurt |= hurtThisTurn;
            }
            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.SwitchTo(1));

            Assert.IsTrue(hurt);
            Assert.That(target.CurrentHp, Is.LessThan(MaxHp));
            Assert.IsFalse(target.IsConfused);
        }

        [Test]
        public void 状態異常は交代しても残る()
        {
            var engine = CreateIdleEngine();
            var target = Active(engine, 1);
            target.SetStatus(StatusCondition.Burn, 0);

            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.SwitchTo(1));

            Assert.AreEqual(StatusCondition.Burn, target.Status);
        }

        [Test]
        public void すでに状態異常なら変化技は決まらない()
        {
            var poisonMove = StatusMove("テストどく", MoveEffect.Ailment(StatusCondition.Poison));
            var engine = CreateEngine(Create("速", 90, poisonMove), Create("遅", 30, Wait));
            Active(engine, 1).SetStatus(StatusCondition.Burn, 0);

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.IsTrue(Has(events, BattleEventKind.Failed, 1));
            Assert.AreEqual(StatusCondition.Burn, Active(engine, 1).Status);
        }

        [TestCase(MonsterType.Fire, StatusCondition.Burn)]
        [TestCase(MonsterType.Ice, StatusCondition.Freeze)]
        [TestCase(MonsterType.Thunder, StatusCondition.Paralysis)]
        public void タイプによっては状態異常にならない(MonsterType type, StatusCondition status)
        {
            var move = StatusMove("テスト", MoveEffect.Ailment(status));
            var engine = CreateEngine(Create("速", 90, move), Create("遅", 30, new[] { Wait, Wait, Wait, Wait }, type));

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.IsTrue(Has(events, BattleEventKind.Failed, 1));
            Assert.AreEqual(StatusCondition.None, Active(engine, 1).Status);
        }

        [Test]
        public void やけどは物理のダメージが半分()
        {
            // 44 → やけど 22（仕様書 §4.3 の式の最後に掛ける）
            Assert.AreEqual(22, DamageCalculator.Compute(80, 120, 100, false, 100, false, 1f, isBurned: true));
        }
    }
}
