namespace PenguinMonster.Battle
{
    /// <summary>体型。体と頭の大きさの比率が変わる</summary>
    public enum BodyShape
    {
        Normal,
        Chubby,
        Slim,
    }

    /// <summary>体の色。実際の RGB は View 側のパレットで決める（ここは Unity に依存させない）</summary>
    public enum BodyColor
    {
        Navy,
        Black,
        Sky,
        Blue,
        Red,
        Yellow,
        Green,
        Brown,
        Purple,
        Pink,
    }

    /// <summary>頭のパーツ</summary>
    public enum HeadPart
    {
        None,
        Crest,
        Horn,
        Flame,
        Leaf,
        Bolt,
        Crown,
    }

    /// <summary>首のパーツ</summary>
    public enum NeckPart
    {
        None,
        Scarf,
        Bell,
    }

    /// <summary>
    /// ペンモンの見た目（仕様書 §6）。基本ペンギン＋パーツの組み合わせで、絵は View がここから生成する
    /// </summary>
    public sealed class MonsterLook
    {
        public static readonly MonsterLook Default = new MonsterLook(BodyShape.Normal, BodyColor.Navy, HeadPart.None, NeckPart.None);

        public BodyShape Shape { get; }
        public BodyColor Color { get; }
        public HeadPart Head { get; }
        public NeckPart Neck { get; }

        public MonsterLook(BodyShape shape, BodyColor color, HeadPart head, NeckPart neck)
        {
            Shape = shape;
            Color = color;
            Head = head;
            Neck = neck;
        }
    }
}
