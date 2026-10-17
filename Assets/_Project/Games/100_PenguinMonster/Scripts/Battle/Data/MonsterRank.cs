namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモンの強さのランク（仕様書 §3.5）。ドラフトの出やすさと、絵の大きさ・飾りに使う
    /// </summary>
    public enum MonsterRank
    {
        /// <summary>種族値は低いが、特性・持ち物・技と編成しだいで化ける</summary>
        Hina,
        Normal,
        /// <summary>純粋に強い。代わりにはっきりした弱点がある</summary>
        Ace,
        /// <summary>ボスの切り札。ドラフトでは出にくく、1チーム1体まで</summary>
        Legend,
        /// <summary>
        /// ストーリーの進化前（No.151〜）。種族値は低いが育てて進化させる前提なので、ヒナの綿毛は付けない。
        /// ドラフトには出さない（ストーリーの子はストーリーで育てるもののため）
        /// </summary>
        Young,
    }
}
