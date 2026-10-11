using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// ずかんに出す技の説明（分類・威力・命中・効果）。技表から作るので、技を足しても説明を書き足さなくてよい
    /// </summary>
    public static class MoveDescriptions
    {
        private const string NoPower = "-";
        private const string PriorityText = "せんせい";
        private const string EffectSeparator = "・";

        private static readonly string[] CategoryNames = { "ぶつり", "とくしゅ", "へんか" };

        public static string CategoryName(this MoveCategory category) => CategoryNames[(int)category];

        /// <summary>例: 「ぶつり　いりょく 75　めいちゅう 100」</summary>
        public static string Numbers(this MoveDefinition move)
        {
            string power = move.IsStatus ? NoPower : move.Power.ToString();
            return $"{move.Category.CategoryName()}　いりょく {power}　めいちゅう {move.Accuracy}";
        }

        /// <summary>先制と効果をつないだ文。何も無ければ空</summary>
        public static string Effects(this MoveDefinition move)
        {
            var parts = new List<string>();
            if (move.Priority > 0)
            {
                parts.Add(PriorityText);
            }
            foreach (var effect in move.Effects)
            {
                parts.Add(Describe(effect));
            }
            return string.Join(EffectSeparator, parts);
        }

        /// <summary>
        /// 相手のタイプに対する相性の倍率（バトルの技ボタンに出す）。
        /// 変化技は相性でダメージが変わらないので、いつも等倍として扱う
        /// </summary>
        public static float Effectiveness(this MoveDefinition move, IReadOnlyList<MonsterType> defenseTypes)
        {
            return move.IsStatus ? 1f : TypeChart.GetMultiplier(move.Type, defenseTypes);
        }

        /// <summary>例: 「30%で あいてを まひに する」「じぶんの こうげきを 2だん あげる」</summary>
        public static string Describe(MoveEffect effect)
        {
            string chance = effect.Chance < MoveEffect.AlwaysPercent ? $"{effect.Chance}%で " : "";
            return chance + Action(effect);
        }

        private static string Action(MoveEffect effect)
        {
            if (effect.Status != StatusCondition.None)
            {
                return $"あいてを {effect.Status.ToJapanese()}に する";
            }
            if (effect.Confuses)
            {
                return "あいてを こんらんさせる";
            }
            string who = effect.Target == EffectTarget.Self ? "じぶん" : "あいて";
            string direction = effect.Stages > 0 ? "あげる" : "さげる";
            return $"{who}の {effect.Stat.ToJapanese()}を {System.Math.Abs(effect.Stages)}だん {direction}";
        }
    }
}
