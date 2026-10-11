using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.View
{
    public partial class BattleController
    {
        /// <summary>イベントを順に画面へ反映し、文があれば1文ずつ表示する</summary>
        private IEnumerator Play(List<BattleEvent> events)
        {
            foreach (var e in events)
            {
                ApplyToView(e);
                string message = BattleMessages.Format(e, PlayerSide);
                if (message != null)
                {
                    yield return _messageWindow.Play(message);
                }
            }
        }

        private void ApplyToView(BattleEvent e)
        {
            switch (e.Kind)
            {
                case BattleEventKind.SentOut:
                    PanelOf(e.Side).Show(e.Monster, e.Hp);
                    break;
                case BattleEventKind.Damaged:
                    PanelOf(e.Side).SetHp(e.Hp, e.Monster.MaxHp);
                    break;
                case BattleEventKind.Fainted:
                    PanelOf(e.Side).SetFainted();
                    break;
            }
        }

        private MonsterPanel PanelOf(int side) => side == PlayerSide ? _playerPanel : _enemyPanel;
    }
}
