using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ユキオコシ町の道場（門下生2人・道場主ライゾウ）、5番道路の途中のユキトとの4戦目、発電所に居座る No.239。
    /// 発電所の子を倒すか捕まえると電気が戻り、ライゾウが扉の前から道場の中へ戻る。ライゾウに勝つと羽章をもらえる
    /// </summary>
    public static partial class Town03People
    {
        /// <summary>発電所に居座る子のレベル</summary>
        public const int HesotoriLevel = 24;

        /// <summary>道場の扉の前。電気が戻るまで道場主ライゾウがここに立つ</summary>
        public static readonly FieldSpot LeaderAtGymDoor = Town01Maps.StepFrom(Town03Maps.Town, Town03Maps.GymDoor, Direction.Down);
        /// <summary>道場主は奥の真ん中（ツララ町・カガミコ町の道場と同じ形）。主人公はその1つ手前で話しかける</summary>
        public static readonly FieldSpot GymLeaderSpot = new FieldSpot(Town03Maps.Gym, new GridPos(5, 1), Direction.Down);
        /// <summary>発電所の奥。No.239 が居座る。主人公はその1つ下で話す</summary>
        public static readonly FieldSpot PowerPlantCoreSpot = new FieldSpot(Town03Maps.PowerPlant, new GridPos(5, 1), Direction.Down);
        /// <summary>5番道路の真ん中の切れ目の西の端。ここから東を見張るユキト（4戦目）と、北へ抜けようとする主人公の目が合う</summary>
        public static readonly FieldSpot Route05RivalSpot = new FieldSpot(Town03Maps.Route05, new GridPos(5, 6), Direction.Right);

        private static IEnumerable<FieldNpc> GymRival4AndPowerPlant()
        {
            return new[]
            {
                Rival4(), PowerPlantHesotori(),
                Town01People.RouteTrainer("yukiokoshiGymDisciple1", NpcLook.Disciple, Town03Trainers.GymDisciple1, Town03Maps.Gym,
                    new GridPos(3, 9), Direction.Right, "押忍！ 雷の 速さに ついて これるか！", "雷の 子には 岩の 子を ぶつけると いい……"),
                Town01People.RouteTrainer("yukiokoshiGymDisciple2", NpcLook.Disciple, Town03Trainers.GymDisciple2, Town03Maps.Gym,
                    new GridPos(8, 5), Direction.Left, "押忍！ ライゾウさんの 前に わしが 相手だ！", "ライゾウさんの 切り札は もっと しびれるぞ"),
                GymLeader(),
            };
        }

        /// <summary>ユキトの4戦目。こちらの最初の子ごとに手持ちが変わるので、選んだ子のフラグごとに置き場所を分ける</summary>
        private static FieldNpc Rival4()
        {
            var placements = Town01People.PerStarter(Town03Trainers.Rival4Beaten, Route05RivalSpot,
                starter => Rival4Battle(Town03Trainers.Rival4(starter)));
            return new FieldNpc("rival4", NpcLook.Rival, new List<NpcPlacement>(placements).ToArray());
        }

        /// <summary>
        /// 3戦目と同じく、負けても話は進み、ユキトが回復してくれる。
        /// ユキオコシ町の小屋はまだ見ていないかもしれないので、戻り先は通ってきたカガミコ町の小屋にする
        /// </summary>
        private static EventStep[] Rival4Battle(TrainerDefinition rival)
        {
            return new[]
            {
                EventStep.Say("よう！ お前も サエさんの 羽章を もらったんだな"),
                EventStep.Say("ユキオコシ町へ 行く 前に、どっちが 強いか 勝負だ！"),
                EventStep.Battle(rival, canLose: true),
                EventStep.Say("ふう…… やっぱり お前と 戦うと 燃えるぜ"),
                EventStep.Heal("ユキトが ペンモンを 元気に してくれた！", Town01People.HutReturnSpotOf(Town02Maps.WarmHut)),
                EventStep.Say("ユキオコシ町の 道場は 雷の 道場 らしいぜ"),
                EventStep.Say("おれは 先に 行くぜ！ じゃあな！"),
            };
        }

        /// <summary>
        /// 発電所の機械に居座る No.239。倒すか捕まえると電気が戻る。
        /// 逃げた・負けたときは居座ったままなので、もう一度話しかければまた戦える
        /// </summary>
        private static FieldNpc PowerPlantHesotori()
        {
            return FieldNpc.WildMonster("powerPlantCore", Town03Species.Hesotori,
                NpcPlacement.Until(Town03Flags.PowerRestored, PowerPlantCoreSpot,
                    EventStep.Say("バチバチッ！ 発電所の 機械に 何かが 居座っている！"),
                    EventStep.WildBattle(Town03Species.Hesotori, HesotoriLevel),
                    EventStep.SetFlag(Town03Flags.PowerRestored),
                    EventStep.Say("電気が 戻った！")));
        }

        /// <summary>
        /// 停電の間は道場が開かないので、扉の前で発電所へ向かわせる。電気が戻ると道場の奥へ戻り、話しかけて挑む。
        /// 負けたら全滅して台本はそこで終わり、もう一度話しかければ再び挑める。勝った後は北の 7番道路を案内する
        /// </summary>
        private static FieldNpc GymLeader()
        {
            var leader = Town03Trainers.GymLeader;
            return new FieldNpc("yukiokoshiGymLeader", NpcLook.GymLeader,
                NpcPlacement.Until(Town03Flags.PowerRestored, LeaderAtGymDoor,
                    EventStep.Say("ガハハ！ わしが ユキオコシ町 道場の 主、ライゾウだ！"),
                    EventStep.Say("……と 言いたい ところだが、停電で 道場が 開けられん"),
                    EventStep.Say("東の 6番道路の 奥の 発電所に、何かが 居座って いるらしい")),
                NpcPlacement.Until(leader.DefeatedFlag, GymLeaderSpot,
                    EventStep.Say("ガハハ！ 電気を 戻して くれたのは お前か！"),
                    EventStep.Say("礼は 雷の 一撃で 返して やろう！ かかって こい！"),
                    EventStep.Battle(leader),
                    EventStep.Say("ぬうっ…… わしの 雷を 受けきるとは！"),
                    EventStep.Say("見事だ！ これを 持って いけ！"),
                    EventStep.GiveBadge(Town03Trainers.BadgeId, Town03Trainers.BadgeDisplayName),
                    EventStep.Say("次の 道場は 町の 北の 7番道路を 抜けた 先、カザハナ町だ")),
                NpcPlacement.Always(GymLeaderSpot,
                    EventStep.Say("ガハハ！ また 腕を 上げたら 相手を してやる！"),
                    EventStep.Say("町の 北の 出口から 7番道路へ 進むと いい"),
                    EventStep.Say("カザハナ町の ハヤテは 風の ように 速いぞ！ ガハハ！")));
        }
    }
}
