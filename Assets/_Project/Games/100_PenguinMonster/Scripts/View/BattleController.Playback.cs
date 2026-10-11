using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Battle;
using PenguinMonster.View.Art;
using UnityEngine;

namespace PenguinMonster.View
{
    public partial class BattleController
    {
        /// <summary>イベントを順に再生する。文・演出・音の順番はダイパに寄せる</summary>
        private IEnumerator Play(List<BattleEvent> events)
        {
            for (int i = 0; i < events.Count; i++)
            {
                yield return PlayEvent(events, i);
            }
        }

        private IEnumerator PlayEvent(List<BattleEvent> events, int index)
        {
            var e = events[index];
            string message = BattleMessages.Format(e, PlayerSide);
            switch (e.Kind)
            {
                case BattleEventKind.SentOut:
                    // 「ゆけっ！」の文を出してから登場させる
                    yield return _messageWindow.Show(message);
                    InfoOf(e.Side).Show(e.Monster, e.Hp);
                    BattleSounds.Play(BattleSound.SendOut);
                    yield return ViewOf(e.Side).Appear(SpriteOf(e.Side, e.Monster));
                    break;
                case BattleEventKind.Withdrawn:
                    yield return _messageWindow.Show(message);
                    yield return ViewOf(e.Side).Withdraw();
                    InfoOf(e.Side).Hide();
                    break;
                case BattleEventKind.MoveUsed:
                    // 技名の文はタップを待たず、そのまま踏み込みとダメージへつなげる
                    yield return _messageWindow.Show(message);
                    BattleSounds.Play(BattleSound.Attack);
                    yield return ViewOf(e.Side).Lunge();
                    break;
                case BattleEventKind.Damaged:
                    BattleSounds.Play(HitSoundFor(events, index));
                    yield return ViewOf(e.Side).Blink();
                    yield return InfoOf(e.Side).AnimateHp(e.Hp);
                    break;
                case BattleEventKind.Fainted:
                    BattleSounds.Play(BattleSound.Faint);
                    yield return ViewOf(e.Side).Faint();
                    InfoOf(e.Side).Hide();
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.BattleEnd:
                    BattleSounds.Play(e.Side == PlayerSide ? BattleSound.Win : BattleSound.Lose);
                    yield return _messageWindow.Play(message);
                    break;
                default:
                    if (message != null)
                    {
                        yield return _messageWindow.Play(message);
                    }
                    break;
            }
        }

        /// <summary>
        /// 当たった音を相性で変える。相性のイベントはダメージの後に来るので、同じ攻撃の続きを先読みする
        /// </summary>
        private static BattleSound HitSoundFor(List<BattleEvent> events, int damagedIndex)
        {
            for (int i = damagedIndex + 1; i < events.Count; i++)
            {
                switch (events[i].Kind)
                {
                    case BattleEventKind.SuperEffective: return BattleSound.HitStrong;
                    case BattleEventKind.NotVeryEffective: return BattleSound.HitWeak;
                    case BattleEventKind.Critical: continue;
                    default: return BattleSound.Hit;
                }
            }
            return BattleSound.Hit;
        }

        private MonsterView ViewOf(int side) => side == PlayerSide ? _playerView : _enemyView;

        private MonsterInfoBox InfoOf(int side) => side == PlayerSide ? _playerInfo : _enemyInfo;

        /// <summary>手前の自分は後ろ姿、奥の相手は正面</summary>
        private static Sprite SpriteOf(int side, BattleMonster monster)
        {
            return side == PlayerSide ? MonsterSprites.Back(monster.Definition) : MonsterSprites.Front(monster.Definition);
        }
    }
}
