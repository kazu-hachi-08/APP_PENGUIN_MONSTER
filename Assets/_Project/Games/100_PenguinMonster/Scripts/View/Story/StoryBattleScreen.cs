using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Common.Scene;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドからストーリーの1戦へ移り、戦いの後のこと（賞金・経験値・レベルアップ）を見せる（PLAN_STORY_TOWN01.md フェーズ5）。
    /// バトルの画面は対戦と同じ BattleController を使い、ここは暗転と前後の文だけを受け持つ。
    /// 経験値・レベルアップ・技・進化の見せ方は .Growth.cs
    /// </summary>
    public partial class StoryBattleScreen : MonoBehaviour
    {
        private const string BlackOutMessage = "目の前が 真っ暗に なった！";
        private const string PrizeFormat = "賞金 {0}円を 手に入れた！";
        private const string SentToBoxFormat = "{0}は あずかり箱へ 送られた！";

        [SerializeField] private BattleController _battle;
        [SerializeField] private FadeController _fade;
        [SerializeField] private float _fadeSeconds = 0.4f;

        public bool PlayerWon { get; private set; }
        /// <summary>負けて、最後に回復した場所へ戻る（負けてもよい戦いでは false）</summary>
        public bool BlackedOut { get; private set; }

        /// <summary>
        /// 1戦して、戦いの後の文まで見せる。終わったときは暗転したまま（呼ぶ側がフィールドを整えてから ReturnToField で明ける）
        /// </summary>
        /// <param name="trainer">野生なら null</param>
        /// <param name="canLose">負けても全滅にしない（最初のライバル戦）</param>
        public IEnumerator Fight(StoryState state, List<OwnedMonster> foes, TrainerDefinition trainer, bool canLose, System.Random random)
        {
            GameMusic.Play(MusicTrack.Battle);
            yield return _fade.FadeOutCoroutine(_fadeSeconds);
            _battle.gameObject.SetActive(true);

            var battle = new StoryBattle(state.Party, foes, trainer != null);
            var cpu = new CpuPlayer(random, trainer?.Cpu ?? CpuLevel.Weak);
            var running = StartCoroutine(_battle.RunStory(battle, state.Bag, cpu, random, trainer?.Name));
            yield return _fade.FadeInCoroutine(_fadeSeconds);
            yield return running;

            battle.ApplyResults();
            foreach (int no in battle.SeenNos)
            {
                state.MarkSeen(no);
            }
            PlayerWon = battle.PlayerWon;
            // 逃げた・捕まえたときは、勝ってはいないが負けでもない
            bool endedEarly = _battle.Fled || battle.Caught != null;
            BlackedOut = !PlayerWon && !endedEarly && !canLose;
            if (PlayerWon)
            {
                yield return AfterWin(battle, trainer);
                MarkPartyCaught(state);
            }
            else if (battle.Caught != null)
            {
                yield return AfterCatch(state, battle.Caught);
            }
            else if (BlackedOut)
            {
                yield return _battle.Say(BlackOutMessage);
            }

            yield return _fade.FadeOutCoroutine(_fadeSeconds);
            _battle.gameObject.SetActive(false);
            // フィールドの曲はまだ無い（フェーズ10）ので、ストーリーに入ったときの曲に戻す
            GameMusic.Play(MusicTrack.Title);
        }

        /// <summary>暗転を明けてフィールドに戻る</summary>
        public IEnumerator ReturnToField()
        {
            yield return _fade.FadeInCoroutine(_fadeSeconds);
        }

        /// <summary>進化した子を図鑑の「捕まえた」に入れる（進化は手持ちの子の種が変わるだけなので、ここでまとめて足す）</summary>
        private static void MarkPartyCaught(StoryState state)
        {
            foreach (var member in state.Party.Members)
            {
                state.MarkCaught(member.No);
            }
        }

        /// <summary>捕まえた子を手持ちへ（いっぱいなら あずかり箱へ）。捕まえても経験値は無い（本家の第3〜4世代と同じ）</summary>
        private IEnumerator AfterCatch(StoryState state, OwnedMonster caught)
        {
            if (!state.Receive(caught))
            {
                yield return _battle.Say(string.Format(SentToBoxFormat, caught.Name));
            }
        }

        private IEnumerator AfterWin(StoryBattle battle, TrainerDefinition trainer)
        {
            if (trainer != null)
            {
                // お金は台本（EventRunner.FinishBattle）が足す。ここは知らせるだけ
                yield return _battle.Say(string.Format(PrizeFormat, trainer.Prize));
            }
            yield return ShowGrowth(battle.ExpRewards());
        }
    }
}
