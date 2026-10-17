using System.Collections.Generic;
using System.Reflection;

namespace PenguinMonster.Battle
{
    /// <summary>技を名前から引く（ストーリーのセーブは技を名前で持つ。並びの番号だと、技を足したときに古い記録の技がずれるため）</summary>
    public static partial class MoveTable
    {
        private static Dictionary<string, MoveDefinition> _byName;

        /// <summary>無ければ null</summary>
        public static MoveDefinition ByName(string name)
        {
            if (_byName == null)
            {
                _byName = CollectByName();
            }
            return name != null && _byName.TryGetValue(name, out var move) ? move : null;
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
