using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 7番道路の途中のユキトとの5戦目、大橋の展望台のハヤテ、カザハナ町の道場（門下生2人・道場主ハヤテ）、大橋の隠しコマンドの虹の羽の子。
    /// ハヤテは展望台で会うと道場の奥へ戻り、話しかけて挑む。勝つと羽章と賢い羽をもらえる
    /// </summary>
    public static partial class Town04People
    {
        /// <summary>虹の羽の子のレベル</summary>
        public const int NijibaneLevel = 30;

        /// <summary>大橋の東の端の展望台。ハヤテが風を読んでいる</summary>
        public static readonly FieldSpot HayateAtLookoutDeck = new FieldSpot(Town04Maps.Bridge, new GridPos(38, 5), Direction.Left);
        /// <summary>道場主は奥の真ん中（ほかの町の道場と同じ形）。主人公はその1つ手前で話しかける</summary>
        public static readonly FieldSpot GymLeaderSpot = new FieldSpot(Town04Maps.Gym, new GridPos(5, 1), Direction.Down);
        /// <summary>7番道路の真ん中の段の西の端。ここから東を見張るユキト（5戦目）と、北へ抜けようとする主人公の目が合う</summary>
        public static readonly FieldSpot Route07RivalSpot = new FieldSpot(Town04Maps.Route07, new GridPos(1, 15), Direction.Right);

        /// <summary>
        /// 隠しコマンドが通ったときの台本。捕まえたときだけフラグが立ち、倒した・逃げたときは空へ帰る（もう一度コマンドを入れれば現れる。
        /// 珍しい子を取り逃がして二度と手に入らない、を起こさないため）
        /// </summary>
        public static readonly IReadOnlyList<EventStep> RainbowScript = new[]
        {
            EventStep.Say("空の 向こうが 虹色に 光った！"),
            EventStep.Say("虹色の 羽の ペンモンが 舞い降りてきた！"),
            EventStep.CatchOnlyBattle(Town04Species.Nijibane, NijibaneLevel,
                EventStep.Say("虹色の 子は 空へ 帰っていった……")),
            EventStep.SetFlag(Town04Flags.RainbowCaught),
        };

        /// <summary>隠しコマンドで虹の羽の子が現れるか（捕まえた後は現れない）</summary>
        public static bool RainbowAppears(StoryFlags flags) => !flags.Has(Town04Flags.RainbowCaught);

        private static IEnumerable<FieldNpc> GymRival5AndHayate()
        {
            return new[]
            {
                Rival5(), Hayate(),
                Town01People.RouteTrainer("kazahanaGymDisciple1", NpcLook.Disciple, Town04Trainers.GymDisciple1, Town04Maps.Gym,
                    new GridPos(3, 9), Direction.Right, "押忍！ 風より 速い 翼を 見よ！", "鳥の 子には 氷や 岩の 技が 効く……"),
                Town01People.RouteTrainer("kazahanaGymDisciple2", NpcLook.Disciple, Town04Trainers.GymDisciple2, Town04Maps.Gym,
                    new GridPos(8, 5), Direction.Left, "押忍！ ハヤテさんの 前に 私が 相手だ！", "ハヤテさんの 切り札は 竜巻の ように 速いぞ"),
            };
        }

        /// <summary>ユキトの5戦目。こちらの最初の子ごとに手持ちが変わるので、選んだ子のフラグごとに置き場所を分ける</summary>
        private static FieldNpc Rival5()
        {
            var placements = Town01People.PerStarter(Town04Trainers.Rival5Beaten, Route07RivalSpot,
                starter => Rival5Battle(Town04Trainers.Rival5(starter)));
            return new FieldNpc("rival5", NpcLook.Rival, new List<NpcPlacement>(placements).ToArray());
        }

        /// <summary>
        /// 4戦目と同じく、負けても話は進み、ユキトが回復してくれる。
        /// カザハナ町の小屋はまだ見ていないので、戻り先は通ってきたユキオコシ町の小屋にする
        /// </summary>
        private static EventStep[] Rival5Battle(TrainerDefinition rival)
        {
            return new[]
            {
                EventStep.Say("よう！ お前も ライゾウさんの 羽章を もらったんだな"),
                EventStep.Say("おれの 子たちも 4体に なったぜ！ 勝負だ！"),
                EventStep.Battle(rival, canLose: true),
                EventStep.Say("くっ…… お前、また 強く なったな"),
                EventStep.Heal("ユキトが ペンモンを 元気に してくれた！", Town01People.HutReturnSpotOf(Town03Maps.WarmHut)),
                EventStep.Say("カザハナ町の 道場主は 留守が 多いって 話だぜ"),
                EventStep.Say("おれは 先に 行くぜ！ じゃあな！"),
            };
        }

        /// <summary>
        /// 展望台で会うと道場の奥へ戻る（道場の前の門下生もどく）。道場では話しかけて挑み、負けたら全滅して台本はそこで終わる。
        /// 勝った後は北の 9番道路を案内する
        /// </summary>
        private static FieldNpc Hayate()
        {
            var leader = Town04Trainers.GymLeader;
            return new FieldNpc("hayate", NpcLook.GymLeader,
                NpcPlacement.Until(Town04Flags.MetHayate, HayateAtLookoutDeck,
                    EventStep.Say("ん？ 自転車で ここまで 来たの？"),
                    EventStep.Say("いい 風だったでしょ？"),
                    EventStep.Say("あたしは ハヤテ。カザハナ町 道場の 主よ"),
                    EventStep.Say("ここで 風を 読むのが 日課なの。今日の 風は…… 挑戦者が 来るって 言ってた"),
                    EventStep.SetFlag(Town04Flags.MetHayate),
                    EventStep.Say("道場で 待ってる！ 追い風に 乗って おいで！")),
                NpcPlacement.Until(leader.DefeatedFlag, GymLeaderSpot,
                    EventStep.Say("来たわね！ 風の 読みどおり！"),
                    EventStep.Say("あたしの 翼に ついて これるかしら？ 勝負よ！"),
                    EventStep.Battle(leader),
                    EventStep.Say("……っ！ あたしの 風を 読み切るなんて！"),
                    EventStep.Say("いいわ、認めてあげる！"),
                    EventStep.GiveBadge(Town04Trainers.BadgeId, Town04Trainers.BadgeDisplayName),
                    EventStep.Say("それと、これも 持って いきなさい"),
                    EventStep.GiveItem(StoryItem.HeldWiseFeather, 1),
                    EventStep.Say("持たせると 特殊技が 少し 強く なるの"),
                    EventStep.Say("次は 町の 北の 9番道路の 先よ。風が そう 言ってる！")),
                NpcPlacement.Always(GymLeaderSpot,
                    EventStep.Say("また 風を 読みに 大橋へ 行こうかな"),
                    EventStep.Say("町の 北の 出口から 9番道路へ 進むと いいわ")));
        }
    }
}
