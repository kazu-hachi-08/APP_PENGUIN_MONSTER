using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>今その場に立っている人。どの人がどの置き場所を使っているか</summary>
    public readonly struct PlacedNpc
    {
        public readonly FieldNpc Npc;
        public readonly NpcPlacement Placement;

        public PlacedNpc(FieldNpc npc, NpcPlacement placement)
        {
            Npc = npc;
            Placement = placement;
        }

        public GridPos Position => Placement.Spot.Position;
    }

    /// <summary>
    /// フィールドの全員。置き場所は聞かれるたびに今のフラグで決め直す（出来事でフラグが立てば、次に描くときには動いている）
    /// </summary>
    public sealed class FieldPeople
    {
        private readonly IReadOnlyList<FieldNpc> _npcs;
        private readonly StoryFlags _flags;

        public FieldPeople(IReadOnlyList<FieldNpc> npcs, StoryFlags flags)
        {
            _npcs = npcs;
            _flags = flags;
        }

        public IEnumerable<PlacedNpc> On(string mapId)
        {
            foreach (var npc in _npcs)
            {
                var placement = npc.PlacementFor(_flags);
                if (placement != null && placement.Spot.MapId == mapId)
                {
                    yield return new PlacedNpc(npc, placement);
                }
            }
        }

        /// <summary>そのマスに立っている人。居なければ null</summary>
        public PlacedNpc? At(string mapId, GridPos pos)
        {
            foreach (var placed in On(mapId))
            {
                if (placed.Position == pos)
                {
                    return placed;
                }
            }
            return null;
        }
    }
}
