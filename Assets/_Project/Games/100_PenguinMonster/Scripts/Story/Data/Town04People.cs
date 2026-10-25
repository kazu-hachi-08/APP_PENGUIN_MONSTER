using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ユキオコシ町の北の出口は ライゾウに勝つまで、大橋の関所は 自転車を持つまで、カザハナ町の道場の扉は ハヤテに会う（Town04Flags.MetHayate）まで人がふさぐ。
    /// 釣り竿をくれる釣り人・自転車屋の主人・展望台のハヤテは仮の人（PLAN_STORY_TOWN04.md フェーズ6）。フェーズ7で作り直す
    /// </summary>
    public static class Town04People
    {
        private const int FoundUltraEggs = 2;

        /// <summary>ユキオコシ町の北の出口の手前。ライゾウに勝つまで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtYukiokoshiNorth =
            Town01Maps.StepFrom(Town03Maps.Town, Town03Maps.Route07Exit, Direction.Down);
        /// <summary>関所の東の扉の手前。自転車を持つまで番の人がここに立つ</summary>
        public static readonly FieldSpot GuardAtGateEast = Town01Maps.StepFrom(Town04Maps.Gate, Town04Maps.GateEast, Direction.Left);
        /// <summary>カザハナ町の道場の扉の前。ハヤテに会うまで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtGymDoor = Town01Maps.StepFrom(Town04Maps.Town, Town04Maps.GymDoor, Direction.Down);
        /// <summary>7番道路の池のほとり。釣り人が釣り竿をくれる</summary>
        public static readonly FieldSpot AnglerSpot = new FieldSpot(Town04Maps.Route07, new GridPos(5, 13), Direction.Down);
        /// <summary>自転車屋の主人。小屋・店の受付と同じく奥の真ん中に立つ</summary>
        public static readonly FieldSpot BikeShopOwnerSpot = new FieldSpot(Town04Maps.BikeShop, new GridPos(3, 1), Direction.Down);
        /// <summary>大橋の東の端の展望台。ハヤテが風を読んでいる</summary>
        public static readonly FieldSpot HayateAtLookoutDeck = new FieldSpot(Town04Maps.Bridge, new GridPos(38, 5), Direction.Left);

        public static IReadOnlyList<FieldNpc> Create()
        {
            return new[]
            {
                YukiokoshiNorthDisciple(), GateGuard(), GymDoorDisciple(), Angler(), BikeShopOwner(), Hayate(),
                Town01People.HutKeeper("kazahanaHutKeeper", Town04Maps.WarmHut),
                Town01People.Clerk("kazahanaClerk", Town04Maps.Shop, Town03People.ShopItems),
                Town01People.DroppedItem(Town04Flags.ItemPrefix, "route07UltraEggs",
                    new FieldSpot(Town04Maps.Route07, new GridPos(20, 2), Direction.Down), StoryItem.UltraEgg, FoundUltraEggs),
            };
        }

        /// <summary>ユキオコシ町の道場に勝つ前に 7番道路へ進ませないため、道場の決まりを口実に出口をふさぐ。勝つと居なくなる</summary>
        private static FieldNpc YukiokoshiNorthDisciple()
        {
            return new FieldNpc("yukiokoshiNorthDisciple", NpcLook.Disciple,
                NpcPlacement.Until(Town03Trainers.GymLeader.DefeatedFlag, DiscipleAtYukiokoshiNorth,
                    EventStep.Say("押忍！ この 先は 7番道路だ"),
                    EventStep.Say("ライゾウさんに 認められた 者しか 通さない 決まりだ！")));
        }

        /// <summary>大橋は自転車でないと通れない。自転車を持つと扉の前からどいて、横で見送る</summary>
        private static FieldNpc GateGuard()
        {
            return new FieldNpc("kazahanaGateGuard", NpcLook.Villager,
                NpcPlacement.Until(Town04Flags.Bicycle, GuardAtGateEast,
                    EventStep.Say("この 先は 8番道路 風花大橋"),
                    EventStep.Say("長い 下り坂が あるから、自転車 専用だよ")),
                NpcPlacement.Always(new FieldSpot(Town04Maps.Gate, GuardAtGateEast.Position + Direction.Up.Offset(), Direction.Down),
                    EventStep.Say("風花大橋は 自転車で 走ると 気持ちいいよ！")));
        }

        /// <summary>道場主が留守の間は道場に入れないので、扉の前で大橋の先へ向かわせる。ハヤテに会うと居なくなる</summary>
        private static FieldNpc GymDoorDisciple()
        {
            return new FieldNpc("kazahanaGymDoorDisciple", NpcLook.Disciple,
                NpcPlacement.Until(Town04Flags.MetHayate, DiscipleAtGymDoor,
                    EventStep.Say("押忍！ ハヤテさんは 今 留守だ"),
                    EventStep.Say("大橋の 先の 展望台で 風を 読んでる はずだ")));
        }

        /// <summary>仮の人。フェーズ7で さっぱり昆布も一緒にくれる形に作り直す</summary>
        private static FieldNpc Angler()
        {
            return new FieldNpc("route07Angler", NpcLook.Villager,
                NpcPlacement.Until(Town04Flags.FishingRod, AnglerSpot,
                    EventStep.Say("この 池は よく 釣れるんだ。君も やって みるかい？"),
                    EventStep.SetFlag(Town04Flags.FishingRod),
                    EventStep.Say("釣り竿を もらった！"),
                    EventStep.Say("水に 向かって A で 釣れるよ")),
                NpcPlacement.Always(AnglerSpot,
                    EventStep.Say("大橋の 下でも 釣れるらしいよ")));
        }

        /// <summary>仮の人。フェーズ7でセリフを作り直す</summary>
        private static FieldNpc BikeShopOwner()
        {
            return new FieldNpc("kazahanaBikeShopOwner", NpcLook.Villager,
                NpcPlacement.Until(Town04Flags.Bicycle, BikeShopOwnerSpot,
                    EventStep.Say("ライゾウさんから 聞いてるよ！ 羽章を 3つ 持ってる 子が 来るって"),
                    EventStep.SetFlag(Town04Flags.Bicycle),
                    EventStep.Say("自転車を もらった！"),
                    EventStep.Say("十字キーの 左上の 「自転車」を 押すと 乗れるよ。パソコンなら C キーだ")),
                NpcPlacement.Always(BikeShopOwnerSpot,
                    EventStep.Say("自転車の 調子は どうだい？")));
        }

        /// <summary>仮の人。会うと道場が開く。フェーズ7で道場へ戻る形に作り直す</summary>
        private static FieldNpc Hayate()
        {
            return new FieldNpc("hayate", NpcLook.GymLeader,
                NpcPlacement.Until(Town04Flags.MetHayate, HayateAtLookoutDeck,
                    EventStep.Say("いい 風だったでしょ？"),
                    EventStep.Say("あたしは ハヤテ。カザハナ町 道場の 主よ"),
                    EventStep.SetFlag(Town04Flags.MetHayate),
                    EventStep.Say("道場で 待ってる！")));
        }
    }
}
