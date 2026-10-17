using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>「自分のチーム」で戦えるかどうか（足りないときに出す文を選ぶため）</summary>
    public enum OnlineTeamProblem
    {
        None,
        /// <summary>手持ちが6体そろっていない</summary>
        NotEnough,
        /// <summary>伝説が2体以上（ドラフトと同じく1体まで）</summary>
        TooManyLegends,
    }

    /// <summary>
    /// オンラインの「自分のチーム」。ストーリーの手持ちの6体を、
    /// 覚えた技・持ち物はそのままに Lv50・HP 満タン・状態異常なしにして出す。手持ちの子そのものは変えない
    /// </summary>
    public static class OnlineTeam
    {
        public static OnlineTeamProblem Check(Party party)
        {
            if (party == null || party.Count < StoryBalance.MaxPartySize)
            {
                return OnlineTeamProblem.NotEnough;
            }
            int legends = 0;
            foreach (var member in party.Members)
            {
                if (member.Species.Definition.Rank == MonsterRank.Legend)
                {
                    legends++;
                }
            }
            return legends > BattleBalance.MaxLegendsPerTeam ? OnlineTeamProblem.TooManyLegends : OnlineTeamProblem.None;
        }

        /// <summary>
        /// バトルに出す6体。定義の写しで出すので、HP・状態異常は持ち越さず満タンで始まる
        /// （OwnedMonster.ToBattle は傷を持ち越すので使わない）
        /// </summary>
        public static List<MonsterDefinition> From(Party party)
        {
            var team = new List<MonsterDefinition>(party.Count);
            foreach (var member in party.Members)
            {
                var moves = new List<MoveDefinition>(member.Moves).ToArray();
                team.Add(member.Species.Definition.WithLoadout(BattleBalance.Level, moves, member.Item));
            }
            return team;
        }
    }
}
