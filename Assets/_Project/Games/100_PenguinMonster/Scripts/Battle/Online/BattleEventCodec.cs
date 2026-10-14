using System.Collections.Generic;

namespace PenguinMonster.Battle
{
    /// <summary>
    /// オンライン対戦で、ホストが計算した BattleEvent をゲストへ送るための int 列への変換（仕様書 §8）。
    /// ペンモンは参照のままでは送れないので「陣営・選出の何番目か」、技は「4つのうち何番目か」で表す。
    /// ゲストは自分が手前（陣営 0）になるよう陣営を入れ替えた写し（BattleState）を持ち、受け取るたびに写しを更新する。
    /// 写しがあれば、交代の画面やひんしの入れ替えをオフラインと同じ BattleSide のまま扱える
    /// </summary>
    public static class BattleEventCodec
    {
        public const int FieldsPerEvent = 9;
        private const int None = -1;

        public static int[] Encode(IReadOnlyList<BattleEvent> events, BattleState state)
        {
            var data = new int[events.Count * FieldsPerEvent];
            for (int i = 0; i < events.Count; i++)
            {
                Write(events[i], state, data, i * FieldsPerEvent);
            }
            return data;
        }

        /// <summary>受け取った列をイベントに戻しながら、写しの HP・状態異常・場・勝敗を更新する</summary>
        /// <param name="swapSides">ゲストは自分を陣営 0 で持つので、陣営を入れ替えて読む</param>
        public static List<BattleEvent> Decode(int[] data, BattleState mirror, bool swapSides)
        {
            var events = new List<BattleEvent>();
            for (int offset = 0; offset + FieldsPerEvent <= data.Length; offset += FieldsPerEvent)
            {
                events.Add(Read(data, offset, mirror, swapSides));
            }
            return events;
        }

        private static void Write(BattleEvent e, BattleState state, int[] data, int offset)
        {
            data[offset] = (int)e.Kind;
            data[offset + 1] = e.Side;
            data[offset + 2] = e.Monster == null ? None : PartyIndexOf(state.Sides[e.Side], e.Monster);
            data[offset + 3] = e.Monster == null ? None : MoveIndexOf(e.Monster.Definition, e.MoveName);
            data[offset + 4] = e.Hp;
            data[offset + 5] = (int)e.Status;
            data[offset + 6] = (int)e.Stat;
            data[offset + 7] = e.Amount;
            data[offset + 8] = e.Slot;
        }

        private static BattleEvent Read(int[] data, int offset, BattleState mirror, bool swapSides)
        {
            var kind = (BattleEventKind)data[offset];
            int side = swapSides ? 1 - data[offset + 1] : data[offset + 1];
            int partyIndex = data[offset + 2];
            int moveIndex = data[offset + 3];
            var monster = partyIndex == None ? null : mirror.Sides[side].Party[partyIndex];
            string moveName = moveIndex == None ? null : monster.Definition.Moves[moveIndex].Name;

            var e = BattleEvent.Restore(kind, side, data[offset + 8], monster, moveName,
                data[offset + 4], (StatusCondition)data[offset + 5], (StatKind)data[offset + 6], data[offset + 7]);
            Apply(mirror, e, partyIndex);
            return e;
        }

        /// <summary>
        /// 表示に要る分だけ写しに入れる（HP・状態異常・場・勝敗・天候・追い風）。
        /// ランクやこんらんの残りは画面に出さず、計算もホストがするので写さない
        /// </summary>
        private static void Apply(BattleState mirror, BattleEvent e, int partyIndex)
        {
            if (e.Kind == BattleEventKind.BattleEnd)
            {
                mirror.Finish(e.Side);
                return;
            }
            if (e.Kind == BattleEventKind.WeatherStarted || e.Kind == BattleEventKind.WeatherEnded)
            {
                // 残りターンはホストが数えるので、写しは天候の種類だけ合わせる
                mirror.SetWeather(e.Kind == BattleEventKind.WeatherStarted ? e.Weather : Weather.None, 0);
            }
            if (e.Kind == BattleEventKind.TailwindStarted || e.Kind == BattleEventKind.TailwindEnded)
            {
                // 天候と同じく、残りはホストが数えるので吹いているかどうかだけ合わせる
                mirror.Sides[e.Side].SetTailwind(e.Kind == BattleEventKind.TailwindStarted ? BattleBalance.TailwindTurns : 0);
            }
            if (e.Monster == null)
            {
                return;
            }
            e.Monster.Restore(e.Hp, e.Status);
            if (e.Kind == BattleEventKind.SentOut)
            {
                mirror.Sides[e.Side].SetActive(e.Slot, partyIndex);
            }
        }

        private static int PartyIndexOf(BattleSide side, BattleMonster monster)
        {
            for (int i = 0; i < side.Party.Count; i++)
            {
                if (side.Party[i] == monster)
                {
                    return i;
                }
            }
            return None;
        }

        private static int MoveIndexOf(MonsterDefinition definition, string moveName)
        {
            if (moveName == null)
            {
                return None;
            }
            for (int i = 0; i < definition.Moves.Count; i++)
            {
                if (definition.Moves[i].Name == moveName)
                {
                    return i;
                }
            }
            return None;
        }
    }
}
