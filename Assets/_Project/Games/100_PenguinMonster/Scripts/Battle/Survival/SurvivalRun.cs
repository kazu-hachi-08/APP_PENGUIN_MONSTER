using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 勝ち抜き戦1回ぶん（仕様書 §2.3）。最初のドラフトの6体で、負けるまで連勝数を数える。
    /// 相手の強さ（CPU の賢さ・ドラフトの取り方・ボス）は連勝数で決める
    /// </summary>
    public sealed class SurvivalRun
    {
        private readonly Random _random;

        public IReadOnlyList<MonsterDefinition> PlayerSix { get; }
        /// <summary>シングルかダブルスか。1回の勝ち抜き戦の中では変えない</summary>
        public BattleFormat Format { get; }
        public int Wins { get; private set; }
        /// <summary>次に戦うのが何戦目か（1 から）</summary>
        public int BattleNumber => Wins + 1;
        public bool IsBossBattle => IsBoss(BattleNumber);

        public SurvivalRun(IReadOnlyList<MonsterDefinition> playerSix, Random random, BattleFormat format = BattleFormat.Single)
        {
            PlayerSix = playerSix;
            _random = random;
            Format = format;
        }

        public void RecordWin() => Wins++;

        /// <summary>
        /// 次の相手。ボスは固定のチーム、それ以外は CPU が連勝数に応じた強さでドラフトする。
        /// どちらも持ち物を1個引き、CPU はランダムな1体、ボスは切り札に持たせる
        /// </summary>
        public SurvivalOpponent NextOpponent()
        {
            var cpu = new CpuPlayer(_random, LevelFor(Wins, IsBossBattle));
            var item = ItemDraw.Draw(_random);
            if (IsBossBattle)
            {
                var boss = BossTable.ForBattle(BattleNumber);
                int trumpIndex = IndexOf(boss.Team, boss.TrumpCard);
                var bossSix = ItemDraw.Give(boss.Team, trumpIndex, item);
                return new SurvivalOpponent(bossSix, cpu, boss, bossSix[trumpIndex]);
            }
            var six = cpu.RunDraft(new Draft(MonsterTable.All, _random), StrongPickPercent(Wins));
            return new SurvivalOpponent(ItemDraw.Give(six, _random.Next(six.Count), item), cpu);
        }

        private static int IndexOf(IReadOnlyList<MonsterDefinition> team, MonsterDefinition monster)
        {
            for (int i = 0; i < team.Count; i++)
            {
                if (team[i] == monster)
                {
                    return i;
                }
            }
            throw new ArgumentException($"{monster.Name} がチームにいない", nameof(monster));
        }

        public static bool IsBoss(int battleNumber) => battleNumber % BattleBalance.BossInterval == 0;

        /// <summary>1〜3戦目 よわい、4〜6戦目 ふつう、7戦目（最初のボス）から つよい。ボスはいつも つよい</summary>
        public static CpuLevel LevelFor(int wins, bool isBoss)
        {
            if (isBoss || wins >= BattleBalance.StrongCpuFromWins)
            {
                return CpuLevel.Strong;
            }
            return wins >= BattleBalance.NormalCpuFromWins ? CpuLevel.Normal : CpuLevel.Weak;
        }

        public static int StrongPickPercent(int wins)
        {
            return Math.Min(BattleBalance.PercentMax, wins * BattleBalance.CpuStrongPickPercentPerWin);
        }
    }
}
