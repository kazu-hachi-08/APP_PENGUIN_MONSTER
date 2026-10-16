namespace PenguinMonster.Battle
{
    /// <summary>
    /// BattleEvent をメッセージ窓の1文にする。文字を出さないイベント（HPの変化など）は null
    /// </summary>
    public static class BattleMessages
    {
        private const string EnemyPrefix = "相手の ";
        private const string WildPrefix = "野生の ";

        /// <param name="playerSide">画面の手前にいる陣営。相手側の主語に「相手の」を付けるのに使う</param>
        /// <param name="foeIsWild">ストーリーの野生との戦い。相手の主語を「野生の」にし、勝っても「勝負に 勝った！」は出さない</param>
        public static string Format(BattleEvent e, int playerSide, bool foeIsWild = false)
        {
            bool isPlayer = e.Side == playerSide;
            string subject = isPlayer ? e.Monster?.Name : (foeIsWild ? WildPrefix : EnemyPrefix) + e.Monster?.Name;

            switch (e.Kind)
            {
                case BattleEventKind.SentOut when foeIsWild && !isPlayer:
                    return $"あっ！ 野生の {e.Monster.Name}が 飛び出してきた！";
                case BattleEventKind.BattleEnd when foeIsWild:
                    return isPlayer ? null : "勝負に 負けた…";
                case BattleEventKind.SentOut:
                    return isPlayer ? $"行けっ！ {e.Monster.Name}！" : $"相手は {e.Monster.Name}を 繰り出した！";
                case BattleEventKind.Withdrawn:
                    return isPlayer ? $"戻れ！ {e.Monster.Name}！" : $"相手は {e.Monster.Name}を 引っ込めた！";
                case BattleEventKind.MoveUsed:
                    return $"{subject}の {e.MoveName}！";
                case BattleEventKind.Missed:
                    // ダブルスでは2体のうちどちらに外れたか分かるよう、外れた相手を主語にする
                    return $"しかし {subject}には 当たらなかった！";
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
                case BattleEventKind.HitCount:
                    return $"{e.Amount}回 当たった！";
                case BattleEventKind.Drained:
                    return $"{subject}は 体力を 吸い取った！";
                case BattleEventKind.Recoil:
                    return $"{subject}は 反動で ダメージを 受けた！";
                case BattleEventKind.Healed:
                    return $"{subject}の 体力が 回復した！";
                case BattleEventKind.WeatherStarted:
                    return WeatherStarted(e.Weather);
                case BattleEventKind.WeatherEnded:
                    return WeatherEnded(e.Weather);
                case BattleEventKind.WeatherDamage:
                    return $"{subject}は 吹雪に 打たれている！";
                case BattleEventKind.ProtectStarted:
                    return $"{subject}は 守りの 体勢に 入った！";
                case BattleEventKind.Protected:
                    return $"{subject}は 攻撃から 身を 守った！";
                case BattleEventKind.TailwindStarted:
                    return $"{TeamName(isPlayer)}の 背中に 追い風が 吹き始めた！";
                case BattleEventKind.TailwindEnded:
                    return $"{TeamName(isPlayer)}の 追い風が 止んだ！";
                case BattleEventKind.Helped:
                    return $"{subject}は 声援を 受けて 張り切っている！";
                default:
                    return null;
            }
        }

        private static string ItemName(BattleEvent e) => e.Monster.Definition.Item.ToJapanese();

        /// <summary>陣営全体にかかる効果（追い風）の主語</summary>
        private static string TeamName(bool isPlayer) => isPlayer ? "味方" : "相手";

        private static string WeatherStarted(Weather weather)
        {
            switch (weather)
            {
                case Weather.Snow: return "吹雪が 吹き始めた！";
                case Weather.Rain: return "大雨が 降り始めた！";
                default: return "日差しが 強くなった！";
            }
        }

        private static string WeatherEnded(Weather weather)
        {
            switch (weather)
            {
                case Weather.Snow: return "吹雪が 止んだ！";
                case Weather.Rain: return "雨が 上がった！";
                default: return "日差しが 元に 戻った！";
            }
        }

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
