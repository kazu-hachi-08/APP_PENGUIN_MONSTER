using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリーの全区切りのマップと人をまとめる入口。区切りをまたいで歩けるように、全部を1つの FieldWorld に入れる。
    /// 区切りを足したらここに足すだけで済むようにする（今は区切り1だけ）
    /// </summary>
    public static class StoryWorld
    {
        public static FieldWorld Create() => new FieldWorld(Town01Maps.CreateMaps());

        public static IReadOnlyList<FieldNpc> People() => Town01People.Create();
    }
}
