using System.Collections.Generic;
using System.Reflection;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// 技を名前・番号から引く。ストーリーのセーブは名前で持つ（並びの番号だと、技を足したときに古い記録の技がずれるため）。
    /// オンラインは番号で送る（その場の通信だけなので、同じ版どうしで並びがそろっていればよい）
    /// </summary>
    public static partial class MoveTable
    {
        private static Dictionary<string, MoveDefinition> _byName;
        private static List<MoveDefinition> _all;
        private static Dictionary<MoveDefinition, int> _indexOf;

        /// <summary>
        /// 全部の技。番号 ＝ この並び。リフレクションの項目の順は決まっていないので、名前の順に並べて
        /// どの端末（Android / Windows / WebGL）でも同じ番号になるようにする
        /// </summary>
        public static IReadOnlyList<MoveDefinition> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<MoveDefinition>(ByNameTable().Values);
                    _all.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
                }
                return _all;
            }
        }

        /// <summary>無ければ null</summary>
        public static MoveDefinition ByName(string name)
        {
            return name != null && ByNameTable().TryGetValue(name, out var move) ? move : null;
        }

        /// <summary>All での番号。表に無ければ -1</summary>
        public static int IndexOf(MoveDefinition move)
        {
            if (_indexOf == null)
            {
                _indexOf = new Dictionary<MoveDefinition, int>();
                for (int i = 0; i < All.Count; i++)
                {
                    _indexOf[All[i]] = i;
                }
            }
            return move != null && _indexOf.TryGetValue(move, out int index) ? index : -1;
        }

        /// <summary>番号から引く。範囲の外なら null</summary>
        public static MoveDefinition ByIndex(int index)
        {
            return index >= 0 && index < All.Count ? All[index] : null;
        }

        private static Dictionary<string, MoveDefinition> ByNameTable()
        {
            if (_byName == null)
            {
                _byName = CollectByName();
            }
            return _byName;
        }

        /// <summary>技は定義表の public static の項目そのものなので、手で一覧を持たずに項目を全部集める（足し忘れを無くすため）</summary>
        private static Dictionary<string, MoveDefinition> CollectByName()
        {
            var byName = new Dictionary<string, MoveDefinition>();
            foreach (var field in typeof(MoveTable).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.GetValue(null) is MoveDefinition move)
                {
                    byName[move.Name] = move;
                }
            }
            return byName;
        }
    }
}
