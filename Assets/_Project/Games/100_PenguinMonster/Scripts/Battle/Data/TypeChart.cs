using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// タイプ相性表（仕様書 §3.4）。行が攻撃する技のタイプ、列が受けるペンモンのタイプ。
    /// 24体での見直しで、ひ が受けで強すぎたため「やみ・オーロラ → ひ」の いまひとつ を外した。
    /// 50体での見直しで、オーロラ が弱点1つ・ノーマル無効で勝ちすぎたため「かみなり → オーロラ」を ばつぐん にした
    /// </summary>
    public static class TypeChart
    {
        private const float X = 1f;
        private const float S = 2f;   // こうかは ばつぐん
        private const float H = 0.5f; // いまひとつ
        private const float N = 0f;   // こうかなし

        // 仕様書の表と見比べやすいよう、同じ並びで1行ずつ書く
        private static readonly float[,] Table =
        {
            //               ノーマル こおり うみ ひ かみなり こけ いわ とり やみ オーロラ
            /* ノーマル */ { X, X, X, X, X, X, H, X, X, N },
            /* こおり   */ { X, H, H, H, X, S, X, S, X, X },
            /* うみ     */ { X, X, H, S, X, H, S, X, X, X },
            /* ひ       */ { X, S, H, H, X, S, H, X, X, X },
            /* かみなり */ { X, X, S, X, H, H, N, S, X, S },
            /* こけ     */ { X, X, S, H, X, H, S, H, X, X },
            /* いわ     */ { X, S, X, S, X, H, X, S, X, X },
            /* とり     */ { X, X, X, X, H, S, H, X, S, X },
            /* やみ     */ { S, X, X, X, X, X, X, X, H, S },
            /* オーロラ */ { X, X, X, X, S, X, X, X, S, H },
        };

        public static float GetMultiplier(MonsterType attack, MonsterType defense)
        {
            return Table[(int)attack, (int)defense];
        }

        /// <summary>2タイプのペンモンには倍率を掛け合わせる（4倍・¼倍もある）</summary>
        public static float GetMultiplier(MonsterType attack, IReadOnlyList<MonsterType> defenseTypes)
        {
            float multiplier = 1f;
            foreach (var type in defenseTypes)
            {
                multiplier *= GetMultiplier(attack, type);
            }
            return multiplier;
        }
    }
}
