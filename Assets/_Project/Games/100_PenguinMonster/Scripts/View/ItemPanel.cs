using System;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 持ち物の画面（仕様書 §2.1.1）。ドラフトの後にもらった持ち物を見せ、下の6体から持たせる1体を押す。
    /// 押したらすぐ決定（ドラフトと同じ手触り）。持たせる処理は ItemDraw に任せ、ここは表示と入力だけ
    /// </summary>
    public class ItemPanel : MonoBehaviour
    {
        [SerializeField] private Text _itemName;
        [SerializeField] private Text _itemDescription;
        [SerializeField] private PopIn _itemPop;
        [SerializeField] private Button[] _cellButtons;
        [SerializeField] private Image[] _cellIcons;
        [SerializeField] private Text[] _cellNames;
        [Tooltip("オンラインの制限時間。オフラインでは隠れたまま")]
        [SerializeField] private CountdownTimer _timer;

        private IReadOnlyList<MonsterDefinition> _six;
        private HeldItem _item;
        private Action<List<MonsterDefinition>> _onDone;

        private void Awake()
        {
            for (int i = 0; i < _cellButtons.Length; i++)
            {
                int index = i;
                _cellButtons[i].onClick.AddListener(() => Give(index));
            }
        }

        /// <param name="seconds">制限時間。0 なら制限なし（オフライン）</param>
        public void Show(IReadOnlyList<MonsterDefinition> six, HeldItem item, Action<List<MonsterDefinition>> onDone, float seconds = 0f)
        {
            _six = six;
            _item = item;
            _onDone = onDone;
            _itemName.text = item.ToJapanese();
            _itemDescription.text = item.Description();
            for (int i = 0; i < _cellButtons.Length; i++)
            {
                _cellIcons[i].sprite = MonsterSprites.Front(six[i]);
                _cellNames[i].SetFitted(six[i].Name);
            }
            gameObject.SetActive(true);
            _itemPop.Play();
            _timer.Begin(seconds, GiveRandom);
        }

        /// <summary>途中で相手が抜けたときなど、外から閉じる</summary>
        public void Hide()
        {
            _timer.Stop();
            gameObject.SetActive(false);
        }

        private void Give(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onDone?.Invoke(ItemDraw.Give(_six, index, _item));
        }

        /// <summary>時間切れは6体からランダムな1体に持たせる（仕様書 §2.1.1）</summary>
        private void GiveRandom()
        {
            Give(UnityEngine.Random.Range(0, _six.Count));
        }
    }
}
