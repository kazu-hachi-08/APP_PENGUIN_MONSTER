using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// カガミコ町の人・3番と4番道路のトレーナー・No.209（オマモリペン）をくれる家のおじいさん
    /// </summary>
    public static partial class Town02People
    {
        /// <summary>もらう子のレベル（3番道路の野生より少し上で、すぐ戦える）</summary>
        public const int OmamoriLevel = 15;

        /// <summary>家のおじいさんの場所。主人公はその1つ下で話す</summary>
        public static readonly FieldSpot OmamoriGiverSpot = new FieldSpot(Town02Maps.TownHouse, new GridPos(2, 2), Direction.Down);

        private static IEnumerable<FieldNpc> TownAndRoutes()
        {
            return new[]
            {
                OmamoriGiver(), TownGuide(), LakeKid(), GymHintResident(),
                Town01People.RouteTrainer("route03Snowfield", NpcLook.Youngster, Town02Trainers.Route03SnowfieldKid, Town02Maps.Route03,
                    new GridPos(6, 19), Direction.Right, "雪原で 育てた 子たちだ！ 勝負！", "3体 いても 勝てないなんて……"),
                Town01People.RouteTrainer("route03Hiker", NpcLook.Villager, Town02Trainers.Route03Hiker, Town02Maps.Route03,
                    new GridPos(7, 14), Direction.Right, "岩場で 鍛えた 子の 固さを 見よ！", "氷の 道場には 岩の 技が よく 効くぞ"),
                Town01People.RouteTrainer("route03Angler", NpcLook.Villager, Town02Trainers.Route03Angler, Town02Maps.Route03,
                    new GridPos(12, 23), Direction.Left, "カガミコの 湖へ 釣りに 行く ところさ！", "湖の ほとりの 草むらにも 海の 子が いるよ"),
                Town01People.RouteTrainer("route04Thunder", NpcLook.Youngster, Town02Trainers.Route04ThunderKid, Town02Maps.Route04,
                    new GridPos(9, 9), Direction.Left, "ビリビリ いくよ！", "雷の 子は 海と 鳥に 強いんだ"),
                Town01People.RouteTrainer("route04Skater", NpcLook.Youngster, Town02Trainers.Route04Skater, Town02Maps.Route04,
                    new GridPos(14, 7), Direction.Down, "湖で 滑る 練習の 帰りさ！", "サエさんの 滑りは もっと きれいなんだ"),
                Town01People.RouteTrainer("route04Night", NpcLook.Villager, Town02Trainers.Route04NightWalker, Town02Maps.Route04,
                    new GridPos(22, 11), Direction.Up, "夜の 闇に まぎれる 子を 見せよう", "奥の 深い 草むらには もっと 不思議な 子が いる らしい"),
            };
        }

        /// <summary>旅に連れて行けなくなった子を託す。はい で1回だけもらえる（手持ちがいっぱいなら あずかり箱へ送られる）</summary>
        private static FieldNpc OmamoriGiver()
        {
            return new FieldNpc("kagamikoOmamoriGiver", NpcLook.Villager,
                NpcPlacement.Until(Town02Flags.GotOmamori, OmamoriGiverSpot,
                    EventStep.Say("わしは この 子と ずっと 湖の ほとりで 暮らして きた"),
                    EventStep.Say("じゃが もう 年での、この 子を 旅に 連れて いけん"),
                    EventStep.Ask("この 子を 育てて くれないか？",
                        EventStep.Say("そうか…… 気が 変わったら また 来て おくれ")),
                    EventStep.Give(Town02Species.Omamori, OmamoriLevel),
                    EventStep.SetFlag(Town02Flags.GotOmamori),
                    EventStep.Say("大事に して やって おくれ")),
                NpcPlacement.Always(OmamoriGiverSpot,
                    EventStep.Say("オマモリペンは 元気に しとるかね？")));
        }

        private static FieldNpc TownGuide()
        {
            return new FieldNpc("kagamikoGuide", NpcLook.Villager,
                NpcPlacement.Always(new FieldSpot(Town02Maps.Town, new GridPos(14, 19), Direction.Left),
                    EventStep.Say("カガミコ町へ ようこそ！"),
                    EventStep.Say("この 町の 湖は 一年中 鏡のように 凍って いるんだ")));
        }

        private static FieldNpc LakeKid()
        {
            return new FieldNpc("kagamikoLakeKid", NpcLook.Youngster,
                NpcPlacement.Always(new FieldSpot(Town02Maps.Town, new GridPos(20, 12), Direction.Left),
                    EventStep.Say("湖の ほとりの 草むらには 氷と 海の ペンモンが いるよ"),
                    EventStep.Say("夜空みたいに 光る 子を 見たって 人も いるんだ！")));
        }

        private static FieldNpc GymHintResident()
        {
            return new FieldNpc("kagamikoGymHint", NpcLook.Mom,
                NpcPlacement.Always(new FieldSpot(Town02Maps.Town, new GridPos(22, 4), Direction.Down),
                    EventStep.Say("道場主の サエさんは 氷の ペンモンの 使い手よ"),
                    EventStep.Say("火や 岩の 技が よく 効くわ")));
        }
    }
}
