using System.Collections;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 名前・Lv・HPバー・状態異常の印の枠。HP は緑→黄→赤で色が変わり、減るときは少しずつ縮む。
    /// HP の数字は自分側だけ出す（相手は _hpText を空にしておく。ダイパと同じ）
    /// </summary>
    public class MonsterInfoBox : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _levelText;
        [SerializeField] private RectTransform _hpFill;
        [SerializeField] private Image _hpFillImage;
        [SerializeField] private Text _hpText;
        [SerializeField] private Image _statusBadge;
        [SerializeField] private Text _statusText;
        [Tooltip("HP が満タンから0になるまでにかかる秒数。減った量に比例して短くなる")]
        [SerializeField] private float _fullDrainSeconds = 1.2f;
        [SerializeField] private float _hpYellowRatio = 0.5f;
        [SerializeField] private float _hpRedRatio = 0.2f;
        [SerializeField] private Color _hpGreen = new Color(0.3f, 0.85f, 0.4f);
        [SerializeField] private Color _hpYellow = new Color(0.97f, 0.8f, 0.2f);
        [SerializeField] private Color _hpRed = new Color(0.92f, 0.3f, 0.25f);

        private const string TypeSize = "28";

        private int _maxHp;
        private float _shownHp;

        public void Show(BattleMonster monster, int hp, StatusCondition status)
        {
            _nameText.text = $"{monster.Name} <size={TypeSize}>{TypeLabel(monster)}</size>";
            _levelText.text = $"Lv{BattleBalance.Level}";
            _maxHp = monster.MaxHp;
            SetHp(hp);
            SetStatus(status);
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

        public void Hide()
        {
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

        private static string TypeLabel(BattleMonster monster)
        {
            var types = monster.Definition.Types;
            return types.Count == 1 ? types[0].ToJapanese() : $"{types[0].ToJapanese()}/{types[1].ToJapanese()}";
        }
    }
}
