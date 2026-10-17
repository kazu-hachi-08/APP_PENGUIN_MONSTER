using System.Collections.Generic;
using PenguinMonster.Common.Audio;
using UnityEngine;

namespace PenguinMonster.View
{
    public enum MusicTrack
    {
        Title,
        Draft,
        Battle,
        /// <summary>ストーリーの村・町・建物の中</summary>
        Village,
        /// <summary>ストーリーの道路</summary>
        Route,
        /// <summary>ストーリーの道場</summary>
        Gym,
    }

    /// <summary>
    /// 仮の BGM。素材がまだ無いので楽譜（GameMusic.Songs.cs）から曲をコードで作る（仕様書 §7）。
    /// 再生は共通基盤の AudioManager に任せる。同じ曲が続くときに頭から流し直さない判定とクロスフェードは、AudioManager がやってくれる。
    /// 音の作り方は GameMusic.Synth.cs
    /// </summary>
    public static partial class GameMusic
    {
        // 効果音が BGM に埋もれないよう、BGM は控えめにする
        private const float MusicVolume = 0.5f;
        private const float StopFadeSeconds = 0.3f;

        // 共通基盤の BgmId は「何番目の曲か」だけを表す汎用の名前なので、ペンモンの曲を順に割り当てる
        private static readonly Dictionary<MusicTrack, BgmId> Ids = new Dictionary<MusicTrack, BgmId>
        {
            { MusicTrack.Title, BgmId.TitleBgm },
            { MusicTrack.Draft, BgmId.GameBgm01 },
            { MusicTrack.Battle, BgmId.GameBgm02 },
            { MusicTrack.Village, BgmId.GameBgm03 },
            { MusicTrack.Route, BgmId.GameBgm04 },
            { MusicTrack.Gym, BgmId.GameBgm05 },
        };

        private static readonly Dictionary<MusicTrack, AudioClip> Clips = new Dictionary<MusicTrack, AudioClip>();

        public static void Play(MusicTrack track)
        {
            if (!Clips.TryGetValue(track, out var clip) || clip == null)
            {
                clip = Render(SongOf(track));
                Clips[track] = clip;
            }
            var audio = AudioManager.Instance;
            if (audio == null)
            {
                return;
            }
            // AudioManager が作り直されても鳴るよう、毎回登録し直す（辞書に入れるだけなので軽い）
            audio.RegisterBgm(Ids[track], clip, MusicVolume);
            audio.PlayBgm(Ids[track]);
        }

        /// <summary>勝ち・負けのジングルと重ならないよう、決着で止める</summary>
        public static void Stop()
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.StopBgm(StopFadeSeconds);
            }
        }
    }
}
