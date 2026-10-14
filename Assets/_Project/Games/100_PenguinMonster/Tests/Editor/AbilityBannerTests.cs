using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static PenguinMonster.Battle.Tests.TestMonsters;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// 特性が働いたときの帯（AbilityActivated）が、文の無かった特性でも出るか（仕様書 §4.6 / §7）。陣営0 は速く、陣営1 は遅い
    /// </summary>
    public class AbilityBannerTests
    {
        private static readonly MoveDefinition RecoilHit = new MoveDefinition("テスト反動", MonsterType.Normal, MoveCategory.Physical,
            80, 100, recoils: true);
        private static readonly MoveDefinition MultiHit = new MoveDefinition("テスト連続", MonsterType.Normal, MoveCategory.Physical,
            10, 100, minHits: 2, maxHits: 5);
        private static readonly MoveDefinition SleepMove = StatusMove("テストねむり", MoveEffect.Ailment(StatusCondition.Sleep));
        private static readonly MoveDefinition SnowMove = StatusMove("テスト雪", MoveEffect.SetWeather(Weather.Snow));

        private static MonsterDefinition Fast(MoveDefinition move, Ability ability = Ability.None)
            => With(Create("速", 90, move), ability);

        private static MonsterDefinition Slow(Ability ability = Ability.None, MoveDefinition move = null)
            => With(Create("遅", 30, move ?? Wait), ability);

        private static BattleEngine CreateEngine(MonsterDefinition mon0, MonsterDefinition mon1)
        {
            return new BattleEngine(new BattleState(Party(mon0), Party(mon1)), new Random(1));
        }

        private static List<BattleEvent> Turn(BattleEngine engine)
        {
            return engine.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
        }

        private static int Banners(List<BattleEvent> events, int side)
        {
            return events.Count(e => e.Kind == BattleEventKind.AbilityActivated && e.Side == side);
        }

        /// <summary>帯がその種類のイベントより前に出ているか</summary>
        private static bool BannerBefore(List<BattleEvent> events, int side, BattleEventKind next)
        {
            int banner = events.FindIndex(e => e.Kind == BattleEventKind.AbilityActivated && e.Side == side);
            int target = events.FindIndex(e => e.Kind == next);
            return banner >= 0 && target >= 0 && banner < target;
        }

        [Test]
        public void 特性の帯は文を出さない()
        {
            var engine = CreateEngine(Fast(Wait, Ability.Intimidate), Slow());
            var activated = engine.StartBattle().First(e => e.Kind == BattleEventKind.AbilityActivated);

            Assert.IsNull(BattleMessages.Format(activated, 0));
        }

        [Test]
        public void 力持ちは物理技のダメージの前に帯が出る()
        {
            var engine = CreateEngine(Fast(Strike, Ability.HugePower), Slow());

            var events = Turn(engine);

            Assert.AreEqual(1, Banners(events, 0));
            Assert.IsTrue(BannerBefore(events, 0, BattleEventKind.Damaged));
        }

        [Test]
        public void 力持ちは特殊技では帯が出ない()
        {
            var engine = CreateEngine(Fast(FireStrike, Ability.HugePower), Slow());

            Assert.AreEqual(0, Banners(Turn(engine), 0));
        }

        [Test]
        public void 仇討ちは倒れた味方がいないと帯が出ない()
        {
            var engine = CreateEngine(Fast(Strike, Ability.Avenger), Slow());

            Assert.AreEqual(0, Banners(Turn(engine), 0));
        }

        [Test]
        public void ふかふか脂肪は火の技を受けると帯が出る()
        {
            var engine = CreateEngine(Fast(FireStrike), Slow(Ability.ThickFat));

            var events = Turn(engine);

            Assert.AreEqual(1, Banners(events, 1));
            Assert.IsTrue(BannerBefore(events, 1, BattleEventKind.Damaged));
        }

        [Test]
        public void まっさらは連続技でも1回だけ帯が出る()
        {
            var engine = CreateEngine(Fast(MultiHit, Ability.SkillLink), Slow(Ability.Multiscale));

            var events = Turn(engine);

            Assert.AreEqual(1, Banners(events, 1));
            Assert.AreEqual(1, Banners(events, 0), "連打名人の帯も1回だけ");
        }

        [Test]
        public void 石頭は反動を受けなかったときに帯が出る()
        {
            var engine = CreateEngine(Fast(RecoilHit, Ability.RockHead), Slow());

            var events = Turn(engine);

            Assert.AreEqual(1, Banners(events, 0));
            Assert.IsFalse(events.Any(e => e.Kind == BattleEventKind.Recoil));
        }

        [Test]
        public void 夜更かしは眠らせる技を防いだときに帯が出る()
        {
            var engine = CreateEngine(Fast(SleepMove), Slow(Ability.NightOwl));

            var events = Turn(engine);

            Assert.IsTrue(BannerBefore(events, 1, BattleEventKind.Failed));
        }

        [Test]
        public void ひねくれ者は能力が逆に変わる前に帯が出る()
        {
            var engine = CreateEngine(Fast(Wait, Ability.Intimidate), Slow(Ability.Contrary));

            var events = engine.StartBattle();

            Assert.AreEqual(1, Banners(events, 1));
            Assert.IsTrue(BannerBefore(events, 1, BattleEventKind.StatChanged));
        }

        [Test]
        public void ちゃっかりは変化技の技名の前に帯が出る()
        {
            var engine = CreateEngine(Fast(Strike), Slow(Ability.Prankster));

            var events = Turn(engine);

            Assert.IsTrue(BannerBefore(events, 1, BattleEventKind.MoveUsed));
        }

        [Test]
        public void 雪滑りは吹雪が始まったときに帯が出る()
        {
            var engine = CreateEngine(Fast(SnowMove), Slow(Ability.SnowSlide));

            var events = Turn(engine);

            int started = events.FindIndex(e => e.Kind == BattleEventKind.WeatherStarted);
            Assert.AreEqual(BattleEventKind.AbilityActivated, events[started + 1].Kind);
            Assert.AreEqual(1, events[started + 1].Side);
        }

        [Test]
        public void ひと休みは減っているときだけ戻る前に帯が出る()
        {
            var resting = With(Create("休", 90, Wait), Ability.Rest);
            var engine = new BattleEngine(new BattleState(new[] { resting, resting, resting }, Party(Slow())), new Random(1));
            engine.State.Sides[0].Active.TakeDamage(100);

            var damaged = engine.ResolveTurn(BattleAction.SwitchTo(1), BattleAction.UseMove(0));
            var full = engine.ResolveTurn(BattleAction.SwitchTo(2), BattleAction.UseMove(0));

            Assert.IsTrue(BannerBefore(damaged, 0, BattleEventKind.Withdrawn));
            Assert.AreEqual(0, Banners(full, 0));
        }
    }
}
