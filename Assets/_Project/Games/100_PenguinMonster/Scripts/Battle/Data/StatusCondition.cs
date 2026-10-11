namespace PenguinMonster.Battle
{
    /// <summary>
    /// 状態異常（仕様書 §4.4）。1体に1つだけで、交代しても残る。
    /// こんらんは交代で治る別枠なので、ここには入れず BattleMonster が持つ
    /// </summary>
    public enum StatusCondition
    {
        None,
        Paralysis,
        Poison,
        BadPoison,
        Burn,
        Sleep,
        Freeze,
    }

    public static class StatusConditionNames
    {
        private static readonly string[] Names =
        {
            "", "まひ", "どく", "もうどく", "やけど", "ねむり", "こおり",
        };

        public static string ToJapanese(this StatusCondition status) => Names[(int)status];
    }

    public static class StatusImmunity
    {
        /// <summary>原作と同じく、ひ はやけど・こおり はこおり・かみなり はまひにならない。特性 よふかし は ねむりにならない</summary>
        public static bool IsImmune(MonsterDefinition monster, StatusCondition status)
        {
            switch (status)
            {
                case StatusCondition.Burn: return monster.HasType(MonsterType.Fire);
                case StatusCondition.Freeze: return monster.HasType(MonsterType.Ice);
                case StatusCondition.Paralysis: return monster.HasType(MonsterType.Thunder);
                case StatusCondition.Sleep: return monster.Ability == Ability.NightOwl;
                default: return false;
            }
        }
    }
}
