using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// ペンモンの絵を実行時に作って使い回す。体数が少ないうちは PNG にせず、見た目の定義を変えたら Play し直すだけで反映させる
    /// </summary>
    public static class MonsterSprites
    {
        // 足元の中央を原点にして、台座の上に立たせやすくする
        private static readonly Vector2 FootPivot = new Vector2(0.5f, 0f);

        private static readonly Dictionary<(MonsterLook, bool), Sprite> Cache = new Dictionary<(MonsterLook, bool), Sprite>();

        public static Sprite Front(MonsterDefinition monster) => Get(monster.Look, true);

        public static Sprite Back(MonsterDefinition monster) => Get(monster.Look, false);

        private static Sprite Get(MonsterLook look, bool front)
        {
            if (Cache.TryGetValue((look, front), out var sprite) && sprite != null)
            {
                return sprite;
            }

            sprite = PenguinPainter.Paint(look, front).ToSprite(front ? "Penguin_Front" : "Penguin_Back", FootPivot);
            Cache[(look, front)] = sprite;
            return sprite;
        }
    }
}
