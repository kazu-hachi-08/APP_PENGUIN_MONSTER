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
        /// 種族値から Lv50 の実数値を出す（仕様書 §3.2）。
        /// 育成が無いので、個体値31・努力値0 相当の固定値を足すだけでよい
        /// </summary>
        public static Stats FromBase(Stats baseStats)
        {
            return new Stats(
                baseStats.Hp + BattleBalance.HpBonus,
                baseStats.Attack + BattleBalance.StatBonus,
                baseStats.Defense + BattleBalance.StatBonus,
                baseStats.SpAttack + BattleBalance.StatBonus,
                baseStats.SpDefense + BattleBalance.StatBonus,
                baseStats.Speed + BattleBalance.StatBonus);
        }
    }
}
