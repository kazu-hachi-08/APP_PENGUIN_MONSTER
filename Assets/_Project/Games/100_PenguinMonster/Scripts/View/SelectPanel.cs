using System;
using System.Collections.Generic;
using System.Linq;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 見せ合い・選出の画面（仕様書 §2.2）。上に相手の6体を見せ、下の自分の6体から3体（ダブルスは4体）を選ぶ。
    /// 選んだ順が出す順（1番目が先頭）。もう一度押すと外せる
    /// </summary>
    public class SelectPanel : MonoBehaviour
    {
        [SerializeField] private Text _titleText;
        [SerializeField] private Button[] _cellButtons;
        [SerializeField] private Image[] _cellIcons;
        [SerializeField] private Text[] _cellLabels;
        [SerializeField] private Text[] _cellOrders;
        [SerializeField] private Image[] _opponentIcons;
        [SerializeField] private Text[] _opponentLabels;
        [SerializeField] private Button _randomButton;
        [SerializeField] private Button _decideButton;
        [Tooltip("オンラインの制限時間。オフラインでは隠れたまま")]
        [SerializeField] private CountdownTimer _timer;
        [SerializeField] private Color _cellColor = new Color(0.3f, 0.5f, 0.8f);
        [SerializeField] private Color _pickedColor = new Color(0.95f, 0.6f, 0.2f);

        private const string Title = "バトルに 出す ペンモンを 選んでください";

        private readonly List<int> _picked = new List<int>();
        private IReadOnlyList<MonsterDefinition> _monsters;
        private int _partySize = BattleBalance.PartySize;
        private Action<List<MonsterDefinition>> _onDecide;

        private void Awake()
        {
            for (int i = 0; i < _cellButtons.Length; i++)
            {
                int index = i;
                _cellButtons[i].onClick.AddListener(() => Toggle(index));
            }
            _randomButton.onClick.AddListener(PickRandom);
            _decideButton.onClick.AddListener(Decide);
        }

        /// <param name="partySize">選ぶ数（シングル 3・ダブルス 4）</param>
        /// <param name="timeLimit">制限時間。0 なら制限なし（オフライン）</param>
        public void Show(IReadOnlyList<MonsterDefinition> own, IReadOnlyList<MonsterDefinition> opponent, int partySize,
            Action<List<MonsterDefinition>> onDecide, float timeLimit = 0f)
        {
            _monsters = own;
            _partySize = partySize;
            _onDecide = onDecide;
            _picked.Clear();

            for (int i = 0; i < _cellButtons.Length; i++)
            {
                _cellIcons[i].sprite = MonsterSprites.Front(own[i]);
                _cellLabels[i].text = Label(own[i]);
            }
            for (int i = 0; i < _opponentIcons.Length; i++)
            {
                _opponentIcons[i].sprite = MonsterSprites.Front(opponent[i]);
                _opponentLabels[i].text = Label(opponent[i]);
            }
            Refresh();
            gameObject.SetActive(true);
            _timer.Begin(timeLimit, DecideByTimeout);
        }

        /// <summary>途中で相手が抜けたときなど、外から閉じる</summary>
        public void Hide()
        {
            _timer.Stop();
            gameObject.SetActive(false);
        }

        /// <summary>時間切れ。選びかけのぶんは残し、足りないぶんを上から埋めて決める（仕様書 §2.2）</summary>
        private void DecideByTimeout()
        {
            for (int i = 0; i < _monsters.Count && _picked.Count < _partySize; i++)
            {
                if (!_picked.Contains(i))
                {
                    _picked.Add(i);
                }
            }
            Decide();
        }

        private void Toggle(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            if (!_picked.Remove(index) && _picked.Count < _partySize)
            {
                _picked.Add(index);
            }
            Refresh();
        }

        /// <summary>足りないぶんをランダムで埋める（迷ったとき用）</summary>
        private void PickRandom()
        {
            BattleSounds.Play(BattleSound.Cursor);
            var rest = Enumerable.Range(0, _monsters.Count).Except(_picked).OrderBy(_ => UnityEngine.Random.value).ToList();
            _picked.AddRange(rest.Take(_partySize - _picked.Count));
            Refresh();
        }

        private void Decide()
        {
            if (_picked.Count != _partySize)
            {
                return;
            }
            BattleSounds.Play(BattleSound.Cursor);
            Hide();
            _onDecide?.Invoke(_picked.Select(i => _monsters[i]).ToList());
        }

        private void Refresh()
        {
            _titleText.text = $"{Title}（{_picked.Count}/{_partySize}）";
            for (int i = 0; i < _cellButtons.Length; i++)
            {
                int order = _picked.IndexOf(i);
                _cellButtons[i].image.color = order >= 0 ? _pickedColor : _cellColor;
                _cellOrders[i].text = order >= 0 ? (order + 1).ToString() : "";
            }
            _decideButton.interactable = _picked.Count == _partySize;
        }

        private static string Label(MonsterDefinition monster)
        {
            string types = string.Join("/", monster.Types.Select(t => t.ToJapanese()));
            return $"{monster.Name}\n<size=22>{types}</size>";
        }
    }
}
