using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.Story;
using PenguinMonster.View.Story;
using UnityEngine;

namespace PenguinMonster.View
{
    /// <summary>
    /// ストーリーのバトル中のバッグ（PLAN_STORY_TOWN01.md フェーズ6）。回復の道具を手持ちの子に使う / 野生に捕獲タマゴを投げる。
    /// どちらもターンの行動の代わりで、使い終えたら相手だけが動く（逃げるのと同じ形）。
    /// トレーナー戦のタマゴは投げずに断り、コマンドへ戻す（タマゴもターンも減らない）
    /// </summary>
    public partial class BattleController
    {
        private const string TrainerEggMessage = "人の ペンモンは 捕まえられない！";
        private const string UsedItemFormat = "{0}に {1}を 使った！";
        private const string HealedFormat = "{0}の HPが {1} 回復した！";
        private const string CuredFormat = "{0}の {1}が 治った！";
        private const string ThrewEggFormat = "{0}を 投げた！";
        private const string CaughtFormat = "やった！ {0}を 捕まえた！";
        // 揺れた回数ごとの「出てきてしまった」の文（本家と同じく、惜しいほど悔しがる）
        private static readonly string[] BrokeOutMessages =
        {
            "ああっ！ 出てきて しまった！",
            "ああ！ 捕まえたと 思ったのに！",
            "ざんねん！ もう少しで 捕まえられたのに！",
            "ああっ！ おしい！ もう 少しだったのに！",
        };

        [Tooltip("ストーリーのバッグの一覧")]
        [SerializeField] private BagPanel _bagPanel;

        private Bag _bag;
        /// <summary>このターンに使うと決めた道具。null なら道具は使わない</summary>
        private StoryItem? _bagItem;
        /// <summary>回復の道具を使う手持ちの番号</summary>
        private int _bagTarget;

        private void ShowBag(int slot, List<int> reserved)
        {
            _bagPanel.Show(_bag, CanUseInBattle,
                onSelect: item => ChooseBagItem(slot, reserved, item),
                onBack: () => ShowCommands(slot, reserved));
        }

        /// <summary>タマゴはいつも押せる（トレーナー戦では断りの文を出す）。回復の道具は効く子がいるときだけ</summary>
        private bool CanUseInBattle(StoryItem item)
        {
            if (StoryItems.Get(item).IsCaptureEgg)
            {
                return true;
            }
            for (int i = 0; i < Player.Party.Count; i++)
            {
                if (_story.CanTreat(item, i))
                {
                    return true;
                }
            }
            return false;
        }

        private void ChooseBagItem(int slot, List<int> reserved, StoryItem item)
        {
            if (!StoryItems.Get(item).IsCaptureEgg)
            {
                _partyPanel.ShowItemTargets(Player, index => _story.CanTreat(item, index),
                    onSelect: index => DecideBagItem(item, index),
                    onBack: () => ShowBag(slot, reserved));
                return;
            }
            if (!_story.CanThrowEgg)
            {
                StartCoroutine(RefuseEgg(slot, reserved));
                return;
            }
            DecideBagItem(item, -1);
        }

        private IEnumerator RefuseEgg(int slot, List<int> reserved)
        {
            yield return _messageWindow.Play(TrainerEggMessage);
            ShowCommands(slot, reserved);
        }

        private void DecideBagItem(StoryItem item, int target)
        {
            _bagItem = item;
            _bagTarget = target;
            _pendingAction = BattleAction.Skip();
        }

        /// <summary>選んだ道具を使う。捕まえたら battle.Caught が入り、呼ぶ側がバトルを終える</summary>
        private IEnumerator UseBagItem()
        {
            var item = _bagItem.Value;
            _bagItem = null;
            if (StoryItems.Get(item).IsCaptureEgg)
            {
                yield return ThrowCaptureEgg();
            }
            else
            {
                yield return UseMedicine(item, _bagTarget);
            }
        }

        private IEnumerator UseMedicine(StoryItem item, int index)
        {
            var target = Player.Party[index];
            var medicine = StoryItems.Get(item);
            int hpBefore = target.CurrentHp;
            var statusBefore = target.Status;
            _story.UseMedicine(_bag, item, index);

            yield return _messageWindow.Play(string.Format(UsedItemFormat, target.Name, medicine.Name));
            bool onField = Player.IsActive(index);
            if (target.CurrentHp > hpBefore)
            {
                BattleSounds.Play(BattleSound.StatUp);
                if (onField)
                {
                    yield return _playerInfo.AnimateHp(target.CurrentHp);
                }
                yield return _messageWindow.Play(string.Format(HealedFormat, target.Name, target.CurrentHp - hpBefore));
            }
            if (target.Status != statusBefore)
            {
                if (onField)
                {
                    _playerInfo.SetStatus(target.Status);
                }
                yield return _messageWindow.Play(string.Format(CuredFormat, target.Name, statusBefore.ToJapanese()));
            }
        }

        /// <summary>
        /// タマゴを投げる → 光って野生が吸い込まれる → 揺れる（0〜3回）→ 捕まえる / 割れて出てくる
        /// </summary>
        private IEnumerator ThrowCaptureEgg()
        {
            var egg = _eggs[PlayerSide];
            var wild = Opponent.Active;
            yield return _messageWindow.Play(string.Format(ThrewEggFormat, StoryItems.Get(StoryItem.CaptureEgg).Name));
            var result = _story.ThrowEgg(_bag, _random);

            yield return _eggThrow.ThrowToCapture(egg, _enemyView.Body, onLanded: _enemyView.Hide);
            for (int i = 0; i < result.Shakes; i++)
            {
                yield return _eggThrow.Shake();
            }
            if (result.Caught)
            {
                BattleSounds.Play(BattleSound.Win);
                yield return _messageWindow.Play(string.Format(CaughtFormat, wild.Name));
                _eggThrow.HideEgg();
                yield break;
            }
            yield return _eggThrow.BreakOut(egg);
            yield return _enemyView.Appear(SpriteOf(OpponentSide, wild));
            yield return _messageWindow.Play(BrokeOutMessages[result.Shakes]);
        }
    }
}
