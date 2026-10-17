using System.Collections;
using PenguinMonster.Common.Scene;
using PenguinMonster.Story;
using PenguinMonster.View.Story;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// ストーリーの流れ。ハツユキ村 〜 ツララ町の道場を歩き、話す・戦う・捕まえる・買う。メニューの「やめる」でタイトルへ戻る。
    /// 記録があれば「続きから / 最初から」を聞き、続きからなら記録した場所から歩き始める。最初からなら家で目覚める文を出す
    /// </summary>
    public partial class GameFlow
    {
        [SerializeField] private FieldController _field;
        [SerializeField] private FadeController _fade;
        [SerializeField] private StoryStartPanel _storyStartPanel;

        private IEnumerator RunStory()
        {
            RestoredStory saved = StorySaveStore.Load();
            if (saved != null)
            {
                bool? continueSaved = null;
                _storyStartPanel.Show(chosen => continueSaved = chosen, onBack: BackToTitle);
                yield return new WaitUntil(() => continueSaved.HasValue);
                if (!continueSaved.Value)
                {
                    saved = null;
                }
            }
            var start = saved?.Position ?? Town01Maps.Start;
            var state = saved?.State ?? new StoryState(Town01Maps.Start);

            // フィールドはバトルの画面の後ろ（カメラ）に映るので、バトルの UI を隠して見えるようにする
            _battle.gameObject.SetActive(false);
            var opening = saved == null ? Town01People.Opening : null;
            var walking = StartCoroutine(_field.Run(Town01Maps.CreateWorld(), start, state, Town01People.Create(), opening));
            yield return _fade.FadeInCoroutine();
            yield return walking;

            yield return _fade.FadeOutCoroutine();
            _field.Hide();
            BackToTitle();
        }
    }
}
