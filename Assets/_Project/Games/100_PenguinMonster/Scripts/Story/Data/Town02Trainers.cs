using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り2のトレーナー（PLAN_STORY_TOWN02.md §3）。3番・4番道路の6人・ユキトの3戦目・道場の門下生2人と道場主サエ。
    /// 道路の子は CPU よわい、ユキトと道場は ふつう。技は覚える技の表から自動で決まる
    /// </summary>
    public static class Town02Trainers
    {
        public const string GymLeaderName = "サエ";
        /// <summary>記録に残す羽章の名前（表示名は BadgeDisplayName）</summary>
        public const string BadgeId = "town02.badge";
        public const string BadgeDisplayName = "こおりかがみ羽章";

        private const string Rival3Id = "town02.rival3";
        private const int Rival3Prize = 1080;
        private const int RivalSight = 4;
        private const int RouteSight = 3;
        private const int GymSight = 3;
        private const int Rival3StarterLevel = 18;
        private const int Rival3TsugumiLevel = 16;
        private const int Rival3ThirdLevel = 15;

        /// <summary>ユキトの3戦目を終えた（負けても話は進むので、戦い終えた）</summary>
        public static readonly string Rival3Beaten = TrainerDefinition.DefeatedFlagOf(Rival3Id);

        /// <summary>3戦目（4番道路の奥）。最初の子に強い子は Lv16 で進化しているので、進化後の姿で出す</summary>
        public static TrainerDefinition Rival3(StorySpecies playerStarter)
        {
            var counter = Town01Starters.CounterOf(playerStarter);
            var party = new[]
            {
                new TrainerMon(Town02Species.Hibana, Rival3ThirdLevel),
                new TrainerMon(Town01Species.Tsugumi, Rival3TsugumiLevel),
                new TrainerMon(StorySpeciesTable.ByNo(counter.Evolution.IntoNo), Rival3StarterLevel),
            };
            return new TrainerDefinition(Rival3Id, Town01Trainers.RivalName, party, Rival3Prize, RivalSight, CpuLevel.Normal);
        }

        // ---- 3番道路 ----
        public static readonly TrainerDefinition Route03SnowfieldKid = new TrainerDefinition("town02.route03.snowfield", "雪原の 子",
            new[] { new TrainerMon(Town02Species.Hibari, 11), new TrainerMon(Town02Species.Hanten, 11), new TrainerMon(Town02Species.Fuyume, 12) },
            prize: 240, sightRange: RouteSight);
        public static readonly TrainerDefinition Route03Angler = new TrainerDefinition("town02.route03.angler", "湖釣りの 人",
            new[] { new TrainerMon(Town02Species.Wakasagi, 12), new TrainerMon(Town01Species.Yamame, 13) }, prize: 260, sightRange: RouteSight);
        public static readonly TrainerDefinition Route03Hiker = new TrainerDefinition("town02.route03.hiker", "山歩きの 人",
            new[] { new TrainerMon(Town02Species.Gorota, 13), new TrainerMon(Town01Species.Ishikoro, 14) }, prize: 280, sightRange: RouteSight);

        // ---- 4番道路 ----
        public static readonly TrainerDefinition Route04ThunderKid = new TrainerDefinition("town02.route04.thunder", "雷好きの 子",
            new[] { new TrainerMon(Town02Species.Hibana, 14), new TrainerMon(Town01Species.Pachiri, 15) }, prize: 300, sightRange: RouteSight);
        public static readonly TrainerDefinition Route04Skater = new TrainerDefinition("town02.route04.skater", "スケートの 子",
            new[] { new TrainerMon(Town02Species.Shimobashira, 14), new TrainerMon(Town02Species.Kohakuchou, 15) }, prize: 300,
            sightRange: RouteSight);
        public static readonly TrainerDefinition Route04NightWalker = new TrainerDefinition("town02.route04.night", "夜歩きの 人",
            new[] { new TrainerMon(Town02Species.Itachi, 15), new TrainerMon(Town01Species.Warashi, 16) }, prize: 320, sightRange: RouteSight);

        // ---- 道場（氷） ----
        public static readonly TrainerDefinition GymDisciple1 = new TrainerDefinition("town02.gym.disciple1", "門下生",
            new[] { new TrainerMon(Town02Species.Shimobashira, 16) }, prize: 320, sightRange: GymSight, cpu: CpuLevel.Normal);
        public static readonly TrainerDefinition GymDisciple2 = new TrainerDefinition("town02.gym.disciple2", "門下生",
            new[] { new TrainerMon(Town01Species.Konayuki, 16), new TrainerMon(Town02Species.Kohakuchou, 17) }, prize: 340,
            sightRange: GymSight, cpu: CpuLevel.Normal);
        /// <summary>道場主は話しかけたときだけ戦う（目が合っても来ない）。最後に切り札のウスライを出す</summary>
        public static readonly TrainerDefinition GymLeader = new TrainerDefinition("town02.gym.leader", "道場主の " + GymLeaderName,
            new[] { new TrainerMon(Town02Species.Hasuha, 17), new TrainerMon(Town02Species.Shimobashira, 18), new TrainerMon(Town02Species.Usurai, 20) },
            prize: 2000, sightRange: 0, cpu: CpuLevel.Normal);
    }
}
