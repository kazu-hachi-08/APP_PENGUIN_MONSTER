using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Common.Scene;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドの進行役。十字キーで1マスずつ歩き、出入口では暗転してマップを移る。
    /// A で向いている先の人・看板を調べ、台本を FieldEventPlayer で見せる。草むらでは1歩ごとに野生に出会い、StoryBattleScreen で戦う。
    /// 全滅したら最後に回復した場所へ戻る。B でメニュー（FieldMenu）を開き、「やめる」で はい と答えたら Run を抜ける（タイトルへ戻る）。
    /// マップを移ったとき・野生とのバトルの後・台本の後に自動で記録する（アプリを閉じられても進み具合を失わないように）
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
        [SerializeField] private StoryBattleScreen _battles;
        [SerializeField] private FieldMenu _menu;
        [Tooltip("1マス歩く秒数。ダイパの歩き（約16フレーム）に合わせる")]
        [SerializeField] private float _stepSeconds = 0.27f;
        [Tooltip("走る ON のときの1マスの秒数（歩きの2倍の速さ）")]
        [SerializeField] private float _runStepSeconds = 0.135f;
        [SerializeField] private float _doorFadeSeconds = 0.25f;
        [Tooltip("トレーナーと目が合ったとき「！」を出しておく秒数")]
        [SerializeField] private float _exclaimSeconds = 0.6f;

        private FieldPlayer _player;
        private FieldPeople _people;
        private StoryState _state;
        private readonly System.Random _random = new System.Random();

        /// <param name="opening">歩き出す前に流す台本（最初から始めたときの目覚め）。無ければ null</param>
        public IEnumerator Run(FieldWorld world, FieldSpot start, StoryState state, IReadOnlyList<FieldNpc> npcs,
            IReadOnlyList<EventStep> opening = null)
        {
            _state = state;
            _people = new FieldPeople(npcs, state.Flags);
            _player = new FieldPlayer(world, start, _people);
            gameObject.SetActive(true);
            _input.gameObject.SetActive(true);
            ShowCurrentMap();
            if (opening != null)
            {
                // 呼ぶ側の暗転が明けてから流す（真っ暗な中で文が進まないように）
                yield return null;
                yield return new WaitWhile(() => _fade.IsFading);
                yield return PlayScript(opening);
            }

            while (true)
            {
                // 切り替えは歩きと同じフレームで受ける（押しながら歩き出せるように）。台本でシューズをもらうこともあるので毎回出し直す
                if (_input.ConsumeRunToggle())
                {
                    RunningShoes.Toggle(_state.Flags);
                }
                _input.ShowRun(RunningShoes.Has(_state.Flags), RunningShoes.IsOn(_state.Flags));
                var direction = _input.HeldDirection;
                if (_input.IsBDown)
                {
                    yield return _menu.Open(_state, _player.Spot);
                    if (_menu.QuitRequested)
                    {
                        yield break;
                    }
                    // 閉じた B・A で歩き出したり話しかけたりしないよう1フレーム空ける
                    yield return null;
                }
                else if (_input.IsADown)
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

            yield return _avatar.Walk(_mapView.FeetPosition(_player.Position), direction, PlayerStepSeconds);
            var warp = _player.WarpHere;
            if (warp != null)
            {
                yield return Enter(warp);
                yield break;
            }
            // 目が合ったトレーナーは、草むらの野生より先に来る
            var sighting = _player.SpottedBy();
            if (sighting != null)
            {
                yield return Challenge(sighting.Value);
                yield break;
            }
            var wild = _player.RollEncounter(_random, _state.Party);
            if (wild != null)
            {
                yield return FightWild(wild);
            }
        }

        /// <summary>建物の中・草むらでも走れる（どこでも同じ速さにする）</summary>
        private float PlayerStepSeconds => RunningShoes.IsOn(_state.Flags) ? _runStepSeconds : _stepSeconds;

        private IEnumerator FightWild(OwnedMonster wild)
        {
            // バトルの画面はフィールドの十字キーより奥にあるので、戦う間は十字キーを隠す
            _input.SetPadVisible(false);
            yield return _battles.Fight(_state, new List<OwnedMonster> { wild }, null, canLose: false, _random);
            yield return AfterBattle(_battles.BlackedOut);
            AutoSave();
        }

        /// <summary>暗転したままのバトルの後。全滅なら最後に回復した場所へ移してから明ける</summary>
        private IEnumerator AfterBattle(bool blackedOut)
        {
            if (blackedOut)
            {
                _player.MoveTo(_state.BlackOut());
                ShowCurrentMap();
            }
            _input.SetPadVisible(true);
            yield return _battles.ReturnToField();
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
            yield return PlayScript(script);
        }

        /// <summary>
        /// トレーナーと目が合った: 「！」→ 隣まで歩いてくる → 主人公がそちらを向く → 台本（会話 → バトル）。
        /// 歩いてきたのは見た目だけで、立つ場所は台本の後に置き場所どおりに並べ直す
        /// </summary>
        private IEnumerator Challenge(TrainerSighting sighting)
        {
            var placed = sighting.Trainer;
            var facing = placed.Placement.Spot.Facing;
            _input.SetPadVisible(false);
            BattleSounds.Play(BattleSound.Status);
            yield return _peopleView.Exclaim(placed.Npc, _exclaimSeconds);
            var pos = placed.Position;
            for (int i = 0; i < sighting.Steps; i++)
            {
                pos += facing.Offset();
                yield return _peopleView.Walk(placed.Npc, _mapView.FeetPosition(pos), facing, _stepSeconds);
            }
            _player.Face(facing.Opposite());
            _avatar.Face(facing.Opposite());
            yield return PlayScript(placed.Placement.Script);
        }

        private IEnumerator PlayScript(IReadOnlyList<EventStep> script)
        {
            // バトルの後に明ける前に人を並べ直す（倒したトレーナーが元の場所へ戻るのを見せない）
            yield return _events.Play(new EventRunner(_state, script), _state, _random, beforeReturn: DrawPeople);
            if (_events.BlackedOut)
            {
                yield return AfterBattle(blackedOut: true);
            }
            // フラグで人が動いたかもしれないので並べ直す
            DrawPeople();
            // もらった・勝った・買った・回復したなどは台本の中で起きるので、台本が終わるたびに記録する
            AutoSave();
            // 出来事の後に目が合うこともある（最初の1体をもらった直後のライバル）
            var sighting = _player.SpottedBy();
            if (sighting != null)
            {
                yield return Challenge(sighting.Value);
                yield break;
            }
            // 最後の文を送った A で、すぐまた話しかけないよう1フレーム空ける
            yield return null;
        }

        private IEnumerator Enter(Warp warp)
        {
            yield return _fade.FadeOutCoroutine(_doorFadeSeconds);
            _player.Enter(warp);
            ShowCurrentMap();
            AutoSave();
            yield return _fade.FadeInCoroutine(_doorFadeSeconds);
            var greeter = _player.GreeterHere();
            if (greeter != null)
            {
                yield return Greet(greeter.Value);
            }
        }

        /// <summary>入ったマップで待っていた人が話してくる（ユキオコシ町の母）。主人公は話してくる人の方を向く</summary>
        private IEnumerator Greet(PlacedNpc greeter)
        {
            var facing = greeter.Placement.Spot.Facing;
            _player.Face(facing.Opposite());
            _avatar.Face(facing.Opposite());
            yield return PlayScript(greeter.Placement.Script);
        }

        /// <summary>
        /// 黙って今の場所ごと記録する。歩いている状態（バトル・会話の途中でない）のときだけ呼ぶ
        /// （続きから始めたとき、途中の場面から再開できないため）
        /// </summary>
        private void AutoSave()
        {
            StorySaveStore.Save(StorySave.Capture(_state, _player.Spot));
        }

        private void ShowCurrentMap()
        {
            _mapView.Draw(_player.Map);
            DrawPeople();
            _avatar.Place(_mapView.FeetPosition(_player.Position), _player.Facing);
            _camera.Follow(_avatar.transform, _player.Map.Width, _player.Map.Height);
            PlayMapMusic();
        }

        /// <summary>同じ曲のマップへ移ったときは頭から流し直さない（AudioManager が同じ曲を見分ける）</summary>
        private void PlayMapMusic()
        {
            var track = FieldMusic.TrackOf(_player.Map.Id);
            _battles.FieldTrack = track;
            GameMusic.Play(track);
        }

        private void DrawPeople()
        {
            _peopleView.Draw(_people.On(_player.Map.Id), _mapView);
        }
    }
}
