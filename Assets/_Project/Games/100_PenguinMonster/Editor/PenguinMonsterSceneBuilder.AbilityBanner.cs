using PenguinMonster.View;
using UnityEditor;
using UnityEngine;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 特性が働いたときの帯（仕様書 §7）。名前の枠と同じ見た目の小さな枠に「〇〇の / 特性の名前」の2段。
    /// ペンモンの絵と名前の枠の間の空いたところに置き、自分側は左から・相手側は右から滑り込ませる
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // 自分側は 自分の絵 と 自分の名前の枠 の間、相手側は 相手の名前の枠 と 相手の絵 の間（基準解像度 1920x1080、画面中央が原点）
        private static readonly Vector2 PlayerBannerPosition = new Vector2(-40f, -130f);
        private static readonly Vector2 EnemyBannerPosition = new Vector2(60f, 380f);
        private static readonly Vector2 AbilityBannerSize = new Vector2(400f, 110f);
        private const float BannerSlideDistance = 300f;
        private const int BannerOwnerFontSize = 28;
        private const int BannerAbilityFontSize = 40;
        // 上の段（〇〇の）と下の段（特性の名前）の境目（枠の中の高さに対する割合）
        private const float BannerSplit = 0.55f;

        private static AbilityBanner CreateAbilityBanner(Transform parent, string name, Vector2 position, bool fromLeft)
        {
            var frame = CreateFramedBox(parent, name, InfoBoxColor, out var inner);
            Place(frame.rectTransform, Center, position, AbilityBannerSize);
            // 出ている間も、場をタップしてメッセージを送れるようにする
            frame.raycastTarget = false;
            var group = frame.gameObject.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;
            group.interactable = false;

            var owner = CreateText(inner, "Owner", "", BannerOwnerFontSize, BoxTextColor, TextAnchor.MiddleLeft);
            SetBand(owner.rectTransform, BannerSplit, 1f, InfoPadding);
            var ability = CreateText(inner, "Ability", "", BannerAbilityFontSize, BoxTextColor, TextAnchor.MiddleRight);
            ability.fontStyle = FontStyle.Bold;
            SetBand(ability.rectTransform, 0f, BannerSplit, InfoPadding);

            var banner = frame.gameObject.AddComponent<AbilityBanner>();
            var so = new SerializedObject(banner);
            so.FindProperty("_ownerText").objectReferenceValue = owner;
            so.FindProperty("_abilityText").objectReferenceValue = ability;
            so.FindProperty("_slideFrom").vector2Value = new Vector2(fromLeft ? -BannerSlideDistance : BannerSlideDistance, 0f);
            so.ApplyModifiedProperties();
            return banner;
        }
    }
}
