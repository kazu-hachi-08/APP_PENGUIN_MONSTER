using System.Collections;
using PenguinMonster.Common.Scene;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの進行役（PLAN_STORY_TOWN01.md フェーズ3）。十字キーで1マスずつ歩き、出入口では暗転してマップを移る。
    /// B を押したら Run を抜ける（今はタイトルへ戻る。フェーズ6でメニューにする）
    /// </summary>
    public class FieldController : MonoBehaviour
    {
        [SerializeField] private FieldMapView _mapView;
        [SerializeField] private FieldAvatar _avatar;
        [SerializeField] private FieldCamera _camera;
        [SerializeField] private FieldInput _input;
        [SerializeField] private FadeController _fade;
        [Tooltip("1マス歩く秒数。ダイパの歩き（約16フレーム）に合わせる")]
        [SerializeField] private float _stepSeconds = 0.27f;
        [SerializeField] private float _doorFadeSeconds = 0.25f;

        private FieldPlayer _player;

        public IEnumerator Run(FieldWorld world, FieldSpot start)
        {
            _player = new FieldPlayer(world, start);
            gameObject.SetActive(true);
            _input.gameObject.SetActive(true);
            ShowCurrentMap();

            while (!_input.IsBDown)
            {
                var direction = _input.HeldDirection;
                if (direction.HasValue)
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
            _avatar.Place(_mapView.FeetPosition(_player.Position), _player.Facing);
            _camera.Follow(_avatar.transform, _player.Map.Width, _player.Map.Height);
        }
    }
}
