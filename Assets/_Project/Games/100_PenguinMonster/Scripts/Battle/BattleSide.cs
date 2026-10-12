using System.Collections.Generic;
using System.Linq;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 1陣営（選出したペンモンと、場に出ているもの）。場は配列で持ち、シングルは要素1つ、ダブルスは2つ。
    /// 控えがいないまま倒れた場は、倒れたペンモンが残ったまま（空き）になる
    /// </summary>
    public sealed class BattleSide
    {
        private readonly int[] _activePartyIndices;

        public int Index { get; }
        public IReadOnlyList<BattleMonster> Party { get; }
        public int ActiveCount => _activePartyIndices.Length;

        /// <summary>シングル用。場の先頭のペンモン</summary>
        public BattleMonster Active => ActiveAt(0);
        public int ActivePartyIndex => _activePartyIndices[0];

        public bool HasRemaining => Party.Any(m => !m.IsFainted);

        /// <summary>場のペンモンが瀕死で、控えに出せるペンモンがいる</summary>
        public bool NeedsReplacement => ReplaceableSlots().Count > 0;

        public BattleSide(int index, IReadOnlyList<MonsterDefinition> party, int activeCount)
        {
            Index = index;
            Party = party.Select(d => new BattleMonster(d) { Side = this }).ToList();
            // 選出の先頭から順に場へ出す
            _activePartyIndices = Enumerable.Range(0, activeCount).ToArray();
        }

        public BattleMonster ActiveAt(int slot) => Party[_activePartyIndices[slot]];

        /// <summary>場の何番目にいるか。場にいなければ -1</summary>
        public int SlotOf(BattleMonster monster)
        {
            for (int slot = 0; slot < ActiveCount; slot++)
            {
                if (ActiveAt(slot) == monster)
                {
                    return slot;
                }
            }
            return -1;
        }

        /// <summary>場で戦える（瀕死でない）ペンモンの場の番号。前から順</summary>
        public List<int> AliveSlots()
        {
            return Enumerable.Range(0, ActiveCount).Where(slot => !ActiveAt(slot).IsFainted).ToList();
        }

        /// <summary>入れ替えが要る場。瀕死の場を前から、控えの数だけ（足りなければ後ろの場は空きのまま）</summary>
        public List<int> ReplaceableSlots()
        {
            int bench = Enumerable.Range(0, Party.Count).Count(CanSwitchTo);
            return Enumerable.Range(0, ActiveCount).Where(slot => ActiveAt(slot).IsFainted).Take(bench).ToList();
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

        internal void SetActive(int slot, int partyIndex)
        {
            _activePartyIndices[slot] = partyIndex;
        }
    }
}
