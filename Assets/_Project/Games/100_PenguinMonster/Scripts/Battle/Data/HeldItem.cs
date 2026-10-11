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
            "", "ほしざかな", "おやつのイカ", "さっぱりこんぶ", "ゆうきのはね", "かしこいはね",
            "いのちのつらら", "めききメガネ", "ふんばりマフラー", "かるいはね", "ゆきのコート",
        };

        private static readonly string[] Descriptions =
        {
            "", "ターンの おわりに すこし かいふく", "HPが はんぶんで かいふく（1かい）",
            "じょうたいいじょうを なおす（1かい）", "ぶつりわざが すこし つよくなる", "とくしゅわざが すこし つよくなる",
            "わざが つよいが うつと HPが へる", "ばつぐんの わざが すこし つよくなる",
            "HPまんタンなら 1のこる（1かい）", "すばやさが すこし あがる", "ばつぐんの わざを すこし たえる",
        };

        public static string ToJapanese(this HeldItem item) => Names[(int)item];

        public static string Description(this HeldItem item) => Descriptions[(int)item];
    }
}
