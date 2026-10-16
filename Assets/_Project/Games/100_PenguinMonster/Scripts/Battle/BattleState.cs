using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// バトル全体の状態。陣営 0 と 1 を持つ
    /// </summary>
    public sealed class BattleState
    {
        public const int SideCount = 2;

        public IReadOnlyList<BattleSide> Sides { get; }
        public bool IsOver { get; private set; }
        /// <summary>勝った陣営。決着前は -1</summary>
        public int Winner { get; private set; } = -1;
        /// <summary>場の天候（仕様書 §4.10）。場全体で1つ</summary>
        public Weather Weather { get; private set; }
        /// <summary>天候が止むまでの残りターン</summary>
        public int WeatherTurns { get; private set; }

        public BattleState(IReadOnlyList<MonsterDefinition> party0, IReadOnlyList<MonsterDefinition> party1,
            int activeCount = BattleBalance.SingleActiveCount)
        {
            Sides = new[]
            {
                new BattleSide(0, party0, activeCount) { State = this },
                new BattleSide(1, party1, activeCount) { State = this },
            };
        }

        /// <summary>
        /// 途中の HP・状態異常から始めるバトル（ストーリー）。手持ちの写しをそのまま使い、瀕死の子は場に出さない
        /// </summary>
        public BattleState(IReadOnlyList<BattleMonster> party0, IReadOnlyList<BattleMonster> party1)
        {
            Sides = new[]
            {
                new BattleSide(0, party0, BattleBalance.SingleActiveCount) { State = this },
                new BattleSide(1, party1, BattleBalance.SingleActiveCount) { State = this },
            };
        }

        public BattleSide Opponent(int side) => Sides[1 - side];

        internal void Finish(int winner)
        {
            IsOver = true;
            Winner = winner;
        }

        /// <summary>天候を変える。None なら止む（オンラインの写しもこれで合わせる）</summary>
        internal void SetWeather(Weather weather, int turns)
        {
            Weather = weather;
            WeatherTurns = weather == Weather.None ? 0 : turns;
        }

        /// <summary>天候の残りを1減らす。止んだら true（天候は None に戻る）</summary>
        internal bool TickWeather()
        {
            if (Weather == Weather.None || --WeatherTurns > 0)
            {
                return false;
            }
            Weather = Weather.None;
            return true;
        }
    }
}
