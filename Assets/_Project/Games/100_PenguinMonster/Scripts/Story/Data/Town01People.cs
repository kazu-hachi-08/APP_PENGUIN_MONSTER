using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り1の人と台本（PLAN_STORY_TOWN01.md フェーズ4〜6）。今は仮のハツユキ村と試しの1番道路の分だけ。
    /// 名前（博士・ライバルなど）はまだ決めていないので役割で呼ぶ。本番の物語はフェーズ8c で書き直す
    /// </summary>
    public static class Town01People
    {
        /// <summary>最初の1体をもらうまで、村の北の出口をふさぐ場所</summary>
        /// <summary>最初の1体と一緒に博士がくれる捕獲タマゴの数（本家の最初のボールと同じ 5個）</summary>
        public const int StarterCaptureEggs = 5;

        public static readonly FieldSpot MomAtExit = new FieldSpot(Town01Maps.Village, Town01Maps.NorthExit + Direction.Down.Offset(), Direction.Down);

        public static IReadOnlyList<FieldNpc> Create()
        {
            return new[] { Mom(), Professor(), Assistant(), Villager(), RivalFamily(), Rival(), Route01Kid(), HutKeeper(), Clerk() };
        }

        private static FieldNpc Mom()
        {
            return new FieldNpc("mom", NpcLook.Mom,
                NpcPlacement.Until(Town01Flags.GotStarter, MomAtExit,
                    EventStep.Say("あら、どこへ 行くの？"),
                    EventStep.Say("博士の 研究所で ペンモンを もらってから に しなさい")),
                NpcPlacement.Always(new FieldSpot(Town01Maps.PlayerHouse, new GridPos(5, 2), Direction.Left),
                    EventStep.Say("おかえり！ 少し 休んで いきなさい"),
                    // ぬくもり小屋（フェーズ6）ができるまでの仮の回復。全滅したら この家に戻る
                    EventStep.Heal("ペンモンたちは すっかり 元気に なった！", Town01Maps.Start),
                    EventStep.Say("村の 外に 行くときは 気を つけるのよ")));
        }

        private static FieldNpc Professor()
        {
            var spot = new FieldSpot(Town01Maps.Lab, new GridPos(4, 2), Direction.Down);
            return new FieldNpc("professor", NpcLook.Professor,
                NpcPlacement.Until(Town01Flags.GotStarter, spot,
                    EventStep.Say("おお、来たか！ 待って おったぞ"),
                    EventStep.Ask("ペンモンを 1体 もらって くれるかね？",
                        EventStep.Say("そうか…… 気が 変わったら また 来なさい")),
                    EventStep.Say("ここに 3つの タマゴが ある。好きな 子を 選ぶと いい！"),
                    EventStep.ChooseStarter(Town01Starters.All, StoryBalance.StarterLevel, Town01Flags.StarterPrefix),
                    EventStep.SetFlag(Town01Flags.GotStarter),
                    EventStep.Say("その子を 大事に 育てて おくれ"),
                    EventStep.Say("それと これも 持って いきなさい"),
                    EventStep.GiveItem(StoryItem.CaptureEgg, StarterCaptureEggs),
                    EventStep.Say("弱った 野生の ペンモンに 投げれば 捕まえられるぞ")),
                NpcPlacement.Always(spot,
                    EventStep.Say("ペンモンは 元気かね？"),
                    EventStep.Say("草むらには 野生の ペンモンが おるから 気を つけるんじゃぞ")));
        }

        private static FieldNpc Assistant()
        {
            var spot = new FieldSpot(Town01Maps.Lab, new GridPos(7, 4), Direction.Left);
            return new FieldNpc("assistant", NpcLook.Assistant,
                NpcPlacement.Until(Town01Flags.GotStarter, spot,
                    EventStep.Say("博士は ペンモンの 研究を しているの")),
                NpcPlacement.Always(spot,
                    EventStep.Say("タマゴから かえった 子、元気そうね！")));
        }

        private static FieldNpc Villager()
        {
            return new FieldNpc("villager", NpcLook.Villager,
                NpcPlacement.Always(new FieldSpot(Town01Maps.Village, new GridPos(12, 9), Direction.Down),
                    EventStep.Say("ハツユキ村は 一年中 雪が 降るんだ")));
        }

        /// <summary>研究所の出口の前で、最初の1体をもらった主人公を待ち構える。こちらの子に強い子を選んでいる</summary>
        private static readonly FieldSpot RivalAtLabExit = new FieldSpot(Town01Maps.Lab, new GridPos(4, 5), Direction.Up);

        /// <summary>ライバル。もらう前は博士のそばで待ち、もらったら出口の前から目が合って勝負を挑む。戦い終えたら先へ行って居なくなる</summary>
        private static FieldNpc Rival()
        {
            var placements = new List<NpcPlacement>
            {
                NpcPlacement.Until(Town01Flags.GotStarter, new FieldSpot(Town01Maps.Lab, new GridPos(2, 2), Direction.Down),
                    EventStep.Say("おそいぞ！ 博士が 待ちくたびれてるぜ")),
            };
            foreach (var starter in Town01Starters.All)
            {
                placements.Add(NpcPlacement.Between(EventStep.ChosenFlag(Town01Flags.StarterPrefix, starter), Town01Trainers.RivalBeaten,
                    RivalAtLabExit, RivalBattle(Town01Trainers.Rival(starter))));
            }
            return new FieldNpc("rival", NpcLook.Rival, placements.ToArray());
        }

        /// <summary>本家と同じく、最初のライバル戦は負けても話が進む。終わったら博士が回復してくれる</summary>
        private static EventStep[] RivalBattle(TrainerDefinition rival)
        {
            return new[]
            {
                EventStep.Say("待てよ！ せっかく ペンモンを もらったんだ"),
                EventStep.Say("おれと 勝負 しようぜ！"),
                EventStep.Battle(rival, canLose: true),
                EventStep.Say("ふう…… いい 勝負 だったな！"),
                EventStep.Heal("博士が ペンモンを 元気に してくれた！", Town01Maps.Start),
                EventStep.Say("おれは 先に 行くぜ！ じゃあな！"),
            };
        }

        /// <summary>試しの1番道路のトレーナー。道を歩いてくる主人公を横から見張る</summary>
        private static FieldNpc Route01Kid()
        {
            var spot = new FieldSpot(Town01Maps.Route01, new GridPos(8, 6), Direction.Left);
            var trainer = Town01Trainers.Route01Kid;
            return new FieldNpc("route01Kid", NpcLook.Youngster,
                NpcPlacement.Until(trainer.DefeatedFlag, spot,
                    EventStep.Say("目が 合ったら 勝負だ！"),
                    EventStep.Battle(trainer)),
                NpcPlacement.Always(spot,
                    EventStep.Say("草むらで 戦って 鍛え直しだ！")));
        }

        // 小屋・店の中の受付は奥の真ん中に立ち、主人公はその1つ手前（扉から2マス奥）で話す
        private static readonly GridPos CounterSpot = new GridPos(3, 1);
        private static readonly GridPos InFrontOfCounter = new GridPos(3, 2);
        private const string HutFarewell = "また いつでも どうぞ！";

        /// <summary>
        /// ぬくもり小屋の受付。休ませる（全回復・全滅したらここへ戻る）→ あずかり箱、の順に聞く（本家の回復と預かりシステムを1人にまとめる）
        /// </summary>
        private static FieldNpc HutKeeper()
        {
            var returnSpot = new FieldSpot(Town01Maps.WarmHut, InFrontOfCounter, Direction.Up);
            return new FieldNpc("hutKeeper", NpcLook.HutKeeper,
                NpcPlacement.Always(new FieldSpot(Town01Maps.WarmHut, CounterSpot, Direction.Down),
                    EventStep.Say("ようこそ ぬくもり小屋へ！"),
                    EventStep.Ask("ペンモンを 休ませて いきますか？", StorageSteps()),
                    EventStep.Heal("ペンモンたちは すっかり 元気に なった！", returnSpot),
                    StorageQuestion(),
                    EventStep.Storage(),
                    EventStep.Say(HutFarewell)));
        }

        /// <summary>休ませるかに「いいえ」と答えたときも、あずかり箱は使えるようにする</summary>
        private static EventStep[] StorageSteps()
        {
            return new[] { StorageQuestion(), EventStep.Storage(), EventStep.Say(HutFarewell) };
        }

        private static EventStep StorageQuestion()
        {
            return EventStep.Ask("あずかり箱を 使いますか？", EventStep.Say(HutFarewell));
        }

        private static FieldNpc Clerk()
        {
            return new FieldNpc("clerk", NpcLook.Clerk,
                NpcPlacement.Always(new FieldSpot(Town01Maps.Shop, CounterSpot, Direction.Down),
                    EventStep.Say("いらっしゃいませ！ よろず屋です"),
                    EventStep.Shop(StoryItem.CaptureEgg, StoryItem.Potion, StoryItem.Antidote, StoryItem.ParalyzeHeal, StoryItem.Awakening),
                    EventStep.Say("まいど ありがとう ございました！")));
        }

        private static FieldNpc RivalFamily()
        {
            var spot = new FieldSpot(Town01Maps.RivalHouse, new GridPos(2, 2), Direction.Down);
            return new FieldNpc("rivalFamily", NpcLook.Villager,
                NpcPlacement.Until(Town01Flags.GotStarter, spot,
                    EventStep.Say("うちの 子なら、先に 研究所へ 行ったよ")),
                NpcPlacement.Always(spot,
                    EventStep.Say("うちの 子も ペンモンを もらって 張り切ってたよ")));
        }
    }
}
