namespace PenguinMonster.Battle
{
    public enum EffectTarget
    {
        Foe,
        Self,
    }

    /// <summary>
    /// 技の効果1つ（状態異常にする / こんらんさせる / ランクを変える）。
    /// 攻撃技では当たった後に Chance% で起きる追加効果、変化技では技そのものの効果になる
    /// </summary>
    public sealed class MoveEffect
    {
        public const int AlwaysPercent = 100;

        public EffectTarget Target { get; }
        /// <summary>起きる確率（%）。変化技では 100</summary>
        public int Chance { get; }
        public StatusCondition Status { get; }
        public bool Confuses { get; }
        public StatKind Stat { get; }
        /// <summary>ランクの増減。0 ならランクは変えない</summary>
        public int Stages { get; }

        private MoveEffect(EffectTarget target, int chance, StatusCondition status, bool confuses, StatKind stat, int stages)
        {
            Target = target;
            Chance = chance;
            Status = status;
            Confuses = confuses;
            Stat = stat;
            Stages = stages;
        }

        public static MoveEffect Ailment(StatusCondition status, int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Foe, chance, status, false, default, 0);

        public static MoveEffect Confusion(int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Foe, chance, StatusCondition.None, true, default, 0);

        public static MoveEffect SelfStat(StatKind stat, int stages, int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Self, chance, StatusCondition.None, false, stat, stages);

        public static MoveEffect FoeStat(StatKind stat, int stages, int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Foe, chance, StatusCondition.None, false, stat, stages);
    }
}
