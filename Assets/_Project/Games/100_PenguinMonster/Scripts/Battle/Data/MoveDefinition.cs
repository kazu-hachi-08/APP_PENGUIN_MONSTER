using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    public enum MoveCategory
    {
        Physical,
        Special,
        /// <summary>ダメージを与えず、効果（状態異常・ランク変化）だけの技</summary>
        Status,
    }

    /// <summary>
    /// 技の定義
    /// </summary>
    public sealed class MoveDefinition
    {
        public string Name { get; }
        public MonsterType Type { get; }
        public MoveCategory Category { get; }
        public int Power { get; }
        /// <summary>命中率（%）</summary>
        public int Accuracy { get; }
        public int Priority { get; }
        /// <summary>相手全員に当たる（ダブルスで意味がある。シングルは相手が1体なので変わらない）</summary>
        public bool HitsAllFoes { get; }
        public IReadOnlyList<MoveEffect> Effects { get; }
        /// <summary>与えたダメージの 1/3 を自分も受ける</summary>
        public bool Recoils { get; }
        /// <summary>与えたダメージの半分回復する</summary>
        public bool Drains { get; }
        /// <summary>1回の技で当たる回数。2〜5 回のものは乱数で決める</summary>
        public int MinHits { get; }
        public int MaxHits { get; }

        public bool IsStatus => Category == MoveCategory.Status;
        public bool IsMultiHit => MaxHits > 1;

        /// <summary>自分だけにかける変化技は、相手の回避に関係なく必ず決まる</summary>
        public bool TargetsSelfOnly
        {
            get
            {
                if (!IsStatus)
                {
                    return false;
                }
                foreach (var effect in Effects)
                {
                    if (effect.Target != EffectTarget.Self)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public MoveDefinition(string name, MonsterType type, MoveCategory category, int power, int accuracy,
            int priority = 0, MoveEffect[] effects = null, bool hitsAllFoes = false, bool recoils = false, bool drains = false,
            int minHits = 1, int maxHits = 1)
        {
            Name = name;
            Type = type;
            Category = category;
            Power = power;
            Accuracy = accuracy;
            Priority = priority;
            HitsAllFoes = hitsAllFoes;
            Effects = effects ?? Array.Empty<MoveEffect>();
            Recoils = recoils;
            Drains = drains;
            MinHits = minHits;
            MaxHits = maxHits;
        }
    }
}
