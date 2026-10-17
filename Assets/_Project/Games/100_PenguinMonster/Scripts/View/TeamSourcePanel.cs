using System;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// オンラインでホストが「ドラフト / 自分のチーム」を選ぶ画面。シングル / ダブルスの次に出す。
    /// 自分のチームは自分と相手の両方の手持ちが使えるときだけ決まり、使えなければ下に理由を出して選び直してもらう
    /// </summary>
    public class TeamSourcePanel : MonoBehaviour
    {
        [SerializeField] private Button _draftButton;
        [SerializeField] private Button _ownTeamButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Text _notice;
        [SerializeField] private Image _draftIcon;
        [SerializeField] private Image _ownTeamIcon;

        private Action<TeamSource> _onChosen;
        private Action _onBack;
        private string _ownTeamProblem;

        private void Awake()
        {
            _draftButton.onClick.AddListener(() => Choose(TeamSource.Draft));
            _ownTeamButton.onClick.AddListener(ChooseOwnTeam);
            _backButton.onClick.AddListener(Back);
        }

        /// <param name="ownTeamProblem">自分のチームを選べない理由。選べるなら null</param>
        /// <param name="ownTeamLeader">自分のチームのボタンに乗せる絵（手持ちの先頭）。無ければドラフトの子から</param>
        public void Show(string ownTeamProblem, MonsterDefinition ownTeamLeader, Action<TeamSource> onChosen, Action onBack)
        {
            _ownTeamProblem = ownTeamProblem;
            _onChosen = onChosen;
            _onBack = onBack;
            _notice.text = string.Empty;
            var all = MonsterTable.All;
            _draftIcon.sprite = MonsterSprites.Front(all[UnityEngine.Random.Range(0, all.Count)]);
            _ownTeamIcon.sprite = MonsterSprites.Front(ownTeamLeader ?? all[UnityEngine.Random.Range(0, all.Count)]);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ChooseOwnTeam()
        {
            if (_ownTeamProblem != null)
            {
                BattleSounds.Play(BattleSound.Cursor);
                _notice.text = _ownTeamProblem;
                return;
            }
            Choose(TeamSource.OwnTeam);
        }

        private void Choose(TeamSource source)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onChosen?.Invoke(source);
        }

        private void Back()
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onBack?.Invoke();
        }
    }
}
