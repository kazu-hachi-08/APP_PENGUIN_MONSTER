using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// オンラインで送る中身（行動・ペンモンの並び）と int 列の変換（仕様書 §8）。
    /// 通信は int 列だけを運ぶので、ここで意味のある形に戻す
    /// </summary>
    public static class OnlineCodec
    {
        public static int[] EncodeAction(BattleAction action) => new[] { (int)action.Kind, action.Index };

        public static BattleAction DecodeAction(int[] data)
        {
            return (BattleActionKind)data[0] == BattleActionKind.Switch
                ? BattleAction.SwitchTo(data[1])
                : BattleAction.UseMove(data[1]);
        }

        /// <summary>ペンモンは No の列で送る（ドラフトの6体・選出の3体。どちらも同じペンモンは重ならない）</summary>
        public static int[] EncodeMonsters(IReadOnlyList<MonsterDefinition> monsters)
        {
            var nos = new int[monsters.Count];
            for (int i = 0; i < nos.Length; i++)
            {
                nos[i] = monsters[i].No;
            }
            return nos;
        }

        public static List<MonsterDefinition> DecodeMonsters(int[] nos)
        {
            var monsters = new List<MonsterDefinition>(nos.Length);
            foreach (int no in nos)
            {
                monsters.Add(MonsterTable.ByNo(no));
            }
            return monsters;
        }
    }
}
