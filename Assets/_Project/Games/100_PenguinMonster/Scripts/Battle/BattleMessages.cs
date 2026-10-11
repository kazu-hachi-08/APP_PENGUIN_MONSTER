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
                case BattleEventKind.Failed:
                    return "しかし うまく きまらなかった！";
                case BattleEventKind.StatusInflicted:
                    return $"{subject}は {Inflicted(e.Status)}";
                case BattleEventKind.Immobilized:
                    return $"{subject}は {Immobilized(e.Status)}";
                case BattleEventKind.WokeUp:
                    return $"{subject}は めを さました！";
                case BattleEventKind.Thawed:
                    return $"{subject}の こおりが とけた！";
                case BattleEventKind.Confused:
                    return $"{subject}は こんらんした！";
                case BattleEventKind.ConfusionCheck:
                    return $"{subject}は こんらんしている！";
                case BattleEventKind.HurtItself:
                    return "わけも わからず じぶんを こうげきした！";
                case BattleEventKind.ConfusionEnded:
                    return $"{subject}の こんらんが とけた！";
                case BattleEventKind.StatusDamage:
                    return $"{subject}は {e.Status.ToJapanese()}の ダメージを うけている！";
                case BattleEventKind.StatChanged:
                    return $"{subject}の {e.Stat.ToJapanese()}が {StatChanged(e.Amount)}";
                case BattleEventKind.StatUnchanged:
                    return $"{subject}の {e.Stat.ToJapanese()}は もう {(e.Amount > 0 ? "あがらない！" : "さがらない！")}";
                default:
                    return null;
            }
        }

        private static string Inflicted(StatusCondition status)
        {
            switch (status)
            {
                case StatusCondition.Paralysis: return "まひして わざが でにくくなった！";
                case StatusCondition.Poison: return "どくを あびた！";
                case StatusCondition.BadPoison: return "もうどくを あびた！";
                case StatusCondition.Burn: return "やけどを おった！";
                case StatusCondition.Sleep: return "ねむってしまった！";
                default: return "こおりついた！";
            }
        }

        private static string Immobilized(StatusCondition status)
        {
            switch (status)
            {
                case StatusCondition.Paralysis: return "からだが しびれて うごけない！";
                case StatusCondition.Sleep: return "ぐうぐう ねむっている";
                default: return "こおってしまって うごかない！";
            }
        }

        /// <summary>原作と同じく、変わった量で「ぐーんと」「がくっと」を付ける</summary>
        private static string StatChanged(int amount)
        {
            switch (amount)
            {
                case 1: return "あがった！";
                case 2: return "ぐーんと あがった！";
                case -1: return "さがった！";
                case -2: return "がくっと さがった！";
                default: return amount > 0 ? "ぐぐーんと あがった！" : "がくーんと さがった！";
            }
        }
    }
}
