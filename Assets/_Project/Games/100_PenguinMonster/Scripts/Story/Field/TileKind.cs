namespace PenguinMonster.Story
{
    /// <summary>マップのマスの種類。歩けるかどうかと、表示のタイルを決める</summary>
    public enum TileKind
    {
        Snow,
        /// <summary>草むら（野生が出る）</summary>
        Grass,
        /// <summary>深い草むら。マップの2つ目の出現表から出る（川べり・林の奥など、場所で出る子を変える）</summary>
        DeepGrass,
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
