using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    public sealed partial class BattleEngine
    {
        private readonly struct QueuedAction
        {
            public readonly int Side;
            public readonly BattleAction Action;
            public readonly int Priority;
            public readonly int Speed;
            public readonly int TieBreak;

            public QueuedAction(int side, BattleAction action, int priority, int speed, int tieBreak)
            {
                Side = side;
                Action = action;
                Priority = priority;
                Speed = speed;
                TieBreak = tieBreak;
            }

            public bool IsSwitch => Action.Kind == BattleActionKind.Switch;
        }

        /// <summary>交代 → 技の優先度 → すばやさ（ランク・まひ込み。同速はランダム）の順に並べる</summary>
        private List<QueuedAction> OrderActions(BattleAction action0, BattleAction action1)
        {
            var queue = new List<QueuedAction> { Enqueue(0, action0), Enqueue(1, action1) };
            queue.Sort(CompareOrder);
            return queue;
        }

        private QueuedAction Enqueue(int side, BattleAction action)
        {
            var active = _state.Sides[side].Active;
            int priority = action.Kind == BattleActionKind.Move ? active.Definition.Moves[action.Index].Priority : 0;
            // 同速の決着用。行動ごとに先に引いておけば、並べ替えの比較回数に乱数の消費が左右されない
            return new QueuedAction(side, action, priority, active.EffectiveSpeed, _random.Next());
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
    }
}
