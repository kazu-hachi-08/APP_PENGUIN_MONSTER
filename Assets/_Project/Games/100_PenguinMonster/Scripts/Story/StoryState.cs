namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリー1回分の持ち物。フラグと手持ち（バッグ・お金・あずかり箱はフェーズ6、セーブはフェーズ7で足す）
    /// </summary>
    public sealed class StoryState
    {
        public StoryFlags Flags { get; } = new StoryFlags();
        public Party Party { get; } = new Party();
    }
}
