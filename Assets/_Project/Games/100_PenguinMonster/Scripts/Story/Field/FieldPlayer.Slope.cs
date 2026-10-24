namespace PenguinMonster.Story
{
    /// <summary>大橋の下り坂と展望マス（PLAN_STORY_TOWN04.md §3.3・§4）</summary>
    public sealed partial class FieldPlayer
    {
        /// <summary>下り坂の向き。大橋は東へ延びる1本道なので東だけ</summary>
        public const Direction DownhillDirection = Direction.Right;

        private LookoutGaze _gaze;

        public bool IsOnDownhill => Map.TileAt(Position) == TileKind.Downhill;

        public bool IsOnLookout => Map.TileAt(Position) == TileKind.Lookout;

        /// <summary>展望マスで景色を眺めている間。十字キーは向きだけ変える</summary>
        public bool IsGazing => _gaze != null;

        /// <summary>
        /// 次に進む向き。下り坂の上では、十字キーが無い・東のときは東へ滑る。上下（橋の左右）・西（上り）は押した向きへ。
        /// 滑るのは東が壁・人・出入口でないときだけ（出入口へは自分で押して入る）。進まないときは null
        /// </summary>
        public Direction? NextStep(Direction? held)
        {
            if (!IsOnDownhill || (held.HasValue && held.Value != DownhillDirection))
            {
                return held;
            }
            return CanSlide() ? DownhillDirection : held;
        }

        private bool CanSlide()
        {
            var next = Position + DownhillDirection.Offset();
            return Map.IsWalkable(next) && !HasPersonAt(next) && Map.WarpAt(next) == null;
        }

        /// <summary>
        /// その向きへの1歩が坂を下るか上るか。下り坂の上から東へは下り、下り坂の上から・下り坂へ西へは上り
        /// </summary>
        public Slope SlopeOf(Direction direction)
        {
            if (IsOnDownhill && direction == DownhillDirection)
            {
                return Slope.Down;
            }
            var next = Position + direction.Offset();
            bool intoDownhill = Map.Contains(next) && Map.TileAt(next) == TileKind.Downhill;
            if (direction == DownhillDirection.Opposite() && (IsOnDownhill || intoDownhill))
            {
                return Slope.Up;
            }
            return Slope.None;
        }

        /// <summary>
        /// 景色を眺め始める（展望マスに乗ったら View が呼ぶ）。隠しコマンドは自転車に乗ったまま来たときだけ数える
        /// </summary>
        public void StartGazing(bool riding)
        {
            _gaze = new LookoutGaze(riding);
        }

        /// <summary>眺めるのをやめる。コマンドの入力途中は忘れる</summary>
        public void StopGazing()
        {
            _gaze = null;
        }

        /// <summary>眺めている間の十字キー。動かずに向きだけ変え、隠しコマンドが通ったら true</summary>
        public bool TurnWhileGazing(Direction direction)
        {
            Facing = direction;
            return _gaze != null && _gaze.Turn(direction);
        }
    }

    /// <summary>展望マスで眺めている間の入力。歩いて来たときはコマンドを数えない</summary>
    public sealed class LookoutGaze
    {
        private readonly SecretCode _code;

        public LookoutGaze(bool riding)
        {
            _code = riding ? new SecretCode(SecretCode.Rainbow) : null;
        }

        public bool Turn(Direction direction) => _code != null && _code.Push(direction);
    }
}
