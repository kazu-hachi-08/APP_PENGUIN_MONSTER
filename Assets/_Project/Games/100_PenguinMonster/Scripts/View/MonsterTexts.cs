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

        /// <summary>特性・持ち物の名前と説明。説明は小さめの文字にして名前を目立たせる</summary>
        public static string Traits(MonsterDefinition monster, int descriptionSize)
        {
            return $"とくせい <b>{monster.Ability.ToJapanese()}</b>\n<size={descriptionSize}>{monster.Ability.Description()}</size>\n"
                + $"もちもの <b>{monster.Item.ToJapanese()}</b>\n<size={descriptionSize}>{monster.Item.Description()}</size>";
        }
    }
}
