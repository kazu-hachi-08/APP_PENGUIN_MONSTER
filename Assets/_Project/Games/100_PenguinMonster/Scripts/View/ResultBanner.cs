using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 結果の画面の演出（仕様書 §7）。文面とボタンは共通基盤の ResultDialog が持ち、ここは勝ち負けで変わる見た目だけ:
    /// 見出しの色・「ぽよん」・勝ったときの紙吹雪・連勝数のバッジ（オンラインは連勝数が無いので隠す）
    /// </summary>
    public class ResultBanner : MonoBehaviour
    {
        [SerializeField] private Text _title;
        [SerializeField] private PopIn _titlePop;
        [SerializeField] private Text _scoreText;
        [SerializeField] private GameObject _scoreBadge;
        [SerializeField] private PopIn _scorePop;
        [SerializeField] private GameObject _confetti;
        [SerializeField] private Color _winColor = new Color(1f, 0.85f, 0.3f);
        [SerializeField] private Color _loseColor = new Color(0.6f, 0.72f, 0.9f);
        [SerializeField] private float _scoreDelay = 0.25f;

        /// <summary>ResultDialog.Show で文面を入れた後に呼ぶ（バッジを出すかは入った文面で決める）</summary>
        public void Play(bool won)
        {
            _title.color = won ? _winColor : _loseColor;
            _confetti.SetActive(won);
            _titlePop.Play();

            bool hasScore = !string.IsNullOrEmpty(_scoreText.text);
            _scoreBadge.SetActive(hasScore);
            if (hasScore) _scorePop.Play(_scoreDelay);
        }
    }
}
