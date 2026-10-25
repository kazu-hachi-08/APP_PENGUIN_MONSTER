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
        /// <summary>海の上の橋の板の道（8番道路 風花大橋）</summary>
        Bridge,
        /// <summary>橋のふちの手すり。通れない</summary>
        Railing,
        /// <summary>東向きの下り坂。乗ると十字キー無しで東へ進み続ける（向きを東だけにしたのは、大橋の1本道で足りるため）</summary>
        Downhill,
        /// <summary>橋の真ん中の展望マス。乗ると止まって景色を眺める</summary>
        Lookout,
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
                case TileKind.Railing:
                    return false;
                default:
                    return true;
            }
        }
    }
}
