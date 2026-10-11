using System;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// タイトル画面（仕様書 §2）。メニューは全部並べておき、まだ作っていないものは押せなくしておく
    /// </summary>
    public class TitlePanel : MonoBehaviour
    {
        [SerializeField] private Button _survivalButton;
        [SerializeField] private Button _onlineButton;
        [SerializeField] private Button[] _comingSoonButtons;
        [SerializeField] private Image[] _mascots;
        [SerializeField] private Text _bestText;

        private Action _onSurvival;
        private Action _onOnline;

        private void Awake()
        {
            _survivalButton.onClick.AddListener(() => Choose(_onSurvival));
            _onlineButton.onClick.AddListener(() => Choose(_onOnline));
            foreach (var button in _comingSoonButtons)
            {
                button.interactable = false;
            }
        }

        public void Show(int bestWins, Action onSurvival, Action onOnline)
        {
            _onSurvival = onSurvival;
            _onOnline = onOnline;
            _bestText.text = $"勝ち抜き戦 さいこう きろく: {bestWins} れんしょう";
            // 飾りのペンモンは定義表の先頭から順に並べる（絵は実行時に作るのでシーンには焼かない）
            for (int i = 0; i < _mascots.Length; i++)
            {
                _mascots[i].sprite = MonsterSprites.Front(MonsterTable.All[i]);
            }
            gameObject.SetActive(true);
        }

        private void Choose(Action onChosen)
        {
            BattleSounds.Play(BattleSound.Cursor);
            gameObject.SetActive(false);
            onChosen?.Invoke();
        }
    }
}
