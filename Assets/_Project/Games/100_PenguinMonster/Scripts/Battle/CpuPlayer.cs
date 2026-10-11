using System;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// CPU の行動（仕様書 §4.8）。賢さで技の選び方・交代を変える。
    /// ドラフトと選出は CpuPlayer.Draft.cs
    /// </summary>
    public sealed partial class CpuPlayer
    {
        private readonly Random _random;

        public CpuLevel Level { get; }

        public CpuPlayer(Random random, CpuLevel level = CpuLevel.Weak)
        {
            _random = random;
            Level = level;
        }

        public BattleAction ChooseAction(BattleSide self, BattleSide foe)
        {
            if (Level == CpuLevel.Strong && TryChooseSwitch(self, foe, out int partyIndex))
            {
                return BattleAction.SwitchTo(partyIndex);
            }
            if (Level == CpuLevel.Weak)
            {
                return BattleAction.UseMove(_random.Next(self.Active.Definition.Moves.Count));
            }
            return BattleAction.UseMove(ChooseStrongestMove(self.Active, foe.Active));
        }

        /// <summary>ひんしのときに出すペンモン。つよい は相性のいいもの、それ以外は控えの先頭</summary>
        public int ChooseReplacement(BattleSide self, BattleSide foe)
        {
            int best = -1;
            float bestScore = float.MinValue;
            for (int i = 0; i < self.Party.Count; i++)
            {
                if (!self.CanSwitchTo(i))
                {
                    continue;
                }
                if (Level != CpuLevel.Strong)
                {
                    return i;
                }
                float score = MatchupScore(self.Party[i], foe.Active);
                if (score > bestScore)
                {
                    best = i;
                    bestScore = score;
                }
            }
            return best;
        }

        /// <summary>
        /// 見込みダメージが一番大きい技。どれも 0（変化技だけ・全部効かない）ならランダム。
        /// 同じ見込みなら前の技を選ぶ（乱数を使わないので、テストで結果を決められる）
        /// </summary>
        private int ChooseStrongestMove(BattleMonster user, BattleMonster target)
        {
            int best = -1;
            float bestDamage = 0f;
            for (int i = 0; i < user.Definition.Moves.Count; i++)
            {
                float damage = DamageCalculator.Expected(user, target, user.Definition.Moves[i]);
                if (damage > bestDamage)
                {
                    best = i;
                    bestDamage = damage;
                }
            }
            return best >= 0 ? best : _random.Next(user.Definition.Moves.Count);
        }

        /// <summary>
        /// 相手に ばつぐん を取られていて、こちらは ばつぐん を取れないときだけ交代する。
        /// 交代先は今より相性がよく、交代先自身も不利でないもの（行ったり来たりを防ぐ）
        /// </summary>
        private bool TryChooseSwitch(BattleSide self, BattleSide foe, out int partyIndex)
        {
            partyIndex = -1;
            if (!IsDisadvantaged(self.Active, foe.Active))
            {
                return false;
            }
            float bestScore = MatchupScore(self.Active, foe.Active);
            for (int i = 0; i < self.Party.Count; i++)
            {
                var candidate = self.Party[i];
                if (!self.CanSwitchTo(i) || IsDisadvantaged(candidate, foe.Active))
                {
                    continue;
                }
                float score = MatchupScore(candidate, foe.Active);
                if (score > bestScore)
                {
                    partyIndex = i;
                    bestScore = score;
                }
            }
            return partyIndex >= 0;
        }

        private static bool IsDisadvantaged(BattleMonster mine, BattleMonster foe)
        {
            return BestEffectiveness(foe, mine) > 1f && BestEffectiveness(mine, foe) <= 1f;
        }

        /// <summary>タイプ相性だけで見た有利さ。こちらの通りやすさ − 相手の通りやすさ</summary>
        private static float MatchupScore(BattleMonster mine, BattleMonster foe)
        {
            return BestEffectiveness(mine, foe) - BestEffectiveness(foe, mine);
        }

        /// <summary>攻撃技のうち、一番通る相性の倍率。攻撃技が無ければ 0</summary>
        private static float BestEffectiveness(BattleMonster attacker, BattleMonster defender)
        {
            float best = 0f;
            foreach (var move in attacker.Definition.Moves)
            {
                if (!move.IsStatus)
                {
                    best = Math.Max(best, TypeChart.GetMultiplier(move.Type, defender.Definition.Types));
                }
            }
            return best;
        }
    }
}
