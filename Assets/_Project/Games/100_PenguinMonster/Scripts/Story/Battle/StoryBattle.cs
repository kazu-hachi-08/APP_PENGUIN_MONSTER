using System;
using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>倒した相手から1体がもらう経験値</summary>
    public readonly struct ExpReward
    {
        public readonly OwnedMonster Member;
        public readonly int Exp;

        public ExpReward(OwnedMonster member, int exp)
        {
            Member = member;
            Exp = exp;
        }
    }

    /// <summary>
    /// ストーリーの1戦（PLAN_STORY_TOWN01.md フェーズ5）。手持ち全員 vs 相手全員のシングルで、見せ合い・選出はない。
    /// 手持ちの HP・状態異常から始め、終わったら持ち帰る。ターンは今までどおり BattleEngine が進め、
    /// ここは「誰が誰と向き合ったか」を覚えて、経験値を出た子だけで分ける
    /// </summary>
    public sealed class StoryBattle
    {
        public const int PlayerSide = 0;
        public const int FoeSide = 1;

        private readonly Party _party;
        private readonly IReadOnlyList<OwnedMonster> _foes;
        /// <summary>相手ごとに、その子と場で向き合った手持ちの番号</summary>
        private readonly List<HashSet<int>> _facedBy = new List<HashSet<int>>();
        private readonly HashSet<int> _seenNos = new HashSet<int>();
        private int _fleeAttempts;

        public BattleState State { get; }
        /// <summary>トレーナー戦（逃げられない・経験値 1.5 倍）。false なら野生</summary>
        public bool IsTrainer { get; }
        public IReadOnlyList<OwnedMonster> Foes => _foes;
        /// <summary>場に出てきた相手の No（図鑑の「見た」に入れる。出てこなかったトレーナーの子は入らない）</summary>
        public IEnumerable<int> SeenNos => _seenNos;

        public bool PlayerWon => State.Winner == PlayerSide;
        /// <summary>捕獲タマゴで捕まえた子。捕まえたらそこでバトルは終わる。捕まえていなければ null</summary>
        public OwnedMonster Caught { get; private set; }
        /// <summary>人の ペンモンは 捕まえられない（トレーナー戦では捕獲タマゴを投げられない）</summary>
        public bool CanThrowEgg => !IsTrainer;

        public StoryBattle(Party party, IReadOnlyList<OwnedMonster> foes, bool isTrainer)
        {
            _party = party;
            _foes = foes;
            IsTrainer = isTrainer;
            State = new BattleState(ToBattle(party.Members), ToBattle(foes));
            for (int i = 0; i < foes.Count; i++)
            {
                _facedBy.Add(new HashSet<int>());
            }
            RecordFacing();
        }

        private static List<BattleMonster> ToBattle(IReadOnlyList<OwnedMonster> members)
        {
            var fighters = new List<BattleMonster>();
            foreach (var member in members)
            {
                fighters.Add(member.ToBattle());
            }
            return fighters;
        }

        /// <summary>
        /// 今の場の組み合わせを覚える。場が変わりうるところ（ターンの後・入れ替えの後）で呼ぶ。
        /// 経験値はバトルの後にまとめて出すので、相手が倒れる前に一度でも向き合っていればよい
        /// </summary>
        public void RecordFacing()
        {
            int foe = State.Sides[FoeSide].ActivePartyIndex;
            _facedBy[foe].Add(State.Sides[PlayerSide].ActivePartyIndex);
            _seenNos.Add(_foes[foe].No);
        }

        /// <summary>
        /// 野生から逃げる（本家の第3世代の簡略版）。自分が速ければ必ず逃げられ、遅くても試すほど逃げやすくなる。
        /// トレーナー戦では逃げられない
        /// </summary>
        public bool TryFlee(Random random)
        {
            if (IsTrainer)
            {
                return false;
            }
            _fleeAttempts++;
            int own = State.Sides[PlayerSide].Active.Stats.Speed;
            int wild = State.Sides[FoeSide].Active.Stats.Speed;
            if (own >= wild)
            {
                return true;
            }
            int odds = own * StoryBalance.FleeSpeedScale / wild + StoryBalance.FleeAttemptBonus * _fleeAttempts;
            return random.Next(StoryBalance.FleeRoll) < odds;
        }

        /// <summary>この手持ちの子に、回復・状態異常を治す道具が効く</summary>
        public bool CanTreat(StoryItem item, int partyIndex)
        {
            var medicine = StoryItems.Get(item);
            var target = State.Sides[PlayerSide].Party[partyIndex];
            return medicine.IsMedicine && medicine.CanTreat(target.CurrentHp, target.MaxHp, target.Status);
        }

        /// <summary>
        /// バトル中に手持ちの子へ道具を使う（ターンの行動の代わりで、相手だけが動く）。効かなければ使わずに false。
        /// 使ったのはバトルの写しなので、手持ちへは ApplyResults で持ち帰る
        /// </summary>
        public bool UseMedicine(Bag bag, StoryItem item, int partyIndex)
        {
            if (!CanTreat(item, partyIndex) || !bag.Remove(item))
            {
                return false;
            }
            var medicine = StoryItems.Get(item);
            var target = State.Sides[PlayerSide].Party[partyIndex];
            target.ApplyMedicine(medicine.HealAmount, medicine.CuresStatus(target.Status));
            return true;
        }

        /// <summary>
        /// 場の野生に捕獲タマゴを投げる（タマゴを1個使う）。捕まえたら Caught に、その時の HP・状態異常のまま入る。
        /// 外れたらターンの行動の代わりで、相手だけが動く
        /// </summary>
        public CaptureResult ThrowEgg(Bag bag, Random random)
        {
            if (!CanThrowEgg || !bag.Remove(StoryItem.CaptureEgg))
            {
                throw new InvalidOperationException("トレーナー戦か、捕獲タマゴを持っていない");
            }
            var foeSide = State.Sides[FoeSide];
            var wild = foeSide.Active;
            var owned = _foes[foeSide.ActivePartyIndex];
            var result = Capture.Roll(Capture.Odds(wild.MaxHp, wild.CurrentHp, owned.Species.CatchRate, wild.Status), random);
            if (result.Caught)
            {
                owned.ApplyBattleResult(wild);
                Caught = owned;
            }
            return result;
        }

        /// <summary>
        /// 倒した相手ごとの経験値を、向き合って最後まで立っていた子で山分けする（瀕死の子はもらえない）。
        /// 1体が何体分もらっても1行にまとめ、手持ちの順に返す
        /// </summary>
        public IReadOnlyList<ExpReward> ExpRewards()
        {
            var totals = new int[_party.Count];
            for (int i = 0; i < _foes.Count; i++)
            {
                if (!State.Sides[FoeSide].Party[i].IsFainted)
                {
                    continue;
                }
                var earners = Earners(i);
                if (earners.Count == 0)
                {
                    continue;
                }
                int exp = Experience.Defeated(_foes[i].Species.BaseExp, _foes[i].Level, IsTrainer);
                foreach (int member in earners)
                {
                    totals[member] += Math.Max(1, exp / earners.Count);
                }
            }
            var rewards = new List<ExpReward>();
            for (int i = 0; i < totals.Length; i++)
            {
                if (totals[i] > 0)
                {
                    rewards.Add(new ExpReward(_party.Members[i], totals[i]));
                }
            }
            return rewards;
        }

        private List<int> Earners(int foeIndex)
        {
            var earners = new List<int>();
            foreach (int member in _facedBy[foeIndex])
            {
                if (!State.Sides[PlayerSide].Party[member].IsFainted)
                {
                    earners.Add(member);
                }
            }
            earners.Sort();
            return earners;
        }

        /// <summary>戦いの後の HP・状態異常を手持ちに持ち帰る（逃げたときも）</summary>
        public void ApplyResults()
        {
            var fighters = State.Sides[PlayerSide].Party;
            for (int i = 0; i < _party.Count; i++)
            {
                _party.Members[i].ApplyBattleResult(fighters[i]);
            }
        }
    }
}
