using System;

namespace PenguinMonster.Story
{
    /// <summary>経験値の伸び方。本家の6種類のうち、よく使う2種類だけにする</summary>
    public enum GrowthRate
    {
        /// <summary>Lv100 で 1,000,000（Lv³）</summary>
        MediumFast,
        /// <summary>Lv100 で 1,059,860。序盤は速く、中盤から遅い（最初の3体など）</summary>
        MediumSlow,
    }

    /// <summary>
    /// 経験値の表と、倒したときにもらえる経験値（本家の式）
    /// </summary>
    public static class Experience
    {
        /// <summary>そのレベルになるのに要る経験値の合計</summary>
        public static int ForLevel(GrowthRate rate, int level)
        {
            if (level <= StoryBalance.MinLevel)
            {
                return 0;
            }
            int cube = level * level * level;
            switch (rate)
            {
                case GrowthRate.MediumSlow:
                    // 6/5n³ − 15n² + 100n − 140。Lv1 だけ負になるので上で 0 にしている
                    return 6 * cube / 5 - 15 * level * level + 100 * level - 140;
                default:
                    return cube;
            }
        }

        /// <summary>相手を倒したときの経験値 ＝ 基礎値 × 相手の Lv ÷ 5。トレーナーの子なら 1.5 倍</summary>
        public static int Defeated(int baseExp, int defeatedLevel, bool isTrainer)
        {
            int exp = baseExp * defeatedLevel / StoryBalance.ExpYieldLevelDivisor;
            if (isTrainer)
            {
                exp = exp * StoryBalance.TrainerExpNumerator / StoryBalance.TrainerExpDenominator;
            }
            // 弱い相手でも 0 にはしない（倒したのに何も起きないのを避ける）
            return Math.Max(1, exp);
        }
    }
}
