using System;
using System.Collections.Generic;
using System.Linq;

namespace PenguinMonster.Battle
{
    /// <summary>1体ぶんの集計。出た試合数と、そのうち勝った試合数</summary>
    public sealed class MonsterWinRate
    {
        public MonsterDefinition Monster { get; }
        public int Battles { get; internal set; }
        public int Wins { get; internal set; }

        public float Rate => Battles == 0 ? 0f : Wins / (float)Battles;

        public MonsterWinRate(MonsterDefinition monster)
        {
            Monster = monster;
        }
    }

    /// <summary>
    /// バランス確認用（Phase 9）。ランダムな3体どうしを CPU（つよい）に大量に戦わせて、ペンモンごとの勝率を出す。
    /// ドラフトの重みを通さず全員を同じ回数ずつ出すので、伝説やヒナも同じ条件で比べられる。
    /// CPU は変化技をほぼ使わないので、能力を上げる子・羽リレーの子は実際より低く出る点に注意
    /// </summary>
    public static class BalanceSimulator
    {
        /// <summary>壁どうしで終わらない試合を打ち切るターン数。引き分けとしてどちらも勝ちに数えない</summary>
        public const int MaxTurns = 200;

        public static IReadOnlyList<MonsterWinRate> Run(IReadOnlyList<MonsterDefinition> pool, int battles, int seed)
        {
            var random = new Random(seed);
            var rates = pool.ToDictionary(m => m, m => new MonsterWinRate(m));
            for (int i = 0; i < battles; i++)
            {
                var team0 = RandomTeam(pool, random);
                var team1 = RandomTeam(pool, random);
                int winner = Play(team0, team1, random);
                Record(rates, team0, winner == 0);
                Record(rates, team1, winner == 1);
            }
            return rates.Values.ToList();
        }

        /// <summary>1試合を最後まで進めて勝った陣営を返す。打ち切りなら -1</summary>
        public static int Play(IReadOnlyList<MonsterDefinition> team0, IReadOnlyList<MonsterDefinition> team1, Random random)
        {
            var state = new BattleState(team0, team1);
            var engine = new BattleEngine(state, random);
            var cpus = new[] { new CpuPlayer(random, CpuLevel.Strong), new CpuPlayer(random, CpuLevel.Strong) };
            engine.StartBattle();
            for (int turn = 0; turn < MaxTurns && !state.IsOver; turn++)
            {
                var action0 = cpus[0].ChooseAction(state.Sides[0], state.Sides[1]);
                var action1 = cpus[1].ChooseAction(state.Sides[1], state.Sides[0]);
                engine.ResolveTurn(action0, action1);
                ReplaceFainted(engine, state, cpus);
            }
            return state.Winner;
        }

        private static void ReplaceFainted(BattleEngine engine, BattleState state, CpuPlayer[] cpus)
        {
            foreach (var side in state.Sides)
            {
                if (!state.IsOver && side.NeedsReplacement)
                {
                    engine.Replace(side.Index, cpus[side.Index].ChooseReplacement(side, state.Opponent(side.Index)));
                }
            }
        }

        private static List<MonsterDefinition> RandomTeam(IReadOnlyList<MonsterDefinition> pool, Random random)
        {
            var rest = new List<MonsterDefinition>(pool);
            var team = new List<MonsterDefinition>();
            while (team.Count < BattleBalance.PartySize)
            {
                int index = random.Next(rest.Count);
                team.Add(rest[index]);
                rest.RemoveAt(index);
            }
            return team;
        }

        private static void Record(Dictionary<MonsterDefinition, MonsterWinRate> rates, List<MonsterDefinition> team, bool won)
        {
            foreach (var monster in team)
            {
                rates[monster].Battles++;
                if (won)
                {
                    rates[monster].Wins++;
                }
            }
        }
    }
}
