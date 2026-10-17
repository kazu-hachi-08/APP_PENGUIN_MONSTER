namespace PenguinMonster.Story
{
    /// <summary>よろず屋の売り買い。1回で1個ずつ</summary>
    public static class Shop
    {
        /// <summary>買う。お金が足りなければ買わずに false</summary>
        public static bool Buy(StoryState state, StoryItem item)
        {
            if (!state.SpendMoney(StoryItems.Get(item).Price))
            {
                return false;
            }
            state.Bag.Add(item);
            return true;
        }

        /// <summary>売る（買う値段の半分）。持っていなければ false</summary>
        public static bool Sell(StoryState state, StoryItem item)
        {
            if (!state.Bag.Remove(item))
            {
                return false;
            }
            state.EarnMoney(StoryItems.Get(item).SellPrice);
            return true;
        }
    }
}
