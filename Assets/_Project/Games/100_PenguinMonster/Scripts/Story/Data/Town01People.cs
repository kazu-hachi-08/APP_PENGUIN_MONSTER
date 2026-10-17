using System;
using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 最初の章の人と台本。ハツユキ村・研究所・ぬくもり小屋・よろず屋の人はここ、
    /// ツララ町・道路の人と落とし物は .Town.cs、道場とライバルの2戦目は .Gym.cs
    /// </summary>
    public static partial class Town01People
    {
        /// <summary>最初の1体と一緒に博士がくれる捕獲タマゴの数（本家の最初のボールと同じ 5個）</summary>
        public const int StarterCaptureEggs = 5;

        /// <summary>最初の1体をもらうまで、村の北の出口をふさぐ場所</summary>
        public static readonly FieldSpot MomAtExit = Town01Maps.StepFrom(Town01Maps.Village, Town01Maps.NorthExit, Direction.Down);

        /// <summary>最初から始めたときだけ、家で目覚めたところで流す文</summary>
        public static readonly IReadOnlyList<EventStep> Opening = new[]
        {
            EventStep.Say("ハツユキ村の 朝。窓の 外は 今日も 雪が 降っている"),
            EventStep.Say("そうだ！ 今日は シラカバ博士の 研究所で ペンモンを もらえる 日だ！"),
        };

        public static IReadOnlyList<FieldNpc> Create()
        {
            var people = new List<FieldNpc> { Mom(), Professor(), Assistant(), Villager(), RivalFamily(), Rival(), HutKeeper(), Clerk() };
            people.AddRange(TownAndRoutes());
            people.AddRange(GymAndRival2());
            return people;
        }

        private static FieldNpc Mom()
        {
            return new FieldNpc("mom", NpcLook.Mom,
                NpcPlacement.Until(Town01Flags.GotStarter, MomAtExit,
                    EventStep.Say("あら、どこへ 行くの？"),
                    EventStep.Say("シラカバ博士の 研究所で ペンモンを もらってから に しなさい")),
                NpcPlacement.Always(new FieldSpot(Town01Maps.PlayerHouse, new GridPos(5, 2), Direction.Left),
                    EventStep.Say("おかえり！ 少し 休んで いきなさい"),
                    // 家でも ぬくもり小屋と同じく休める。全滅したら この家に戻る
                    EventStep.Heal("ペンモンたちは すっかり 元気に なった！", Town01Maps.Start),
                    EventStep.Say("村の 外に 行くときは 気を つけるのよ")));
        }

        private static FieldNpc Professor()
        {
            var spot = new FieldSpot(Town01Maps.Lab, new GridPos(4, 2), Direction.Down);
            return new FieldNpc("professor", NpcLook.Professor,
                NpcPlacement.Until(Town01Flags.GotStarter, spot,
                    EventStep.Say("おお、来たか！ わしは シラカバ。ペンモンの 研究を しておる"),
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
                    EventStep.Say("シラカバ博士は 雪国の ペンモンの 研究を しているの")),
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
                    EventStep.Say("おそいぞ！ おれは ユキト。博士が 待ちくたびれてるぜ")),
            };
            placements.AddRange(PerStarter(Town01Trainers.RivalBeaten, RivalAtLabExit, starter => RivalBattle(Town01Trainers.Rival(starter))));
            return new FieldNpc("rival", NpcLook.Rival, placements.ToArray());
        }

        /// <summary>
        /// ライバル戦の置き場所を、こちらが選んだ最初の子ごとに1つずつ作る（ライバルの手持ちがこちらの子で変わるため）。
        /// 選んだ子のフラグが立っていて、戦い終えるまでの間だけ立つ
        /// </summary>
        private static IEnumerable<NpcPlacement> PerStarter(string beatenFlag, FieldSpot spot, Func<StorySpecies, EventStep[]> script)
        {
            foreach (var starter in Town01Starters.All)
            {
                yield return NpcPlacement.Between(EventStep.ChosenFlag(Town01Flags.StarterPrefix, starter), beatenFlag, spot, script(starter));
            }
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
                EventStep.Say("おれは 先に ツララ町へ 行くぜ！ じゃあな！"),
            };
        }

        // 小屋・店の中の受付は奥の真ん中に立ち、主人公はその1つ手前（扉から2マス奥）で話す
        private static readonly GridPos CounterSpot = new GridPos(3, 1);
        private static readonly GridPos InFrontOfCounter = new GridPos(3, 2);
        private const string HutFarewell = "また いつでも どうぞ！";

        /// <summary>ぬくもり小屋で休ませたときの、全滅したときの戻り先（受付の前）</summary>
        private static readonly FieldSpot HutReturnSpot = new FieldSpot(Town01Maps.WarmHut, InFrontOfCounter, Direction.Up);

        /// <summary>
        /// ぬくもり小屋の受付。休ませる（全回復・全滅したらここへ戻る）→ あずかり箱、の順に聞く（本家の回復と預かりシステムを1人にまとめる）
        /// </summary>
        private static FieldNpc HutKeeper()
        {
            return new FieldNpc("hutKeeper", NpcLook.HutKeeper,
                NpcPlacement.Always(new FieldSpot(Town01Maps.WarmHut, CounterSpot, Direction.Down),
                    EventStep.Say("ようこそ ぬくもり小屋へ！"),
                    EventStep.Ask("ペンモンを 休ませて いきますか？", StorageSteps()),
                    EventStep.Heal("ペンモンたちは すっかり 元気に なった！", HutReturnSpot),
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
                    EventStep.Say("ユキトなら、先に 研究所へ 行ったよ")),
                NpcPlacement.Always(spot,
                    EventStep.Say("ユキトも ペンモンを もらって 張り切ってたよ")));
        }
    }
}
