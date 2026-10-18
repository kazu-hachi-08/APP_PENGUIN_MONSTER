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

        /// <summary>
        /// 種の定義の表（params のタプル）を、表の順のまま LevelMove の配列に詰め直す。
        /// Learnset は表の順（レベルの低い順）で読むので、並べ替えはしない
        /// </summary>
        public static LevelMove[] FromTable(params (int Level, MoveDefinition Move)[] learnset)
        {
            var moves = new LevelMove[learnset.Length];
            for (int i = 0; i < learnset.Length; i++)
            {
                moves[i] = new LevelMove(learnset[i].Level, learnset[i].Move);
            }
            return moves;
        }
    }

    /// <summary>進化の表の1行「Lv N で No.M へ」。当面はレベル進化だけ</summary>
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
