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
    /// 技の定義（仕様書 §4.2）
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
        public IReadOnlyList<MoveEffect> Effects { get; }

        public bool IsStatus => Category == MoveCategory.Status;

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
            int priority = 0, MoveEffect[] effects = null)
        {
            Name = name;
            Type = type;
            Category = category;
            Power = power;
            Accuracy = accuracy;
            Priority = priority;
            Effects = effects ?? Array.Empty<MoveEffect>();
        }
    }
}
