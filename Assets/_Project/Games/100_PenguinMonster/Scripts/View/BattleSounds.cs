using System.Collections.Generic;
using PenguinMonster.Common.Audio;
using UnityEngine;

namespace PenguinMonster.View
{
    public enum BattleSound
    {
        Cursor,
        Attack,
        Hit,
        HitStrong,
        HitWeak,
        SendOut,
        Faint,
        Win,
        Lose,
    }

    /// <summary>
    /// バトルの仮の効果音。素材がまだ無いのでコードで作る（仕様書 §7）。
    /// 共通基盤の SeId はゲーム固有の音を持たないので、クリップを作って AudioManager に直接渡す（音量設定はそちらで掛かる）。
    /// 音の作り方は BattleSounds.Synth.cs
    /// </summary>
    public static partial class BattleSounds
    {
        private static readonly Dictionary<BattleSound, AudioClip> Clips = new Dictionary<BattleSound, AudioClip>();

        private static readonly Dictionary<BattleSound, float> Volumes = new Dictionary<BattleSound, float>
        {
            { BattleSound.Cursor, 0.35f },
            { BattleSound.Attack, 0.5f },
            { BattleSound.Hit, 0.7f },
            { BattleSound.HitStrong, 0.8f },
            { BattleSound.HitWeak, 0.5f },
            { BattleSound.SendOut, 0.5f },
            { BattleSound.Faint, 0.5f },
            { BattleSound.Win, 0.5f },
            { BattleSound.Lose, 0.5f },
        };

        public static void Play(BattleSound sound)
        {
            if (!Clips.TryGetValue(sound, out var clip) || clip == null)
            {
                clip = Create(sound);
                Clips[sound] = clip;
            }
            AudioManager.Instance?.PlaySeClip(clip, Volumes[sound]);
        }

        private static AudioClip Create(BattleSound sound)
        {
            switch (sound)
            {
                case BattleSound.Cursor: return Tone("Se_Cursor", 0.05f, 1320f);
                case BattleSound.Attack: return Whoosh("Se_Attack");
                case BattleSound.Hit: return Impact("Se_Hit", 0.16f, 110f);
                case BattleSound.HitStrong: return Impact("Se_HitStrong", 0.3f, 70f);
                case BattleSound.HitWeak: return Impact("Se_HitWeak", 0.1f, 220f);
                case BattleSound.SendOut: return Sweep("Se_SendOut", 0.22f, 400f, 1400f);
                case BattleSound.Faint: return Sweep("Se_Faint", 0.5f, 700f, 120f);
                case BattleSound.Win: return Jingle("Se_Win", new[] { 523f, 659f, 784f, 1047f });
                default: return Jingle("Se_Lose", new[] { 392f, 311f, 262f });
            }
        }
    }
}
