using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// CPU の行動（仕様書 §4.8 の「よわい」）。ふつう・つよいは Phase 5 で足す。
    /// ドラフトと選出もここで決める（いまはどちらもランダム）
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

        /// <summary>ドラフトを最後まで進める。提示からランダムに取る</summary>
        public IReadOnlyList<MonsterDefinition> RunDraft(Draft draft)
        {
            while (!draft.IsDone)
            {
                draft.Pick(_random.Next(draft.Offer.Count));
            }
            return draft.Picked;
        }

        /// <summary>候補からバトルに出す数をランダムに選ぶ。並びがそのまま出す順（先頭が1番目）</summary>
        public List<MonsterDefinition> ChooseTeam(IReadOnlyList<MonsterDefinition> candidates)
        {
            var rest = new List<MonsterDefinition>(candidates);
            var team = new List<MonsterDefinition>();
            while (team.Count < BattleBalance.PartySize)
            {
                int index = _random.Next(rest.Count);
                team.Add(rest[index]);
                rest.RemoveAt(index);
            }
            return team;
        }
    }
}
