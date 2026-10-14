using PenguinMonster.Battle;

namespace PenguinMonster.View
{
    /// <summary>
    /// ドラフトのカードとずかんで共通の文面（種族値・特性と持ち物）
    /// </summary>
    public static class MonsterTexts
    {
        /// <summary>種族値を1行ずつ。見出し（HP・こうげき…）はシーン側の別の Text に並べてある</summary>
        public static string StatValues(Stats s)
        {
            return $"{s.Hp}\n{s.Attack}\n{s.Defense}\n{s.SpAttack}\n{s.SpDefense}\n{s.Speed}\n{s.Total}";
        }

        /// <summary>
        /// 特性の名前と説明。持ち物を持っていればその行、ヒナなら綿毛の行も足す。
        /// 説明は小さめの文字にして名前を目立たせる
        /// </summary>
        public static string Traits(MonsterDefinition monster, int descriptionSize)
        {
            string text = Line("特性", monster.Ability.ToJapanese(), monster.Ability.Description(), descriptionSize);
            if (monster.Rank == MonsterRank.Hina)
            {
                text += "\n" + Line("ヒナ", HeldItem.ChickDown.ToJapanese(), HeldItem.ChickDown.Description(), descriptionSize);
            }
            if (monster.Item != HeldItem.None)
            {
                text += "\n" + Line("持ち物", monster.Item.ToJapanese(), monster.Item.Description(), descriptionSize);
            }
            return text;
        }

        private static string Line(string label, string name, string description, int descriptionSize)
        {
            return $"{label} <b>{name}</b>\n<size={descriptionSize}>{description}</size>";
        }
    }
}
