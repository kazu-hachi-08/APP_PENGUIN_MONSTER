using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 能力変化（仕様書 §4.5）。倍率・上限・交代で戻ること・急所での無視・羽リレーでの引き継ぎ（§4.2）
    /// </summary>
    public class StatStageTests
    {
        [TestCase(0, 100)]
        [TestCase(1, 150)]
        [TestCase(2, 200)]
        [TestCase(6, 400)]
        [TestCase(-1, 66)]
        [TestCase(-2, 50)]
        [TestCase(-6, 25)]
        public void 能力の倍率は2分の2から(int stage, int expected)
        {
            Assert.AreEqual(expected, StageMultiplier.ApplyStat(100, stage));
        }

        [TestCase(1, 133)]
        [TestCase(6, 300)]
        [TestCase(-1, 75)]
        [TestCase(-6, 33)]
        public void 命中の倍率は3分の3から(int stage, int expected)
        {
            Assert.AreEqual(expected, StageMultiplier.ApplyAccuracy(100, stage));
        }

        [Test]
        public void ランクは6で止まり_それ以上はもう上がらない()
        {
            var engine = new BattleEngine(new BattleState(Party(Create("速", 90, AttackUp)), Party(Create("遅", 30, Wait))),
                new System.Random(1));
            var user = engine.State.Sides[0].Active;

            var first = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
            var fourth = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            var changed = first.First(e => e.Kind == BattleEventKind.StatChanged && e.Side == 0);
            Assert.AreEqual(StatKind.Attack, changed.Stat);
            Assert.AreEqual(2, changed.Amount);
            Assert.AreEqual(BattleBalance.MaxStatStage, user.GetStage(StatKind.Attack));
            Assert.IsTrue(fourth.Any(e => e.Kind == BattleEventKind.StatUnchanged && e.Side == 0));
        }

        [Test]
        public void 交代するとランクが戻る()
        {
            var engine = new BattleEngine(new BattleState(Party(Create("速", 90, AttackUp)), Party(Create("遅", 30, Wait))),
                new System.Random(1));
            var user = engine.State.Sides[0].Active;
            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            engine.ResolveTurn(BattleAction.SwitchTo(1), BattleAction.UseMove(0));

            Assert.AreEqual(0, user.GetStage(StatKind.Attack));
        }

        [Test]
        public void 羽リレーはランクを引き継いで控えの先頭と交代する()
        {
            var relayer = Create("リレー", 90, new[] { AttackUp, MoveTable.FeatherRelay, Wait, Wait }, MonsterType.Normal);
            var party = new[] { relayer, Create("次", 50, Wait), Create("最後", 50, Wait) };
            var engine = new BattleEngine(new BattleState(party, Party(Create("遅", 30, Wait))), new System.Random(1));
            var side = engine.State.Sides[0];
            var user = side.Active;
            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            var events = engine.ResolveTurn(BattleAction.UseMove(1), BattleAction.UseMove(0));

            Assert.AreEqual(1, side.ActivePartyIndex);
            Assert.AreEqual(2, side.Active.GetStage(StatKind.Attack));
            Assert.AreEqual(0, user.GetStage(StatKind.Attack));
            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.Withdrawn && e.Monster == user));
            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.SentOut && e.Monster == side.Active));
            var passed = events.Single(e => e.Kind == BattleEventKind.StagesPassed);
            Assert.AreEqual(2, passed.PassedStages()[(int)StatKind.Attack]);
        }

        [Test]
        public void 引き継いだランクは上限と下限も詰めて戻せる()
        {
            var monster = new BattleMonster(Create("詰め", 50, Wait));
            monster.SetStages(new[] { 6, -6, 0, 1, -1, 3, -3 });

            var passed = BattleEvent.StagesPassed(0, monster);

            CollectionAssert.AreEqual(new[] { 6, -6, 0, 1, -1, 3, -3 }, passed.PassedStages());
        }

        [Test]
        public void 羽リレーは控えがいないと決まらない()
        {
            var relayer = Create("リレー", 90, MoveTable.FeatherRelay);
            var engine = new BattleEngine(new BattleState(new[] { relayer }, Party(Create("遅", 30, Wait))), new System.Random(1));

            var events = engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.Failed && e.Side == 0));
            Assert.AreEqual(0, engine.State.Sides[0].ActivePartyIndex);
        }

        [Test]
        public void すばやさのランクで行動順が変わる()
        {
            var speedUp = StatusMove("テストはやく", MoveEffect.SelfStat(StatKind.Speed, 2));
            var engine = new BattleEngine(new BattleState(
                    Party(Create("遅", 50, new[] { speedUp, Strike, Strike, Strike }, MonsterType.Normal)),
                    Party(Create("速", 80, Strike))),
                new System.Random(1));
            engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));

            // 70 → 140 で 100 を追い抜く
            var events = engine.ResolveTurn(BattleAction.UseMove(1), BattleAction.UseMove(0));

            Assert.AreEqual(0, events.First(e => e.Kind == BattleEventKind.MoveUsed).Side);
        }

        [Test]
        public void 急所のときは下がった攻撃と上がった防御を無視する()
        {
            var attacker = new BattleMonster(Create("A", 50, Strike));
            var defender = new BattleMonster(Create("B", 50, Strike));
            attacker.ChangeStage(StatKind.Attack, -2);
            defender.ChangeStage(StatKind.Defense, 2);

            Assert.AreEqual(35, attacker.AttackStat(true, false));
            Assert.AreEqual(70, attacker.AttackStat(true, true));
            Assert.AreEqual(140, defender.DefenseStat(true, false));
            Assert.AreEqual(70, defender.DefenseStat(true, true));
        }

        [Test]
        public void 急所でも上がった攻撃はそのまま使う()
        {
            var attacker = new BattleMonster(Create("A", 50, Strike));
            attacker.ChangeStage(StatKind.Attack, 2);

            Assert.AreEqual(140, attacker.AttackStat(true, true));
        }
    }
}
