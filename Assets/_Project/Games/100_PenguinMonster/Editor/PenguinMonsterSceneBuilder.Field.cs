using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 上側のバトル場（空・地面・台座・ペンモン・名前とHPの枠）。ダイパ風に奥右に相手、手前左に自分。
    /// ダブルスはダイパと同じく1つの台座に2体を左右に並べ、絵を一回り小さくする。名前の枠は縦に2つ重ねる
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // 地面の上端（画面の下からの割合）。相手の台座が地面に乗る高さにする
        private const float HorizonRatio = 0.7f;

        // 配置（基準解像度 1920x1080、画面中央が原点）
        private static readonly Vector2 EnemyPlatformPosition = new Vector2(420f, 150f);
        private static readonly Vector2 EnemyPlatformSize = new Vector2(560f, 140f);
        private static readonly Vector2 PlayerPlatformPosition = new Vector2(-480f, -200f);
        private static readonly Vector2 PlayerPlatformSize = new Vector2(760f, 180f);

        // 64x64 の絵を整数倍で拡大すると、ドットの大きさがそろう（相手 5倍・自分 7倍）
        private const float EnemySpriteSize = 320f;
        private const float PlayerSpriteSize = 448f;
        // 台座の中心より少し奥に足を置いて、台座に立っているように見せる
        private const float FeetAbovePlatform = 12f;
        // 踏み込み・登場の「ぽよん」がマスクで切れないための余白
        private const float SlotMargin = 120f;
        private static readonly Vector2 EnemyLungeDirection = new Vector2(-1f, 0f);
        private static readonly Vector2 PlayerLungeDirection = new Vector2(1f, 0.5f);

        private static readonly Vector2 EnemyInfoPosition = new Vector2(-500f, 400f);
        // 相手の枠は下の行にランク変化（攻↑ 等）を入れるぶん、縦に伸ばしてある
        private static readonly Vector2 EnemyInfoSize = new Vector2(700f, 190f);
        private static readonly Vector2 PlayerInfoPosition = new Vector2(500f, -110f);
        private static readonly Vector2 PlayerInfoSize = new Vector2(700f, 190f);

        // ダブルス。台座の中心から左右にずらして並べる（相手 4倍・自分 6倍）
        private const float EnemyDoubleOffsetX = 140f;
        private const float PlayerDoubleOffsetX = 190f;
        private const float EnemyDoubleSpriteSize = 256f;
        private const float PlayerDoubleSpriteSize = 384f;
        // 相手の枠は状態異常の印を枠のすぐ下に出すので、そのぶん（60）間を空けて重ねる。上端は画面の端から 50 空ける
        private static readonly Vector2[] EnemyDoubleInfoPositions = { new Vector2(-540f, 415f), new Vector2(-540f, 205f) };
        private static readonly Vector2 EnemyDoubleInfoSize = new Vector2(640f, 150f);
        // 下の枠の HP の数字がメッセージ窓に隠れないよう、窓の上端（y=-240）から 20 空ける
        private static readonly Vector2[] PlayerDoubleInfoPositions = { new Vector2(560f, 25f), new Vector2(560f, -140f) };
        private static readonly Vector2 PlayerDoubleInfoSize = new Vector2(640f, 160f);
        private const float InfoPadding = 28f;
        private const float HpBarHeight = 22f;
        private const float HpFillInset = 3f;
        // 相手の枠の、ランク変化を出す下の行の上端（枠の中の高さに対する割合）
        private const float EnemyStatBandTop = 0.27f;

        private const int NameFontSize = 40;
        private const int HpLabelFontSize = 26;
        private const int HpFontSize = 32;
        private const int StatusFontSize = 28;
        private const int StatIndicatorFontSize = 24;
        private static readonly Vector2 StatusBadgeSize = new Vector2(130f, 44f);
        private const float StatusBadgeGap = 6f;

        private static readonly Color HpLabelColor = new Color(0.95f, 0.6f, 0.15f);
        private static readonly Color HpBackColor = new Color(0.25f, 0.27f, 0.3f);

        private static void BuildField(Transform parent, out MonsterView playerView, out MonsterView enemyView,
            out MonsterInfoBox playerInfo, out MonsterInfoBox enemyInfo)
        {
            CreateBackdrop(parent);
            enemyView = CreateMonsterView(parent, "EnemyView", EnemyPlatformPosition, EnemySpriteSize, EnemyLungeDirection);
            playerView = CreateMonsterView(parent, "PlayerView", PlayerPlatformPosition, PlayerSpriteSize, PlayerLungeDirection);
            enemyInfo = CreateInfoBox(parent, "EnemyInfo", EnemyInfoPosition, EnemyInfoSize, showHpNumbers: false);
            playerInfo = CreateInfoBox(parent, "PlayerInfo", PlayerInfoPosition, PlayerInfoSize, showHpNumbers: true);
        }

        /// <summary>ダブルスの場（場の順。0 が左）。シングルの場と同じ所に重ねて作り、使わない方は隠れたまま</summary>
        private static void BuildDoubleField(Transform parent, out MonsterView[] playerViews, out MonsterView[] enemyViews,
            out MonsterInfoBox[] playerInfos, out MonsterInfoBox[] enemyInfos)
        {
            int count = BattleBalance.DoublesActiveCount;
            playerViews = new MonsterView[count];
            enemyViews = new MonsterView[count];
            playerInfos = new MonsterInfoBox[count];
            enemyInfos = new MonsterInfoBox[count];
            for (int slot = 0; slot < count; slot++)
            {
                float side = slot == 0 ? -1f : 1f;
                enemyViews[slot] = CreateMonsterView(parent, $"EnemyView{slot}",
                    EnemyPlatformPosition + new Vector2(side * EnemyDoubleOffsetX, 0f), EnemyDoubleSpriteSize, EnemyLungeDirection);
                playerViews[slot] = CreateMonsterView(parent, $"PlayerView{slot}",
                    PlayerPlatformPosition + new Vector2(side * PlayerDoubleOffsetX, 0f), PlayerDoubleSpriteSize, PlayerLungeDirection);
            }
            for (int slot = 0; slot < count; slot++)
            {
                enemyInfos[slot] = CreateInfoBox(parent, $"EnemyInfo{slot}", EnemyDoubleInfoPositions[slot], EnemyDoubleInfoSize,
                    showHpNumbers: false);
                playerInfos[slot] = CreateInfoBox(parent, $"PlayerInfo{slot}", PlayerDoubleInfoPositions[slot], PlayerDoubleInfoSize,
                    showHpNumbers: true);
            }
        }

        private static void CreateBackdrop(Transform parent)
        {
            var sky = CreateImage(parent, "Sky", Color.white);
            UIDialogBuilder.SetStretchAll(sky.rectTransform);
            var ground = CreateImage(parent, "Ground", Color.white);
            UIDialogBuilder.SetStretchAll(ground.rectTransform);
            ground.rectTransform.anchorMax = new Vector2(1f, HorizonRatio);

            var enemyPlatform = CreateImage(parent, "EnemyPlatform", Color.white);
            Place(enemyPlatform.rectTransform, Center, EnemyPlatformPosition, EnemyPlatformSize);
            var playerPlatform = CreateImage(parent, "PlayerPlatform", Color.white);
            Place(playerPlatform.rectTransform, Center, PlayerPlatformPosition, PlayerPlatformSize);

            foreach (var image in new[] { sky, ground, enemyPlatform, playerPlatform })
            {
                image.raycastTarget = false;
            }

            var backdrop = sky.gameObject.AddComponent<BattleBackdrop>();
            var so = new SerializedObject(backdrop);
            so.FindProperty("_sky").objectReferenceValue = sky;
            so.FindProperty("_ground").objectReferenceValue = ground;
            SetArray(so.FindProperty("_platforms"), new Object[] { enemyPlatform, playerPlatform });
            so.ApplyModifiedProperties();
        }

        /// <summary>
        /// マスク（下端 = 足元）の中に絵を置く。ひんしで下へ沈めたとき、足元より下が見えなくなる
        /// </summary>
        private static MonsterView CreateMonsterView(Transform parent, string name, Vector2 platformPosition, float spriteSize, Vector2 lungeDirection)
        {
            var slot = UIDialogBuilder.CreateUIObject(name, parent);
            var slotRect = slot.GetComponent<RectTransform>();
            Place(slotRect, Center, platformPosition + new Vector2(0f, FeetAbovePlatform),
                new Vector2(spriteSize + SlotMargin * 2f, spriteSize + SlotMargin));
            slotRect.pivot = new Vector2(0.5f, 0f);
            slot.AddComponent<RectMask2D>();

            var image = CreateImage(slot.transform, "Sprite", Color.white);
            image.preserveAspect = true;
            image.raycastTarget = false;
            var rect = image.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(spriteSize, spriteSize);

            var view = slot.AddComponent<MonsterView>();
            var so = new SerializedObject(view);
            so.FindProperty("_image").objectReferenceValue = image;
            so.FindProperty("_lungeDirection").vector2Value = lungeDirection;
            so.ApplyModifiedProperties();
            return view;
        }

        /// <summary>
        /// 技のエフェクトを出す層。ペンモンの絵は台座のマスクの中にあって上や横にはみ出すと切れるので、場全体に重ねて別に置く
        /// </summary>
        private static MoveEffectView CreateMoveEffectLayer(Transform parent)
        {
            var layer = UIDialogBuilder.CreateUIObject("MoveEffects", parent);
            UIDialogBuilder.SetStretchAll(layer.GetComponent<RectTransform>());
            var particles = new Image[MoveEffectView.ParticleCount];
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = CreateImage(layer.transform, $"Particle{i}", Color.white);
                particles[i].raycastTarget = false;
                particles[i].enabled = false;
            }

            var view = layer.AddComponent<MoveEffectView>();
            var so = new SerializedObject(view);
            SetArray(so.FindProperty("_particles"), particles);
            so.ApplyModifiedProperties();
            return view;
        }

        private static MonsterInfoBox CreateInfoBox(Transform parent, string name, Vector2 position, Vector2 size, bool showHpNumbers)
        {
            var frame = CreateFramedBox(parent, name, InfoBoxColor, out var inner);
            Place(frame.rectTransform, Center, position, size);

            // 数字が無い相手側は、下の行（0〜EnemyStatBandTop）をランク変化に使い、その上に名前とHPバーを置く
            float nameBottom = showHpNumbers ? 0.6f : 0.55f;
            float barY = showHpNumbers ? 0.45f : 0.4f;

            var nameText = CreateText(inner, "Name", "", NameFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            SetBand(nameText.rectTransform, nameBottom, 1f, InfoPadding);
            nameText.supportRichText = true;
            var levelText = CreateText(inner, "Level", "", NameFontSize, BoxTextColor, TextAnchor.MiddleRight);
            SetBand(levelText.rectTransform, nameBottom, 1f, InfoPadding);

            var hpLabel = CreateText(inner, "HpLabel", "HP", HpLabelFontSize, HpLabelColor, TextAnchor.MiddleLeft);
            hpLabel.fontStyle = FontStyle.Bold;
            hpLabel.rectTransform.anchorMin = new Vector2(0f, barY);
            hpLabel.rectTransform.anchorMax = new Vector2(0.2f, barY);
            hpLabel.rectTransform.offsetMin = new Vector2(InfoPadding, -HpBarHeight);
            hpLabel.rectTransform.offsetMax = new Vector2(0f, HpBarHeight);

            var hpBack = CreateImage(inner, "HpBar", HpBackColor);
            var hpBackRect = hpBack.rectTransform;
            hpBackRect.anchorMin = new Vector2(0.18f, barY);
            hpBackRect.anchorMax = new Vector2(1f, barY);
            hpBackRect.offsetMin = new Vector2(0f, -HpBarHeight / 2f);
            hpBackRect.offsetMax = new Vector2(-InfoPadding, HpBarHeight / 2f);
            var hpArea = UIDialogBuilder.CreateUIObject("FillArea", hpBack.transform).GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(hpArea);
            hpArea.offsetMin = Vector2.one * HpFillInset;
            hpArea.offsetMax = -Vector2.one * HpFillInset;
            var hpFill = CreateImage(hpArea, "Fill", Color.green);
            UIDialogBuilder.SetStretchAll(hpFill.rectTransform);

            Text hpText = null;
            if (showHpNumbers)
            {
                hpText = CreateText(inner, "HpText", "", HpFontSize, BoxTextColor, TextAnchor.MiddleRight);
                SetBand(hpText.rectTransform, 0f, 0.3f, InfoPadding);
            }

            var badge = CreateStatusBadge(showHpNumbers ? inner : frame.rectTransform, showHpNumbers);

            // ランク変化インジケーター（攻↑ 防↓↓ 等）。枠の外だと空の上に文字が浮いて見えるので、両側とも枠の中に置く
            var statIndicator = CreateText(inner, "StatIndicator", "", StatIndicatorFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            statIndicator.supportRichText = true;
            var statRect = statIndicator.rectTransform;
            if (showHpNumbers)
            {
                // 自分側: HP数字と同じ行の左半分に置く
                SetBand(statRect, 0f, 0.3f, InfoPadding);
                statRect.anchorMax = new Vector2(0.65f, 0.3f);
            }
            else
            {
                // 相手側: HPバーの下の行
                SetBand(statRect, 0f, EnemyStatBandTop, InfoPadding);
            }
            statIndicator.gameObject.SetActive(false);

            var box = frame.gameObject.AddComponent<MonsterInfoBox>();
            var so = new SerializedObject(box);
            so.FindProperty("_nameText").objectReferenceValue = nameText;
            so.FindProperty("_levelText").objectReferenceValue = levelText;
            so.FindProperty("_hpFill").objectReferenceValue = hpFill.rectTransform;
            so.FindProperty("_hpFillImage").objectReferenceValue = hpFill;
            so.FindProperty("_hpText").objectReferenceValue = hpText;
            so.FindProperty("_statusBadge").objectReferenceValue = badge;
            so.FindProperty("_statusText").objectReferenceValue = badge.GetComponentInChildren<Text>();
            so.FindProperty("_statIndicatorText").objectReferenceValue = statIndicator;
            so.ApplyModifiedProperties();
            return box;
        }

        /// <summary>
        /// 状態異常の印（色付きの札に「まひ」など）。自分側は HP の数字の左の空き、
        /// 相手側は枠の中に余白が無いので枠のすぐ下に置く
        /// </summary>
        private static Image CreateStatusBadge(RectTransform parent, bool inside)
        {
            var badge = CreateImage(parent, "StatusBadge", Color.gray);
            badge.raycastTarget = false;
            var rect = badge.rectTransform;
            rect.anchorMin = rect.anchorMax = inside ? new Vector2(0f, 0.15f) : Vector2.zero;
            rect.pivot = inside ? new Vector2(0f, 0.5f) : new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(InfoPadding, inside ? 0f : -StatusBadgeGap);
            rect.sizeDelta = StatusBadgeSize;

            var label = CreateText(badge.transform, "Label", "", StatusFontSize, Color.white, TextAnchor.MiddleCenter);
            label.fontStyle = FontStyle.Bold;
            badge.gameObject.SetActive(false);
            return badge;
        }
    }
}
