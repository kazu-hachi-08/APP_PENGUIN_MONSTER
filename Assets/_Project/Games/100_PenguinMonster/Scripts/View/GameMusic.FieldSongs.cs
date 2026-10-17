namespace PenguinMonster.View
{
    /// <summary>
    /// ストーリーのフィールドの曲（PLAN_STORY_TOWN01.md フェーズ10）。書き方は GameMusic.Songs.cs と同じ。
    /// 楽譜は # しか読めないので、B♭ は A# と書く
    /// </summary>
    public static partial class GameMusic
    {
        // 村・町: 雪の村ののんびりした曲（F - Dm - B♭ - C）。何度も聞くので、タイトルより遅く静かにする
        private static readonly Song VillageSong = new Song
        {
            Name = "Bgm_Village",
            Bpm = 100f,
            Melody = @"
                A4 - - C5 A4 - F4 -
                D5 - C5 - A4 - - -
                A#4 - D5 - F5 - D5 -
                C5 - - - G4 - . .
                A4 - C5 - F5 - E5 -
                D5 - A4 - F4 - A4 -
                A#4 - A4 - G4 - A#4 -
                A4 - G4 - F4 - - -",
            Bass = @"
                F2 - C3 - F2 - C3 -
                D2 - A2 - D2 - A2 -
                A#2 - F3 - A#2 - F3 -
                C3 - G2 - C3 - G2 -
                F2 - C3 - F2 - C3 -
                D2 - A2 - D2 - A2 -
                A#2 - F3 - A#2 - F3 -
                C3 - G2 - C3 - E2 -",
            Drums = "k..hk.h.",
        };

        // 道路: 歩き出したくなる明るい曲（D - G - A - D / Bm - G - A - A）
        private static readonly Song RouteSong = new Song
        {
            Name = "Bgm_Route",
            Bpm = 132f,
            Melody = @"
                D5 - A4 - F#4 - A4 D5
                B4 - - - G4 - B4 -
                C#5 - E5 - A4 - C#5 -
                D5 - - - - - . .
                F#5 - E5 D5 B4 - D5 -
                G5 - F#5 - E5 - D5 -
                E5 - C#5 - A4 - E5 -
                A4 - B4 - C#5 - E5 -",
            Bass = @"
                D2 A2 D3 A2 D2 A2 D3 A2
                G2 D3 G3 D3 G2 D3 G3 D3
                A2 E3 A3 E3 A2 E3 A3 E3
                D2 A2 D3 A2 D2 A2 D3 A2
                B2 F#3 B3 F#3 B2 F#3 B3 F#3
                G2 D3 G3 D3 G2 D3 G3 D3
                A2 E3 A3 E3 A2 E3 A3 E3
                A2 E3 A3 E3 A2 C#3 E3 G3",
            Drums = "k.hhs.hh",
        };

        // 道場: 岩の道場の、重くて張りつめた短調の曲（Em - Em - C - D / Em - C - D - B）
        private static readonly Song GymSong = new Song
        {
            Name = "Bgm_Gym",
            Bpm = 138f,
            Melody = @"
                E5 - B4 - E5 - G5 -
                F#5 - E5 - D5 - B4 -
                C5 - E5 - G5 - E5 -
                D5 - F#5 - A5 - F#5 -
                G5 - - F#5 E5 - B4 -
                C5 - - D5 E5 - G5 -
                F#5 - E5 - D5 - A4 -
                B4 - D#5 - F#5 - B5 -",
            Bass = @"
                E2 E2 E3 E2 E2 E2 E3 E2
                E2 E2 E3 E2 E2 E2 E3 E2
                C2 C2 C3 C2 C2 C2 C3 C2
                D2 D2 D3 D2 D2 D2 D3 D2
                E2 E2 E3 E2 E2 E2 E3 E2
                C2 C2 C3 C2 C2 C2 C3 C2
                D2 D2 D3 D2 D2 D2 D3 D2
                B1 B1 B2 B1 B1 B1 B2 D#2",
            Drums = "k.skk.s.",
        };
    }
}
