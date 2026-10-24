namespace PenguinMonster.Story
{
    /// <summary>
    /// 自転車。持っているか・乗っているかはフラグで持つ（ランニングシューズと同じく、記録の形を変えずに済むように）。
    /// 乗れるのは外のマップだけ。乗っている間は走る ON より自転車を優先する（速さを決めるのは View）
    /// </summary>
    public static class Bicycle
    {
        public const string OnFlag = "story.bikeOn";

        public static bool Has(StoryFlags flags) => flags.Has(Town04Flags.Bicycle);

        /// <summary>自転車を持っていないときは、乗っているフラグがあっても歩く</summary>
        public static bool IsOn(StoryFlags flags) => Has(flags) && flags.Has(OnFlag);

        /// <summary>もらうのは自転車屋（建物の中）なので、もらった時点では乗らない</summary>
        public static void Give(StoryFlags flags)
        {
            flags.Set(Town04Flags.Bicycle);
        }

        /// <summary>乗る / 降りるを切り替える。持っていない・建物の中で乗ろうとしたときは何もしない</summary>
        public static void Toggle(StoryFlags flags, FieldMap map)
        {
            if (!Has(flags))
            {
                return;
            }
            if (flags.Has(OnFlag))
            {
                flags.Clear(OnFlag);
            }
            else if (!map.IsIndoors)
            {
                flags.Set(OnFlag);
            }
        }

        /// <summary>建物の中に入ったら降りる（扉をくぐったとき・建物の中の記録を読み直したとき）</summary>
        public static void GetOffIfIndoors(StoryFlags flags, FieldMap map)
        {
            if (map.IsIndoors)
            {
                flags.Clear(OnFlag);
            }
        }
    }
}
