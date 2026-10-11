namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 目の種類（仕様書 §6）。全員が同じ目だとシルエットを変えても同じ顔に見えたので、1体ずつ選ぶ
    /// </summary>
    public enum EyeStyle
    {
        Dot,    // 点目: ゆるい・素朴な子
        Round,  // 丸目: 光の入った小さい目。主役っぽい子
        Smile,  // にっこり目（^ ^）
        Sleepy, // ねむそう目: のんびり・夜の子
        Sharp,  // キリッ目: 強そうな子
    }
}
