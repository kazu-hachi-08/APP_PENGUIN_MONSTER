namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り1のマップ（PLAN_STORY_TOWN01.md §2）。今は仮のハツユキ村と、家・研究所の中だけ。
    /// 本番の中身はフェーズ8で作り直す。
    /// 文字: . 雪 / , 草むら / = 道 / T 木 / ~ 水 / W 家の壁 / R 屋根 / D 扉 / _ 床 / S 看板
    /// </summary>
    public static class Town01Maps
    {
        public const string Village = "HatsuyukiVillage";
        public const string PlayerHouse = "PlayerHouse";
        public const string RivalHouse = "RivalHouse";
        public const string Lab = "Lab";

        // 村の扉と、外へ出たときに立つ扉の前のマス
        private static readonly GridPos PlayerHouseDoor = new GridPos(4, 4);
        private static readonly GridPos RivalHouseDoor = new GridPos(13, 4);
        private static readonly GridPos LabDoor = new GridPos(6, 10);

        /// <summary>村の北の出口（1番道路はまだ無いので行き止まり）。最初の1体をもらうまで母が手前でふさぐ</summary>
        public static readonly GridPos NorthExit = new GridPos(9, 0);
        private static readonly GridPos ExitSign = new GridPos(10, 1);
        private static readonly GridPos VillageSign = new GridPos(8, 7);

        /// <summary>「ストーリー」を始めたときに立つ場所。自分の家の中で目覚める</summary>
        public static readonly FieldSpot Start = new FieldSpot(PlayerHouse, new GridPos(2, 2), Direction.Down);

        public static FieldWorld CreateWorld()
        {
            return new FieldWorld(new[] { CreateVillage(), CreateHouse(PlayerHouse, PlayerHouseDoor), CreateHouse(RivalHouse, RivalHouseDoor), CreateLab() });
        }

        private static FieldMap CreateVillage()
        {
            var rows = new[]
            {
                "TTTTTTTTT=TTTTTTTTTT",
                "T........=S........T",
                "T..RRRR..=.RRRR....T",
                "T..RRRR..=.RRRR....T",
                "T..WDWW..=.WWDW....T",
                "T...=....=...=.....T",
                "T...==========.....T",
                "T.......S=.....,,,.T",
                "T..RRRRRR=.....,,,.T",
                "T..RRRRRR=.....,,,.T",
                "T..WWWDWW=.........T",
                "T.....====.........T",
                "T..................T",
                "T..~~~~............T",
                "T..~~~~............T",
                "TTTTTTTTTTTTTTTTTTTT",
            };
            return new FieldMap(Village, rows,
                Inside(PlayerHouseDoor, PlayerHouse, HouseArrival),
                Inside(RivalHouseDoor, RivalHouse, HouseArrival),
                Inside(LabDoor, Lab, LabArrival))
                .WithSign(ExitSign, "この先 1番道路", "（まだ 準備中です）")
                .WithSign(VillageSign, "ここは ハツユキ村", "雪の 降る 静かな 村");
        }

        // 中の扉と、入ったときに立つ扉の前のマス
        private static readonly GridPos HouseExit = new GridPos(3, 5);
        private static readonly GridPos HouseArrival = new GridPos(3, 4);
        private static readonly GridPos LabExit = new GridPos(4, 7);
        private static readonly GridPos LabArrival = new GridPos(4, 6);

        private static FieldMap CreateHouse(string id, GridPos villageDoor)
        {
            var rows = new[]
            {
                "WWWWWWWW",
                "W______W",
                "W______W",
                "W______W",
                "W______W",
                "WWWDWWWW",
            };
            return new FieldMap(id, rows, Outside(HouseExit, villageDoor));
        }

        private static FieldMap CreateLab()
        {
            var rows = new[]
            {
                "WWWWWWWWWW",
                "W________W",
                "W________W",
                "W________W",
                "W________W",
                "W________W",
                "W________W",
                "WWWWDWWWWW",
            };
            return new FieldMap(Lab, rows, Outside(LabExit, LabDoor));
        }

        /// <summary>村の扉から建物の中へ。中では扉の前で奥（上）を向いて立つ</summary>
        private static Warp Inside(GridPos villageDoor, string mapId, GridPos arrival)
        {
            return new Warp(villageDoor, new FieldSpot(mapId, arrival, Direction.Up));
        }

        /// <summary>建物の扉から村へ。村では扉の1つ下で手前（下）を向いて立つ</summary>
        private static Warp Outside(GridPos exit, GridPos villageDoor)
        {
            var front = villageDoor + Direction.Down.Offset();
            return new Warp(exit, new FieldSpot(Village, front, Direction.Down));
        }
    }
}
