using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    public class BattleEngineTests
    {
        private static BattleEngine CreateEngine(MonsterDefinition[] party0, MonsterDefinition[] party1, int seed = 1)
        {
            return new BattleEngine(new BattleState(party0, party1), new System.Random(seed));
        }

        /// <summary>最初に行動した陣営（技を使った / 交代した）</summary>
        private static int FirstActor(List<BattleEvent> events)
        {
            return events.First(e => e.Kind == BattleEventKind.MoveUsed || e.Kind == BattleEventKind.Withdrawn).Side;
        }

        [Test]
        public void FasterSideMovesFirst()
        {
            var engine = CreateEngine(
                TestMonsters.Party("遅", 30, TestMonsters.Strike),
                TestMonsters.Party("速", 90, TestMonsters.Strike));

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.AreEqual(1, FirstActor(events));
        }

        [Test]
        public void PriorityBeatsSpeed()
        {
            var engine = CreateEngine(
                TestMonsters.Party("遅", 30, TestMonsters.QuickStrike),
                TestMonsters.Party("速", 90, TestMonsters.Strike));

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.AreEqual(0, FirstActor(events));
        }

        [Test]
        public void SwitchBeatsPriority()
        {
            var engine = CreateEngine(
                TestMonsters.Party("速", 90, TestMonsters.QuickStrike),
                TestMonsters.Party("遅", 30, TestMonsters.Strike));

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.SwitchTo(1));

            Assert.AreEqual(1, FirstActor(events));
            Assert.AreEqual(1, engine.State.Sides[1].ActivePartyIndex);
            // 交代で出てきたペンモンが攻撃を受ける
            var damaged = events.First(e => e.Kind == BattleEventKind.Damaged);
            Assert.AreSame(engine.State.Sides[1].Party[1], damaged.Monster);
        }

        [Test]
        public void FaintedMonsterDoesNotAct()
        {
            var engine = CreateEngine(
                TestMonsters.Party("強", 90, TestMonsters.Finisher, attack: 200),
                TestMonsters.Party("弱", 30, TestMonsters.Strike, defense: 10));

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.Fainted && e.Side == 1));
            Assert.IsFalse(events.Any(e => e.Kind == BattleEventKind.MoveUsed && e.Side == 1));
            Assert.IsTrue(engine.State.Sides[1].NeedsReplacement);
        }

        [Test]
        public void AllThreeFainted_DecidesWinner()
        {
            var engine = CreateEngine(
                TestMonsters.Party("強", 90, TestMonsters.Finisher, attack: 200),
                TestMonsters.Party("弱", 30, TestMonsters.Strike, defense: 10));
            var cpu = new CpuPlayer(new System.Random(1));
            var events = new List<BattleEvent>();

            for (int turn = 0; turn < 3; turn++)
            {
                Assert.IsFalse(engine.State.IsOver);
                events.AddRange(engine.ResolveTurn(BattleAction.UseMove(0), cpu.ChooseAction(engine.State.Sides[1])));
                if (engine.State.Sides[1].NeedsReplacement)
                {
                    engine.Replace(1, cpu.ChooseReplacement(engine.State.Sides[1]));
                }
            }

            Assert.IsTrue(engine.State.IsOver);
            Assert.AreEqual(0, engine.State.Winner);
            Assert.AreEqual(BattleEventKind.BattleEnd, events.Last().Kind);
            Assert.AreEqual(0, events.Last().Side);
        }

        [Test]
        public void Replace_FaintedMonster_Throws()
        {
            var engine = CreateEngine(
                TestMonsters.Party("強", 90, TestMonsters.Finisher, attack: 200),
                TestMonsters.Party("弱", 30, TestMonsters.Strike, defense: 10));
            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.Throws<System.ArgumentException>(() => engine.Replace(1, 0));
        }

        [Test]
        public void NoEffectMove_DealsNoDamage()
        {
            var thunder = new MoveDefinition("テストでんき", MonsterType.Thunder, MoveCategory.Special, 90, 100);
            var attacker = new MonsterDefinition(0, "でんき", new[] { MonsterType.Thunder },
                new Stats(50, 50, 50, 50, 50, 90), new[] { thunder, thunder, thunder, thunder });
            var rock = new MonsterDefinition(0, "いわ", new[] { MonsterType.Rock },
                new Stats(50, 50, 50, 50, 50, 30), new[] { TestMonsters.Strike, TestMonsters.Strike, TestMonsters.Strike, TestMonsters.Strike });
            var engine = CreateEngine(new[] { attacker, attacker, attacker }, new[] { rock, rock, rock });

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.NoEffect && e.Side == 1));
            Assert.AreEqual(engine.State.Sides[1].Active.MaxHp, engine.State.Sides[1].Active.CurrentHp);
        }
    }
}
