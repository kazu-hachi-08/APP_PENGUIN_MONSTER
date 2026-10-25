using System;
using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 出来事の台本を画面で見せる。進め方は EventRunner が決め、ここは窓を出して答えを返すだけ。
    /// 会話窓はバトルと同じ MessageWindow（同じ見た目・送り方）。A / B でも送れるようにする。
    /// 会話の間だけこの層を出し、十字キーは隠す（下の窓と重なるため）
    /// </summary>
    public class FieldEventPlayer : MonoBehaviour
    {
        private const string ConfirmStarterFormat = "{0}に しますか？";
        private const string FishWaitText = "……";
        private const string FishBiteText = "！ 釣れた！";
        private const string FishMissText = "釣れなかった……";
        private const string NothingToFishText = "何も 釣れそうに ない";

        [SerializeField] private MessageWindow _window;
        [SerializeField] private YesNoWindow _yesNo;
        [SerializeField] private StarterPanel _starters;
        [SerializeField] private FieldInput _input;
        [SerializeField] private StoryBattleScreen _battles;
        [Tooltip("よろず屋・あずかり箱の画面。会話窓より奥に置き、結果の文は会話窓に出す")]
        [SerializeField] private ShopPanel _shop;
        [SerializeField] private BoxPanel _box;
        [Tooltip("釣り竿を投げてから、かかったか分かるまでの秒数（A で飛ばせない。待つ間がおもしろさなので）")]
        [SerializeField] private float _fishWaitSeconds = 1.2f;

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
            _window.SetText("");
            ShowLayer();
            while (!runner.IsDone)
            {
                yield return PlayStep(runner, state, random);
            }
            gameObject.SetActive(false);
            _input.SetPadVisible(true);
        }

        /// <summary>台本の外から「はい / いいえ」だけ聞く（メニューの「やめる」「記録する」の確かめ）</summary>
        public IEnumerator Confirm(string question, Action<bool> onAnswer)
        {
            ShowLayer();
            bool yes = false;
            yield return Ask(question, answer => yes = answer);
            gameObject.SetActive(false);
            onAnswer(yes);
        }

        /// <summary>台本の外から1文だけ見せ、送られるまで待つ（メニューの「記録した！」）</summary>
        public IEnumerator Say(string message)
        {
            ShowLayer();
            yield return _window.Play(message);
            gameObject.SetActive(false);
        }

        /// <summary>釣りの文。「……」と少し待ってから、かかった・外れたを見せる。かかったときのバトルは呼ぶ側が始める</summary>
        public IEnumerator Fish(FishResult result)
        {
            ShowLayer();
            if (result == FishResult.NothingHere)
            {
                yield return _window.Play(NothingToFishText);
            }
            else
            {
                yield return _window.Type(FishWaitText);
                yield return new WaitForSeconds(_fishWaitSeconds);
                bool bite = result == FishResult.Bite;
                if (bite)
                {
                    BattleSounds.Play(BattleSound.Status);
                }
                yield return _window.Play(bite ? FishBiteText : FishMissText);
            }
            gameObject.SetActive(false);
            _input.SetPadVisible(true);
        }

        /// <summary>会話の層を出す（十字キーを隠し、前に残った「はい / いいえ」も消す）</summary>
        private void ShowLayer()
        {
            _input.SetPadVisible(false);
            _yesNo.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        /// <summary>問いを書き終えてから「はい / いいえ」を出す</summary>
        private IEnumerator Ask(string question, Action<bool> onAnswer)
        {
            yield return _window.Type(question);
            yield return _yesNo.Ask(onAnswer);
        }

        private IEnumerator PlayStep(EventRunner runner, StoryState state, System.Random random)
        {
            switch (runner.Current.Kind)
            {
                case EventStepKind.Battle:
                    yield return Fight(runner, state, random);
                    break;
                case EventStepKind.WildBattle:
                    yield return FightWild(runner, state, random);
                    break;
                case EventStepKind.Ask:
                    yield return Ask(runner.Message, runner.Answer);
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
                case EventStepKind.GiveBadge:
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
            yield return AfterBattle();
        }

        /// <summary>全滅なら暗転したまま呼ぶ側へ返す。そうでなければ人を並べ直してから明け、台本の続きへ</summary>
        private IEnumerator AfterBattle()
        {
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

        /// <summary>台本の野生と戦う。中身は草むらの野生と同じ（タマゴを投げられ、逃げられる）。明け方はトレーナー戦と同じ</summary>
        private IEnumerator FightWild(EventRunner runner, StoryState state, System.Random random)
        {
            var step = runner.Current;
            gameObject.SetActive(false);
            var foes = new List<OwnedMonster> { OwnedMonster.Create(step.Species, step.Level) };
            yield return _battles.Fight(state, foes, null, canLose: false, random);
            if (step.CatchOnly)
            {
                runner.FinishCatchOnlyBattle(_battles.Caught, _battles.BlackedOut);
            }
            else
            {
                runner.FinishWildBattle(_battles.PlayerWon || _battles.Caught);
            }
            yield return AfterBattle();
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
                bool yes = false;
                yield return Ask(string.Format(ConfirmStarterFormat, picked.Name), answer => yes = answer);
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
