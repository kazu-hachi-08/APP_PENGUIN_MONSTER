using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 反動・吸収・連続・回復の技と、それに絡む特性・持ち物（仕様書 §4.2 / §4.6。Phase 11）。陣営0 は速く、陣営1 は遅い
    /// </summary>
    public class MoveMechanicsTests
    {
        // テスト用ペンモンの HP は 種族値50 + 75
        private const int MaxHp = 125;

        private static readonly MoveDefinition RecoilHit = new MoveDefinition("テスト反動", MonsterType.Normal, MoveCategory.Physical,
            80, 100, recoils: true);
        private static readonly MoveDefinition DrainHit = new MoveDefinition("テスト吸収", MonsterType.Normal, MoveCategory.Physical,
            80, 100, drains: true);
        private static readonly MoveDefinition MultiHit = new MoveDefinition("テスト連続", MonsterType.Normal, MoveCategory.Physical,
            10, 100, minHits: 2, maxHits: 5);
        private static readonly MoveDefinition TwiceHit = new MoveDefinition("テスト2回", MonsterType.Normal, MoveCategory.Physical,
            10, 100, minHits: 2, maxHits: 2);
        private static readonly MoveDefinition HealMove = StatusMove("テスト回復", MoveEffect.Heal());

        private static MonsterDefinition Fast(MoveDefinition move, Ability ability = Ability.None, HeldItem item = HeldItem.None)
            => With(Create("速", 90, move), ability, item);

        private static MonsterDefinition Slow(Ability ability = Ability.None, HeldItem item = HeldItem.None)
            => With(Create("遅", 30, Wait), ability, item);

        private static BattleEngine CreateEngine(MonsterDefinition mon0, MonsterDefinition mon1, int seed = 1)
        {
            return new BattleEngine(new BattleState(Party(mon0), Party(mon1)), new Random(seed));
        }

        private static List<BattleEvent> Turn(BattleEngine engine)
        {
            return engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
        }

        private static BattleMonster Active(BattleEngine engine, int side) => engine.State.Sides[side].Active;

        private static int Dealt(BattleEngine engine) => MaxHp - Active(engine, 1).CurrentHp;

        [Test]
        public void 反動の技は与えたダメージの3分の1を受ける()
        {
            var engine = CreateEngine(Fast(RecoilHit), Slow());

            var events = Turn(engine);

            Assert.AreEqual(MaxHp - Dealt(engine) / BattleBalance.RecoilDenominator, Active(engine, 0).CurrentHp);
            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.Recoil && e.Side == 0));
        }

        [Test]
        public void 石頭なら反動を受けない()
        {
            var engine = CreateEngine(Fast(RecoilHit, Ability.RockHead), Slow());

            var events = Turn(engine);

            Assert.AreEqual(MaxHp, Active(engine, 0).CurrentHp);
            Assert.IsFalse(events.Any(e => e.Kind == BattleEventKind.Recoil));
        }

        [Test]
        public void 吸収の技は与えたダメージの半分を回復する()
        {
            var engine = CreateEngine(Fast(DrainHit), Slow());
            Active(engine, 0).TakeDamage(80);

            var events = Turn(engine);

            Assert.AreEqual(MaxHp - 80 + Dealt(engine) / BattleBalance.DrainDenominator, Active(engine, 0).CurrentHp);
            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.Drained && e.Side == 0));
        }

        [Test]
        public void HPが満タンなら吸収の文は出ない()
        {
            var engine = CreateEngine(Fast(DrainHit), Slow());

            var events = Turn(engine);

            Assert.IsFalse(events.Any(e => e.Kind == BattleEventKind.Drained));
        }

        [Test]
        public void 連続技は2から5回当たり_回数の文を出す()
        {
            var seen = new HashSet<int>();
            for (int seed = 0; seed < 50; seed++)
            {
                var engine = CreateEngine(Fast(MultiHit), Slow(), seed);

                var events = Turn(engine);

                var count = events.Single(e => e.Kind == BattleEventKind.HitCount);
                Assert.AreEqual(1, count.Side);
                Assert.AreEqual(count.Amount, events.Count(e => e.Kind == BattleEventKind.Damaged && e.Side == 1));
                seen.Add(count.Amount);
            }
            CollectionAssert.AreEquivalent(new[] { 2, 3, 4, 5 }, seen);
        }

        [Test]
        public void 回数が決まった連続技はいつもその回数()
        {
            var engine = CreateEngine(Fast(TwiceHit), Slow());

            var events = Turn(engine);

            Assert.AreEqual(2, events.Single(e => e.Kind == BattleEventKind.HitCount).Amount);
        }

        [Test]
        public void 連打名人なら連続技はいつも5回()
        {
            for (int seed = 0; seed < 10; seed++)
            {
                var engine = CreateEngine(Fast(MultiHit, Ability.SkillLink), Slow(), seed);

                var events = Turn(engine);

                Assert.AreEqual(5, events.Single(e => e.Kind == BattleEventKind.HitCount).Amount);
            }
        }

        [Test]
        public void 連続技はとげとげ肌を当たった回数ぶん受ける()
        {
            var engine = CreateEngine(Fast(MultiHit, Ability.SkillLink), Slow(Ability.SpikySkin));

            var events = Turn(engine);

            int spikes = MaxHp / BattleBalance.SpikySkinDamageDenominator;
            Assert.AreEqual(5, events.Count(e => e.Kind == BattleEventKind.AbilityDamage));
            Assert.AreEqual(MaxHp - spikes * 5, Active(engine, 0).CurrentHp);
        }

        [Test]
        public void 連続技は踏ん張りマフラーを1回目で使わせて次で倒せる()
        {
            var heavyTwice = new MoveDefinition("テスト重い2回", MonsterType.Normal, MoveCategory.Physical, 250, 100,
                minHits: 2, maxHits: 2);
            var engine = CreateEngine(Fast(heavyTwice), Slow(item: HeldItem.GritScarf));

            var events = Turn(engine);

            // 1回目で HP1 に残り、2回目で倒れる（満タンのときしか持ちこたえないため）
            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.ItemEndured));
            Assert.IsTrue(Active(engine, 1).IsFainted);
        }

        [Test]
        public void 回復の技は最大HPの半分回復し_満タンなら決まらない()
        {
            var engine = CreateEngine(Fast(HealMove), Slow());
            Active(engine, 0).TakeDamage(100);

            var healed = Turn(engine);
            Turn(engine);
            var failed = Turn(engine);

            Assert.IsTrue(healed.Any(e => e.Kind == BattleEventKind.Healed && e.Hp == MaxHp - 100 + MaxHp / 2));
            Assert.AreEqual(MaxHp, Active(engine, 0).CurrentHp);
            Assert.IsTrue(failed.Any(e => e.Kind == BattleEventKind.Failed && e.Side == 0));
        }

        [Test]
        public void ひと休みは交代で引っ込むと3分の1回復する()
        {
            var resting = With(Create("休", 90, Wait), Ability.Rest);
            var engine = new BattleEngine(new BattleState(new[] { resting, resting, resting }, Party(Slow())), new Random(1));
            var leaving = Active(engine, 0);
            leaving.TakeDamage(100);

            var events = engine.ResolveTurn(BattleAction.SwitchTo(1), BattleAction.UseMove(0));

            Assert.AreEqual(MaxHp - 100 + MaxHp / BattleBalance.RestHealDenominator, leaving.CurrentHp);
            Assert.AreEqual(leaving.CurrentHp, events.Single(e => e.Kind == BattleEventKind.Withdrawn).Hp);
        }

        [Test]
        public void 貝の首飾りは与えたダメージの8分の1回復する()
        {
            var engine = CreateEngine(Fast(Strike, item: HeldItem.ShellNecklace), Slow());
            Active(engine, 0).TakeDamage(80);

            Turn(engine);

            Assert.AreEqual(MaxHp - 80 + Math.Max(1, Dealt(engine) / BattleBalance.ShellNecklaceHealDenominator),
                Active(engine, 0).CurrentHp);
        }

        [Test]
        public void 図鑑の説明に連続_吸収_反動_回復が出る()
        {
            Assert.AreEqual("2〜5回 当たる", MultiHit.Effects());
            Assert.AreEqual("2回 当たる", TwiceHit.Effects());
            Assert.AreEqual("与えた ダメージの 半分 回復する", DrainHit.Effects());
            Assert.AreEqual("与えた ダメージの 1/3を 自分も 受ける", RecoilHit.Effects());
            Assert.AreEqual("自分の HPを 半分 回復する", HealMove.Effects());
        }
    }
}
