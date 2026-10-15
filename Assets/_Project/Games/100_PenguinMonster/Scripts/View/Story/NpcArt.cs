using System.Collections.Generic;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 村の人の絵。主人公と同じ形で、帽子・髪・上着の色だけ替える（人ごとに描き分けるのは後回し。PLAN_STORY_TOWN01.md フェーズ4）。
    /// 遠くからでも誰か分かるよう、上着の色を人ごとにはっきり変える
    /// </summary>
    public static class NpcArt
    {
        private static readonly Dictionary<NpcLook, Dictionary<char, Color32>> Changes = new Dictionary<NpcLook, Dictionary<char, Color32>>
        {
            // 母: 帽子の代わりに髪をまとめた形に見せる（帽子を髪と同じ色に）。縁取りは髪留め・胸元に出るので淡い桃色、上着は桃色
            { NpcLook.Mom, Colors(hat: new Color32(120, 70, 48, 255), trim: new Color32(250, 230, 236, 255), hair: new Color32(120, 70, 48, 255),
                coat: new Color32(236, 130, 160, 255), hem: new Color32(196, 90, 120, 255)) },
            // 博士: 白髪に白衣
            { NpcLook.Professor, Colors(hat: new Color32(200, 200, 205, 255), trim: new Color32(230, 230, 235, 255), hair: new Color32(200, 200, 205, 255),
                coat: new Color32(240, 240, 244, 255), hem: new Color32(170, 175, 190, 255)) },
            // 助手: 緑の帽子に白衣
            { NpcLook.Assistant, Colors(hat: new Color32(70, 160, 110, 255), trim: new Color32(248, 248, 252, 255), hair: new Color32(40, 36, 40, 255),
                coat: new Color32(240, 240, 244, 255), hem: new Color32(70, 160, 110, 255)) },
            // 村の人: 黄色い帽子に緑の上着
            { NpcLook.Villager, Colors(hat: new Color32(232, 180, 60, 255), trim: new Color32(248, 248, 252, 255), hair: new Color32(60, 44, 34, 255),
                coat: new Color32(90, 150, 80, 255), hem: new Color32(60, 110, 56, 255)) },
        };

        private static readonly Dictionary<NpcLook, Sprite[,]> Cache = new Dictionary<NpcLook, Sprite[,]>();

        /// <summary>立ちの絵（村の人はまだ歩かない）</summary>
        public static Sprite Get(NpcLook look, Direction facing)
        {
            if (!Cache.TryGetValue(look, out var sprites) || sprites[0, 0] == null)
            {
                sprites = HeroArt.CreateAll($"Npc_{look}", HeroArt.Recolor(Changes[look]));
                Cache[look] = sprites;
            }
            return sprites[(int)facing, 0];
        }

        private static Dictionary<char, Color32> Colors(Color32 hat, Color32 trim, Color32 hair, Color32 coat, Color32 hem)
        {
            return new Dictionary<char, Color32> { { 'r', hat }, { 'w', trim }, { 'h', hair }, { 'b', coat }, { 'B', hem } };
        }
    }
}
