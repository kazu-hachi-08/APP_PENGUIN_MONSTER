using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>手持ち（最大6体）。先頭の子からバトルに出る。いっぱいのときの あずかり箱はフェーズ6で足す</summary>
    public sealed class Party
    {
        private readonly List<OwnedMonster> _members = new List<OwnedMonster>();

        public IReadOnlyList<OwnedMonster> Members => _members;
        public int Count => _members.Count;
        public bool IsFull => _members.Count >= StoryBalance.MaxPartySize;

        /// <summary>手持ちに足す。いっぱいなら足さずに false</summary>
        public bool Add(OwnedMonster monster)
        {
            if (IsFull)
            {
                return false;
            }
            _members.Add(monster);
            return true;
        }
    }
}
