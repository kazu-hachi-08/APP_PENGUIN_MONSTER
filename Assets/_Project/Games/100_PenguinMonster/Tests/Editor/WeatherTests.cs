using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 天候（仕様書 §4.10）。陣営0 は速く、陣営1 は遅い
    /// </summary>
    public class WeatherTests
    {
        private const int MaxHp = 125;

        private static readonly MoveDefinition SnowMove = StatusMove("テスト雪", MoveEffect.SetWeather(Weather.Snow));
        private static readonly MoveDefinition RainMove = StatusMove("テスト雨", MoveEffect.SetWeather(Weather.Rain));
        private static readonly MoveDefinition SeaHit = new MoveDefinition("テスト海", MonsterType.Sea, MoveCategory.Special, 50, 100);
        private static readonly MoveDefinition FireHit = new MoveDefinition("テスト火", MonsterType.Fire, MoveCategory.Special, 50, 100);

        private static MonsterDefinition Fast(MoveDefinition move, Ability ability = Ability.None, HeldItem item = HeldItem.None,
            MonsterType type = MonsterType.Normal)
            => With(Create("速", 90, new[] { move, move, move, move }, type), ability, item);

        private static MonsterDefinition Slow(Ability ability = Ability.None, MonsterType type = MonsterType.Normal)
            => With(Create("遅", 30, new[] { Wait, Wait, Wait, Wait }, type), ability);

        private static BattleEngine CreateEngine(MonsterDefinition mon0, MonsterDefinition mon1)
        {
            return new BattleEngine(new BattleState(Party(mon0), Party(mon1)), new Random(1));
        }

        private static List<BattleEvent> Turn(BattleEngine engine, int move0 = 0)
        {
            return engine.ResolveTurn(BattleAction.UseMove(move0), BattleAction.UseMove(0));
        }

        private static BattleMonster Active(BattleEngine engine, int side) => engine.State.Sides[side].Active;

        private static bool Has(List<BattleEvent> events, BattleEventKind kind) => events.Any(e => e.Kind == kind);

        [Test]
        public void 天候の技で天候になり_5ターン目の終わりに止む()
        {
            var engine = CreateEngine(Fast(RainMove), Slow());

            var first = Turn(engine);
            Assert.AreEqual(Weather.Rain, engine.State.Weather);
            Assert.AreEqual(Weather.Rain, first.Single(e => e.Kind == BattleEventKind.WeatherStarted).Weather);
            for (int turn = 2; turn < BattleBalance.WeatherTurns; turn++)
            {
                Assert.IsFalse(Has(engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0)), BattleEventKind.WeatherEnded));
            }
            // 同じ天候の技はもう決まらない
            var last = Turn(engine);

            Assert.IsTrue(Has(last, BattleEventKind.Failed));
            Assert.AreEqual(Weather.Rain, last.Single(e => e.Kind == BattleEventKind.WeatherEnded).Weather);
            Assert.AreEqual(Weather.None, engine.State.Weather);
        }

        [Test]
        public void お天気石を持った子が始めた天候は長く続く()
        {
            var engine = CreateEngine(Fast(RainMove, item: HeldItem.WeatherStone), Slow());

            Turn(engine);

            Assert.AreEqual(BattleBalance.WeatherStoneTurns - 1, engine.State.WeatherTurns);
        }

        [Test]
        public void 吹雪は氷タイプ以外にターン終わりのダメージ()
        {
            var engine = CreateEngine(Fast(SnowMove, type: MonsterType.Ice), Slow());

            var events = Turn(engine);

            Assert.AreEqual(MaxHp, Active(engine, 0).CurrentHp);
            Assert.AreEqual(MaxHp - MaxHp / BattleBalance.SnowDamageDenominator, Active(engine, 1).CurrentHp);
            Assert.AreEqual(1, events.Count(e => e.Kind == BattleEventKind.WeatherDamage));
        }

        [Test]
        public void 雪雲呼びは場に出たとき吹雪にし_自分は吹雪のダメージを受けない()
        {
            var engine = CreateEngine(Fast(Wait, Ability.SnowCloud), Slow());

            var start = engine.StartBattle();
            Turn(engine);

            Assert.AreEqual(BattleEventKind.AbilityActivated, start[start.Count - 2].Kind);
            Assert.AreEqual(BattleEventKind.WeatherStarted, start[start.Count - 1].Kind);
            Assert.AreEqual(MaxHp, Active(engine, 0).CurrentHp);
        }

        [Test]
        public void 後から出た天候の特性が上書きする()
        {
            var engine = CreateEngine(Fast(Wait, Ability.SnowCloud), Slow(Ability.RainCall));

            engine.StartBattle();

            // 場に出たときの特性は素早さの高い順なので、遅い側の大雨が残る
            Assert.AreEqual(Weather.Rain, engine.State.Weather);
        }

        [Test]
        public void 大雨は海技が強く火技が弱い_日差しはその逆()
        {
            var sea = new BattleMonster(Fast(SeaHit));
            var fire = new BattleMonster(Fast(FireHit));
            var target = new BattleMonster(Slow());
            float seaPlain = DamageCalculator.Expected(sea, target, SeaHit);
            float firePlain = DamageCalculator.Expected(fire, target, FireHit);

            var rain = CreateEngine(Fast(SeaHit, Ability.RainCall), Slow());
            rain.StartBattle();
            var sun = CreateEngine(Fast(SeaHit, Ability.SunCall), Slow());
            sun.StartBattle();

            Assert.That(DamageCalculator.Expected(Active(rain, 0), Active(rain, 1), SeaHit), Is.GreaterThan(seaPlain));
            Assert.That(DamageCalculator.Expected(Active(rain, 0), Active(rain, 1), FireHit), Is.LessThan(firePlain));
            Assert.That(DamageCalculator.Expected(Active(sun, 0), Active(sun, 1), SeaHit), Is.LessThan(seaPlain));
            Assert.That(DamageCalculator.Expected(Active(sun, 0), Active(sun, 1), FireHit), Is.GreaterThan(firePlain));
        }

        [Test]
        public void 吹雪のとき氷タイプは物理技を受けにくい()
        {
            var plain = CreateEngine(Fast(Strike), Slow(type: MonsterType.Ice));
            var snow = CreateEngine(Fast(Strike), Slow(Ability.SnowCloud, MonsterType.Ice));
            snow.StartBattle();

            float before = DamageCalculator.Expected(Active(plain, 0), Active(plain, 1), Strike);
            float after = DamageCalculator.Expected(Active(snow, 0), Active(snow, 1), Strike);

            Assert.That(after, Is.LessThan(before));
        }

        [Test]
        public void 天候に乗る特性はその天候のとき素早さ2倍()
        {
            var engine = CreateEngine(Fast(Wait, Ability.RainCall), Slow(Ability.RainSurf));
            var slow = Active(engine, 1);
            int before = slow.EffectiveSpeed;

            engine.StartBattle();

            Assert.AreEqual(before * BattleBalance.WeatherSpeedMultiplier, slow.EffectiveSpeed);
        }

        [Test]
        public void 図鑑の説明に天候が出る()
        {
            Assert.AreEqual("5ターン 吹雪に する", SnowMove.Effects());
        }

        [Test]
        public void CPUは出てすぐ天候の技を使い_HPが半分以下なら回復の技を使う()
        {
            var heal = StatusMove("テスト回復", MoveEffect.Heal());
            var moves = new[] { Strike, RainMove, heal, Strike };
            var state = new BattleState(Party(Create("CPU", 50, moves, MonsterType.Normal)), Party(Slow()));
            var cpu = new CpuPlayer(new Random(1), CpuLevel.Normal);
            var self = state.Sides[0];

            Assert.AreEqual(1, cpu.ChooseAction(self, state.Sides[1]).Index);
            self.Active.TakeDamage(MaxHp / 2 + 1);
            Assert.AreEqual(2, cpu.ChooseAction(self, state.Sides[1]).Index);
        }
    }
}
