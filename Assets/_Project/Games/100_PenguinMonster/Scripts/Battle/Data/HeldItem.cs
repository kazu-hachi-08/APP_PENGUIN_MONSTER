namespace PenguinMonster.Battle
{
    /// <summary>
    /// 持ち物（仕様書 §4.6）。ペンモンごとに1つ固定。「1回だけ」のものは使うとそのバトル中は無くなる（試合ごとに戻る）。
    /// 効果の中身は BattleEngine.Items.cs と DamageCalculator（ダメージの倍率）・BattleMonster（すばやさ）
    /// </summary>
    public enum HeldItem
    {
        /// <summary>テスト用のペンモンなど、持ち物なし</summary>
        None,
        /// <summary>ターン終わりに最大HPの 1/16 回復</summary>
        StarFish,
        /// <summary>HPが半分以下になったら最大HPの 1/4 回復（1回だけ）</summary>
        SquidSnack,
        /// <summary>状態異常になったらすぐ治す（1回だけ）</summary>
        FreshKelp,
        /// <summary>物理技のダメージ 1.2 倍</summary>
        BraveFeather,
        /// <summary>特殊技のダメージ 1.2 倍</summary>
        WiseFeather,
        /// <summary>攻撃技のダメージ 1.3 倍。当てるたびに最大HPの 1/10 減る</summary>
        LifeIcicle,
        /// <summary>ばつぐんの技のダメージ 1.2 倍</summary>
        SharpGlasses,
        /// <summary>HPが満タンのとき、たおれる攻撃を受けても HP1 で残る（1回だけ）</summary>
        GritScarf,
        /// <summary>すばやさ 1.2 倍</summary>
        LightFeather,
        /// <summary>ばつぐんの技で受けるダメージ 0.75 倍</summary>
        SnowCoat,
    }

    public static class HeldItemNames
    {
        private static readonly string[] Names =
        {
            "", "干し魚", "おやつのイカ", "さっぱり昆布", "勇気の羽", "賢い羽",
            "命のつらら", "目利きメガネ", "踏ん張りマフラー", "軽い羽", "雪のコート",
        };

        private static readonly string[] Descriptions =
        {
            "", "ターンの 終わりに 少し 回復", "HPが 半分で 回復（1回）",
            "状態異常を 治す（1回）", "物理技が 少し 強くなる", "特殊技が 少し 強くなる",
            "技が 強いが 撃つと HPが 減る", "抜群の 技が 少し 強くなる",
            "HP満タンなら 1残る（1回）", "素早さが 少し 上がる", "抜群の 技を 少し 耐える",
        };

        public static string ToJapanese(this HeldItem item) => Names[(int)item];

        public static string Description(this HeldItem item) => Descriptions[(int)item];
    }
}
