using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Common.Online;
using PenguinMonster.Online;
using PenguinMonster.Story;
using PenguinMonster.View.Story;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// オンライン対戦の流れ: 部屋を作る / コードで参加 → シングル / ダブルス → ドラフト / 自分のチーム（どちらもホストが選ぶ）
    /// → 以降は GameFlow.cs と同じ（持ち物 → 見せ合い・選出 → バトル → 結果 → タイトル）。自分のチームはドラフト・持ち物を飛ばし、最後に記録した手持ちの6体を Lv50 で出す。
    /// ドラフトの抽選はホストが決めた種でゲストの端末が引く（提示はお互い別々で、相手の取り方に左右されないため）。
    /// 相手が抜けたら、どこにいても勝ち扱いで終わる
    /// </summary>
    public partial class GameFlow
    {
        private const string DisconnectedDetail = "相手が 切断しました";
        private const string ConnectionLostDetail = "通信が 切れました";
        private const string OnlineQuitMessage = "相手の 勝ちに なります";
        private const string ChoosingRulesText = "相手が ルールを 選んでいます…";
        private const string OwnNotEnoughText = "手持ちが 6体 必要です";
        private const string OwnTooManyLegendsText = "伝説の ペンモンは 1体までです";
        private const string GuestNotEnoughText = "相手の 手持ちが 6体 そろっていません";
        private const string GuestTooManyLegendsText = "相手の 手持ちに 伝説が 2体 以上 います";

        [SerializeField] private OnlineSession _session;
        [SerializeField] private ModeSelectPanel _modeSelect;
        [SerializeField] private TeamSourcePanel _teamSourcePanel;

        private OnlineLink _link;

        private IEnumerator RunOnline()
        {
            bool? isHost = null;
            // 通信路は接続できたコールバックの中で開く（ホストは接続直後に種を送ってくるため）
            _modeSelect.Show(onNpcSelected: null, onOnlineReady: host =>
            {
                OpenLink();
                isHost = host;
            }, onTitle: BackToTitle);
            yield return new WaitUntil(() => isHost.HasValue);
            // シーンを読み直せば OnlineSession が部屋を抜けるので、相手には切断（相手の勝ち）として伝わる
            _quitButton.Show(OnlineQuitMessage, BackToTitle);

            // 自分のチームはストーリーの記録から読む（オンラインはタイトルから入るので、遊んでいる途中の手持ちは無い）
            var ownParty = StorySaveStore.Load()?.State.Party;
            var ownProblem = OnlineTeam.Check(ownParty);
            if (!isHost.Value)
            {
                _link.Send(LinkMessage.TeamReady, new[] { (int)ownProblem });
            }

            var random = new System.Random();
            System.Random draftRandom = null;
            var format = BattleFormat.Single;
            var source = TeamSource.Draft;
            yield return ShareRules(isHost.Value, random, ownParty, ownProblem, (seeded, chosen, chosenSource) =>
            {
                draftRandom = seeded;
                format = chosen;
                source = chosenSource;
            });

            GameMusic.Play(MusicTrack.Draft);
            List<MonsterDefinition> withItem = null;
            if (source == TeamSource.OwnTeam)
            {
                withItem = OnlineTeam.From(ownParty);
            }
            else
            {
                yield return DraftOnline(draftRandom, six => withItem = six);
            }
            List<MonsterDefinition> opponentSix = null;
            yield return Exchange(LinkMessage.Six, withItem, received => opponentSix = received);

            List<MonsterDefinition> ownTeam = null;
            _selectPanel.Show(withItem, opponentSix, format.PartySize(), team => ownTeam = team, BattleBalance.OnlineSelectSeconds);
            yield return new WaitUntil(() => ownTeam != null);
            List<MonsterDefinition> opponentTeam = null;
            yield return Exchange(LinkMessage.Team, ownTeam, received => opponentTeam = received);

            GameMusic.Play(MusicTrack.Battle);
            yield return isHost.Value
                ? _battle.RunHost(ownTeam, opponentTeam, random, _link, format)
                : _battle.RunGuest(ownTeam, opponentTeam, _link, format);

            // 決着の後に相手がタイトルへ戻っても、切断で結果を上書きしない
            CloseLink();
            ShowOnlineResult(_battle.PlayerWon);
        }

        /// <summary>ドラフトで6体を取り、持ち物を配る</summary>
        private IEnumerator DraftOnline(System.Random draftRandom, System.Action<List<MonsterDefinition>> onDone)
        {
            IReadOnlyList<MonsterDefinition> ownSix = null;
            _draftPanel.Show(new Draft(MonsterTable.All, draftRandom), six => ownSix = six, BattleBalance.OnlineDraftSeconds);
            yield return new WaitUntil(() => ownSix != null);
            // 持ち物も各自の端末で引く。結果は Six で送るので、種をそろえる必要はない
            List<MonsterDefinition> withItem = null;
            _itemPanel.Show(ownSix, ItemDraw.Draw(draftRandom), six => withItem = six, BattleBalance.OnlineDraftSeconds);
            yield return new WaitUntil(() => withItem != null);
            onDone(withItem);
        }

        /// <summary>
        /// ホストがシングル / ダブルス → ドラフト / 自分のチームを選び、ゲストのドラフトの種と一緒に送る。ゲストは届くのを待つ。
        /// ホスト自身のドラフトはホストの乱数で引く。自分のチームはお互いの手持ちが使えるときだけ選べるので、
        /// 先にゲストから「使えるか」を受け取っておく
        /// </summary>
        private IEnumerator ShareRules(bool isHost, System.Random random, Party ownParty, OnlineTeamProblem ownProblem,
            System.Action<System.Random, BattleFormat, TeamSource> onReady)
        {
            if (!isHost)
            {
                _battle.ShowWaiting(ChoosingRulesText);
                yield return _link.Wait(LinkMessage.DraftSeed,
                    data => onReady(new System.Random(data[0]), (BattleFormat)data[1], (TeamSource)data[2]));
                yield break;
            }

            var guestProblem = OnlineTeamProblem.NotEnough;
            yield return _link.Wait(LinkMessage.TeamReady, data => guestProblem = (OnlineTeamProblem)data[0]);
            string problemText = OwnTeamProblemText(ownProblem, guestProblem);
            var leader = ownParty != null && ownParty.Count > 0 ? ownParty.Members[0].Species.Definition : null;
            while (true)
            {
                BattleFormat? format = null;
                _formatPanel.Show(chosen => format = chosen, onBack: BackToTitle);
                yield return new WaitUntil(() => format.HasValue);

                // 「戻る」はシングル / ダブルスを選び直す
                TeamSource? source = null;
                bool back = false;
                _teamSourcePanel.Show(problemText, leader, chosen => source = chosen, () => back = true);
                yield return new WaitUntil(() => source.HasValue || back);
                if (back)
                {
                    continue;
                }

                _link.Send(LinkMessage.DraftSeed, new[] { random.Next(), (int)format.Value, (int)source.Value });
                onReady(random, format.Value, source.Value);
                yield break;
            }
        }

        /// <summary>自分のチームを選べない理由（自分の方を先に出す）。選べるなら null</summary>
        private static string OwnTeamProblemText(OnlineTeamProblem own, OnlineTeamProblem guest)
        {
            switch (own)
            {
                case OnlineTeamProblem.NotEnough: return OwnNotEnoughText;
                case OnlineTeamProblem.TooManyLegends: return OwnTooManyLegendsText;
            }
            switch (guest)
            {
                case OnlineTeamProblem.NotEnough: return GuestNotEnoughText;
                case OnlineTeamProblem.TooManyLegends: return GuestTooManyLegendsText;
            }
            return null;
        }

        /// <summary>自分のペンモンの並びを送り、相手のが届くまで待つ</summary>
        private IEnumerator Exchange(LinkMessage kind, IReadOnlyList<MonsterDefinition> own,
            System.Action<List<MonsterDefinition>> onReceived)
        {
            _link.Send(kind, OnlineCodec.EncodeMonsters(own));
            _battle.ShowWaiting();
            yield return _link.Wait(kind, data => onReceived(OnlineCodec.DecodeMonsters(data)));
        }

        private void OpenLink()
        {
            _link = new OnlineLink();
            _link.Open();
            _link.OnTimedOut += HandleConnectionLost;
            _session.OnPeerDisconnected += HandleDisconnected;
            _session.OnConnectionLost += HandleConnectionLost;
        }

        private void CloseLink()
        {
            if (_link == null)
            {
                return;
            }
            _session.OnPeerDisconnected -= HandleDisconnected;
            _session.OnConnectionLost -= HandleConnectionLost;
            _link.OnTimedOut -= HandleConnectionLost;
            _link.Close();
            _link = null;
        }

        /// <summary>相手が抜けたら、ドラフト・持ち物・選出・バトルのどこでも止めて勝ち扱い</summary>
        private void HandleDisconnected()
        {
            StopOnline();
            ShowResult(true, "", DisconnectedDetail);
        }

        /// <summary>
        /// 自分の端末が裏で止まっていた／相手の返事が来ないまま止まった。どちらに原因があるか分からないので負け扱いにし、
        /// 部屋を抜けて相手には切断として伝える
        /// </summary>
        private void HandleConnectionLost()
        {
            StopOnline();
            _session.Leave();
            ShowResult(false, "", ConnectionLostDetail);
        }

        private void StopOnline()
        {
            CloseLink();
            StopAllCoroutines();
            _formatPanel.Hide();
            _teamSourcePanel.Hide();
            _draftPanel.Hide();
            _itemPanel.Hide();
            _selectPanel.Hide();
            _battle.Abort();
            GameMusic.Stop();
            _quitButton.Hide();
        }

        private void ShowOnlineResult(bool won)
        {
            _quitButton.Hide();
            ShowResult(won, "", won ? WinDetail : LoseDetail);
        }

        private void OnDestroy()
        {
            CloseLink();
        }
    }
}
