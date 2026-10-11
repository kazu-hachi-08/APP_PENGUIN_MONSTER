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
    }

    /// <summary>
    /// バトルで起きたことを1つずつ表す。表示（メッセージ・HPバー）は View がこれを順に再生して行う。
    /// HP は起きた時点の値を持たせる（BattleMonster.CurrentHp はターン終わりの値なので、途中の表示に使えない）
    /// </summary>
    public sealed class BattleEvent
    {
        public BattleEventKind Kind { get; }
        /// <summary>主語になる陣営。BattleEnd のときは勝った陣営</summary>
        public int Side { get; }
        public BattleMonster Monster { get; }
        public string MoveName { get; }
        public int Hp { get; }

        private BattleEvent(BattleEventKind kind, int side, BattleMonster monster, string moveName = null)
        {
            Kind = kind;
            Side = side;
            Monster = monster;
            MoveName = moveName;
            Hp = monster?.CurrentHp ?? 0;
        }

        public static BattleEvent Of(BattleEventKind kind, int side, BattleMonster monster)
            => new BattleEvent(kind, side, monster);

        public static BattleEvent MoveUsed(int side, BattleMonster monster, MoveDefinition move)
            => new BattleEvent(BattleEventKind.MoveUsed, side, monster, move.Name);

        public static BattleEvent BattleEnd(int winner)
            => new BattleEvent(BattleEventKind.BattleEnd, winner, null);
    }
}
