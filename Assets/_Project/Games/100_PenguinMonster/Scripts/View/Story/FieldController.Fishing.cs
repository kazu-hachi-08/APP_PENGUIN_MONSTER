using System.Collections;
using PenguinMonster.Story;

namespace PenguinMonster.View.Story
{
    /// <summary>水に向いて A で釣る（PLAN_STORY_TOWN04.md §5）。かかったら草むらの野生と同じバトルになる</summary>
    public partial class FieldController
    {
        private IEnumerator Fish()
        {
            var result = _player.Fish(FishingRod.Has(_state.Flags), _random, out var wild);
            if (result == FishResult.None)
            {
                // 何も無い所で A を押したときも1フレーム進める（同じフレームの A をまた見て止まらないように）
                yield return null;
                yield break;
            }
            yield return _events.Fish(result);
            if (wild != null)
            {
                yield return FightWild(wild);
                yield break;
            }
            // 最後の文を送った A で、すぐまた投げないよう1フレーム空ける
            yield return null;
        }
    }
}
