using System;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// バトル中のペンモン1体（定義＋現在のHP・状態異常・ランク・持ち物を使ったか）。
    /// 状態異常は交代しても残り、ランク・こんらん・もうどくの段階は交代で戻る（仕様書 §4.4 / §4.5）
    /// </summary>
    public sealed class BattleMonster
    {
        private readonly int[] _stages = new int[StatKindNames.Count];

        public MonsterDefinition Definition { get; }
        public Stats Stats { get; }
        public int CurrentHp { get; private set; }
        public StatusCondition Status { get; private set; }
        /// <summary>ねむりのとき、あと何ターン動けないか</summary>
        public int SleepTurns { get; private set; }
        /// <summary>もうどくのダメージの段階（1/16, 2/16…の分子）。ターン終わりに1ずつ増える</summary>
        public int BadPoisonCount { get; private set; }
        /// <summary>こんらんの残り回数。0 ならこんらんしていない</summary>
        public int ConfusionTurns { get; private set; }
        /// <summary>1回だけの持ち物を使った。交代しても戻らない</summary>
        public bool ItemUsed { get; private set; }
        /// <summary>このターン、守る が決まっていて相手の技を受けない</summary>
        public bool IsProtecting { get; private set; }
        /// <summary>前のターンに 守る が決まった。続けて使うと失敗する（守り続けるだけの試合にしないため）</summary>
        public bool ProtectedLastTurn { get; private set; }
        /// <summary>このターン、味方の 手助け を受けていて技が強くなる</summary>
        public bool IsHelped { get; private set; }

        public string Name => Definition.Name;
        public int MaxHp => Stats.Hp;
        public bool IsFainted => CurrentHp <= 0;
        public bool IsConfused => ConfusionTurns > 0;
        public bool IsFullHp => CurrentHp == MaxHp;

        /// <summary>選出した陣営。仇討ちで味方の様子を見るのに使う。テストで単体で作ったときは null</summary>
        internal BattleSide Side { get; set; }

        /// <summary>選出の味方（自分以外）で瀕死になっている数</summary>
        public int FaintedAllies
        {
            get
            {
                if (Side == null)
                {
                    return 0;
                }
                int count = 0;
                foreach (var member in Side.Party)
                {
                    if (member != this && member.IsFainted)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        /// <summary>場の天候。テストで単体で作ったときは None</summary>
        public Weather CurrentWeather => Side?.State?.Weather ?? Weather.None;

        /// <summary>
        /// 行動順に使うすばやさ。ランクを掛け、まひなら半分。かるいはね なら 1.2 倍。天候に乗る特性なら 2 倍。
        /// 陣営に追い風が吹いていればさらに 2 倍
        /// </summary>
        public int EffectiveSpeed
        {
            get
            {
                int speed = StageMultiplier.ApplyStat(Stats.Speed, GetStage(StatKind.Speed));
                if (Status == StatusCondition.Paralysis)
                {
                    speed /= BattleBalance.ParalysisSpeedDivisor;
                }
                if (HasItem(HeldItem.LightFeather))
                {
                    speed = (int)(speed * BattleBalance.LightFeatherSpeedMultiplier);
                }
                if (RidesWeather)
                {
                    speed *= BattleBalance.WeatherSpeedMultiplier;
                }
                return Side != null && Side.HasTailwind ? speed * BattleBalance.TailwindSpeedMultiplier : speed;
            }
        }

        /// <summary>雪滑り・波乗り・光合成 が、それぞれの天候で働いている</summary>
        internal bool RidesWeather
        {
            get
            {
                var weather = CurrentWeather;
                return (weather == Weather.Snow && HasAbility(Ability.SnowSlide))
                    || (weather == Weather.Rain && HasAbility(Ability.RainSurf))
                    || (weather == Weather.Sun && HasAbility(Ability.Photosynthesis));
            }
        }

        public BattleMonster(MonsterDefinition definition)
        {
            Definition = definition;
            Stats = Stats.FromBase(definition.BaseStats, definition.Level);
            CurrentHp = Stats.Hp;
        }

        /// <summary>
        /// 途中の HP・状態異常から始める。ストーリーで前の戦いの傷を持ち越すため。
        /// HP は 1〜最大HP に収める（瀕死の子は出さない前提）
        /// </summary>
        public BattleMonster(MonsterDefinition definition, int startHp, StatusCondition startStatus) : this(definition)
        {
            CurrentHp = Math.Max(1, Math.Min(MaxHp, startHp));
            Status = startStatus;
        }

        public int GetStage(StatKind stat) => _stages[(int)stat];

        public bool HasAbility(Ability ability) => Definition.Ability == ability;

        /// <summary>その持ち物をまだ持っている（1回だけのものは使ったら false）</summary>
        public bool HasItem(HeldItem item) => Definition.Item == item && !ItemUsed;

        /// <summary>攻撃側の能力。急所のときは下がったランクを無視する（仕様書 §4.3）</summary>
        public int AttackStat(bool isPhysical, bool isCritical)
        {
            var kind = isPhysical ? StatKind.Attack : StatKind.SpAttack;
            int stage = GetStage(kind);
            return StageMultiplier.ApplyStat(isPhysical ? Stats.Attack : Stats.SpAttack, isCritical ? Math.Max(0, stage) : stage);
        }

        /// <summary>
        /// 防御側の能力。急所のときは上がったランクを無視する。
        /// ヒナの綿毛（ランク「ヒナ」全員の補正）は能力変化ではないので急所でも消えない
        /// </summary>
        public int DefenseStat(bool isPhysical, bool isCritical)
        {
            var kind = isPhysical ? StatKind.Defense : StatKind.SpDefense;
            int stage = GetStage(kind);
            int defense = StageMultiplier.ApplyStat(isPhysical ? Stats.Defense : Stats.SpDefense, isCritical ? Math.Min(0, stage) : stage);
            return Definition.Rank == MonsterRank.Hina ? (int)(defense * BattleBalance.ChickDownDefenseMultiplier) : defense;
        }

        public bool CanReceive(StatusCondition status)
        {
            return Status == StatusCondition.None && !StatusImmunity.IsImmune(Definition, status);
        }

        public void TakeDamage(int damage)
        {
            CurrentHp = Math.Max(0, CurrentHp - damage);
        }

        internal void Heal(int amount)
        {
            CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
        }

        internal void UseItem() => ItemUsed = true;

        /// <summary>ランクを変える。-6〜+6 に収め、実際に変わった量を返す（0 なら もう変わらない）</summary>
        internal int ChangeStage(StatKind stat, int delta)
        {
            int before = _stages[(int)stat];
            int after = Math.Max(-BattleBalance.MaxStatStage, Math.Min(BattleBalance.MaxStatStage, before + delta));
            _stages[(int)stat] = after;
            return after - before;
        }

        internal void SetStatus(StatusCondition status, int sleepTurns)
        {
            Status = status;
            SleepTurns = sleepTurns;
            BadPoisonCount = 0;
        }

        internal void CureStatus() => SetStatus(StatusCondition.None, 0);

        /// <summary>このターンも眠っているなら残りを1減らして true。残りが 0 なら目を覚ます番なので false</summary>
        internal bool TickSleep()
        {
            if (SleepTurns <= 0)
            {
                return false;
            }
            SleepTurns--;
            return true;
        }

        internal int AdvanceBadPoison() => ++BadPoisonCount;

        internal void Confuse(int turns) => ConfusionTurns = turns;

        /// <summary>こんらんの残りを1減らす。まだこんらんしているなら true</summary>
        internal bool TickConfusion()
        {
            ConfusionTurns--;
            return ConfusionTurns > 0;
        }

        /// <summary>オンラインのゲストが持つ写しに、ホストから届いた HP・状態異常をそのまま入れる</summary>
        internal void Restore(int hp, StatusCondition status)
        {
            CurrentHp = hp;
            Status = status;
        }

        /// <summary>羽リレーで引き継ぐランクの写し</summary>
        internal int[] CopyStages() => (int[])_stages.Clone();

        internal void SetStages(int[] stages) => Array.Copy(stages, _stages, _stages.Length);

        internal void Protect() => IsProtecting = true;

        internal void ReceiveHelp() => IsHelped = true;

        /// <summary>ターンの最後に、そのターンだけの守る・手助けを消し、守ったかどうかを前のターンの記録へ移す</summary>
        internal void EndTurn()
        {
            ProtectedLastTurn = IsProtecting;
            IsProtecting = false;
            IsHelped = false;
        }

        /// <summary>交代でひっこめたとき、場にいる間だけの変化を戻す</summary>
        internal void OnWithdrawn()
        {
            Array.Clear(_stages, 0, _stages.Length);
            ConfusionTurns = 0;
            BadPoisonCount = 0;
            IsProtecting = false;
            ProtectedLastTurn = false;
            IsHelped = false;
        }
    }
}
