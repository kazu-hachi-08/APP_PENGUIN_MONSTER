using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 最初の章のマップ。ハツユキ村（北）→ 1番道路（北）→ ツララ町、町の東から 2番道路（行き止まり）。
    /// 文字: . 雪 / , 草むら / ; 深い草むら / = 道 / T 木 / ~ 水 / W 家の壁 / R 屋根 / D 扉 / _ 床 / S 看板
    /// </summary>
    public static class Town01Maps
    {
        public const string Village = "HatsuyukiVillage";
        public const string PlayerHouse = "PlayerHouse";
        public const string RivalHouse = "RivalHouse";
        public const string Lab = "Lab";
        public const string Route01 = "Route01";
        public const string Town = "TsuraraTown";
        public const string TownHouse = "TsuraraHouse";
        /// <summary>ぬくもり小屋・よろず屋（ツララ町）。マップ ID は記録に入るので名前を変えない</summary>
        public const string WarmHut = "WarmHut";
        public const string Shop = "Shop";
        public const string Gym = "TsuraraGym";
        public const string Route02 = "Route02";

        /// <summary>草むらを1歩進むごとに野生に出会う確率（％）。本家の草むら（おおよそ 1割）に合わせる</summary>
        internal const int EncounterPercent = 10;

        /// <summary>「ストーリー」を始めたときに立つ場所。自分の家の中で目覚める</summary>
        public static readonly FieldSpot Start = new FieldSpot(PlayerHouse, new GridPos(2, 2), Direction.Down);

        public static IEnumerable<FieldMap> CreateMaps()
        {
            return new[]
            {
                CreateVillage(), CreateHouse(PlayerHouse, Village, PlayerHouseDoor), CreateHouse(RivalHouse, Village, RivalHouseDoor),
                CreateLab(), CreateRoute01(), CreateTown(), CreateHouse(TownHouse, Town, TownHouseDoor),
                CreateHouse(WarmHut, Town, WarmHutDoor), CreateHouse(Shop, Town, ShopDoor), CreateGym(Gym, Town, GymDoor), CreateRoute02(),
            };
        }

        // ---- ハツユキ村（20x16） ----
        private static readonly GridPos PlayerHouseDoor = new GridPos(4, 4);
        private static readonly GridPos RivalHouseDoor = new GridPos(13, 4);
        private static readonly GridPos LabDoor = new GridPos(6, 10);
        /// <summary>村の北の出口（1番道路へ）。最初の1体をもらうまで母が手前でふさぐ</summary>
        public static readonly GridPos NorthExit = new GridPos(9, 0);
        private static readonly GridPos ExitSign = new GridPos(10, 1);
        private static readonly GridPos VillageSign = new GridPos(8, 7);

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
                "T.....====.....T...T",
                "T.........TT.......T",
                "T..~~~~.....T..TT..T",
                "T..~~~~.......T....T",
                "TTTTTTTTTTTTTTTTTTTT",
            };
            return new FieldMap(Village, rows,
                Inside(PlayerHouseDoor, PlayerHouse, HouseArrival),
                Inside(RivalHouseDoor, RivalHouse, HouseArrival),
                Inside(LabDoor, Lab, LabArrival),
                new Warp(NorthExit, StepFrom(Route01, Route01South, Direction.Up)))
                .WithSign(ExitSign, "この先 1番道路", "1番道路を 抜けると ツララ町")
                .WithSign(VillageSign, "ここは ハツユキ村", "雪の 降る 静かな 村");
        }

        // ---- 家・研究所・小屋・店の中 ----
        private static readonly GridPos HouseExit = new GridPos(3, 5);
        /// <summary>家・小屋・店に入ったときに立つ場所（扉の1つ奥）</summary>
        internal static readonly GridPos HouseArrival = new GridPos(3, 4);
        private static readonly GridPos LabExit = new GridPos(4, 7);
        private static readonly GridPos LabArrival = new GridPos(4, 6);

        /// <summary>家・小屋・店の中（8x6）。どの町でも同じ形を使い回す</summary>
        internal static FieldMap CreateHouse(string id, string outsideMap, GridPos outsideDoor)
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
            return new FieldMap(id, rows, Outside(HouseExit, outsideMap, outsideDoor));
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
            return new FieldMap(Lab, rows, Outside(LabExit, Village, LabDoor));
        }

        // ---- 1番道路（16x30）。西に川が流れ、川べりは深い草むら ----
        private static readonly GridPos Route01South = new GridPos(7, 29);
        private static readonly GridPos Route01North = new GridPos(7, 0);

        private static FieldMap CreateRoute01()
        {
            var rows = new[]
            {
                "TTTTTTT=TTTTTTTT",
                "T......=.S.....T",
                "T.,,,,.=....,,,T",
                "T.,,,,.=....,,,T",
                "T.,,,,.=....,,,T",
                "T......=.......T",
                "TTTT...=...TTTTT",
                "T......=.......T",
                "T~~;;..=..,,,,.T",
                "T~~;;..=..,,,,.T",
                "T~~;;..=..,,,,.T",
                "T~~;;..=.......T",
                "T~~....=.......T",
                "T~~..TT=TT.....T",
                "T~~....=.......T",
                "T~~;;;.=.,,,,,.T",
                "T~~;;;.=.,,,,,.T",
                "T~~;;;.=.,,,,,.T",
                "T~~....=.......T",
                "T~~....=....TTTT",
                "T~~....=.......T",
                "T~~,,,.=.......T",
                "T~~,,,.=..,,,..T",
                "T~~,,,.=..,,,..T",
                "T~~....=..,,,..T",
                "T~~....=.......T",
                "T~~....=.......T",
                "T......=..S....T",
                "T......=.......T",
                "TTTTTTT=TTTTTTTT",
            };
            return new FieldMap(Route01, rows,
                new Warp(Route01South, StepFrom(Village, NorthExit, Direction.Down)),
                new Warp(Route01North, StepFrom(Town, TownSouth, Direction.Up)))
                .WithSign(new GridPos(9, 1), "この先 ツララ町")
                .WithSign(new GridPos(10, 27), "1番道路", "川べりの 深い 草むらには 川の ペンモンが いるらしい")
                .WithEncounters(
                    new EncounterTable(EncounterPercent,
                        new EncounterSlot(Town01Species.Kogara, 2, 4, 50),
                        new EncounterSlot(Town01Species.Omochi, 2, 4, 50)),
                    new EncounterTable(EncounterPercent,
                        new EncounterSlot(Town01Species.Yamame, 3, 5, 50),
                        new EncounterSlot(Town01Species.Kogara, 3, 5, 30),
                        new EncounterSlot(Town01Species.Omochi, 3, 5, 20)));
        }

        // ---- ツララ町（24x20）。道場・ぬくもり小屋・よろず屋・家 ----
        private static readonly GridPos TownSouth = new GridPos(7, 19);
        private static readonly GridPos TownEast = new GridPos(23, 9);
        /// <summary>道場の扉。最初は門下生が扉の前（1つ下）でふさぐ</summary>
        public static readonly GridPos GymDoor = new GridPos(7, 5);
        private static readonly GridPos TownHouseDoor = new GridPos(17, 4);
        private static readonly GridPos WarmHutDoor = new GridPos(4, 13);
        private static readonly GridPos ShopDoor = new GridPos(11, 13);

        private static FieldMap CreateTown()
        {
            var rows = new[]
            {
                "TTTTTTTTTTTTTTTTTTTTTTTT",
                "T......................T",
                "T..RRRRRRRR....RRRRR...T",
                "T..RRRRRRRR....RRRRR...T",
                "T..RRRRRRRR....WWDWW...T",
                "T..WWWWDWWW......=.....T",
                "T......=..S......=.....T",
                "T......===========.....T",
                "T............=.........T",
                "T..S.........===========",
                "T............=.........T",
                "T..RRRR..RRRR=...~~~~..T",
                "T..RRRR..RRRR=...~~~~..T",
                "T..WDWW..WWDW=...~~~~..T",
                "T...=..S...===.........T",
                "T...==========.........T",
                "T......=...............T",
                "T......=.....TT....TT..T",
                "T......=...............T",
                "TTTTTTT=TTTTTTTTTTTTTTTT",
            };
            return new FieldMap(Town, rows,
                new Warp(TownSouth, StepFrom(Route01, Route01North, Direction.Down)),
                new Warp(TownEast, StepFrom(Route02, Route02West, Direction.Right)),
                Inside(GymDoor, Gym, GymArrival),
                Inside(TownHouseDoor, TownHouse, HouseArrival),
                Inside(WarmHutDoor, WarmHut, HouseArrival),
                Inside(ShopDoor, Shop, HouseArrival))
                .WithSign(new GridPos(3, 9), "ここは ツララ町", "つららの 下がる 雪国の 町")
                .WithSign(new GridPos(10, 6), "ツララ町 道場", "岩のように 固い 守りの 道場")
                .WithSign(new GridPos(7, 14), "左: ぬくもり小屋（ペンモンの 回復・あずかり箱）", "右: よろず屋（道具の 売り買い）");
        }

        // ---- 道場（12x14） ----
        private static readonly GridPos GymExit = new GridPos(5, 13);
        /// <summary>道場に入ったときに立つ場所（扉の1つ奥）</summary>
        internal static readonly GridPos GymArrival = new GridPos(5, 12);

        /// <summary>道場の中（12x14）。どの町の道場も同じ形を使い回す</summary>
        internal static FieldMap CreateGym(string id, string outsideMap, GridPos outsideDoor)
        {
            var rows = new[]
            {
                "WWWWWWWWWWWW",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "W__________W",
                "WWWWWDWWWWWW",
            };
            return new FieldMap(id, rows, Outside(GymExit, outsideMap, outsideDoor));
        }

        // ---- 2番道路（30x24）。雪の林。東の奥は暗い林で深い草むら。南東の奥の東の端から 3番道路へ ----
        private static readonly GridPos Route02West = new GridPos(0, 12);
        /// <summary>3番道路への出口（南東の奥の東の端）。ガンテツに勝つまで町の人が手前でふさぐ</summary>
        public static readonly GridPos Route02East = new GridPos(29, 21);

        private static FieldMap CreateRoute02()
        {
            var rows = new[]
            {
                "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
                "T.....,,,,...TTTT....TTT;;;;;T",
                "T.....,,,,...TTTT....TTT;;;;;T",
                "T..TT.,,,,...........TTT;;;;;T",
                "T..TT..........,,,,......;;;.T",
                "T..............,,,,..TT......T",
                "TTT....TTT.....,,,,..TT..TTTTT",
                "T......TTT...........TT..;;;;T",
                "T..,,,,......TT..........;;;;T",
                "T..,,,,......TT..,,,,....;;;;T",
                "T..,,,,..........,,,,..TT....T",
                "T................,,,,..TT....T",
                "=.........TT.................T",
                "T.........TT..,,,,,..TTTT..TTT",
                "T..TT.........,,,,,..TTTT....T",
                "T..TT..,,,,...,,,,,......;;;.T",
                "T......,,,,..........TT..;;;.T",
                "T......,,,,....TTT...TT..;;;.T",
                "TTT............TTT.......;;;.T",
                "T.....,,,,.............T.....T",
                "T.....,,,,.....,,,,....T..S..T",
                "T..TT.,,,,.....,,,,....T......",
                "T..TT..........,,,,....T.....T",
                "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
            };
            return new FieldMap(Route02, rows,
                new Warp(Route02West, StepFrom(Town, TownEast, Direction.Left)),
                new Warp(Route02East, StepFrom(Town02Maps.Route03, Town02Maps.Route03West, Direction.Right)))
                .WithSign(new GridPos(26, 20), "この先 3番道路")
                .WithEncounters(
                    new EncounterTable(EncounterPercent,
                        new EncounterSlot(Town01Species.Konayuki, 6, 9, 25),
                        new EncounterSlot(Town01Species.Tsukushi, 6, 9, 25),
                        new EncounterSlot(Town01Species.Ishikoro, 6, 9, 20),
                        new EncounterSlot(Town01Species.Kogara, 6, 9, 15),
                        new EncounterSlot(Town01Species.Omochi, 6, 9, 10),
                        new EncounterSlot(Town01Species.Pachiri, 6, 9, 5)),
                    new EncounterTable(EncounterPercent,
                        new EncounterSlot(Town01Species.Warashi, 7, 10, 40),
                        new EncounterSlot(Town01Species.Konayuki, 7, 10, 25),
                        new EncounterSlot(Town01Species.Tsukushi, 7, 10, 25),
                        new EncounterSlot(Town01Species.Pachiri, 7, 10, 9),
                        new EncounterSlot(Town01Species.DiamondDust, 7, 10, 1)));
        }

        /// <summary>外の扉から建物の中へ。中では扉の前で奥（上）を向いて立つ</summary>
        internal static Warp Inside(GridPos outsideDoor, string mapId, GridPos arrival)
        {
            return new Warp(outsideDoor, new FieldSpot(mapId, arrival, Direction.Up));
        }

        /// <summary>建物の扉から外へ。外では扉の1つ下で手前（下）を向いて立つ</summary>
        internal static Warp Outside(GridPos exit, string outsideMap, GridPos outsideDoor)
        {
            return new Warp(exit, StepFrom(outsideMap, outsideDoor, Direction.Down));
        }

        /// <summary>from から facing へ1マス進んだマスに、facing を向いて立つ（出入口の手前・出口をふさぐ場所）</summary>
        internal static FieldSpot StepFrom(string mapId, GridPos from, Direction facing)
        {
            return new FieldSpot(mapId, from + facing.Offset(), facing);
        }
    }
}
