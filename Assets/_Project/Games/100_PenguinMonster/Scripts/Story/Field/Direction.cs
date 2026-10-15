namespace PenguinMonster.Story
{
    /// <summary>フィールドで向く・歩く向き。主人公の絵の並び（下・上・左・右）と同じ順にする</summary>
    public enum Direction
    {
        Down,
        Up,
        Left,
        Right,
    }

    public static class DirectionExtensions
    {
        /// <summary>1マス進んだときの差分。マップは文字の表と同じく、上の行ほど y が小さい</summary>
        public static GridPos Offset(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return new GridPos(0, -1);
                case Direction.Left: return new GridPos(-1, 0);
                case Direction.Right: return new GridPos(1, 0);
                default: return new GridPos(0, 1);
            }
        }
    }
}
