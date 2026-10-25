using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ユキオコシ町の北の出口は ライゾウに勝つまで、大橋の関所は 自転車を持つまで、カザハナ町の道場の扉は ハヤテに会う（Town04Flags.MetHayate）まで人がふさぐ。
    /// 町の人・釣り竿・自転車・道路と大橋のトレーナー・落とし物は .Town.cs、ユキトの5戦目・ハヤテ・道場・虹の羽の子は .Gym.cs
    /// </summary>
    public static partial class Town04People
    {
        /// <summary>ユキオコシ町の北の出口の手前。ライゾウに勝つまで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtYukiokoshiNorth =
            Town01Maps.StepFrom(Town03Maps.Town, Town03Maps.Route07Exit, Direction.Down);
        /// <summary>関所の東の扉の手前。自転車を持つまで番の人がここに立つ</summary>
        public static readonly FieldSpot GuardAtGateEast = Town01Maps.StepFrom(Town04Maps.Gate, Town04Maps.GateEast, Direction.Left);
        /// <summary>カザハナ町の道場の扉の前。ハヤテに会うまで門下生がここに立つ</summary>
        public static readonly FieldSpot DiscipleAtGymDoor = Town01Maps.StepFrom(Town04Maps.Town, Town04Maps.GymDoor, Direction.Down);

        public static IReadOnlyList<FieldNpc> Create()
        {
            var people = new List<FieldNpc>
            {
                YukiokoshiNorthDisciple(), GateGuard(), GymDoorDisciple(),
                Town01People.HutKeeper("kazahanaHutKeeper", Town04Maps.WarmHut),
                Town01People.Clerk("kazahanaClerk", Town04Maps.Shop, Town03People.ShopItems),
            };
            people.AddRange(TownAndRoutes());
            people.AddRange(GymRival5AndHayate());
            return people;
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
                    EventStep.Say("長い 下り坂が あるから、自転車 専用だよ"),
                    EventStep.Say("町の 自転車屋さんに 寄って みると いい")),
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
    }
}
