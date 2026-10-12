using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>ダブルス（仕様書 §4.9）。4体選出・場に2体ずつ</summary>
    public class DoublesTests
    {
        private static readonly MoveDefinition Spread = new MoveDefinition("テストぜんたい", MonsterType.Normal, MoveCategory.Special, 50, 100,
            hitsAllFoes: true);

        private static BattleState CreateState(MonsterDefinition[] party0, MonsterDefinition[] party1)
        {
            return new BattleState(party0, party1, BattleFormat.Double.ActiveCount());
        }

        private static BattleEngine CreateEngine(MonsterDefinition[] party0, MonsterDefinition[] party1, int seed = 1)
        {
            return new BattleEngine(CreateState(party0, party1), new System.Random(seed));
        }

        private static MonsterDefinition[] Party(string prefix, int speed, MoveDefinition move, int defense = 50)
        {
            return Enumerable.Range(1, BattleFormat.Double.PartySize())
                .Select(i => TestMonsters.Create(prefix + i, speed, move, defense: defense))
                .ToArray();
        }

        private static BattleAction[] Moves(int target0 = 0, int target1 = 0)
        {
            return new[] { BattleAction.UseMove(0, target0), BattleAction.UseMove(0, target1) };
        }

        [Test]
        public void 場の2体ずつが選出の先頭から出る()
        {
            var engine = CreateEngine(Party("味", 50, TestMonsters.Wait), Party("敵", 50, TestMonsters.Wait));

            var sentOut = engine.StartBattle().Where(e => e.Kind == BattleEventKind.SentOut).ToList();

            Assert.AreEqual(4, sentOut.Count);
            CollectionAssert.AreEqual(new[] { 0, 1, 0, 1 }, sentOut.Select(e => e.Slot));
            CollectionAssert.AreEqual(new[] { 0, 0, 1, 1 }, sentOut.Select(e => e.Side));
            Assert.AreSame(engine.State.Sides[1].Party[1], engine.State.Sides[1].ActiveAt(1));
        }

        [Test]
        public void 狙った相手に当たる()
        {
            var engine = CreateEngine(Party("味", 90, TestMonsters.Strike), Party("敵", 10, TestMonsters.Wait));

            var events = engine.ResolveTurn(Moves(target0: 1, target1: 1), Moves());

            var damaged = events.Where(e => e.Kind == BattleEventKind.Damaged).ToList();
            Assert.AreEqual(2, damaged.Count);
            Assert.IsTrue(damaged.All(e => e.Side == 1 && e.Slot == 1));
            Assert.IsTrue(engine.State.Sides[1].ActiveAt(0).IsFullHp);
        }

        [Test]
        public void 狙った相手が倒れていたらもう片方に当たる()
        {
            var engine = CreateEngine(Party("味", 90, TestMonsters.Strike), Party("敵", 10, TestMonsters.Wait));
            engine.State.Sides[1].ActiveAt(1).TakeDamage(int.MaxValue);

            var events = engine.ResolveTurn(Moves(target0: 1, target1: 1), Moves());

            var damaged = events.Where(e => e.Kind == BattleEventKind.Damaged).ToList();
            Assert.AreEqual(2, damaged.Count);
            Assert.IsTrue(damaged.All(e => e.Slot == 0));
        }

        [Test]
        public void 全体技は相手2体に当たる()
        {
            var engine = CreateEngine(Party("味", 90, Spread), Party("敵", 10, TestMonsters.Wait, defense: 200));

            var events = engine.ResolveTurn(new[] { BattleAction.UseMove(0), BattleAction.UseMove(0) }, Moves());

            var damaged = events.Where(e => e.Kind == BattleEventKind.Damaged).ToList();
            Assert.AreEqual(4, damaged.Count);
            CollectionAssert.AreEquivalent(new[] { 0, 0, 1, 1 }, damaged.Select(e => e.Slot));
        }

        [Test]
        public void 全体技は2体に当たるとダメージが減る()
        {
            var attacker = new BattleMonster(TestMonsters.Create("攻", 50, Spread));
            var defender = new BattleMonster(TestMonsters.Create("受", 50, Spread));

            float single = DamageCalculator.Expected(attacker, defender, Spread);
            float spread = DamageCalculator.Expected(attacker, defender, Spread, BattleBalance.SpreadMultiplier);

            Assert.AreEqual((int)(single * BattleBalance.SpreadMultiplier), spread, 1f);
        }

        [Test]
        public void 行動順は4体の素早さで決まる()
        {
            var party0 = Party("味", 30, TestMonsters.Wait);
            party0[1] = TestMonsters.Create("最速", 120, TestMonsters.Wait);
            var party1 = Party("敵", 60, TestMonsters.Wait);
            var engine = CreateEngine(party0, party1);

            var users = engine.ResolveTurn(Moves(), Moves()).Where(e => e.Kind == BattleEventKind.MoveUsed).ToList();

            Assert.AreEqual(4, users.Count);
            Assert.AreEqual("最速", users[0].Monster.Name);
            Assert.AreEqual(0, users[3].Side);
        }

        [Test]
        public void 倒れた場は控えの数だけ入れ替えが要る()
        {
            var state = CreateState(Party("味", 50, TestMonsters.Wait), Party("敵", 50, TestMonsters.Wait));
            var side = state.Sides[0];
            side.ActiveAt(0).TakeDamage(int.MaxValue);
            side.ActiveAt(1).TakeDamage(int.MaxValue);
            side.Party[3].TakeDamage(int.MaxValue);

            // 控えは3番目の1体だけなので、前の場だけ埋める
            CollectionAssert.AreEqual(new[] { 0 }, side.ReplaceableSlots());
        }

        [Test]
        public void 入れ替えは指定した場に出る()
        {
            var engine = CreateEngine(Party("味", 50, TestMonsters.Wait), Party("敵", 50, TestMonsters.Wait));
            var side = engine.State.Sides[0];
            side.ActiveAt(1).TakeDamage(int.MaxValue);

            var events = engine.Replace(0, 1, 3);

            Assert.AreSame(side.Party[3], side.ActiveAt(1));
            Assert.AreEqual(1, events.Single(e => e.Kind == BattleEventKind.SentOut).Slot);
            Assert.IsFalse(side.NeedsReplacement);
        }

        [Test]
        public void 控えがいなければ1体で戦い続け全員倒れたら負け()
        {
            var party1 = Party("敵", 10, TestMonsters.Wait);
            var engine = CreateEngine(Party("味", 90, TestMonsters.Finisher), party1);
            var foe = engine.State.Sides[1];
            foe.Party[2].TakeDamage(int.MaxValue);
            foe.Party[3].TakeDamage(int.MaxValue);
            foe.ActiveAt(1).TakeDamage(int.MaxValue);

            // 空いた場の行動は使われない
            engine.ResolveTurn(Moves(target0: 1, target1: 1), Moves());

            Assert.IsTrue(engine.State.IsOver);
            Assert.AreEqual(0, engine.State.Winner);
        }

        [Test]
        public void こわもては相手2体の攻撃を下げる()
        {
            var party0 = Party("味", 90, TestMonsters.Wait);
            party0[0] = TestMonsters.With(party0[0], Ability.Intimidate);
            var engine = CreateEngine(party0, Party("敵", 10, TestMonsters.Wait));

            engine.StartBattle();

            Assert.AreEqual(-1, engine.State.Sides[1].ActiveAt(0).GetStage(StatKind.Attack));
            Assert.AreEqual(-1, engine.State.Sides[1].ActiveAt(1).GetStage(StatKind.Attack));
        }

        [Test]
        public void 同じ控えへの2回目の交代は取りやめる()
        {
            var engine = CreateEngine(Party("味", 50, TestMonsters.Wait), Party("敵", 50, TestMonsters.Wait));

            var events = engine.ResolveTurn(new[] { BattleAction.SwitchTo(2), BattleAction.SwitchTo(2) }, Moves());

            Assert.AreEqual(1, events.Count(e => e.Kind == BattleEventKind.Withdrawn));
            // 同速なのでどちらが先に交代するかはランダム。どちらにしても控えの2番目は出ない
            Assert.IsTrue(engine.State.Sides[0].IsActive(2));
            Assert.IsFalse(engine.State.Sides[0].IsActive(3));
        }

        [Test]
        public void 羽リレーは使った場に控えの先頭が出る()
        {
            var party0 = Party("味", 50, TestMonsters.Wait);
            party0[1] = TestMonsters.Create("リレー", 50, MoveTable.FeatherRelay);
            var engine = CreateEngine(party0, Party("敵", 10, TestMonsters.Wait));

            engine.ResolveTurn(Moves(), Moves());

            Assert.AreSame(engine.State.Sides[0].Party[0], engine.State.Sides[0].ActiveAt(0));
            Assert.AreSame(engine.State.Sides[0].Party[2], engine.State.Sides[0].ActiveAt(1));
        }

        [Test]
        public void CPUは倒しやすい相手を狙う()
        {
            var cpu = new CpuPlayer(new System.Random(1), CpuLevel.Normal);
            // 場の1番目だけ 苔（火が抜群）にする
            var party1 = Party("敵", 50, TestMonsters.Wait);
            party1[1] = TestMonsters.Create("苔", 50, Enumerable.Repeat(TestMonsters.Wait, 4).ToArray(), MonsterType.Moss);
            var state = CreateState(Party("味", 50, TestMonsters.FireStrike), party1);

            var actions = cpu.ChooseActions(state.Sides[0], state.Sides[1]);

            Assert.AreEqual(2, actions.Count);
            Assert.IsTrue(actions.All(a => a.Kind == BattleActionKind.Move && a.Target == 1));
        }

        [Test]
        public void 行動2つを送って戻せる()
        {
            var actions = new[] { BattleAction.UseMove(3, 1), BattleAction.SwitchTo(2) };

            var copy = OnlineCodec.DecodeActions(OnlineCodec.EncodeActions(actions));

            Assert.AreEqual(BattleActionKind.Move, copy[0].Kind);
            Assert.AreEqual(3, copy[0].Index);
            Assert.AreEqual(1, copy[0].Target);
            Assert.AreEqual(BattleActionKind.Switch, copy[1].Kind);
            Assert.AreEqual(2, copy[1].Index);
        }

        /// <summary>ホストの計算をイベントだけで写しても、最後まで場・HP・状態異常がずれない（オンラインのダブルス）</summary>
        [Test]
        public void 写しはダブルスでも最後までずれない()
        {
            var random = new System.Random(3);
            var pool = MonsterTable.All;
            var team0 = pool.Take(4).ToList();
            var team1 = pool.Skip(10).Take(4).ToList();
            var host = new BattleEngine(new BattleState(team0, team1, 2), random);
            var mirror = new BattleState(team1, team0, 2);
            var cpus = new[] { new CpuPlayer(random, CpuLevel.Strong), new CpuPlayer(random, CpuLevel.Strong) };
            var sides = host.State.Sides;

            Send(host.StartBattle(), host, mirror);
            for (int turn = 0; turn < BalanceSimulator.MaxTurns && !host.State.IsOver; turn++)
            {
                Send(host.ResolveTurn(cpus[0].ChooseActions(sides[0], sides[1]), cpus[1].ChooseActions(sides[1], sides[0])), host, mirror);
                foreach (var side in sides)
                {
                    foreach (int slot in side.ReplaceableSlots())
                    {
                        Send(host.Replace(side.Index, slot, cpus[side.Index].ChooseReplacement(side, sides[1 - side.Index])), host, mirror);
                    }
                }
                AssertMirrored(host.State, mirror);
            }

            Assert.IsTrue(mirror.IsOver);
            Assert.AreEqual(1 - host.State.Winner, mirror.Winner);
        }

        [Test]
        public void ダブルスのバランス検証も最後まで回る()
        {
            const int battles = 50;
            var rates = BalanceSimulator.Run(MonsterTable.All, battles, 1, BattleFormat.Double);

            Assert.AreEqual(battles * BattleFormat.Double.PartySize() * 2, rates.Sum(r => r.Battles));
        }

        private static void Send(List<BattleEvent> events, BattleEngine host, BattleState mirror)
        {
            BattleEventCodec.Decode(BattleEventCodec.Encode(events, host.State), mirror, swapSides: true);
        }

        private static void AssertMirrored(BattleState host, BattleState mirror)
        {
            for (int side = 0; side < BattleState.SideCount; side++)
            {
                var original = host.Sides[side];
                var copy = mirror.Sides[1 - side];
                for (int slot = 0; slot < original.ActiveCount; slot++)
                {
                    Assert.AreEqual(original.Party.ToList().IndexOf(original.ActiveAt(slot)), copy.Party.ToList().IndexOf(copy.ActiveAt(slot)));
                }
                for (int i = 0; i < original.Party.Count; i++)
                {
                    Assert.AreEqual(original.Party[i].CurrentHp, copy.Party[i].CurrentHp);
                    Assert.AreEqual(original.Party[i].Status, copy.Party[i].Status);
                }
            }
        }
    }
}
