using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>バッグ（道具の種類と数）。並びは StoryItem の順で、1個も無い道具は出さない</summary>
    public sealed class Bag
    {
        private readonly int[] _counts = new int[StoryItems.Count];

        public int CountOf(StoryItem item) => _counts[(int)item];

        public bool Has(StoryItem item) => CountOf(item) > 0;

        public void Add(StoryItem item, int count = 1)
        {
            _counts[(int)item] += count;
        }

        /// <summary>1個減らす。持っていなければ減らさずに false</summary>
        public bool Remove(StoryItem item)
        {
            if (!Has(item))
            {
                return false;
            }
            _counts[(int)item]--;
            return true;
        }

        /// <summary>持っている道具（StoryItem の順）</summary>
        public List<StoryItem> Items()
        {
            var items = new List<StoryItem>();
            for (int i = 0; i < _counts.Length; i++)
            {
                if (_counts[i] > 0)
                {
                    items.Add((StoryItem)i);
                }
            }
            return items;
        }
    }
}
