using PenguinMonster.Common.Editor;
using PenguinMonster.View;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// タイトル以外の画面もタイトルと同じ見た目にそろえる部品: 夜空と雪の背景・ロゴ風の見出し・ぷっくりボタン・
    /// 「ぽよん」と滑り込みの演出（仕様書 §7）
    /// </summary>
    public static partial class PenguinMonsterSceneBuilder
    {
        private static readonly Vector2 ButtonTextEdge = new Vector2(3f, -3f);
        private static readonly Color ButtonTextEdgeColor = new Color(0f, 0f, 0f, 0.5f);

        // ロゴ（文字 180）と同じ重ね方を、見出しの大きさ（50〜100）に合わせて細くしたもの
        private static readonly Vector2 HeadingInnerEdge = new Vector2(3f, -3f);
        private static readonly Vector2 HeadingOuterEdge = new Vector2(6f, -6f);
        private static readonly Vector2 HeadingShadow = new Vector2(6f, -9f);

        /// <summary>
        /// 画面いっぱいの夜空（NightSky）と、降る雪。中身はこの上に重ねる。
        /// 夜空の絵は実行時にはめるので、シーン上では白い四角のまま
        /// </summary>
        private static Image CreateNightBack(Transform parent, string name)
        {
            var back = CreateImage(parent, name, Color.white);
            UIDialogBuilder.SetStretchAll(back.rectTransform);
            back.gameObject.AddComponent<NightSky>();
            CreateTitleSnow(back.transform);
            return back;
        }

        /// <summary>見出しをロゴと同じ「白と紺の2重の縁取り＋影」にする。夜空の上でも目立たせるため</summary>
        private static void StyleHeading(Text text)
        {
            text.fontStyle = FontStyle.Bold;
            text.color = TitleLogoColor;
            AddOutline(text, Color.white, HeadingInnerEdge);
            AddOutline(text, TitleLogoEdgeColor, HeadingOuterEdge);
            var shadow = text.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TitleLogoEdgeColor;
            shadow.effectDistance = HeadingShadow;
        }

        /// <summary>
        /// 角丸の絵と押し込みは ChunkyButton が実行時にはめる（絵をアセットにしないので、シーン上では四角のまま）。
        /// 共通基盤が作ったボタン（確認ダイアログなど）にも後から掛けられるよう、ボタン単位で付ける
        /// </summary>
        private static void StyleButton(GameObject button, RectTransform label)
        {
            var chunky = button.AddComponent<ChunkyButton>();
            var so = new SerializedObject(chunky);
            so.FindProperty("_label").objectReferenceValue = label;
            so.ApplyModifiedProperties();
        }

        /// <param name="startScale">0 なら小さい所から弾んで出る。1 より大きければ上から叩きつけたように縮む</param>
        private static PopIn AddPopIn(GameObject target, float startScale = 0f)
        {
            var pop = target.AddComponent<PopIn>();
            var so = new SerializedObject(pop);
            so.FindProperty("_startScale").floatValue = startScale;
            so.ApplyModifiedProperties();
            return pop;
        }

        /// <param name="from">元の位置からどれだけ離れた所から滑り込むか</param>
        private static SlideIn AddSlideIn(GameObject target, Vector2 from)
        {
            var slide = target.AddComponent<SlideIn>();
            var so = new SerializedObject(slide);
            so.FindProperty("_from").vector2Value = from;
            so.ApplyModifiedProperties();
            return slide;
        }
    }
}
