using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 1レベル上がったときの結果。画面が「Lv○に 上がった！」と上がった能力の窓、覚えた技を1段ずつ出すのに使う
    /// </summary>
    public sealed class LevelUp
    {
        public int Level { get; }
        public Stats Before { get; }
        public Stats After { get; }
        public MoveLearning Moves { get; }

        public LevelUp(int level, Stats before, Stats after, MoveLearning moves)
        {
            Level = level;
            Before = before;
            After = after;
            Moves = moves;
        }
    }

    /// <summary>
    /// 技を覚えた結果。空きに入って覚えた技と、4つ埋まっていて「どれか忘れるか」を聞く技に分ける
    /// </summary>
    public sealed class MoveLearning
    {
        public IReadOnlyList<MoveDefinition> Learned { get; }
        /// <summary>覚えるには1つ忘れる必要がある技。画面で選ばせて ReplaceMove で入れる（やめれば覚えない）</summary>
        public IReadOnlyList<MoveDefinition> ToChoose { get; }

        public MoveLearning(IReadOnlyList<MoveDefinition> learned, IReadOnlyList<MoveDefinition> toChoose)
        {
            Learned = learned;
            ToChoose = toChoose;
        }
    }
}
