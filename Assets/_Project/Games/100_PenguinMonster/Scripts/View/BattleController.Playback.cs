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
                    InfoOf(e).Show(e.Monster, e.Hp, e.Status);
                    BattleSounds.Play(BattleSound.SendOut);
                    yield return ViewOf(e).Appear(SpriteOf(e.Side, e.Monster));
                    break;
                case BattleEventKind.Withdrawn:
                    yield return _messageWindow.Show(message);
                    yield return ViewOf(e).Withdraw();
                    InfoOf(e).Hide();
                    break;
                case BattleEventKind.MoveUsed:
                    // 技名の文はタップを待たず、そのまま踏み込みとダメージへつなげる
                    yield return _messageWindow.Show(message);
                    BattleSounds.Play(BattleSound.Attack);
                    yield return ViewOf(e).Lunge();
                    break;
                case BattleEventKind.Damaged:
                    BattleSounds.Play(HitSoundFor(events, index));
                    yield return ViewOf(e).Blink();
                    yield return InfoOf(e).AnimateHp(e.Hp);
                    break;
                case BattleEventKind.Fainted:
                    BattleSounds.Play(BattleSound.Faint);
                    yield return ViewOf(e).Faint();
                    InfoOf(e).Hide();
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.StatusDamage:
                case BattleEventKind.HurtItself:
                case BattleEventKind.AbilityDamage:
                case BattleEventKind.ItemRecoil:
                    // 技以外のダメージは、先に HP を減らしてから理由の文を出す
                    BattleSounds.Play(BattleSound.Hit);
                    yield return ViewOf(e).Blink();
                    yield return InfoOf(e).AnimateHp(e.Hp);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.StatusInflicted:
                case BattleEventKind.WokeUp:
                case BattleEventKind.Thawed:
                case BattleEventKind.ItemCured:
                    yield return PlayStatusChange(e, message);
                    break;
                case BattleEventKind.ItemHealed:
                    // 回復は HP が増えてから理由の文を出す（減るときと同じ順）
                    BattleSounds.Play(BattleSound.StatUp);
                    yield return InfoOf(e).AnimateHp(e.Hp);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.Confused:
                    BattleSounds.Play(BattleSound.Status);
                    yield return ViewOf(e).Flash(StatusColors.Confusion);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.StatChanged:
                    BattleSounds.Play(e.Amount > 0 ? BattleSound.StatUp : BattleSound.StatDown);
                    yield return ViewOf(e).Flash(e.Amount > 0 ? StatusColors.StatUp : StatusColors.StatDown);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.BattleEnd:
                    GameMusic.Stop();
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

        /// <summary>状態異常になった・治ったとき。印を付け替え、なったときだけ色を光らせる</summary>
        private IEnumerator PlayStatusChange(BattleEvent e, string message)
        {
            if (e.Status != StatusCondition.None)
            {
                BattleSounds.Play(BattleSound.Status);
                yield return ViewOf(e).Flash(StatusColors.Get(e.Status));
            }
            InfoOf(e).SetStatus(e.Status);
            yield return _messageWindow.Play(message);
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

        /// <summary>イベントの主語がいた場の絵。ダブルスは場の番号で左右を選ぶ</summary>
        private MonsterView ViewOf(BattleEvent e)
        {
            if (!IsDoubles)
            {
                return e.Side == PlayerSide ? _playerView : _enemyView;
            }
            return e.Side == PlayerSide ? _playerDoubleViews[e.Slot] : _enemyDoubleViews[e.Slot];
        }

        private MonsterInfoBox InfoOf(BattleEvent e)
        {
            if (!IsDoubles)
            {
                return e.Side == PlayerSide ? _playerInfo : _enemyInfo;
            }
            return e.Side == PlayerSide ? _playerDoubleInfos[e.Slot] : _enemyDoubleInfos[e.Slot];
        }

        private bool IsDoubles => _state.Sides[PlayerSide].ActiveCount > 1;

        /// <summary>手前の自分は後ろ姿、奥の相手は正面</summary>
        private static Sprite SpriteOf(int side, BattleMonster monster)
        {
            return side == PlayerSide ? MonsterSprites.Back(monster.Definition) : MonsterSprites.Front(monster.Definition);
        }
    }
}
