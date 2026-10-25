using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// カガミコ町の北の出口は サエに勝つまで、ユキオコシ町の道場の扉は 発電所の出来事（Town03Flags.PowerRestored）まで人がふさぐ。
    /// 町の人・母のシューズ・道路のトレーナー・No.238 をくれる人は .Town.cs、道場・ユキトの4戦目・発電所の No.239 は .Gym.cs
    /// </summary>
    public static partial class Town03People
    {
        private const int FoundPotions = 2;
        private const int FoundGreatEggs = 2;

        /// <summary>新しいタマゴ6種を並べた品ぞろえ。区切り1・2の店は基本の5種のまま、区切り4の店もこれと同じ</summary>
        internal static readonly StoryItem[] ShopItems =
        {
            StoryItem.CaptureEgg, StoryItem.GreatEgg, StoryItem.UltraEgg, StoryItem.NetEgg, StoryItem.NightEgg,
            StoryItem.HeavyEgg, StoryItem.SkyEgg, StoryItem.Potion, StoryItem.Antidote, StoryItem.ParalyzeHeal, StoryItem.Awakening,
        };

        /// <summary>カガミコ町の北の出口の手前。サエに勝つまで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtKagamikoNorth =
            Town01Maps.StepFrom(Town02Maps.Town, Town02Maps.Route05Exit, Direction.Down);

        public static IReadOnlyList<FieldNpc> Create()
        {
            var people = new List<FieldNpc>
            {
                KagamikoNorthDisciple(),
                Town01People.HutKeeper("yukiokoshiHutKeeper", Town03Maps.WarmHut),
                Town01People.Clerk("yukiokoshiClerk", Town03Maps.Shop, ShopItems),
                Town01People.DroppedItem(Town03Flags.ItemPrefix, "route05Potion",
                    new FieldSpot(Town03Maps.Route05, new GridPos(18, 13), Direction.Down), StoryItem.Potion, FoundPotions),
                Town01People.DroppedItem(Town03Flags.ItemPrefix, "route06GreatEggs",
                    new FieldSpot(Town03Maps.Route06, new GridPos(28, 1), Direction.Down), StoryItem.GreatEgg, FoundGreatEggs),
                Town01People.DroppedItem(Town03Flags.ItemPrefix, "route06Awakening",
                    new FieldSpot(Town03Maps.Route06, new GridPos(1, 18), Direction.Down), StoryItem.Awakening, 1),
            };
            people.AddRange(TownAndRoutes());
            people.AddRange(GymRival4AndPowerPlant());
            return people;
        }

        /// <summary>カガミコ町の道場に勝つ前に 5番道路へ進ませないため、道場の決まりを口実に出口をふさぐ。勝つと居なくなる</summary>
        private static FieldNpc KagamikoNorthDisciple()
        {
            return new FieldNpc("kagamikoNorthDisciple", NpcLook.Disciple,
                NpcPlacement.Until(Town02Trainers.GymLeader.DefeatedFlag, DiscipleAtKagamikoNorth,
                    EventStep.Say("押忍！ この 先は 5番道路だ"),
                    EventStep.Say("サエさんに 認められた 者しか 通さない 決まりだ！")));
        }
    }
}
