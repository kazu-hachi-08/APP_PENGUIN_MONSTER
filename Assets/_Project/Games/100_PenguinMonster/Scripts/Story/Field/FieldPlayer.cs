using System;
using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>1歩の結果。向きはどちらでも変わる</summary>
    public enum StepResult
    {
        /// <summary>壁などで進めず、向きだけ変えた</summary>
        Blocked,
        Moved,
    }

    /// <summary>
    /// フィールドの主人公の位置と向き。1マスずつ歩き、出入口に乗ったらマップを移る。人の立っているマスは通れない。
    /// 歩く動き・暗転は View が受け持ち、ここはマスの上の位置だけを決める
    /// </summary>
    public sealed class FieldPlayer
    {
        private readonly FieldWorld _world;
        private readonly FieldPeople _people;

        public FieldMap Map { get; private set; }
        public GridPos Position { get; private set; }
        public Direction Facing { get; private set; }

        /// <summary>今いるマスの出入口。View は歩き終えたらこれを見て、暗転してから Enter を呼ぶ</summary>
        public Warp WarpHere => Map.WarpAt(Position);

        /// <summary>向いている1マス先（A で調べるマス）</summary>
        public GridPos Ahead => Position + Facing.Offset();

        /// <summary>向いている先に立っている人。居なければ null</summary>
        public PlacedNpc? PersonAhead => _people?.At(Map.Id, Ahead);

        /// <param name="people">人の居ないマップ（テスト）では省く</param>
        public FieldPlayer(FieldWorld world, FieldSpot start, FieldPeople people = null)
        {
            _world = world;
            _people = people;
            MoveTo(start);
        }

        public StepResult Step(Direction direction)
        {
            Facing = direction;
            var next = Position + direction.Offset();
            if (!Map.IsWalkable(next) || _people?.At(Map.Id, next) != null)
            {
                return StepResult.Blocked;
            }
            Position = next;
            return StepResult.Moved;
        }

        /// <summary>A で調べたときの台本。人が居ればその人の、看板ならその文。何も無ければ null</summary>
        public IReadOnlyList<EventStep> ScriptAhead()
        {
            var person = PersonAhead;
            if (person != null)
            {
                return person.Value.Placement.Script;
            }
            var sign = Map.SignAt(Ahead);
            return sign != null ? EventStep.Lines(sign) : null;
        }

        /// <summary>
        /// 今いるマスで野生に出会うか（草むらを1歩進むごとに View が呼ぶ）。草むらでない・出現表が無い・
        /// 戦える子がいないときは出会わない。出会わなければ null
        /// </summary>
        public OwnedMonster RollEncounter(Random random, Party party)
        {
            var table = Map.Encounters;
            if (table == null || Map.TileAt(Position) != TileKind.Grass || !party.CanFight)
            {
                return null;
            }
            return table.Roll(random);
        }

        /// <summary>今の場所で目が合ったトレーナー。居なければ null</summary>
        public TrainerSighting? SpottedBy() => _people?.SpottedBy(Map, Position);

        /// <summary>その場で向きだけ変える（近づいてきたトレーナーの方を向く）</summary>
        public void Face(Direction direction)
        {
            Facing = direction;
        }

        public void Enter(Warp warp)
        {
            MoveTo(warp.To);
        }

        /// <summary>出入口を通らずにその場所へ移る（全滅して最後に回復した場所へ戻る）</summary>
        public void MoveTo(FieldSpot spot)
        {
            Map = _world.Get(spot.MapId);
            Position = spot.Position;
            Facing = spot.Facing;
        }
    }
}
