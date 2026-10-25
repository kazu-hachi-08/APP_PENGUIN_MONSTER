using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// カザハナ町の人・隠しのヒントのおじいさん・釣り竿をくれる釣り人・自転車屋の主人・7番道路と大橋のトレーナー・落とし物
    /// </summary>
    public static partial class Town04People
    {
        private const int FoundUltraEggs = 2;

        /// <summary>7番道路の池のほとり。釣り人が釣り竿をくれる</summary>
        public static readonly FieldSpot AnglerSpot = new FieldSpot(Town04Maps.Route07, new GridPos(5, 13), Direction.Down);
        /// <summary>自転車屋の主人。小屋・店の受付と同じく奥の真ん中に立つ</summary>
        public static readonly FieldSpot BikeShopOwnerSpot = new FieldSpot(Town04Maps.BikeShop, new GridPos(3, 1), Direction.Down);
        /// <summary>カザハナ町の家のおじいさん（隠しコマンドのヒント）。主人公はその1つ下で話す</summary>
        public static readonly FieldSpot HintElderSpot = new FieldSpot(Town04Maps.TownHouse, new GridPos(2, 2), Direction.Down);
        /// <summary>おやつのイカ をくれる町の人</summary>
        public static readonly FieldSpot SquidSnackGiverSpot = new FieldSpot(Town04Maps.Town, new GridPos(9, 9), Direction.Down);

        private static IEnumerable<FieldNpc> TownAndRoutes()
        {
            return new[]
            {
                Angler(), BikeShopOwner(), HintElder(), SquidSnackGiver(), TownGuide(), GymHintResident(), BikeKid(),
                Town01People.DroppedItem(Town04Flags.ItemPrefix, "route07UltraEggs",
                    new FieldSpot(Town04Maps.Route07, new GridPos(20, 2), Direction.Down), StoryItem.UltraEgg, FoundUltraEggs),
                Town01People.DroppedItem(Town04Flags.ItemPrefix, "route07DriedFish",
                    new FieldSpot(Town04Maps.Route07, new GridPos(20, 28), Direction.Down), StoryItem.HeldDriedFish, 1),
                Town01People.DroppedItem(Town04Flags.ItemPrefix, "bridgeBraveFeather",
                    new FieldSpot(Town04Maps.Bridge, new GridPos(15, 1), Direction.Down), StoryItem.HeldBraveFeather, 1),
                Town01People.RouteTrainer("route07AnglerTrainer", NpcLook.Villager, Town04Trainers.Route07Angler, Town04Maps.Route07,
                    new GridPos(9, 9), Direction.Right, "池の ぬしを 狙って いるんだ。邪魔は させないぞ！", "池の ぬしは 大橋の 下に 移ったって 噂も あるな"),
                Town01People.RouteTrainer("route07Hiker", NpcLook.Villager, Town04Trainers.Route07Hiker, Town04Maps.Route07,
                    new GridPos(13, 20), Direction.Left, "岩場で 鍛えた 子は 風にも 負けないぞ！", "鳥の 子には 岩や 氷の 技が よく 効くんだ"),
                Town01People.RouteTrainer("route07KiteKid", NpcLook.Youngster, Town04Trainers.Route07KiteKid, Town04Maps.Route07,
                    new GridPos(7, 27), Direction.Right, "今日は 凧あげ 日和だ！ 勝負しよう！", "風に 乗った 子たちが 負けちゃった……"),
                Town01People.RouteTrainer("bridgeCyclist1", NpcLook.Youngster, Town04Trainers.BridgeCyclist1, Town04Maps.Bridge,
                    new GridPos(18, 4), Direction.Down, "ひと休み していたら 君が 来た！ 勝負だ！", "休憩所の 草むらには 海鳥の 子が 多いよ"),
                Town01People.RouteTrainer("bridgeCyclist2", NpcLook.Youngster, Town04Trainers.BridgeCyclist2, Town04Maps.Bridge,
                    new GridPos(23, 7), Direction.Up, "風花大橋を 一番 速く 走るのは おれだ！", "速さだけじゃ 勝てないのか……"),
                Town01People.RouteTrainer("bridgeCyclist3", NpcLook.Villager, Town04Trainers.BridgeCyclist3, Town04Maps.Bridge,
                    new GridPos(29, 7), Direction.Up, "この 先の 下り坂は 最高だよ！ その前に 勝負！", "真ん中の 展望マスから の 眺めも 最高なんだ"),
            };
        }

        /// <summary>釣り竿と一緒に、さっぱり昆布（持たせる道具）もくれる。1回だけ</summary>
        private static FieldNpc Angler()
        {
            return new FieldNpc("route07Angler", NpcLook.Villager,
                NpcPlacement.Until(Town04Flags.FishingRod, AnglerSpot,
                    EventStep.Say("この 池は よく 釣れるんだ。君も やって みるかい？"),
                    EventStep.Say("予備の 竿を あげよう！"),
                    EventStep.SetFlag(Town04Flags.FishingRod),
                    EventStep.Say("釣り竿を もらった！"),
                    EventStep.Say("水に 向かって A で 釣れるよ"),
                    EventStep.Say("おまけに、釣れた 昆布も あげよう"),
                    EventStep.GiveItem(StoryItem.HeldFreshKelp, 1),
                    EventStep.Say("ペンモンに 持たせて おくと、まひや どくを 1回 治して くれるんだ")),
                NpcPlacement.Always(AnglerSpot,
                    EventStep.Say("大橋の 下でも 釣れるらしいよ"),
                    EventStep.Say("大きな ぬしが いるって 噂だ")));
        }

        /// <summary>ライゾウから話が通っている形にして、羽章3つの節目でもらえる理由にする。1回だけ</summary>
        private static FieldNpc BikeShopOwner()
        {
            return new FieldNpc("kazahanaBikeShopOwner", NpcLook.Villager,
                NpcPlacement.Until(Town04Flags.Bicycle, BikeShopOwnerSpot,
                    EventStep.Say("いらっしゃい！ ……おっ、君が うわさの トレーナーだね"),
                    EventStep.Say("ライゾウさんから 聞いてるよ！ 羽章を 3つ 持ってる 子が 来るって"),
                    EventStep.Say("風花大橋は 自転車 専用。これで 走って おいで！"),
                    EventStep.SetFlag(Town04Flags.Bicycle),
                    EventStep.Say("自転車を もらった！"),
                    EventStep.Say("十字キーの 左上の 「自転車」を 押すと 乗れるよ。パソコンなら C キーだ"),
                    EventStep.Say("建物に 入ると 自動で 降りるからね")),
                NpcPlacement.Always(BikeShopOwnerSpot,
                    EventStep.Say("自転車の 調子は どうだい？"),
                    EventStep.Say("大橋の 下り坂は 風に なった 気分に なれるよ！")));
        }

        /// <summary>隠しコマンド（上上下下左右左右）を、昔話の形でそれとなく教える</summary>
        private static FieldNpc HintElder()
        {
            return new FieldNpc("kazahanaHintElder", NpcLook.Villager,
                NpcPlacement.Always(HintElderSpot,
                    EventStep.Say("わしが 若い ころの 話じゃ"),
                    EventStep.Say("大橋の 真ん中で 自転車に またがって、空を 2度 海を 2度 見て……"),
                    EventStep.Say("左右を 2回 確かめたら、虹色の 鳥が 降りてきたんじゃ"),
                    EventStep.Say("……夢じゃったのかのう")));
        }

        /// <summary>おやつのイカ（持たせる道具）を1回だけくれる。持たせ方も教える</summary>
        private static FieldNpc SquidSnackGiver()
        {
            return new FieldNpc("kazahanaSquidSnackGiver", NpcLook.Mom,
                NpcPlacement.Until(Town04Flags.GotSquidSnack, SquidSnackGiverSpot,
                    EventStep.Say("あら、旅の 子ね。おやつを 作りすぎ ちゃったの"),
                    EventStep.GiveItem(StoryItem.HeldSquidSnack, 1),
                    EventStep.SetFlag(Town04Flags.GotSquidSnack),
                    EventStep.Say("手持ちの 画面で ペンモンに 持たせて あげてね"),
                    EventStep.Say("HPが 半分に なると 食べて 元気に なるのよ")),
                NpcPlacement.Always(SquidSnackGiverSpot,
                    EventStep.Say("おやつの イカは 気に入って もらえた？")));
        }

        private static FieldNpc TownGuide()
        {
            return new FieldNpc("kazahanaGuide", NpcLook.Villager,
                NpcPlacement.Always(new FieldSpot(Town04Maps.Town, new GridPos(4, 18), Direction.Right),
                    EventStep.Say("カザハナ町へ ようこそ！"),
                    EventStep.Say("晴れた 日に 舞う 雪を 「風花」って 言うんだ"),
                    EventStep.Say("山から 吹く 風が、遠くの 雪を 運んで くるのさ")));
        }

        private static FieldNpc GymHintResident()
        {
            return new FieldNpc("kazahanaGymHint", NpcLook.Mom,
                NpcPlacement.Always(new FieldSpot(Town04Maps.Town, new GridPos(8, 6), Direction.Down),
                    EventStep.Say("道場主の ハヤテさんは 鳥の ペンモンの 使い手よ"),
                    EventStep.Say("鳥の 子は 氷・雷・岩の 技が 苦手 なんですって")));
        }

        private static FieldNpc BikeKid()
        {
            return new FieldNpc("kazahanaBikeKid", NpcLook.Youngster,
                NpcPlacement.Until(Town04Flags.Bicycle, new FieldSpot(Town04Maps.Town, new GridPos(22, 9), Direction.Left),
                    EventStep.Say("北の 自転車屋さんの 自転車、かっこいいんだ！"),
                    EventStep.Say("ぼくも 大きく なったら 風花大橋を 走るんだ")),
                NpcPlacement.Always(new FieldSpot(Town04Maps.Town, new GridPos(22, 9), Direction.Left),
                    EventStep.Say("あっ！ 自転車 もらったんだ！ いいなあ！")));
        }
    }
}
