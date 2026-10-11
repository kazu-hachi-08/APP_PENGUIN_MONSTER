using System;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// CPU の行動（仕様書 §4.8 の「よわい」）。ふつう・つよいは Phase 5 で足す
    /// </summary>
    public sealed class CpuPlayer
    {
        private readonly Random _random;

        public CpuPlayer(Random random)
        {
            _random = random;
        }

        /// <summary>技をランダムに選ぶ。交代はしない</summary>
        public BattleAction ChooseAction(BattleSide self)
        {
            return BattleAction.UseMove(_random.Next(self.Active.Definition.Moves.Count));
        }

        /// <summary>ひんしのときは控えの先頭を出す</summary>
        public int ChooseReplacement(BattleSide self)
        {
            for (int i = 0; i < self.Party.Count; i++)
            {
                if (self.CanSwitchTo(i))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
