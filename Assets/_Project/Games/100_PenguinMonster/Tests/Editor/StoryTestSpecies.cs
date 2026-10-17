using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story.Tests
{
    /// <summary>
    /// ストーリーのテスト用の仮の3体（ヒナ →Lv16→ ワカ →Lv36→ オトナ の1系統）。
    /// 本番の定義表はフェーズ8で埋まるので、テストはここの値で確かめる
    /// </summary>
    internal static class StoryTestSpecies
    {
        public static readonly MoveDefinition Peck = Move("テストつつく");
        public static readonly MoveDefinition Growl = Move("テストなきごえ");
        public static readonly MoveDefinition Splash = Move("テストみずかけ");
        public static readonly MoveDefinition Bite = Move("テストかみつく");
        public static readonly MoveDefinition Kick = Move("テストけり");
        public static readonly MoveDefinition WingStrike = Move("テストつばさ");
        public static readonly MoveDefinition Charge = Move("テストとっしん");
        public static readonly MoveDefinition Finale = Move("テストおおわざ");

        private static readonly LevelMove[] ChickMoves =
        {
            new LevelMove(1, Peck), new LevelMove(1, Growl), new LevelMove(7, Splash), new LevelMove(10, Bite), new LevelMove(13, Kick),
        };
        private static readonly LevelMove[] JuniorMoves =
        {
            new LevelMove(1, Peck), new LevelMove(1, Growl), new LevelMove(7, Splash), new LevelMove(10, Bite), new LevelMove(13, Kick),
            new LevelMove(16, WingStrike), new LevelMove(20, Charge),
        };
        private static readonly LevelMove[] AdultMoves =
        {
            new LevelMove(1, Peck), new LevelMove(1, Growl), new LevelMove(7, Splash), new LevelMove(10, Bite), new LevelMove(13, Kick),
            new LevelMove(16, WingStrike), new LevelMove(20, Charge), new LevelMove(36, Finale),
        };

        public static readonly StorySpecies Chick = new StorySpecies(Definition(901, "テストヒナ", new Stats(45, 49, 49, 65, 65, 45)),
            GrowthRate.MediumFast, 64, ChickMoves, new Evolution(16, 902));
        public static readonly StorySpecies Junior = new StorySpecies(Definition(902, "テストワカ", new Stats(60, 62, 63, 80, 80, 60)),
            GrowthRate.MediumFast, 142, JuniorMoves, new Evolution(36, 903));
        public static readonly StorySpecies Adult = new StorySpecies(Definition(903, "テストオトナ", new Stats(80, 82, 83, 100, 100, 80)),
            GrowthRate.MediumFast, 236, AdultMoves);

        private static MoveDefinition Move(string name)
        {
            return new MoveDefinition(name, MonsterType.Normal, MoveCategory.Physical, 40, 100);
        }

        private static MonsterDefinition Definition(int no, string name, Stats baseStats)
        {
            var moves = new[] { Peck, Peck, Peck, Peck };
            return new MonsterDefinition(no, name, new[] { MonsterType.Normal }, baseStats, moves, rank: MonsterRank.Young);
        }

        /// <summary>技を1つだけ覚えた子（空きがあるので、レベルが上がると技を自動で覚える）</summary>
        public static OwnedMonster Owned(StorySpecies species, int level)
        {
            return new OwnedMonster(species, level, new[] { Peck });
        }

        /// <summary>経験値を、そのレベルにちょうど届くだけ与える</summary>
        public static IReadOnlyList<LevelUp> GrowTo(OwnedMonster owned, int level)
        {
            return owned.GainExp(Experience.ForLevel(owned.Species.GrowthRate, level) - owned.Exp);
        }

        /// <summary>バトルでダメージを受けた後の状態にする</summary>
        public static void Hurt(OwnedMonster owned, int damage)
        {
            var fighter = owned.ToBattle();
            fighter.TakeDamage(damage);
            owned.ApplyBattleResult(fighter);
        }
    }
}
