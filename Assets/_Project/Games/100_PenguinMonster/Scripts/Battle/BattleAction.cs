namespace PenguinMonster.Battle
{
    public enum BattleActionKind
    {
        Move,
        Switch,
        /// <summary>何もしない（ストーリーで逃げ損ねたとき。相手だけが動く）</summary>
        Skip,
    }

    /// <summary>
    /// 1ターンに選ぶ行動（技 / 交代 / 何もしない）。ダブルスでは場の1体ごとに1つ選ぶ
    /// </summary>
    public sealed class BattleAction
    {
        public BattleActionKind Kind { get; }
        /// <summary>技のとき: 4つのうち何番目か / 交代のとき: 選出の何番目を出すか</summary>
        public int Index { get; }
        /// <summary>技のとき: 狙う相手の場の何番目か（シングルはいつも 0。倒れていたらもう片方へ）</summary>
        public int Target { get; }

        private BattleAction(BattleActionKind kind, int index, int target)
        {
            Kind = kind;
            Index = index;
            Target = target;
        }

        public static BattleAction UseMove(int moveIndex, int target = 0) => new BattleAction(BattleActionKind.Move, moveIndex, target);
        public static BattleAction SwitchTo(int partyIndex) => new BattleAction(BattleActionKind.Switch, partyIndex, 0);
        public static BattleAction Skip() => new BattleAction(BattleActionKind.Skip, 0, 0);
    }
}
