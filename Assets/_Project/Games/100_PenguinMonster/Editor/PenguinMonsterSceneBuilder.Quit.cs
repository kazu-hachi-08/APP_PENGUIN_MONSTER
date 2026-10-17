using PenguinMonster.Common.Editor;
using PenguinMonster.Common.UI;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// ドラフト〜バトルの間に出す「やめる」と、その確認。
    /// 左上はバトルで相手の名前の枠があるので右上に置く（ドラフト・選出の残り秒数はその左）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 QuitButtonPosition = new Vector2(-150f, -55f);
        private static readonly Vector2 QuitButtonSize = new Vector2(220f, 80f);
        private static readonly Color QuitButtonColor = new Color(0.45f, 0.45f, 0.5f);
        private static readonly string[] QuitDialogButtons = { "Btn_Cancel", "Btn_Confirm" };

        private static QuitButton CreateQuitButton(Transform parent)
        {
            // 押せる所はセーフエリアの内側。背景は無いので、ボタン以外のタップは下の画面へ通る
            var root = UIDialogBuilder.CreateUIObject("QuitButton", parent);
            UIDialogBuilder.SetStretchAll(root.GetComponent<RectTransform>());
            root.AddComponent<SafeAreaFitter>();
            var buttonObj = CreateButton(root.transform, "Btn_Quit", "やめる", QuitButtonSize, QuitButtonColor);
            Place(buttonObj.GetComponent<RectTransform>(), TopRight, QuitButtonPosition, QuitButtonSize);

            // 確認は画面いっぱいに暗幕を敷きたいので、セーフエリアの外（兄弟）に置く
            var dialog = CreateConfirmDialog(parent);

            var quit = root.AddComponent<QuitButton>();
            var so = new SerializedObject(quit);
            so.FindProperty("_button").objectReferenceValue = buttonObj.GetComponent<Button>();
            so.FindProperty("_dialog").objectReferenceValue = dialog;
            so.ApplyModifiedProperties();
            root.SetActive(false);
            return quit;
        }

        /// <summary>確かめの窓。箱は共通基盤のままだが、ボタンだけはゲームのほかのボタンと同じ見た目にそろえる</summary>
        private static CommonDialog CreateConfirmDialog(Transform parent)
        {
            var dialog = UIDialogBuilder.CreateCommonDialog(parent);
            foreach (var name in QuitDialogButtons)
            {
                var button = dialog.transform.Find($"Panel/ButtonArea/{name}");
                StyleButton(button.gameObject, (RectTransform)button.GetComponentInChildren<Text>().transform);
            }
            return dialog;
        }
    }
}
