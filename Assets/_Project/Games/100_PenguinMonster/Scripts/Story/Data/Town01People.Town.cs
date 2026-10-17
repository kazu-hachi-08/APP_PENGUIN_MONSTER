using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ツララ町・2番道路の人と、道路の落とし物（PLAN_STORY_TOWN01.md フェーズ8b）。
    /// 道場の前の門下生は、2番道路の奥の出来事（Town01Flags.Route02Cleared）まで扉をふさぐ。奥の出来事は 8c で本物にするまで仮の人が立てる
    /// </summary>
    public static partial class Town01People
    {
        private const int FoundEggs = 3;

        /// <summary>道場の扉の前。2番道路の奥の出来事まで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtGymDoor = new FieldSpot(Town01Maps.Town, Town01Maps.GymDoor + Direction.Down.Offset(), Direction.Down);

        private static IEnumerable<FieldNpc> TownAndRoutes()
        {
            return new[]
            {
                DroppedItem("route01Potion", new FieldSpot(Town01Maps.Route01, new GridPos(14, 5), Direction.Down), StoryItem.Potion, 1),
                DroppedItem("route02Eggs", new FieldSpot(Town01Maps.Route02, new GridPos(4, 1), Direction.Down), StoryItem.CaptureEgg, FoundEggs),
                DroppedItem("route02Awakening", new FieldSpot(Town01Maps.Route02, new GridPos(28, 4), Direction.Down), StoryItem.Awakening, 1),
                GymDisciple(), Route02Woodcutter(), GymPlaceholder(), TownGuide(), PondKid(), TownHouseResident(),
            };
        }

        /// <summary>道路の落とし物。人と同じく前から A で調べると拾い、拾ったフラグで消える</summary>
        private static FieldNpc DroppedItem(string id, FieldSpot spot, StoryItem item, int count)
        {
            string flag = Town01Flags.ItemPrefix + id;
            return new FieldNpc("item." + id, NpcLook.Item,
                NpcPlacement.Until(flag, spot,
                    EventStep.FindItem(item, count),
                    EventStep.SetFlag(flag)));
        }

        private static FieldNpc GymDisciple()
        {
            return new FieldNpc("gymDisciple", NpcLook.Disciple,
                NpcPlacement.Until(Town01Flags.Route02Cleared, DiscipleAtGymDoor,
                    EventStep.Say("押忍！ 道場主は いま 留守に している！"),
                    EventStep.Say("東の 2番道路の 奥へ 出かけた ままだ")),
                NpcPlacement.Always(new FieldSpot(Town01Maps.Town, new GridPos(6, 6), Direction.Right),
                    EventStep.Say("押忍！ 道場主が 中で お待ちだ！")));
        }

        /// <summary>2番道路の奥の仮の人。話すと道場の前の門下生がどく（8c で本物の出来事に作り直す）</summary>
        private static FieldNpc Route02Woodcutter()
        {
            var spot = new FieldSpot(Town01Maps.Route02, new GridPos(27, 21), Direction.Left);
            return new FieldNpc("route02Woodcutter", NpcLook.Villager,
                NpcPlacement.Until(Town01Flags.Route02Cleared, spot,
                    EventStep.Say("道場主かい？ さっき 町へ 帰って いったよ"),
                    EventStep.SetFlag(Town01Flags.Route02Cleared)),
                NpcPlacement.Always(spot,
                    EventStep.Say("この先は 雪が 深くて 通れないんだ")));
        }

        /// <summary>道場の中の仮の人（門下生・道場主は 8c）</summary>
        private static FieldNpc GymPlaceholder()
        {
            return new FieldNpc("gymPlaceholder", NpcLook.Disciple,
                NpcPlacement.Always(new FieldSpot(Town01Maps.Gym, new GridPos(5, 2), Direction.Down),
                    EventStep.Say("押忍！ 道場は 稽古の 準備中だ！")));
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
