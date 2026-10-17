using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリーの全区切りのマップと人をまとめる入口。区切りをまたいで歩けるように、全部を1つの FieldWorld に入れる。
    /// 区切りを足したらここに足すだけで済むようにする
    /// </summary>
    public static class StoryWorld
    {
        public static FieldWorld Create()
        {
            var maps = new List<FieldMap>(Town01Maps.CreateMaps());
            maps.AddRange(Town02Maps.CreateMaps());
            return new FieldWorld(maps);
        }

        public static IReadOnlyList<FieldNpc> People()
        {
            var people = new List<FieldNpc>(Town01People.Create());
            people.AddRange(Town02People.Create());
            return people;
        }
    }
}
