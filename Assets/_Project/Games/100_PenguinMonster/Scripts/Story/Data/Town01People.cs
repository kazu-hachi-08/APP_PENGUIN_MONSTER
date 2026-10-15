using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 区切り1の人と台本（PLAN_STORY_TOWN01.md フェーズ4）。今は仮のハツユキ村の分だけ。
    /// 名前（博士・ライバルなど）はまだ決めていないので役割で呼ぶ。本番の物語はフェーズ8c で書き直す
    /// </summary>
    public static class Town01People
    {
        /// <summary>最初の1体をもらうまで、村の北の出口をふさぐ場所</summary>
        public static readonly FieldSpot MomAtExit = new FieldSpot(Town01Maps.Village, Town01Maps.NorthExit + Direction.Down.Offset(), Direction.Down);

        public static IReadOnlyList<FieldNpc> Create()
        {
            return new[] { Mom(), Professor(), Assistant(), Villager(), RivalFamily() };
        }

        private static FieldNpc Mom()
        {
            return new FieldNpc("mom", NpcLook.Mom,
                NpcPlacement.Until(Town01Flags.GotStarter, MomAtExit,
                    EventStep.Say("あら、どこへ 行くの？"),
                    EventStep.Say("博士の 研究所で ペンモンを もらってから に しなさい")),
                NpcPlacement.Always(new FieldSpot(Town01Maps.PlayerHouse, new GridPos(5, 2), Direction.Left),
                    EventStep.Say("ペンモンを もらったのね！"),
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
                    EventStep.ChooseStarter(Town01Starters.All, StoryBalance.StarterLevel),
                    EventStep.SetFlag(Town01Flags.GotStarter),
                    EventStep.Say("その子を 大事に 育てて おくれ")),
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
                NpcPlacement.Always(new FieldSpot(Town01Maps.Village, new GridPos(15, 11), Direction.Down),
                    EventStep.Say("ハツユキ村は 一年中 雪が 降るんだ")));
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
