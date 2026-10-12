using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ドラフト（仕様書 §2.1）。3体の提示から1体ずつ取って6体そろえる。提示はランクの重み付きで引く。
    /// 提示はプレイヤーごとに別々なので、1人につき1つ作る（相手と同じペンモンを取ってもよい）
    /// </summary>
    public sealed class Draft
    {
        private readonly IReadOnlyList<MonsterDefinition> _pool;
        private readonly Random _random;
        private readonly List<MonsterDefinition> _picked = new List<MonsterDefinition>();
        private readonly List<MonsterDefinition> _offer = new List<MonsterDefinition>();

        /// <summary>いま提示されている3体。取り終わったら空</summary>
        public IReadOnlyList<MonsterDefinition> Offer => _offer;
        public IReadOnlyList<MonsterDefinition> Picked => _picked;
        public bool IsDone => _picked.Count >= BattleBalance.DraftRounds;

        public Draft(IReadOnlyList<MonsterDefinition> pool, Random random)
        {
            // 最後の回でも、まだ取っていない中から提示数ぶん引ける必要がある
            if (pool.Count < BattleBalance.DraftRounds - 1 + BattleBalance.DraftOfferCount)
            {
                throw new ArgumentException("ドラフトに必要な数のペンモンがいない", nameof(pool));
            }
            _pool = pool;
            _random = random;
            DrawOffer();
        }

        public void Pick(int offerIndex)
        {
            if (IsDone)
            {
                throw new InvalidOperationException("ドラフトはもう終わっている");
            }
            _picked.Add(_offer[offerIndex]);
            DrawOffer();
        }

        /// <summary>
        /// 自分がまだ取っていない中から、ランクの重みを付けて重複なしで提示数ぶん引く（仕様書 §2.1）。
        /// 伝説を上限まで取ったら、もう伝説は候補に入れない。タイプの偏りの補正はしない。
        /// 1枠につき乱数を1回だけ引く（オンラインのゲストが同じ種で同じ提示を引けるよう、引き方を変えないこと）
        /// </summary>
        private void DrawOffer()
        {
            _offer.Clear();
            if (IsDone)
            {
                return;
            }
            var candidates = Candidates();
            for (int i = 0; i < BattleBalance.DraftOfferCount; i++)
            {
                int index = WeightedIndex(candidates);
                _offer.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
        }

        private List<MonsterDefinition> Candidates()
        {
            bool legendAllowed = CountPicked(MonsterRank.Legend) < BattleBalance.MaxLegendsPerTeam;
            var candidates = new List<MonsterDefinition>();
            foreach (var monster in _pool)
            {
                if (!_picked.Contains(monster) && (legendAllowed || monster.Rank != MonsterRank.Legend))
                {
                    candidates.Add(monster);
                }
            }
            return candidates;
        }

        private int CountPicked(MonsterRank rank)
        {
            int count = 0;
            foreach (var monster in _picked)
            {
                if (monster.Rank == rank)
                {
                    count++;
                }
            }
            return count;
        }

        private int WeightedIndex(List<MonsterDefinition> candidates)
        {
            int total = 0;
            foreach (var monster in candidates)
            {
                total += Weight(monster.Rank);
            }
            int roll = _random.Next(total);
            for (int i = 0; i < candidates.Count; i++)
            {
                roll -= Weight(candidates[i].Rank);
                if (roll < 0)
                {
                    return i;
                }
            }
            return candidates.Count - 1;
        }

        public static int Weight(MonsterRank rank)
        {
            switch (rank)
            {
                case MonsterRank.Hina: return BattleBalance.DraftWeightHina;
                case MonsterRank.Ace: return BattleBalance.DraftWeightAce;
                case MonsterRank.Legend: return BattleBalance.DraftWeightLegend;
                default: return BattleBalance.DraftWeightNormal;
            }
        }
    }
}
