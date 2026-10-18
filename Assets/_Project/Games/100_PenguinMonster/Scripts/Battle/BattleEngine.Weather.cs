using System;
using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 天候。技か特性で始まり、ターン終わりに残りを数えて止む。
    /// ダメージの倍率は DamageCalculator、すばやさ2倍の特性は BattleMonster.EffectiveSpeed
    /// </summary>
    public sealed partial class BattleEngine
    {
        /// <summary>
        /// 天候を始める。すでに同じ天候なら何もせず false（技なら「うまく 決まらなかった」）。
        /// お天気石を持った子が始めると長く続く
        /// </summary>
        private bool TryStartWeather(int side, BattleMonster source, Weather weather, List<BattleEvent> events)
        {
            if (_state.Weather == weather)
            {
                return false;
            }
            int turns = source.HasItem(HeldItem.WeatherStone) ? BattleBalance.WeatherStoneTurns : BattleBalance.WeatherTurns;
            _state.SetWeather(weather, turns);
            events.Add(BattleEvent.WeatherChange(BattleEventKind.WeatherStarted, side, source, weather));
            ShowWeatherRiders(events);
            return true;
        }

        /// <summary>ターン終わりの最初に残りを数える。止んだターンは吹雪のダメージも無い</summary>
        private void AdvanceWeather(List<BattleEvent> events)
        {
            var weather = _state.Weather;
            if (_state.TickWeather())
            {
                events.Add(BattleEvent.WeatherChange(BattleEventKind.WeatherEnded, 0, null, weather));
            }
        }

        /// <summary>吹雪のダメージ。氷タイプと、吹雪を好む特性（雪雲呼び・雪滑り）は受けない</summary>
        private void ApplyWeatherDamage(int side, BattleMonster monster, List<BattleEvent> events)
        {
            if (_state.Weather != Weather.Snow || monster.Definition.HasType(MonsterType.Ice)
                || monster.HasAbility(Ability.SnowCloud) || monster.HasAbility(Ability.SnowSlide))
            {
                return;
            }
            int damage = Math.Max(1, monster.MaxHp / BattleBalance.SnowDamageDenominator);
            ApplyDirectDamage(side, monster, damage, BattleEventKind.WeatherDamage, events);
        }
    }
}
