namespace PenguinMonster.Story
{
    /// <summary>最初の章のフラグ。名前は頭に town01. を付けて、後の章のフラグと重ならないようにする</summary>
    public static class Town01Flags
    {
        /// <summary>博士から最初の1体をもらった</summary>
        public const string GotStarter = "town01.gotStarter";
        /// <summary>選んだ最初の1体（後ろに No が付く）。ライバルが相性のいい子を選ぶのに使う</summary>
        public const string StarterPrefix = "town01.starter.";
        /// <summary>2番道路の奥の出来事を見た。道場の前の門下生がどく</summary>
        public const string Route02Cleared = "town01.route02Cleared";
        /// <summary>道路の落とし物を拾った（後ろに落とし物の名前が付く）</summary>
        public const string ItemPrefix = "town01.item.";
    }
}
