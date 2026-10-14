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
                    // 「行けっ！」の文を出し、タマゴを投げて割れてから登場させる（登場の音はタマゴが割れたときに鳴る）
                    yield return _messageWindow.Show(message);
                    yield return _eggThrow.Throw(_eggs[e.Side], ViewOf(e).Body, e.Side == PlayerSide);
                    InfoOf(e).Show(e.Monster, e.Hp, e.Status);
                    TrayOf(e.Side).SetStatus(PartyIndexOf(e), e.Status);
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
                    // エフェクトが当たり切ってから音と点滅を出し、「当たった」瞬間をそろえる
                    yield return PlayMoveEffect(events, index);
                    BattleSounds.Play(HitSoundFor(events, index));
                    yield return ViewOf(e).Blink();
                    yield return InfoOf(e).AnimateHp(e.Hp);
                    break;
                case BattleEventKind.Fainted:
                    BattleSounds.Play(BattleSound.Faint);
                    yield return ViewOf(e).Faint();
                    InfoOf(e).Hide();
                    TrayOf(e.Side).SetFainted(PartyIndexOf(e));
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.StatusDamage:
                case BattleEventKind.HurtItself:
                case BattleEventKind.AbilityDamage:
                case BattleEventKind.ItemRecoil:
                case BattleEventKind.Recoil:
                case BattleEventKind.WeatherDamage:
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
                case BattleEventKind.Drained:
                case BattleEventKind.Healed:
                    // 回復は HP が増えてから理由の文を出す（減るときと同じ順）
                    BattleSounds.Play(BattleSound.StatUp);
                    yield return InfoOf(e).AnimateHp(e.Hp);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.Confused:
                    BattleSounds.Play(BattleSound.Status);
                    yield return ViewOf(e).Flash(StatusColors.Confusion);
                    InfoOf(e).SetConfused(true);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.ConfusionEnded:
                    InfoOf(e).SetConfused(false);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.StatChanged:
                    yield return PlayStatChange(e, message);
                    break;
                case BattleEventKind.AbilityActivated:
                    // 文は出さず、特性の帯を出してから続きの演出（能力が下がる など）へ進む。帯は続きの間も出しておく
                    yield return BannerOf(e.Side).Show(OwnerOf(e), e.Monster.Definition.Ability.ToJapanese());
                    break;
                case BattleEventKind.StagesPassed:
                    // 羽リレーで引き継いだ。文は出さず、名前の枠の印だけ合わせる
                    InfoOf(e).UpdateStatStages(e.Monster);
                    break;
                case BattleEventKind.WeatherStarted:
                    // 場の色が変わってから「吹雪が 吹き始めた！」
                    BattleSounds.Play(BattleSound.Status);
                    _weather.Show(e.Weather);
                    yield return _messageWindow.Play(message);
                    break;
                case BattleEventKind.WeatherEnded:
                    _weather.Hide();
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
            TrayOf(e.Side).SetStatus(PartyIndexOf(e), e.Status);
            yield return _messageWindow.Play(message);
        }

        /// <summary>能力が上がる・下がる。色が光るのと同時に、上がると ▲ が立ちのぼり、下がると ▼ が流れ落ちる</summary>
        private IEnumerator PlayStatChange(BattleEvent e, string message)
        {
            bool up = e.Amount > 0;
            BattleSounds.Play(up ? BattleSound.StatUp : BattleSound.StatDown);
            var arrows = StartCoroutine(_moveEffect.PlayStatArrows(up, ViewOf(e).Body));
            yield return ViewOf(e).Flash(up ? StatusColors.StatUp : StatusColors.StatDown);
            yield return arrows;
            InfoOf(e).UpdateStatStages(e.Monster);
            yield return _messageWindow.Play(message);
        }

        private AbilityBanner BannerOf(int side) => side == PlayerSide ? _playerBanner : _enemyBanner;

        /// <summary>帯の上の段。相手側は文と同じく「相手の」を付ける</summary>
        private static string OwnerOf(BattleEvent e)
        {
            return e.Side == PlayerSide ? $"{e.Monster.Name}の" : $"相手の {e.Monster.Name}の";
        }

        /// <summary>
        /// 技のエフェクトを受けた側に出す。ダメージのイベントは技の名前を持たないので、前の「〇〇の △△！」までさかのぼる。
        /// 技は名前から引き直す（オンラインのゲストも技の名前は受け取っているので、通信を増やさずに済む）
        /// </summary>
        private IEnumerator PlayMoveEffect(List<BattleEvent> events, int damagedIndex)
        {
            int usedIndex = damagedIndex - 1;
            while (usedIndex >= 0 && events[usedIndex].Kind != BattleEventKind.MoveUsed)
            {
                usedIndex--;
            }
            var move = usedIndex < 0 ? null : MoveOf(events[usedIndex]);
            if (move == null)
            {
                yield break;
            }
            yield return _moveEffect.Play(move, ViewOf(events[damagedIndex]).Body, ViewOf(events[usedIndex]).Body);
        }

        private static MoveDefinition MoveOf(BattleEvent used)
        {
            foreach (var move in used.Monster.Definition.Moves)
            {
                if (move.Name == used.MoveName)
                {
                    return move;
                }
            }
            return null;
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

        private PartyEggTray TrayOf(int side) => side == PlayerSide ? _playerTray : _enemyTray;

        /// <summary>イベントの主語が選出の何番目か。タマゴの列は選出の順に並んでいる</summary>
        private int PartyIndexOf(BattleEvent e)
        {
            var party = _state.Sides[e.Side].Party;
            for (int i = 0; i < party.Count; i++)
            {
                if (party[i] == e.Monster)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool IsDoubles => _state.Sides[PlayerSide].ActiveCount > 1;

        /// <summary>手前の自分は後ろ姿、奥の相手は正面</summary>
        private static Sprite SpriteOf(int side, BattleMonster monster)
        {
            return side == PlayerSide ? MonsterSprites.Back(monster.Definition) : MonsterSprites.Front(monster.Definition);
        }
    }
}
