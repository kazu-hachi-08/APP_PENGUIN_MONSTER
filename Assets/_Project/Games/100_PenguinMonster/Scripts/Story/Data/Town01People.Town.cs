using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ツララ町・道路の人とトレーナー、道路の落とし物。
    /// 道場の前の門下生は、2番道路の奥のユキトとの2戦目（Town01Flags.Route02Cleared）まで扉をふさぐ
    /// </summary>
    public static partial class Town01People
    {
        private const int FoundEggs = 3;

        /// <summary>道場の扉の前。2番道路の奥の出来事まで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtGymDoor = Town01Maps.StepFrom(Town01Maps.Town, Town01Maps.GymDoor, Direction.Down);

        private static IEnumerable<FieldNpc> TownAndRoutes()
        {
            return new[]
            {
                DroppedItem(Town01Flags.ItemPrefix, "route01Potion", new FieldSpot(Town01Maps.Route01, new GridPos(14, 5), Direction.Down), StoryItem.Potion, 1),
                DroppedItem(Town01Flags.ItemPrefix, "route02Eggs", new FieldSpot(Town01Maps.Route02, new GridPos(4, 1), Direction.Down), StoryItem.CaptureEgg, FoundEggs),
                DroppedItem(Town01Flags.ItemPrefix, "route02Awakening", new FieldSpot(Town01Maps.Route02, new GridPos(28, 4), Direction.Down), StoryItem.Awakening, 1),
                GymDoorDisciple(), TownGuide(), PondKid(), TownHouseResident(),
                RouteTrainer("route01Kid", NpcLook.Youngster, Town01Trainers.Route01Kid, Town01Maps.Route01, new GridPos(9, 11), Direction.Left,
                    "目が 合ったら 勝負だ！", "草むらで 戦って 鍛え直しだ！"),
                RouteTrainer("route01Angler", NpcLook.Youngster, Town01Trainers.Route01Angler, Town01Maps.Route01, new GridPos(6, 16), Direction.Right,
                    "川で 釣った ペンモンを 見せて やる！", "川べりの 深い 草むらで 釣ったんだ"),
                RouteTrainer("route02Snowball", NpcLook.Youngster, Town01Trainers.Route02SnowballKid, Town01Maps.Route02, new GridPos(12, 4), Direction.Down,
                    "雪玉 くらえー！ 勝負だ！", "雪合戦なら 負けないのに……"),
                RouteTrainer("route02Hiker", NpcLook.Villager, Town01Trainers.Route02Hiker, Town01Maps.Route02, new GridPos(16, 11), Direction.Left,
                    "山で 鍛えた 岩の 守り、崩せるかな？", "道場の ガンテツさんは もっと 固いぞ"),
                RouteTrainer("route02Forest", NpcLook.Youngster, Town01Trainers.Route02ForestKid, Town01Maps.Route02, new GridPos(28, 11), Direction.Up,
                    "林の 奥で 見つけた 不思議な 子だよ", "暗い 林には まだ 見たことの ない 子が いるかも"),
            };
        }

        /// <summary>道路の落とし物。人と同じく前から A で調べると拾い、拾ったフラグ（区切りごとの頭 ＋ id）で消える</summary>
        internal static FieldNpc DroppedItem(string flagPrefix, string id, FieldSpot spot, StoryItem item, int count)
        {
            string flag = flagPrefix + id;
            return new FieldNpc("item." + id, NpcLook.Item,
                NpcPlacement.Until(flag, spot,
                    EventStep.FindItem(item, count),
                    EventStep.SetFlag(flag)));
        }

        /// <summary>道路・道場のトレーナー。向いている先を見張り、目が合ったら勝負。勝った後は別のセリフ</summary>
        private static FieldNpc RouteTrainer(string id, NpcLook look, TrainerDefinition trainer, string mapId, GridPos pos, Direction facing,
            string challenge, string afterwards)
        {
            var spot = new FieldSpot(mapId, pos, facing);
            return new FieldNpc(id, look,
                NpcPlacement.Until(trainer.DefeatedFlag, spot,
                    EventStep.Say(challenge),
                    EventStep.Battle(trainer)),
                NpcPlacement.Always(spot,
                    EventStep.Say(afterwards)));
        }

        private static FieldNpc GymDoorDisciple()
        {
            return new FieldNpc("gymDoorDisciple", NpcLook.Disciple,
                NpcPlacement.Until(Town01Flags.Route02Cleared, DiscipleAtGymDoor,
                    EventStep.Say("押忍！ 道場主の ガンテツさんは いま 留守だ！"),
                    EventStep.Say("東の 2番道路の 奥へ 修行に 出かけた ままだ")),
                NpcPlacement.Always(new FieldSpot(Town01Maps.Town, new GridPos(6, 6), Direction.Right),
                    EventStep.Say("押忍！ ガンテツさんが 中で お待ちだ！")));
        }

        private static FieldNpc TownGuide()
        {
            return new FieldNpc("townGuide", NpcLook.Villager,
                NpcPlacement.Always(new FieldSpot(Town01Maps.Town, new GridPos(9, 17), Direction.Down),
                    EventStep.Say("ツララ町へ ようこそ！"),
                    EventStep.Say("ペンモンが 疲れたら ぬくもり小屋で 休ませると いい")));
        }

        private static FieldNpc PondKid()
        {
            return new FieldNpc("pondKid", NpcLook.Youngster,
                NpcPlacement.Always(new FieldSpot(Town01Maps.Town, new GridPos(19, 14), Direction.Up),
                    EventStep.Say("2番道路の 林の 奥は 昼でも 暗いんだ"),
                    EventStep.Say("深い 草むらには 見たことの ない ペンモンが いるって！")));
        }

        private static FieldNpc TownHouseResident()
        {
            return new FieldNpc("townHouseResident", NpcLook.Mom,
                NpcPlacement.Always(new FieldSpot(Town01Maps.TownHouse, new GridPos(2, 2), Direction.Down),
                    EventStep.Say("道場主は 岩の ペンモンの 使い手よ"),
                    EventStep.Say("海や 苔の 技が よく 効くわ")));
        }
    }
}
