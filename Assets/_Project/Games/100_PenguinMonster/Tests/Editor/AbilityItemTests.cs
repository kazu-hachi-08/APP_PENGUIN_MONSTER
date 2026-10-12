using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 特性・持ち物（仕様書 §4.6）。陣営0 は速く、陣営1 は遅い。何もしない側は Wait だけを持つ
    /// </summary>
    public class AbilityItemTests
    {
        // テスト用ペンモンの HP は 種族値50 + 75
        private const int MaxHp = 125;

        private static readonly MoveDefinition[] Waits = { Wait, Wait, Wait, Wait };

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

        private static bool Has(List<BattleEvent> events, BattleEventKind kind, int side)
        {
            return events.Any(e => e.Kind == kind && e.Side == side);
        }

        // ---- 特性 ----

        [Test]
        public void こわもては場に出たとき相手のこうげきを下げる()
        {
            var engine = CreateEngine(Fast(Wait, Ability.Intimidate), Slow());

            var events = engine.StartBattle();

            Assert.IsTrue(Has(events, BattleEventKind.AbilityActivated, 0));
            Assert.AreEqual(-1, Active(engine, 1).GetStage(StatKind.Attack));
        }

        [Test]
        public void こわもては交代で出ても働く()
        {
            var plain = Create("ふつう", 90, Wait);
            var party0 = new[] { plain, With(plain, Ability.Intimidate), plain };
            var engine = new BattleEngine(new BattleState(party0, Party(Slow())), new Random(1));

            engine.ResolveTurn(BattleAction.SwitchTo(1), BattleAction.UseMove(0));

            Assert.AreEqual(-1, Active(engine, 1).GetStage(StatKind.Attack));
        }

        [Test]
        public void よふかしはねむりにならない()
        {
            var sleep = StatusMove("テストねむり", MoveEffect.Ailment(StatusCondition.Sleep));
            var engine = CreateEngine(Fast(sleep), Slow(Ability.NightOwl));

            var events = Turn(engine);

            Assert.IsTrue(Has(events, BattleEventKind.Failed, 1));
            Assert.AreEqual(StatusCondition.None, Active(engine, 1).Status);
        }

        [Test]
        public void おいかぜのりはターン終わりにすばやさが上がる()
        {
            var engine = CreateEngine(Fast(Wait), Slow(Ability.Tailwind));

            var events = Turn(engine);

            Assert.IsTrue(Has(events, BattleEventKind.AbilityActivated, 1));
            Assert.AreEqual(1, Active(engine, 1).GetStage(StatKind.Speed));
        }

        [Test]
        public void いきおいづくは相手をたおすとこうげきが上がる()
        {
            var engine = CreateEngine(Fast(Finisher, Ability.Momentum), Slow());

            var events = Turn(engine);

            Assert.IsTrue(Has(events, BattleEventKind.Fainted, 1));
            Assert.IsTrue(Has(events, BattleEventKind.AbilityActivated, 0));
            Assert.AreEqual(1, Active(engine, 0).GetStage(StatKind.Attack));
        }

        [Test]
        public void とげとげはだは物理で殴った相手に8分の1のダメージ()
        {
            var engine = CreateEngine(Fast(Strike), Slow(Ability.SpikySkin));

            var events = Turn(engine);

            Assert.IsTrue(Has(events, BattleEventKind.AbilityDamage, 0));
            Assert.AreEqual(MaxHp - MaxHp / 8, Active(engine, 0).CurrentHp);
        }

        [Test]
        public void とげとげはだは特殊技では働かない()
        {
            var engine = CreateEngine(Fast(FireStrike), Slow(Ability.SpikySkin));

            Turn(engine);

            Assert.AreEqual(MaxHp, Active(engine, 0).CurrentHp);
        }

        [TestCase(Ability.StaticBody, StatusCondition.Paralysis)]
        [TestCase(Ability.PoisonSlime, StatusCondition.Poison)]
        public void 物理で殴ると状態異常をもらうことがある(Ability ability, StatusCondition expected)
        {
            // 30% なので、たおれないよう硬くして何ターンか殴らせる
            var tough = new MonsterDefinition(0, "かたい", new[] { MonsterType.Normal }, new Stats(255, 50, 255, 50, 50, 30), Waits,
                ability: ability);
            var engine = CreateEngine(Fast(Strike), tough);
            var attacker = Active(engine, 0);

            bool activated = false;
            for (int turn = 0; turn < 20 && attacker.Status == StatusCondition.None; turn++)
            {
                activated |= Has(Turn(engine), BattleEventKind.AbilityActivated, 1);
            }

            Assert.IsTrue(activated);
            Assert.AreEqual(expected, attacker.Status);
        }

        [Test]
        public void かたいはねは急所に当たらない()
        {
            var attacker = new BattleMonster(Create("攻", 50, Strike));
            var defender = new BattleMonster(With(Create("守", 50, Strike), Ability.HardFeathers));
            var random = new Random(1);

            for (int i = 0; i < 500; i++)
            {
                Assert.IsFalse(DamageCalculator.Calculate(attacker, defender, Strike, random).IsCritical);
            }
        }

        [Test]
        public void ふかふかしぼうはこおりとひのダメージ半分()
        {
            AssertModifier(Create("攻", 50, FireStrike), With(Create("守", 50, Strike), Ability.ThickFat), FireStrike,
                BattleBalance.ThickFatMultiplier);
            AssertModifier(Create("攻", 50, Strike), With(Create("守", 50, Strike), Ability.ThickFat), Strike, 1f);
        }

        [Test]
        public void せとぎわはHPが3分の1以下でタイプ一致が強くなる()
        {
            var attacker = new BattleMonster(With(Create("攻", 50, Strike), Ability.LastStand));
            var defender = Create("守", 50, Strike);
            AssertModifier(attacker, defender, Strike, 1f);

            attacker.TakeDamage(attacker.MaxHp - attacker.MaxHp / BattleBalance.LastStandHpDenominator);

            AssertModifier(attacker, defender, Strike, BattleBalance.LastStandMultiplier);
        }

        [Test]
        public void 力持ちは物理技だけダメージ2倍()
        {
            var strong = With(Create("攻", 50, Strike), Ability.HugePower);
            var defender = Create("守", 50, Strike);

            AssertModifier(strong, defender, Strike, BattleBalance.HugePowerMultiplier);
            AssertModifier(strong, defender, FireStrike, 1f);
        }

        [Test]
        public void 仇討ちは瀕死の味方1体ごとに強くなる()
        {
            var avenger = With(Create("仇", 50, Strike), Ability.Avenger);
            var plain = Create("味方", 50, Strike);
            var state = new BattleState(new[] { plain, plain, avenger }, Party(Slow()));
            var party = state.Sides[0].Party;
            var defender = Create("守", 50, Strike);
            AssertModifier(party[2], defender, Strike, 1f);

            party[0].TakeDamage(party[0].MaxHp);
            AssertModifier(party[2], defender, Strike, BattleBalance.AvengerMultiplierPerFainted);

            party[1].TakeDamage(party[1].MaxHp);
            AssertModifier(party[2], defender, Strike,
                BattleBalance.AvengerMultiplierPerFainted * BattleBalance.AvengerMultiplierPerFainted);
        }

        // ---- 持ち物 ----

        [Test]
        public void ヒナの綿毛は防御と特防が1点5倍で_急所でも消えない()
        {
            var chick = new BattleMonster(Slow(item: HeldItem.ChickDown));
            int defense = (int)(chick.Stats.Defense * BattleBalance.ChickDownDefenseMultiplier);
            int spDefense = (int)(chick.Stats.SpDefense * BattleBalance.ChickDownDefenseMultiplier);

            Assert.AreEqual(defense, chick.DefenseStat(true, false));
            Assert.AreEqual(spDefense, chick.DefenseStat(false, false));
            Assert.AreEqual(defense, chick.DefenseStat(true, true));
        }

        [Test]
        public void ほしざかなはターン終わりに16分の1回復()
        {
            var engine = CreateEngine(Fast(Wait), Slow(item: HeldItem.StarFish));
            Active(engine, 1).TakeDamage(50);

            var events = Turn(engine);

            Assert.IsTrue(Has(events, BattleEventKind.ItemHealed, 1));
            Assert.AreEqual(MaxHp - 50 + MaxHp / 16, Active(engine, 1).CurrentHp);
        }

        [Test]
        public void ほしざかなはHP満タンなら何もしない()
        {
            var engine = CreateEngine(Fast(Wait), Slow(item: HeldItem.StarFish));

            Assert.IsFalse(Has(Turn(engine), BattleEventKind.ItemHealed, 1));
        }

        [Test]
        public void おやつのイカはHPが半分以下で1回だけ回復()
        {
            var engine = CreateEngine(Fast(Strike), Slow(item: HeldItem.SquidSnack));
            var holder = Active(engine, 1);
            holder.TakeDamage(40);

            var first = Turn(engine);
            var second = Turn(engine);

            Assert.IsTrue(Has(first, BattleEventKind.ItemHealed, 1));
            Assert.IsTrue(holder.ItemUsed);
            Assert.IsFalse(Has(second, BattleEventKind.ItemHealed, 1));
        }

        [Test]
        public void さっぱりこんぶは状態異常を1回だけ治す()
        {
            var poison = StatusMove("テストどく", MoveEffect.Ailment(StatusCondition.Poison));
            var engine = CreateEngine(Fast(poison), Slow(item: HeldItem.FreshKelp));
            var holder = Active(engine, 1);

            var first = Turn(engine);
            Assert.IsTrue(Has(first, BattleEventKind.ItemCured, 1));
            Assert.AreEqual(StatusCondition.None, holder.Status);

            Turn(engine);
            Assert.AreEqual(StatusCondition.Poison, holder.Status);
        }

        [Test]
        public void いのちのつららは当てるたびに10分の1減る()
        {
            var engine = CreateEngine(Fast(Strike, item: HeldItem.LifeIcicle), Slow());

            var events = Turn(engine);

            Assert.IsTrue(Has(events, BattleEventKind.ItemRecoil, 0));
            Assert.AreEqual(MaxHp - MaxHp / 10, Active(engine, 0).CurrentHp);
        }

        [Test]
        public void ふんばりマフラーはHP満タンならHP1で残る()
        {
            var engine = CreateEngine(Fast(Finisher), Slow(item: HeldItem.GritScarf));
            var holder = Active(engine, 1);

            var first = Turn(engine);
            Assert.IsTrue(Has(first, BattleEventKind.ItemEndured, 1));
            Assert.AreEqual(1, holder.CurrentHp);

            Assert.IsTrue(Has(Turn(engine), BattleEventKind.Fainted, 1));
        }

        [Test]
        public void かるいはねはすばやさ1点2倍()
        {
            var monster = new BattleMonster(Fast(Wait, item: HeldItem.LightFeather));

            Assert.AreEqual((int)(monster.Stats.Speed * BattleBalance.LightFeatherSpeedMultiplier), monster.EffectiveSpeed);
        }

        [Test]
        public void はねは物理と特殊で別々に強くなる()
        {
            var defender = Create("守", 50, Strike);
            AssertModifier(With(Create("攻", 50, Strike), item: HeldItem.BraveFeather), defender, Strike, BattleBalance.FeatherMultiplier);
            AssertModifier(With(Create("攻", 50, Strike), item: HeldItem.BraveFeather), defender, FireStrike, 1f);
            AssertModifier(With(Create("攻", 50, Strike), item: HeldItem.WiseFeather), defender, FireStrike, BattleBalance.FeatherMultiplier);
        }

        [Test]
        public void ばつぐんのときだけめききメガネとゆきのコートが働く()
        {
            var moss = Create("こけ", 50, Waits, MonsterType.Moss);
            var normal = Create("ふつう", 50, Strike);
            var glasses = With(Create("攻", 50, Strike), item: HeldItem.SharpGlasses);

            AssertModifier(glasses, moss, FireStrike, BattleBalance.SharpGlassesMultiplier);
            AssertModifier(glasses, normal, FireStrike, 1f);
            AssertModifier(Create("攻", 50, Strike), With(moss, item: HeldItem.SnowCoat), FireStrike, BattleBalance.SnowCoatMultiplier);
        }

        // ---- 定義表・文 ----

        [Test]
        public void 定義表の全員が特性と持ち物を持ち_全種類が使われている()
        {
            foreach (var monster in MonsterTable.All)
            {
                Assert.AreNotEqual(Ability.None, monster.Ability, monster.Name);
                Assert.AreNotEqual(HeldItem.None, monster.Item, monster.Name);
            }
            CollectionAssert.IsSubsetOf(Enum.GetValues(typeof(Ability)).Cast<Ability>().Where(a => a != Ability.None),
                MonsterTable.All.Select(m => m.Ability));
            CollectionAssert.IsSubsetOf(Enum.GetValues(typeof(HeldItem)).Cast<HeldItem>().Where(i => i != HeldItem.None),
                MonsterTable.All.Select(m => m.Item));
        }

        [Test]
        public void 特性が働くと特性の名前が文に出る()
        {
            var engine = CreateEngine(Fast(Wait, Ability.Intimidate), Slow());
            var activated = engine.StartBattle().First(e => e.Kind == BattleEventKind.AbilityActivated);

            Assert.AreEqual("速の 強面！", BattleMessages.Format(activated, 0));
        }

        /// <summary>CPU 用の見込みダメージ（乱数なし）で、倍率が最後に掛かっているかを確かめる</summary>
        private static void AssertModifier(MonsterDefinition attacker, MonsterDefinition defender, MoveDefinition move, float modifier)
        {
            AssertModifier(new BattleMonster(attacker), defender, move, modifier);
        }

        private static void AssertModifier(BattleMonster attacker, MonsterDefinition defender, MoveDefinition move, float modifier)
        {
            var target = new BattleMonster(defender);
            bool isPhysical = move.Category == MoveCategory.Physical;
            int expected = DamageCalculator.Compute(move.Power,
                isPhysical ? attacker.Stats.Attack : attacker.Stats.SpAttack,
                isPhysical ? target.Stats.Defense : target.Stats.SpDefense,
                false, BattleBalance.DamageRandomMaxPercent, attacker.Definition.HasType(move.Type),
                TypeChart.GetMultiplier(move.Type, defender.Types), modifier: modifier);

            // 命中100の技なので、見込みはダメージそのもの
            Assert.AreEqual((float)expected, DamageCalculator.Expected(attacker, target, move));
        }
    }
}
