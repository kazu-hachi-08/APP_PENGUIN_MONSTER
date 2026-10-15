using System;
using System.Collections;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 出来事の台本を画面で見せる（PLAN_STORY_TOWN01.md フェーズ4）。進め方は EventRunner が決め、ここは窓を出して答えを返すだけ。
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

        private void Update()
        {
            if (_input.IsADown || _input.IsBDown)
            {
                _window.RequestAdvance();
            }
        }

        public IEnumerator Play(EventRunner runner)
        {
            _input.SetPadVisible(false);
            _yesNo.gameObject.SetActive(false);
            _window.SetText("");
            gameObject.SetActive(true);
            while (!runner.IsDone)
            {
                yield return PlayStep(runner);
            }
            gameObject.SetActive(false);
            _input.SetPadVisible(true);
        }

        private IEnumerator PlayStep(EventRunner runner)
        {
            switch (runner.Current.Kind)
            {
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
                case EventStepKind.Give:
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
