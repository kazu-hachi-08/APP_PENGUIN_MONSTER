using PenguinMonster.Battle;
using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// 選出の残りを見せるタマゴの列と、繰り出すときにタマゴを投げる層（仕様書 §7）。
    /// 列は名前の枠のそばに置く。自分は枠の上の右寄せ、相手は枠の下の右寄せ（枠の下の左には状態異常の印があるため）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        // 16x16 のタマゴを3倍で出す
        private const float TrayEggSize = 48f;
        private const float TrayEggGap = 6f;
        private const float TrayDotSize = 18f;
        // 列の置き場（基準解像度、画面中央が原点）。名前の枠の端から 8 空ける
        private static readonly Vector2 PlayerTraySinglePosition = new Vector2(850f, -7f);
        private static readonly Vector2 PlayerTrayDoublePosition = new Vector2(880f, 113f);
        private static readonly Vector2 EnemyTraySinglePosition = new Vector2(-150f, 297f);
        private static readonly Vector2 EnemyTrayDoublePosition = new Vector2(-220f, 122f);
        // 自分の列は下端、相手の列は上端を名前の枠に合わせる。どちらも右寄せ
        private static readonly Vector2 PlayerTrayPivot = new Vector2(1f, 0f);
        private static readonly Vector2 EnemyTrayPivot = new Vector2(1f, 1f);

        private static PartyEggTray CreatePartyTray(Transform parent, string name, Vector2 pivot, Vector2 singlePosition,
            Vector2 doublePosition)
        {
            var tray = UIDialogBuilder.CreateUIObject(name, parent);
            var rect = tray.GetComponent<RectTransform>();
            int count = Mathf.Max(BattleBalance.PartySize, BattleBalance.DoublesPartySize);
            Place(rect, Center, singlePosition, new Vector2(count * (TrayEggSize + TrayEggGap), TrayEggSize));
            rect.pivot = pivot;

            // 3体のときも右に詰めて並べたいので、隠したタマゴを詰めるレイアウトにする
            var layout = tray.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleRight;
            layout.spacing = TrayEggGap;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            var eggs = new Image[count];
            var dots = new Image[count];
            for (int i = 0; i < count; i++)
            {
                eggs[i] = CreateImage(tray.transform, $"Egg{i}", Color.white);
                eggs[i].raycastTarget = false;
                eggs[i].rectTransform.sizeDelta = Vector2.one * TrayEggSize;

                // 状態異常の点はタマゴの右下に重ねる
                dots[i] = CreateImage(eggs[i].transform, "StatusDot", Color.white);
                dots[i].raycastTarget = false;
                var dotRect = dots[i].rectTransform;
                dotRect.anchorMin = dotRect.anchorMax = new Vector2(1f, 0f);
                dotRect.pivot = new Vector2(1f, 0f);
                dotRect.anchoredPosition = Vector2.zero;
                dotRect.sizeDelta = Vector2.one * TrayDotSize;
                dots[i].gameObject.SetActive(false);
            }

            var view = tray.AddComponent<PartyEggTray>();
            var so = new SerializedObject(view);
            SetArray(so.FindProperty("_eggs"), eggs);
            SetArray(so.FindProperty("_statusDots"), dots);
            so.FindProperty("_singlePosition").vector2Value = singlePosition;
            so.FindProperty("_doublePosition").vector2Value = doublePosition;
            so.ApplyModifiedProperties();
            tray.SetActive(false);
            return view;
        }

        private static void BuildPartyTrays(Transform parent, out PartyEggTray playerTray, out PartyEggTray enemyTray)
        {
            playerTray = CreatePartyTray(parent, "PlayerEggTray", PlayerTrayPivot, PlayerTraySinglePosition, PlayerTrayDoublePosition);
            enemyTray = CreatePartyTray(parent, "EnemyEggTray", EnemyTrayPivot, EnemyTraySinglePosition, EnemyTrayDoublePosition);
        }

        /// <summary>
        /// タマゴを投げる層。画面の外から飛んでくるので、台座のマスクの外（場全体）に重ねる
        /// </summary>
        private static EggThrowView CreateEggThrowLayer(Transform parent)
        {
            var layer = UIDialogBuilder.CreateUIObject("EggThrow", parent);
            UIDialogBuilder.SetStretchAll(layer.GetComponent<RectTransform>());
            var egg = CreateImage(layer.transform, "Egg", Color.white);
            var burst = CreateImage(layer.transform, "Burst", Color.white);
            foreach (var image in new[] { egg, burst })
            {
                image.raycastTarget = false;
                image.enabled = false;
            }

            var view = layer.AddComponent<EggThrowView>();
            var so = new SerializedObject(view);
            so.FindProperty("_egg").objectReferenceValue = egg;
            so.FindProperty("_burst").objectReferenceValue = burst;
            so.ApplyModifiedProperties();
            return view;
        }
    }
}
