namespace PenguinMonster.Battle
{
    /// <summary>
    /// ペンモンを入れて繰り出す「タマゴ」の種類（仕様書 §7）。見た目だけでバトルの計算には効かない。
    /// いまはシングル・ダブルスとも Normal だけ。あとでストーリーの相手ごとに変えられるよう、
    /// 相手の定義表（純C#）からも参照できるこちらに置き、色は View 側の EggStyleTable で引く
    /// </summary>
    public enum EggKind
    {
        Normal,
    }
}
