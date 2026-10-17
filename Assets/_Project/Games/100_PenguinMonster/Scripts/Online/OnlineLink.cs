using System;
using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Common.Online;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace PenguinMonster.Online
{
    /// <summary>ペンモンの対戦中にやり取りするメッセージの種類（仕様書 §8 のメッセージ一覧）</summary>
    public enum LinkMessage
    {
        /// <summary>ホスト → ゲスト: ゲストのドラフトの抽選に使う種・シングル / ダブルス・ドラフト / 自分のチーム</summary>
        DraftSeed,
        /// <summary>お互い: ドラフトで取った6体か、自分のチームの6体（No・持ち物・技の列）</summary>
        Six,
        /// <summary>お互い: 選出した3体（No の列。並びが出す順）</summary>
        Team,
        /// <summary>ゲスト → ホスト: そのターンの行動</summary>
        Action,
        /// <summary>ゲスト → ホスト: ひんしの後に出すペンモン（選出の何番目か）</summary>
        Replace,
        /// <summary>ホスト → ゲスト: 計算した結果の BattleEvent の列</summary>
        Events,
        /// <summary>ゲスト → ホスト: 自分のチームで戦えるか（OnlineTeamProblem。ホストが選べるかを決めるため）</summary>
        TeamReady,
    }

    /// <summary>
    /// 相手と int 列を送り合うだけの通信路。接続は共通基盤の OnlineSession に任せ、ここは中身の受け渡しだけを持つ。
    /// 届いたものは種類ごとに貯めておき、待つ側が取りに来る（相手の方が先に進んでいても取りこぼさない）
    /// </summary>
    public sealed class OnlineLink
    {
        private const string MessageName = "penmon.link";
        private const int InitialMessageSize = 256;
        // 1ターンのイベントは多くても数十個（1個 8 int）。余裕を持たせ、MTU を超える分は分割して送る
        private const int MaxMessageSize = 64 * 1024;

        private readonly Dictionary<LinkMessage, Queue<int[]>> _inbox = new Dictionary<LinkMessage, Queue<int[]>>();

        private static CustomMessagingManager Messaging =>
            NetworkManager.Singleton != null ? NetworkManager.Singleton.CustomMessagingManager : null;

        /// <summary>
        /// 接続できたコールバックの中で、すぐに呼ぶこと。
        /// ホストは接続直後に送ってくるので、1フレームでも遅れると最初のメッセージを取りこぼす
        /// </summary>
        public void Open()
        {
            Messaging?.RegisterNamedMessageHandler(MessageName, Receive);
        }

        public void Close()
        {
            Messaging?.UnregisterNamedMessageHandler(MessageName);
            _inbox.Clear();
        }

        public void Send(LinkMessage kind, int[] data)
        {
            var messaging = Messaging;
            if (messaging == null || !OnlineSession.TryGetPeerId(out ulong peerId))
            {
                return;
            }

            using var writer = new FastBufferWriter(InitialMessageSize, Allocator.Temp, MaxMessageSize);
            writer.WriteValueSafe((int)kind);
            writer.WriteValueSafe(data.Length);
            foreach (int value in data)
            {
                writer.WriteValueSafe(value);
            }
            messaging.SendNamedMessage(MessageName, peerId, writer, NetworkDelivery.ReliableFragmentedSequenced);
        }

        /// <summary>
        /// Wait が上限まで待っても届かなかった。接続は生きていても、お互いが相手の返事を待ち合って止まっているので、
        /// 受け取った側で試合を終える（onReceived は呼ばれない）
        /// </summary>
        public event Action OnTimedOut;

        /// <summary>指定の種類が届くまで待ってから渡す（コルーチン用）。上限なし（相手が人の操作で決めるのを待つとき用）</summary>
        public IEnumerator Wait(LinkMessage kind, Action<int[]> onReceived)
        {
            return Wait(kind, onReceived, float.PositiveInfinity);
        }

        /// <summary>指定の種類が届くまで、最大 timeoutSeconds 秒待ってから渡す。過ぎたら OnTimedOut を出す</summary>
        public IEnumerator Wait(LinkMessage kind, Action<int[]> onReceived, float timeoutSeconds)
        {
            var queue = QueueOf(kind);
            // アプリが裏で止まっていた分は OnlineSession が別に扱うので、ここは実時間でよい
            float deadline = Time.realtimeSinceStartup + timeoutSeconds;
            while (queue.Count == 0)
            {
                if (Time.realtimeSinceStartup >= deadline)
                {
                    Debug.LogWarning($"[OnlineLink] {kind} が {timeoutSeconds} 秒届かないので、通信が止まったとみなします");
                    OnTimedOut?.Invoke();
                    yield break;
                }
                yield return null;
            }
            onReceived(queue.Dequeue());
        }

        private void Receive(ulong senderId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out int kind);
            reader.ReadValueSafe(out int length);
            var data = new int[length];
            for (int i = 0; i < length; i++)
            {
                reader.ReadValueSafe(out data[i]);
            }
            QueueOf((LinkMessage)kind).Enqueue(data);
        }

        private Queue<int[]> QueueOf(LinkMessage kind)
        {
            if (!_inbox.TryGetValue(kind, out var queue))
            {
                queue = new Queue<int[]>();
                _inbox[kind] = queue;
            }
            return queue;
        }
    }
}
