using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// CPU の行動（仕様書 §4.8）。賢さで技の選び方・交代を変える。ダブルスでは場の1体ずつ選ぶ。
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

        /// <summary>シングル用。場の先頭の行動</summary>
        public BattleAction ChooseAction(BattleSide self, BattleSide foe)
        {
            return ChooseAction(self, foe, 0, new List<int>());
        }

        /// <summary>場の順の行動。倒れて空いた場は技の 0 番（使われない）。2体が同じ控えへ交代しないようにする</summary>
        public List<BattleAction> ChooseActions(BattleSide self, BattleSide foe)
        {
            var actions = new List<BattleAction>();
            var reserved = new List<int>();
            for (int slot = 0; slot < self.ActiveCount; slot++)
            {
                var action = self.ActiveAt(slot).IsFainted ? BattleAction.UseMove(0) : ChooseAction(self, foe, slot, reserved);
                if (action.Kind == BattleActionKind.Switch)
                {
                    reserved.Add(action.Index);
                }
                actions.Add(action);
            }
            return actions;
        }

        private BattleAction ChooseAction(BattleSide self, BattleSide foe, int slot, List<int> reserved)
        {
            var user = self.ActiveAt(slot);
            if (Level == CpuLevel.Strong && TryChooseSwitch(self, user, FacingFoe(foe, slot), reserved, out int partyIndex))
            {
                return BattleAction.SwitchTo(partyIndex);
            }
            if (Level == CpuLevel.Weak)
            {
                int move = _random.Next(user.Definition.Moves.Count);
                var alive = foe.AliveSlots();
                // 相手が1体なら狙いは決まっているので乱数を引かない（シングルの乱数の消費を変えないため）
                return BattleAction.UseMove(move, alive.Count > 1 ? alive[_random.Next(alive.Count)] : alive[0]);
            }
            return ChooseStrongestMove(user, foe);
        }

        /// <summary>瀕死のときに出すペンモン。つよい は相性のいいもの、それ以外は控えの先頭</summary>
        public int ChooseReplacement(BattleSide self, BattleSide foe)
        {
            var facing = FacingFoe(foe, 0);
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
                float score = MatchupScore(self.Party[i], facing);
                if (score > bestScore)
                {
                    best = i;
                    bestScore = score;
                }
            }
            return best;
        }

        /// <summary>
        /// 交代・入れ替えの相性を見る相手。向かいの場（同じ番号）が戦えればそれ、倒れていれば戦えるほう。
        /// 両方倒れていたら（入れ替えの途中）向かいの場をそのまま見る（相性はタイプだけで見るので困らない）
        /// </summary>
        private static BattleMonster FacingFoe(BattleSide foe, int slot)
        {
            var alive = foe.AliveSlots();
            int facing = Math.Min(slot, foe.ActiveCount - 1);
            if (alive.Count == 0 || alive.Contains(facing))
            {
                return foe.ActiveAt(facing);
            }
            return foe.ActiveAt(alive[0]);
        }

        /// <summary>
        /// 見込みダメージが一番大きい技と狙う相手。全体技は当たる相手全員の合計で比べる。どれも 0（変化技だけ・全部効かない）ならランダム。
        /// 同じ見込みなら前の技・前の相手を選ぶ（乱数を使わないので、テストで結果を決められる）
        /// </summary>
        private BattleAction ChooseStrongestMove(BattleMonster user, BattleSide foe)
        {
            var alive = foe.AliveSlots();
            float spread = alive.Count > 1 ? BattleBalance.SpreadMultiplier : 1f;
            int bestMove = -1;
            int bestTarget = alive[0];
            float bestDamage = 0f;
            for (int i = 0; i < user.Definition.Moves.Count; i++)
            {
                var move = user.Definition.Moves[i];
                if (move.HitsAllFoes)
                {
                    float total = 0f;
                    alive.ForEach(slot => total += DamageCalculator.Expected(user, foe.ActiveAt(slot), move, spread));
                    Keep(i, alive[0], total, ref bestMove, ref bestTarget, ref bestDamage);
                    continue;
                }
                foreach (int slot in alive)
                {
                    Keep(i, slot, DamageCalculator.Expected(user, foe.ActiveAt(slot), move), ref bestMove, ref bestTarget, ref bestDamage);
                }
            }
            return bestMove >= 0
                ? BattleAction.UseMove(bestMove, bestTarget)
                : BattleAction.UseMove(_random.Next(user.Definition.Moves.Count), bestTarget);
        }

        private static void Keep(int move, int target, float damage, ref int bestMove, ref int bestTarget, ref float bestDamage)
        {
            if (damage > bestDamage)
            {
                bestMove = move;
                bestTarget = target;
                bestDamage = damage;
            }
        }

        /// <summary>
        /// 相手に ばつぐん を取られていて、こちらは ばつぐん を取れないときだけ交代する。
        /// 交代先は今より相性がよく、交代先自身も不利でないもの（行ったり来たりを防ぐ）。もう1体が選んだ控えは選ばない
        /// </summary>
        private static bool TryChooseSwitch(BattleSide self, BattleMonster user, BattleMonster foe, List<int> reserved, out int partyIndex)
        {
            partyIndex = -1;
            if (!IsDisadvantaged(user, foe))
            {
                return false;
            }
            float bestScore = MatchupScore(user, foe);
            for (int i = 0; i < self.Party.Count; i++)
            {
                var candidate = self.Party[i];
                if (!self.CanSwitchTo(i) || reserved.Contains(i) || IsDisadvantaged(candidate, foe))
                {
                    continue;
                }
                float score = MatchupScore(candidate, foe);
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
