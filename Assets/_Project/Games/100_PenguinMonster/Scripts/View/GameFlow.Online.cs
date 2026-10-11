using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Common.Online;
using PenguinMonster.Online;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// オンライン対戦の流れ（仕様書 §8）: 部屋を作る / コードで参加 → ドラフト → 見せ合い・選出 → バトル → 結果 → タイトル。
    /// ドラフトの抽選はホストが決めた種でゲストの端末が引く（提示はお互い別々で、相手の取り方に左右されないため）。
    /// 相手が抜けたら、どこにいても勝ち扱いで終わる
    /// </summary>
    public partial class GameFlow
    {
        private const string DisconnectedDetail = "相手が 切断しました";
        private const string OnlineQuitMessage = "相手の 勝ちに なります";

        [SerializeField] private OnlineSession _session;
        [SerializeField] private ModeSelectPanel _modeSelect;

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

            var random = new System.Random();
            System.Random draftRandom = null;
            yield return ShareDraftSeed(isHost.Value, random, seeded => draftRandom = seeded);

            IReadOnlyList<MonsterDefinition> ownSix = null;
            _draftPanel.Show(new Draft(MonsterTable.All, draftRandom), six => ownSix = six, BattleBalance.OnlineDraftSeconds);
            yield return new WaitUntil(() => ownSix != null);
            List<MonsterDefinition> opponentSix = null;
            yield return Exchange(LinkMessage.Six, ownSix, received => opponentSix = received);

            List<MonsterDefinition> ownTeam = null;
            _selectPanel.Show(ownSix, opponentSix, team => ownTeam = team, BattleBalance.OnlineSelectSeconds);
            yield return new WaitUntil(() => ownTeam != null);
            List<MonsterDefinition> opponentTeam = null;
            yield return Exchange(LinkMessage.Team, ownTeam, received => opponentTeam = received);

            yield return isHost.Value
                ? _battle.RunHost(ownTeam, opponentTeam, random, _link)
                : _battle.RunGuest(ownTeam, opponentTeam, _link);

            // 決着の後に相手がタイトルへ戻っても、切断で結果を上書きしない
            CloseLink();
            ShowOnlineResult(_battle.PlayerWon);
        }

        /// <summary>ホストは種を決めて送り、ゲストは届くのを待つ。ホスト自身のドラフトはホストの乱数で引く</summary>
        private IEnumerator ShareDraftSeed(bool isHost, System.Random random, System.Action<System.Random> onReady)
        {
            if (isHost)
            {
                _link.Send(LinkMessage.DraftSeed, new[] { random.Next() });
                onReady(random);
                yield break;
            }
            yield return _link.Wait(LinkMessage.DraftSeed, data => onReady(new System.Random(data[0])));
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
            _session.OnPeerDisconnected += HandleDisconnected;
        }

        private void CloseLink()
        {
            if (_link == null)
            {
                return;
            }
            _session.OnPeerDisconnected -= HandleDisconnected;
            _link.Close();
            _link = null;
        }

        /// <summary>相手が抜けたら、ドラフト・選出・バトルのどこでも止めて勝ち扱い（仕様書 §2.4）</summary>
        private void HandleDisconnected()
        {
            CloseLink();
            StopAllCoroutines();
            _draftPanel.Hide();
            _selectPanel.Hide();
            _battle.Abort();
            _quitButton.Hide();
            _resultDialog.Show(WinTitle, "", DisconnectedDetail, onTitle: BackToTitle);
        }

        private void ShowOnlineResult(bool won)
        {
            _quitButton.Hide();
            _resultDialog.Show(won ? WinTitle : LoseTitle, "", won ? WinDetail : LoseDetail, onTitle: BackToTitle);
        }

        private void OnDestroy()
        {
            CloseLink();
        }
    }
}
