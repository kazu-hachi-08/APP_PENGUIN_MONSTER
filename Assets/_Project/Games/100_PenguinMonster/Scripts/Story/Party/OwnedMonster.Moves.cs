using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>技を覚える・忘れる</summary>
    public sealed partial class OwnedMonster
    {
        /// <summary>技を1つ忘れて、新しい技を覚える。「忘れる候補」になった技を、画面で選んだ枠に入れる</summary>
        public void ReplaceMove(int slot, MoveDefinition move)
        {
            _moves[slot] = move;
        }

        /// <summary>
        /// そのレベルで覚える技を、空きがあれば覚え、4つ埋まっていれば「忘れる候補」に回す。もう覚えている技は飛ばす
        /// </summary>
        private MoveLearning LearnMovesAt(int level)
        {
            var learned = new List<MoveDefinition>();
            var toChoose = new List<MoveDefinition>();
            foreach (var move in Species.MovesLearnedAt(level))
            {
                if (_moves.Contains(move))
                {
                    continue;
                }
                if (_moves.Count < StoryBalance.MaxMoves)
                {
                    _moves.Add(move);
                    learned.Add(move);
                }
                else
                {
                    toChoose.Add(move);
                }
            }
            return new MoveLearning(learned, toChoose);
        }

        /// <summary>そのレベルまでに覚える技の、新しい方から4つ（同じ技は1つに数える）。並びは覚えた順</summary>
        private static List<MoveDefinition> LatestMoves(StorySpecies species, int level)
        {
            var moves = new List<MoveDefinition>();
            foreach (var entry in species.Learnset)
            {
                if (entry.Level > level || moves.Contains(entry.Move))
                {
                    continue;
                }
                if (moves.Count == StoryBalance.MaxMoves)
                {
                    moves.RemoveAt(0);
                }
                moves.Add(entry.Move);
            }
            return moves;
        }
    }
}
