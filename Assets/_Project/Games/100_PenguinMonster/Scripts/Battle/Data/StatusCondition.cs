namespace PenguinMonster.Battle
{
    /// <summary>
    /// 状態異常。1体に1つだけで、交代しても残る。
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
            "", "麻痺", "毒", "猛毒", "火傷", "眠り", "氷",
        };

        public static string ToJapanese(this StatusCondition status) => Names[(int)status];
    }

    public static class StatusImmunity
    {
        /// <summary>原作と同じく、ひタイプは やけどに、こおりタイプは こおりに、かみなりタイプは まひに ならない。特性 よふかし は ねむりにならない</summary>
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
