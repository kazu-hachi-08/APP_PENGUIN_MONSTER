namespace PenguinMonster.Battle
{
    public enum BattleEventKind
    {
        SentOut,
        Withdrawn,
        MoveUsed,
        Missed,
        Damaged,
        Critical,
        SuperEffective,
        NotVeryEffective,
        NoEffect,
        Fainted,
        BattleEnd,
        /// <summary>変化技が決まらなかった（すでに状態異常・タイプで効かない など）</summary>
        Failed,
        StatusInflicted,
        /// <summary>まひ・ねむり・こおりで動けなかった（理由は Status）</summary>
        Immobilized,
        WokeUp,
        Thawed,
        Confused,
        /// <summary>こんらん中に技を使おうとした（「こんらんしている！」）</summary>
        ConfusionCheck,
        HurtItself,
        ConfusionEnded,
        /// <summary>ターン終わりの どく・やけど のダメージ（理由は Status）</summary>
        StatusDamage,
        /// <summary>ランクが変わった。Amount が変わった量（＋で上がる）</summary>
        StatChanged,
        /// <summary>ランクがもう変わらない。Amount の符号が上げようとしたか下げようとしたか</summary>
        StatUnchanged,
    }

    /// <summary>
    /// バトルで起きたことを1つずつ表す。表示（メッセージ・HPバー・状態異常の印）は View がこれを順に再生して行う。
    /// HP と状態異常は起きた時点の値を持たせる（BattleMonster の値はターン終わりの値なので、途中の表示に使えない）
    /// </summary>
    public sealed class BattleEvent
    {
        public BattleEventKind Kind { get; }
        /// <summary>主語になる陣営。BattleEnd のときは勝った陣営</summary>
        public int Side { get; }
        public BattleMonster Monster { get; }
        public string MoveName { get; }
        public int Hp { get; }
        public StatusCondition Status { get; }
        public StatKind Stat { get; }
        public int Amount { get; }

        private BattleEvent(BattleEventKind kind, int side, BattleMonster monster, string moveName = null,
            StatKind stat = default, int amount = 0)
        {
            Kind = kind;
            Side = side;
            Monster = monster;
            MoveName = moveName;
            Hp = monster?.CurrentHp ?? 0;
            Status = monster?.Status ?? StatusCondition.None;
            Stat = stat;
            Amount = amount;
        }

        public static BattleEvent Of(BattleEventKind kind, int side, BattleMonster monster)
            => new BattleEvent(kind, side, monster);

        public static BattleEvent MoveUsed(int side, BattleMonster monster, MoveDefinition move)
            => new BattleEvent(BattleEventKind.MoveUsed, side, monster, move.Name);

        public static BattleEvent StatChange(BattleEventKind kind, int side, BattleMonster monster, StatKind stat, int amount)
            => new BattleEvent(kind, side, monster, stat: stat, amount: amount);

        public static BattleEvent BattleEnd(int winner)
            => new BattleEvent(BattleEventKind.BattleEnd, winner, null);
    }
}
