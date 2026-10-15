using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 1レベル上がったときの結果。画面（フェーズ5）が「Lv○に 上がった！」と上がった能力の窓を1段ずつ出すのに使う
    /// </summary>
    public sealed class LevelUp
    {
        public int Level { get; }
        public Stats Before { get; }
        public Stats After { get; }

        public LevelUp(int level, Stats before, Stats after)
        {
            Level = level;
            Before = before;
            After = after;
        }
    }
}
