using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 最初の章のトレーナー。ライバルのユキト2戦・道路の5人・道場の門下生2人と道場主ガンテツ。
    /// 道場主の最後の子は Lv14。道路の子は CPU よわい、道場は ふつう
    /// </summary>
    public static class Town01Trainers
    {
        public const string RivalName = "ユキト";
        public const string GymLeaderName = "ガンテツ";
        /// <summary>記録に残す羽章の名前（表示名は BadgeDisplayName）</summary>
        public const string BadgeId = "town01.badge";
        public const string BadgeDisplayName = "いわおこし羽章";

        private const string RivalId = "town01.rival1";
        private const string Rival2Id = "town01.rival2";
        private const int RivalPrize = 300;
        private const int Rival2Prize = 540;
        private const int RivalSight = 4;
        private const int RouteSight = 3;
        private const int GymSight = 3;
        private const int Rival2StarterLevel = 9;
        private const int Rival2SecondLevel = 8;

        /// <summary>ライバルに勝った（負けても話は進むので、戦い終えた）</summary>
        public static readonly string RivalBeaten = TrainerDefinition.DefeatedFlagOf(RivalId);
        public static readonly string Rival2Beaten = TrainerDefinition.DefeatedFlagOf(Rival2Id);

        /// <summary>最初のライバル戦（研究所）。こちらの最初の1体に強い子を、同じ Lv5 で出す</summary>
        public static TrainerDefinition Rival(StorySpecies playerStarter)
        {
            var party = new[] { new TrainerMon(Town01Starters.CounterOf(playerStarter), StoryBalance.StarterLevel) };
            return new TrainerDefinition(RivalId, RivalName, party, RivalPrize, RivalSight);
        }

        /// <summary>2戦目（2番道路の奥）。最初の子に強い子と、道路で捕まえたコガラ</summary>
        public static TrainerDefinition Rival2(StorySpecies playerStarter)
        {
            var party = new[]
            {
                new TrainerMon(Town01Species.Kogara, Rival2SecondLevel),
                new TrainerMon(Town01Starters.CounterOf(playerStarter), Rival2StarterLevel),
            };
            return new TrainerDefinition(Rival2Id, RivalName, party, Rival2Prize, RivalSight);
        }

        // ---- 1番道路 ----
        public static readonly TrainerDefinition Route01Kid = new TrainerDefinition("town01.route01.kid", "雪あそびの 子",
            new[] { new TrainerMon(Town01Species.Kogara, 3), new TrainerMon(Town01Species.Omochi, 4) }, prize: 120, sightRange: RouteSight);
        public static readonly TrainerDefinition Route01Angler = new TrainerDefinition("town01.route01.angler", "川釣りの 子",
            new[] { new TrainerMon(Town01Species.Yamame, 5) }, prize: 100, sightRange: RouteSight);

        // ---- 2番道路 ----
        public static readonly TrainerDefinition Route02SnowballKid = new TrainerDefinition("town01.route02.snowball", "雪合戦の 子",
            new[] { new TrainerMon(Town01Species.Konayuki, 8), new TrainerMon(Town01Species.Kogara, 8) }, prize: 160, sightRange: RouteSight);
        public static readonly TrainerDefinition Route02Hiker = new TrainerDefinition("town01.route02.hiker", "山歩きの 人",
            new[] { new TrainerMon(Town01Species.Ishikoro, 9), new TrainerMon(Town01Species.Tsukushi, 9) }, prize: 200, sightRange: RouteSight);
        public static readonly TrainerDefinition Route02ForestKid = new TrainerDefinition("town01.route02.forest", "林の 子",
            new[] { new TrainerMon(Town01Species.Warashi, 10) }, prize: 200, sightRange: RouteSight);

        // ---- 道場（岩） ----
        public static readonly TrainerDefinition GymDisciple1 = new TrainerDefinition("town01.gym.disciple1", "門下生",
            new[] { new TrainerMon(Town01Species.Ishikoro, 10) }, prize: 200, sightRange: GymSight, cpu: CpuLevel.Normal);
        public static readonly TrainerDefinition GymDisciple2 = new TrainerDefinition("town01.gym.disciple2", "門下生",
            new[] { new TrainerMon(Town01Species.Ishikoro, 10), new TrainerMon(Town01Species.Kawara, 11) }, prize: 220,
            sightRange: GymSight, cpu: CpuLevel.Normal);
        /// <summary>道場主は話しかけたときだけ戦う（目が合っても来ない）</summary>
        public static readonly TrainerDefinition GymLeader = new TrainerDefinition("town01.gym.leader", "道場主の " + GymLeaderName,
            new[] { new TrainerMon(Town01Species.Ishikoro, 12), new TrainerMon(Town01Species.Kawara, 14) }, prize: 1400, sightRange: 0,
            cpu: CpuLevel.Normal);
    }
}
