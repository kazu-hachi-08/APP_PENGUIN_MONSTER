namespace PenguinMonster.Battle
{
    /// <summary>
    /// BattleEvent をメッセージ窓の1文にする。文字を出さないイベント（HPの変化など）は null
    /// </summary>
    public static class BattleMessages
    {
        private const string EnemyPrefix = "あいての ";

        /// <param name="playerSide">画面の手前にいる陣営。相手側の主語に「あいての」を付けるのに使う</param>
        public static string Format(BattleEvent e, int playerSide)
        {
            bool isPlayer = e.Side == playerSide;
            string subject = isPlayer ? e.Monster?.Name : EnemyPrefix + e.Monster?.Name;

            switch (e.Kind)
            {
                case BattleEventKind.SentOut:
                    return isPlayer ? $"ゆけっ！ {e.Monster.Name}！" : $"あいては {e.Monster.Name}を くりだした！";
                case BattleEventKind.Withdrawn:
                    return isPlayer ? $"もどれ！ {e.Monster.Name}！" : $"あいては {e.Monster.Name}を ひっこめた！";
                case BattleEventKind.MoveUsed:
                    return $"{subject}の {e.MoveName}！";
                case BattleEventKind.Missed:
                    return "しかし こうげきは はずれた！";
                case BattleEventKind.Critical:
                    return "きゅうしょに あたった！";
                case BattleEventKind.SuperEffective:
                    return "こうかは ばつぐんだ！";
                case BattleEventKind.NotVeryEffective:
                    return "こうかは いまひとつの ようだ";
                case BattleEventKind.NoEffect:
                    return $"{subject}には こうかが ないようだ…";
                case BattleEventKind.Fainted:
                    return $"{subject}は たおれた！";
                case BattleEventKind.BattleEnd:
                    return isPlayer ? "しょうぶに かった！" : "しょうぶに まけた…";
                default:
                    return null;
            }
        }
    }
}
