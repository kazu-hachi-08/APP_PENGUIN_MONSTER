using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// トレーナー。7番道路の3人・大橋の自転車乗り3人・ユキトの5戦目・道場の門下生2人と道場主ハヤテ。
    /// 道路の子は CPU よわい、ユキトと道場は ふつう。技は覚える技の表から自動で決まる
    /// </summary>
    public static class Town04Trainers
    {
        public const string GymLeaderName = "ハヤテ";
        /// <summary>記録に残す羽章の名前（表示名は BadgeDisplayName）</summary>
        public const string BadgeId = "town04.badge";
        public const string BadgeDisplayName = "かざきり羽章";

        private const string Rival5Id = "town04.rival5";
        private const int Rival5Prize = 1800;
        /// <summary>
        /// 7番道路の真ん中の段（西の端から東の木まで）を横に見張る。北へ抜けるには必ずこの段を通るので、段の幅いっぱいまで見る
        /// </summary>
        public const int Rival5Sight = 15;
        private const int RouteSight = 3;
        private const int GymSight = 3;
        private const int Rival5StarterLevel = 30;
        private const int Rival5OtherLevel = 28;
        private const int Rival5HayabusaLevel = 27;

        /// <summary>ユキトの5戦目を終えた（負けても話は進むので、戦い終えた）</summary>
        public static readonly string Rival5Beaten = TrainerDefinition.DefeatedFlagOf(Rival5Id);

        /// <summary>5戦目（7番道路の途中）。最初の子は進化後のまま（次の進化は Lv36）。4戦目の3体に ハヤブサペンを足す</summary>
        public static TrainerDefinition Rival5(StorySpecies playerStarter)
        {
            var counter = Town01Starters.CounterOf(playerStarter);
            var party = new[]
            {
                new TrainerMon(Town03Species.Hayabusa, Rival5HayabusaLevel),
                new TrainerMon(Town02Species.Ikazuchi, Rival5OtherLevel),
                new TrainerMon(Town01Species.Tsugumi, Rival5OtherLevel),
                new TrainerMon(StorySpeciesTable.ByNo(counter.Evolution.IntoNo), Rival5StarterLevel),
            };
            return new TrainerDefinition(Rival5Id, Town01Trainers.RivalName, party, Rival5Prize, Rival5Sight, CpuLevel.Normal);
        }

        // ---- 7番道路 ----
        public static readonly TrainerDefinition Route07Angler = new TrainerDefinition("town04.route07.angler", "釣りの 人",
            new[] { new TrainerMon(Town04Species.Haze, 24), new TrainerMon(Town03Species.Hatahata, 24) }, prize: 480, sightRange: RouteSight);
        public static readonly TrainerDefinition Route07Hiker = new TrainerDefinition("town04.route07.hiker", "山歩きの 人",
            new[] { new TrainerMon(Town04Species.Gareba, 24), new TrainerMon(Town04Species.Mizore, 25) }, prize: 500, sightRange: RouteSight);
        public static readonly TrainerDefinition Route07KiteKid = new TrainerDefinition("town04.route07.kite", "凧あげの 子",
            new[] { new TrainerMon(Town04Species.Soyokaze, 23), new TrainerMon(Town04Species.Fuusen, 24), new TrainerMon(Town04Species.Kazaguruma, 25) },
            prize: 500, sightRange: RouteSight);

        // ---- 8番道路 風花大橋 ----
        public static readonly TrainerDefinition BridgeCyclist1 = new TrainerDefinition("town04.bridge.cyclist1", "自転車乗り",
            new[] { new TrainerMon(Town04Species.Umineko, 25), new TrainerMon(Town04Species.Soyokaze, 26) }, prize: 520, sightRange: RouteSight);
        public static readonly TrainerDefinition BridgeCyclist2 = new TrainerDefinition("town04.bridge.cyclist2", "自転車乗り",
            new[] { new TrainerMon(Town04Species.Kazaguruma, 27), new TrainerMon(Town04Species.Oikaze, 27) }, prize: 540, sightRange: RouteSight);
        public static readonly TrainerDefinition BridgeCyclist3 = new TrainerDefinition("town04.bridge.cyclist3", "自転車乗り",
            new[] { new TrainerMon(Town04Species.Shabon, 27), new TrainerMon(Town04Species.Umineko, 28) }, prize: 560, sightRange: RouteSight);

        // ---- 道場（鳥） ----
        public static readonly TrainerDefinition GymDisciple1 = new TrainerDefinition("town04.gym.disciple1", "門下生",
            new[] { new TrainerMon(Town03Species.Hayabusa, 28) }, prize: 560, sightRange: GymSight, cpu: CpuLevel.Normal);
        public static readonly TrainerDefinition GymDisciple2 = new TrainerDefinition("town04.gym.disciple2", "門下生",
            new[] { new TrainerMon(Town04Species.Umineko, 28), new TrainerMon(Town04Species.Oikaze, 29) }, prize: 580,
            sightRange: GymSight, cpu: CpuLevel.Normal);
        /// <summary>
        /// 道場主は話しかけたときだけ戦う（目が合っても来ない）。最後に切り札のツムジペン（向かい風を持つ）を出す。
        /// 切り札は 軽い羽 で先手を取りやすくし、風の速さを見せる
        /// </summary>
        public static readonly TrainerDefinition GymLeader = new TrainerDefinition("town04.gym.leader", "道場主の " + GymLeaderName,
            new[]
            {
                new TrainerMon(Town04Species.Oikaze, 29), new TrainerMon(Town04Species.Ookamome, 30),
                new TrainerMon(Town04Species.Tsumuji, 32, HeldItem.LightFeather),
            },
            prize: 3200, sightRange: 0, cpu: CpuLevel.Normal);
    }
}
