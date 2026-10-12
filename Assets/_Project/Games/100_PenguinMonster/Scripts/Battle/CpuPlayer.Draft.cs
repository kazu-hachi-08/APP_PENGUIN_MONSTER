using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    public sealed partial class CpuPlayer
    {
        /// <summary>
        /// ドラフトを最後まで進める。strongPickPercent% の確率で提示のうち種族値の合計が一番高いのを取り、
        /// それ以外はランダム。勝ち抜き戦で連勝するほど相手のチームが強くなるようにするため
        /// </summary>
        public IReadOnlyList<MonsterDefinition> RunDraft(Draft draft, int strongPickPercent = 0)
        {
            while (!draft.IsDone)
            {
                draft.Pick(ChooseDraftPick(draft.Offer, strongPickPercent));
            }
            return draft.Picked;
        }

        internal int ChooseDraftPick(IReadOnlyList<MonsterDefinition> offer, int strongPickPercent)
        {
            bool pickStrong = _random.Next(BattleBalance.PercentMax) < strongPickPercent;
            return pickStrong ? StrongestIndex(offer) : _random.Next(offer.Count);
        }

        /// <summary>
        /// 候補からバトルに出す数をランダムに選ぶ。並びがそのまま出す順（先頭が1番目）。
        /// trumpCard を渡すと必ず入れて最後に出す（ボスの切り札。仕様書 §2.3）。partySize はシングル 3・ダブルス 4
        /// </summary>
        public List<MonsterDefinition> ChooseTeam(IReadOnlyList<MonsterDefinition> candidates, MonsterDefinition trumpCard = null,
            int partySize = BattleBalance.PartySize)
        {
            var rest = new List<MonsterDefinition>(candidates);
            var team = new List<MonsterDefinition>();
            int randomCount = partySize;
            if (trumpCard != null)
            {
                rest.Remove(trumpCard);
                randomCount--;
            }
            while (team.Count < randomCount)
            {
                int index = _random.Next(rest.Count);
                team.Add(rest[index]);
                rest.RemoveAt(index);
            }
            if (trumpCard != null)
            {
                team.Add(trumpCard);
            }
            return team;
        }

        private static int StrongestIndex(IReadOnlyList<MonsterDefinition> offer)
        {
            int best = 0;
            for (int i = 1; i < offer.Count; i++)
            {
                if (offer[i].BaseStats.Total > offer[best].BaseStats.Total)
                {
                    best = i;
                }
            }
            return best;
        }
    }
}
