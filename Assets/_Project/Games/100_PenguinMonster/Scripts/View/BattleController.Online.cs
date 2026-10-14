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
        private const string WaitingText = "相手を 待っています…";

        /// <summary>ドラフト・選出の後など、相手がそろうまでの間に出しておく</summary>
        public void ShowWaiting(string text = WaitingText)
        {
            ClearField();
            _messageWindow.SetText(text);
        }

        /// <summary>相手が抜けたとき、選択の窓と制限時間を止める</summary>
        public void Abort()
        {
            HideChoices();
        }

        public IEnumerator RunHost(List<MonsterDefinition> ownParty, List<MonsterDefinition> guestParty,
            System.Random random, OnlineLink link, BattleFormat format)
        {
            BeginLocal(ownParty, guestParty, random, format, BattleBalance.OnlineTurnSeconds);

            yield return SendAndPlay(_engine.StartBattle(), link);
            while (!_state.IsOver)
            {
                yield return WaitForPlayerActions();
                List<BattleAction> guestActions = null;
                _messageWindow.SetText(WaitingText);
                yield return link.Wait(LinkMessage.Action, data => guestActions = OnlineCodec.DecodeActions(data));
                yield return SendAndPlay(_engine.ResolveTurn(_playerActions, guestActions), link);
                if (!_state.IsOver)
                {
                    yield return ReplaceFaintedAsHost(link);
                }
            }
        }

        public IEnumerator RunGuest(List<MonsterDefinition> ownParty, List<MonsterDefinition> hostParty, OnlineLink link,
            BattleFormat format)
        {
            ClearField();
            _engine = null;
            _state = new BattleState(ownParty, hostParty, format.ActiveCount());
            _choiceSeconds = BattleBalance.OnlineTurnSeconds;
            ShowTrays();

            yield return ReceiveAndPlay(link);
            while (!_state.IsOver)
            {
                yield return WaitForPlayerActions();
                link.Send(LinkMessage.Action, OnlineCodec.EncodeActions(_playerActions));
                _messageWindow.SetText(WaitingText);
                yield return ReceiveAndPlay(link);
                if (!_state.IsOver)
                {
                    yield return ReplaceFaintedAsGuest(link);
                }
            }
        }

        /// <summary>両方瀕死なら両方そろってから、まとめて出して送る（ゲストは1回の受信で全部を再生する）</summary>
        private IEnumerator ReplaceFaintedAsHost(OnlineLink link)
        {
            var ownSlots = Player.ReplaceableSlots();
            bool guest = Opponent.NeedsReplacement;
            if (ownSlots.Count == 0 && !guest)
            {
                yield break;
            }
            var ownChoices = new List<int>();
            yield return ChooseReplacements(ownSlots, ownChoices);
            int[] guestChoices = null;
            if (guest)
            {
                _messageWindow.SetText(WaitingText);
                yield return link.Wait(LinkMessage.Replace, data => guestChoices = data);
            }

            var events = new List<BattleEvent>();
            for (int i = 0; i < ownSlots.Count; i++)
            {
                events.AddRange(_engine.Replace(PlayerSide, ownSlots[i], ownChoices[i]));
            }
            // ゲストからは「場の番号・選出の何番目か」の組が並んで届く
            for (int i = 0; guestChoices != null && i + 1 < guestChoices.Length; i += 2)
            {
                events.AddRange(_engine.Replace(OpponentSide, guestChoices[i], guestChoices[i + 1]));
            }
            yield return SendAndPlay(events, link);
        }

        private IEnumerator ReplaceFaintedAsGuest(OnlineLink link)
        {
            var ownSlots = Player.ReplaceableSlots();
            if (ownSlots.Count == 0 && !Opponent.NeedsReplacement)
            {
                yield break;
            }
            if (ownSlots.Count > 0)
            {
                var choices = new List<int>();
                yield return ChooseReplacements(ownSlots, choices);
                var data = new List<int>();
                for (int i = 0; i < ownSlots.Count; i++)
                {
                    data.Add(ownSlots[i]);
                    data.Add(choices[i]);
                }
                link.Send(LinkMessage.Replace, data.ToArray());
            }
            _messageWindow.SetText(WaitingText);
            yield return ReceiveAndPlay(link);
        }

        /// <summary>空いた場ごとに出すペンモンを選ぶ。ホストがまとめて出すので、先に選んだ子は次の場では選べなくする</summary>
        private IEnumerator ChooseReplacements(List<int> slots, List<int> choices)
        {
            foreach (int slot in slots)
            {
                yield return ChooseReplacement(slot, choices);
                choices.Add(_replacementIndex);
            }
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
