using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Common.Scene;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの進行役（PLAN_STORY_TOWN01.md フェーズ3・4）。十字キーで1マスずつ歩き、出入口では暗転してマップを移る。
    /// A で向いている先の人・看板を調べ、台本を FieldEventPlayer で見せる。
    /// B を押したら Run を抜ける（今はタイトルへ戻る。フェーズ6でメニューにする）
    /// </summary>
    public class FieldController : MonoBehaviour
    {
        [SerializeField] private FieldMapView _mapView;
        [SerializeField] private FieldAvatar _avatar;
        [SerializeField] private FieldPeopleView _peopleView;
        [SerializeField] private FieldEventPlayer _events;
        [SerializeField] private FieldCamera _camera;
        [SerializeField] private FieldInput _input;
        [SerializeField] private FadeController _fade;
        [Tooltip("1マス歩く秒数。ダイパの歩き（約16フレーム）に合わせる")]
        [SerializeField] private float _stepSeconds = 0.27f;
        [SerializeField] private float _doorFadeSeconds = 0.25f;

        private FieldPlayer _player;
        private FieldPeople _people;
        private StoryState _state;

        public IEnumerator Run(FieldWorld world, FieldSpot start, StoryState state, IReadOnlyList<FieldNpc> npcs)
        {
            _state = state;
            _people = new FieldPeople(npcs, state.Flags);
            _player = new FieldPlayer(world, start, _people);
            gameObject.SetActive(true);
            _input.gameObject.SetActive(true);
            ShowCurrentMap();

            while (!_input.IsBDown)
            {
                var direction = _input.HeldDirection;
                if (_input.IsADown)
                {
                    yield return Inspect();
                }
                else if (direction.HasValue)
                {
                    yield return Step(direction.Value);
                }
                else
                {
                    yield return null;
                }
            }
        }

        /// <summary>フィールドを片付ける。暗転し終えてから呼ぶ（景色が消えるのを見せない）</summary>
        public void Hide()
        {
            _camera.Stop();
            _input.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private IEnumerator Step(Direction direction)
        {
            if (_player.Step(direction) == StepResult.Blocked)
            {
                // 壁に向かって押している間は向きだけ変えて、次のフレームでまた入力を見る
                _avatar.Face(direction);
                yield return null;
                yield break;
            }

            yield return _avatar.Walk(_mapView.FeetPosition(_player.Position), direction, _stepSeconds);
            var warp = _player.WarpHere;
            if (warp != null)
            {
                yield return Enter(warp);
            }
        }

        /// <summary>向いている先の人・看板の台本を流す。人は話しかけた側を向く</summary>
        private IEnumerator Inspect()
        {
            var script = _player.ScriptAhead();
            if (script == null)
            {
                // 何も無い所で A を押したときも1フレーム進める（同じフレームの A をまた見て止まらないように）
                yield return null;
                yield break;
            }
            var person = _player.PersonAhead;
            if (person != null)
            {
                _peopleView.Face(person.Value.Npc, _player.Facing.Opposite());
            }
            yield return _events.Play(new EventRunner(_state, script));
            // フラグで人が動いたかもしれないので並べ直す
            DrawPeople();
            // 最後の文を送った A で、すぐまた話しかけないよう1フレーム空ける
            yield return null;
        }

        private IEnumerator Enter(Warp warp)
        {
            yield return _fade.FadeOutCoroutine(_doorFadeSeconds);
            _player.Enter(warp);
            ShowCurrentMap();
            yield return _fade.FadeInCoroutine(_doorFadeSeconds);
        }

        private void ShowCurrentMap()
        {
            _mapView.Draw(_player.Map);
            DrawPeople();
            _avatar.Place(_mapView.FeetPosition(_player.Position), _player.Facing);
            _camera.Follow(_avatar.transform, _player.Map.Width, _player.Map.Height);
        }

        private void DrawPeople()
        {
            _peopleView.Draw(_people.On(_player.Map.Id), _mapView);
        }
    }
}
