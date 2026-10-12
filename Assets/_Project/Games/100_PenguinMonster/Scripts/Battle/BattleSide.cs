using System.Collections.Generic;
using System.Linq;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 1陣営（選出したペンモンと、場に出ているもの）。
    /// ダブルスで作り直さないよう、場は配列（シングルは要素1つ）で持つ
    /// </summary>
    public sealed class BattleSide
    {
        private readonly int[] _activePartyIndices;

        public int Index { get; }
        public IReadOnlyList<BattleMonster> Party { get; }
        public int ActiveCount => _activePartyIndices.Length;

        /// <summary>シングル用。場の先頭のペンモン</summary>
        public BattleMonster Active => Party[_activePartyIndices[0]];
        public int ActivePartyIndex => _activePartyIndices[0];

        public bool HasRemaining => Party.Any(m => !m.IsFainted);

        /// <summary>場のペンモンがひんしで、控えに出せるペンモンがいる</summary>
        public bool NeedsReplacement => Active.IsFainted && Enumerable.Range(0, Party.Count).Any(CanSwitchTo);

        public BattleSide(int index, IReadOnlyList<MonsterDefinition> party, int activeCount)
        {
            Index = index;
            Party = party.Select(d => new BattleMonster(d) { Side = this }).ToList();
            // 選出の先頭から順に場へ出す
            _activePartyIndices = Enumerable.Range(0, activeCount).ToArray();
        }

        public bool IsActive(int partyIndex) => _activePartyIndices.Contains(partyIndex);

        /// <summary>選出の順で一番前の、出せる控え。いなければ -1（羽リレーの交代先）</summary>
        public int FirstBenchIndex()
        {
            for (int i = 0; i < Party.Count; i++)
            {
                if (CanSwitchTo(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool CanSwitchTo(int partyIndex)
        {
            return partyIndex >= 0 && partyIndex < Party.Count && !Party[partyIndex].IsFainted && !IsActive(partyIndex);
        }

        internal void SetActive(int partyIndex)
        {
            _activePartyIndices[0] = partyIndex;
        }
    }
}
