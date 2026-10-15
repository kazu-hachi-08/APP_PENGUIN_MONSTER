namespace PenguinMonster.Battle
{
    /// <summary>
    /// 能力値6つ。種族値と実数値の両方をこの型で持つ
    /// </summary>
    public readonly struct Stats
    {
        public readonly int Hp;
        public readonly int Attack;
        public readonly int Defense;
        public readonly int SpAttack;
        public readonly int SpDefense;
        public readonly int Speed;

        /// <summary>6つの合計。種族値の合計は強さの目安に使う（ドラフトのカード・CPU のドラフト）</summary>
        public int Total => Hp + Attack + Defense + SpAttack + SpDefense + Speed;

        public Stats(int hp, int attack, int defense, int spAttack, int spDefense, int speed)
        {
            Hp = hp;
            Attack = attack;
            Defense = defense;
            SpAttack = spAttack;
            SpDefense = spDefense;
            Speed = speed;
        }

        /// <summary>
        /// 種族値とレベルから実数値を出す（仕様書 §3.2）。
        /// 努力値の育成は無いので、本家の式の個体値31・努力値0 で固定する。Lv50 なら今までの値（種族値＋75 / ＋20）と同じ
        /// </summary>
        public static Stats FromBase(Stats baseStats, int level = BattleBalance.Level)
        {
            return new Stats(
                LevelPart(baseStats.Hp, level) + level + BattleBalance.HpFlatBonus,
                OtherStat(baseStats.Attack, level),
                OtherStat(baseStats.Defense, level),
                OtherStat(baseStats.SpAttack, level),
                OtherStat(baseStats.SpDefense, level),
                OtherStat(baseStats.Speed, level));
        }

        private static int OtherStat(int baseStat, int level) => LevelPart(baseStat, level) + BattleBalance.StatFlatBonus;

        /// <summary>⌊(2×種族値＋個体値)×Lv÷100⌋。HP とほかで共通の部分</summary>
        private static int LevelPart(int baseStat, int level)
        {
            return (2 * baseStat + BattleBalance.IndividualValue) * level / BattleBalance.StatLevelDivisor;
        }
    }
}
