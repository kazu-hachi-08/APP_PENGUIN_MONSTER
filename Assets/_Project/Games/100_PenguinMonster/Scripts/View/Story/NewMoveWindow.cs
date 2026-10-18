using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// レベルアップ・進化で覚える技の説明。覚えるかどうか・どれを忘れるかを中身を見て決められるよう、
    /// 「覚えた！」「覚えたい……」の文と忘れる技の窓の間だけ出す。
    /// 今の技の長押しの説明（左）と見比べられるよう、右に置く
    /// </summary>
    public class NewMoveWindow : MonoBehaviour
    {
        private const string Heading = "<color=#F5C542>新しい技</color>　";

        [SerializeField] private Text _text;

        public void Show(MoveDefinition move)
        {
            gameObject.SetActive(true);
            _text.SetFitted(Heading + MonsterTexts.MoveHint(move));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
