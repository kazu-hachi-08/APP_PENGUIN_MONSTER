using System;
using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// 台本を1行ずつ進める。画面に出す行（Say / Ask / Give / GiveItem / GiveBadge / ChooseStarter / Battle / WildBattle / Heal / Shop / Storage）で止まり、View が見せ終えたら
    /// Advance / Answer / Choose / FinishBattle / FinishWildBattle で先へ送る。フラグ・手持ち・お金・回復はここで行い、View は表示だけを受け持つ
    /// </summary>
    public sealed class EventRunner
    {
        private const string ReceivedFormat = "{0}を もらった！";
        private const string SentToBoxFormat = "{0}を もらった！\n手持ちが いっぱいなので あずかり箱へ 送られた";
        private const string ReceivedItemFormat = "{0}を {1}個 もらった！";
        private const string FoundItemFormat = "{0}を {1}個 見つけた！";

        private readonly StoryState _state;
        private readonly List<EventStep> _steps;
        private int _index;
        /// <summary>今の Give で、手持ちがいっぱいで あずかり箱へ送った</summary>
        private bool _sentToBox;

        /// <summary>今 View に見せてほしい行。台本を最後まで進めたら null</summary>
        public EventStep Current => _index < _steps.Count ? _steps[_index] : null;
        public bool IsDone => Current == null;

        /// <summary>今の行で窓に出す文</summary>
        public string Message
        {
            get
            {
                switch (Current?.Kind)
                {
                    case EventStepKind.Give:
                        return string.Format(_sentToBox ? SentToBoxFormat : ReceivedFormat, Current.Species.Name);
                    case EventStepKind.GiveItem:
                        return string.Format(Current.Found ? FoundItemFormat : ReceivedItemFormat, StoryItems.Get(Current.Item).Name, Current.Count);
                    case EventStepKind.GiveBadge:
                        return string.Format(ReceivedFormat, Current.Text);
                    default:
                        return Current?.Text;
                }
            }
        }

        public EventRunner(StoryState state, IEnumerable<EventStep> steps)
        {
            _state = state;
            _steps = new List<EventStep>(steps);
            RunUntilShown();
        }

        /// <summary>Say・Give・GiveItem・GiveBadge・Heal を読み終えた / Shop・Storage の画面を閉じた（売り買い・入れ替えは画面が StoryState に直接行う）</summary>
        public void Advance()
        {
            Expect(EventStepKind.Say, EventStepKind.Give, EventStepKind.GiveItem, EventStepKind.GiveBadge, EventStepKind.Heal,
                EventStepKind.Shop, EventStepKind.Storage);
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

        /// <summary>
        /// WildBattle の決着。倒すか捕まえたら続ける。逃げた・負けたときは残りの台本を流さずに終わる
        /// （同じ置き場所の人が残るので、もう一度話しかければまた戦える）
        /// </summary>
        public void FinishWildBattle(bool defeatedOrCaught)
        {
            Expect(EventStepKind.WildBattle);
            if (!defeatedOrCaught)
            {
                _steps.RemoveRange(_index, _steps.Count - _index);
                return;
            }
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
                        _sentToBox = !_state.Receive(OwnedMonster.Create(Current.Species, Current.Level));
                        return;
                    case EventStepKind.GiveItem:
                        _state.Bag.Add(Current.Item, Current.Count);
                        return;
                    case EventStepKind.GiveBadge:
                        _state.Badges.Add(Current.Flag);
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
