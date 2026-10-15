using System.Collections;
using PenguinMonster.Common.Scene;
using PenguinMonster.Story;
using PenguinMonster.View.Story;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// ストーリーの流れ（PLAN_STORY_TOWN01.md）。今はフェーズ3: 仮のハツユキ村を歩き、B でタイトルへ戻る
    /// </summary>
    public partial class GameFlow
    {
        [SerializeField] private FieldController _field;
        [SerializeField] private FadeController _fade;

        private IEnumerator RunStory()
        {
            // フィールドはバトルの画面の後ろ（カメラ）に映るので、バトルの UI を隠して見えるようにする
            _battle.gameObject.SetActive(false);
            var walking = StartCoroutine(_field.Run(Town01Maps.CreateWorld(), Town01Maps.Start));
            yield return _fade.FadeInCoroutine();
            yield return walking;

            yield return _fade.FadeOutCoroutine();
            _field.Hide();
            BackToTitle();
        }
    }
}
