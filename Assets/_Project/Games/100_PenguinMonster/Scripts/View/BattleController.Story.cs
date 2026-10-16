using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Story;
using PenguinMonster.View.Art;
using PenguinMonster.View.Story;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// ストーリーの1戦（PLAN_STORY_TOWN01.md フェーズ5）。手持ち全員 vs 相手全員のシングルで、見せ合い・選出はない。
    /// コマンドは「戦う / バッグ / ペンモン / 逃げる」。逃げられるのは野生だけで、失敗したら相手だけが動く（バッグは .Bag.cs）。
    /// 経験値・賞金など戦いの後のことは、呼ぶ側（StoryBattleScreen）が受け持つ
    /// </summary>
    public partial class BattleController
    {
        private const string FledMessage = "うまく 逃げ切れた！";
        private const string FleeFailedMessage = "しかし 逃げられなかった！";
        private const string ChallengeFormat = "{0}が 勝負を しかけてきた！";
        private const string ForgetPrompt = "どの 技を 忘れさせますか？\n（戻る なら 覚えない）";
        private const string EvolvingFormat = "おや？ {0}の 様子が……";
        private const string EvolvedFormat = "おめでとう！ {0}は {1}に 進化した！";
        // 進化の演出: 前と後の絵を、だんだん速く入れ替える
        private const int EvolutionSwaps = 14;
        private const float EvolutionFirstInterval = 0.4f;
        private const float EvolutionIntervalRate = 0.82f;

        [Header("ストーリー")]
        [Tooltip("レベルが上がったときの能力の窓")]
        [SerializeField] private LevelUpWindow _levelUpWindow;

        /// <summary>ストーリーの1戦の間だけ入る。null なら対戦（勝ち抜き戦・オンライン）</summary>
        private StoryBattle _story;
        private bool _fleeChosen;
        private System.Random _random;

        private bool FoeIsWild => _story != null && !_story.IsTrainer;

        /// <summary>野生から逃げ切って終わった</summary>
        public bool Fled { get; private set; }

        /// <summary>
        /// ストーリーの1戦を決着（か逃げ切る・捕まえる）まで進める。HP・状態異常はまだ手持ちに戻さない（呼ぶ側が StoryBattle.ApplyResults）
        /// </summary>
        /// <param name="bag">バトル中に使う道具。使ったぶんはその場で減る</param>
        /// <param name="trainerName">トレーナー戦なら名前（「〇〇が 勝負を しかけてきた！」）。野生は null</param>
        public IEnumerator RunStory(StoryBattle battle, Bag bag, CpuPlayer cpu, System.Random random, string trainerName)
        {
            ClearField();
            _story = battle;
            _engine = new BattleEngine(battle.State, random);
            _state = battle.State;
            _choiceSeconds = 0f;
            _cpu = cpu;
            _random = random;
            _bag = bag;
            _bagItem = null;
            Fled = false;
            _fleeChosen = false;

            if (trainerName != null)
            {
                yield return _messageWindow.Play(string.Format(ChallengeFormat, trainerName));
            }
            yield return Play(FoeFirst(_engine.StartBattle()));
            while (!_state.IsOver)
            {
                yield return WaitForPlayerActions();
                if (_fleeChosen)
                {
                    yield return TryFlee();
                    if (Fled)
                    {
                        break;
                    }
                }
                if (_bagItem.HasValue)
                {
                    yield return UseBagItem();
                    if (battle.Caught != null)
                    {
                        break;
                    }
                }
                yield return Play(_engine.ResolveTurn(_playerActions, _cpu.ChooseActions(Opponent, Player)));
                battle.RecordFacing();
                if (!_state.IsOver)
                {
                    yield return ReplaceFainted();
                    battle.RecordFacing();
                }
            }
            HideChoices();
        }

        /// <summary>戦いの後の文（経験値・レベルアップなど）を同じ窓に出す</summary>
        public IEnumerator Say(string message)
        {
            yield return _messageWindow.Play(message);
        }

        /// <summary>「レベル N に 上がった！」と上がった分を見せ、次のタップで新しい値、もう1回で閉じる</summary>
        public IEnumerator ShowLevelUp(string message, Stats before, Stats after)
        {
            _levelUpWindow.ShowGains(before, after);
            yield return _messageWindow.Play(message);
            _levelUpWindow.ShowTotals(after);
            yield return _messageWindow.WaitForTap();
            _levelUpWindow.Hide();
        }

        /// <summary>技の窓で忘れる技を選ばせる。戻るなら -1</summary>
        public IEnumerator ChooseMoveToForget(BattleMonster member, System.Action<int> onChosen)
        {
            const int waiting = -2;
            int chosen = waiting;
            _messageWindow.SetText(ForgetPrompt);
            _movePanel.Show(member, new List<BattleMonster>(), onMove: index => chosen = index, onBack: () => chosen = -1);
            yield return new WaitUntil(() => chosen != waiting);
            onChosen(chosen);
        }

        /// <summary>
        /// 進化の演出。場の手前の子をしまい、奥（正面の絵が見える側）で前と後の絵をだんだん速く入れ替えてから、後の絵で止める
        /// </summary>
        public IEnumerator PlayEvolution(string fromName, MonsterDefinition from, MonsterDefinition into)
        {
            _playerView.Hide();
            _playerInfo.Hide();
            _enemyView.Hide();
            _enemyInfo.Hide();
            yield return _messageWindow.Play(string.Format(EvolvingFormat, fromName));
            var before = MonsterSprites.Front(from);
            var after = MonsterSprites.Front(into);
            yield return _enemyView.Appear(before);
            float interval = EvolutionFirstInterval;
            for (int i = 0; i < EvolutionSwaps; i++)
            {
                _enemyView.SetSprite(i % 2 == 0 ? after : before);
                yield return new WaitForSeconds(interval);
                interval *= EvolutionIntervalRate;
            }
            _enemyView.SetSprite(after);
            BattleSounds.Play(BattleSound.Win);
            yield return _enemyView.Flash(StatusColors.StatUp);
            yield return _messageWindow.Play(string.Format(EvolvedFormat, fromName, into.Name));
            _enemyView.Hide();
        }

        /// <summary>本家と同じく相手が先に出る（野生が飛び出す・トレーナーが繰り出す → 自分が繰り出す）。特性はその後のまま</summary>
        private static List<BattleEvent> FoeFirst(List<BattleEvent> start)
        {
            int foe = start.FindIndex(e => e.Kind == BattleEventKind.SentOut && e.Side == OpponentSide);
            if (foe > 0)
            {
                var sentOut = start[foe];
                start.RemoveAt(foe);
                start.Insert(0, sentOut);
            }
            return start;
        }

        private IEnumerator TryFlee()
        {
            _fleeChosen = false;
            if (_story.TryFlee(_random))
            {
                Fled = true;
                BattleSounds.Play(BattleSound.Cursor);
                yield return _messageWindow.Play(FledMessage);
                yield break;
            }
            yield return _messageWindow.Play(FleeFailedMessage);
        }

        private void ShowStoryCommands(int slot, List<int> reserved)
        {
            System.Action onRun = null;
            if (FoeIsWild)
            {
                // 逃げるのはターンの行動の代わり。失敗したら何もしないで相手だけが動く
                onRun = () =>
                {
                    _fleeChosen = true;
                    _pendingAction = BattleAction.Skip();
                };
            }
            _storyCommandPanel.Show(Player, reserved,
                onFight: () => ShowMoveSelect(slot, reserved),
                onParty: () => ShowSwitchSelect(slot, reserved),
                onRun: onRun,
                onBag: () => ShowBag(slot, reserved));
        }
    }
}
