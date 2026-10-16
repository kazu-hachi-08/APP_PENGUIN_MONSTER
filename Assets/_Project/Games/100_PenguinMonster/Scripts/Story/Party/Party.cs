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

        /// <summary>戦える子が1体でもいる（いなければ野生に出会わない）</summary>
        public bool CanFight
        {
            get
            {
                foreach (var member in _members)
                {
                    if (!member.IsFainted)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>全員の HP を満タンにし、状態異常を治す（母・ぬくもり小屋・全滅）</summary>
        public void HealAll()
        {
            foreach (var member in _members)
            {
                member.HealFully();
            }
        }

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
