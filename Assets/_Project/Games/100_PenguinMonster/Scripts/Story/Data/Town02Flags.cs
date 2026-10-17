namespace PenguinMonster.Story
{
    /// <summary>区切り2のフラグ。名前は頭に town02. を付けて、区切り1のフラグと重ならないようにする</summary>
    public static class Town02Flags
    {
        /// <summary>4番道路の奥の出来事を見た。カガミコ町の道場の前の門下生がどく</summary>
        public const string Route04Cleared = "town02.route04Cleared";
        /// <summary>カガミコ町の家のおじいさんから No.209（オマモリペン）をもらった。1回しかもらえない</summary>
        public const string GotOmamori = "town02.gotOmamori";
        /// <summary>道路の落とし物を拾った（後ろに落とし物の名前が付く）</summary>
        public const string ItemPrefix = "town02.item.";
    }
}
