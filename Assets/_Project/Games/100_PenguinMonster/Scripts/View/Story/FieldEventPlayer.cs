using System;
using System.Collections;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 出来事の台本を画面で見せる（PLAN_STORY_TOWN01.md フェーズ4〜6）。進め方は EventRunner が決め、ここは窓を出して答えを返すだけ。
    /// 会話窓はバトルと同じ MessageWindow（同じ見た目・送り方。仕様書 §7）。A / B でも送れるようにする。
    /// 会話の間だけこの層を出し、十字キーは隠す（下の窓と重なるため）
    /// </summary>
    public class FieldEventPlayer : MonoBehaviour
    {
        private const string ConfirmStarterFormat = "{0}に しますか？";

        [SerializeField] private MessageWindow _window;
        [SerializeField] private YesNoWindow _yesNo;
        [SerializeField] private StarterPanel _starters;
        [SerializeField] private FieldInput _input;
        [SerializeField] private StoryBattleScreen _battles;
        [Tooltip("よろず屋・あずかり箱の画面（フェーズ6）。会話窓より奥に置き、結果の文は会話窓に出す")]
        [SerializeField] private ShopPanel _shop;
        [SerializeField] private BoxPanel _box;

        private Action _beforeReturn;

        /// <summary>台本のバトルに負けて全滅した（残りの台本は流さずに終わっている）。呼ぶ側が最後に回復した場所へ戻す</summary>
        public bool BlackedOut { get; private set; }

        private void Update()
        {
            if (_input.IsADown || _input.IsBDown)
            {
                _window.RequestAdvance();
            }
        }

        /// <param name="beforeReturn">台本のバトルの後、フィールドへ明ける前に呼ぶ（人の並べ直し）</param>
        public IEnumerator Play(EventRunner runner, StoryState state, System.Random random, Action beforeReturn = null)
        {
            _beforeReturn = beforeReturn;
            BlackedOut = false;
            _input.SetPadVisible(false);
            _yesNo.gameObject.SetActive(false);
            _window.SetText("");
            gameObject.SetActive(true);
            while (!runner.IsDone)
            {
                yield return PlayStep(runner, state, random);
            }
            gameObject.SetActive(false);
            _input.SetPadVisible(true);
        }

        /// <summary>台本の外から「はい / いいえ」だけ聞く（メニューの「やめる」の確かめ）</summary>
        public IEnumerator Confirm(string question, Action<bool> onAnswer)
        {
            _input.SetPadVisible(false);
            _yesNo.gameObject.SetActive(false);
            gameObject.SetActive(true);
            yield return _window.Type(question);
            bool yes = false;
            yield return _yesNo.Ask(answer => yes = answer);
            gameObject.SetActive(false);
            onAnswer(yes);
        }

        private IEnumerator PlayStep(EventRunner runner, StoryState state, System.Random random)
        {
            switch (runner.Current.Kind)
            {
                case EventStepKind.Battle:
                    yield return Fight(runner, state, random);
                    break;
                case EventStepKind.Ask:
                    yield return _window.Type(runner.Message);
                    bool yes = false;
                    yield return _yesNo.Ask(answer => yes = answer);
                    runner.Answer(yes);
                    break;
                case EventStepKind.ChooseStarter:
                    StorySpecies chosen = null;
                    yield return ChooseStarter(runner.Current, species => chosen = species);
                    runner.Choose(chosen);
                    break;
                case EventStepKind.Shop:
                    yield return _shop.Open(state, runner.Current.ShopItems);
                    runner.Advance();
                    break;
                case EventStepKind.Storage:
                    yield return _box.Open(state);
                    runner.Advance();
                    break;
                case EventStepKind.Give:
                case EventStepKind.GiveItem:
                    BattleSounds.Play(BattleSound.Win);
                    yield return _window.Play(runner.Message);
                    runner.Advance();
                    break;
                default:
                    yield return _window.Play(runner.Message);
                    runner.Advance();
                    break;
            }
        }

        /// <summary>トレーナーと戦う。会話窓はバトルの間だけ隠し、全滅でなければ明けてから台本の続きへ</summary>
        private IEnumerator Fight(EventRunner runner, StoryState state, System.Random random)
        {
            var step = runner.Current;
            gameObject.SetActive(false);
            yield return _battles.Fight(state, step.Trainer.CreateParty(), step.Trainer, step.CanLose, random);
            runner.FinishBattle(_battles.PlayerWon);
            if (_battles.BlackedOut)
            {
                BlackedOut = true;
                yield break;
            }
            _window.SetText("");
            gameObject.SetActive(true);
            _beforeReturn?.Invoke();
            yield return _battles.ReturnToField();
        }

        /// <summary>3つのタマゴから選び、「〇〇に しますか？」で はい と答えるまで選び直す（もらった後は替えられないので確かめる）</summary>
        private IEnumerator ChooseStarter(EventStep step, Action<StorySpecies> onChosen)
        {
            _starters.Open(step.Choices);
            while (true)
            {
                _window.gameObject.SetActive(false);
                StorySpecies picked = null;
                yield return _starters.Pick(species => picked = species);

                _window.gameObject.SetActive(true);
                yield return _window.Type(string.Format(ConfirmStarterFormat, picked.Name));
                bool yes = false;
                yield return _yesNo.Ask(answer => yes = answer);
                if (yes)
                {
                    _starters.Close();
                    onChosen(picked);
                    yield break;
                }
            }
        }
    }
}
