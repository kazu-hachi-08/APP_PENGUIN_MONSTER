using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 物語の進み具合。立てたフラグの名前だけを持つ。
    /// 名前は文字なので、章が増えても番号の割り振りを気にせず足せて、セーブにもそのまま書き出せる
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

        /// <summary>
        /// フラグを下ろす。物語の進み具合は戻さないので、切り替えの設定（走る ON など）にだけ使う。
        /// 記録には「立っているフラグ」だけが入るので、下ろしたフラグは読み直しても戻らない
        /// </summary>
        public void Clear(string flag)
        {
            _set.Remove(flag);
        }
    }
}
