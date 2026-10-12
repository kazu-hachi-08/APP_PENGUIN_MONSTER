using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// オンラインで送る中身（行動・ペンモンの並び）と int 列の変換（仕様書 §8）。
    /// 通信は int 列だけを運ぶので、ここで意味のある形に戻す
    /// </summary>
    public static class OnlineCodec
    {
        /// <summary>行動1つは 3 int（種類・技の番号か選出の何番目か・狙う相手の場）</summary>
        public const int FieldsPerAction = 3;

        public static int[] EncodeAction(BattleAction action) => EncodeActions(new[] { action });

        public static BattleAction DecodeAction(int[] data) => DecodeActions(data)[0];

        /// <summary>場の順の行動（シングルは1つ、ダブルスは2つ）を1つの列にする</summary>
        public static int[] EncodeActions(IReadOnlyList<BattleAction> actions)
        {
            var data = new int[actions.Count * FieldsPerAction];
            for (int i = 0; i < actions.Count; i++)
            {
                data[i * FieldsPerAction] = (int)actions[i].Kind;
                data[i * FieldsPerAction + 1] = actions[i].Index;
                data[i * FieldsPerAction + 2] = actions[i].Target;
            }
            return data;
        }

        public static List<BattleAction> DecodeActions(int[] data)
        {
            var actions = new List<BattleAction>();
            for (int offset = 0; offset + FieldsPerAction <= data.Length; offset += FieldsPerAction)
            {
                actions.Add((BattleActionKind)data[offset] == BattleActionKind.Switch
                    ? BattleAction.SwitchTo(data[offset + 1])
                    : BattleAction.UseMove(data[offset + 1], data[offset + 2]));
            }
            return actions;
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
