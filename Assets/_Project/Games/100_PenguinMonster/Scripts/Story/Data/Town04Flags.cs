namespace PenguinMonster.Story
{
    /// <summary>名前は頭に town04. を付けて、他の町のフラグと重ならないようにする</summary>
    public static class Town04Flags
    {
        /// <summary>カザハナ町の自転車屋で自転車をもらった。ここから外のマップで自転車に乗れる</summary>
        public const string Bicycle = "town04.bicycle";

        /// <summary>7番道路の釣り人から釣り竿をもらった。ここから水に向いて A で釣れる</summary>
        public const string FishingRod = "town04.fishingRod";

        /// <summary>大橋の先の展望台でハヤテに会った。カザハナ町の道場の前の門下生がどく</summary>
        public const string MetHayate = "town04.metHayate";

        /// <summary>道路の落とし物を拾った（後ろに落とし物の名前が付く）</summary>
        public const string ItemPrefix = "town04.item.";
    }
}
