using System;
using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 台本を1行ずつ進める。画面に出す行（Say / Ask / Give / ChooseStarter / Battle / Heal）で止まり、View が見せ終えたら
    /// Advance / Answer / Choose / FinishBattle で先へ送る。フラグ・手持ち・お金・回復はここで行い、View は表示だけを受け持つ
    /// </summary>
    public sealed class EventRunner
    {
        private const string ReceivedFormat = "{0}を もらった！";

        private readonly StoryState _state;
        private readonly List<EventStep> _steps;
        private int _index;

        /// <summary>今 View に見せてほしい行。台本を最後まで進めたら null</summary>
        public EventStep Current => _index < _steps.Count ? _steps[_index] : null;
        public bool IsDone => Current == null;

        /// <summary>今の行で窓に出す文</summary>
        public string Message => Current?.Kind == EventStepKind.Give ? string.Format(ReceivedFormat, Current.Species.Name) : Current?.Text;

        public EventRunner(StoryState state, IEnumerable<EventStep> steps)
        {
            _state = state;
            _steps = new List<EventStep>(steps);
            RunUntilShown();
        }

        /// <summary>Say・Give・Heal を読み終えた</summary>
        public void Advance()
        {
            Expect(EventStepKind.Say, EventStepKind.Give, EventStepKind.Heal);
            Next();
        }

        /// <summary>Ask の答え。いいえなら、残りの台本の代わりに OnNo を流す</summary>
        public void Answer(bool yes)
        {
            Expect(EventStepKind.Ask);
            if (!yes)
            {
                var onNo = Current.OnNo;
                _steps.RemoveRange(_index + 1, _steps.Count - _index - 1);
                _steps.AddRange(onNo);
            }
            Next();
        }

        /// <summary>最初の3体から選んだ子。その子をもらう行に差し替える</summary>
        public void Choose(StorySpecies species)
        {
            Expect(EventStepKind.ChooseStarter);
            var chosenFlag = EventStep.ChosenFlag(Current.Flag, species);
            _steps[_index] = EventStep.Give(species, Current.Level);
            _steps.Insert(_index + 1, EventStep.SetFlag(chosenFlag));
            RunUntilShown();
        }

        /// <summary>
        /// Battle の決着。勝てば賞金をもらってフラグを立てる。負けたら、負けてもよい戦いならフラグを立てて続け、
        /// そうでなければ残りの台本を流さずに終わる（全滅して戻すのは View が StoryState.BlackOut で行う）
        /// </summary>
        public void FinishBattle(bool won)
        {
            Expect(EventStepKind.Battle);
            var step = Current;
            if (won)
            {
                _state.EarnMoney(step.Trainer.Prize);
            }
            else if (!step.CanLose)
            {
                _steps.RemoveRange(_index, _steps.Count - _index);
                return;
            }
            _state.Flags.Set(step.Trainer.DefeatedFlag);
            Next();
        }

        private void Next()
        {
            _index++;
            RunUntilShown();
        }

        /// <summary>画面に何も出さない行は、ここで済ませて飛ばす</summary>
        private void RunUntilShown()
        {
            while (!IsDone)
            {
                switch (Current.Kind)
                {
                    case EventStepKind.SetFlag:
                        _state.Flags.Set(Current.Flag);
                        _index++;
                        break;
                    case EventStepKind.Give:
                        // 手持ちがいっぱいのときの あずかり箱はフェーズ6で足す（区切り1の最初の1体では起きない）
                        _state.Party.Add(OwnedMonster.Create(Current.Species, Current.Level));
                        return;
                    case EventStepKind.Heal:
                        _state.Party.HealAll();
                        _state.LastHealSpot = Current.HealSpot;
                        return;
                    default:
                        return;
                }
            }
        }

        /// <summary>View の送り方の間違い（Say なのに Answer など）は、黙って進めず止める</summary>
        private void Expect(params EventStepKind[] kinds)
        {
            if (IsDone || Array.IndexOf(kinds, Current.Kind) < 0)
            {
                throw new InvalidOperationException($"今の行は {Current?.Kind.ToString() ?? "終わり"}");
            }
        }
    }
}
