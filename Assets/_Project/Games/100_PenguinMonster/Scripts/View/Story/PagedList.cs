using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 1ページずつ見せる一覧（バッグ・よろず屋）で共通の、ページの数え方と ← → の出し方。
    /// PC は ← → のキーでもページを送る（上下はボタンの選択に使う）
    /// </summary>
    public static class PagedList
    {
        public static int PageCount(int count, int pageSize)
        {
            return Mathf.Max(1, (count + pageSize - 1) / pageSize);
        }

        /// <summary>ページが1つしか無ければ ← → は出さない。端では押せなくする</summary>
        public static void ShowPageButtons(Button prev, Button next, int page, int pageCount)
        {
            bool paged = pageCount > 1;
            prev.gameObject.SetActive(paged);
            next.gameObject.SetActive(paged);
            prev.interactable = page > 0;
            next.interactable = page < pageCount - 1;
        }

        /// <summary>このフレームに押された ← は -1、→ は 1、どちらも無ければ 0</summary>
        public static int KeyDelta()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return 0;
            }
            if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                return -1;
            }
            return keyboard.rightArrowKey.wasPressedThisFrame ? 1 : 0;
        }
    }
}
