using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// マップ。2番道路の奥 → 3番道路（北）→ カガミコ町、町の東から 4番道路（行き止まり）。
    /// 町の北から 5番道路（サエに勝つまで門下生がふさぐ）。文字は Town01Maps と同じ。
    /// マップ ID は記録に入るので、一度出したら名前を変えないこと
    /// </summary>
    public static class Town02Maps
    {
        public const string Route03 = "Route03";
        public const string Town = "KagamikoTown";
        public const string TownHouse = "KagamikoHouse";
        public const string WarmHut = "KagamikoWarmHut";
        public const string Shop = "KagamikoShop";
        public const string Gym = "KagamikoGym";
        public const string Route04 = "Route04";

        public static IEnumerable<FieldMap> CreateMaps()
        {
            return new[]
            {
                CreateRoute03(), CreateTown(), Town01Maps.CreateHouse(TownHouse, Town, TownHouseDoor), Town01Maps.CreateHouse(WarmHut, Town, WarmHutDoor),
                Town01Maps.CreateHouse(Shop, Town, ShopDoor), Town01Maps.CreateGym(Gym, Town, GymDoor), CreateRoute04(),
            };
        }

        // ---- 3番道路（20x32）。西の入口から北へ。東に川が流れ、川べりと西の岩場が深い草むら ----
        /// <summary>2番道路からの入口（南西の西の端）</summary>
        public static readonly GridPos Route03West = new GridPos(0, 28);
        private static readonly GridPos Route03North = new GridPos(9, 0);

        private static FieldMap CreateRoute03()
        {
            var rows = new[]
            {
                "TTTTTTTTT=TTTTTTTTTT",
                "T........=S....;~~~T",
                "T.,,,,...=.....;~~~T",
                "T.,,,,...=.....;~~~T",
                "T.,,,,...=..TT.;~~~T",
                "T........=..TT.;~~~T",
                "TTTT.....=.......~~T",
                "T........=.......~~T",
                "T..,,,,,.=...;;;.~~T",
                "T..,,,,,.=...;;;.~~T",
                "T..,,,,,.=...;;;.~~T",
                "T........=.......~~T",
                "T.TT.TT..=..TTTT.~~T",
                "T.;;;;T..=.......~~T",
                "T.;TT;...=..,,,,.~~T",
                "T.;;;;T..=..,,,,.~~T",
                "T.TT.TT..=..,,,,.~~T",
                "T........=.......~~T",
                "TTTT.....=....TTT~~T",
                "T........=.......~~T",
                "T..,,,,..=..,,,..~~T",
                "T..,,,,..=..,,,..~~T",
                "T..,,,,..=..,,,..~~T",
                "T........=.......~~T",
                "T.TT.....=...;;;;~~T",
                "T.TT.....=...;;;;~~T",
                "T........=.......~~T",
                "T........=..S....~~T",
                "==========.......~~T",
                "T.,,,,.........TTTTT",
                "T.,,,,..........TTTT",
                "TTTTTTTTTTTTTTTTTTTT",
            };
            // 深い草むらの表は1枚のマップに1つなので、川べりと岩場は同じ表にまとめる（川の子と岩の子が両方出る）
            return new FieldMap(Route03, rows,
                new Warp(Route03West, Town01Maps.StepFrom(Town01Maps.Route02, Town01Maps.Route02East, Direction.Left)),
                new Warp(Route03North, Town01Maps.StepFrom(Town, TownSouth, Direction.Up)))
                .WithSign(new GridPos(10, 1), "この先 カガミコ町")
                .WithSign(new GridPos(12, 27), "3番道路", "川べりと 岩場の 深い 草むらには 川や 岩の ペンモンが いるらしい")
                .WithEncounters(
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town02Species.Hibari, 10, 13, 25),
                        new EncounterSlot(Town02Species.Hanten, 10, 13, 25),
                        new EncounterSlot(Town02Species.Fuyume, 10, 13, 20),
                        new EncounterSlot(Town01Species.Kogara, 10, 13, 10),
                        new EncounterSlot(Town01Species.Konayuki, 10, 13, 10),
                        new EncounterSlot(Town01Species.Tsukushi, 10, 13, 5),
                        new EncounterSlot(Town02Species.Yutanpo, 10, 13, 5)),
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town02Species.Wakasagi, 11, 14, 35),
                        new EncounterSlot(Town02Species.Gorota, 11, 14, 35),
                        new EncounterSlot(Town01Species.Yamame, 11, 14, 10),
                        new EncounterSlot(Town01Species.Ishikoro, 11, 14, 10),
                        new EncounterSlot(Town02Species.Yutanpo, 11, 14, 5),
                        new EncounterSlot(Town01Species.Omochi, 11, 14, 5)))
                .WithFishing(EncounterTable.Fishing(
                    new EncounterSlot(Town02Species.Wakasagi, 10, 13, 70),
                    new EncounterSlot(Town01Species.Yamame, 10, 13, 30)));
        }

        // ---- カガミコ町（26x22）。真ん中に凍った湖。上の段に道場・ぬくもり小屋・よろず屋、左下に家 ----
        private static readonly GridPos TownSouth = new GridPos(12, 21);
        private static readonly GridPos TownEast = new GridPos(25, 7);
        /// <summary>道場の扉。4番道路の奥の出来事まで門下生が扉の前（1つ下）でふさぐ</summary>
        public static readonly GridPos GymDoor = new GridPos(6, 5);
        private static readonly GridPos WarmHutDoor = new GridPos(13, 5);
        private static readonly GridPos ShopDoor = new GridPos(19, 5);
        private static readonly GridPos TownHouseDoor = new GridPos(3, 18);
        /// <summary>北の出口（5番道路）。サエに勝つまで門下生が手前でふさぐ</summary>
        public static readonly GridPos Route05Exit = new GridPos(11, 0);
        /// <summary>北の出口の横の看板。出口の上に立つと道をふさぐので、道の横に置く</summary>
        public static readonly GridPos Route05Sign = new GridPos(12, 1);

        private static FieldMap CreateTown()
        {
            var rows = new[]
            {
                "TTTTTTTTTTT=TTTTTTTTTTTTTT",
                "T..........=S............T",
                "T.RRRRRRRR.=.............T",
                "T.RRRRRRRR.=RRRR.RRRR....T",
                "T.RRRRRRRR.=RRRR.RRRR....T",
                "T.WWWWDWWW.=WDWW.WWDW....T",
                "T.....=..S.=.=..S..=.....T",
                "T..=======================",
                "T..=.,,,,,,,,,,,,,,......T",
                "T..=.,~~~~~~~~~~~~,......T",
                "T..=.,~~~~~~~~~~~~,..S...T",
                "T..=.,~~~~~~~~~~~~,......T",
                "T..=.,~~~~~~~~~~~~,......T",
                "T..=.,~~~~~~~~~~~~,......T",
                "T..=.,,,,,,,,,,,,,,......T",
                "T..==========.......TT...T",
                "T.RRRR......=............T",
                "T.RRRR......=.....TT.....T",
                "T.WDWW......=............T",
                "T..=........=........TT..T",
                "T..==========............T",
                "TTTTTTTTTTTT=TTTTTTTTTTTTT",
            };
            // 湖は水のタイルを使い回す。まわりの草むらが「湖のほとり」
            return new FieldMap(Town, rows,
                new Warp(TownSouth, Town01Maps.StepFrom(Route03, Route03North, Direction.Down)),
                new Warp(TownEast, Town01Maps.StepFrom(Route04, Route04West, Direction.Right)),
                new Warp(Route05Exit, Town01Maps.StepFrom(Town03Maps.Route05, Town03Maps.Route05South, Direction.Up)),
                Town01Maps.Inside(GymDoor, Gym, Town01Maps.GymArrival),
                Town01Maps.Inside(WarmHutDoor, WarmHut, Town01Maps.HouseArrival),
                Town01Maps.Inside(ShopDoor, Shop, Town01Maps.HouseArrival),
                Town01Maps.Inside(TownHouseDoor, TownHouse, Town01Maps.HouseArrival))
                .WithSign(Route05Sign, "この先 5番道路")
                .WithSign(new GridPos(9, 6), "カガミコ町 道場", "鏡のように 冴えた 氷の 道場")
                .WithSign(new GridPos(16, 6), "左: ぬくもり小屋（ペンモンの 回復・あずかり箱）", "右: よろず屋（道具の 売り買い）")
                .WithSign(new GridPos(21, 10), "ここは カガミコ町", "鏡のように 凍った 湖の ほとりの 町")
                .WithEncounters(
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town02Species.Wakasagi, 11, 14, 40),
                        new EncounterSlot(Town02Species.Hasuha, 11, 14, 40),
                        new EncounterSlot(Town01Species.Yamame, 11, 14, 15),
                        new EncounterSlot(Town02Species.Tsukiakari, 11, 14, 5)))
                .WithFishing(EncounterTable.Fishing(
                    new EncounterSlot(Town02Species.Wakasagi, 11, 14, 60),
                    new EncounterSlot(Town02Species.Hasuha, 11, 14, 40)));
        }

        // ---- 4番道路（30x20）。行き止まり。東の奥は木で仕切った細長い深い草むらで、入口は1か所 ----
        private static readonly GridPos Route04West = new GridPos(0, 9);
        /// <summary>奥の深い草むらの入口。ここで4番道路の奥の出来事が起きる</summary>
        public static readonly GridPos Route04InnerEntrance = new GridPos(25, 8);

        private static FieldMap CreateRoute04()
        {
            var rows = new[]
            {
                "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
                "T....,,,,....TT.....,,,,.T;;;T",
                "T....,,,,....TT.....,,,,.T;;;T",
                "T..TT,,,,...........,,,,.T;;;T",
                "T..TT.......,,,,.........T...T",
                "T...........,,,,....TT...T;;;T",
                "TTT.....TT..,,,,....TT...T;;;T",
                "T.......TT...............T;;;T",
                "T..,,,,...........,,,,.......T",
                "=..,,,,....TT....,,,,,...T;;;T",
                "T..,,,,....TT....,,,,,...T;;;T",
                "T...............,,,,,....T;;;T",
                "T..TT....,,,,.........TT.T...T",
                "T..TT....,,,,.........TT.T;;;T",
                "T........,,,,....,,,,....T;;;T",
                "TTT..............,,,,....T;;;T",
                "T....,,,,...TT...,,,,....T;;;T",
                "T....,,,,...TT...........T;;;T",
                "T....,,,,.....S..........TTTTT",
                "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
            };
            return new FieldMap(Route04, rows,
                new Warp(Route04West, Town01Maps.StepFrom(Town, TownEast, Direction.Left)))
                .WithSign(new GridPos(14, 18), "4番道路", "東の 奥の 深い 草むらには 闇の ペンモンが ひそむという")
                .WithEncounters(
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town02Species.Hibana, 12, 15, 25),
                        new EncounterSlot(Town02Species.Shimobashira, 12, 15, 25),
                        new EncounterSlot(Town02Species.Kohakuchou, 12, 15, 20),
                        new EncounterSlot(Town02Species.Hibari, 12, 15, 15),
                        new EncounterSlot(Town01Species.Pachiri, 12, 15, 10),
                        new EncounterSlot(Town01Species.Konayuki, 12, 15, 5)),
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town02Species.Itachi, 13, 16, 45),
                        new EncounterSlot(Town01Species.Warashi, 13, 16, 20),
                        new EncounterSlot(Town02Species.Hibana, 13, 16, 15),
                        new EncounterSlot(Town02Species.Shimobashira, 13, 16, 15),
                        new EncounterSlot(Town02Species.Kohakuchou, 13, 16, 4),
                        new EncounterSlot(Town02Species.Shinkirou, 13, 16, 1)));
        }
    }
}
