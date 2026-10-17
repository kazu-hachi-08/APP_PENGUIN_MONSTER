using System;
using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>出現表の1行「この種が Lv min〜max で、重み weight の割合で出る」</summary>
    public readonly struct EncounterSlot
    {
        public readonly StorySpecies Species;
        public readonly int MinLevel;
        public readonly int MaxLevel;
        public readonly int Weight;

        public EncounterSlot(StorySpecies species, int minLevel, int maxLevel, int weight)
        {
            Species = species;
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            Weight = weight;
        }
    }

    /// <summary>
    /// マップの草むらの出現表。草むらを1歩進むごとに RatePercent の確率で出会い、
    /// 出会ったら重みで種を、その行の幅でレベルを引く
    /// </summary>
    public sealed class EncounterTable
    {
        private readonly EncounterSlot[] _slots;
        private readonly int _totalWeight;

        /// <summary>1歩ごとに出会う確率（％）</summary>
        public int RatePercent { get; }
        public IReadOnlyList<EncounterSlot> Slots => _slots;

        public EncounterTable(int ratePercent, params EncounterSlot[] slots)
        {
            RatePercent = ratePercent;
            _slots = slots;
            foreach (var slot in slots)
            {
                _totalWeight += slot.Weight;
            }
        }

        /// <summary>1歩ぶんの判定。出会わなければ null、出会えば野生の子（技は覚える技の表から決める）</summary>
        public OwnedMonster Roll(Random random)
        {
            if (random.Next(StoryBalance.Percent) >= RatePercent)
            {
                return null;
            }
            var slot = PickSlot(random.Next(_totalWeight));
            return OwnedMonster.Create(slot.Species, random.Next(slot.MinLevel, slot.MaxLevel + 1));
        }

        private EncounterSlot PickSlot(int roll)
        {
            foreach (var slot in _slots)
            {
                if (roll < slot.Weight)
                {
                    return slot;
                }
                roll -= slot.Weight;
            }
            return _slots[_slots.Length - 1];
        }
    }
}
