using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;
using static PenguinMonster.Battle.MonsterType;

namespace PenguinMonster.View
{
    /// <summary>
    /// 特別な技の専用エフェクト。ここにある技は型（MoveStyleTable）より優先する。
    /// 段を並べると前の段が消えてから次の段を出す（粒の Image を使い回せるので、SceneBuilder の粒の数を増やさずに済む）。
    /// 新しく足すときは、型の段（StageOf）を組み合わせるか、専用の動き方を .Recipes.cs に足して Stage を作る
    /// </summary>
    public partial class MoveEffectView
    {
        // 伝説の専用技は、ふつうの技より粒を大きくして格の違いを見せる
        private const float LegendScale = 1.5f;

        private static readonly Dictionary<MoveDefinition, Func<Vector2, List<Stage>>> Signatures =
            new Dictionary<MoveDefinition, Func<Vector2, List<Stage>>>
            {
                // 大氷河: 大きな氷塊が落ち、まわりから結晶が刺さる
                {
                    MoveTable.GreatGlacier, toward => new List<Stage>
                    {
                        Bigger(StageOf(MoveStyle.Fall, Ice, toward), LegendScale),
                        StageOf(MoveStyle.Converge, Ice, toward),
                    }
                },
                // 天の雷: 大きな稲妻が落ち、まわりで火花がまたたく
                {
                    MoveTable.HeavenlyThunder, toward => new List<Stage>
                    {
                        Bigger(StageOf(MoveStyle.Plunge, Thunder, toward), LegendScale),
                        StageOf(MoveStyle.Twinkle, Thunder, toward),
                    }
                },
                // 明けない夜: 三日月がまたたいてから、大きな切り傷が走る
                {
                    MoveTable.EndlessNight, toward => new List<Stage>
                    {
                        StageOf(MoveStyle.Twinkle, Dark, toward),
                        Bigger(StageOf(MoveStyle.Slash, Dark, toward), LegendScale),
                    }
                },
                // 太古の大突進: 大きな岩の体当たりのあと、岩くずが降る
                {
                    MoveTable.AncientCharge, toward => new List<Stage>
                    {
                        Bigger(StageOf(MoveStyle.Hit, Rock, toward), LegendScale),
                        StageOf(MoveStyle.Fall, Rock, toward),
                    }
                },
                // 世界樹の息吹: 葉が渦を巻いて集まり、足元から芽吹く
                {
                    MoveTable.WorldTreeBreath, toward => new List<Stage>
                    {
                        Bigger(StageOf(MoveStyle.Swirl, Moss, toward), LegendScale),
                        StageOf(MoveStyle.Rise, Moss, toward),
                    }
                },
                // 始まりの咆哮: 大きな声が吹き抜け、まわりで光がまたたく
                {
                    MoveTable.FirstRoar, toward => new List<Stage>
                    {
                        Bigger(StageOf(MoveStyle.Blow, MonsterType.Normal, toward), LegendScale),
                        StageOf(MoveStyle.Twinkle, Aurora, toward),
                    }
                },
                // 黒い日輪: 闇がまたたいてから、大きな炎が立ちのぼる
                {
                    MoveTable.BlackSun, toward => new List<Stage>
                    {
                        StageOf(MoveStyle.Twinkle, Dark, toward),
                        Bigger(StageOf(MoveStyle.Rise, Fire, toward), LegendScale),
                    }
                },
                // 天翔ける翼: 空から大きく急降下し、オーロラの光が散る
                {
                    MoveTable.SkySoar, toward => new List<Stage>
                    {
                        Bigger(StageOf(MoveStyle.Plunge, Bird, toward), LegendScale),
                        StageOf(MoveStyle.Twinkle, Aurora, toward),
                    }
                },
            };
    }
}
