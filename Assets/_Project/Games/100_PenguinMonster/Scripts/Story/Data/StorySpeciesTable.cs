namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリーの全区切りの種を No から引く入口。記録を戻す・進化先を探すときはここを使い、
    /// 区切りを足したら ByNo に1つ足すだけで済むようにする
    /// </summary>
    public static class StorySpeciesTable
    {
        /// <summary>No から種を引く。どの区切りにも無ければ null</summary>
        public static StorySpecies ByNo(int no) => Town01Species.ByNo(no) ?? Town02Species.ByNo(no);
    }
}
