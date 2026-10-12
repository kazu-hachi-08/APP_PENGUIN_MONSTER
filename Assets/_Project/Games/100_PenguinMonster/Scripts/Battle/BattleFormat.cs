namespace PenguinMonster.Battle
{
    /// <summary>
    /// 対戦のルール（仕様書 §4.9）。シングルは 3体選出・場に1体、ダブルスは 4体選出・場に2体
    /// </summary>
    public enum BattleFormat
    {
        Single,
        Double,
    }

    public static class BattleFormats
    {
        public static int ActiveCount(this BattleFormat format)
        {
            return format == BattleFormat.Double ? BattleBalance.DoublesActiveCount : BattleBalance.SingleActiveCount;
        }

        public static int PartySize(this BattleFormat format)
        {
            return format == BattleFormat.Double ? BattleBalance.DoublesPartySize : BattleBalance.PartySize;
        }

        public static string ToJapanese(this BattleFormat format)
        {
            return format == BattleFormat.Double ? "ダブルス" : "シングル";
        }
    }
}
