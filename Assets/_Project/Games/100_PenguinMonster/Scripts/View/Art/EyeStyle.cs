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
        Sparkle, // きらきら目: 光を2つ入れた大きめの目。ヒナ用
        Cross,  // 星目: 十字のきらめき。派手な子・オーロラの子
        Line,   // 糸目: 細い1本線。おだやかな子
        Worried, // こまり目: 上の辺が顔の内側へ上がる。臆病な子・泣き虫
    }
}
