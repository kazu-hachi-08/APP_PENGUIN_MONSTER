using PenguinMonster.Battle;

namespace PenguinMonster.View
{
    /// <summary>
    /// ドラフトのカード・ずかん・バトルの交代画面で共通の文面（種族値・特性と持ち物・技）
    /// </summary>
    public static class MonsterTexts
    {
        private const string StatusMoveText = "変化";
        private const string HasEffectsMark = "　＋効果";
        private const string TraitInlineSeparator = "　";
        private const string ItemUsedMark = "（使った）";

        /// <summary>種族値を1行ずつ。見出し（HP・こうげき…）はシーン側の別の Text に並べてある</summary>
        public static string StatValues(Stats s)
        {
            return $"{s.Hp}\n{s.Attack}\n{s.Defense}\n{s.SpAttack}\n{s.SpDefense}\n{s.Speed}\n{s.Total}";
        }

        /// <summary>
        /// 特性の名前と説明。持ち物を持っていればその行、ヒナなら綿毛の行も足す。
        /// 説明は小さめの文字にして名前を目立たせる
        /// </summary>
        /// <param name="oneLine">名前と説明を同じ行に並べる（横に広く縦に狭い欄で、ヒナの行が増えても枠に収めるため）</param>
        /// <param name="itemUsed">バトル中に1回だけの持ち物を使った。持っているように見せないよう印を付ける</param>
        public static string Traits(MonsterDefinition monster, int descriptionSize, bool oneLine = false, bool itemUsed = false)
        {
            string separator = oneLine ? TraitInlineSeparator : "\n";
            string text = Line("特性", monster.Ability.ToJapanese(), monster.Ability.Description(), descriptionSize, separator);
            if (monster.Rank == MonsterRank.Hina)
            {
                text += "\n" + Line("ヒナ", HeldItem.ChickDown.ToJapanese(), HeldItem.ChickDown.Description(), descriptionSize, separator);
            }
            if (monster.Item != HeldItem.None)
            {
                string itemName = monster.Item.ToJapanese() + (itemUsed ? ItemUsedMark : "");
                text += "\n" + Line("持ち物", itemName, monster.Item.Description(), descriptionSize, separator);
            }
            return text;
        }

        /// <summary>技の 分類・威力（変化技は「変化」）と「＋効果」の印。タイプはマスの色で見せるときに使う</summary>
        public static string MoveDetail(MoveDefinition move)
        {
            if (move.IsStatus) return StatusMoveText;
            string mark = move.Effects().Length > 0 ? HasEffectsMark : "";
            return $"{move.Category.CategoryName()} 威力 {move.Power}{mark}";
        }

        private static string Line(string label, string name, string description, int descriptionSize, string separator)
        {
            return $"{label} <b>{name}</b>{separator}<size={descriptionSize}>{description}</size>";
        }
    }
}
