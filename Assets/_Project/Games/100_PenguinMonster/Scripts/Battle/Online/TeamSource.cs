namespace PenguinMonster.Battle
{
    /// <summary>
    /// オンラインの6体の出どころ（PLAN_STORY_TOWN01.md フェーズ9）。ホストが選び、DraftSeed に入れて送る。
    /// 自分のチームはストーリーの手持ちの6体を Lv50 にして戦う
    /// </summary>
    public enum TeamSource
    {
        Draft,
        OwnTeam,
    }
}
