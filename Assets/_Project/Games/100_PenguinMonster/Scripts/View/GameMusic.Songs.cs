namespace PenguinMonster.View
{
    /// <summary>
    /// 曲の楽譜。1つの記号が8分音符1つ分で、1行が1小節（8つ）。
    /// 音名（C4, G#3 など）で鳴らし、「-」は前の音を伸ばす、「.」は休み。
    /// メロディーとベースは同じ長さにする（曲の長さはメロディーで決まり、そこでループする）
    /// </summary>
    public static partial class GameMusic
    {
        private sealed class Song
        {
            public string Name;
            public float Bpm;
            public string Melody;
            public string Bass;
            public string Drums;
        }

        private static Song SongOf(MusicTrack track)
        {
            switch (track)
            {
                case MusicTrack.Draft: return DraftSong;
                case MusicTrack.Battle: return BattleSong;
                default: return TitleSong;
            }
        }

        // タイトル: 雪原をぴょこぴょこ跳ねる感じの、明るくのんびりした曲（C - Am - F - G）
        private static readonly Song TitleSong = new Song
        {
            Name = "Bgm_Title",
            Bpm = 120f,
            Melody = @"
                G4 - E4 G4 C5 - B4 -
                A4 - - G4 E4 - C4 -
                F4 - A4 - C5 - A4 -
                G4 - - - D4 - . .
                G4 - E4 G4 C5 - E5 -
                D5 - C5 - B4 - A4 -
                A4 - G4 - F4 - A4 -
                G4 - F4 - D4 - B3 -",
            Bass = @"
                C3 - G3 - C3 - G3 -
                A2 - E3 - A2 - E3 -
                F2 - C3 - F2 - C3 -
                G2 - D3 - G2 - D3 -
                C3 - G3 - C3 - G3 -
                A2 - E3 - A2 - E3 -
                F2 - C3 - F2 - C3 -
                G2 - D3 - G2 - B2 -",
            Drums = "k.h.s.h.",
        };

        // ドラフト〜選出: 考えている間の、少し落ち着いた曲（F - G - Em - Am）
        private static readonly Song DraftSong = new Song
        {
            Name = "Bgm_Draft",
            Bpm = 112f,
            Melody = @"
                A4 - - - C5 - A4 -
                B4 - - - D5 - B4 -
                G4 - - - B4 - G4 -
                A4 - - - E4 - - -
                F4 - A4 - C5 - F5 -
                D5 - B4 - G4 - B4 -
                E5 - D5 - B4 - G4 -
                A4 - - - - - . .",
            Bass = @"
                F2 - C3 - F2 - C3 -
                G2 - D3 - G2 - D3 -
                E2 - B2 - E2 - B2 -
                A2 - E3 - A2 - E3 -
                F2 - C3 - F2 - C3 -
                G2 - D3 - G2 - D3 -
                E2 - B2 - E2 - B2 -
                A2 - E3 - A2 - E3 -",
            Drums = "k.h.k.hh",
        };

        // バトル: 速くて勢いのある短調の曲（Am - F - G - E）
        private static readonly Song BattleSong = new Song
        {
            Name = "Bgm_Battle",
            Bpm = 150f,
            Melody = @"
                A4 - E4 - A4 B4 C5 -
                A4 - F4 - C5 - A4 -
                B4 - G4 - D5 - B4 -
                G#4 - E4 - B4 - G#4 -
                A4 - C5 - E5 - D5 C5
                C5 - A4 - F4 - A4 -
                B4 - D5 - G4 - B4 -
                E4 - G#4 - B4 - E5 -",
            Bass = @"
                A2 A3 A2 A3 A2 A3 A2 A3
                F2 F3 F2 F3 F2 F3 F2 F3
                G2 G3 G2 G3 G2 G3 G2 G3
                E2 E3 E2 E3 E2 E3 E2 E3
                A2 A3 A2 A3 A2 A3 A2 A3
                F2 F3 F2 F3 F2 F3 F2 F3
                G2 G3 G2 G3 G2 G3 G2 G3
                E2 E3 E2 E3 E2 E3 G#2 B2",
            Drums = "khshkksh",
        };
    }
}
