namespace PenguinMonster.Battle
{
    /// <summary>
    /// ランクが変わる能力。HP は変わらないので入れない
    /// </summary>
    public enum StatKind
    {
        Attack,
        Defense,
        SpAttack,
        SpDefense,
        Speed,
        Accuracy,
        Evasion,
    }

    public static class StatKindNames
    {
        public const int Count = 7;

        private static readonly string[] Names =
        {
            "攻撃", "防御", "特攻", "特防", "素早さ", "命中率", "回避率",
        };

        public static string ToJapanese(this StatKind stat) => Names[(int)stat];
    }

    /// <summary>
    /// ランクの倍率（原作どおり）。能力は 2/8〜8/2、命中・回避は 3/9〜9/3
    /// </summary>
    public static class StageMultiplier
    {
        public static int ApplyStat(int value, int stage)
        {
            return Apply(value, stage, BattleBalance.StatStageBase);
        }

        public static int ApplyAccuracy(int accuracy, int stage)
        {
            return Apply(accuracy, stage, BattleBalance.AccuracyStageBase);
        }

        private static int Apply(int value, int stage, int stageBase)
        {
            return stage >= 0
                ? value * (stageBase + stage) / stageBase
                : value * stageBase / (stageBase - stage);
        }
    }
}
