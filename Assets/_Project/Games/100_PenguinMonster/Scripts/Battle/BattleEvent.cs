namespace PenguinMonster.Battle
{
    public enum BattleEventKind
    {
        SentOut,
        Withdrawn,
        MoveUsed,
        Missed,
        Damaged,
        Critical,
        SuperEffective,
        NotVeryEffective,
        NoEffect,
        Fainted,
        BattleEnd,
        /// <summary>変化技が決まらなかった（すでに状態異常・タイプで効かない など）</summary>
        Failed,
        StatusInflicted,
        /// <summary>まひ・ねむり・こおりで動けなかった（理由は Status）</summary>
        Immobilized,
        WokeUp,
        Thawed,
        Confused,
        /// <summary>こんらん中に技を使おうとした（「こんらんしている！」）</summary>
        ConfusionCheck,
        HurtItself,
        ConfusionEnded,
        /// <summary>ターン終わりの どく・やけど のダメージ（理由は Status）</summary>
        StatusDamage,
        /// <summary>ランクが変わった。Amount が変わった量（＋で上がる）</summary>
        StatChanged,
        /// <summary>ランクがもう変わらない。Amount の符号が上げようとしたか下げようとしたか</summary>
        StatUnchanged,
        /// <summary>特性が働いた。文は出さず、画面に特性の帯（「〇〇の / 強面」）を出す。続くイベントがその効果</summary>
        AbilityActivated,
        /// <summary>特性 とげとげはだ で受けたダメージ</summary>
        AbilityDamage,
        /// <summary>持ち物で HP が回復した（ほしざかな・おやつのイカ）</summary>
        ItemHealed,
        /// <summary>持ち物で状態異常が治った（さっぱりこんぶ）</summary>
        ItemCured,
        /// <summary>持ち物で HP1 で残った（ふんばりマフラー）</summary>
        ItemEndured,
        /// <summary>持ち物の反動で HP が減った（いのちのつらら）</summary>
        ItemRecoil,
        /// <summary>連続技が当たった回数。Amount が回数、主語は受けた側</summary>
        HitCount,
        /// <summary>吸収の技で HP が回復した</summary>
        Drained,
        /// <summary>技の反動で HP が減った</summary>
        Recoil,
        /// <summary>回復の技で HP が回復した</summary>
        Healed,
        /// <summary>天候が始まった。Amount が天候（Weather）。主語は始めたペンモン</summary>
        WeatherStarted,
        /// <summary>天候が止んだ。Amount が止んだ天候。主語はいない</summary>
        WeatherEnded,
        /// <summary>吹雪のターン終わりのダメージ</summary>
        WeatherDamage,
        /// <summary>守る が決まった（「守りの 体勢に 入った！」）</summary>
        ProtectStarted,
        /// <summary>守っている相手に技が防がれた。主語は守った側</summary>
        Protected,
        /// <summary>追い風が吹き始めた。主語は始めたペンモン</summary>
        TailwindStarted,
        /// <summary>追い風が止んだ。Side が止んだ陣営、主語はいない</summary>
        TailwindEnded,
        /// <summary>手助けを受けた。主語は受けた味方</summary>
        Helped,
        /// <summary>羽リレーでランクを引き継いだ。主語は出てきた子。Amount に7つのランクを詰めてある（文は出さない）</summary>
        StagesPassed,
    }

    /// <summary>
    /// バトルで起きたことを1つずつ表す。表示（メッセージ・HPバー・状態異常の印）は View がこれを順に再生して行う。
    /// HP と状態異常は起きた時点の値を持たせる（BattleMonster の値はターン終わりの値なので、途中の表示に使えない）
    /// </summary>
    public sealed class BattleEvent
    {
        // -6〜+6 を 0〜12 にずらして入れるので 4bit で足りる
        private const int StageBits = 4;
        private const int StageMask = (1 << StageBits) - 1;

        public BattleEventKind Kind { get; }
        /// <summary>主語になる陣営。BattleEnd のときは勝った陣営</summary>
        public int Side { get; }
        /// <summary>主語のペンモンが場の何番目にいたか（ダブルスの表示用。シングルはいつも 0）</summary>
        public int Slot { get; }
        public BattleMonster Monster { get; }
        public string MoveName { get; }
        public int Hp { get; }
        public StatusCondition Status { get; }
        public StatKind Stat { get; }
        public int Amount { get; }

        private BattleEvent(BattleEventKind kind, int side, BattleMonster monster, string moveName = null,
            StatKind stat = default, int amount = 0)
            : this(kind, side, SlotNow(monster), monster, moveName, monster?.CurrentHp ?? 0, monster?.Status ?? StatusCondition.None,
                stat, amount)
        {
        }

        private BattleEvent(BattleEventKind kind, int side, int slot, BattleMonster monster, string moveName, int hp,
            StatusCondition status, StatKind stat, int amount)
        {
            Kind = kind;
            Side = side;
            Slot = slot;
            Monster = monster;
            MoveName = moveName;
            Hp = hp;
            Status = status;
            Stat = stat;
            Amount = amount;
        }

        public static BattleEvent Of(BattleEventKind kind, int side, BattleMonster monster)
            => new BattleEvent(kind, side, monster);

        public static BattleEvent MoveUsed(int side, BattleMonster monster, MoveDefinition move)
            => new BattleEvent(BattleEventKind.MoveUsed, side, monster, move.Name);

        public static BattleEvent StatChange(BattleEventKind kind, int side, BattleMonster monster, StatKind stat, int amount)
            => new BattleEvent(kind, side, monster, stat: stat, amount: amount);

        /// <summary>数を持つイベント（連続技の回数など）</summary>
        public static BattleEvent Count(BattleEventKind kind, int side, BattleMonster monster, int amount)
            => new BattleEvent(kind, side, monster, amount: amount);

        /// <summary>天候の始まり・終わり。止んだときは主語がいないので monster は null</summary>
        public static BattleEvent WeatherChange(BattleEventKind kind, int side, BattleMonster monster, Weather weather)
            => new BattleEvent(kind, side, monster, amount: (int)weather);

        public Weather Weather => (Weather)Amount;

        /// <summary>
        /// 羽リレーで引き継いだランク。オンラインのイベントは int 9個で送るので、7つのランク（-6〜+6）を 4bit ずつ Amount に詰める
        /// </summary>
        public static BattleEvent StagesPassed(int side, BattleMonster monster)
        {
            int packed = 0;
            for (int i = 0; i < StatKindNames.Count; i++)
            {
                packed |= (monster.GetStage((StatKind)i) + BattleBalance.MaxStatStage) << (i * StageBits);
            }
            return new BattleEvent(BattleEventKind.StagesPassed, side, monster, amount: packed);
        }

        /// <summary>StagesPassed の Amount から7つのランクを取り出す</summary>
        public int[] PassedStages()
        {
            var stages = new int[StatKindNames.Count];
            for (int i = 0; i < stages.Length; i++)
            {
                stages[i] = ((Amount >> (i * StageBits)) & StageMask) - BattleBalance.MaxStatStage;
            }
            return stages;
        }

        public static BattleEvent BattleEnd(int winner)
            => new BattleEvent(BattleEventKind.BattleEnd, winner, null);

        /// <summary>オンラインで受け取った値から作り直す。HP・状態異常は送られてきた時点の値を使う</summary>
        internal static BattleEvent Restore(BattleEventKind kind, int side, int slot, BattleMonster monster, string moveName,
            int hp, StatusCondition status, StatKind stat, int amount)
            => new BattleEvent(kind, side, slot, monster, moveName, hp, status, stat, amount);

        /// <summary>
        /// 起きた時点の場の番号。再生はターンを計算し終えてからなので、そのときには交代で別のペンモンが入っていることがある。
        /// 場にいない（単体で作ったテスト用のペンモンなど）ときは 0
        /// </summary>
        private static int SlotNow(BattleMonster monster)
        {
            int slot = monster?.Side?.SlotOf(monster) ?? 0;
            return slot < 0 ? 0 : slot;
        }
    }
}
