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
                    return false;
                default:
                    return true;
            }
        }
    }
}
