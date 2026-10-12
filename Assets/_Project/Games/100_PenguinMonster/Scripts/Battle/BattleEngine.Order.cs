using System.Collections.Generic;
using System.Linq;

namespace PenguinMonster.Battle
{
    public sealed partial class BattleEngine
    {
        /// <summary>場の位置（陣営と、場の何番目か）</summary>
        private readonly struct FieldPosition
        {
            public readonly int Side;
            public readonly int Slot;

            public FieldPosition(int side, int slot)
            {
                Side = side;
                Slot = slot;
            }
        }

        private readonly struct QueuedAction
        {
            public readonly int Side;
            public readonly int Slot;
            public readonly BattleAction Action;
            public readonly int Priority;
            public readonly int Speed;
            public readonly int TieBreak;

            public QueuedAction(int side, int slot, BattleAction action, int priority, int speed, int tieBreak)
            {
                Side = side;
                Slot = slot;
                Action = action;
                Priority = priority;
                Speed = speed;
                TieBreak = tieBreak;
            }

            public bool IsSwitch => Action.Kind == BattleActionKind.Switch;
        }

        /// <summary>
        /// 場の全員の行動を、交代 → 技の優先度 → すばやさ（ランク・まひ込み。同速はランダム）の順に並べる。
        /// 倒れて空いた場の行動は並べない
        /// </summary>
        private List<QueuedAction> OrderActions(IReadOnlyList<BattleAction> actions0, IReadOnlyList<BattleAction> actions1)
        {
            var queue = new List<QueuedAction>();
            EnqueueSide(0, actions0, queue);
            EnqueueSide(1, actions1, queue);
            queue.Sort(CompareOrder);
            return queue;
        }

        private void EnqueueSide(int side, IReadOnlyList<BattleAction> actions, List<QueuedAction> queue)
        {
            var battleSide = _state.Sides[side];
            for (int slot = 0; slot < battleSide.ActiveCount; slot++)
            {
                var active = battleSide.ActiveAt(slot);
                if (active.IsFainted)
                {
                    continue;
                }
                var action = actions[slot];
                int priority = action.Kind == BattleActionKind.Move ? MovePriority(active, active.Definition.Moves[action.Index]) : 0;
                // 同速の決着用。行動ごとに先に引いておけば、並べ替えの比較回数に乱数の消費が左右されない
                queue.Add(new QueuedAction(side, slot, action, priority, active.EffectiveSpeed, _random.Next()));
            }
        }

        /// <summary>ちゃっかり は変化技を先に出せる</summary>
        private static int MovePriority(BattleMonster user, MoveDefinition move)
        {
            bool prankster = move.IsStatus && user.HasAbility(Ability.Prankster);
            return move.Priority + (prankster ? BattleBalance.PranksterPriorityBonus : 0);
        }

        private static int CompareOrder(QueuedAction a, QueuedAction b)
        {
            if (a.IsSwitch != b.IsSwitch)
            {
                return a.IsSwitch ? -1 : 1;
            }
            if (a.Priority != b.Priority)
            {
                return b.Priority.CompareTo(a.Priority);
            }
            if (a.Speed != b.Speed)
            {
                return b.Speed.CompareTo(a.Speed);
            }
            return a.TieBreak.CompareTo(b.TieBreak);
        }

        /// <summary>
        /// 場の全員を すばやさ の高い順に（同速は陣営 0 → 陣営 1、場の前から）。
        /// 場に出たときの特性とターン終わりの処理に使う。倒れて空いた場も含むので、使う側で飛ばす
        /// </summary>
        private List<FieldPosition> ActivesBySpeed()
        {
            var positions = new List<FieldPosition>();
            foreach (var side in _state.Sides)
            {
                for (int slot = 0; slot < side.ActiveCount; slot++)
                {
                    positions.Add(new FieldPosition(side.Index, slot));
                }
            }
            // OrderByDescending は同じ値の並びを保つので、同速は上の順のまま残る
            return positions.OrderByDescending(p => _state.Sides[p.Side].ActiveAt(p.Slot).EffectiveSpeed).ToList();
        }
    }
}
