using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// トレーナー。5番・6番道路の6人・ユキトの4戦目・道場の門下生2人と道場主ライゾウ。
    /// 道路の子は CPU よわい、ユキトと道場は ふつう。技は覚える技の表から自動で決まる
    /// </summary>
    public static class Town03Trainers
    {
        public const string GymLeaderName = "ライゾウ";
        /// <summary>記録に残す羽章の名前（表示名は BadgeDisplayName）</summary>
        public const string BadgeId = "town03.badge";
        public const string BadgeDisplayName = "いなづま羽章";

        private const string Rival4Id = "town03.rival4";
        private const int Rival4Prize = 1440;
        /// <summary>
        /// 5番道路の真ん中の切れ目（横9マス）の端から横を見張る。北へ抜けるには必ずこの列を通るので、切れ目の幅いっぱいまで見る
        /// </summary>
        public const int Rival4Sight = 8;
        private const int RouteSight = 3;
        private const int GymSight = 3;
        private const int Rival4StarterLevel = 24;
        private const int Rival4TsugumiLevel = 22;
        private const int Rival4ThirdLevel = 21;

        /// <summary>ユキトの4戦目を終えた（負けても話は進むので、戦い終えた）</summary>
        public static readonly string Rival4Beaten = TrainerDefinition.DefeatedFlagOf(Rival4Id);

        /// <summary>4戦目（5番道路の途中）。3戦目のヒバナペンは Lv18 で進化しているので、進化後のイカズチペンで出す</summary>
        public static TrainerDefinition Rival4(StorySpecies playerStarter)
        {
            var counter = Town01Starters.CounterOf(playerStarter);
            var party = new[]
            {
                new TrainerMon(Town02Species.Ikazuchi, Rival4ThirdLevel),
                new TrainerMon(Town01Species.Tsugumi, Rival4TsugumiLevel),
                new TrainerMon(StorySpeciesTable.ByNo(counter.Evolution.IntoNo), Rival4StarterLevel),
            };
            return new TrainerDefinition(Rival4Id, Town01Trainers.RivalName, party, Rival4Prize, Rival4Sight, CpuLevel.Normal);
        }

        // ---- 5番道路 ----
        public static readonly TrainerDefinition Route05Hiker = new TrainerDefinition("town03.route05.hiker", "山歩きの 人",
            new[] { new TrainerMon(Town03Species.Jari, 17), new TrainerMon(Town03Species.Yukiishi, 18) }, prize: 360, sightRange: RouteSight);
        public static readonly TrainerDefinition Route05SnowfieldKid = new TrainerDefinition("town03.route05.snowfield", "雪原の 子",
            new[] { new TrainerMon(Town03Species.Yukigutsu, 17), new TrainerMon(Town03Species.Tsubame, 17), new TrainerMon(Town03Species.Sasa, 18) },
            prize: 360, sightRange: RouteSight);
        public static readonly TrainerDefinition Route05ThunderKid = new TrainerDefinition("town03.route05.thunder", "雷好きの 子",
            new[] { new TrainerMon(Town03Species.Mameden, 19), new TrainerMon(Town03Species.Gorogoro, 20) }, prize: 400, sightRange: RouteSight);

        // ---- 6番道路 ----
        public static readonly TrainerDefinition Route06Angler = new TrainerDefinition("town03.route06.angler", "釣りの 人",
            new[] { new TrainerMon(Town03Species.Hatahata, 20), new TrainerMon(Town02Species.Wakasagi, 21) }, prize: 420, sightRange: RouteSight);
        public static readonly TrainerDefinition Route06LineWorker = new TrainerDefinition("town03.route06.lineWorker", "電線工事の 人",
            new[] { new TrainerMon(Town03Species.Densen, 21), new TrainerMon(Town03Species.Gorogoro, 21) }, prize: 420, sightRange: RouteSight);
        public static readonly TrainerDefinition Route06NightWalker = new TrainerDefinition("town03.route06.night", "夜歩きの 人",
            new[] { new TrainerMon(Town03Species.Teiden, 21), new TrainerMon(Town03Species.Yukiakari, 22) }, prize: 440, sightRange: RouteSight);

        // ---- 道場（雷） ----
        public static readonly TrainerDefinition GymDisciple1 = new TrainerDefinition("town03.gym.disciple1", "門下生",
            new[] { new TrainerMon(Town03Species.Gorogoro, 22) }, prize: 440, sightRange: GymSight, cpu: CpuLevel.Normal);
        public static readonly TrainerDefinition GymDisciple2 = new TrainerDefinition("town03.gym.disciple2", "門下生",
            new[] { new TrainerMon(Town03Species.Densen, 22), new TrainerMon(Town03Species.Gorogoro, 23) }, prize: 460,
            sightRange: GymSight, cpu: CpuLevel.Normal);
        /// <summary>道場主は話しかけたときだけ戦う（目が合っても来ない）。最後に切り札のハツデンペンを出す</summary>
        public static readonly TrainerDefinition GymLeader = new TrainerDefinition("town03.gym.leader", "道場主の " + GymLeaderName,
            new[] { new TrainerMon(Town03Species.Gorogoro, 23), new TrainerMon(Town03Species.Tettou, 24), new TrainerMon(Town03Species.Hatsuden, 26) },
            prize: 2600, sightRange: 0, cpu: CpuLevel.Normal);
    }
}
