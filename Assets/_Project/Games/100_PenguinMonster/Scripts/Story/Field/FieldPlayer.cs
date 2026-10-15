namespace PenguinMonster.Story
{
    /// <summary>1歩の結果。向きはどちらでも変わる</summary>
    public enum StepResult
    {
        /// <summary>壁などで進めず、向きだけ変えた</summary>
        Blocked,
        Moved,
    }

    /// <summary>
    /// フィールドの主人公の位置と向き。1マスずつ歩き、出入口に乗ったらマップを移る。
    /// 歩く動き・暗転は View が受け持ち、ここはマスの上の位置だけを決める
    /// </summary>
    public sealed class FieldPlayer
    {
        private readonly FieldWorld _world;

        public FieldMap Map { get; private set; }
        public GridPos Position { get; private set; }
        public Direction Facing { get; private set; }

        /// <summary>今いるマスの出入口。View は歩き終えたらこれを見て、暗転してから Enter を呼ぶ</summary>
        public Warp WarpHere => Map.WarpAt(Position);

        public FieldPlayer(FieldWorld world, FieldSpot start)
        {
            _world = world;
            MoveTo(start);
        }

        public StepResult Step(Direction direction)
        {
            Facing = direction;
            var next = Position + direction.Offset();
            if (!Map.IsWalkable(next))
            {
                return StepResult.Blocked;
            }
            Position = next;
            return StepResult.Moved;
        }

        public void Enter(Warp warp)
        {
            MoveTo(warp.To);
        }

        private void MoveTo(FieldSpot spot)
        {
            Map = _world.Get(spot.MapId);
            Position = spot.Position;
            Facing = spot.Facing;
        }
    }
}
