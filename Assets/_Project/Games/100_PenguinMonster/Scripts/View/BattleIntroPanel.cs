using System;
using System.Collections;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 勝ち抜き戦の毎試合の前に出す画面。何戦目かを見せ、ボスのときは
    /// 背景を変えて名前・セリフを出し、先頭のペンモンを「ぽよん」と登場させる。どこを押しても次へ。
    /// 演出: 「N戦目」を上から叩きつけ、着いた瞬間に画面を揺らしてから連勝数のバッジを出す
    /// </summary>
    public class BattleIntroPanel : MonoBehaviour
    {
        [SerializeField] private Image _back;
        [Tooltip("揺らす中身。背景は端まで塗ったまま動かさない")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private Text _headline;
        [SerializeField] private PopIn _headlinePop;
        [SerializeField] private Text _subText;
        [SerializeField] private PopIn _badgePop;
        [SerializeField] private Text _bossName;
        [SerializeField] private Text _bossLine;
        [SerializeField] private Image _bossAce;
        [SerializeField] private PopIn _acePop;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Color _bossColor = new Color(0.4f, 0.08f, 0.12f);
        [Tooltip("見出しが叩きつけられるまでの時間（PopIn の長さとそろえる）")]
        [SerializeField] private float _slamSeconds = 0.35f;
        [SerializeField] private float _shakeSeconds = 0.25f;
        [SerializeField] private float _shakeWidth = 18f;
        [SerializeField] private float _aceDelay = 0.15f;

        private Action _onNext;

        private void Awake()
        {
            _nextButton.onClick.AddListener(Next);
        }

        public void Show(SurvivalRun run, SurvivalOpponent opponent, Action onNext)
        {
            _onNext = onNext;
            bool isBoss = opponent.IsBoss;
            // ボスは夜空をやめて真っ赤にし、いつもの試合と一目で違うと分かるようにする
            _back.sprite = isBoss ? null : NightSky.Sprite;
            _back.color = isBoss ? _bossColor : Color.white;
            _headline.text = isBoss ? $"ボス戦！ {run.BattleNumber}戦目" : $"{run.BattleNumber}戦目";
            _subText.text = $"{run.Format.ToJapanese()}　今の 連勝: {run.Wins}";
            _bossName.gameObject.SetActive(isBoss);
            _bossLine.gameObject.SetActive(isBoss);
            _bossAce.gameObject.SetActive(isBoss);
            if (isBoss)
            {
                _bossName.text = opponent.Boss.Name;
                _bossLine.text = $"「{opponent.Boss.Line}」";
                _bossAce.sprite = MonsterSprites.Front(opponent.Boss.TrumpCard);
            }
            gameObject.SetActive(true);

            _headlinePop.Play();
            _badgePop.Play(_slamSeconds);
            if (isBoss) _acePop.Play(_slamSeconds + _aceDelay);
            StartCoroutine(Land(isBoss));
        }

        /// <summary>見出しが着いたら、重さが伝わるよう音と一緒に中身を揺らす</summary>
        private IEnumerator Land(bool isBoss)
        {
            yield return new WaitForSeconds(_slamSeconds);
            BattleSounds.Play(isBoss ? BattleSound.HitStrong : BattleSound.Hit);
            for (float elapsed = 0f; elapsed < _shakeSeconds; elapsed += Time.deltaTime)
            {
                float width = _shakeWidth * (1f - elapsed / _shakeSeconds);
                _content.anchoredPosition = new Vector2(UnityEngine.Random.Range(-width, width), UnityEngine.Random.Range(-width, width));
                yield return null;
            }
            _content.anchoredPosition = Vector2.zero;
        }

        private void Next()
        {
            BattleSounds.Play(BattleSound.Cursor);
            StopAllCoroutines();
            _content.anchoredPosition = Vector2.zero;
            gameObject.SetActive(false);
            _onNext?.Invoke();
        }
    }
}
