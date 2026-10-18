namespace PenguinMonster.Story
{
    /// <summary>名前は頭に town03. を付けて、他の町のフラグと重ならないようにする</summary>
    public static class Town03Flags
    {
        /// <summary>ユキオコシ町で母からランニングシューズをもらった。ここから走れる</summary>
        public const string RunningShoes = "town03.runningShoes";
        /// <summary>発電所に居座る子を片付けて、ユキオコシ町の電気が戻った。道場主ライゾウが道場の前からどく</summary>
        public const string PowerRestored = "town03.powerRestored";
        /// <summary>ユキオコシ町の家のおばあさんから No.238（カンテラペン）をもらった。1回しかもらえない</summary>
        public const string GotKantera = "town03.gotKantera";
        /// <summary>道路の落とし物を拾った（後ろに落とし物の名前が付く）</summary>
        public const string ItemPrefix = "town03.item.";
    }
}
