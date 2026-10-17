namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り1のトレーナー（PLAN_STORY_TOWN01.md フェーズ5）。今は研究所のライバルと、仮の1番道路の1人だけ。
    /// 本番の顔ぶれと手持ちはフェーズ8c で作り直す
    /// </summary>
    public static class Town01Trainers
    {
        private const string RivalId = "town01.rival1";
        private const int RivalPrize = 300;
        private const int RivalSight = 4;
        private const int RouteSight = 3;

        /// <summary>ライバルに勝った（負けても話は進むので、戦い終えた）</summary>
        public static readonly string RivalBeaten = TrainerDefinition.DefeatedFlagOf(RivalId);

        /// <summary>最初のライバル戦。こちらの最初の1体に強い子を、同じ Lv5 で出す</summary>
        public static TrainerDefinition Rival(StorySpecies playerStarter)
        {
            var party = new[] { new TrainerMon(Town01Starters.CounterOf(playerStarter), StoryBalance.StarterLevel) };
            return new TrainerDefinition(RivalId, "ライバル", party, RivalPrize, RivalSight);
        }

        public static readonly TrainerDefinition Route01Kid = new TrainerDefinition("town01.route01.kid", "雪あそびの 子",
            new[] { new TrainerMon(Town01Species.Kogara, 3), new TrainerMon(Town01Species.Omochi, 4) }, prize: 120, sightRange: RouteSight);
    }
}
