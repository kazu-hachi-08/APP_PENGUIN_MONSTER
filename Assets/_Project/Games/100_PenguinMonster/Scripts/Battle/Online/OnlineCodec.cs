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

        /// <summary>ペンモン1体は 6 int（No・持ち物・技4つ）。技は MoveTable.All の番号で、空きは -1</summary>
        public const int FieldsPerMonster = 2 + MaxMoves;
        private const int MaxMoves = 4;
        private const int NoMove = -1;

        /// <summary>
        /// ペンモンは「No・持ち物・技」の組の列で送る（ドラフトの6体・自分のチームの6体・選出の3体）。
        /// 持ち物はドラフトの後に配るので定義表には無く、送らないとホストの計算でゲストの持ち物が働かない。
        /// 技も「自分のチーム」ではストーリーで覚えた技なので定義表と違い、送らないと相手の端末で別の技になる
        /// </summary>
        public static int[] EncodeMonsters(IReadOnlyList<MonsterDefinition> monsters)
        {
            var data = new int[monsters.Count * FieldsPerMonster];
            for (int i = 0; i < monsters.Count; i++)
            {
                int offset = i * FieldsPerMonster;
                data[offset] = monsters[i].No;
                data[offset + 1] = (int)monsters[i].Item;
                for (int m = 0; m < MaxMoves; m++)
                {
                    data[offset + 2 + m] = m < monsters[i].Moves.Count ? MoveTable.IndexOf(monsters[i].Moves[m]) : NoMove;
                }
            }
            return data;
        }

        public static List<MonsterDefinition> DecodeMonsters(int[] data)
        {
            var monsters = new List<MonsterDefinition>(data.Length / FieldsPerMonster);
            for (int offset = 0; offset + FieldsPerMonster <= data.Length; offset += FieldsPerMonster)
            {
                var monster = MonsterTable.ByNo(data[offset]);
                var item = (HeldItem)data[offset + 1];
                var moves = DecodeMoves(data, offset + 2);
                // ドラフトの子は定義表のままの方が、表の子と同じものとして扱える（選出・図鑑の照合など）
                bool asDefined = item == HeldItem.None && SameMoves(monster.Moves, moves);
                monsters.Add(asDefined ? monster : monster.WithLoadout(BattleBalance.Level, moves, item));
            }
            return monsters;
        }

        private static MoveDefinition[] DecodeMoves(int[] data, int offset)
        {
            var moves = new List<MoveDefinition>(MaxMoves);
            for (int m = 0; m < MaxMoves; m++)
            {
                var move = MoveTable.ByIndex(data[offset + m]);
                if (move != null)
                {
                    moves.Add(move);
                }
            }
            return moves.ToArray();
        }

        private static bool SameMoves(IReadOnlyList<MoveDefinition> a, IReadOnlyList<MoveDefinition> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
