namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り1のマップ（PLAN_STORY_TOWN01.md §2）。今は仮のハツユキ村と、家・研究所・仮のぬくもり小屋・よろず屋の中、試しの1番道路だけ。
    /// 本番の中身はフェーズ8で作り直す。
    /// 文字: . 雪 / , 草むら / = 道 / T 木 / ~ 水 / W 家の壁 / R 屋根 / D 扉 / _ 床 / S 看板
    /// </summary>
    public static class Town01Maps
    {
        public const string Village = "HatsuyukiVillage";
        public const string PlayerHouse = "PlayerHouse";
        public const string RivalHouse = "RivalHouse";
        public const string Lab = "Lab";
        public const string Route01 = "Route01";
        /// <summary>ぬくもり小屋・よろず屋。本番はツララ町に置くが、8b で町ができるまで村に仮で置く（PLAN_STORY_TOWN01.md フェーズ6）</summary>
        public const string WarmHut = "WarmHut";
        public const string Shop = "Shop";

        // 村の扉と、外へ出たときに立つ扉の前のマス
        private static readonly GridPos PlayerHouseDoor = new GridPos(4, 4);
        private static readonly GridPos RivalHouseDoor = new GridPos(13, 4);
        private static readonly GridPos LabDoor = new GridPos(6, 10);
        private static readonly GridPos WarmHutDoor = new GridPos(11, 13);
        private static readonly GridPos ShopDoor = new GridPos(16, 13);
        private static readonly GridPos HutShopSign = new GridPos(14, 13);

        /// <summary>村の北の出口（1番道路へ）。最初の1体をもらうまで母が手前でふさぐ</summary>
        public static readonly GridPos NorthExit = new GridPos(9, 0);
        private static readonly GridPos ExitSign = new GridPos(10, 1);
        private static readonly GridPos VillageSign = new GridPos(8, 7);

        /// <summary>「ストーリー」を始めたときに立つ場所。自分の家の中で目覚める</summary>
        public static readonly FieldSpot Start = new FieldSpot(PlayerHouse, new GridPos(2, 2), Direction.Down);

        public static FieldWorld CreateWorld()
        {
            return new FieldWorld(new[]
            {
                CreateVillage(), CreateHouse(PlayerHouse, PlayerHouseDoor), CreateHouse(RivalHouse, RivalHouseDoor), CreateLab(), CreateRoute01(),
                CreateHouse(WarmHut, WarmHutDoor), CreateHouse(Shop, ShopDoor),
            });
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
                "T.....====RRRR.RRRRT",
                "T.........RRRR.RRRRT",
                "T..~~~~...WDWWSWDWWT",
                "T..~~~~....=....=..T",
                "TTTTTTTTTTTTTTTTTTTT",
            };
            return new FieldMap(Village, rows,
                Inside(PlayerHouseDoor, PlayerHouse, HouseArrival),
                Inside(RivalHouseDoor, RivalHouse, HouseArrival),
                Inside(LabDoor, Lab, LabArrival),
                Inside(WarmHutDoor, WarmHut, HouseArrival),
                Inside(ShopDoor, Shop, HouseArrival),
                new Warp(NorthExit, new FieldSpot(Route01, Route01Exit + Direction.Up.Offset(), Direction.Up)))
                .WithSign(ExitSign, "この先 1番道路", "草むらでは 野生の ペンモンが 飛び出して くるぞ")
                .WithSign(VillageSign, "ここは ハツユキ村", "雪の 降る 静かな 村")
                .WithSign(HutShopSign, "左: ぬくもり小屋（ペンモンの 回復・あずかり箱）", "右: よろず屋（道具の 売り買い）");
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

        // 試しの1番道路の南の出口（村へ）と、北の行き止まりの看板
        private static readonly GridPos Route01Exit = new GridPos(6, 17);
        private static readonly GridPos Route01Sign = new GridPos(5, 1);
        /// <summary>草むらを1歩進むごとに野生に出会う確率（％）。本家の草むら（おおよそ 1割）に合わせる</summary>
        private const int Route01EncounterPercent = 10;

        /// <summary>
        /// 試しの1番道路（フェーズ5で野生・トレーナーと戦うため）。本番の 16x30 の道路はフェーズ8b で作り直す
        /// </summary>
        private static FieldMap CreateRoute01()
        {
            var rows = new[]
            {
                "TTTTTTTTTTTT",
                "T....S=....T",
                "T,,,,.=,,,,T",
                "T,,,,.=,,,,T",
                "T,,,,.=,,,,T",
                "T.....=....T",
                "T.....=....T",
                "TTT...=..TTT",
                "T.....=....T",
                "T..,,,=,,,.T",
                "T..,,,=,,,.T",
                "T..,,,=,,,.T",
                "T.....=....T",
                "T.....=....T",
                "T~~...=....T",
                "T~~...=....T",
                "T.....=....T",
                "TTTTTT=TTTTT",
            };
            var toVillage = new Warp(Route01Exit, new FieldSpot(Village, NorthExit + Direction.Down.Offset(), Direction.Down));
            return new FieldMap(Route01, rows, toVillage)
                .WithSign(Route01Sign, "この先 ツララ町", "（まだ 準備中です）")
                .WithEncounters(new EncounterTable(Route01EncounterPercent,
                    new EncounterSlot(Town01Species.Kogara, 2, 4, 50),
                    new EncounterSlot(Town01Species.Omochi, 2, 4, 50)));
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
