using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>覚える技の表の1行「Lv N で 技」。Lv1 の行は最初から覚えている技</summary>
    public readonly struct LevelMove
    {
        public readonly int Level;
        public readonly MoveDefinition Move;

        public LevelMove(int level, MoveDefinition move)
        {
            Level = level;
            Move = move;
        }
    }

    /// <summary>進化の表の1行「Lv N で No.M へ」。当面はレベル進化だけ（STORY_ROADMAP.md §3.3）</summary>
    public sealed class Evolution
    {
        public int Level { get; }
        public int IntoNo { get; }

        public Evolution(int level, int intoNo)
        {
            Level = level;
            IntoNo = intoNo;
        }
    }
}
