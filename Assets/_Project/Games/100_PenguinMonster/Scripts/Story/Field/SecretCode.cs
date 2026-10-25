using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 十字キーの隠しコマンド。向きを1つずつ受け取り、最後に受け取った並びがコマンドと同じになったら通る。
    /// 途中で押し間違えても、そこから打ち直せば通る（最初からやり直しを求めない）
    /// </summary>
    public sealed class SecretCode
    {
        /// <summary>大橋の展望マスで虹の羽の子を呼ぶ並び（上・上・下・下・左・右・左・右）</summary>
        public static readonly Direction[] Rainbow =
        {
            Direction.Up, Direction.Up, Direction.Down, Direction.Down,
            Direction.Left, Direction.Right, Direction.Left, Direction.Right,
        };

        private readonly Direction[] _code;
        private readonly List<Direction> _recent = new List<Direction>();

        public SecretCode(Direction[] code)
        {
            _code = code;
        }

        /// <summary>向きを1つ足す。最後の並びがコマンドと同じになったら true（通ったら入力を忘れる）</summary>
        public bool Push(Direction direction)
        {
            _recent.Add(direction);
            if (_recent.Count > _code.Length)
            {
                _recent.RemoveAt(0);
            }
            if (!Matches())
            {
                return false;
            }
            _recent.Clear();
            return true;
        }

        private bool Matches()
        {
            if (_recent.Count != _code.Length)
            {
                return false;
            }
            for (int i = 0; i < _code.Length; i++)
            {
                if (_recent[i] != _code[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
