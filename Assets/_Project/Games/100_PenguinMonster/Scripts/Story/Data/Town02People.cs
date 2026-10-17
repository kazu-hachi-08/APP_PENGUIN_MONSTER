using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り2の人と落とし物（PLAN_STORY_TOWN02.md フェーズ3）。
    /// 2番道路の出口は ガンテツに勝つまで、カガミコ町の道場の扉は 4番道路の奥の出来事（Town02Flags.Route04Cleared）まで人がふさぐ。
    /// トレーナー・町の人のセリフ・No.209 をくれる人はフェーズ4で足す
    /// </summary>
    public static class Town02People
    {
        private const int FoundEggs = 3;
        private const int FoundPotions = 2;

        /// <summary>2番道路の出口の手前。ガンテツに勝つまで雪かきの町の人がここに立つ</summary>
        public static readonly FieldSpot SnowShovelerAtRoute02Exit =
            Town01Maps.StepFrom(Town01Maps.Route02, Town01Maps.Route02East, Direction.Left);
        /// <summary>道場の扉の前。4番道路の奥の出来事まで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtGymDoor = Town01Maps.StepFrom(Town02Maps.Town, Town02Maps.GymDoor, Direction.Down);
        /// <summary>4番道路の奥の入口。フェーズ4でユキトの3戦目にする</summary>
        public static readonly FieldSpot Route04EndSpot = new FieldSpot(Town02Maps.Route04, Town02Maps.Route04InnerEntrance, Direction.Left);

        public static IReadOnlyList<FieldNpc> Create()
        {
            return new[]
            {
                SnowShoveler(), GymDoorDisciple(), Route04EndPlaceholder(),
                Town01People.HutKeeper("kagamikoHutKeeper", Town02Maps.WarmHut),
                Town01People.Clerk("kagamikoClerk", Town02Maps.Shop),
                Town01People.DroppedItem(Town02Flags.ItemPrefix, "route03Potion",
                    new FieldSpot(Town02Maps.Route03, new GridPos(1, 30), Direction.Down), StoryItem.Potion, FoundPotions),
                Town01People.DroppedItem(Town02Flags.ItemPrefix, "route04Eggs",
                    new FieldSpot(Town02Maps.Route04, new GridPos(1, 1), Direction.Down), StoryItem.CaptureEgg, FoundEggs),
                Town01People.DroppedItem(Town02Flags.ItemPrefix, "route04ParalyzeHeal",
                    new FieldSpot(Town02Maps.Route04, new GridPos(28, 17), Direction.Down), StoryItem.ParalyzeHeal, 1),
            };
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

        /// <summary>
        /// 仮の人（フェーズ4でユキトの3戦目に作り直す）。話しかけると道場主が戻ったと教えて、門下生をどかせる。
        /// フェーズ3でも道場の前まで通して遊べるようにするため
        /// </summary>
        private static FieldNpc Route04EndPlaceholder()
        {
            return new FieldNpc("rival3", NpcLook.Rival,
                NpcPlacement.Until(Town02Flags.Route04Cleared, Route04EndSpot,
                    EventStep.Say("よう！ この奥で 修行してたんだ"),
                    EventStep.Say("そうだ、道場主の サエさんが 町に 戻ったってさ"),
                    EventStep.SetFlag(Town02Flags.Route04Cleared),
                    EventStep.Say("先に 道場へ 行って こいよ！ じゃあな！")));
        }
    }
}
