using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// あそびかた。上のタブで ドラフト・選出・バトル・ダブルス・勝ち抜き戦・ストーリー を切り替える。
    /// 文面は仕様書のルールの要約。ルールを変えたらここも直す
    /// </summary>
    public class HowToPanel : SubMenuPanel
    {
        public static readonly string[] PageTitles = { "ドラフト", "選出", "バトル", "ダブルス", "勝ち抜き戦", "ストーリー" };

        private static readonly string[] PageBodies =
        {
            "・3体の 中から 欲しい ペンモンを 1体 選びます。これを 6回 繰り返して 6体の チームを 作ります\n"
            + "・左の カードを 押すと、右に タイプ・特性・種族値・技 が 出ます。「これに 決めた！」で 決定\n"
            + "・相手と 同じ ペンモンを 取っても 構いません\n"
            + "・オンライン対戦では 1回 15秒。過ぎると 3体から ランダムで 決まります\n"
            + "・6体 そろったら 持ち物を 1つ もらえます。好きな ペンモンに 持たせよう",

            "・お互いの 6体を 見せ合ってから、バトルに 出す 3体を 選びます\n"
            + "・押した 順番が 出す 順番 です（1番目が 先頭）\n"
            + "・もう一度 押すと 外せます。「おまかせ」で 残りを ランダムに 埋めます\n"
            + "・相手の タイプを 見て、相性の いい ペンモンを 選びましょう",

            "・毎ターン「戦う（技 4つ）」か「ペンモン（交代）」を 選びます\n"
            + "・交代が 先。次に 先制技、その後は 素早さの 高い 順に 動きます\n"
            + "・技の タイプが 相手に 抜群 だと 2倍、自分の タイプと 同じ 技は 1.5倍\n"
            + "・技の ボタンを 長押し すると、技の 説明が 出ます（交代の 画面では 特性の 説明）\n"
            + "・麻痺・毒・火傷・眠り・氷・混乱 や、能力の ランク にも 注意\n"
            + "・相手の 3体を 全部 倒したら 勝ち！",

            "・4体を 選んで、場に 2体ずつ 出して 戦います（最初に シングルか ダブルスを 選びます）\n"
            + "・左の ペンモンから 順に 行動を 選びます。技を 選んだら、狙う 相手も 選びます\n"
            + "・「相手 全員に 当たる」技は 2体に 当たりますが、ダメージは 0.75倍\n"
            + "・倒れたら 控えから 出します。控えが いなければ 1体で 戦い続けます\n"
            + "・相手の 4体を 全部 倒したら 勝ち！",

            "・最初の ドラフトで 選んだ 6体で、負けるまで 戦い続けます\n"
            + "・HP や 状態異常 は 毎回 全回復 します\n"
            + "・勝つほど 相手の CPU が 賢く、強く なります\n"
            + "・7戦目・14戦目… は ボス戦！\n"
            + "・何連勝 できるか 挑戦 しよう。最高記録は シングル・ダブルス 別々に タイトルに 出ます",

            "・博士から 最初の 1体を もらって 旅に 出よう。町の 道場で 羽章を 集めよう！\n"
            + "・十字キー（矢印・WASD）で 歩く。A（Z）で 話す・調べる、B（X）で メニュー\n"
            + "・シューズを もらうと「走る」（Shift）、自転車を もらうと「自転車」（C）で 外を 速く 進めます\n"
            + "・釣り竿を もらうと 水に 向いて A で 釣り。拾った 持ち物は 手持ちの 画面で「持たせる」\n"
            + "・草むらでは 野生の ペンモンが 出ます。弱らせて 捕獲タマゴを 投げると 捕まえられる\n"
            + "・上等・特上タマゴは よく 捕まる。網・夜・重い・空タマゴは 合う タイプに よく 効く\n"
            + "・トレーナーと 目が 合うと バトル。勝つと 経験値。レベルが 上がると 技を 覚え、進化も します\n"
            + "・ぬくもり小屋で 回復と あずかり箱、よろず屋で 道具の 売り買い。全滅すると お金が 半分に\n"
            + "・メニューの「記録する」で 保存。次は タイトルの「ストーリー」→「続きから」\n"
            + "・手持ちが 6体 そろうと、オンラインの「自分のチーム」で 戦えます（全員 Lv50）",
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
