using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り2の人と落とし物（PLAN_STORY_TOWN02.md）。
    /// 2番道路の出口は ガンテツに勝つまで、カガミコ町の道場の扉は 4番道路の奥の出来事（Town02Flags.Route04Cleared）まで人がふさぐ。
    /// 町の人・道路のトレーナー・No.209 をくれる人は .Town.cs、道場とユキトの3戦目は .Gym.cs
    /// </summary>
    public static partial class Town02People
    {
        private const int FoundEggs = 3;
        private const int FoundPotions = 2;

        /// <summary>2番道路の出口の手前。ガンテツに勝つまで雪かきの町の人がここに立つ</summary>
        public static readonly FieldSpot SnowShovelerAtRoute02Exit =
            Town01Maps.StepFrom(Town01Maps.Route02, Town01Maps.Route02East, Direction.Left);
        /// <summary>道場の扉の前。4番道路の奥の出来事まで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtGymDoor = Town01Maps.StepFrom(Town02Maps.Town, Town02Maps.GymDoor, Direction.Down);
        /// <summary>4番道路の奥の入口。ここから左を見張るユキト（3戦目）と、奥へ入ろうとする主人公の目が合う</summary>
        public static readonly FieldSpot Route04EndSpot = new FieldSpot(Town02Maps.Route04, Town02Maps.Route04InnerEntrance, Direction.Left);

        public static IReadOnlyList<FieldNpc> Create()
        {
            var people = new List<FieldNpc>
            {
                SnowShoveler(), GymDoorDisciple(),
                Town01People.HutKeeper("kagamikoHutKeeper", Town02Maps.WarmHut),
                Town01People.Clerk("kagamikoClerk", Town02Maps.Shop),
                Town01People.DroppedItem(Town02Flags.ItemPrefix, "route03Potion",
                    new FieldSpot(Town02Maps.Route03, new GridPos(1, 30), Direction.Down), StoryItem.Potion, FoundPotions),
                Town01People.DroppedItem(Town02Flags.ItemPrefix, "route04Eggs",
                    new FieldSpot(Town02Maps.Route04, new GridPos(1, 1), Direction.Down), StoryItem.CaptureEgg, FoundEggs),
                Town01People.DroppedItem(Town02Flags.ItemPrefix, "route04ParalyzeHeal",
                    new FieldSpot(Town02Maps.Route04, new GridPos(28, 17), Direction.Down), StoryItem.ParalyzeHeal, 1),
            };
            people.AddRange(TownAndRoutes());
            people.AddRange(GymAndRival3());
            return people;
        }

        /// <summary>区切り1の道場に勝つ前に 3番道路へ進ませないため、雪かきを口実に出口をふさぐ。勝つと雪が片付いて居なくなる</summary>
        private static FieldNpc SnowShoveler()
        {
            return new FieldNpc("snowShoveler", NpcLook.Villager,
                NpcPlacement.Until(Town01Trainers.GymLeader.DefeatedFlag, SnowShovelerAtRoute02Exit,
                    EventStep.Say("この先は 3番道路 だけど、雪が 深くて 通れないよ"),
                    EventStep.Say("いま 雪かきを してるから、ツララ町の 道場にでも 行って おいで")));
        }

        private static FieldNpc GymDoorDisciple()
        {
            return new FieldNpc("kagamikoGymDoorDisciple", NpcLook.Disciple,
                NpcPlacement.Until(Town02Flags.Route04Cleared, DiscipleAtGymDoor,
                    EventStep.Say("押忍！ 道場主の サエさんは いま 留守だ！"),
                    EventStep.Say("東の 4番道路の 奥へ 出かけた ままだ")),
                NpcPlacement.Always(new FieldSpot(Town02Maps.Town, new GridPos(5, 6), Direction.Right),
                    EventStep.Say("押忍！ サエさんが 中で お待ちだ！")));
        }
    }
}
