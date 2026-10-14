using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ドラフトの右の大きな詳細（仕様書 §2.1）。選んでいる1体の 絵・名前・タイプ・合計・特性・種族値（バー）・技4つ をまとめて出す。
    /// 広い欄に両方並べられるので、種族値 ⇄ 技 の切り替えは要らない
    /// </summary>
    public class DraftDetailView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private Image[] _typeBadges;
        [SerializeField] private Text[] _typeLabels;
        [Tooltip("種族値の合計。強さの目安として名前の帯の右に大きく出す")]
        [SerializeField] private Text _statTotal;
        [Tooltip("特性と持ち物（名前と説明を1行ずつ）")]
        [SerializeField] private Text _traits;
        [Tooltip("HP・攻撃・防御・特攻・特防・素早さ の順")]
        [SerializeField] private Text[] _statValues;
        [Tooltip("バーの中身。右端のアンカーを種族値の割合にして伸ばす")]
        [SerializeField] private RectTransform[] _statFills;
        [Tooltip("技1つにつき1マス。マスをタイプの色で塗る")]
        [SerializeField] private Image[] _moveTiles;
        [SerializeField] private Text[] _moveNames;
        [SerializeField] private Text[] _moveDetails;
        [SerializeField] private PopIn _pop;

        private const int TraitDescriptionSize = 30;
        // 一番高い種族値（135）でもバーが振り切れないよう、少し上を満タンにする
        private const float StatBarMax = 150f;

        // 低い・ふつう・高い・とても高い を色でも見分けられるようにする（数字を読まなくても比べられるため）
        private static readonly int[] StatThresholds = { 60, 90, 110 };
        private static readonly Color[] StatBarColors =
        {
            new Color(0.9f, 0.45f, 0.3f),
            new Color(0.95f, 0.75f, 0.25f),
            new Color(0.45f, 0.8f, 0.35f),
            new Color(0.3f, 0.72f, 0.92f),
        };

        /// <summary>取ったペンモンが下のマスへ飛び立つ位置</summary>
        public RectTransform IconRect => _icon.rectTransform;

        public void Show(MonsterDefinition monster)
        {
            _icon.sprite = MonsterSprites.Front(monster);
            _name.SetFitted(monster.Name);
            TypeColors.PaintBadges(monster, _typeBadges, _typeLabels);
            _traits.SetFitted(MonsterTexts.Traits(monster, TraitDescriptionSize, oneLine: true));
            ShowStats(monster.BaseStats);
            ShowMoves(monster);
            _pop.Play();
        }

        private void ShowStats(Stats stats)
        {
            int[] values = { stats.Hp, stats.Attack, stats.Defense, stats.SpAttack, stats.SpDefense, stats.Speed };
            for (int i = 0; i < values.Length; i++)
            {
                _statValues[i].text = values[i].ToString();
                _statFills[i].anchorMax = new Vector2(Mathf.Clamp01(values[i] / StatBarMax), 1f);
                _statFills[i].GetComponent<Image>().color = StatBarColor(values[i]);
            }
            _statTotal.text = stats.Total.ToString();
        }

        private void ShowMoves(MonsterDefinition monster)
        {
            for (int i = 0; i < _moveTiles.Length; i++)
            {
                bool exists = i < monster.Moves.Count;
                _moveTiles[i].gameObject.SetActive(exists);
                if (!exists) continue;
                var move = monster.Moves[i];
                _moveTiles[i].color = TypeColors.Get(move.Type);
                _moveNames[i].text = move.Name;
                _moveDetails[i].text = $"{move.Type.ToJapanese()}　{MonsterTexts.MoveDetail(move)}";
            }
        }

        private static Color StatBarColor(int value)
        {
            for (int i = 0; i < StatThresholds.Length; i++)
            {
                if (value < StatThresholds[i]) return StatBarColors[i];
            }
            return StatBarColors[StatBarColors.Length - 1];
        }
    }
}
