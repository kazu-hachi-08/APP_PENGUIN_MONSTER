using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り3の人と落とし物（PLAN_STORY_TOWN03.md フェーズ4）。
    /// カガミコ町の北の出口は サエに勝つまで、ユキオコシ町の道場の扉は 発電所の出来事（Town03Flags.PowerRestored）まで人がふさぐ。
    /// トレーナー・町の人のセリフ・母のシューズ・No.238 をくれる人はフェーズ5で足す
    /// </summary>
    public static class Town03People
    {
        private const int FoundPotions = 2;
        private const int FoundGreatEggs = 2;

        /// <summary>カガミコ町の北の出口の手前。サエに勝つまで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtKagamikoNorth =
            Town01Maps.StepFrom(Town02Maps.Town, Town02Maps.Route05Exit, Direction.Down);
        /// <summary>道場の扉の前。電気が戻るまで道場主ライゾウがここに立つ</summary>
        public static readonly FieldSpot LeaderAtGymDoor = Town01Maps.StepFrom(Town03Maps.Town, Town03Maps.GymDoor, Direction.Down);
        /// <summary>発電所の奥。フェーズ5で No.239（居座る野生の子）にする。主人公はその1つ下で話す</summary>
        public static readonly FieldSpot PowerPlantCoreSpot = new FieldSpot(Town03Maps.PowerPlant, new GridPos(5, 1), Direction.Down);

        public static IReadOnlyList<FieldNpc> Create()
        {
            return new[]
            {
                KagamikoNorthDisciple(), GymDoorLeader(), PowerPlantPlaceholder(),
                Town01People.HutKeeper("yukiokoshiHutKeeper", Town03Maps.WarmHut),
                // 区切り3から新しいタマゴ6種が並ぶ（PLAN_STORY_TOWN03.md §3.1）。区切り1・2の店は今のまま
                Town01People.Clerk("yukiokoshiClerk", Town03Maps.Shop,
                    StoryItem.CaptureEgg, StoryItem.GreatEgg, StoryItem.UltraEgg, StoryItem.NetEgg, StoryItem.NightEgg,
                    StoryItem.HeavyEgg, StoryItem.SkyEgg, StoryItem.Potion, StoryItem.Antidote, StoryItem.ParalyzeHeal, StoryItem.Awakening),
                Town01People.DroppedItem(Town03Flags.ItemPrefix, "route05Potion",
                    new FieldSpot(Town03Maps.Route05, new GridPos(18, 13), Direction.Down), StoryItem.Potion, FoundPotions),
                Town01People.DroppedItem(Town03Flags.ItemPrefix, "route06GreatEggs",
                    new FieldSpot(Town03Maps.Route06, new GridPos(28, 1), Direction.Down), StoryItem.GreatEgg, FoundGreatEggs),
                Town01People.DroppedItem(Town03Flags.ItemPrefix, "route06Awakening",
                    new FieldSpot(Town03Maps.Route06, new GridPos(1, 18), Direction.Down), StoryItem.Awakening, 1),
            };
        }

        /// <summary>区切り2の道場に勝つ前に 5番道路へ進ませないため、道場の決まりを口実に出口をふさぐ。勝つと居なくなる</summary>
        private static FieldNpc KagamikoNorthDisciple()
        {
            return new FieldNpc("kagamikoNorthDisciple", NpcLook.Disciple,
                NpcPlacement.Until(Town02Trainers.GymLeader.DefeatedFlag, DiscipleAtKagamikoNorth,
                    EventStep.Say("押忍！ この 先は 5番道路だ"),
                    EventStep.Say("サエさんに 認められた 者しか 通さない 決まりだ！")));
        }

        /// <summary>停電の間は道場が開かないので、道場主が扉の前で発電所へ向かわせる。電気が戻ると道場の中へ戻る（中の台本はフェーズ5）</summary>
        private static FieldNpc GymDoorLeader()
        {
            return new FieldNpc("yukiokoshiGymDoorLeader", NpcLook.GymLeader,
                NpcPlacement.Until(Town03Flags.PowerRestored, LeaderAtGymDoor,
                    EventStep.Say("ガハハ！ わしが ユキオコシ町 道場の 主、ライゾウだ！"),
                    EventStep.Say("……と 言いたい ところだが、停電で 道場が 開けられん"),
                    EventStep.Say("東の 6番道路の 奥の 発電所に、何かが 居座って いるらしい")));
        }

        /// <summary>
        /// 仮の人（フェーズ5で No.239 との戦いに作り直す）。話しかけると電気が戻り、道場主が扉の前からどく。
        /// フェーズ4でも道場の中まで通して遊べるようにするため
        /// </summary>
        private static FieldNpc PowerPlantPlaceholder()
        {
            return new FieldNpc("powerPlantCore", NpcLook.Villager,
                NpcPlacement.Until(Town03Flags.PowerRestored, PowerPlantCoreSpot,
                    EventStep.Say("発電所の 見回りに 来たんだ。機械に 居座って いた 子は どこかへ 行った みたいだ"),
                    EventStep.SetFlag(Town03Flags.PowerRestored),
                    EventStep.Say("電気が 戻った！")));
        }
    }
}
