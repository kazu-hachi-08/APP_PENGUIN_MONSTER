using System;
using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>経験値・レベルアップ・進化</summary>
    public sealed partial class OwnedMonster
    {
        /// <summary>進化の表のレベルに届いている。画面はバトルの後にこれを見て進化の演出を出す</summary>
        public bool CanEvolve => Species.Evolution != null && Level >= Species.Evolution.Level;

        /// <summary>
        /// 経験値を足し、届いたぶんだけレベルを上げる。上がったレベルごとの結果を順に返す（上がらなければ空）。
        /// Lv100 より先の経験値は捨てる
        /// </summary>
        public IReadOnlyList<LevelUp> GainExp(int amount)
        {
            var levelUps = new List<LevelUp>();
            Exp = Math.Min(Exp + amount, ExpForLevel(StoryBalance.MaxLevel));
            while (Level < StoryBalance.MaxLevel && Exp >= ExpForLevel(Level + 1))
            {
                levelUps.Add(RaiseLevel());
            }
            return levelUps;
        }

        /// <summary>
        /// 進化先の種に変わる。種は呼ぶ側が進化の表の No から引いて渡す。
        /// 進化先がいまのレベルで覚える技を返す
        /// </summary>
        public MoveLearning Evolve(StorySpecies into)
        {
            int maxHpBefore = MaxHp;
            Species = into;
            AddMaxHpGain(maxHpBefore);
            return LearnMovesAt(Level);
        }

        private LevelUp RaiseLevel()
        {
            var before = Stats;
            Level++;
            AddMaxHpGain(before.Hp);
            return new LevelUp(Level, before, Stats, LearnMovesAt(Level));
        }

        /// <summary>
        /// 最大HPが増えたぶんだけ今の HP も増やす（本家と同じ。満タンなら満タンのまま、傷はそのまま残る）。
        /// 瀕死の子は起こさない
        /// </summary>
        private void AddMaxHpGain(int maxHpBefore)
        {
            if (!IsFainted)
            {
                CurrentHp += MaxHp - maxHpBefore;
            }
        }

        private int ExpForLevel(int level) => Experience.ForLevel(Species.GrowthRate, level);
    }
}
