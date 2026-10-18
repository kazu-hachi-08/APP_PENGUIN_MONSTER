namespace PenguinMonster.Battle
{
    public enum EffectTarget
    {
        Foe,
        Self,
    }

    /// <summary>
    /// 技の効果1つ（状態異常にする / こんらんさせる / ランクを変える / 羽リレー / 回復 / 天候 / 守る / 追い風 / 手助け）。
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
        /// <summary>羽リレー: ランクを引き継いで控えの先頭と交代する</summary>
        public bool Relays { get; }
        public StatKind Stat { get; }
        /// <summary>ランクの増減。0 ならランクは変えない</summary>
        public int Stages { get; }
        /// <summary>自分の HP を最大HPの半分回復する</summary>
        public bool Heals { get; }
        /// <summary>場をこの天候にする。None なら天候は変えない</summary>
        public Weather Weather { get; }
        /// <summary>そのターン、相手の技を防ぐ</summary>
        public bool Protects { get; }
        /// <summary>自分の陣営に追い風を吹かせる（素早さ2倍）</summary>
        public bool Tailwind { get; }
        /// <summary>ダブルスで、隣の味方のそのターンの技を強くする</summary>
        public bool HelpsAlly { get; }

        private MoveEffect(EffectTarget target, int chance, StatusCondition status, bool confuses, StatKind stat, int stages,
            bool relays = false, bool heals = false, Weather weather = Weather.None, bool protects = false, bool tailwind = false,
            bool helpsAlly = false)
        {
            Protects = protects;
            Tailwind = tailwind;
            HelpsAlly = helpsAlly;
            Heals = heals;
            Weather = weather;
            Target = target;
            Chance = chance;
            Status = status;
            Confuses = confuses;
            Stat = stat;
            Stages = stages;
            Relays = relays;
        }

        public static MoveEffect Ailment(StatusCondition status, int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Foe, chance, status, false, default, 0);

        public static MoveEffect Confusion(int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Foe, chance, StatusCondition.None, true, default, 0);

        public static MoveEffect SelfStat(StatKind stat, int stages, int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Self, chance, StatusCondition.None, false, stat, stages);

        public static MoveEffect FoeStat(StatKind stat, int stages, int chance = AlwaysPercent)
            => new MoveEffect(EffectTarget.Foe, chance, StatusCondition.None, false, stat, stages);

        public static MoveEffect Relay()
            => new MoveEffect(EffectTarget.Self, AlwaysPercent, StatusCondition.None, false, default, 0, relays: true);

        public static MoveEffect Heal()
            => new MoveEffect(EffectTarget.Self, AlwaysPercent, StatusCondition.None, false, default, 0, heals: true);

        /// <summary>場全体にかかる天候（回避の扱いは下の Protect() と同じ）</summary>
        public static MoveEffect SetWeather(Weather weather)
            => new MoveEffect(EffectTarget.Self, AlwaysPercent, StatusCondition.None, false, default, 0, weather: weather);

        // 守る・追い風・手助けも、相手の回避で外れないよう自分にかける技として扱う（ダブルスで狙いも選ばせずに済む）
        public static MoveEffect Protect()
            => new MoveEffect(EffectTarget.Self, AlwaysPercent, StatusCondition.None, false, default, 0, protects: true);

        public static MoveEffect SetTailwind()
            => new MoveEffect(EffectTarget.Self, AlwaysPercent, StatusCondition.None, false, default, 0, tailwind: true);

        public static MoveEffect HelpAlly()
            => new MoveEffect(EffectTarget.Self, AlwaysPercent, StatusCondition.None, false, default, 0, helpsAlly: true);
    }
}
