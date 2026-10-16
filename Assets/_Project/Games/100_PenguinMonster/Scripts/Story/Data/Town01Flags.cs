namespace PenguinMonster.Story
{
    /// <summary>区切り1のフラグ。名前は区切りの頭を付けて、後の区切りと重ならないようにする</summary>
    public static class Town01Flags
    {
        /// <summary>博士から最初の1体をもらった</summary>
        public const string GotStarter = "town01.gotStarter";
        /// <summary>選んだ最初の1体（後ろに No が付く）。ライバルが相性のいい子を選ぶのに使う</summary>
        public const string StarterPrefix = "town01.starter.";
    }
}
