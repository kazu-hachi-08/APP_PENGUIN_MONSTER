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
        /// <summary>トレーナーと戦う。負けたら（負けてもよい戦いでなければ）残りの台本は流さない</summary>
        Battle,
        /// <summary>手持ちを全回復し、全滅したときの戻り先をここにして、文を出す</summary>
        Heal,
        /// <summary>バッグに道具を足し、「〇〇を N個 もらった！」を出す</summary>
        GiveItem,
        /// <summary>よろず屋の売り買いの画面を出す。売るものは ShopItems</summary>
        Shop,
        /// <summary>あずかり箱の画面を出す（ぬくもり小屋）</summary>
        Storage,
        /// <summary>羽章をもらう（Flag の名前で StoryState.Badges に足す）。「〇〇を もらった！」を出す</summary>
        GiveBadge,
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
        /// <summary>SetFlag のフラグ。ChooseStarter では、選んだ子の No を後ろに付けて立てるフラグの頭</summary>
        public string Flag { get; }
        /// <summary>Give でもらう子</summary>
        public StorySpecies Species { get; }
        /// <summary>ChooseStarter の候補（海・火・苔の順）</summary>
        public IReadOnlyList<StorySpecies> Choices { get; }
        /// <summary>Give / ChooseStarter でもらう子のレベル</summary>
        public int Level { get; }
        /// <summary>Ask でいいえを選んだときに流す台本</summary>
        public IReadOnlyList<EventStep> OnNo { get; }
        /// <summary>Battle の相手</summary>
        public TrainerDefinition Trainer { get; }
        /// <summary>Battle で負けても台本を続ける（最初のライバル戦。本家と同じく負けても話が進む）</summary>
        public bool CanLose { get; }
        /// <summary>Heal で全滅したときの戻り先にする場所</summary>
        public FieldSpot HealSpot { get; }
        /// <summary>GiveItem でもらう道具</summary>
        public StoryItem Item { get; }
        /// <summary>GiveItem でもらう数</summary>
        public int Count { get; }
        /// <summary>GiveItem が落とし物を拾ったもの（「見つけた！」と出す）</summary>
        public bool Found { get; }
        /// <summary>Shop で売っている道具</summary>
        public IReadOnlyList<StoryItem> ShopItems { get; }

        private EventStep(EventStepKind kind, string text = null, string flag = null, StorySpecies species = null,
            IReadOnlyList<StorySpecies> choices = null, int level = 0, IReadOnlyList<EventStep> onNo = null,
            TrainerDefinition trainer = null, bool canLose = false, FieldSpot healSpot = default, StoryItem item = default,
            int count = 0, IReadOnlyList<StoryItem> shopItems = null, bool found = false)
        {
            Kind = kind;
            Text = text;
            Flag = flag;
            Species = species;
            Choices = choices;
            Level = level;
            OnNo = onNo ?? None;
            Trainer = trainer;
            CanLose = canLose;
            HealSpot = healSpot;
            Item = item;
            Count = count;
            ShopItems = shopItems;
            Found = found;
        }

        public static EventStep Say(string text) => new EventStep(EventStepKind.Say, text);

        public static EventStep Ask(string text, params EventStep[] onNo) => new EventStep(EventStepKind.Ask, text, onNo: onNo);

        public static EventStep SetFlag(string flag) => new EventStep(EventStepKind.SetFlag, flag: flag);

        public static EventStep Give(StorySpecies species, int level) => new EventStep(EventStepKind.Give, species: species, level: level);

        /// <param name="chosenFlagPrefix">選んだ子の No を後ろに付けて立てるフラグ（ライバルが相性のいい子を選ぶのに使う）</param>
        public static EventStep ChooseStarter(IReadOnlyList<StorySpecies> choices, int level, string chosenFlagPrefix)
        {
            return new EventStep(EventStepKind.ChooseStarter, flag: chosenFlagPrefix, choices: choices, level: level);
        }

        /// <summary>選んだ最初の1体のフラグ（ChooseStarter のフラグの頭 ＋ No）</summary>
        public static string ChosenFlag(string prefix, StorySpecies species) => prefix + species.No;

        public static EventStep Battle(TrainerDefinition trainer, bool canLose = false)
        {
            return new EventStep(EventStepKind.Battle, trainer: trainer, canLose: canLose);
        }

        /// <param name="returnSpot">これから全滅したときに戻る場所</param>
        public static EventStep Heal(string text, FieldSpot returnSpot)
        {
            return new EventStep(EventStepKind.Heal, text, healSpot: returnSpot);
        }

        public static EventStep GiveItem(StoryItem item, int count)
        {
            return new EventStep(EventStepKind.GiveItem, item: item, count: count);
        }

        /// <summary>落とし物を拾う。もらうのと同じくバッグに入り、文だけ「見つけた！」にする</summary>
        public static EventStep FindItem(StoryItem item, int count)
        {
            return new EventStep(EventStepKind.GiveItem, item: item, count: count, found: true);
        }

        public static EventStep Shop(params StoryItem[] items) => new EventStep(EventStepKind.Shop, shopItems: items);

        public static EventStep Storage() => new EventStep(EventStepKind.Storage);

        /// <param name="badgeId">記録に残す羽章の名前（表示名を後で変えても記録がずれないように、表示名とは分ける）</param>
        /// <param name="displayName">「〇〇を もらった！」の〇〇</param>
        public static EventStep GiveBadge(string badgeId, string displayName)
        {
            return new EventStep(EventStepKind.GiveBadge, displayName, flag: badgeId);
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
