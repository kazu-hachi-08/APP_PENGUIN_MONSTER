using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ドラフト（仕様書 §2.1）。3体の提示から1体ずつ取って6体そろえる。
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
        /// 自分がまだ取っていない中から純ランダムに引く（タイプの偏りの補正はしない）。
        /// 先頭から部分的にシャッフルして、重複なしで提示数ぶん取り出す
        /// </summary>
        private void DrawOffer()
        {
            _offer.Clear();
            if (IsDone)
            {
                return;
            }
            var candidates = new List<MonsterDefinition>();
            foreach (var monster in _pool)
            {
                if (!_picked.Contains(monster))
                {
                    candidates.Add(monster);
                }
            }
            for (int i = 0; i < BattleBalance.DraftOfferCount; i++)
            {
                int j = _random.Next(i, candidates.Count);
                (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
                _offer.Add(candidates[i]);
            }
        }
    }
}
