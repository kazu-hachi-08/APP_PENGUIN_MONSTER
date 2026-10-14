using System;
using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// ドラフト画面（仕様書 §2.1）。提示された3体のカードから1体を押して取る × 6回。
    /// 抽選は Draft に任せ、ここは表示と入力だけ。
    /// 演出: 提示のカードは1枚ずつ「ぽよん」と配り、取ったペンモンは下のマスへ飛んでいく
    /// </summary>
    public class DraftPanel : MonoBehaviour
    {
        [SerializeField] private Text _titleText;
        [SerializeField] private Button[] _cardButtons;
        [SerializeField] private Image[] _cardIcons;
        [SerializeField] private Text[] _cardNames;
        [SerializeField] private Text[] _cardStats;
        [Tooltip("種族値の見出しと数字をまとめた親。技を見ている間は隠す")]
        [SerializeField] private GameObject[] _cardStatGroups;
        [Tooltip("種族値と同じ場所に重ねた技の一覧。最初は隠れている")]
        [SerializeField] private Text[] _cardMoves;
        [Tooltip("全カードの 種族値 ⇄ 技 を切り替える。カードは押したらすぐ決定なので、カードの外に1つだけ置く")]
        [SerializeField] private Button _viewToggle;
        [SerializeField] private Text _viewToggleLabel;
        [Tooltip("特性と持ち物（名前と説明を1行ずつ）")]
        [SerializeField] private Text[] _cardTraits;
        /// <summary>1枚につき2つ（2タイプ目が無ければ隠す）。カード i のタイプ k は [i * 2 + k]</summary>
        [SerializeField] private Image[] _typeBadges;
        [SerializeField] private Text[] _typeLabels;
        [SerializeField] private Image[] _pickedIcons;
        [Tooltip("オンラインの1回ごとの制限時間。オフラインでは隠れたまま")]
        [SerializeField] private CountdownTimer _timer;
        [Tooltip("取ったペンモンがカードから下のマスへ飛ぶ絵。ほかの部品より手前に置く")]
        [SerializeField] private Image _flyer;
        [SerializeField] private float _dealInterval = 0.08f;
        [SerializeField] private float _flySeconds = 0.3f;

        private const int BadgesPerCard = TypeColors.BadgesPerMonster;
        private const int TraitDescriptionSize = 20;
        private const int MoveDetailSize = 22;
        private const string ShowMovesLabel = "技を 見る";
        private const string ShowStatsLabel = "種族値を 見る";
        private const string TitleFormat = "ドラフト {0}/{1}　欲しい ペンモンを 選んでください";

        private Draft _draft;
        private Action<IReadOnlyList<MonsterDefinition>> _onDone;
        private float _secondsPerPick;
        private PopIn[] _cardPops;
        private Coroutine _flying;
        // 次の提示や次のドラフトでも見たい方を見続けられるよう、Show で戻さない
        private bool _showingMoves;

        private void Awake()
        {
            _cardPops = new PopIn[_cardButtons.Length];
            for (int i = 0; i < _cardButtons.Length; i++)
            {
                int index = i;
                _cardButtons[i].onClick.AddListener(() => Pick(index));
                _cardPops[i] = _cardButtons[i].GetComponent<PopIn>();
            }
            _viewToggle.onClick.AddListener(ToggleView);
            ApplyView();
        }

        /// <param name="secondsPerPick">1回の制限時間。0 なら制限なし（オフライン）</param>
        public void Show(Draft draft, Action<IReadOnlyList<MonsterDefinition>> onDone, float secondsPerPick = 0f)
        {
            _draft = draft;
            _onDone = onDone;
            _secondsPerPick = secondsPerPick;
            _flyer.enabled = false;
            _flying = null;
            gameObject.SetActive(true);
            Refresh();
        }

        private void Pick(int index)
        {
            BattleSounds.Play(BattleSound.Cursor);
            // カードは Refresh で配り直して縮むので、飛び立つ位置はその前に覚える
            var from = _cardIcons[index].rectTransform;
            var fromPosition = from.position;
            var fromSize = from.rect.size;
            _draft.Pick(index);
            if (_draft.IsDone)
            {
                Hide();
                _onDone?.Invoke(_draft.Picked);
                return;
            }
            Refresh();
            FlyToSlot(fromPosition, fromSize, _draft.Picked.Count - 1);
        }

        private void Refresh()
        {
            _titleText.text = string.Format(TitleFormat, _draft.Picked.Count + 1, BattleBalance.DraftRounds);
            for (int i = 0; i < _cardButtons.Length; i++)
            {
                ShowCard(i, _draft.Offer[i]);
                _cardPops[i].Play(i * _dealInterval);
            }
            for (int i = 0; i < _pickedIcons.Length; i++)
            {
                bool picked = i < _draft.Picked.Count;
                _pickedIcons[i].enabled = picked;
                _pickedIcons[i].sprite = picked ? MonsterSprites.Front(_draft.Picked[i]) : null;
            }
            _timer.Begin(_secondsPerPick, PickRandom);
        }

        /// <summary>
        /// 取ったカードの絵の位置から下のマスへ飛ばし、着いたらマスに絵を出す。
        /// 続けてすぐ取ったときは前の飛行を打ち切る（そのマスは Refresh で表示済み）
        /// </summary>
        private void FlyToSlot(Vector3 fromPosition, Vector2 fromSize, int slot)
        {
            if (_flying != null) StopCoroutine(_flying);
            _flying = StartCoroutine(Fly(fromPosition, fromSize, _pickedIcons[slot]));
        }

        private IEnumerator Fly(Vector3 fromPosition, Vector2 fromSize, Image target)
        {
            target.enabled = false;
            _flyer.sprite = target.sprite;
            _flyer.enabled = true;
            var rect = _flyer.rectTransform;
            for (float elapsed = 0f; elapsed < _flySeconds; elapsed += Time.deltaTime)
            {
                // 速く飛び出して、マスの手前でゆっくり止まる
                float t = 1f - Mathf.Pow(1f - elapsed / _flySeconds, 2f);
                rect.position = Vector3.Lerp(fromPosition, target.rectTransform.position, t);
                rect.sizeDelta = Vector2.Lerp(fromSize, target.rectTransform.rect.size, t);
                yield return null;
            }
            _flyer.enabled = false;
            target.enabled = true;
            _flying = null;
        }

        private void ToggleView()
        {
            BattleSounds.Play(BattleSound.Cursor);
            _showingMoves = !_showingMoves;
            ApplyView();
        }

        private void ApplyView()
        {
            for (int i = 0; i < _cardButtons.Length; i++)
            {
                _cardStatGroups[i].SetActive(!_showingMoves);
                _cardMoves[i].gameObject.SetActive(_showingMoves);
            }
            _viewToggleLabel.text = _showingMoves ? ShowStatsLabel : ShowMovesLabel;
        }

        /// <summary>時間切れは3体からランダム（仕様書 §2.1）</summary>
        private void PickRandom()
        {
            Pick(UnityEngine.Random.Range(0, _draft.Offer.Count));
        }

        /// <summary>途中で相手が抜けたときなど、外から閉じる</summary>
        public void Hide()
        {
            _timer.Stop();
            gameObject.SetActive(false);
        }

        private void ShowCard(int card, MonsterDefinition monster)
        {
            _cardIcons[card].sprite = MonsterSprites.Front(monster);
            _cardNames[card].text = monster.Name;
            _cardStats[card].text = MonsterTexts.StatValues(monster.BaseStats);
            _cardTraits[card].text = MonsterTexts.Traits(monster, TraitDescriptionSize);
            _cardMoves[card].text = MonsterTexts.MoveList(monster, MoveDetailSize);
            TypeColors.PaintBadges(monster, _typeBadges, _typeLabels, card * BadgesPerCard);
        }
    }
}
