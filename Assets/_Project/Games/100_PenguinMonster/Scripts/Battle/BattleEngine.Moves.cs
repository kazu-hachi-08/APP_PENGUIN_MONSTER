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
            if (move.IsStatus && user.HasAbility(Ability.Prankster))
            {
                // 先に動けた理由が分かるよう、技名の前に帯を出す
                ShowAbility(side, user, events);
            }
            events.Add(BattleEvent.MoveUsed(side, user, move));
            if (move.TargetsSelfOnly)
            {
                ApplyStatusMove(side, user, targets[0], move, events);
            }
            else if (move.IsStatus)
            {
                if (!BlockedByProtect(side, targets[0], events) && RollHitOrMiss(side, user, targets[0], move, events))
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
            // 全体技でも、使った側の帯は1回だけ出す
            bool attackerShown = false;
            int totalDealt = 0;
            foreach (var target in targets)
            {
                // 前の相手への攻撃で決着した・とげとげはだ で自分が倒れたら、残りの相手には当てない
                if (_state.IsOver || user.IsFainted)
                {
                    break;
                }
                // 全体技は守っていない相手にだけ当たる（倍率は2体ぶんの 0.75 倍のまま。原作と同じ）
                if (BlockedByProtect(side, target, events) || !RollHitOrMiss(side, user, target, move, events))
                {
                    continue;
                }
                if (!Strike(side, user, target, move, targetMultiplier, ref attackerShown, events, out int dealt))
                {
                    continue;
                }
                totalDealt += dealt;
                ThawByFire(1 - side, target, move, events);
                ApplyDrainOrRecoil(side, user, move, dealt, events);
                // 自分にかかる追加効果は、何体に当たっても1回だけ
                ApplySecondaryEffects(side, user, target, move, includeSelf: !hitAny, events);
                hitAny = true;
                AfterHit(side, user, target, move, events);
            }
            if (hitAny)
            {
                ApplyLifeIcicleRecoil(side, user, events);
                ApplyShellNecklace(side, user, totalDealt, events);
            }
        }

        /// <summary>
        /// 1体にダメージを与える。連続技は回数ぶんくり返し、途中の当たりごとに受けた側の特性（とげとげ肌など）を起こす。
        /// 最後の当たりの特性は、ふつうの技と同じ順（追加効果の後）になるよう AfterHit に任せる。
        /// 効かなかったら false。dealt は実際に減ったHPの合計（吸収・反動に使う）
        /// </summary>
        private bool Strike(int side, BattleMonster user, BattleMonster target, MoveDefinition move, float targetMultiplier,
            ref bool attackerShown, List<BattleEvent> events, out int dealt)
        {
            dealt = 0;
            int hits = RollHitCount(user, move);
            int landed = 0;
            bool defenderShown = false;
            while (true)
            {
                int hpBefore = target.CurrentHp;
                var result = DamageCalculator.Calculate(user, target, move, _random, targetMultiplier);
                ShowDamageAbilities(side, user, target, move, result, ref attackerShown, ref defenderShown, events);
                // 連続技の相性の文は1回目だけ（毎回「効果は 抜群だ！」が出るとくどいため）
                ApplyDamage(1 - side, target, result, events, reportEffectiveness: landed == 0);
                if (result.Effectiveness == 0f)
                {
                    return false;
                }
                dealt += hpBefore - target.CurrentHp;
                landed++;
                if (landed >= hits || target.IsFainted || _state.IsOver)
                {
                    break;
                }
                if (move.Category == MoveCategory.Physical)
                {
                    OnHitByPhysical(side, user, 1 - side, target, events);
                }
                if (user.IsFainted || _state.IsOver)
                {
                    break;
                }
            }
            if (move.IsMultiHit)
            {
                events.Add(BattleEvent.Count(BattleEventKind.HitCount, 1 - side, target, landed));
            }
            return true;
        }

        /// <summary>当たる回数。2〜5回の技は原作と同じ確率で決め、連打名人なら最大</summary>
        private int RollHitCount(BattleMonster user, MoveDefinition move)
        {
            if (move.MinHits == move.MaxHits || user.HasAbility(Ability.SkillLink))
            {
                return move.MaxHits;
            }
            int roll = _random.Next(BattleBalance.PercentMax);
            int hits = move.MinHits;
            foreach (int percent in BattleBalance.MultiHitPercents)
            {
                roll -= percent;
                if (roll < 0 || hits >= move.MaxHits)
                {
                    break;
                }
                hits++;
            }
            return hits;
        }

        /// <summary>吸収は与えたダメージの半分を回復、反動は 1/3 を受ける（石頭なら受けない）</summary>
        private void ApplyDrainOrRecoil(int side, BattleMonster user, MoveDefinition move, int dealt, List<BattleEvent> events)
        {
            if (_state.IsOver || user.IsFainted || dealt <= 0)
            {
                return;
            }
            if (move.Drains && !user.IsFullHp)
            {
                user.Heal(Math.Max(1, dealt / BattleBalance.DrainDenominator));
                events.Add(BattleEvent.Of(BattleEventKind.Drained, side, user));
            }
            if (move.Recoils && user.HasAbility(Ability.RockHead))
            {
                // 反動が来なかった理由として帯だけ出す
                ShowAbility(side, user, events);
            }
            else if (move.Recoils)
            {
                ApplyDirectDamage(side, user, Math.Max(1, dealt / BattleBalance.RecoilDenominator), BattleEventKind.Recoil, events);
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
