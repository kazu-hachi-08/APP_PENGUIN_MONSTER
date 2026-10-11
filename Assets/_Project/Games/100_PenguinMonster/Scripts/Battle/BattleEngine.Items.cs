using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 持ち物の発動（仕様書 §4.6）。HPが減ったとき・状態異常になったとき・ターン終わりに呼ぶ。
    /// ダメージの倍率になる持ち物は DamageCalculator、かるいはね は BattleMonster.EffectiveSpeed
    /// </summary>
    public sealed partial class BattleEngine
    {
        /// <summary>ふんばりマフラー: HP満タンからたおれるダメージなら HP1 で残す。使ったら true</summary>
        private static bool TryEndure(BattleMonster target, ref int damage)
        {
            if (!target.IsFullHp || damage < target.CurrentHp || !target.HasItem(HeldItem.GritScarf))
            {
                return false;
            }
            damage = target.CurrentHp - 1;
            target.UseItem();
            return true;
        }

        /// <summary>おやつのイカ: HPが半分以下になったら1回だけ回復。HPが減るたびに呼ぶ</summary>
        private void CheckHpItem(int side, BattleMonster monster, List<BattleEvent> events)
        {
            if (_state.IsOver || monster.IsFainted || !monster.HasItem(HeldItem.SquidSnack)
                || monster.CurrentHp > monster.MaxHp / BattleBalance.SquidSnackHpDenominator)
            {
                return;
            }
            monster.UseItem();
            monster.Heal(monster.MaxHp / BattleBalance.SquidSnackHealDenominator);
            events.Add(BattleEvent.Of(BattleEventKind.ItemHealed, side, monster));
        }

        /// <summary>さっぱりこんぶ: 状態異常になったらすぐ治す</summary>
        private static void CureByItem(int side, BattleMonster monster, List<BattleEvent> events)
        {
            if (!monster.HasItem(HeldItem.FreshKelp))
            {
                return;
            }
            monster.UseItem();
            monster.CureStatus();
            events.Add(BattleEvent.Of(BattleEventKind.ItemCured, side, monster));
        }

        /// <summary>いのちのつらら: 攻撃技を当てたら自分の最大HPの 1/10 減る</summary>
        private void ApplyLifeIcicleRecoil(int side, BattleMonster user, List<BattleEvent> events)
        {
            if (_state.IsOver || user.IsFainted || !user.HasItem(HeldItem.LifeIcicle))
            {
                return;
            }
            int damage = Math.Max(1, user.MaxHp / BattleBalance.LifeIcicleRecoilDenominator);
            ApplyDirectDamage(side, user, damage, BattleEventKind.ItemRecoil, events);
        }

        /// <summary>ほしざかな: ターン終わりに最大HPの 1/16 回復</summary>
        private void ApplyEndOfTurnItem(int side, BattleMonster monster, List<BattleEvent> events)
        {
            if (_state.IsOver || monster.IsFainted || monster.IsFullHp || !monster.HasItem(HeldItem.StarFish))
            {
                return;
            }
            monster.Heal(Math.Max(1, monster.MaxHp / BattleBalance.StarFishHealDenominator));
            events.Add(BattleEvent.Of(BattleEventKind.ItemHealed, side, monster));
        }
    }
}
