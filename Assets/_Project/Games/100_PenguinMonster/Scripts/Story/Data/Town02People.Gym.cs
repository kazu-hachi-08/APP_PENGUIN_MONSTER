using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// カガミコ町の道場（門下生2人・道場主サエ）と、4番道路の奥のユキトとの3戦目（PLAN_STORY_TOWN02.md フェーズ4）。
    /// 3戦目を終えると門下生が道場の前からどき、サエに勝つと羽章をもらえる
    /// </summary>
    public static partial class Town02People
    {
        /// <summary>道場主は奥の真ん中（区切り1の道場と同じ形）。主人公はその1つ手前で話しかける</summary>
        public static readonly FieldSpot GymLeaderSpot = new FieldSpot(Town02Maps.Gym, new GridPos(5, 1), Direction.Down);

        private static IEnumerable<FieldNpc> GymAndRival3()
        {
            return new[]
            {
                Rival3(),
                Town01People.RouteTrainer("kagamikoGymDisciple1", NpcLook.Disciple, Town02Trainers.GymDisciple1, Town02Maps.Gym,
                    new GridPos(3, 9), Direction.Right, "押忍！ 氷の 上の 戦い、見せて やろう！", "氷の 子には 火と 岩の 技が よく 効く……"),
                Town01People.RouteTrainer("kagamikoGymDisciple2", NpcLook.Disciple, Town02Trainers.GymDisciple2, Town02Maps.Gym,
                    new GridPos(8, 5), Direction.Left, "押忍！ サエさんの 前に わたしが 相手だ！", "サエさんの 切り札は もっと 冷たいぞ"),
                GymLeader(),
            };
        }

        /// <summary>ユキトの3戦目。こちらの最初の子ごとに手持ちが変わるので、選んだ子のフラグごとに置き場所を分ける</summary>
        private static FieldNpc Rival3()
        {
            var placements = Town01People.PerStarter(Town02Trainers.Rival3Beaten, Route04EndSpot,
                starter => Rival3Battle(Town02Trainers.Rival3(starter)));
            return new FieldNpc("rival3", NpcLook.Rival, new List<NpcPlacement>(placements).ToArray());
        }

        /// <summary>
        /// 2戦目と同じく、負けても話は進み、ユキトが回復してくれる（行き止まりで動けなくならないように）。
        /// 終わると道場主が戻ったと教えて、門下生をどかせる
        /// </summary>
        private static EventStep[] Rival3Battle(TrainerDefinition rival)
        {
            return new[]
            {
                EventStep.Say("よう！ やっぱり ここまで 来たな"),
                EventStep.Say("サエさんに 挑む 前に、おれと 勝負だ！"),
                EventStep.Battle(rival, canLose: true),
                EventStep.Say("ふう…… お前、また 腕を 上げたな"),
                EventStep.Heal("ユキトが ペンモンを 元気に してくれた！", Town01People.HutReturnSpotOf(Town02Maps.WarmHut)),
                EventStep.Say("そうだ、道場主の サエさんが 道場に 戻ったってさ"),
                EventStep.SetFlag(Town02Flags.Route04Cleared),
                EventStep.Say("おれは この 奥で もう少し 鍛えてくぜ。先に 行けよ！"),
            };
        }

        /// <summary>話しかけて挑む。負けたら全滅して台本はそこで終わり、もう一度話しかければ再び挑める。勝った後は北の 5番道路を案内する</summary>
        private static FieldNpc GymLeader()
        {
            var leader = Town02Trainers.GymLeader;
            return new FieldNpc("kagamikoGymLeader", NpcLook.GymLeader,
                NpcPlacement.Until(leader.DefeatedFlag, GymLeaderSpot,
                    EventStep.Say("ようこそ。わたしが カガミコ町 道場の 主、サエ"),
                    EventStep.Say("鏡のような 湖で 磨いた 氷の 技、見切れる かしら？"),
                    EventStep.Battle(leader),
                    EventStep.Say("……見事。あなたの 動きは 少しも 曇って いなかった"),
                    EventStep.Say("その 強さの 証に これを"),
                    EventStep.GiveBadge(Town02Trainers.BadgeId, Town02Trainers.BadgeDisplayName),
                    EventStep.Say("次の 道場へは 町の 北から 5番道路を 進むと いいわ")),
                NpcPlacement.Always(GymLeaderSpot,
                    EventStep.Say("あなたなら きっと 次の 道場も 越えられる"),
                    EventStep.Say("町の 北の 出口から 5番道路へ 進みなさい")));
        }
    }
}
