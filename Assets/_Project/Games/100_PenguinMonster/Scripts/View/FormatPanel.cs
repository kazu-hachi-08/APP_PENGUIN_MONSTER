using System;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// シングルかダブルスかを選ぶ画面。勝ち抜き戦はドラフトの前、オンラインはホストだけが接続の後に選ぶ。
    /// ボタンには場に出る数（1体 / 2体）だけペンモンの絵を乗せ、開くたびに顔ぶれを変える
    /// </summary>
    public class FormatPanel : MonoBehaviour
    {
        [SerializeField] private Button _singleButton;
        [SerializeField] private Button _doubleButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Image[] _icons;

        private Action<BattleFormat> _onChosen;
        private Action _onBack;

        private void Awake()
        {
            _singleButton.onClick.AddListener(() => Choose(BattleFormat.Single));
            _doubleButton.onClick.AddListener(() => Choose(BattleFormat.Double));
            _backButton.onClick.AddListener(Back);
        }

        public void Show(Action<BattleFormat> onChosen, Action onBack)
        {
            _onChosen = onChosen;
            _onBack = onBack;
            var all = MonsterTable.All;
            foreach (var icon in _icons)
            {
                icon.sprite = MonsterSprites.Front(all[UnityEngine.Random.Range(0, all.Count)]);
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Choose(BattleFormat format)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onChosen?.Invoke(format);
        }

        private void Back()
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onBack?.Invoke();
        }
    }
}
