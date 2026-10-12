using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 段階4（150体）で足した特性（仕様書 §4.6）。陣営0 は速く、陣営1 は遅い
    /// </summary>
    public class Stage4AbilityTests
    {
        private static readonly MoveDefinition RecoilHit = new MoveDefinition("テスト反動", MonsterType.Normal, MoveCategory.Physical,
            80, 100, recoils: true);
        private static readonly MoveDefinition StrongHit = new MoveDefinition("テスト強打", MonsterType.Normal, MoveCategory.Physical,
            80, 100);
        private static readonly MoveDefinition SpeedUp = StatusMove("テストはやく", MoveEffect.SelfStat(StatKind.Speed, 1));

        private static MonsterDefinition Fast(MoveDefinition move, Ability ability = Ability.None)
            => With(Create("速", 90, move), ability);

        private static MonsterDefinition Slow(MoveDefinition move, Ability ability = Ability.None)
            => With(Create("遅", 30, move), ability);

        private static BattleEngine CreateEngine(MonsterDefinition mon0, MonsterDefinition mon1, int seed = 1)
        {
            return new BattleEngine(new BattleState(Party(mon0), Party(mon1)), new Random(seed));
        }

        private static List<BattleEvent> Turn(BattleEngine engine)
        {
            return engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
        }

        private static BattleMonster Active(BattleEngine engine, int side) => engine.State.Sides[side].Active;

        [Test]
        public void 小技名人は威力60以下の技だけ強くなる()
        {
            var technician = new BattleMonster(With(Create("攻", 50, Strike), Ability.Technician));

            AssertModifier(technician, new BattleMonster(Create("守", 50, Strike)), Strike, BattleBalance.TechnicianMultiplier);
            AssertModifier(technician, new BattleMonster(Create("守", 50, Strike)), StrongHit, 1f);
        }

        [Test]
        public void 向こう見ずは反動の技だけ強くなる()
        {
            var reckless = new BattleMonster(With(Create("攻", 50, Strike), Ability.Reckless));

            AssertModifier(reckless, new BattleMonster(Create("守", 50, Strike)), RecoilHit, BattleBalance.RecklessMultiplier);
            AssertModifier(reckless, new BattleMonster(Create("守", 50, Strike)), StrongHit, 1f);
        }

        [Test]
        public void 向こう見ずは反動を受ける()
        {
            var engine = CreateEngine(Fast(RecoilHit, Ability.Reckless), Slow(Wait));

            var events = Turn(engine);

            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.Recoil && e.Side == 0));
        }

        [Test]
        public void まっさらはHP満タンのときだけダメージ半分()
        {
            var attacker = new BattleMonster(Create("攻", 50, Strike));
            var defender = new BattleMonster(With(Create("守", 50, Strike), Ability.Multiscale));
            AssertModifier(attacker, defender, Strike, BattleBalance.MultiscaleMultiplier);

            defender.TakeDamage(1);

            AssertModifier(attacker, defender, Strike, 1f);
        }

        [Test]
        public void ひねくれ者は自分で上げた能力が下がる()
        {
            var engine = CreateEngine(Fast(AttackUp, Ability.Contrary), Slow(Wait));

            Turn(engine);

            Assert.AreEqual(-2, Active(engine, 0).GetStage(StatKind.Attack));
        }

        [Test]
        public void ひねくれ者は強面で攻撃が上がる()
        {
            var engine = CreateEngine(Fast(Wait, Ability.Intimidate), Slow(Wait, Ability.Contrary));

            engine.StartBattle();

            Assert.AreEqual(1, Active(engine, 1).GetStage(StatKind.Attack));
        }

        [Test]
        public void 気まぐれはターン終わりに1つ2段上がり別の1つが1段下がる()
        {
            var engine = CreateEngine(Fast(Wait), Slow(Wait, Ability.Moody));

            var events = Turn(engine);

            Assert.IsTrue(events.Any(e => e.Kind == BattleEventKind.AbilityActivated && e.Side == 1));
            var stages = new[] { StatKind.Attack, StatKind.Defense, StatKind.SpAttack, StatKind.SpDefense, StatKind.Speed }
                .Select(s => Active(engine, 1).GetStage(s)).ToList();
            // Wait で 特防 が1段上がっているので、それを除いて数える
            int spDefenseFromWait = 1;
            stages[3] -= spDefenseFromWait;
            Assert.AreEqual(1, stages.Count(s => s == BattleBalance.MoodyRaiseStages));
            Assert.AreEqual(1, stages.Count(s => s == BattleBalance.MoodyLowerStages));
            Assert.AreEqual(0, Active(engine, 1).GetStage(StatKind.Accuracy));
        }

        [Test]
        public void ちゃっかりは遅くても変化技を先に出す()
        {
            var engine = CreateEngine(Fast(Strike), Slow(SpeedUp, Ability.Prankster));

            var events = Turn(engine);

            var moves = events.Where(e => e.Kind == BattleEventKind.MoveUsed).ToList();
            Assert.AreEqual(1, moves[0].Side);
        }

        [Test]
        public void ちゃっかりでも攻撃技はふつうの順番()
        {
            var engine = CreateEngine(Fast(Strike), Slow(Strike, Ability.Prankster));

            var events = Turn(engine);

            var moves = events.Where(e => e.Kind == BattleEventKind.MoveUsed).ToList();
            Assert.AreEqual(0, moves[0].Side);
        }

        private static void AssertModifier(BattleMonster attacker, BattleMonster target, MoveDefinition move, float modifier)
        {
            bool isPhysical = move.Category == MoveCategory.Physical;
            int expected = DamageCalculator.Compute(move.Power,
                isPhysical ? attacker.Stats.Attack : attacker.Stats.SpAttack,
                isPhysical ? target.Stats.Defense : target.Stats.SpDefense,
                false, BattleBalance.DamageRandomMaxPercent, attacker.Definition.HasType(move.Type),
                TypeChart.GetMultiplier(move.Type, target.Definition.Types), modifier: modifier);

            // 命中100の技なので、見込みはダメージそのもの
            Assert.AreEqual((float)expected, DamageCalculator.Expected(attacker, target, move));
        }
    }
}
