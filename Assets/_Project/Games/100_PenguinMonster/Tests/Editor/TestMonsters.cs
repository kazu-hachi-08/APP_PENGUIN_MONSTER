using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// テスト用のペンモン。本番の定義表は数値が変わるので、テストはここの値で確かめる
    /// </summary>
    internal static class TestMonsters
    {
        public static readonly MoveDefinition Strike = new MoveDefinition("テストうち", MonsterType.Normal, MoveCategory.Physical, 50, 100);
        public static readonly MoveDefinition QuickStrike = new MoveDefinition("テストはやうち", MonsterType.Normal, MoveCategory.Physical, 40, 100, priority: 1);
        public static readonly MoveDefinition Finisher = new MoveDefinition("テストひっさつ", MonsterType.Normal, MoveCategory.Physical, 250, 100);
        public static readonly MoveDefinition FireStrike = new MoveDefinition("テストほのお", MonsterType.Fire, MoveCategory.Special, 40, 100);

        /// <summary>何もしないのと同じ変化技（物理の技しか使わないテストでは、とくぼう が上がっても影響しない）</summary>
        public static readonly MoveDefinition Wait = StatusMove("テストまつ", MoveEffect.SelfStat(StatKind.SpDefense, 1));
        public static readonly MoveDefinition AttackUp = StatusMove("テストつよく", MoveEffect.SelfStat(StatKind.Attack, 2));
        public static readonly MoveDefinition SleepMove = StatusMove("テストねむり", MoveEffect.Ailment(StatusCondition.Sleep));
        public static readonly MoveDefinition SnowMove = StatusMove("テスト雪", MoveEffect.SetWeather(Weather.Snow));

        public static readonly MoveDefinition RecoilHit = new MoveDefinition("テスト反動", MonsterType.Normal, MoveCategory.Physical,
            80, 100, recoils: true);
        public static readonly MoveDefinition MultiHit = new MoveDefinition("テスト連続", MonsterType.Normal, MoveCategory.Physical,
            10, 100, minHits: 2, maxHits: 5);
        public static readonly MoveDefinition Spread = new MoveDefinition("テストぜんたい", MonsterType.Normal, MoveCategory.Special, 50, 100,
            hitsAllFoes: true);

        public static MoveDefinition StatusMove(string name, MoveEffect effect)
        {
            return new MoveDefinition(name, MonsterType.Normal, MoveCategory.Status, 0, 100, effects: new[] { effect });
        }

        public static MonsterDefinition Create(string name, int speed, MoveDefinition move, int attack = 50, int defense = 50)
        {
            return Create(name, speed, new[] { move, move, move, move }, MonsterType.Normal, attack, defense);
        }

        public static MonsterDefinition Create(string name, int speed, MoveDefinition[] moves, MonsterType type,
            int attack = 50, int defense = 50)
        {
            return new MonsterDefinition(0, name, new[] { type }, new Stats(50, attack, defense, 50, 50, speed), moves);
        }

        /// <summary>特性・持ち物だけを変えた写し</summary>
        public static MonsterDefinition With(MonsterDefinition monster, Ability ability = Ability.None, HeldItem item = HeldItem.None)
        {
            return new MonsterDefinition(monster.No, monster.Name, monster.Types.ToArray(), monster.BaseStats, monster.Moves.ToArray(),
                ability, item);
        }

        public static MonsterDefinition[] Party(string prefix, int speed, MoveDefinition move, int attack = 50, int defense = 50)
        {
            return new[]
            {
                Create(prefix + "1", speed, move, attack, defense),
                Create(prefix + "2", speed, move, attack, defense),
                Create(prefix + "3", speed, move, attack, defense),
            };
        }

        public static MonsterDefinition[] Party(MonsterDefinition monster)
        {
            return new[] { monster, monster, monster };
        }
    }

    /// <summary>
    /// 1対1のバトルを組んで1ターンずつ進めるテストの共通部分。陣営0 は速く、陣営1 は遅い
    /// </summary>
    internal static class TestBattle
    {
        // テスト用ペンモンの HP は 種族値50 + 75
        public const int MaxHp = 125;

        private const int FastSpeed = 90;
        private const int SlowSpeed = 30;

        public static MonsterDefinition Fast(MoveDefinition move, Ability ability = Ability.None, HeldItem item = HeldItem.None,
            MonsterType type = MonsterType.Normal)
        {
            return TestMonsters.With(TestMonsters.Create("速", FastSpeed, new[] { move, move, move, move }, type), ability, item);
        }

        /// <summary>技を渡さなければ何もしない（Wait だけを持つ）</summary>
        public static MonsterDefinition Slow(Ability ability = Ability.None, HeldItem item = HeldItem.None,
            MonsterType type = MonsterType.Normal, MoveDefinition move = null)
        {
            var only = move ?? TestMonsters.Wait;
            return TestMonsters.With(TestMonsters.Create("遅", SlowSpeed, new[] { only, only, only, only }, type), ability, item);
        }

        /// <summary>どちらも同じ子を3体ずつ並べる</summary>
        public static BattleEngine CreateEngine(MonsterDefinition mon0, MonsterDefinition mon1, int seed = 1)
        {
            return new BattleEngine(new BattleState(TestMonsters.Party(mon0), TestMonsters.Party(mon1)), new Random(seed));
        }

        public static List<BattleEvent> Turn(BattleEngine engine, int move0 = 0, int move1 = 0)
        {
            return engine.ResolveTurn(BattleAction.UseMove(move0), BattleAction.UseMove(move1));
        }

        public static BattleMonster Active(BattleEngine engine, int side) => engine.State.Sides[side].Active;

        public static bool Has(List<BattleEvent> events, BattleEventKind kind) => events.Any(e => e.Kind == kind);

        public static bool Has(List<BattleEvent> events, BattleEventKind kind, int side)
        {
            return events.Any(e => e.Kind == kind && e.Side == side);
        }

        /// <summary>CPU 用の見込みダメージ（乱数なし）で、倍率が最後に掛かっているかを確かめる</summary>
        public static void AssertModifier(MonsterDefinition attacker, MonsterDefinition defender, MoveDefinition move, float modifier)
        {
            AssertModifier(new BattleMonster(attacker), new BattleMonster(defender), move, modifier);
        }

        public static void AssertModifier(BattleMonster attacker, MonsterDefinition defender, MoveDefinition move, float modifier)
        {
            AssertModifier(attacker, new BattleMonster(defender), move, modifier);
        }

        public static void AssertModifier(BattleMonster attacker, BattleMonster target, MoveDefinition move, float modifier)
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
