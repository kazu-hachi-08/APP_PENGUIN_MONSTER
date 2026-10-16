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

    /// <summary>トレーナーと目が合った。トレーナーは Steps マス歩いて、主人公の隣まで来る</summary>
    public readonly struct TrainerSighting
    {
        public readonly PlacedNpc Trainer;
        public readonly int Steps;

        public TrainerSighting(PlacedNpc trainer, int steps)
        {
            Trainer = trainer;
            Steps = steps;
        }
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

        /// <summary>
        /// 主人公と目が合ったトレーナー。トレーナーの向いている先を見える距離までたどり、
        /// 通れないマス・ほかの人で視線が切れる前に主人公がいれば目が合う。居なければ null
        /// </summary>
        public TrainerSighting? SpottedBy(FieldMap map, GridPos playerPos)
        {
            foreach (var placed in On(map.Id))
            {
                var trainer = placed.Placement.Trainer;
                if (trainer == null)
                {
                    continue;
                }
                int distance = SightDistance(map, placed, trainer.SightRange, playerPos);
                if (distance > 0)
                {
                    return new TrainerSighting(placed, distance - 1);
                }
            }
            return null;
        }

        /// <summary>見える範囲に主人公がいれば、何マス先か。いなければ 0</summary>
        private int SightDistance(FieldMap map, PlacedNpc placed, int range, GridPos playerPos)
        {
            var offset = placed.Placement.Spot.Facing.Offset();
            var pos = placed.Position;
            for (int distance = 1; distance <= range; distance++)
            {
                pos += offset;
                if (pos == playerPos)
                {
                    return distance;
                }
                if (!map.IsWalkable(pos) || At(map.Id, pos) != null)
                {
                    return 0;
                }
            }
            return 0;
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
