using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Story;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 戦いの後の経験値 → レベルアップ（能力の窓）→ 技を覚える（いっぱいなら忘れる技を選ぶ）→ 進化。
    /// 進化は本家と同じく、この戦いでレベルが上がった子だけ、経験値を配り終えてから
    /// </summary>
    public partial class StoryBattleScreen
    {
        private const string ExpFormat = "{0}は {1} 経験値を もらった！";
        private const string BenchExpText = "手持ちの 仲間も 経験値を もらった！";
        private const string LevelUpFormat = "{0}は レベル {1}に 上がった！";
        private const string LearnedFormat = "{0}は {1}を 覚えた！";
        private const string WantsToLearnFormat = "{0}は 新しく {1}を 覚えたい……";
        private const string MovesFullFormat = "しかし 技を {0}つ 覚えているので これ以上 覚えられない！";
        private const string GaveUpFormat = "{0}は {1}を 覚えずに 終わった！";
        private const string ForgotFormat = "1、2の…… ポカン！ {0}は {1}の 使い方を きれいに 忘れた！";
        private const string LearnedInsteadFormat = "そして {0}は 新しく {1}を 覚えた！";

        private IEnumerator ShowGrowth(IReadOnlyList<ExpReward> rewards)
        {
            var leveledUp = new List<OwnedMonster>();
            foreach (var reward in rewards)
            {
                if (!reward.FromBench)
                {
                    yield return _battle.Say(string.Format(ExpFormat, reward.Member.Name, reward.Exp));
                    yield return GainExp(reward, leveledUp);
                }
            }
            // ベンチの子は1体ずつ言うと長いので1行にまとめ、レベルが上がった子だけ個別に見せる
            bool benchAnnounced = false;
            foreach (var reward in rewards)
            {
                if (reward.FromBench)
                {
                    if (!benchAnnounced)
                    {
                        yield return _battle.Say(BenchExpText);
                        benchAnnounced = true;
                    }
                    yield return GainExp(reward, leveledUp);
                }
            }
            foreach (var member in leveledUp)
            {
                if (member.CanEvolve)
                {
                    yield return Evolve(member);
                }
            }
        }

        private IEnumerator GainExp(ExpReward reward, List<OwnedMonster> leveledUp)
        {
            var member = reward.Member;
            foreach (var levelUp in member.GainExp(reward.Exp))
            {
                yield return ShowLevelUp(member, levelUp);
                if (!leveledUp.Contains(member))
                {
                    leveledUp.Add(member);
                }
            }
        }

        private IEnumerator ShowLevelUp(OwnedMonster member, LevelUp levelUp)
        {
            BattleSounds.Play(BattleSound.StatUp);
            yield return _battle.ShowLevelUp(string.Format(LevelUpFormat, member.Name, levelUp.Level), levelUp.Before, levelUp.After);
            yield return ShowMoves(member, levelUp.Moves);
        }

        private IEnumerator ShowMoves(OwnedMonster member, MoveLearning learning)
        {
            foreach (var move in learning.Learned)
            {
                _battle.ShowNewMove(move);
                yield return _battle.Say(string.Format(LearnedFormat, member.Name, move.Name));
                _battle.HideNewMove();
            }
            foreach (var move in learning.ToChoose)
            {
                yield return OfferMove(member, move);
            }
        }

        /// <summary>技が4つ埋まっているとき、どれか忘れて覚えるかを選ばせる。戻るなら覚えない</summary>
        private IEnumerator OfferMove(OwnedMonster member, MoveDefinition move)
        {
            // 覚えたい技の中身を、忘れる技を選び終えるまで出しておく（今の技は長押しで説明が出るので見比べられる）
            _battle.ShowNewMove(move);
            yield return _battle.Say(string.Format(WantsToLearnFormat, member.Name, move.Name));
            yield return _battle.Say(string.Format(MovesFullFormat, StoryBalance.MaxMoves));
            int slot = -1;
            yield return _battle.ChooseMoveToForget(member.ToBattle(), chosen => slot = chosen);
            _battle.HideNewMove();
            if (slot < 0)
            {
                yield return _battle.Say(string.Format(GaveUpFormat, member.Name, move.Name));
                yield break;
            }
            var forgotten = member.Moves[slot];
            member.ReplaceMove(slot, move);
            yield return _battle.Say(string.Format(ForgotFormat, member.Name, forgotten.Name));
            yield return _battle.Say(string.Format(LearnedInsteadFormat, member.Name, move.Name));
        }

        private IEnumerator Evolve(OwnedMonster member)
        {
            // 進化先は全区切りの種から探す（区切りをまたいで進化する子がいても引けるように）
            var into = StorySpeciesTable.ByNo(member.Species.Evolution.IntoNo);
            if (into == null)
            {
                yield break;
            }
            var from = member.Species.Definition;
            string fromName = member.Name;
            var learning = member.Evolve(into);
            yield return _battle.PlayEvolution(fromName, from, into.Definition);
            yield return ShowMoves(member, learning);
        }
    }
}
