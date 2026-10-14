using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// ドラフトのカードとずかんで共通の文面（種族値・特性と持ち物・技）
    /// </summary>
    public static class MonsterTexts
    {
        private const string StatusMoveText = "変化";
        private const string HasEffectsMark = "　＋効果";

        /// <summary>種族値を1行ずつ。見出し（HP・こうげき…）はシーン側の別の Text に並べてある</summary>
        public static string StatValues(Stats s)
        {
            return $"{s.Hp}\n{s.Attack}\n{s.Defense}\n{s.SpAttack}\n{s.SpDefense}\n{s.Speed}\n{s.Total}";
        }

        /// <summary>
        /// 特性の名前と説明。持ち物を持っていればその行、ヒナなら綿毛の行も足す。
        /// 説明は小さめの文字にして名前を目立たせる
        /// </summary>
        public static string Traits(MonsterDefinition monster, int descriptionSize)
        {
            string text = Line("特性", monster.Ability.ToJapanese(), monster.Ability.Description(), descriptionSize);
            if (monster.Rank == MonsterRank.Hina)
            {
                text += "\n" + Line("ヒナ", HeldItem.ChickDown.ToJapanese(), HeldItem.ChickDown.Description(), descriptionSize);
            }
            if (monster.Item != HeldItem.None)
            {
                text += "\n" + Line("持ち物", monster.Item.ToJapanese(), monster.Item.Description(), descriptionSize);
            }
            return text;
        }

        /// <summary>
        /// ドラフトのカード用の短い技の一覧。1つにつき2行（名前 / タイプ・威力）。
        /// 効果の中身はカードに入り切らないので、バトルの技ボタンと同じく「＋効果」の印だけにする
        /// </summary>
        public static string MoveList(MonsterDefinition monster, int detailSize)
        {
            var lines = new List<string>();
            foreach (var move in monster.Moves)
            {
                string color = ColorUtility.ToHtmlStringRGB(TypeColors.Get(move.Type));
                string power = move.IsStatus ? StatusMoveText : $"{move.Category.CategoryName()} 威力 {move.Power}";
                string mark = move.Effects().Length > 0 && !move.IsStatus ? HasEffectsMark : "";
                lines.Add($"<b>{move.Name}</b>
<size={detailSize}><b><color=#{color}>{move.Type.ToJapanese()}</color></b>　{power}{mark}</size>");
            }
            return string.Join("
", lines);
        }

        private static string Line(string label, string name, string description, int descriptionSize)
        {
            return $"{label} <b>{name}</b>\n<size={descriptionSize}>{description}</size>";
        }
    }
}
