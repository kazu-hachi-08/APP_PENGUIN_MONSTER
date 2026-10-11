using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// あそびかた（Phase 8）。上のタブで ドラフト・選出・バトル・勝ち抜き戦 を切り替える。
    /// 文面は仕様書 §2・§4 の要約。ルールを変えたらここも直す
    /// </summary>
    public class HowToPanel : SubMenuPanel
    {
        public static readonly string[] PageTitles = { "ドラフト", "せんしゅつ", "バトル", "かちぬきせん" };

        private static readonly string[] PageBodies =
        {
            "・3体の なかから ほしい ペンモンを 1体 えらびます。これを 6回 くりかえして 6体の チームを つくります\n"
            + "・カードには タイプ・とくせい・もちもの・しゅぞくち が かいてあります\n"
            + "・あいてと おなじ ペンモンを とっても かまいません\n"
            + "・オンライン対戦では 1回 15びょう。すぎると 3体から ランダムで きまります",

            "・おたがいの 6体を みせあってから、バトルに だす 3体を えらびます\n"
            + "・おした じゅんばんが だす じゅんばん です（1ばんめが せんとう）\n"
            + "・もういちど おすと はずせます。「おまかせ」で のこりを ランダムに うめます\n"
            + "・あいての タイプを みて、あいしょうの いい ペンモンを えらびましょう",

            "・まいターン「たたかう（わざ 4つ）」か「ペンモン（こうたい）」を えらびます\n"
            + "・こうたいが さき。つぎに せんせいわざ、そのあとは すばやさの たかい じゅんに うごきます\n"
            + "・わざの タイプが あいてに ばつぐん だと 2ばい、じぶんの タイプと おなじ わざは 1.5ばい\n"
            + "・わざの ボタンを ながおし すると、わざの せつめいが でます\n"
            + "・まひ・どく・やけど・ねむり・こおり・こんらん や、のうりょくの ランク にも ちゅうい\n"
            + "・あいての 3体を ぜんぶ たおしたら かち！",

            "・さいしょの ドラフトで えらんだ 6体で、まけるまで たたかいつづけます\n"
            + "・HP や じょうたいいじょう は まいかい ぜんかいふく します\n"
            + "・かつほど あいての CPU が かしこく、つよく なります\n"
            + "・7せんめ・14せんめ… は ボスせん！\n"
            + "・なんれんしょう できるか ちょうせん しよう。さいこう きろくは タイトルに でます",
        };

        [SerializeField] private Button[] _tabButtons;
        [SerializeField] private Text _bodyText;

        private Color[] _tabColors;

        protected override void Awake()
        {
            base.Awake();
            _tabColors = ButtonHighlight.CaptureColors(_tabButtons);
            for (int i = 0; i < _tabButtons.Length; i++)
            {
                int index = i;
                _tabButtons[i].onClick.AddListener(() =>
                {
                    BattleSounds.Play(BattleSound.Cursor);
                    ShowPage(index);
                });
            }
        }

        protected override void OnShow()
        {
            ShowPage(0);
        }

        private void ShowPage(int index)
        {
            _bodyText.text = PageBodies[index];
            ButtonHighlight.Paint(_tabButtons, _tabColors, index);
        }
    }
}
