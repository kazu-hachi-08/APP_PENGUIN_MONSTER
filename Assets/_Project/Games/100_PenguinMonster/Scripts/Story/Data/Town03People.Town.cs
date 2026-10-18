using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ユキオコシ町の人・母のランニングシューズ・5番と6番道路のトレーナー・No.238（カンテラペン）をくれる家のおばあさん
    /// </summary>
    public static partial class Town03People
    {
        /// <summary>もらう子のレベル（5番道路の野生 Lv16〜19 より少し上で、すぐ戦える）</summary>
        public const int KanteraLevel = 20;

        /// <summary>ユキオコシ町の南の入口の横。初めて町に入ると、ここで待っていた母が話してくる</summary>
        public static readonly FieldSpot MomAtTownSouth = new FieldSpot(Town03Maps.Town, new GridPos(13, 20), Direction.Left);
        /// <summary>家のおばあさんの場所。主人公はその1つ下で話す</summary>
        public static readonly FieldSpot KanteraGiverSpot = new FieldSpot(Town03Maps.TownHouse, new GridPos(2, 2), Direction.Down);

        private static IEnumerable<FieldNpc> TownAndRoutes()
        {
            return new[]
            {
                MomWithShoes(), KanteraGiver(), TownGuide(), GymHintResident(), ThunderKid(),
                Town01People.RouteTrainer("route05Hiker", NpcLook.Villager, Town03Trainers.Route05Hiker, Town03Maps.Route05,
                    new GridPos(7, 25), Direction.Right, "岩場で 鍛えた 子の 固さを 見よ！", "雷の 技は 岩の 子には 効かないんだ"),
                Town01People.RouteTrainer("route05Snowfield", NpcLook.Youngster, Town03Trainers.Route05SnowfieldKid, Town03Maps.Route05,
                    new GridPos(12, 18), Direction.Left, "雪原で 見つけた 子たちだ！ 勝負！", "3体 いても かなわないなんて……"),
                Town01People.RouteTrainer("route05Thunder", NpcLook.Youngster, Town03Trainers.Route05ThunderKid, Town03Maps.Route05,
                    new GridPos(6, 13), Direction.Right, "ユキオコシ町の 道場に 入門 したいんだ！", "ライゾウさんの 雷は もっと すごいんだ"),
                Town01People.RouteTrainer("route06Angler", NpcLook.Villager, Town03Trainers.Route06Angler, Town03Maps.Route06,
                    new GridPos(8, 9), Direction.Down, "水辺の 子の 力、見せて やろう！", "水辺の 深い 草むらには 海の 子が 多いよ"),
                Town01People.RouteTrainer("route06LineWorker", NpcLook.Villager, Town03Trainers.Route06LineWorker, Town03Maps.Route06,
                    new GridPos(26, 7), Direction.Left, "停電の 原因を 調べて いる ところだ！", "発電所の 中に 何かが 居座って いるらしい"),
                Town01People.RouteTrainer("route06Night", NpcLook.Villager, Town03Trainers.Route06NightWalker, Town03Maps.Route06,
                    new GridPos(16, 13), Direction.Up, "停電の 夜は 闇の 子が 元気なのさ", "夜タマゴなら 闇の 子が 捕まえやすいよ"),
            };
        }

        /// <summary>
        /// 初めて町に入ったとき、入口で待っていた母がランニングシューズをくれる。
        /// もらってすぐ走れるよう、走る ON も立てる。渡したら村へ帰って居なくなる
        /// </summary>
        private static FieldNpc MomWithShoes()
        {
            return new FieldNpc("yukiokoshiMom", NpcLook.Mom,
                NpcPlacement.GreetUntil(Town03Flags.RunningShoes, MomAtTownSouth,
                    EventStep.Say("あら！ やっと 来たわね"),
                    EventStep.Say("シラカバ博士から 羽章を 2つも もらったって 聞いたわよ"),
                    EventStep.Say("旅が 楽に なるように これを 持って きたの"),
                    EventStep.SetFlag(Town03Flags.RunningShoes),
                    EventStep.SetFlag(RunningShoes.RunOnFlag),
                    EventStep.Say("ランニングシューズを もらった！"),
                    EventStep.Say("十字キーの 右上の 「走る」を 押すと 走れるわ。パソコンなら Shift キーよ"),
                    EventStep.Say("体に 気を つけてね。いってらっしゃい！")));
        }

        /// <summary>停電の夜を照らしてくれた子を託す。はい で1回だけもらえる（手持ちがいっぱいなら あずかり箱へ送られる）</summary>
        private static FieldNpc KanteraGiver()
        {
            return new FieldNpc("yukiokoshiKanteraGiver", NpcLook.Villager,
                NpcPlacement.Until(Town03Flags.GotKantera, KanteraGiverSpot,
                    EventStep.Say("停電の 夜は、この 子の 灯りが 頼りでねえ"),
                    EventStep.Say("でも この 子は 広い 世界を 見たがって いるのさ"),
                    EventStep.Ask("この 子を 旅に 連れて いって くれるかい？",
                        EventStep.Say("そうかい…… 気が 変わったら また おいで")),
                    EventStep.Give(Town03Species.Kantera, KanteraLevel),
                    EventStep.SetFlag(Town03Flags.GotKantera),
                    EventStep.Say("寒い 夜は その 子の 灯りで 温まって おくれ")),
                NpcPlacement.Always(KanteraGiverSpot,
                    EventStep.Say("カンテラペンは 元気に 灯って いるかい？")));
        }

        /// <summary>停電の間と、電気が戻った後でセリフを変える（発電所の出来事を済ませたと分かるように）</summary>
        private static FieldNpc TownGuide()
        {
            var spot = new FieldSpot(Town03Maps.Town, new GridPos(10, 13), Direction.Right);
            return new FieldNpc("yukiokoshiGuide", NpcLook.Villager,
                NpcPlacement.Until(Town03Flags.PowerRestored, spot,
                    EventStep.Say("ユキオコシ町へ ようこそ！"),
                    EventStep.Say("……と 言いたいけど、今は 町じゅう 停電中 なんだ")),
                NpcPlacement.Always(spot,
                    EventStep.Say("電気が 戻って、町に 明かりが ついたよ！"),
                    EventStep.Say("発電所の 子を 片付けて くれたのは 君かい？ ありがとう！")));
        }

        private static FieldNpc GymHintResident()
        {
            return new FieldNpc("yukiokoshiGymHint", NpcLook.Mom,
                NpcPlacement.Always(new FieldSpot(Town03Maps.Town, new GridPos(8, 8), Direction.Down),
                    EventStep.Say("道場主の ライゾウさんは 雷の ペンモンの 使い手よ"),
                    EventStep.Say("岩の 子なら 雷の 技を 受けても へっちゃら なんですって")));
        }

        private static FieldNpc ThunderKid()
        {
            return new FieldNpc("yukiokoshiThunderKid", NpcLook.Youngster,
                NpcPlacement.Always(new FieldSpot(Town03Maps.Town, new GridPos(22, 9), Direction.Left),
                    EventStep.Say("冬に 鳴る 雷を 「雪起こし」って 言うんだ"),
                    EventStep.Say("雪起こしが 鳴ると、もうすぐ 大雪が 来るんだって！")));
        }
    }
}
