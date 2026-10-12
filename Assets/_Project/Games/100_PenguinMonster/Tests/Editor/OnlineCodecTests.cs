using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PenguinMonster.Battle.Tests
{
    /// <summary>
    /// オンラインの送受信の変換。ホストのバトルを最後まで進めながら、ゲスト側の写し（陣営を入れ替えたもの）が同じ状態になるかを見る
    /// </summary>
    public class OnlineCodecTests
    {
        private static List<MonsterDefinition> HostTeam => MonsterTable.All.Take(3).ToList();
        private static List<MonsterDefinition> GuestTeam => MonsterTable.All.Skip(3).Take(3).ToList();

        [Test]
        public void ActionRoundTrips()
        {
            var move = OnlineCodec.DecodeAction(OnlineCodec.EncodeAction(BattleAction.UseMove(3)));
            var change = OnlineCodec.DecodeAction(OnlineCodec.EncodeAction(BattleAction.SwitchTo(2)));

            Assert.AreEqual(BattleActionKind.Move, move.Kind);
            Assert.AreEqual(3, move.Index);
            Assert.AreEqual(BattleActionKind.Switch, change.Kind);
            Assert.AreEqual(2, change.Index);
        }

        [Test]
        public void MonstersRoundTripByNo()
        {
            var six = MonsterTable.All.Skip(10).Take(6).ToList();

            var decoded = OnlineCodec.DecodeMonsters(OnlineCodec.EncodeMonsters(six));

            CollectionAssert.AreEqual(six, decoded);
        }

        [Test]
        public void DecodedEventsSwapSidesAndRestoreMoveNames()
        {
            var host = new BattleEngine(new BattleState(HostTeam, GuestTeam), new System.Random(1));
            var mirror = new BattleState(GuestTeam, HostTeam);
            BattleEventCodec.Decode(BattleEventCodec.Encode(host.StartBattle(), host.State), mirror, swapSides: true);

            var sent = host.ResolveTurn(BattleAction.UseMove(0), BattleAction.UseMove(0));
            var received = BattleEventCodec.Decode(BattleEventCodec.Encode(sent, host.State), mirror, swapSides: true);

            Assert.AreEqual(sent.Count, received.Count);
            for (int i = 0; i < sent.Count; i++)
            {
                Assert.AreEqual(sent[i].Kind, received[i].Kind);
                Assert.AreEqual(1 - sent[i].Side, received[i].Side);
                Assert.AreEqual(sent[i].MoveName, received[i].MoveName);
                Assert.AreEqual(sent[i].Hp, received[i].Hp);
                Assert.AreEqual(sent[i].Monster?.Name, received[i].Monster?.Name);
            }
        }

        /// <summary>6体ずつに分けて戦わせ、特性・持ち物・天候で HP が増減しても写しがずれないかも見る（50〜 は Phase 11 の子）</summary>
        [TestCase(0)]
        [TestCase(6)]
        [TestCase(12)]
        [TestCase(18)]
        [TestCase(50)]
        [TestCase(56)]
        [TestCase(62)]
        [TestCase(68)]
        public void MirrorFollowsHostUntilTheEnd(int firstIndex)
        {
            var hostTeam = MonsterTable.All.Skip(firstIndex).Take(3).ToList();
            var guestTeam = MonsterTable.All.Skip(firstIndex + 3).Take(3).ToList();
            var random = new System.Random(7);
            var host = new BattleEngine(new BattleState(hostTeam, guestTeam), random);
            var mirror = new BattleState(guestTeam, hostTeam);
            var cpu = new CpuPlayer(new System.Random(3), CpuLevel.Normal);

            Send(host.StartBattle(), host, mirror);
            for (int turn = 0; turn < 100 && !host.State.IsOver; turn++)
            {
                var sides = host.State.Sides;
                Send(host.ResolveTurn(cpu.ChooseAction(sides[0], sides[1]), cpu.ChooseAction(sides[1], sides[0])), host, mirror);
                for (int side = 0; side < BattleState.SideCount && !host.State.IsOver; side++)
                {
                    if (sides[side].NeedsReplacement)
                    {
                        Send(host.Replace(side, cpu.ChooseReplacement(sides[side], sides[1 - side])), host, mirror);
                    }
                }
                AssertMirrored(host.State, mirror);
            }

            Assert.IsTrue(host.State.IsOver);
            Assert.IsTrue(mirror.IsOver);
            Assert.AreEqual(1 - host.State.Winner, mirror.Winner);
        }

        private static void Send(List<BattleEvent> events, BattleEngine host, BattleState mirror)
        {
            BattleEventCodec.Decode(BattleEventCodec.Encode(events, host.State), mirror, swapSides: true);
        }

        private static void AssertMirrored(BattleState host, BattleState mirror)
        {
            for (int side = 0; side < BattleState.SideCount; side++)
            {
                var original = host.Sides[side];
                var copy = mirror.Sides[1 - side];
                Assert.AreEqual(host.Weather, mirror.Weather);
                Assert.AreEqual(original.ActivePartyIndex, copy.ActivePartyIndex);
                for (int i = 0; i < original.Party.Count; i++)
                {
                    Assert.AreEqual(original.Party[i].CurrentHp, copy.Party[i].CurrentHp);
                    Assert.AreEqual(original.Party[i].Status, copy.Party[i].Status);
                }
            }
        }
    }
}
