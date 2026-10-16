using System.Collections;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドのメニュー（PLAN_STORY_TOWN01.md フェーズ6）。B で開き、手持ち / バッグ / 図鑑 / 記録する / やめる / 閉じる。
    /// 記録するはセーブ（フェーズ7）まで押せない。やめるは確かめてからタイトルへ戻る。B か「閉じる」で歩くのに戻る
    /// </summary>
    public class FieldMenu : MonoBehaviour
    {
        private const string QuitQuestion = "タイトルに 戻りますか？\n記録して いない 進み具合は 消えます";

        private enum Choice
        {
            None,
            Party,
            Bag,
            Dex,
            Quit,
            Close,
        }

        [SerializeField] private Button _partyButton;
        [SerializeField] private Button _bagButton;
        [SerializeField] private Button _dexButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _closeButton;
        [Tooltip("項目を並べた窓。手持ち・バッグなどを開いている間は隠す")]
        [SerializeField] private GameObject _list;
        [SerializeField] private PartyScreen _party;
        [SerializeField] private BagPanel _bag;
        [Tooltip("タイトルの図鑑をそのまま開く（見た / 捕まえた はフェーズ8）")]
        [SerializeField] private DexPanel _dex;
        [Tooltip("やめるの「はい / いいえ」を会話窓で聞く")]
        [SerializeField] private FieldEventPlayer _events;
        [SerializeField] private FieldInput _input;

        private Choice _choice;

        /// <summary>「やめる」で はい と答えた（呼ぶ側がタイトルへ戻る）</summary>
        public bool QuitRequested { get; private set; }

        private void Awake()
        {
            _partyButton.onClick.AddListener(() => Choose(Choice.Party));
            _bagButton.onClick.AddListener(() => Choose(Choice.Bag));
            _dexButton.onClick.AddListener(() => Choose(Choice.Dex));
            _quitButton.onClick.AddListener(() => Choose(Choice.Quit));
            _closeButton.onClick.AddListener(() => Choose(Choice.Close));
            // セーブはフェーズ7で足す
            _saveButton.interactable = false;
        }

        /// <summary>閉じるか、やめるで はい と答えるまで開いておく</summary>
        public IEnumerator Open(StoryState state)
        {
            QuitRequested = false;
            _input.SetPadVisible(false);
            gameObject.SetActive(true);
            bool open = true;
            while (open)
            {
                yield return WaitForChoice();
                switch (_choice)
                {
                    case Choice.Party:
                        yield return _party.Open(state);
                        break;
                    case Choice.Bag:
                        yield return OpenBag(state);
                        break;
                    case Choice.Dex:
                        yield return OpenDex();
                        break;
                    case Choice.Quit:
                        yield return _events.Confirm(QuitQuestion, yes => QuitRequested = yes);
                        open = !QuitRequested;
                        break;
                    default:
                        open = false;
                        break;
                }
            }
            gameObject.SetActive(false);
            _input.SetPadVisible(true);
        }

        private IEnumerator WaitForChoice()
        {
            _choice = Choice.None;
            _list.SetActive(true);
            // メニューを開いた B（や前の画面を閉じた B）で、すぐ閉じないよう1フレーム空ける
            yield return null;
            while (_choice == Choice.None)
            {
                if (_input.IsBDown)
                {
                    Choose(Choice.Close);
                }
                yield return null;
            }
            _list.SetActive(false);
        }

        private void Choose(Choice choice)
        {
            BattleSounds.Play(BattleSound.Cursor);
            _choice = choice;
        }

        /// <summary>フィールドで使えるのは回復の道具だけ。選んだら手持ちから使う子を選ぶ</summary>
        private IEnumerator OpenBag(StoryState state)
        {
            StoryItem? chosen = null;
            bool back = false;
            _bag.Show(state.Bag, item => StoryItems.Get(item).IsMedicine, item => chosen = item, () => back = true);
            yield return new WaitUntil(() => chosen.HasValue || back);
            if (chosen.HasValue)
            {
                yield return _party.Open(state, chosen.Value);
            }
        }

        private IEnumerator OpenDex()
        {
            bool closed = false;
            _dex.Show(() => closed = true);
            yield return new WaitUntil(() => closed);
        }
    }
}
