using System.Linq;

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
}
