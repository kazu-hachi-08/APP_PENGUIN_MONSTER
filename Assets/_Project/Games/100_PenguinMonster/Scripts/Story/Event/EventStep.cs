using System.Collections.Generic;

namespace PenguinMonster.Story
{
    public enum EventStepKind
    {
        /// <summary>1文を出して、送られるのを待つ</summary>
        Say,
        /// <summary>1文を出して「はい / いいえ」を選ばせる。いいえなら OnNo を流して終わる</summary>
        Ask,
        /// <summary>フラグを立てる。画面には何も出さない</summary>
        SetFlag,
        /// <summary>手持ちに足し、「〇〇を もらった！」を出す</summary>
        Give,
        /// <summary>最初の3体を選ぶ画面を出す。選んだ子が Give になる</summary>
        ChooseStarter,
    }

    /// <summary>
    /// 出来事の台本の1行（PLAN_STORY_TOWN01.md フェーズ4）。台本は「会話 → 選ぶ → フラグ → 手持ちに足す」をこの並びで書く。
    /// 種類ごとに使う中身だけ入れる（ほかは空のまま）。作るときは下の Say などから作る
    /// </summary>
    public sealed class EventStep
    {
        private static readonly EventStep[] None = new EventStep[0];

        public EventStepKind Kind { get; }
        /// <summary>Say / Ask の文</summary>
        public string Text { get; }
        /// <summary>SetFlag のフラグ</summary>
        public string Flag { get; }
        /// <summary>Give でもらう子</summary>
        public StorySpecies Species { get; }
        /// <summary>ChooseStarter の候補（海・火・苔の順）</summary>
        public IReadOnlyList<StorySpecies> Choices { get; }
        /// <summary>Give / ChooseStarter でもらう子のレベル</summary>
        public int Level { get; }
        /// <summary>Ask でいいえを選んだときに流す台本</summary>
        public IReadOnlyList<EventStep> OnNo { get; }

        private EventStep(EventStepKind kind, string text = null, string flag = null, StorySpecies species = null,
            IReadOnlyList<StorySpecies> choices = null, int level = 0, IReadOnlyList<EventStep> onNo = null)
        {
            Kind = kind;
            Text = text;
            Flag = flag;
            Species = species;
            Choices = choices;
            Level = level;
            OnNo = onNo ?? None;
        }

        public static EventStep Say(string text) => new EventStep(EventStepKind.Say, text);

        public static EventStep Ask(string text, params EventStep[] onNo) => new EventStep(EventStepKind.Ask, text, onNo: onNo);

        public static EventStep SetFlag(string flag) => new EventStep(EventStepKind.SetFlag, flag: flag);

        public static EventStep Give(StorySpecies species, int level) => new EventStep(EventStepKind.Give, species: species, level: level);

        public static EventStep ChooseStarter(IReadOnlyList<StorySpecies> choices, int level)
        {
            return new EventStep(EventStepKind.ChooseStarter, choices: choices, level: level);
        }

        /// <summary>話すだけの台本（村の人・看板）</summary>
        public static EventStep[] Lines(IEnumerable<string> lines)
        {
            var steps = new List<EventStep>();
            foreach (var line in lines)
            {
                steps.Add(Say(line));
            }
            return steps.ToArray();
        }
    }
}
