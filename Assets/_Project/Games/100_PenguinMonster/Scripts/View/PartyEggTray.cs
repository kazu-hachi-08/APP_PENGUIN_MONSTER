using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 選出のペンモンをタマゴの列で見せ、残りが何体か分かるようにする（仕様書 §7）。選出の順に左から並べ、瀕死は暗くし、状態異常は色の点を付ける。
    /// 状態から作り直さずイベントで塗り替える。ホストは1ターンぶんの計算を先に済ませるので、状態を見ると再生より先に瀕死が見えてしまうため
    /// </summary>
    public class PartyEggTray : MonoBehaviour
    {
        [Tooltip("選出の数のうち多い方（ダブルスの4）だけ用意し、使わないぶんは隠す")]
        [SerializeField] private Image[] _eggs;
        [Tooltip("タマゴごとの状態異常の点")]
        [SerializeField] private Image[] _statusDots;
        [Tooltip("シングル / ダブルスで名前の枠の位置が違うので、置き場も分ける")]
        [SerializeField] private Vector2 _singlePosition;
        [SerializeField] private Vector2 _doublePosition;
        [SerializeField] private Color _faintedTint = new Color(0.3f, 0.3f, 0.35f, 0.8f);

        public void Show(BattleSide side, EggKind kind)
        {
            ((RectTransform)transform).anchoredPosition = side.ActiveCount > 1 ? _doublePosition : _singlePosition;
            var sprite = EggSprites.Get(kind);
            for (int i = 0; i < _eggs.Length; i++)
            {
                _eggs[i].gameObject.SetActive(i < side.Party.Count);
                _eggs[i].sprite = sprite;
                _eggs[i].color = Color.white;
                _statusDots[i].gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
        }

        public void SetStatus(int partyIndex, StatusCondition status)
        {
            if (!Has(partyIndex)) return;
            bool has = status != StatusCondition.None;
            _statusDots[partyIndex].gameObject.SetActive(has);
            if (has)
            {
                _statusDots[partyIndex].color = StatusColors.Get(status);
            }
        }

        public void SetFainted(int partyIndex)
        {
            if (!Has(partyIndex)) return;
            _eggs[partyIndex].color = _faintedTint;
            _statusDots[partyIndex].gameObject.SetActive(false);
        }

        public void Hide() => gameObject.SetActive(false);

        private bool Has(int partyIndex) => partyIndex >= 0 && partyIndex < _eggs.Length;
    }
}
