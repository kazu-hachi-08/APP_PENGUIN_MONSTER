using UnityEngine;

namespace PenguinMonster.View.Art
{
    /// <summary>
    /// 正面の直立以外の姿勢を描くための筆（傾いた体・斜めの顔・ヒレ）。
    /// 全員が正面立ちだと色違いに見えるので、姿勢とシルエットで描き分ける。
    /// 向き dir は -1 が画面の左。正面の絵は左（手前の自分側）を向かせ、後ろ姿は反転で右を向く
    /// </summary>
    public sealed partial class PenguinBrush
    {
        public const int FaceLeft = -1;
        public const int FaceRight = 1;

        private const float TurnedBeakLength = 4f;
        // 斜めの顔では、向いている側の目を顔の中心に寄せ、反対の目を外へ離すと奥行きが出る
        private const float FarEyeRate = 0.35f;
        private const float NearEyeRate = 0.65f;
        private const float BeakForwardRate = 0.6f;

        public void Tilted(float x, float y, float radiusX, float radiusY, float angle, Color32 color)
            => _canvas.FillTiltedEllipse(x, y, radiusX, radiusY, angle, color);

        /// <summary>傾いた楕円に Shaded と同じ影を付ける</summary>
        public void ShadedTilted(float x, float y, float radiusX, float radiusY, float angle, Color32 light)
            => TwoToneTilted(x, y, radiusX, radiusY, angle, PenguinPalette.Shade(light), light);

        public void BellyTilted(float x, float y, float radiusX, float radiusY, float angle)
        {
            if (!Front) return;
            TwoToneTilted(x, y, radiusX, radiusY, angle, PenguinPalette.BellyShade, PenguinPalette.Belly);
        }

        /// <summary>TwoTone の傾けた版</summary>
        private void TwoToneTilted(float x, float y, float radiusX, float radiusY, float angle, Color32 shade, Color32 light)
        {
            Tilted(x, y, radiusX, radiusY, angle, shade);
            Tilted(x - 1f, y + 1f, radiusX - LightInset, radiusY - LightInset, angle, light);
        }

        public void WhiteTilted(float x, float y, float radiusX, float radiusY, float angle)
        {
            if (!Front) return;
            Tilted(x, y, radiusX, radiusY, angle, PenguinPalette.Belly);
        }

        /// <summary>付け根が太く先が細いヒレ。向きと長さで「振る・構える・垂らす」を描き分ける</summary>
        public void Flipper(Vector2 root, Vector2 tip, float width, Color32 color)
        {
            var along = tip - root;
            float length = Vector2.Distance(root, tip);
            if (length <= 0f) return;
            var side = new Vector2(-along.y / length * width, along.x / length * width);
            Ellipse(root.x, root.y, width, width, color);
            Triangle(root + side, root - side, tip, color);
            Ellipse(tip.x, tip.y, 1f, 1f, color);
        }

        /// <summary>左右対称のヒレ。2本目は Mirror で写した位置に描く。Shaded2 / Triangle2 と同じく、片側だけ書けばよいようにするため</summary>
        public void Flipper2(Vector2 root, Vector2 tip, float width, Color32 color)
        {
            Flipper(root, tip, width, color);
            Flipper(Mirror(root), Mirror(tip), width, color);
        }

        public void Foot(float x, float y, float angle = 0f, float size = 1f)
            => Tilted(x, y, FootRadiusX * size, FootRadiusY * size, angle, PenguinPalette.Beak);

        /// <summary>左右対称に開いた1組の足。右だけ角度の符号を反対にして、左右の向きをそろえる</summary>
        public void SplayedFeet(float leftX, float rightX, float y, float splay, float size)
        {
            Foot(leftX, y, splay, size);
            Foot(rightX, y, -splay, size);
        }

        public void TurnedFace(float x, float eyeY, int dir, EyeStyle eyes, float eyeGap = 7f, bool open = false)
            => TurnedFace(x, eyeY, dir, eyes, eyes, eyeGap, open);

        /// <summary>
        /// 斜めを向いた顔。くちばしを向いた側へ突き出し、シルエットからも向きがわかるようにする。
        /// 目を左右で変えられるのは、ウインクのような片目だけの表情のため
        /// </summary>
        public void TurnedFace(float x, float eyeY, int dir, EyeStyle nearEye, EyeStyle farEye, float eyeGap = 7f, bool open = false)
        {
            if (!Front) return;
            float farX = x + dir * eyeGap * FarEyeRate;
            float nearX = x - dir * eyeGap * NearEyeRate;
            Eye(farX, eyeY, farEye, farX > nearX);
            Eye(nearX, eyeY, nearEye, nearX > farX);
            Cheek(nearX - dir, eyeY - CheekDrop);
            SideBeak(x + dir * eyeGap * BeakForwardRate, eyeY - BeakDrop, dir, open);
        }

        /// <summary>横向きのくちばし。open で口を開け、叫ぶ・笑うなどの勢いを出す</summary>
        public void SideBeak(float x, float y, int dir, bool open = false, float length = TurnedBeakLength)
        {
            float tipX = x + dir * length;
            if (open)
            {
                Ellipse(x, y, 2.5f, 2f, PenguinPalette.Mouth);
                Triangle(V(x - dir, y + 2f), V(x - dir, y), V(tipX, y + 0.5f), PenguinPalette.Beak);
                Triangle(V(x - dir, y - 1f), V(x - dir, y - 2.5f), V(tipX - dir * 1.5f, y - 1.5f), PenguinPalette.BeakShade);
                return;
            }
            Ellipse(x, y, 2.5f, 1.5f, PenguinPalette.BeakShade);
            Triangle(V(x - dir, y + 1.5f), V(x - dir, y - 1.5f), V(tipX, y - 0.5f), PenguinPalette.BeakShade);
            Ellipse(x, y + 0.5f, 2f, 1f, PenguinPalette.Beak);
            Triangle(V(x - dir, y + 1.5f), V(x - dir, y), V(tipX - dir, y), PenguinPalette.Beak);
        }

        /// <summary>目を1つだけ描く。rightSide は顔の右側の目（キリッ目・こまり目の形を反転する）</summary>
        public void Eye(float x, float y, EyeStyle eyes, bool rightSide)
        {
            if (!Front) return;
            if (eyes == EyeStyle.Smile)
            {
                SmileEye(x, y);
                return;
            }
            var pattern = EyePattern(eyes);
            int left = Mathf.RoundToInt(x - pattern[0].Length / 2f);
            int top = Mathf.RoundToInt(y + pattern.Length / 2f) - 1;
            StampEye(left, top, pattern, rightSide && FlipsOnRight(eyes));
        }

        public void Cheek(float x, float y)
        {
            if (!Front) return;
            Rect(Mathf.RoundToInt(x - CheekWidth / 2f), Mathf.RoundToInt(y), CheekWidth, CheekHeight, PenguinPalette.Cheek);
        }
    }
}
