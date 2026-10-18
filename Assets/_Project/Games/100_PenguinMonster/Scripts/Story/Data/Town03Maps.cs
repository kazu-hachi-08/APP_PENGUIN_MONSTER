using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り3のマップ（PLAN_STORY_TOWN03.md §2）。カガミコ町の北 → 5番道路（北）→ ユキオコシ町、町の東から 6番道路（行き止まり）。
    /// 6番道路の奥に発電所。町の北は 7番道路の看板で止まる。文字は Town01Maps と同じ。
    /// マップ ID は記録に入るので、一度出したら名前を変えないこと
    /// </summary>
    public static class Town03Maps
    {
        public const string Route05 = "Route05";
        public const string Town = "YukiokoshiTown";
        public const string TownHouse = "YukiokoshiHouse";
        public const string WarmHut = "YukiokoshiWarmHut";
        public const string Shop = "YukiokoshiShop";
        public const string Gym = "YukiokoshiGym";
        public const string Route06 = "Route06";
        public const string PowerPlant = "PowerPlant";

        public static IEnumerable<FieldMap> CreateMaps()
        {
            return new[]
            {
                CreateRoute05(), CreateTown(), Town01Maps.CreateHouse(TownHouse, Town, TownHouseDoor), Town01Maps.CreateHouse(WarmHut, Town, WarmHutDoor),
                Town01Maps.CreateHouse(Shop, Town, ShopDoor), Town01Maps.CreateGym(Gym, Town, GymDoor), CreateRoute06(), CreatePowerPlant(),
            };
        }

        // ---- 5番道路（20x32）。南の入口から北へ。西と東に岩の転がる深い草むら（岩場） ----
        /// <summary>カガミコ町からの入口（南の端）</summary>
        public static readonly GridPos Route05South = new GridPos(9, 31);
        private static readonly GridPos Route05North = new GridPos(9, 0);

        private static FieldMap CreateRoute05()
        {
            var rows = new[]
            {
                "TTTTTTTTT=TTTTTTTTTT",
                "T........=S.......TT",
                "T.,,,,...=....,,,,.T",
                "T.,,,,...=....,,,,.T",
                "T.,,,,...=....,,,,.T",
                "T........=.........T",
                "TTTTT....=....TTTTTT",
                "T;;;;T...=.........T",
                "T;TT;T...=..,,,,,..T",
                "T;;;;....=..,,,,,..T",
                "T;TT;T...=..,,,,,..T",
                "T;;;;T...=.........T",
                "TTTTTT...=...TT....T",
                "T........=...TT....T",
                "T..,,,,..=.........T",
                "T..,,,,..=....;;;;.T",
                "T..,,,,..=....;TT;.T",
                "T........=....;;;;.T",
                "TTT......=.........T",
                "T........=......TTTT",
                "T.,,,,,..=.........T",
                "T.,,,,,..=..,,,,...T",
                "T.,,,,,..=..,,,,...T",
                "T........=..,,,,...T",
                "T..TT....=.........T",
                "T..TT....=...;;;;..T",
                "T........=...;TT;..T",
                "T;;;;....=...;;;;..T",
                "T;TT;....=.........T",
                "T;;;;....=..S......T",
                "T........=.........T",
                "TTTTTTTTT=TTTTTTTTTT",
            };
            // 深い草むらの中の T は岩のつもり。岩場の子（岩・氷）は深い草むらの表にまとめる
            return new FieldMap(Route05, rows,
                new Warp(Route05South, Town01Maps.StepFrom(Town02Maps.Town, Town02Maps.Route05Exit, Direction.Down)),
                new Warp(Route05North, Town01Maps.StepFrom(Town, TownSouth, Direction.Up)))
                .WithSign(new GridPos(10, 1), "この先 ユキオコシ町")
                .WithSign(new GridPos(12, 29), "5番道路", "岩場の 深い 草むらには 岩や 氷の ペンモンが いるらしい")
                .WithEncounters(
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town03Species.Mameden, 16, 19, 25),
                        new EncounterSlot(Town03Species.Tsubame, 16, 19, 20),
                        new EncounterSlot(Town03Species.Sasa, 16, 19, 15),
                        new EncounterSlot(Town03Species.Arare, 16, 19, 15),
                        new EncounterSlot(Town03Species.Yukigutsu, 16, 19, 15),
                        new EncounterSlot(Town02Species.Hibari, 16, 19, 5),
                        new EncounterSlot(Town02Species.Hanten, 16, 19, 5)),
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town03Species.Jari, 17, 20, 35),
                        new EncounterSlot(Town03Species.Yukiishi, 17, 20, 30),
                        new EncounterSlot(Town03Species.Arare, 17, 20, 10),
                        new EncounterSlot(Town02Species.Gorota, 17, 20, 15),
                        new EncounterSlot(Town01Species.Ishikoro, 17, 20, 10)));
        }

        // ---- ユキオコシ町（26x22）。上の段に道場と家、下の段にぬくもり小屋とよろず屋。草むらは無し ----
        private static readonly GridPos TownSouth = new GridPos(12, 21);
        private static readonly GridPos TownEast = new GridPos(25, 12);
        /// <summary>道場の扉。発電所の出来事（Town03Flags.PowerRestored）まで道場主が扉の前（1つ下）でふさぐ</summary>
        public static readonly GridPos GymDoor = new GridPos(6, 5);
        private static readonly GridPos TownHouseDoor = new GridPos(19, 5);
        private static readonly GridPos WarmHutDoor = new GridPos(16, 17);
        private static readonly GridPos ShopDoor = new GridPos(21, 17);
        /// <summary>北の出口（7番道路）は看板で止まる。看板の上の道は、道が先へ続いて見えるように残す</summary>
        public static readonly GridPos Route07Sign = new GridPos(12, 1);

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
                "T..TT.......=....TT......T",
                "T...........=............T",
                "T...........=.......S....T",
                "T...........==============",
                "T...........=............T",
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
                new Warp(TownSouth, Town01Maps.StepFrom(Route05, Route05North, Direction.Down)),
                new Warp(TownEast, Town01Maps.StepFrom(Route06, Route06West, Direction.Right)),
                Town01Maps.Inside(GymDoor, Gym, Town01Maps.GymArrival),
                Town01Maps.Inside(TownHouseDoor, TownHouse, Town01Maps.HouseArrival),
                Town01Maps.Inside(WarmHutDoor, WarmHut, Town01Maps.HouseArrival),
                Town01Maps.Inside(ShopDoor, Shop, Town01Maps.HouseArrival))
                .WithSign(Route07Sign, "この先 7番道路", "続きは 次の 更新で！")
                .WithSign(new GridPos(9, 6), "ユキオコシ町 道場", "雷鳴の とどろく 雷の 道場")
                .WithSign(new GridPos(18, 18), "左: ぬくもり小屋（ペンモンの 回復・あずかり箱）", "右: よろず屋（道具の 売り買い）")
                .WithSign(new GridPos(20, 11), "ここは ユキオコシ町", "冬の 雷 「雪起こし」が 鳴り響く 町");
        }

        // ---- 6番道路（30x20）。行き止まり。北西に水辺の深い草むら、南東の奥に深い草むら、北東の奥に発電所 ----
        private static readonly GridPos Route06West = new GridPos(0, 10);
        /// <summary>発電所の扉（6番道路の北東）</summary>
        public static readonly GridPos PowerPlantDoor = new GridPos(24, 4);

        private static FieldMap CreateRoute06()
        {
            var rows = new[]
            {
                "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
                "T~~~~~~~;;;......,,,,.RRRRRR.T",
                "T~~~~~~~;;;......,,,,.RRRRRR.T",
                "T~~~~~~~;;;..TT.......RRRRRR.T",
                "T~~~~~~~;;;..TT.......WWDWWW.T",
                "T~~~~~~;;;;...........S.=....T",
                "T~~~~~;;;;..,,,,........=....T",
                "T~~~~;;;;...,,,,....TT..=....T",
                "T~~~;;;;....,,,,....TT..=....T",
                "TTT;;;;.................=....T",
                "=========================....T",
                "T....,,,,....TT.....,,,,.T;;;T",
                "T....,,,,....TT.....,,,,.T;;;T",
                "T....,,,,...........,,,,..;;;T",
                "T..TT.........,,,,.......T;;;T",
                "T..TT.........,,,,.......T;;;T",
                "T......,,,,...,,,,...TT..T;;;T",
                "T......,,,,.....S....TT..T;;;T",
                "T......,,,,..............TTTTT",
                "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
            };
            // 深い草むらの表は1枚のマップに1つなので、水辺と奥は同じ表にまとめる（海の子と闇・オーロラの子が両方出る）
            return new FieldMap(Route06, rows,
                new Warp(Route06West, Town01Maps.StepFrom(Town, TownEast, Direction.Left)),
                Town01Maps.Inside(PowerPlantDoor, PowerPlant, PowerPlantArrival))
                .WithSign(new GridPos(22, 5), "ユキオコシ発電所", "停電中…… 中で 何かが バチバチ 光っている")
                .WithSign(new GridPos(16, 17), "6番道路", "水辺と 奥の 深い 草むらには 海や 闇の ペンモンが いるらしい")
                .WithEncounters(
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town03Species.Mameden, 18, 21, 25),
                        new EncounterSlot(Town03Species.Densen, 18, 21, 25),
                        new EncounterSlot(Town03Species.Tsubame, 18, 21, 20),
                        new EncounterSlot(Town03Species.Kairo, 18, 21, 5),
                        new EncounterSlot(Town02Species.Hibana, 18, 21, 15),
                        new EncounterSlot(Town02Species.Hibari, 18, 21, 10)),
                    new EncounterTable(Town01Maps.EncounterPercent,
                        new EncounterSlot(Town03Species.Hatahata, 19, 22, 30),
                        new EncounterSlot(Town03Species.Teiden, 19, 22, 30),
                        new EncounterSlot(Town03Species.Yukiakari, 19, 22, 9),
                        new EncounterSlot(Town03Species.SaintElmo, 19, 22, 1),
                        new EncounterSlot(Town02Species.Wakasagi, 19, 22, 15),
                        new EncounterSlot(Town02Species.Itachi, 19, 22, 15)));
        }

        // ---- 発電所（12x10）。建物の中。並んだ機械（壁）の間を通って奥へ。野生は出ない ----
        private static readonly GridPos PowerPlantExit = new GridPos(5, 9);
        private static readonly GridPos PowerPlantArrival = new GridPos(5, 8);

        private static FieldMap CreatePowerPlant()
        {
            var rows = new[]
            {
                "WWWWWWWWWWWW",
                "W__________W",
                "W__________W",
                "W_WW____WW_W",
                "W_WW____WW_W",
                "W__________W",
                "W_WW____WW_W",
                "W_WW____WW_W",
                "W__________W",
                "WWWWWDWWWWWW",
            };
            return new FieldMap(PowerPlant, rows, Town01Maps.Outside(PowerPlantExit, Route06, PowerPlantDoor));
        }
    }
}
