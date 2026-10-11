namespace PenguinMonster.Battle
{
    public enum BattleActionKind
    {
        Move,
        Switch,
    }

    /// <summary>
    /// 1ターンに選ぶ行動（技 / 交代）
    /// </summary>
    public sealed class BattleAction
    {
        public BattleActionKind Kind { get; }
        /// <summary>技のとき: 4つのうち何番目か / 交代のとき: 選出の何番目を出すか</summary>
        public int Index { get; }

        private BattleAction(BattleActionKind kind, int index)
        {
            Kind = kind;
            Index = index;
        }

        public static BattleAction UseMove(int moveIndex) => new BattleAction(BattleActionKind.Move, moveIndex);
        public static BattleAction SwitchTo(int partyIndex) => new BattleAction(BattleActionKind.Switch, partyIndex);
    }
}
