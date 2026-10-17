using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ツララ町の道場（門下生2人・道場主ガンテツ）と、2番道路の奥のユキトとの2戦目。
    /// 2戦目を終えると門下生が道場の前からどき、ガンテツに勝つと羽章をもらえる
    /// </summary>
    public static partial class Town01People
    {
        /// <summary>2番道路の奥の行き止まりの入口。ここから右（奥）を見張り、入ってきた主人公と目が合う</summary>
        private static readonly FieldSpot RivalAtRoute02End = new FieldSpot(Town01Maps.Route02, new GridPos(24, 19), Direction.Right);
        /// <summary>道場主は奥の真ん中。主人公はその1つ手前で話しかける</summary>
        public static readonly FieldSpot GymLeaderSpot = new FieldSpot(Town01Maps.Gym, new GridPos(5, 1), Direction.Down);

        private static IEnumerable<FieldNpc> GymAndRival2()
        {
            return new[]
            {
                Rival2(),
                RouteTrainer("gymDisciple1", NpcLook.Disciple, Town01Trainers.GymDisciple1, Town01Maps.Gym, new GridPos(3, 9), Direction.Right,
                    "押忍！ 道場主に 挑むなら まず わたしを 倒してからだ！", "岩の 子には 海と 苔の 技が よく 効く……"),
                RouteTrainer("gymDisciple2", NpcLook.Disciple, Town01Trainers.GymDisciple2, Town01Maps.Gym, new GridPos(8, 5), Direction.Left,
                    "押忍！ 固い 守りを 見せて やろう！", "ガンテツさんの 守りは こんな ものでは ないぞ"),
                GymLeader(),
            };
        }

        /// <summary>ユキトの2戦目。こちらの最初の子ごとに手持ちが変わるので、選んだ子のフラグごとに置き場所を分ける</summary>
        private static FieldNpc Rival2()
        {
            var placements = PerStarter(Town01Trainers.Rival2Beaten, RivalAtRoute02End, starter => Rival2Battle(Town01Trainers.Rival2(starter)));
            return new FieldNpc("rival2", NpcLook.Rival, new List<NpcPlacement>(placements).ToArray());
        }

        /// <summary>負けても話は進み、ユキトが回復してくれる（行き止まりで動けなくならないように）。終わると道場主が戻ったと教えて先へ行く</summary>
        private static EventStep[] Rival2Battle(TrainerDefinition rival)
        {
            return new[]
            {
                EventStep.Say("よう！ おれも ここで 修行してたんだ"),
                EventStep.Say("どっちが 強く なったか 勝負だ！"),
                EventStep.Battle(rival, canLose: true),
                EventStep.Say("ふう…… やっぱり お前とは いい 勝負に なるな"),
                EventStep.Heal("ユキトが ペンモンを 元気に してくれた！", HutReturnSpot),
                EventStep.Say("そうだ、道場主の ガンテツさんが 町に 戻ったってさ"),
                EventStep.SetFlag(Town01Flags.Route02Cleared),
                EventStep.Say("おれは もっと 鍛えてから 挑むぜ。先に 行って こいよ！"),
            };
        }

        /// <summary>話しかけて挑む。負けたら全滅して台本はそこで終わり、もう一度話しかければ再び挑める</summary>
        private static FieldNpc GymLeader()
        {
            var leader = Town01Trainers.GymLeader;
            return new FieldNpc("gymLeader", NpcLook.GymLeader,
                NpcPlacement.Until(leader.DefeatedFlag, GymLeaderSpot,
                    EventStep.Say("よく 来た！ わしが ツララ町 道場の 主、ガンテツだ"),
                    EventStep.Say("岩のように 固い この 守り、崩せる ものなら 崩して みよ！"),
                    EventStep.Battle(leader),
                    EventStep.Say("むう…… 見事な 攻めで あった"),
                    EventStep.Say("その 強さの 証に これを 授けよう"),
                    EventStep.GiveBadge(Town01Trainers.BadgeId, Town01Trainers.BadgeDisplayName),
                    EventStep.Say("羽章を 集めて いけば、いずれ 雪の 王にも 届くだろう")),
                NpcPlacement.Always(GymLeaderSpot,
                    EventStep.Say("道場を 巡り、もっと 強く なるのだ"),
                    EventStep.Say("次の 道場は 3番道路の 先…… 雪が 解けたら 行って みよ")));
        }
    }
}
