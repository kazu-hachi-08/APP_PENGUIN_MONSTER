using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// CPU の賢さ（仕様書 §4.8）。技の選び方・不利なときの交代・ドラフトの取り方
    /// </summary>
    public class CpuPlayerTests
    {
        private const int Trials = 200;

        private static readonly MoveDefinition IceHit = TypedMove(MonsterType.Ice);
        private static readonly MoveDefinition FireHit = TypedMove(MonsterType.Fire);
        private static readonly MoveDefinition SeaHit = TypedMove(MonsterType.Sea);

        private static MoveDefinition TypedMove(MonsterType type)
        {
            return new MoveDefinition("テスト" + type, type, MoveCategory.Physical, 50, 100);
        }

        private static MonsterDefinition Mono(string name, MonsterType type, MoveDefinition move)
        {
            return TestMonsters.Create(name, 50, new[] { move, move, move, move }, type);
        }

        /// <summary>CPU を陣営 0、相手を陣営 1 に置く</summary>
        private static BattleState Battle(MonsterDefinition[] cpuParty, MonsterDefinition foe)
        {
            return new BattleState(cpuParty, TestMonsters.Party(foe));
        }

        private static BattleAction Act(CpuLevel level, BattleState state)
        {
            return new CpuPlayer(new Random(1), level).ChooseAction(state.Sides[0], state.Sides[1]);
        }

        /// <summary>こおり2体（ひ に弱い）と、うみ1体（ひ に強い）</summary>
        private static MonsterDefinition[] IceIceSea()
        {
            return new[] { Mono("こおり1", MonsterType.Ice, IceHit), Mono("こおり2", MonsterType.Ice, IceHit), Mono("うみ", MonsterType.Sea, SeaHit) };
        }

        private static MonsterDefinition FireFoe() => Mono("ひ", MonsterType.Fire, FireHit);

        [Test]
        public void ふつうは一番ダメージが出る技を選ぶ()
        {
            var moves = new[] { TestMonsters.Wait, TestMonsters.Strike, TestMonsters.Finisher, TestMonsters.QuickStrike };
            var user = TestMonsters.Create("つかうがわ", 50, moves, MonsterType.Normal);
            var state = Battle(TestMonsters.Party(user), TestMonsters.Create("まと", 50, TestMonsters.Strike));

            var action = Act(CpuLevel.Normal, state);

            Assert.AreEqual(BattleActionKind.Move, action.Kind);
            Assert.AreEqual(2, action.Index);
        }

        [Test]
        public void ふつうは効かない技を選ばない()
        {
            // ノーマルは オーロラ に効かないので、威力が低くても ひ の技を選ぶ
            var moves = new[] { TestMonsters.Finisher, TestMonsters.Wait, FireHit, TestMonsters.Strike };
            var user = TestMonsters.Create("つかうがわ", 50, moves, MonsterType.Normal);
            var state = Battle(TestMonsters.Party(user), Mono("オーロラ", MonsterType.Aurora, TestMonsters.Strike));

            Assert.AreEqual(2, Act(CpuLevel.Normal, state).Index);
        }

        [Test]
        public void ふつうは攻撃技が無ければ4つの中からランダム()
        {
            var user = TestMonsters.Create("まつだけ", 50, TestMonsters.Wait);
            var state = Battle(TestMonsters.Party(user), TestMonsters.Create("まと", 50, TestMonsters.Strike));
            var cpu = new CpuPlayer(new Random(2), CpuLevel.Normal);

            for (int i = 0; i < Trials; i++)
            {
                var action = cpu.ChooseAction(state.Sides[0], state.Sides[1]);
                Assert.AreEqual(BattleActionKind.Move, action.Kind);
                Assert.That(action.Index, Is.InRange(0, 3));
            }
        }

        [Test]
        public void よわいは技をランダムに選ぶ()
        {
            var moves = new[] { TestMonsters.Wait, TestMonsters.Strike, TestMonsters.Finisher, TestMonsters.QuickStrike };
            var user = TestMonsters.Create("つかうがわ", 50, moves, MonsterType.Normal);
            var state = Battle(TestMonsters.Party(user), TestMonsters.Create("まと", 50, TestMonsters.Strike));
            var cpu = new CpuPlayer(new Random(3), CpuLevel.Weak);

            var chosen = Enumerable.Range(0, Trials).Select(_ => cpu.ChooseAction(state.Sides[0], state.Sides[1]).Index).Distinct();

            Assert.AreEqual(moves.Length, chosen.Count());
        }

        [Test]
        public void つよいは不利なら相性のいい控えに交代する()
        {
            var state = Battle(IceIceSea(), FireFoe());

            var action = Act(CpuLevel.Strong, state);

            Assert.AreEqual(BattleActionKind.Switch, action.Kind);
            Assert.AreEqual(2, action.Index);
        }

        [Test]
        public void つよいでも不利でなければ交代しない()
        {
            var party = new[] { Mono("うみ", MonsterType.Sea, SeaHit), Mono("こおり1", MonsterType.Ice, IceHit), Mono("こおり2", MonsterType.Ice, IceHit) };
            var state = Battle(party, FireFoe());

            Assert.AreEqual(BattleActionKind.Move, Act(CpuLevel.Strong, state).Kind);
        }

        [Test]
        public void つよいでも控えが全員不利なら交代しない()
        {
            var party = new[] { Mono("こおり1", MonsterType.Ice, IceHit), Mono("こおり2", MonsterType.Ice, IceHit), Mono("こおり3", MonsterType.Ice, IceHit) };
            var state = Battle(party, FireFoe());

            Assert.AreEqual(BattleActionKind.Move, Act(CpuLevel.Strong, state).Kind);
        }

        [Test]
        public void ふつうは不利でも交代しない()
        {
            var state = Battle(IceIceSea(), FireFoe());

            Assert.AreEqual(BattleActionKind.Move, Act(CpuLevel.Normal, state).Kind);
        }

        [Test]
        public void ひんしのあと_つよいは相性のいいペンモン_それ以外は控えの先頭を出す()
        {
            var state = Battle(IceIceSea(), FireFoe());
            state.Sides[0].Active.TakeDamage(int.MaxValue);

            Assert.AreEqual(2, new CpuPlayer(new Random(1), CpuLevel.Strong).ChooseReplacement(state.Sides[0], state.Sides[1]));
            Assert.AreEqual(1, new CpuPlayer(new Random(1), CpuLevel.Weak).ChooseReplacement(state.Sides[0], state.Sides[1]));
        }

        [Test]
        public void ドラフトで強く取る確率が100なら種族値の合計が一番高いのを取る()
        {
            var offer = new List<MonsterDefinition> { Flat("ひくい", 50), Flat("たかい", 90), Flat("ふつう", 70) };
            var cpu = new CpuPlayer(new Random(5));

            for (int i = 0; i < Trials; i++)
            {
                Assert.AreEqual(1, cpu.ChooseDraftPick(offer, BattleBalance.PercentMax));
            }
        }

        [Test]
        public void ドラフトで強く取る確率が0ならランダムに取る()
        {
            var offer = new List<MonsterDefinition> { Flat("ひくい", 50), Flat("たかい", 90), Flat("ふつう", 70) };
            var cpu = new CpuPlayer(new Random(6));

            var picked = Enumerable.Range(0, Trials).Select(_ => cpu.ChooseDraftPick(offer, 0)).Distinct();

            Assert.AreEqual(offer.Count, picked.Count());
        }

        [Test]
        public void 見込みダメージは変化技と効かない技で0_命中が低いと下がる()
        {
            var user = new BattleMonster(TestMonsters.Create("つかうがわ", 50, TestMonsters.Strike));
            var normal = new BattleMonster(TestMonsters.Create("まと", 50, TestMonsters.Strike));
            var aurora = new BattleMonster(Mono("オーロラ", MonsterType.Aurora, TestMonsters.Strike));
            var halfHit = new MoveDefinition("テストはずれやすい", MonsterType.Normal, MoveCategory.Physical, 50, 50);

            Assert.AreEqual(0f, DamageCalculator.Expected(user, normal, TestMonsters.Wait));
            Assert.AreEqual(0f, DamageCalculator.Expected(user, aurora, TestMonsters.Strike));
            Assert.AreEqual(DamageCalculator.Expected(user, normal, TestMonsters.Strike) / 2f,
                DamageCalculator.Expected(user, normal, halfHit), 0.001f);
        }

        /// <summary>種族値が全部 each のペンモン（合計で強さを比べるテスト用）</summary>
        private static MonsterDefinition Flat(string name, int each)
        {
            return TestMonsters.Create(name, each, new[] { TestMonsters.Strike, TestMonsters.Strike, TestMonsters.Strike, TestMonsters.Strike },
                MonsterType.Normal, each, each);
        }
    }
}
