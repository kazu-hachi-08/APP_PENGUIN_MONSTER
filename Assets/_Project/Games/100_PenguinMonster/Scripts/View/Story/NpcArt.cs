using System.Collections.Generic;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
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
            // ライバル: 主人公（赤）と並んで見分けやすい青の帽子に紺の上着
            { NpcLook.Rival, Colors(hat: new Color32(60, 110, 210, 255), trim: new Color32(248, 248, 252, 255), hair: new Color32(230, 170, 70, 255),
                coat: new Color32(50, 60, 120, 255), hem: new Color32(36, 42, 90, 255)) },
            // 道路のトレーナー: 橙の帽子に水色の上着（雪あそびの子）
            { NpcLook.Youngster, Colors(hat: new Color32(240, 130, 50, 255), trim: new Color32(248, 248, 252, 255), hair: new Color32(60, 44, 34, 255),
                coat: new Color32(110, 190, 230, 255), hem: new Color32(70, 140, 190, 255)) },
        };

        private static readonly Dictionary<NpcLook, Sprite[,]> Cache = new Dictionary<NpcLook, Sprite[,]>();

        // 「！」の吹き出し（トレーナーと目が合ったとき）。人の絵と同じ 16 ドット幅
        private const int MarkSize = 16;
        private static readonly Color32 MarkFill = new Color32(255, 255, 255, 255);
        private static readonly Color32 MarkLine = new Color32(40, 40, 52, 255);
        private static readonly Color32 MarkRed = new Color32(220, 50, 50, 255);
        private static Sprite _exclamation;

        /// <param name="frame">0 が立ち、1 が歩き（目が合ったトレーナーが近づくときだけ歩く）</param>
        public static Sprite Get(NpcLook look, Direction facing, int frame = 0)
        {
            if (!Cache.TryGetValue(look, out var sprites) || sprites[0, 0] == null)
            {
                sprites = HeroArt.CreateAll($"Npc_{look}", HeroArt.Recolor(Changes[look]));
                Cache[look] = sprites;
            }
            return sprites[(int)facing, frame];
        }

        /// <summary>頭の上に出す「！」。下の真ん中を足元にして、人の絵の上に重ねる</summary>
        public static Sprite Exclamation()
        {
            if (_exclamation != null)
            {
                return _exclamation;
            }
            var canvas = new PixelCanvas(MarkSize, MarkSize);
            canvas.FillRoundedRect(3, 3, 10, 12, 3f, MarkFill);
            canvas.FillTriangle(new Vector2(6f, 3f), new Vector2(10f, 3f), new Vector2(7f, 0f), MarkFill);
            canvas.Outline(MarkLine);
            canvas.FillRect(7, 8, 2, 5, MarkRed);
            canvas.FillRect(7, 5, 2, 2, MarkRed);
            _exclamation = canvas.ToSprite("Npc_Exclamation", new Vector2(0.5f, 0f));
            return _exclamation;
        }

        private static Dictionary<char, Color32> Colors(Color32 hat, Color32 trim, Color32 hair, Color32 coat, Color32 hem)
        {
            return new Dictionary<char, Color32> { { 'r', hat }, { 'w', trim }, { 'h', hair }, { 'b', coat }, { 'B', hem } };
        }
    }
}
