using System;
using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// バッグに入る道具。バトルの持ち物（HeldItem）とは別物。
    /// 並びはバッグの並び（捕獲タマゴ → 回復 → 状態異常を治すもの → 持たせる道具）。
    /// 記録には名前で入るので途中に足してよいが、名前は変えない・消さない（古い記録が読めなくなる）
    /// </summary>
    public enum StoryItem
    {
        CaptureEgg,
        GreatEgg,
        UltraEgg,
        NetEgg,
        NightEgg,
        HeavyEgg,
        SkyEgg,
        Potion,
        Antidote,
        ParalyzeHeal,
        Awakening,
        // 持たせる道具（区切り4）。持たせるとバトルの持ち物（HeldItem）として働く
        HeldDriedFish,
        HeldSquidSnack,
        HeldFreshKelp,
        HeldBraveFeather,
        HeldWiseFeather,
    }

    /// <summary>道具1種類の中身。回復の量・治す状態異常・値段、捕獲タマゴなら捕まえやすさの倍率、持たせる道具なら持ち物</summary>
    public sealed class StoryItemDefinition
    {
        private static readonly StatusCondition[] NoCures = new StatusCondition[0];
        private static readonly MonsterType[] NoTypes = new MonsterType[0];

        public StoryItem Item { get; }
        public string Name { get; }
        public string Description { get; }
        /// <summary>よろず屋で買う値段。売るとこの半分</summary>
        public int Price { get; }
        /// <summary>HP を回復する量。0 なら回復しない</summary>
        public int HealAmount { get; }
        public StatusCondition[] Cures { get; }
        /// <summary>捕まえやすさに掛ける倍率（百分率）。0 なら捕獲タマゴではない</summary>
        public int CatchPercent { get; private set; }
        /// <summary>この中のどれかのタイプを持つ相手には CatchPercent ではなく StoryBalance.TypeEggCatchPercent を掛ける</summary>
        public MonsterType[] BonusTypes { get; private set; } = NoTypes;
        /// <summary>投げるときのタマゴの見た目</summary>
        public EggKind Egg { get; private set; }
        /// <summary>持たせたときのバトルの持ち物。None なら持たせる道具ではない</summary>
        public HeldItem Held { get; private set; }

        public bool IsCaptureEgg => CatchPercent > 0;
        /// <summary>手持ちの子に使う道具（回復・状態異常を治す）</summary>
        public bool IsMedicine => HealAmount > 0 || Cures.Length > 0;
        public bool IsHeldItem => Held != HeldItem.None;
        /// <summary>値段の無い道具（1つずつしか手に入らない持たせる道具）は売らせない。0円で手放して取り直せなくなるため</summary>
        public bool CanSell => Price > 0;
        public int SellPrice => Price / StoryBalance.SellPriceDivisor;

        public StoryItemDefinition(StoryItem item, string name, string description, int price, int healAmount = 0,
            params StatusCondition[] cures)
        {
            Item = item;
            Name = name;
            Description = description;
            Price = price;
            HealAmount = healAmount;
            Cures = cures ?? NoCures;
        }

        /// <summary>捕獲タマゴの定義。bonusTypes を渡すと、そのタイプの相手にだけ大きく効くタマゴになる</summary>
        public static StoryItemDefinition CaptureEgg(StoryItem item, string name, string description, int price,
            int catchPercent, EggKind egg, params MonsterType[] bonusTypes)
        {
            return new StoryItemDefinition(item, name, description, price)
            {
                CatchPercent = catchPercent,
                Egg = egg,
                BonusTypes = bonusTypes ?? NoTypes,
            };
        }

        /// <summary>持たせる道具の定義。名前・説明はバトルの持ち物と同じにする（バトル中の文と揃えるため）。よろず屋では扱わない</summary>
        public static StoryItemDefinition HeldItemOf(StoryItem item, HeldItem held)
        {
            return new StoryItemDefinition(item, held.ToJapanese(), held.Description(), 0) { Held = held };
        }

        /// <summary>
        /// このタマゴを、このタイプの相手に投げたときの倍率。
        /// 2タイプの子はどちらかが合えば特化の倍率（両方合っても重ねない）
        /// </summary>
        public int CatchPercentAgainst(IReadOnlyList<MonsterType> types)
        {
            foreach (var type in types)
            {
                if (Array.IndexOf(BonusTypes, type) >= 0)
                {
                    return StoryBalance.TypeEggCatchPercent;
                }
            }
            return CatchPercent;
        }

        /// <summary>
        /// この子に使って効き目がある（瀕死には効かない。満タンで治す状態異常も無ければ使わせない）。
        /// フィールドの手持ちにもバトルの写しにも使えるよう、HP・状態異常だけ受け取る
        /// </summary>
        public bool CanTreat(int currentHp, int maxHp, StatusCondition status)
        {
            if (currentHp <= 0)
            {
                return false;
            }
            return (HealAmount > 0 && currentHp < maxHp) || CuresStatus(status);
        }

        public bool CuresStatus(StatusCondition status)
        {
            return status != StatusCondition.None && Array.IndexOf(Cures, status) >= 0;
        }
    }

    /// <summary>道具の一覧（値段は本家に寄せる）。並びは StoryItem と同じにする（Get が番号で引くため）</summary>
    public static class StoryItems
    {
        private static readonly StoryItemDefinition[] All =
        {
            StoryItemDefinition.CaptureEgg(StoryItem.CaptureEgg, "捕獲タマゴ", "弱った 野生の ペンモンに 投げて 捕まえる", 200,
                StoryBalance.Percent, EggKind.Normal),
            StoryItemDefinition.CaptureEgg(StoryItem.GreatEgg, "上等タマゴ", "ふつうより 捕まえやすい", 600,
                StoryBalance.GreatEggCatchPercent, EggKind.Great),
            StoryItemDefinition.CaptureEgg(StoryItem.UltraEgg, "特上タマゴ", "上等より さらに 捕まえやすい", 1200,
                StoryBalance.UltraEggCatchPercent, EggKind.Ultra),
            StoryItemDefinition.CaptureEgg(StoryItem.NetEgg, "網タマゴ", "海と 苔の ペンモンが 捕まえやすい", 1000,
                StoryBalance.Percent, EggKind.Net, MonsterType.Sea, MonsterType.Moss),
            StoryItemDefinition.CaptureEgg(StoryItem.NightEgg, "夜タマゴ", "闇と オーロラの ペンモンが 捕まえやすい", 1000,
                StoryBalance.Percent, EggKind.Night, MonsterType.Dark, MonsterType.Aurora),
            StoryItemDefinition.CaptureEgg(StoryItem.HeavyEgg, "重いタマゴ", "岩と 氷の ペンモンが 捕まえやすい", 1000,
                StoryBalance.Percent, EggKind.Heavy, MonsterType.Rock, MonsterType.Ice),
            StoryItemDefinition.CaptureEgg(StoryItem.SkyEgg, "空タマゴ", "鳥と 雷の ペンモンが 捕まえやすい", 1000,
                StoryBalance.Percent, EggKind.Sky, MonsterType.Bird, MonsterType.Thunder),
            new StoryItemDefinition(StoryItem.Potion, "傷薬", "HPを 20 回復する", 300, healAmount: 20),
            new StoryItemDefinition(StoryItem.Antidote, "毒消し", "毒を 治す", 100, 0, StatusCondition.Poison, StatusCondition.BadPoison),
            new StoryItemDefinition(StoryItem.ParalyzeHeal, "麻痺治し", "麻痺を 治す", 200, 0, StatusCondition.Paralysis),
            new StoryItemDefinition(StoryItem.Awakening, "目覚まし", "眠りを 覚ます", 250, 0, StatusCondition.Sleep),
            StoryItemDefinition.HeldItemOf(StoryItem.HeldDriedFish, HeldItem.StarFish),
            StoryItemDefinition.HeldItemOf(StoryItem.HeldSquidSnack, HeldItem.SquidSnack),
            StoryItemDefinition.HeldItemOf(StoryItem.HeldFreshKelp, HeldItem.FreshKelp),
            StoryItemDefinition.HeldItemOf(StoryItem.HeldBraveFeather, HeldItem.BraveFeather),
            StoryItemDefinition.HeldItemOf(StoryItem.HeldWiseFeather, HeldItem.WiseFeather),
        };

        public static StoryItemDefinition Get(StoryItem item) => All[(int)item];

        public static int Count => All.Length;

        /// <summary>持ち物をバッグの道具に戻すとき。ストーリーの道具に無い持ち物なら false</summary>
        public static bool TryFromHeld(HeldItem held, out StoryItem item)
        {
            foreach (var definition in All)
            {
                if (definition.IsHeldItem && definition.Held == held)
                {
                    item = definition.Item;
                    return true;
                }
            }
            item = default;
            return false;
        }

        /// <summary>定義の並び（テストで StoryItem と同じ並びか確かめるため）</summary>
        public static IReadOnlyList<StoryItemDefinition> Definitions => All;
    }
}
