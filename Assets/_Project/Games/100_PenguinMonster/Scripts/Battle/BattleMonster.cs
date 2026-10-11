using System;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// バトル中のペンモン1体（定義＋現在のHP・状態異常・ランク）。
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

        public string Name => Definition.Name;
        public int MaxHp => Stats.Hp;
        public bool IsFainted => CurrentHp <= 0;
        public bool IsConfused => ConfusionTurns > 0;

        /// <summary>行動順に使うすばやさ。ランクを掛け、まひなら半分</summary>
        public int EffectiveSpeed
        {
            get
            {
                int speed = StageMultiplier.ApplyStat(Stats.Speed, GetStage(StatKind.Speed));
                return Status == StatusCondition.Paralysis ? speed / BattleBalance.ParalysisSpeedDivisor : speed;
            }
        }

        public BattleMonster(MonsterDefinition definition)
        {
            Definition = definition;
            Stats = Stats.FromBase(definition.BaseStats);
            CurrentHp = Stats.Hp;
        }

        public int GetStage(StatKind stat) => _stages[(int)stat];

        /// <summary>攻撃側の能力。急所のときは下がったランクを無視する（仕様書 §4.3）</summary>
        public int AttackStat(bool isPhysical, bool isCritical)
        {
            var kind = isPhysical ? StatKind.Attack : StatKind.SpAttack;
            int stage = GetStage(kind);
            return StageMultiplier.ApplyStat(isPhysical ? Stats.Attack : Stats.SpAttack, isCritical ? Math.Max(0, stage) : stage);
        }

        /// <summary>防御側の能力。急所のときは上がったランクを無視する</summary>
        public int DefenseStat(bool isPhysical, bool isCritical)
        {
            var kind = isPhysical ? StatKind.Defense : StatKind.SpDefense;
            int stage = GetStage(kind);
            return StageMultiplier.ApplyStat(isPhysical ? Stats.Defense : Stats.SpDefense, isCritical ? Math.Min(0, stage) : stage);
        }

        public bool CanReceive(StatusCondition status)
        {
            return Status == StatusCondition.None && !StatusImmunity.IsImmune(Definition, status);
        }

        public void TakeDamage(int damage)
        {
            CurrentHp = Math.Max(0, CurrentHp - damage);
        }

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

        /// <summary>交代でひっこめたとき、場にいる間だけの変化を戻す</summary>
        internal void OnWithdrawn()
        {
            Array.Clear(_stages, 0, _stages.Length);
            ConfusionTurns = 0;
            BadPoisonCount = 0;
        }
    }
}
