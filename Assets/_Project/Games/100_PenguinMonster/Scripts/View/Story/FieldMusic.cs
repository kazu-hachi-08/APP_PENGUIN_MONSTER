using PenguinMonster.Story;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// マップごとのフィールドの曲。
    /// 道路と道場のほかは村の曲にする（町・建物の中も村の曲。曲を増やしすぎない）
    /// </summary>
    public static class FieldMusic
    {
        public static MusicTrack TrackOf(string mapId)
        {
            switch (mapId)
            {
                case Town01Maps.Route01:
                case Town01Maps.Route02:
                case Town02Maps.Route03:
                case Town02Maps.Route04:
                    return MusicTrack.Route;
                case Town01Maps.Gym:
                case Town02Maps.Gym:
                    return MusicTrack.Gym;
                default:
                    return MusicTrack.Village;
            }
        }
    }
}
