namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリー1回分の持ち物。フラグ・手持ち・お金・最後に回復した場所（バッグ・あずかり箱はフェーズ6、セーブはフェーズ7で足す）
    /// </summary>
    public sealed class StoryState
    {
        public StoryFlags Flags { get; } = new StoryFlags();
        public Party Party { get; } = new Party();
        public int Money { get; private set; } = StoryBalance.StartMoney;
        /// <summary>全滅したときに戻る場所。回復したところで書き換える</summary>
        public FieldSpot LastHealSpot { get; set; }

        /// <param name="home">最初の戻り先（自分の家）</param>
        public StoryState(FieldSpot home)
        {
            LastHealSpot = home;
        }

        public void EarnMoney(int amount)
        {
            Money += amount;
        }

        /// <summary>全滅した。お金を半分にし、手持ちを全回復して、戻る場所を返す（本家と同じく戻った時点で元気になっている）</summary>
        public FieldSpot BlackOut()
        {
            Money /= StoryBalance.BlackOutMoneyDivisor;
            Party.HealAll();
            return LastHealSpot;
        }
    }
}
