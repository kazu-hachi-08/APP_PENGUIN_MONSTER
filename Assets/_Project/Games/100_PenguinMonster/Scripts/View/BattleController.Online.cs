using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Online;

namespace PenguinMonster.View
{
    /// <summary>
    /// オンライン対戦（仕様書 §8）。計算はホストだけがする。
    /// ホスト: 自分の行動とゲストから届いた行動でターンを解決し、出てきた BattleEvent をゲストへ送ってから再生する。
    /// ゲスト: 行動を送り、届いた BattleEvent で写しを更新して再生する。写しは自分が手前（陣営 0）になるよう陣営を入れ替えてある
    /// </summary>
    public partial class BattleController
    {
        private const string WaitingText = "あいてを まっています…";

        /// <summary>ドラフト・選出の後、相手がそろうまでの間に出しておく</summary>
        public void ShowWaiting()
        {
            ClearField();
            _messageWindow.SetText(WaitingText);
        }

        /// <summary>相手が抜けたとき、選択の窓と制限時間を止める</summary>
        public void Abort()
        {
            HideChoices();
        }

        public IEnumerator RunHost(List<MonsterDefinition> ownParty, List<MonsterDefinition> guestParty,
            System.Random random, OnlineLink link)
        {
            BeginLocal(ownParty, guestParty, random, BattleBalance.OnlineTurnSeconds);

            yield return SendAndPlay(_engine.StartBattle(), link);
            while (!_state.IsOver)
            {
                yield return WaitForPlayerAction();
                BattleAction guestAction = null;
                _messageWindow.SetText(WaitingText);
                yield return link.Wait(LinkMessage.Action, data => guestAction = OnlineCodec.DecodeAction(data));
                yield return SendAndPlay(_engine.ResolveTurn(_playerAction, guestAction), link);
                if (!_state.IsOver)
                {
                    yield return ReplaceFaintedAsHost(link);
                }
            }
        }

        public IEnumerator RunGuest(List<MonsterDefinition> ownParty, List<MonsterDefinition> hostParty, OnlineLink link)
        {
            ClearField();
            _engine = null;
            _state = new BattleState(ownParty, hostParty);
            _choiceSeconds = BattleBalance.OnlineTurnSeconds;

            yield return ReceiveAndPlay(link);
            while (!_state.IsOver)
            {
                yield return WaitForPlayerAction();
                link.Send(LinkMessage.Action, OnlineCodec.EncodeAction(_playerAction));
                _messageWindow.SetText(WaitingText);
                yield return ReceiveAndPlay(link);
                if (!_state.IsOver)
                {
                    yield return ReplaceFaintedAsGuest(link);
                }
            }
        }

        /// <summary>両方ひんしなら両方そろってから、まとめて出して送る（ゲストは1回の受信で両方を再生する）</summary>
        private IEnumerator ReplaceFaintedAsHost(OnlineLink link)
        {
            bool own = Player.NeedsReplacement;
            bool guest = Opponent.NeedsReplacement;
            if (!own && !guest)
            {
                yield break;
            }
            if (own)
            {
                yield return ChooseReplacement();
            }
            int guestIndex = -1;
            if (guest)
            {
                _messageWindow.SetText(WaitingText);
                yield return link.Wait(LinkMessage.Replace, data => guestIndex = data[0]);
            }

            var events = new List<BattleEvent>();
            if (own)
            {
                events.AddRange(_engine.Replace(PlayerSide, _replacementIndex));
            }
            if (guest)
            {
                events.AddRange(_engine.Replace(OpponentSide, guestIndex));
            }
            yield return SendAndPlay(events, link);
        }

        private IEnumerator ReplaceFaintedAsGuest(OnlineLink link)
        {
            if (!Player.NeedsReplacement && !Opponent.NeedsReplacement)
            {
                yield break;
            }
            if (Player.NeedsReplacement)
            {
                yield return ChooseReplacement();
                link.Send(LinkMessage.Replace, new[] { _replacementIndex });
            }
            _messageWindow.SetText(WaitingText);
            yield return ReceiveAndPlay(link);
        }

        /// <summary>送ってから再生する。ゲストはホストの再生を待たずに同時に見られる</summary>
        private IEnumerator SendAndPlay(List<BattleEvent> events, OnlineLink link)
        {
            link.Send(LinkMessage.Events, BattleEventCodec.Encode(events, _state));
            yield return Play(events);
        }

        private IEnumerator ReceiveAndPlay(OnlineLink link)
        {
            List<BattleEvent> events = null;
            yield return link.Wait(LinkMessage.Events, data => events = BattleEventCodec.Decode(data, _state, swapSides: true));
            yield return Play(events);
        }
    }
}
