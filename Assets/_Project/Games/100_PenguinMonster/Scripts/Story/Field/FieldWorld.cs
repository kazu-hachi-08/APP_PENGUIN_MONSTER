using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>ストーリーの全マップ。出入口の行き先を Id から引く</summary>
    public sealed class FieldWorld
    {
        private readonly Dictionary<string, FieldMap> _maps = new Dictionary<string, FieldMap>();

        public IEnumerable<FieldMap> Maps => _maps.Values;

        public FieldWorld(IEnumerable<FieldMap> maps)
        {
            foreach (var map in maps)
            {
                _maps.Add(map.Id, map);
            }
        }

        public bool Contains(string id) => _maps.ContainsKey(id);

        public FieldMap Get(string id) => _maps[id];
    }
}
