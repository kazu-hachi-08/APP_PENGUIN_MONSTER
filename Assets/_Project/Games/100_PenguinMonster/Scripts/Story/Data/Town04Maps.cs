using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// マップ。ユキオコシ町の北 → 7番道路（北）→ カザハナ町、町の東から 関所 → 8番道路 風花大橋（東へ延びる行き止まり）。
    /// 町の北は 9番道路の看板で止まる。文字は Town01Maps と同じ（大橋の # I &gt; * は FieldMap の表）。
    /// マップ ID は記録に入るので、一度出したら名前を変えないこと
    /// </summary>
    public static class Town04Maps
    {
        public const string Route07 = "Route07";
        public const string Town = "KazahanaTown";
        public const string TownHouse = "KazahanaHouse";
        public const string WarmHut = "KazahanaWarmHut";
        public const string Shop = "KazahanaShop";
        public const string BikeShop = "KazahanaBikeShop";
        public const string Gym = "KazahanaGym";
        public const string Gate = "KazahanaGate";
        public const string Bridge = "Route08Bridge";

        public static IEnumerable<FieldMap> CreateMaps()
        {
            return new[]
            {
                CreateRoute07(), CreateTown(), Town01Maps.CreateHouse(TownHouse, Town, TownHouseDoor), Town01Maps.CreateHouse(WarmHut, Town, WarmHutDoor),
                Town01Maps.CreateHouse(Shop, Town, ShopDoor), Town01Maps.CreateHouse(BikeShop, Town, BikeShopDoor), Town01Maps.CreateGym(Gym, Town, GymDoor),
                CreateGate(), CreateBridge(),
            };
        }

        // ---- 7番道路（22x34）。南の入口から北へ。北東と南東に岩の転がる深い草むら（岩場）、西に池、南西に奥の深い草むら ----
        /// <summary>ユキオコシ町からの入口（南の端）</summary>
        public static readonly GridPos Route07South = new GridPos(10, 33);
        private static readonly GridPos Route07North = new GridPos(10, 0);

        private static FieldMap CreateRoute07()
        {
            var rows = new[]
            {
                "TTTTTTTTTT=TTTTTTTTTTT",
                "T.........=S.........T",
                "T.,,,,....=....;;;;;.T",
                "T.,,,,....=....;T;;;.T",
                "T.,,,,....=....;;;T;.T",
                "T.........=....;;;;;.T",
                "TTTT......=..........T",
                "T.........=.....TT...T",
                "T..~~~~~..=..........T",
                "T.~~~~~~~.=..,,,,,...T",
                "T.~~~~~~~.=..,,,,,...T",
                "T.~~~~~~~.=..,,,,,...T",
                "T..~~~~~..=..........T",
                "T.........=....TT....T",
                "TTTTT.....=..........T",
                "T.........=......TTTTT",
                "T.;;;;;...=..........T",
                "T.;T;;T...=..,,,,....T",
                "T.;;;;;...=..,,,,....T",
                "T.;;T;;...=..,,,,....T",
                "T.........=..........T",
                "T....TT...=....;;;;..T",
                "T.........=....;T;;..T",
                "T.,,,,....=....;;T;..T",
                "T.,,,,....=....;;;;..T",
                "T.,,,,....=..........T",
                "T.........=..TT......T",
                "TTT.......=.....,,,,.T",
                "T.........=.....,,,,.T",
                "T..,,,,...=.....,,,,.T",
                "T..,,,,...=..........T",
                "T.........=.S........T",
                "T.........=..........T",
                "TTTTTTTTTT=TTTTTTTTTTT",
            };
            // 深い草むらの表は1枚のマップに1つなので、岩場（岩・氷）と奥（闇）は同じ表にまとめる
            return new FieldMap(Route07, rows,
                new Warp(Route07South, Town01Maps.StepFrom(Town03Maps.Town, Town03Maps.Route07Exit, Direction.Down)),
                new Warp(Route07North, Town01Maps.StepFrom(Town, TownSouth, Direction.Up)))
                .WithSign(new GridPos(11, 1), "この先 カザハナ町")
                .WithSign(new GridPos(12, 31), "7番道路", "岩場の 深い 草むらには 岩や 闇の ペンモンが いるらしい")
                .WithEncounters(
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town04Species.Soyokaze, 22, 25, 25),
                        new EncounterSlot(Town04Species.Yomogi, 22, 25, 15),
                        new EncounterSlot(Town04Species.Fuusen, 22, 25, 15),
                        new EncounterSlot(Town04Species.Kazaguruma, 22, 25, 15),
                        new EncounterSlot(Town04Species.Mizore, 22, 25, 10),
                        new EncounterSlot(Town04Species.Hiuchi, 22, 25, 5),
                        new EncounterSlot(Town03Species.Tsubame, 22, 25, 10),
                        new EncounterSlot(Town02Species.Hibari, 22, 25, 5)),
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town04Species.Gareba, 23, 26, 30),
                        new EncounterSlot(Town04Species.Yokaze, 23, 26, 30),
                        new EncounterSlot(Town04Species.Mizore, 23, 26, 15),
                        new EncounterSlot(Town04Species.Hiuchi, 23, 26, 5),
                        new EncounterSlot(Town03Species.Jari, 23, 26, 10),
                        new EncounterSlot(Town03Species.Yukiishi, 23, 26, 10)))
                .WithFishing(EncounterTable.Fishing(
                    new EncounterSlot(Town04Species.Haze, 22, 25, 80),
                    new EncounterSlot(Town03Species.Hatahata, 22, 25, 20)));
        }

        // ---- カザハナ町（26x22）。上の段に道場と自転車屋、真ん中に家、下の段にぬくもり小屋とよろず屋。草むらは無し ----
        private static readonly GridPos TownSouth = new GridPos(12, 21);
        private static readonly GridPos TownEast = new GridPos(25, 12);
        /// <summary>道場の扉。ハヤテに会う（Town04Flags.MetHayate）まで門下生が扉の前（1つ下）でふさぐ</summary>
        public static readonly GridPos GymDoor = new GridPos(6, 5);
        private static readonly GridPos BikeShopDoor = new GridPos(19, 5);
        private static readonly GridPos TownHouseDoor = new GridPos(3, 11);
        private static readonly GridPos WarmHutDoor = new GridPos(16, 17);
        private static readonly GridPos ShopDoor = new GridPos(21, 17);
        /// <summary>北の出口（9番道路）は看板で止まる。看板の上の道は、道が先へ続いて見えるように残す</summary>
        public static readonly GridPos Route09Sign = new GridPos(12, 1);

        private static FieldMap CreateTown()
        {
            var rows = new[]
            {
                "TTTTTTTTTTTT=TTTTTTTTTTTTT",
                "T...........S............T",
                "T.RRRRRRRR..=.....RRRR...T",
                "T.RRRRRRRR..=.....RRRR...T",
                "T.RRRRRRRR..=.....RRRR...T",
                "T.WWWWDWWW..=.....WDWW...T",
                "T.....=..S..=......=.....T",
                "T.....===============....T",
                "T...........=............T",
                "T.RRRR......=....TT......T",
                "T.RRRR......=............T",
                "T.WDWW......=.......S....T",
                "T..=........==============",
                "T..==========............T",
                "T...........=.RRRR..RRRR.T",
                "T..TT.......=.RRRR..RRRR.T",
                "T..TT.......=.RRRR..RRRR.T",
                "T...........=.WWDW..WDWW.T",
                "T...........=...=.S..=...T",
                "T...........==========...T",
                "T...........=......TT....T",
                "TTTTTTTTTTTT=TTTTTTTTTTTTT",
            };
            return new FieldMap(Town, rows,
                new Warp(TownSouth, Town01Maps.StepFrom(Route07, Route07North, Direction.Down)),
                new Warp(TownEast, Town01Maps.StepFrom(Gate, GateWest, Direction.Right)),
                Town01Maps.Inside(GymDoor, Gym, Town01Maps.GymArrival),
                Town01Maps.Inside(BikeShopDoor, BikeShop, Town01Maps.HouseArrival),
                Town01Maps.Inside(TownHouseDoor, TownHouse, Town01Maps.HouseArrival),
                Town01Maps.Inside(WarmHutDoor, WarmHut, Town01Maps.HouseArrival),
                Town01Maps.Inside(ShopDoor, Shop, Town01Maps.HouseArrival))
                .WithSign(Route09Sign, "この先 9番道路", "続きは 次の 更新で！")
                .WithSign(new GridPos(9, 6), "カザハナ町 道場", "風を 読み 空を 舞う 鳥の 道場")
                .WithSign(new GridPos(18, 18), "左: ぬくもり小屋（ペンモンの 回復・あずかり箱）", "右: よろず屋（道具の 売り買い）")
                .WithSign(new GridPos(20, 11), "ここは カザハナ町", "晴れた 空に 雪が 舞う 風の 町。東へ 行くと 風花大橋");
        }

        // ---- 関所（8x6）。西の扉が町、東の扉が大橋。建物の中なので自転車は降りる ----
        private static readonly GridPos GateWest = new GridPos(0, 2);
        /// <summary>大橋へ出る東の扉。自転車を持つまで番の人が手前でふさぐ</summary>
        public static readonly GridPos GateEast = new GridPos(7, 2);

        private static FieldMap CreateGate()
        {
            var rows = new[]
            {
                "WWWWWWWW",
                "W______W",
                "D______D",
                "W______W",
                "W______W",
                "WWWWWWWW",
            };
            return new FieldMap(Gate, rows,
                new Warp(GateWest, Town01Maps.StepFrom(Town, TownEast, Direction.Left)),
                new Warp(GateEast, Town01Maps.StepFrom(Bridge, BridgeWest, Direction.Right)))
                .AsIndoors();
        }

        // ---- 8番道路 風花大橋（40x12）。海の上を東へ延びる行き止まり ----
        // 西から: 下り坂 → 休憩所（草むら・南の桟橋で釣れる）→ 北にふくらんだ展望マス → 下り坂 → 東の端の展望台
        private static readonly GridPos BridgeWest = new GridPos(0, 5);
        /// <summary>橋の真ん中の展望マス（隠しコマンドの場所）</summary>
        public static readonly GridPos Lookout = new GridPos(27, 3);

        private static FieldMap CreateBridge()
        {
            var rows = new[]
            {
                "~~~~~~~~~~~~~~IIIIIII~~~~~~~~~~~~~~~~~~~",
                "~~~~~~~~~~~~~~I,,,,,I~~~~~~~~~~~~~~~IIII",
                "~~~~~~~~~~~~~~I,,,,,I~~~~IIIII~~~~~~###I",
                "IISIIIIIIIIIIII,,,,,IIIIII#*#IIIIIII###I",
                "I#######>>>>>>################>>>>#####I",
                "########>>>>>>################>>>>#####I",
                "I#######>>>>>>################>>>>#####I",
                "I#######>>>>>>################>>>>#####I",
                "IIIIIIIIIIIIIII,,,,,IIIIIIIIIIIIIIII###I",
                "~~~~~~~~~~~~~~I,,,,,I~~~~~~~~~~~~~~~###I",
                "~~~~~~~~~~~~~~I,,,,,####I~~~~~~~~~~~IIII",
                "~~~~~~~~~~~~~~IIIIIII~~~~~~~~~~~~~~~~~~~",
            };
            return new FieldMap(Bridge, rows,
                new Warp(BridgeWest, Town01Maps.StepFrom(Gate, GateEast, Direction.Left)))
                .WithSign(new GridPos(2, 3), "8番道路 風花大橋", "東の 端の 展望台まで 自転車で 一気に 下ろう！")
                .AsBikeRoad()
                .WithEncounters(
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town04Species.Umineko, 24, 27, 45),
                        new EncounterSlot(Town04Species.Soyokaze, 24, 27, 30),
                        new EncounterSlot(Town04Species.Kazaguruma, 24, 27, 14),
                        new EncounterSlot(Town04Species.Shabon, 24, 27, 10),
                        new EncounterSlot(Town04Species.Hiraishin, 24, 27, 1)))
                .WithFishing(EncounterTable.Fishing(
                    new EncounterSlot(Town04Species.Komai, 24, 28, 45),
                    new EncounterSlot(Town04Species.Haze, 24, 28, 40),
                    new EncounterSlot(Town03Species.Hatahata, 24, 28, 14),
                    new EncounterSlot(Town04Species.Oonamazu, 24, 28, 1)));
        }
    }
}
