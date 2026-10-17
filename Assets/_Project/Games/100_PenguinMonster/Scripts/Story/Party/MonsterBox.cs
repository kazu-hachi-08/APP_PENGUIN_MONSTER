using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// あずかり箱。手持ちがいっぱいのときに捕まえた子が入り、ぬくもり小屋で手持ちと入れ替える。
    /// 今は数が少ないので、箱の数・入る数の上限は持たない
    /// </summary>
    public sealed class MonsterBox
    {
        private readonly List<OwnedMonster> _members = new List<OwnedMonster>();

        public IReadOnlyList<OwnedMonster> Members => _members;
        public int Count => _members.Count;

        public void Add(OwnedMonster monster)
        {
            _members.Add(monster);
        }

        public OwnedMonster RemoveAt(int index)
        {
            var member = _members[index];
            _members.RemoveAt(index);
            return member;
        }

        /// <summary>その枠の子を入れ替え、元の子を返す</summary>
        public OwnedMonster Replace(int index, OwnedMonster monster)
        {
            var before = _members[index];
            _members[index] = monster;
            return before;
        }
    }
}
