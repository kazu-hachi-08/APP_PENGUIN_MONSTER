using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 交代先を選ぶ画面。自分で交代するとき（もどれる）と、ひんしで出し直すとき（もどれない）に使う。
    /// 枠はダブルスの4体ぶん作ってあり、シングルでは余った枠を隠す。
    /// 枠を長押し（マウスは乗せる）すると、横に特性・持ち物の説明を出す（技ボタンと同じ決まり）
    /// </summary>
    public class PartyPanel : MonoBehaviour
    {
        private const int AbilityLineSize = 28;
        private const int HintDescriptionSize = 28;

        [SerializeField] private Button[] _slotButtons;
        [SerializeField] private Text[] _slotLabels;
        [SerializeField] private Image[] _slotIcons;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _hintBox;
        [SerializeField] private Text _hintText;

        private MoveHoldHint[] _hints;
        private BattleSide _side;
        private Action<int> _onSelect;
        private Action _onBack;

        private void Awake()
        {
            _hints = new MoveHoldHint[_slotButtons.Length];
            for (int i = 0; i < _slotButtons.Length; i++)
            {
                int index = i;
                _hints[i] = _slotButtons[i].GetComponent<MoveHoldHint>();
                _hints[i].Shown += () => ShowHint(index);
                _hints[i].Hidden += HideHint;
                _slotButtons[i].onClick.AddListener(() => OnSlotClicked(index));
            }
            _backButton.onClick.AddListener(Back);
        }

        /// <summary>
        /// ストーリーで道具を使う相手を選ぶ。場に出ている子も選べるので、全員の HP を出す
        /// </summary>
        /// <param name="canPick">道具が効く子</param>
        public void ShowItemTargets(BattleSide side, Func<int, bool> canPick, Action<int> onSelect, Action onBack)
        {
            Show(side, onSelect, onBack, canPick: canPick);
        }

        /// <summary>
        /// 交代先を選ぶ（自分で交代するときは もどれる、ひんしの入れ替えは もどれない）。
        /// canPick があれば道具を使う相手を選ぶ画面になる
        /// </summary>
        /// <param name="onBack">null ならもどれない（ひんしの入れ替え）</param>
        /// <param name="reserved">ダブルスで同じターンにもう1体が選んだ子（選べなくする）</param>
        /// <param name="canPick">道具を使う相手を選ぶときだけ。null なら交代先を選ぶ</param>
        public void Show(BattleSide side, Action<int> onSelect, Action onBack, ICollection<int> reserved = null,
            Func<int, bool> canPick = null)
        {
            _side = side;
            _onSelect = onSelect;
            _onBack = onBack;

            for (int i = 0; i < _slotButtons.Length; i++)
            {
                bool exists = i < side.Party.Count;
                _slotButtons[i].gameObject.SetActive(exists);
                if (!exists)
                {
                    continue;
                }
                var monster = side.Party[i];
                bool isReserved = reserved != null && reserved.Contains(i);
                bool showActive = canPick == null && side.IsActive(i);
                string state = monster.IsFainted ? "瀕死" : showActive ? "戦闘中" : isReserved ? "交代先" : $"HP {monster.CurrentHp}/{monster.MaxHp}";
                string status = monster.IsFainted ? "" : monster.Status.ToJapanese();
                _slotLabels[i].SetFitted($"{monster.Name}   {state}  {status}\n<size={AbilityLineSize}>特性 {monster.Definition.Ability.ToJapanese()}</size>");
                _slotIcons[i].sprite = MonsterSprites.Front(monster.Definition);
                _slotButtons[i].interactable = canPick?.Invoke(i) ?? (side.CanSwitchTo(i) && !isReserved);
            }
            _backButton.gameObject.SetActive(onBack != null);
            HideHint();
            gameObject.SetActive(true);
        }

        /// <summary>選べない子（戦闘中・瀕死）も説明は見られる。Button が押せなくても長押しの合図は届くため</summary>
        private void ShowHint(int index)
        {
            var monster = _side.Party[index];
            _hintBox.SetActive(true);
            _hintText.SetFitted($"<b>{monster.Name}</b>\n" + MonsterTexts.Traits(monster.Definition, HintDescriptionSize, itemUsed: monster.ItemUsed));
        }

        private void HideHint()
        {
            _hintBox.SetActive(false);
        }

        /// <summary>長押しで説明を見ただけのときは、指を離しても交代しない</summary>
        private void OnSlotClicked(int index)
        {
            if (_hints[index].WasLongPress)
            {
                return;
            }
            Select(index);
        }

        private void Select(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            HideHint();
            gameObject.SetActive(false);
            _onSelect?.Invoke(index);
        }

        private void Back()
        {
            BattleSounds.Play(BattleSound.Cursor);
            HideHint();
            gameObject.SetActive(false);
            _onBack?.Invoke();
        }
    }
}
