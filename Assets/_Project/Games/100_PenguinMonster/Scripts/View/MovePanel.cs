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
        // タイプの明るい色の上でも読めるよう、濃い色にする
        private const string SuperEffectiveTag = "<color=#C0182A>抜群！</color>";
        private const string NotVeryEffectiveTag = "<color=#555A66>今ひとつ</color>";
        private const string NoEffectTag = "<color=#2A2A33>効果なし</color>";
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

        public void Show(BattleMonster monster, BattleMonster target, Action<int> onMove, Action onBack)
        {
            _onMove = onMove;
            _onBack = onBack;
            _moves = monster.Definition.Moves;

            for (int i = 0; i < _moveButtons.Length; i++)
            {
                var move = _moves[i];
                string tag = EffectivenessTag(move.Effectiveness(target.Definition.Types));
                _moveLabels[i].text = $"{move.Name}{tag}\n<size=26>{move.Type.ToJapanese()}  {Detail(move)}</size>";
                _moveButtons[i].image.color = TypeColors.Get(move.Type);
            }
            HideHint();
            gameObject.SetActive(true);
        }

        /// <summary>等倍は何も出さない（いつも出すと大事な ばつぐん が目立たなくなる）</summary>
        private static string EffectivenessTag(float multiplier)
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
