using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// ペンモンの絵を実行時に作って使い回す。体数が少ないうちは PNG にせず、デザインのコードを変えたら Play し直すだけで反映させる
    /// </summary>
    public static class MonsterSprites
    {
        // 足元の中央を原点にして、台座の上に立たせやすくする
        private static readonly Vector2 FootPivot = new Vector2(0.5f, 0f);

        private static readonly Dictionary<(int, bool), Sprite> Cache = new Dictionary<(int, bool), Sprite>();

        public static Sprite Front(MonsterDefinition monster) => Get(monster.No, true);

        public static Sprite Back(MonsterDefinition monster) => Get(monster.No, false);

        private static Sprite Get(int no, bool front)
        {
            if (Cache.TryGetValue((no, front), out var sprite) && sprite != null)
            {
                return sprite;
            }

            sprite = PenguinDesigns.Paint(no, front).ToSprite(front ? "Penguin_Front" : "Penguin_Back", FootPivot);
            Cache[(no, front)] = sprite;
            return sprite;
        }
    }
}
