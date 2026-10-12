using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 技4つ（タイプの色付き・相手への相性つき）と「もどる」。
    /// 技ボタンを長押し（マウスは乗せる）すると、上に技の説明を出す
    /// </summary>
    public class MovePanel : MonoBehaviour
    {
        // 統一背景色の上で目立つよう、相性タグは明るい色にする
        private const string SuperEffectiveTag = "<color=#FF6B6B>抜群！</color>";
        private const string NotVeryEffectiveTag = "<color=#8E9AAF>今ひとつ</color>";
        private const string NoEffectTag = "<color=#6C6C7A>効果なし</color>";
        private const string HasEffectsMark = "　＋効果";
        private const string NoEffectsText = "特別な 効果は ない";

        [SerializeField] private Button[] _moveButtons;
        [SerializeField] private Text[] _moveLabels;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _hintBox;
        [SerializeField] private Text _hintText;

        private MoveHoldHint[] _hints;
        private IReadOnlyList<MoveDefinition> _moves;
        private Action<int> _onMove;
        private Action _onBack;

        private void Awake()
        {
            _hints = new MoveHoldHint[_moveButtons.Length];
            for (int i = 0; i < _moveButtons.Length; i++)
            {
                int index = i;
                _hints[i] = _moveButtons[i].GetComponent<MoveHoldHint>();
                _hints[i].Shown += () => ShowHint(index);
                _hints[i].Hidden += HideHint;
                _moveButtons[i].onClick.AddListener(() => OnMoveClicked(index));
            }
            _backButton.onClick.AddListener(() => Choose(_onBack));
        }

        /// <param name="targets">場の相手（ダブルスは2体）。相性の札はどの相手にも同じときだけ出す</param>
        public void Show(BattleMonster monster, IReadOnlyList<BattleMonster> targets, Action<int> onMove, Action onBack)
        {
            _onMove = onMove;
            _onBack = onBack;
            _moves = monster.Definition.Moves;

            for (int i = 0; i < _moveButtons.Length; i++)
            {
                var move = _moves[i];
                string tag = CommonTag(move, targets);
                string typeHex = ColorUtility.ToHtmlStringRGB(TypeColors.Get(move.Type));
                _moveLabels[i].text = $"{move.Name}\n<size=28><color=#{typeHex}>■</color> {move.Type.ToJapanese()}</size>{tag}";
            }
            HideHint();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 相手全員に同じ札が付くときだけその札。ダブルスで相手によって違うときは出さず、狙う相手を選ぶ窓で1体ずつ見せる
        /// </summary>
        private static string CommonTag(MoveDefinition move, IReadOnlyList<BattleMonster> targets)
        {
            if (targets.Count == 0)
            {
                return "";
            }
            string tag = EffectivenessTag(move.Effectiveness(targets[0].Definition.Types));
            for (int i = 1; i < targets.Count; i++)
            {
                if (EffectivenessTag(move.Effectiveness(targets[i].Definition.Types)) != tag)
                {
                    return "";
                }
            }
            return tag;
        }

        /// <summary>等倍は何も出さない（いつも出すと大事な ばつぐん が目立たなくなる）</summary>
        internal static string EffectivenessTag(float multiplier)
        {
            if (multiplier == 0f)
            {
                return "　" + NoEffectTag;
            }
            if (multiplier > 1f)
            {
                return "　" + SuperEffectiveTag;
            }
            return multiplier < 1f ? "　" + NotVeryEffectiveTag : "";
        }

        /// <summary>変化技は威力が無いので「へんか」と出す。効果の中身はボタンに入り切らないので説明に回す</summary>
        private static string Detail(MoveDefinition move)
        {
            string power = move.IsStatus ? "変化" : $"威力 {move.Power}";
            return move.Effects().Length > 0 ? power + HasEffectsMark : power;
        }

        private void ShowHint(int index)
        {
            var move = _moves[index];
            string effects = move.Effects();
            _hintText.text = $"{move.Name}　{move.Type.ToJapanese()}\n{move.Numbers()}\n"
                + (effects.Length > 0 ? effects : NoEffectsText);
            _hintBox.SetActive(true);
        }

        private void HideHint()
        {
            _hintBox.SetActive(false);
        }

        /// <summary>長押しで説明を見ただけのときは、指を離しても技を出さない</summary>
        private void OnMoveClicked(int index)
        {
            if (_hints[index].WasLongPress)
            {
                return;
            }
            Choose(() => _onMove?.Invoke(index));
        }

        private void Choose(Action action)
        {
            BattleSounds.Play(BattleSound.Cursor);
            HideHint();
            gameObject.SetActive(false);
            action?.Invoke();
        }
    }
}
