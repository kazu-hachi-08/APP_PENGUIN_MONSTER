namespace PenguinMonster.Battle
{
    /// <summary>
    /// 特性（仕様書 §4.6）。ペンモンごとに1つ固定。名前は原作と被らないオリジナル（§1.2）。
    /// 効果の中身は BattleEngine.Abilities.cs（場に出たとき・攻撃を受けたとき・ターン終わり）と DamageCalculator（ダメージの倍率）
    /// </summary>
    public enum Ability
    {
        /// <summary>テスト用のペンモンなど、特性なし</summary>
        None,
        /// <summary>場に出たとき、相手の こうげき を1段下げる</summary>
        Intimidate,
        /// <summary>HPが 1/3 以下のとき、タイプ一致の技の威力 1.5 倍</summary>
        LastStand,
        /// <summary>こおり・ひ の技で受けるダメージが半分</summary>
        ThickFat,
        /// <summary>物理技を受けたら 30% で相手を まひ</summary>
        StaticBody,
        /// <summary>物理技を受けたら 30% で相手を どく</summary>
        PoisonSlime,
        /// <summary>物理技を受けたら、相手に相手の最大HPの 1/8 のダメージ</summary>
        SpikySkin,
        /// <summary>ねむり にならない</summary>
        NightOwl,
        /// <summary>ターン終わりに すばやさ が1段上がる</summary>
        Tailwind,
        /// <summary>相手をたおしたら こうげき が1段上がる</summary>
        Momentum,
        /// <summary>急所に当たらない</summary>
        HardFeathers,
    }

    public static class AbilityNames
    {
        private static readonly string[] Names =
        {
            "", "こわもて", "せとぎわ", "ふかふかしぼう", "ビリビリボディ", "どくぬめり",
            "とげとげはだ", "よふかし", "おいかぜのり", "いきおいづく", "かたいはね",
        };

        // ドラフトのカードに1行で出すので短く
        private static readonly string[] Descriptions =
        {
            "", "でたとき あいての こうげきを さげる", "ピンチで タイプいっちの わざが つよい",
            "こおり・ひの わざの ダメージ はんぶん", "ぶつりで うけると まひさせることがある",
            "ぶつりで うけると どくにすることがある", "ぶつりで うけると あいてに ダメージ",
            "ねむりに ならない", "ターンの おわりに すばやさが あがる", "あいてを たおすと こうげきが あがる",
            "きゅうしょに あたらない",
        };

        public static string ToJapanese(this Ability ability) => Names[(int)ability];

        public static string Description(this Ability ability) => Descriptions[(int)ability];
    }
}
