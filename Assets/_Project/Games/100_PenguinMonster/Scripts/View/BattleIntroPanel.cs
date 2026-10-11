using System;
using System.Collections;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 勝ち抜き戦の毎試合の前に出す画面（仕様書 §2.3）。何戦目かを見せ、ボスのときは
    /// 背景を変えて名前・セリフを出し、先頭のペンモンを「ぽよん」と登場させる。どこを押しても次へ
    /// </summary>
    public class BattleIntroPanel : MonoBehaviour
    {
        [SerializeField] private Image _back;
        [SerializeField] private Text _headline;
        [SerializeField] private Text _subText;
        [SerializeField] private Text _bossName;
        [SerializeField] private Text _bossLine;
        [SerializeField] private Image _bossAce;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Color _normalColor = new Color(0.16f, 0.24f, 0.36f);
        [SerializeField] private Color _bossColor = new Color(0.4f, 0.08f, 0.12f);
        [SerializeField] private float _acePopSeconds = 0.4f;

        // 登場で少し大きくなってから戻る強さ（MonsterView の「ぽよん」と同じ式）
        private const float PopOvershoot = 1.7f;

        private Action _onNext;

        private void Awake()
        {
            _nextButton.onClick.AddListener(Next);
        }

        public void Show(SurvivalRun run, SurvivalOpponent opponent, Action onNext)
        {
            _onNext = onNext;
            bool isBoss = opponent.IsBoss;
            _back.color = isBoss ? _bossColor : _normalColor;
            _headline.text = isBoss ? $"ボスせん！ {run.BattleNumber}せんめ" : $"{run.BattleNumber}せんめ";
            _subText.text = $"いまの れんしょう: {run.Wins}";
            _bossName.gameObject.SetActive(isBoss);
            _bossLine.gameObject.SetActive(isBoss);
            _bossAce.gameObject.SetActive(isBoss);
            gameObject.SetActive(true);

            BattleSounds.Play(isBoss ? BattleSound.SendOut : BattleSound.Cursor);
            if (isBoss)
            {
                _bossName.text = opponent.Boss.Name;
                _bossLine.text = $"「{opponent.Boss.Line}」";
                _bossAce.sprite = MonsterSprites.Front(opponent.Boss.Team[0]);
                StartCoroutine(PopAce());
            }
        }

        private IEnumerator PopAce()
        {
            var rect = _bossAce.rectTransform;
            for (float elapsed = 0f; elapsed < _acePopSeconds; elapsed += Time.deltaTime)
            {
                float p = elapsed / _acePopSeconds - 1f;
                rect.localScale = Vector3.one * (1f + p * p * ((PopOvershoot + 1f) * p + PopOvershoot));
                yield return null;
            }
            rect.localScale = Vector3.one;
        }

        private void Next()
        {
            BattleSounds.Play(BattleSound.Cursor);
            StopAllCoroutines();
            gameObject.SetActive(false);
            _onNext?.Invoke();
        }
    }
}
