namespace PenguinMonster.Battle
{
    /// <summary>
    /// BattleEvent をメッセージ窓の1文にする。文字を出さないイベント（HPの変化など）は null
    /// </summary>
    public static class BattleMessages
    {
        private const string EnemyPrefix = "相手の ";

        /// <param name="playerSide">画面の手前にいる陣営。相手側の主語に「相手の」を付けるのに使う</param>
        public static string Format(BattleEvent e, int playerSide)
        {
            bool isPlayer = e.Side == playerSide;
            string subject = isPlayer ? e.Monster?.Name : EnemyPrefix + e.Monster?.Name;

            switch (e.Kind)
            {
                case BattleEventKind.SentOut:
                    return isPlayer ? $"行けっ！ {e.Monster.Name}！" : $"相手は {e.Monster.Name}を 繰り出した！";
                case BattleEventKind.Withdrawn:
                    return isPlayer ? $"戻れ！ {e.Monster.Name}！" : $"相手は {e.Monster.Name}を 引っ込めた！";
                case BattleEventKind.MoveUsed:
                    return $"{subject}の {e.MoveName}！";
                case BattleEventKind.Missed:
                    return "しかし 攻撃は 外れた！";
                case BattleEventKind.Critical:
                    return "急所に 当たった！";
                case BattleEventKind.SuperEffective:
                    return "効果は 抜群だ！";
                case BattleEventKind.NotVeryEffective:
                    return "効果は 今ひとつの ようだ";
                case BattleEventKind.NoEffect:
                    return $"{subject}には 効果が ないようだ…";
                case BattleEventKind.Fainted:
                    return $"{subject}は 倒れた！";
                case BattleEventKind.BattleEnd:
                    return isPlayer ? "勝負に 勝った！" : "勝負に 負けた…";
                case BattleEventKind.Failed:
                    return "しかし うまく 決まらなかった！";
                case BattleEventKind.StatusInflicted:
                    return $"{subject}は {Inflicted(e.Status)}";
                case BattleEventKind.Immobilized:
                    return $"{subject}は {Immobilized(e.Status)}";
                case BattleEventKind.WokeUp:
                    return $"{subject}は 目を 覚ました！";
                case BattleEventKind.Thawed:
                    return $"{subject}の 氷が 溶けた！";
                case BattleEventKind.Confused:
                    return $"{subject}は 混乱した！";
                case BattleEventKind.ConfusionCheck:
                    return $"{subject}は 混乱している！";
                case BattleEventKind.HurtItself:
                    return "訳も 分からず 自分を 攻撃した！";
                case BattleEventKind.ConfusionEnded:
                    return $"{subject}の 混乱が 解けた！";
                case BattleEventKind.StatusDamage:
                    return $"{subject}は {e.Status.ToJapanese()}の ダメージを 受けている！";
                case BattleEventKind.StatChanged:
                    return $"{subject}の {e.Stat.ToJapanese()}が {StatChanged(e.Amount)}";
                case BattleEventKind.StatUnchanged:
                    return $"{subject}の {e.Stat.ToJapanese()}は もう {(e.Amount > 0 ? "上がらない！" : "下がらない！")}";
                case BattleEventKind.AbilityActivated:
                    return $"{subject}の {e.Monster.Definition.Ability.ToJapanese()}！";
                case BattleEventKind.AbilityDamage:
                    return $"{subject}は 傷ついた！";
                case BattleEventKind.ItemHealed:
                    return $"{subject}は {ItemName(e)}で HPを 回復した！";
                case BattleEventKind.ItemCured:
                    return $"{subject}は {ItemName(e)}で 元気に なった！";
                case BattleEventKind.ItemEndured:
                    return $"{subject}は {ItemName(e)}で 持ちこたえた！";
                case BattleEventKind.ItemRecoil:
                    return $"{subject}は {ItemName(e)}で 少し HPが 減った！";
                default:
                    return null;
            }
        }

        private static string ItemName(BattleEvent e) => e.Monster.Definition.Item.ToJapanese();

        private static string Inflicted(StatusCondition status)
        {
            switch (status)
            {
                case StatusCondition.Paralysis: return "麻痺して 技が 出にくくなった！";
                case StatusCondition.Poison: return "毒を 浴びた！";
                case StatusCondition.BadPoison: return "猛毒を 浴びた！";
                case StatusCondition.Burn: return "火傷を 負った！";
                case StatusCondition.Sleep: return "眠ってしまった！";
                default: return "凍りついた！";
            }
        }

        private static string Immobilized(StatusCondition status)
        {
            switch (status)
            {
                case StatusCondition.Paralysis: return "体が 痺れて 動けない！";
                case StatusCondition.Sleep: return "ぐうぐう 眠っている";
                default: return "凍ってしまって 動かない！";
            }
        }

        /// <summary>原作と同じく、変わった量で「ぐーんと」「がくっと」を付ける</summary>
        private static string StatChanged(int amount)
        {
            switch (amount)
            {
                case 1: return "上がった！";
                case 2: return "ぐーんと 上がった！";
                case -1: return "下がった！";
                case -2: return "がくっと 下がった！";
                default: return amount > 0 ? "ぐぐーんと 上がった！" : "がくーんと 下がった！";
            }
        }
    }
}
