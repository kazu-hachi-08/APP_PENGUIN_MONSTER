namespace PenguinMonster.Story
{
    /// <summary>
    /// ランニングシューズ。持っているか・走る ON かはフラグで持つ（記録の形を変えずに済むように）。
    /// 走る ON はシューズより後の区切りでも使う設定なので、頭は town03. ではなく story. にする
    /// </summary>
    public static class RunningShoes
    {
        public const string RunOnFlag = "story.runOn";

        public static bool Has(StoryFlags flags) => flags.Has(Town03Flags.RunningShoes);

        /// <summary>シューズを持っていないときは、走る ON のフラグがあっても歩く</summary>
        public static bool IsOn(StoryFlags flags) => Has(flags) && flags.Has(RunOnFlag);

        /// <summary>もらってすぐ走れるよう、走る ON も立てる</summary>
        public static void Give(StoryFlags flags)
        {
            flags.Set(Town03Flags.RunningShoes);
            flags.Set(RunOnFlag);
        }

        /// <summary>走る ON / OFF を切り替える。シューズを持っていなければ何もしない</summary>
        public static void Toggle(StoryFlags flags)
        {
            if (!Has(flags))
            {
                return;
            }
            if (flags.Has(RunOnFlag))
            {
                flags.Clear(RunOnFlag);
            }
            else
            {
                flags.Set(RunOnFlag);
            }
        }
    }
}
