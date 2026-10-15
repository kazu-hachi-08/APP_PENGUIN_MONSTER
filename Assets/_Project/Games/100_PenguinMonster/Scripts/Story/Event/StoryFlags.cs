using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 物語の進み具合（STORY_ROADMAP.md §3.3）。立てたフラグの名前だけを持つ。
    /// 名前は文字なので、区切りが増えても番号の割り振りを気にせず足せて、セーブ（フェーズ7）もそのまま書き出せる
    /// </summary>
    public sealed class StoryFlags
    {
        private readonly HashSet<string> _set = new HashSet<string>();

        public IEnumerable<string> All => _set;

        public bool Has(string flag) => _set.Contains(flag);

        public void Set(string flag)
        {
            _set.Add(flag);
        }
    }
}
