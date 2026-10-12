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
        /// <summary>物理技のダメージ 2 倍。種族値の低いヒナに持たせて「意外と殴れる」子にする</summary>
        HugePower,
        /// <summary>選出の味方が瀕死になっているほど攻撃技のダメージが上がる。出す順番で強さが変わる</summary>
        Avenger,
        /// <summary>反動のある技を使っても反動を受けない</summary>
        RockHead,
        /// <summary>連続技がいつも最大の回数当たる</summary>
        SkillLink,
        /// <summary>交代で引っ込むと最大HPの 1/3 回復。出し入れしながら粘る子にする</summary>
        Rest,
        /// <summary>場に出たとき 吹雪 にする</summary>
        SnowCloud,
        /// <summary>場に出たとき 大雨 にする</summary>
        RainCall,
        /// <summary>場に出たとき 日差し を強くする</summary>
        SunCall,
        /// <summary>吹雪のとき すばやさ 2 倍</summary>
        SnowSlide,
        /// <summary>大雨のとき すばやさ 2 倍</summary>
        RainSurf,
        /// <summary>日差しが強いとき すばやさ 2 倍</summary>
        Photosynthesis,
        /// <summary>威力 60 以下の技のダメージ 1.5 倍。先制技・連続技を主力にする子にする</summary>
        Technician,
        /// <summary>反動のある技のダメージ 1.2 倍（反動は受ける。石頭の逆）</summary>
        Reckless,
        /// <summary>能力のランクの変化が逆になる。自分の能力が下がる大技を撃つほど強くなる</summary>
        Contrary,
        /// <summary>ターン終わりに、ランダムな能力が2段上がり、別の能力が1段下がる</summary>
        Moody,
        /// <summary>変化技を先に出せる（優先度 +1）</summary>
        Prankster,
        /// <summary>HP が満タンのとき、受けるダメージ半分</summary>
        Multiscale,
    }

    public static class AbilityNames
    {
        private static readonly string[] Names =
        {
            "", "強面", "瀬戸際", "ふかふか脂肪", "ビリビリボディ", "毒ぬめり",
            "とげとげ肌", "夜更かし", "追い風乗り", "勢いづく", "硬い羽", "力持ち", "仇討ち",
            "石頭", "連打名人", "ひと休み", "雪雲呼び", "雨呼び", "日照り", "雪滑り", "波乗り", "光合成",
            "小技名人", "向こう見ず", "ひねくれ者", "気まぐれ", "ちゃっかり", "まっさら",
        };

        // ドラフトのカードに1行で出すので短く
        private static readonly string[] Descriptions =
        {
            "", "出たとき 相手の 攻撃を 下げる", "ピンチで タイプ一致の 技が 強い",
            "氷・火の 技の ダメージ 半分", "物理で 受けると 麻痺させることがある",
            "物理で 受けると 毒にすることがある", "物理で 受けると 相手に ダメージ",
            "眠りに ならない", "ターンの 終わりに 素早さが 上がる", "相手を 倒すと 攻撃が 上がる",
            "急所に 当たらない", "物理技の ダメージが 2倍", "味方が 倒れているほど 技が 強い",
            "技の 反動を 受けない", "連続技が いつも 5回 当たる", "引っ込むと HPが 少し 回復",
            "出たとき 吹雪に する", "出たとき 大雨に する", "出たとき 日差しを 強くする",
            "吹雪のとき 素早さ 2倍", "大雨のとき 素早さ 2倍", "日差しが 強いとき 素早さ 2倍",
            "威力60以下の 技が 1.5倍", "反動のある 技が 強い", "能力の 上がり下がりが 逆になる",
            "毎ターン 能力が 上がり 別のが 下がる", "変化技を 先に 出せる", "HP満タンなら ダメージ 半分",
        };

        public static string ToJapanese(this Ability ability) => Names[(int)ability];

        public static string Description(this Ability ability) => Descriptions[(int)ability];
    }
}
