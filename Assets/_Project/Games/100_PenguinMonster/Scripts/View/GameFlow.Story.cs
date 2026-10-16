using System.Collections;
using PenguinMonster.Common.Scene;
using PenguinMonster.Story;
using PenguinMonster.View.Story;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// ストーリーの流れ（PLAN_STORY_TOWN01.md）。今は仮のハツユキ村と試しの1番道路を歩き、話す・戦う・捕まえる・買う。メニューの「やめる」でタイトルへ戻る。
    /// 進み具合（StoryState）はまだ保存しないので、始めるたびに最初から（セーブはフェーズ7）
    /// </summary>
    public partial class GameFlow
    {
        [SerializeField] private FieldController _field;
        [SerializeField] private FadeController _fade;

        private IEnumerator RunStory()
        {
            // フィールドはバトルの画面の後ろ（カメラ）に映るので、バトルの UI を隠して見えるようにする
            _battle.gameObject.SetActive(false);
            var walking = StartCoroutine(_field.Run(Town01Maps.CreateWorld(), Town01Maps.Start, new StoryState(Town01Maps.Start), Town01People.Create()));
            yield return _fade.FadeInCoroutine();
            yield return walking;

            yield return _fade.FadeOutCoroutine();
            _field.Hide();
            BackToTitle();
        }
    }
}
