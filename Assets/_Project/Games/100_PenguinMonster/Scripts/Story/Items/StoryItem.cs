using System;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// バッグに入る道具（PLAN_STORY_TOWN01.md フェーズ6）。バトルの持ち物（HeldItem）とは別物。
    /// 並びはバッグの並び（捕獲タマゴ → 回復 → 状態異常を治すもの）
    /// </summary>
    public enum StoryItem
    {
        CaptureEgg,
        Potion,
        Antidote,
        ParalyzeHeal,
        Awakening,
    }

    /// <summary>道具1種類の中身。回復の量・治す状態異常・値段</summary>
    public sealed class StoryItemDefinition
    {
        private static readonly StatusCondition[] NoCures = new StatusCondition[0];

        public StoryItem Item { get; }
        public string Name { get; }
        public string Description { get; }
        /// <summary>よろず屋で買う値段。売るとこの半分</summary>
        public int Price { get; }
        /// <summary>HP を回復する量。0 なら回復しない</summary>
        public int HealAmount { get; }
        public StatusCondition[] Cures { get; }

        public bool IsCaptureEgg => Item == StoryItem.CaptureEgg;
        /// <summary>手持ちの子に使う道具（回復・状態異常を治す）</summary>
        public bool IsMedicine => HealAmount > 0 || Cures.Length > 0;
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

    /// <summary>区切り1の道具の一覧（PLAN_STORY_TOWN01.md フェーズ6。値段は本家に寄せる）</summary>
    public static class StoryItems
    {
        private static readonly StoryItemDefinition[] All =
        {
            new StoryItemDefinition(StoryItem.CaptureEgg, "捕獲タマゴ", "弱った 野生の ペンモンに 投げて 捕まえる", 200),
            new StoryItemDefinition(StoryItem.Potion, "傷薬", "HPを 20 回復する", 300, healAmount: 20),
            new StoryItemDefinition(StoryItem.Antidote, "毒消し", "毒を 治す", 100, 0, StatusCondition.Poison, StatusCondition.BadPoison),
            new StoryItemDefinition(StoryItem.ParalyzeHeal, "麻痺治し", "麻痺を 治す", 200, 0, StatusCondition.Paralysis),
            new StoryItemDefinition(StoryItem.Awakening, "目覚まし", "眠りを 覚ます", 250, 0, StatusCondition.Sleep),
        };

        public static StoryItemDefinition Get(StoryItem item) => All[(int)item];

        public static int Count => All.Length;
    }
}
