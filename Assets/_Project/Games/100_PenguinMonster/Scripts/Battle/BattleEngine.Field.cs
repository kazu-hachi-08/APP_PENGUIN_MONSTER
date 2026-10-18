using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 戦況を変える技。守る・手助けはそのターンだけ、追い風は陣営に数ターンかかる。
    /// 追い風のすばやさ2倍は BattleMonster.EffectiveSpeed、手助けのダメージ倍率は DamageCalculator
    /// </summary>
    public sealed partial class BattleEngine
    {
        /// <summary>
        /// ターンの最後に、そのターンだけの守る・手助けを消す（守ったかどうかは続けて使えないよう覚えておく）。
        /// 次のターンの行動を選ぶ前に消しておけば、CPU の見込みダメージに前のターンの手助けが混ざらない
        /// </summary>
        private void EndTurnGuards()
        {
            foreach (var side in _state.Sides)
            {
                foreach (var monster in side.Party)
                {
                    monster.EndTurn();
                }
            }
        }

        /// <summary>前のターンに守りが決まっていたら失敗する</summary>
        private static void TryProtect(int side, BattleMonster user, bool reportFailure, List<BattleEvent> events)
        {
            if (user.ProtectedLastTurn)
            {
                AddFailed(side, user, reportFailure, events);
                return;
            }
            user.Protect();
            events.Add(BattleEvent.Of(BattleEventKind.ProtectStarted, side, user));
        }

        /// <summary>守っている相手には技が届かない。防いだら文を出して true</summary>
        private static bool BlockedByProtect(int side, BattleMonster target, List<BattleEvent> events)
        {
            if (!target.IsProtecting)
            {
                return false;
            }
            events.Add(BattleEvent.Of(BattleEventKind.Protected, 1 - side, target));
            return true;
        }

        /// <summary>手助け: 隣の戦える味方を強くする。シングルや味方が倒れているときは失敗</summary>
        private void TryHelpAlly(int side, BattleMonster user, bool reportFailure, List<BattleEvent> events)
        {
            var battleSide = _state.Sides[side];
            foreach (int slot in battleSide.AliveSlots())
            {
                var ally = battleSide.ActiveAt(slot);
                if (ally != user)
                {
                    ally.ReceiveHelp();
                    events.Add(BattleEvent.Of(BattleEventKind.Helped, side, ally));
                    return;
                }
            }
            AddFailed(side, user, reportFailure, events);
        }

        /// <summary>追い風: すでに吹いていれば失敗</summary>
        private void TryStartTailwind(int side, BattleMonster user, bool reportFailure, List<BattleEvent> events)
        {
            var battleSide = _state.Sides[side];
            if (battleSide.HasTailwind)
            {
                AddFailed(side, user, reportFailure, events);
                return;
            }
            battleSide.SetTailwind(BattleBalance.TailwindTurns);
            events.Add(BattleEvent.Of(BattleEventKind.TailwindStarted, side, user));
        }

        /// <summary>ターン終わりに追い風の残りを数える（天候と同じく、始めたターンも1と数える）</summary>
        private void AdvanceTailwind(List<BattleEvent> events)
        {
            foreach (var side in _state.Sides)
            {
                if (side.TickTailwind())
                {
                    events.Add(BattleEvent.Of(BattleEventKind.TailwindEnded, side.Index, null));
                }
            }
        }
    }
}
