using System.Collections;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 自転車の速さ・大橋の下り坂・展望マスで景色を眺める間（PLAN_STORY_TOWN04.md §3・§4）
    /// </summary>
    public partial class FieldController
    {
        private static readonly string[] GazeLines = { "いい 眺めだ……" };

        [Tooltip("自転車の1マスの秒数（歩きの3倍の速さ）")]
        [SerializeField] private float _bikeStepSeconds = 0.09f;
        [Tooltip("自転車で下り坂を東へ下るときの1マスの秒数")]
        [SerializeField] private float _downhillStepSeconds = 0.07f;
        [Tooltip("自転車で下り坂を西へ上るときの1マスの秒数（歩きと同じ）")]
        [SerializeField] private float _uphillStepSeconds = 0.27f;

        // 眺めている間は、押し続けた向きを何度も数えないよう、前のフレームの向きから変わった瞬間だけ見る
        private Direction? _lastGazeHeld;

        /// <summary>自転車は走るより優先する。建物の中・草むらでも走れる（どこでも同じ速さにする）</summary>
        private float StepSeconds(Slope slope)
        {
            if (!Bicycle.IsOn(_state.Flags))
            {
                return RunningShoes.IsOn(_state.Flags) ? _runStepSeconds : _stepSeconds;
            }
            switch (slope)
            {
                case Slope.Down: return _downhillStepSeconds;
                case Slope.Up: return _uphillStepSeconds;
                default: return _bikeStepSeconds;
            }
        }

        /// <summary>自転車のボタンは、持っていて外にいるときだけ出す。乗り降りしたら主人公の絵も替える</summary>
        private void ShowBicycle()
        {
            bool isOn = Bicycle.IsOn(_state.Flags);
            _input.ShowBike(Bicycle.Has(_state.Flags) && !_player.Map.IsIndoors, isOn);
            _avatar.SetRiding(isOn);
        }

        /// <summary>展望マスに乗った: 止まって景色を眺める。自転車に乗ったまま来たときだけ隠しコマンドを数える</summary>
        private IEnumerator StartGazing()
        {
            _player.StartGazing(Bicycle.IsOn(_state.Flags));
            yield return PlayScript(EventStep.Lines(GazeLines));
            // 乗ってきた十字キーを押したままでも、1回目の向きに数えない
            _lastGazeHeld = _input.HeldDirection;
        }

        /// <summary>眺めている間の十字キー。動かずに向きだけ変え、隠しコマンドが通ったら虹の羽の子が舞い降りる（捕まえた後は何も起きない）</summary>
        private IEnumerator TurnWhileGazing(Direction? held)
        {
            bool pressed = held.HasValue && held != _lastGazeHeld;
            _lastGazeHeld = held;
            if (!pressed)
            {
                yield return null;
                yield break;
            }
            _avatar.Face(held.Value);
            if (_player.TurnWhileGazing(held.Value) && Town04People.RainbowAppears(_state.Flags))
            {
                _player.StopGazing();
                yield return PlayScript(Town04People.RainbowScript);
                yield break;
            }
            yield return null;
        }
    }
}
