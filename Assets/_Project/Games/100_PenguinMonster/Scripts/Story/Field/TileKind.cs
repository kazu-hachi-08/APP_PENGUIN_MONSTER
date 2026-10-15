namespace PenguinMonster.Story
{
    /// <summary>マップのマスの種類。歩けるかどうかと、表示のタイルを決める</summary>
    public enum TileKind
    {
        Snow,
        /// <summary>草むら（フェーズ5で野生が出る）</summary>
        Grass,
        Path,
        Tree,
        Water,
        HouseWall,
        Roof,
        Door,
        Floor,
        /// <summary>看板。通れず、前から A で読む</summary>
        Sign,
    }

    public static class TileKindExtensions
    {
        public static bool IsWalkable(this TileKind kind)
        {
            switch (kind)
            {
                case TileKind.Tree:
                case TileKind.Water:
                case TileKind.HouseWall:
                case TileKind.Roof:
                case TileKind.Sign:
                    return false;
                default:
                    return true;
            }
        }
    }
}
