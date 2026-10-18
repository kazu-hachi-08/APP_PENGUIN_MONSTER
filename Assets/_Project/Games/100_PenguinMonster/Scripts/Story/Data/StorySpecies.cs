using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリーに出る種（No.151〜）の、バトルの定義に足す育成の情報。
    /// 種族値・タイプ・特性は MonsterDefinition に持ち、ここでは包むだけにする
    /// </summary>
    public sealed class StorySpecies
    {
        public MonsterDefinition Definition { get; }
        public GrowthRate GrowthRate { get; }
        /// <summary>倒されたときの経験値の基礎値（本家の「基礎経験値」）</summary>
        public int BaseExp { get; }
        /// <summary>覚える技の表。レベルの低い順に並べる</summary>
        public IReadOnlyList<LevelMove> Learnset { get; }
        /// <summary>進化しない種は null</summary>
        public Evolution Evolution { get; }
        /// <summary>捕まえやすさ（本家と同じ 3〜255。大きいほど捕まえやすい）</summary>
        public int CatchRate { get; }

        public int No => Definition.No;
        public string Name => Definition.Name;

        public StorySpecies(MonsterDefinition definition, GrowthRate growthRate, int baseExp, LevelMove[] learnset,
            Evolution evolution = null, int catchRate = StoryBalance.CaptureMaxOdds)
        {
            Definition = definition;
            GrowthRate = growthRate;
            BaseExp = baseExp;
            Learnset = learnset;
            Evolution = evolution;
            CatchRate = catchRate;
        }

        /// <summary>ちょうどそのレベルで覚える技（表の順）</summary>
        public IEnumerable<MoveDefinition> MovesLearnedAt(int level)
        {
            foreach (var entry in Learnset)
            {
                if (entry.Level == level)
                {
                    yield return entry.Move;
                }
            }
        }
    }
}
