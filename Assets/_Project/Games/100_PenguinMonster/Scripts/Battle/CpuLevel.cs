namespace PenguinMonster.Battle
{
    /// <summary>
    /// CPU の賢さ（仕様書 §4.8）。勝ち抜き戦では連勝数で上がる
    /// </summary>
    public enum CpuLevel
    {
        /// <summary>技をランダムに選ぶ</summary>
        Weak,
        /// <summary>一番ダメージが出る技を選ぶ。HPが減ったら回復、出てすぐなら天候の技を先に使う</summary>
        Normal,
        /// <summary>ふつう ＋ タイプで不利なら交代する。ひんしの後も相性のいいペンモンを出す</summary>
        Strong,
    }
}
