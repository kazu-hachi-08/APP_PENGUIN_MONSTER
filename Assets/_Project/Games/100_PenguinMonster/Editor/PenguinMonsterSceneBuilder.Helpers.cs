using PenguinMonster.Common.Editor;
using PenguinMonster.Common.UI;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 Center = new Vector2(0.5f, 0.5f);
        private const float FrameThickness = 6f;
        private const float DefaultBandPadding = 20f;
        private const float IconTopMargin = 8f;
        private static readonly Vector2 CountdownSize = new Vector2(360f, 70f);
        private const int CountdownFontSize = 44;
        private static readonly Color CountdownColor = new Color(1f, 0.95f, 0.75f);
        private static readonly Vector2 CountdownOutline = new Vector2(2f, -2f);

        private static void Place(RectTransform rect, Vector2 anchor, Vector2 position, Vector2 size)
        {
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        /// <summary>親の上下 [min, max] の帯に横いっぱいで置く（左右に少し余白）</summary>
        private static void SetBand(RectTransform rect, float min, float max, float padding = DefaultBandPadding)
        {
            rect.anchorMin = new Vector2(0f, min);
            rect.anchorMax = new Vector2(1f, max);
            rect.offsetMin = new Vector2(padding, 0f);
            rect.offsetMax = new Vector2(-padding, 0f);
        }

        /// <summary>
        /// 画面いっぱいの背景の上に、Screen.safeArea に合わせた中身の置き場を作る。
        /// 背景色は端まで塗りつつ、文字やボタンはスマホのカメラ・角丸に被らないようにするため
        /// </summary>
        private static Transform CreateSafeContent(Transform back, string name = "SafeContent")
        {
            var content = UIDialogBuilder.CreateUIObject(name, back);
            UIDialogBuilder.SetStretchAll(content.GetComponent<RectTransform>());
            content.AddComponent<SafeAreaFitter>();
            return content.transform;
        }

        /// <summary>濃い色の縁の内側に、指定の色の面を重ねた枠（ダイパ風の窓）。中身は inner に置く</summary>
        private static Image CreateFramedBox(Transform parent, string name, Color fillColor, out RectTransform inner)
        {
            var frame = CreateImage(parent, name, FrameColor);
            var fill = CreateImage(frame.transform, "Fill", fillColor);
            fill.raycastTarget = false;
            inner = fill.rectTransform;
            UIDialogBuilder.SetStretchAll(inner);
            inner.offsetMin = Vector2.one * FrameThickness;
            inner.offsetMax = -Vector2.one * FrameThickness;
            return frame;
        }

        private static Image CreateImage(Transform parent, string name, Color color)
        {
            var obj = UIDialogBuilder.CreateUIObject(name, parent);
            var image = obj.AddComponent<Image>();
            image.color = color;
            return image;
        }

        private static Text CreateText(Transform parent, string name, string content, int fontSize, Color color, TextAnchor alignment)
        {
            var obj = UIDialogBuilder.CreateUIObject(name, parent);
            UIDialogBuilder.SetStretchAll(obj.GetComponent<RectTransform>());
            var text = obj.AddComponent<Text>();
            text.text = content;
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = alignment;
            // 表示用の Text がタップを奪わないよう、レイキャストを切る
            text.raycastTarget = false;
            return text;
        }

        /// <summary>
        /// ゲームのボタンはすべて、タイトルと同じ「ぷっくり」ボタンにする（StyleButton）。
        /// 文字があれば影を付けて、色の付いた面の上でも読めるようにする
        /// </summary>
        /// <param name="sinkLabel">押したときに文字も沈めるか。中身を自分で並べるカードは false にして面だけ沈める</param>
        private static GameObject CreateButton(Transform parent, string name, string label, Vector2 size, Color color, bool sinkLabel = true, int fontSize = ButtonFontSize)
        {
            var obj = UIDialogBuilder.CreateButton(parent, name, label, size.x, size.y, color);
            var text = obj.GetComponentInChildren<Text>();
            text.fontSize = fontSize;
            text.raycastTarget = false;
            if (!string.IsNullOrEmpty(label))
            {
                AddOutline(text, ButtonTextEdgeColor, ButtonTextEdge);
            }
            StyleButton(obj, sinkLabel ? text.rectTransform : null);
            return obj;
        }

        /// <summary>親の上端の中央に、ペンモンの絵を置く枠（絵は実行時にはめる）</summary>
        private static Image CreateTopIcon(Transform parent, float size, float top = IconTopMargin)
        {
            var icon = CreateImage(parent, "Icon", Color.white);
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            var rect = icon.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -top);
            rect.sizeDelta = new Vector2(size, size);
            return icon;
        }

        /// <summary>画面の下に高さ height の帯を作り、中身を横に並べる</summary>
        private static Transform CreateFooter(Transform parent, float height, float spacing)
        {
            var footer = UIDialogBuilder.CreateUIObject("Footer", parent);
            var rect = footer.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rect);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.offsetMax = new Vector2(0f, height);
            AddChildLayout<HorizontalLayoutGroup>(footer, spacing, TextAnchor.MiddleCenter);
            return footer.transform;
        }

        /// <summary>
        /// 中身を横か縦に並べる箱を付ける。子の幅・高さは箱が決めず、子のサイズのまま置く。
        /// 呼び出し側は戻り値に padding などを足せる（設定の順番は今と同じに保つ）
        /// </summary>
        private static T AddChildLayout<T>(GameObject obj, float spacing, TextAnchor alignment) where T : HorizontalOrVerticalLayoutGroup
        {
            var layout = obj.AddComponent<T>();
            layout.spacing = spacing;
            layout.childAlignment = alignment;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            return layout;
        }

        /// <summary>
        /// 画面の上端に高さ height の見出しを置く。ロゴ風の縁取りは StyleHeading が付ける（文字色もそこで白になる）。
        /// 右側の余白は呼び出し側が offsetMax で決める
        /// </summary>
        private static Text CreateTopTitle(Transform parent, string text, int fontSize, float height, TextAnchor alignment = TextAnchor.MiddleCenter, float left = 0f)
        {
            var title = CreateText(parent, "Title", text, fontSize, Color.white, alignment);
            StyleHeading(title);
            title.rectTransform.anchorMin = new Vector2(0f, 1f);
            title.rectTransform.offsetMin = new Vector2(left, -height);
            return title;
        }

        /// <summary>
        /// 横一列にマスを並べる行。左右は親いっぱい、下と上は offset で決める。
        /// 見せ合い・選出と持ち物の画面で同じ作りを共有する
        /// </summary>
        private static Transform CreateCellRow(Transform parent, string name, float bottom, float top, float spacing)
        {
            var row = UIDialogBuilder.CreateUIObject(name, parent);
            var rect = row.GetComponent<RectTransform>();
            UIDialogBuilder.SetStretchAll(rect);
            rect.offsetMin = new Vector2(0f, bottom);
            rect.offsetMax = new Vector2(0f, -top);
            AddChildLayout<HorizontalLayoutGroup>(row, spacing, TextAnchor.MiddleCenter);
            return row.transform;
        }

        /// <summary>オンラインの制限時間の表示。止めている間は隠れるので、オフラインでは見えない</summary>
        private static CountdownTimer CreateCountdown(Transform parent, Vector2 anchor, Vector2 position)
        {
            var text = CreateText(parent, "Countdown", "", CountdownFontSize, CountdownColor, TextAnchor.MiddleCenter);
            text.fontStyle = FontStyle.Bold;
            // 空や白い窓の上にも出るので、縁取りで読めるようにする
            text.gameObject.AddComponent<Outline>().effectDistance = CountdownOutline;
            Place(text.rectTransform, anchor, position, CountdownSize);

            var timer = text.gameObject.AddComponent<CountdownTimer>();
            var so = new SerializedObject(timer);
            so.FindProperty("_text").objectReferenceValue = text;
            so.ApplyModifiedProperties();
            text.gameObject.SetActive(false);
            return timer;
        }

        /// <summary>
        /// 右下の会話窓。セーフエリアの中身 safe（呼び出し側が AddComponent と SetActive をする）と、
        /// 窓の中身の置き場 inner を返す。position は右下からの余白、size は窓の大きさ
        /// </summary>
        private static Image CreateBottomRightWindow(Transform parent, string safeName, string frameName, Vector2 position, Vector2 size,
            out Transform safe, out RectTransform inner)
        {
            safe = CreateSafeContent(parent);
            safe.name = safeName;
            var frame = CreateFramedBox(safe, frameName, MessageBoxColor, out inner);
            var rect = frame.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            return frame;
        }

        /// <summary>
        /// タイトルと同じ夜空・雪・歩くペンモンの上に、セーフエリアの中身（戻り値）と見出しを置く。
        /// 戻るボタンは中身の行を並べた後に呼び出し側で置く（兄弟の順番を今と同じにするため）
        /// </summary>
        private static Transform CreateWalkerScreen(Transform parent, string name, string title, int titleFontSize,
            Vector2 titlePosition, Vector2 titleSize, out Image background)
        {
            background = CreateNightBack(parent, name);
            CreateSnowField(background.transform);
            CreateTitleWalkers(background.transform);
            var content = CreateSafeContent(background.transform);

            var text = CreateText(content, "Title", title, titleFontSize, Color.white, TextAnchor.MiddleCenter);
            StyleHeading(text);
            Place(text.rectTransform, new Vector2(0.5f, 1f), titlePosition, titleSize);
            return content;
        }

        /// <summary>手持ちの1枠ずつ = 押せるボタン・名前（絵の右）・絵。件数は buttons の長さで決める</summary>
        private static void CreatePartySlots(Transform parent, Vector2 slotSize, Button[] buttons, Text[] labels, Image[] icons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var slot = CreateButton(parent, $"Btn_Slot{i}", "", slotSize, SlotColor);
                buttons[i] = slot.GetComponent<Button>();
                labels[i] = slot.GetComponentInChildren<Text>();
                labels[i].rectTransform.offsetMin = new Vector2(BoxIconSize + 24f, 0f);
                labels[i].rectTransform.offsetMax = new Vector2(-SlotTextRightPadding, 0f);
                labels[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                icons[i] = CreatePartyIcon(slot.transform);
                icons[i].rectTransform.sizeDelta = Vector2.one * BoxIconSize;
            }
        }

        private static void SetArray(SerializedProperty property, Object[] values)
        {
            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            }
        }
    }
}
