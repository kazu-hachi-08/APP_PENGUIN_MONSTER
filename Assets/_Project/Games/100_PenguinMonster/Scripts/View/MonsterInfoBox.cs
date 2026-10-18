using System.Collections;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 名前・Lv・HPバー・状態異常の印の枠。HP は緑→黄→赤で色が変わり、減るときは少しずつ縮む。
    /// HP の数字は自分側だけ出す（相手は _hpText を空にしておく。ダイパと同じ）。
    /// ランクは枠に ▲▼ の印だけ出し、段数は枠をタップして開く一覧で見せる（原作と同じく枠を狭くしないため）
    /// </summary>
    public class MonsterInfoBox : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _levelText;
        [SerializeField] private RectTransform _hpFill;
        [SerializeField] private Image _hpFillImage;
        [SerializeField] private Text _hpText;
        [SerializeField] private Image _statusBadge;
        [SerializeField] private Text _statusText;
        [Tooltip("混乱中に出す印。状態異常と重なるので、状態異常の印とは別に持つ")]
        [SerializeField] private GameObject _confusionBadge;
        [Tooltip("ランクが変わっているときに出す ▲▼ の印")]
        [SerializeField] private Text _stageMarkText;
        [Tooltip("タップで開くランクの一覧（全部の枠で1つを共有する）")]
        [SerializeField] private StatStagePanel _stagePanel;
        [Tooltip("HP が満タンから0になるまでにかかる秒数。減った量に比例して短くなる")]
        [SerializeField] private float _fullDrainSeconds = 1.2f;
        [SerializeField] private float _hpYellowRatio = 0.5f;
        [SerializeField] private float _hpRedRatio = 0.2f;
        [SerializeField] private Color _hpGreen = new Color(0.3f, 0.85f, 0.4f);
        [SerializeField] private Color _hpYellow = new Color(0.97f, 0.8f, 0.2f);
        [SerializeField] private Color _hpRed = new Color(0.92f, 0.3f, 0.25f);

        private const string TypeSize = "28";

        private const string UpMark = "<color=#4F7FE8>▲</color>";
        private const string DownMark = "<color=#E85454>▼</color>";

        private BattleMonster _monster;
        private int _maxHp;
        private float _shownHp;

        public void Show(BattleMonster monster, int hp, StatusCondition status)
        {
            _monster = monster;
            _nameText.SetFitted($"{monster.Name} <size={TypeSize}>{TypeLabel(monster)}</size>");
            _levelText.text = $"Lv{monster.Definition.Level}";
            _maxHp = monster.MaxHp;
            SetHp(hp);
            SetStatus(status);
            // 混乱は交代で治るので、場に出た時点では必ず消えている
            SetConfused(false);
            UpdateStatStages(monster);
            gameObject.SetActive(true);
        }

        /// <summary>状態異常の印。なしのときは印ごと隠す</summary>
        public void SetStatus(StatusCondition status)
        {
            bool has = status != StatusCondition.None;
            _statusBadge.gameObject.SetActive(has);
            if (has)
            {
                _statusBadge.color = StatusColors.Get(status);
                _statusText.text = status.ToJapanese();
            }
        }

        public void SetConfused(bool confused) => _confusionBadge.SetActive(confused);

        public void Hide()
        {
            _stagePanel.HideIfShowing(_monster);
            gameObject.SetActive(false);
        }

        public IEnumerator AnimateHp(int hp)
        {
            float from = _shownHp;
            float seconds = _fullDrainSeconds * Mathf.Abs(from - hp) / Mathf.Max(1, _maxHp);
            for (float elapsed = 0f; elapsed < seconds; elapsed += Time.deltaTime)
            {
                SetHp(Mathf.Lerp(from, hp, elapsed / seconds));
                yield return null;
            }
            SetHp(hp);
        }

        private void SetHp(float hp)
        {
            _shownHp = hp;
            float ratio = _maxHp > 0 ? hp / _maxHp : 0f;
            _hpFill.anchorMax = new Vector2(ratio, 1f);
            _hpFillImage.color = ratio > _hpYellowRatio ? _hpGreen : ratio > _hpRedRatio ? _hpYellow : _hpRed;
            if (_hpText != null)
            {
                // 減っている途中も数字が動くよう、表示中の値を切り上げて出す（0 になるのは本当に 0 のときだけ）
                _hpText.text = $"{Mathf.CeilToInt(hp)} / {_maxHp}";
            }
        }

        /// <summary>上がった能力があれば ▲、下がった能力があれば ▼ を出す（両方なら ▲▼）</summary>
        public void UpdateStatStages(BattleMonster monster)
        {
            bool anyUp = false;
            bool anyDown = false;
            for (int i = 0; i < StatKindNames.Count; i++)
            {
                int stage = monster.GetStage((StatKind)i);
                anyUp |= stage > 0;
                anyDown |= stage < 0;
            }
            _stageMarkText.gameObject.SetActive(anyUp || anyDown);
            _stageMarkText.text = (anyUp ? UpMark : "") + (anyDown ? DownMark : "");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_monster != null)
            {
                _stagePanel.Show(_monster);
            }
        }

        private static string TypeLabel(BattleMonster monster)
        {
            var types = monster.Definition.Types;
            return types.Count == 1 ? types[0].ToJapanese() : $"{types[0].ToJapanese()}/{types[1].ToJapanese()}";
        }
    }
}
