using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 技を出す（仕様書 §4.2 / §4.9）。対象を決めて、相手ごとに 命中 → ダメージ → 追加効果 → 特性 を順に済ませる。
    /// 相手全員に当たる技は、ダブルスで2体に当たるとダメージが 0.75 倍
    /// </summary>
    public sealed partial class BattleEngine
    {
        private void ExecuteMove(int side, int slot, BattleAction action, List<BattleEvent> events)
        {
            var user = _state.Sides[side].ActiveAt(slot);
            var targets = TargetsOf(side, user.Definition.Moves[action.Index], action.Target);
            // 先に倒されたペンモンと、相手の場に戦えるペンモンがいないときは行動しない
            if (user.IsFainted || targets.Count == 0)
            {
                return;
            }
            if (!TryAct(side, user, events))
            {
                return;
            }

            var move = user.Definition.Moves[action.Index];
            events.Add(BattleEvent.MoveUsed(side, user, move));
            if (move.TargetsSelfOnly)
            {
                ApplyStatusMove(side, user, targets[0], move, events);
            }
            else if (move.IsStatus)
            {
                if (RollHitOrMiss(side, user, targets[0], move, events))
                {
                    ApplyStatusMove(side, user, targets[0], move, events);
                }
            }
            else
            {
                ExecuteAttack(side, user, targets, move, events);
            }
        }

        /// <summary>
        /// 技が当たる相手。全体技は場の相手全員、それ以外は狙った相手（倒れていたらもう片方）。
        /// 自分にかける技も、相手がいるかどうかの判定に1体返す
        /// </summary>
        private List<BattleMonster> TargetsOf(int side, MoveDefinition move, int aimedSlot)
        {
            var foe = _state.Opponent(side);
            var alive = foe.AliveSlots();
            var targets = new List<BattleMonster>();
            if (alive.Count == 0)
            {
                return targets;
            }
            if (move.HitsAllFoes)
            {
                alive.ForEach(slot => targets.Add(foe.ActiveAt(slot)));
                return targets;
            }
            targets.Add(foe.ActiveAt(alive.Contains(aimedSlot) ? aimedSlot : alive[0]));
            return targets;
        }

        private void ExecuteAttack(int side, BattleMonster user, List<BattleMonster> targets, MoveDefinition move, List<BattleEvent> events)
        {
            float targetMultiplier = targets.Count > 1 ? BattleBalance.SpreadMultiplier : 1f;
            bool hitAny = false;
            foreach (var target in targets)
            {
                // 前の相手への攻撃で決着した・とげとげはだ で自分が倒れたら、残りの相手には当てない
                if (_state.IsOver || user.IsFainted)
                {
                    break;
                }
                if (!RollHitOrMiss(side, user, target, move, events))
                {
                    continue;
                }
                var result = DamageCalculator.Calculate(user, target, move, _random, targetMultiplier);
                ApplyDamage(1 - side, target, result, events);
                if (result.Effectiveness == 0f)
                {
                    continue;
                }
                ThawByFire(1 - side, target, move, events);
                // 自分にかかる追加効果は、何体に当たっても1回だけ
                ApplySecondaryEffects(side, user, target, move, includeSelf: !hitAny, events);
                hitAny = true;
                AfterHit(side, user, target, move, events);
            }
            if (hitAny)
            {
                ApplyLifeIcicleRecoil(side, user, events);
            }
        }

        /// <summary>命中を引き、外れたら外れた文（主語は外れた相手）を出して false</summary>
        private bool RollHitOrMiss(int side, BattleMonster user, BattleMonster target, MoveDefinition move, List<BattleEvent> events)
        {
            if (RollHit(user, target, move))
            {
                return true;
            }
            events.Add(BattleEvent.Of(BattleEventKind.Missed, 1 - side, target));
            return false;
        }

        /// <summary>命中 × 命中ランク ÷ 回避ランク（仕様書 §4.3）。ランクは差を -6〜+6 に収めて1つの倍率にする</summary>
        private bool RollHit(BattleMonster user, BattleMonster target, MoveDefinition move)
        {
            int stage = user.GetStage(StatKind.Accuracy) - target.GetStage(StatKind.Evasion);
            stage = Math.Max(-BattleBalance.MaxStatStage, Math.Min(BattleBalance.MaxStatStage, stage));
            return _random.Next(BattleBalance.AccuracyPercentMax) < StageMultiplier.ApplyAccuracy(move.Accuracy, stage);
        }
    }
}
