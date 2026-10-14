using System.Text;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 名前の枠をタップしたときに出す、ランク（能力変化）の一覧。原作のバトル情報画面と同じく ▲ を6つ並べて段数を見せる。
    /// 画面いっぱいの暗幕ごとボタンなので、どこをタップしても閉じる（開いている間は下の窓を押せない）
    /// </summary>
    public class StatStagePanel : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Text _titleText;
        [Tooltip("能力名・▲の並び・数字を別の列にして、行の頭をそろえる")]
        [SerializeField] private Text _namesText;
        [SerializeField] private Text _pipsText;
        [SerializeField] private Text _valuesText;

        private const string UpColor = "#4F7FE8";
        private const string DownColor = "#E85454";
        private const string EmptyColor = "#C8C8C8";

        private BattleMonster _shown;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
        }

        public void Show(BattleMonster monster)
        {
            _shown = monster;
            _titleText.SetFitted($"{monster.Name}の 能力変化");
            var names = new StringBuilder();
            var pips = new StringBuilder();
            var values = new StringBuilder();
            for (int i = 0; i < StatKindNames.Count; i++)
            {
                var stat = (StatKind)i;
                int stage = monster.GetStage(stat);
                if (i > 0)
                {
                    names.Append('\n');
                    pips.Append('\n');
                    values.Append('\n');
                }
                names.Append(stat.ToJapanese());
                pips.Append(Pips(stage));
                values.Append(Value(stage));
            }
            _namesText.text = names.ToString();
            _pipsText.text = pips.ToString();
            _valuesText.text = values.ToString();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _shown = null;
            gameObject.SetActive(false);
        }

        /// <summary>そのペンモンが場から消えたら、古い一覧を残さないよう閉じる</summary>
        public void HideIfShowing(BattleMonster monster)
        {
            if (_shown != null && _shown == monster)
            {
                Hide();
            }
        }

        /// <summary>上限（6段）までの枠を全部出し、埋まった数で段数を見せる。0 は空の ▲ だけ</summary>
        private static string Pips(int stage)
        {
            char mark = stage < 0 ? '▼' : '▲';
            int filled = Mathf.Abs(stage);
            int empty = BattleBalance.MaxStatStage - filled;
            string color = stage > 0 ? UpColor : DownColor;
            var sb = new StringBuilder();
            if (filled > 0) sb.Append($"<color={color}>{new string(mark, filled)}</color>");
            if (empty > 0) sb.Append($"<color={EmptyColor}>{new string(mark, empty)}</color>");
            return sb.ToString();
        }

        private static string Value(int stage)
        {
            if (stage == 0) return $"<color={EmptyColor}>±0</color>";
            string color = stage > 0 ? UpColor : DownColor;
            return $"<color={color}>{stage:+0;-0}</color>";
        }
    }
}
