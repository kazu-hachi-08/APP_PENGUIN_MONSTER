using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り1の種の一覧（進化先を No から引く）。本物の No.151〜180 はフェーズ8a で作るので、今は既存の子を仮に使う:
    /// 最初の3体は既存のヒナを進化前（Young）にし、同じタイプの既存のふつうの子へ Lv16 で進化させる。
    /// 野生も既存のヒナを進化前にして出す。進化前にするのは、ストーリーの子にヒナの綿毛を付けないため（PLAN_STORY_TOWN01.md §1）
    /// </summary>
    public static class Town01Species
    {
        // 仮の進化は本家の最初の3体の1段目と同じ Lv16。8 で1つ技を覚え、進化で もう1つ覚える（技を忘れる画面を試せるように）
        private const int EvolveLevel = 16;
        private const int FirstNewMoveLevel = 8;
        // 本家の最初の3体と同じ「やや遅い」・基礎経験値 62（進化後は 142）。序盤の野生は「中速」・50
        private const int StarterBaseExp = 62;
        private const int EvolvedBaseExp = 142;
        private const int WildBaseExp = 50;

        // 仮の最初の3体（ウキワヒナペン・マッチヒナペン・フタバヒナペン）と進化先（ナミペン・ヒノペン・ノリペン）
        public static readonly StorySpecies SeaStarter = Starter(128, 2);
        public static readonly StorySpecies FireStarter = Starter(130, 3);
        public static readonly StorySpecies MossStarter = Starter(132, 19);
        // 仮の野生（ハネヒナペン・ギョウレツヒナペン）
        public static readonly StorySpecies WildBird = Wild(39);
        public static readonly StorySpecies WildNormal = Wild(127);

        private static readonly Dictionary<int, StorySpecies> ByNumber = new Dictionary<int, StorySpecies>();

        static Town01Species()
        {
            foreach (var species in new[] { SeaStarter, FireStarter, MossStarter, WildBird, WildNormal })
            {
                ByNumber[species.No] = species;
            }
            AddEvolved(SeaStarter);
            AddEvolved(FireStarter);
            AddEvolved(MossStarter);
        }

        /// <summary>No から種を引く（進化先を探すのに使う）。無ければ null</summary>
        public static StorySpecies ByNo(int no) => ByNumber.TryGetValue(no, out var species) ? species : null;

        private static StorySpecies Starter(int no, int evolvedNo)
        {
            var source = MonsterTable.ByNo(no);
            return new StorySpecies(Copy(source, MonsterRank.Young), GrowthRate.MediumSlow, StarterBaseExp,
                StarterLearnset(source, MonsterTable.ByNo(evolvedNo)), new Evolution(EvolveLevel, evolvedNo));
        }

        /// <summary>定義表の4つを最初から覚えていて、Lv8 で進化先の1つ目の技を覚える</summary>
        private static LevelMove[] StarterLearnset(MonsterDefinition starter, MonsterDefinition evolved)
        {
            var learnset = FirstMoves(starter);
            learnset.Add(new LevelMove(FirstNewMoveLevel, evolved.Moves[0]));
            return learnset.ToArray();
        }

        /// <summary>進化先は進化前の技の表を引き継ぎ、Lv16 で2つ目の技を覚える（進化した時点で覚える）</summary>
        private static void AddEvolved(StorySpecies starter)
        {
            var source = MonsterTable.ByNo(starter.Evolution.IntoNo);
            var learnset = new List<LevelMove>(starter.Learnset) { new LevelMove(EvolveLevel, source.Moves[1]) };
            var evolved = new StorySpecies(Copy(source, source.Rank), GrowthRate.MediumSlow, EvolvedBaseExp, learnset.ToArray());
            ByNumber[evolved.No] = evolved;
        }

        private static StorySpecies Wild(int no)
        {
            var source = MonsterTable.ByNo(no);
            return new StorySpecies(Copy(source, MonsterRank.Young), GrowthRate.MediumFast, WildBaseExp, FirstMoves(source).ToArray());
        }

        private static List<LevelMove> FirstMoves(MonsterDefinition source)
        {
            var learnset = new List<LevelMove>();
            foreach (var move in source.Moves)
            {
                learnset.Add(new LevelMove(StoryBalance.MinLevel, move));
            }
            return learnset;
        }

        private static MonsterDefinition Copy(MonsterDefinition source, MonsterRank rank)
        {
            return new MonsterDefinition(source.No, source.Name, (MonsterType[])source.Types, source.BaseStats,
                (MoveDefinition[])source.Moves, source.Ability, rank: rank);
        }
    }
}
