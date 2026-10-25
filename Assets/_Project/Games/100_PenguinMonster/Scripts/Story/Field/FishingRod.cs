namespace PenguinMonster.Story
{
    /// <summary>
    /// 釣り竿。持っているかはフラグで持つ（ランニングシューズ・自転車と同じく、記録の形を変えずに済むように）
    /// </summary>
    public static class FishingRod
    {
        public static bool Has(StoryFlags flags) => flags.Has(Town04Flags.FishingRod);

        public static void Give(StoryFlags flags)
        {
            flags.Set(Town04Flags.FishingRod);
        }
    }
}
