using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>手持ち（最大6体）。先頭の子からバトルに出る。いっぱいのときは StoryState.Receive が あずかり箱へ送る</summary>
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

        /// <summary>並べ替え（フィールドのメニュー）。先頭の子が次のバトルで最初に出る</summary>
        public void Swap(int a, int b)
        {
            (_members[a], _members[b]) = (_members[b], _members[a]);
        }

        /// <summary>
        /// その子が抜けても、戦える子が残る（あずかり箱へ預けるとき。本家と同じく、戦える子が居なくなる預け方はさせない）
        /// </summary>
        public bool CanFightWithout(int index)
        {
            for (int i = 0; i < _members.Count; i++)
            {
                if (i != index && !_members[i].IsFainted)
                {
                    return true;
                }
            }
            return false;
        }

        public OwnedMonster RemoveAt(int index)
        {
            var member = _members[index];
            _members.RemoveAt(index);
            return member;
        }

        /// <summary>その枠の子を入れ替え、元の子を返す（あずかり箱の子と入れ替えるとき）</summary>
        public OwnedMonster Replace(int index, OwnedMonster monster)
        {
            var before = _members[index];
            _members[index] = monster;
            return before;
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
