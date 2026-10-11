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
        public int Wins { get; private set; }
        /// <summary>次に戦うのが何戦目か（1 から）</summary>
        public int BattleNumber => Wins + 1;
        public bool IsBossBattle => IsBoss(BattleNumber);

        public SurvivalRun(IReadOnlyList<MonsterDefinition> playerSix, Random random)
        {
            PlayerSix = playerSix;
            _random = random;
        }

        public void RecordWin() => Wins++;

        /// <summary>次の相手。ボスは固定のチーム、それ以外は CPU が連勝数に応じた強さでドラフトする</summary>
        public SurvivalOpponent NextOpponent()
        {
            var cpu = new CpuPlayer(_random, LevelFor(Wins, IsBossBattle));
            if (IsBossBattle)
            {
                var boss = BossTable.ForBattle(BattleNumber);
                return new SurvivalOpponent(boss.Team, cpu, boss);
            }
            var six = cpu.RunDraft(new Draft(MonsterTable.All, _random), StrongPickPercent(Wins));
            return new SurvivalOpponent(six, cpu);
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
