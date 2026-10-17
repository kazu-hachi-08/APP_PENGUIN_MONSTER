using System;
using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>記録（セーブ）から戻す</summary>
    public sealed partial class OwnedMonster
    {
        /// <summary>
        /// 記録の中身のまま作る。経験値はそのレベルの下限より少なくしない（次のレベルまでの残りがおかしくならないように）、
        /// HP は 0〜最大HP に収める（種の種族値が後で変わっても壊れないように）
        /// </summary>
        internal static OwnedMonster Restore(StorySpecies species, int level, int exp, IEnumerable<MoveDefinition> moves, int hp,
            StatusCondition status, HeldItem item)
        {
            var monster = new OwnedMonster(species, level, moves, item);
            monster.Exp = Math.Max(exp, monster.Exp);
            monster.CurrentHp = Math.Max(0, Math.Min(hp, monster.MaxHp));
            monster.Status = status;
            return monster;
        }
    }
}
