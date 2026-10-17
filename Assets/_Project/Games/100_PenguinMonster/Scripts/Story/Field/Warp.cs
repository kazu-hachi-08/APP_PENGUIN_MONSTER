namespace PenguinMonster.Story
{
    /// <summary>どのマップの、どのマスに、どちらを向いて立つか（始まりの場所・出入口の行き先）</summary>
    public readonly struct FieldSpot
    {
        public readonly string MapId;
        public readonly GridPos Position;
        public readonly Direction Facing;

        public FieldSpot(string mapId, GridPos position, Direction facing)
        {
            MapId = mapId;
            Position = position;
            Facing = facing;
        }
    }

    /// <summary>
    /// 出入口。From のマスに乗ったら To へ移る。
    /// 行き先は出入口の手前のマスにする（出入口の上に出すと、向こうの出入口に乗ったままになり紛らわしいため）
    /// </summary>
    public sealed class Warp
    {
        public GridPos From { get; }
        public FieldSpot To { get; }

        public Warp(GridPos from, FieldSpot to)
        {
            From = from;
            To = to;
        }
    }
}
