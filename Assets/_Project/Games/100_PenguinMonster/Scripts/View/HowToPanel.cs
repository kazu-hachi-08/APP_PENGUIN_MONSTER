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
        public static readonly string[] PageTitles = { "ドラフト", "選出", "バトル", "勝ち抜き戦" };

        private static readonly string[] PageBodies =
        {
            "・3体の 中から 欲しい ペンモンを 1体 選びます。これを 6回 繰り返して 6体の チームを 作ります\n"
            + "・カードには タイプ・特性・持ち物・種族値 が 書いてあります\n"
            + "・相手と 同じ ペンモンを 取っても 構いません\n"
            + "・オンライン対戦では 1回 15秒。過ぎると 3体から ランダムで 決まります",

            "・お互いの 6体を 見せ合ってから、バトルに 出す 3体を 選びます\n"
            + "・押した 順番が 出す 順番 です（1番目が 先頭）\n"
            + "・もう一度 押すと 外せます。「おまかせ」で 残りを ランダムに 埋めます\n"
            + "・相手の タイプを 見て、相性の いい ペンモンを 選びましょう",

            "・毎ターン「戦う（技 4つ）」か「ペンモン（交代）」を 選びます\n"
            + "・交代が 先。次に 先制技、その後は 素早さの 高い 順に 動きます\n"
            + "・技の タイプが 相手に 抜群 だと 2倍、自分の タイプと 同じ 技は 1.5倍\n"
            + "・技の ボタンを 長押し すると、技の 説明が 出ます\n"
            + "・麻痺・毒・火傷・眠り・氷・混乱 や、能力の ランク にも 注意\n"
            + "・相手の 3体を 全部 倒したら 勝ち！",

            "・最初の ドラフトで 選んだ 6体で、負けるまで 戦い続けます\n"
            + "・HP や 状態異常 は 毎回 全回復 します\n"
            + "・勝つほど 相手の CPU が 賢く、強く なります\n"
            + "・7戦目・14戦目… は ボス戦！\n"
            + "・何連勝 できるか 挑戦 しよう。最高記録は タイトルに 出ます",
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
