using System;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// タイトルから開く画面（ずかん・タイプ相性表・あそびかた・設定）の共通部分。「もどる」でタイトルへ帰る。
    /// 中身が固定の画面（タイプ相性表）はこのまま使い、動きのある画面は継承して OnShow / OnHide を書く
    /// </summary>
    public class SubMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _backButton;

        private Action _onClose;

        protected virtual void Awake()
        {
            _backButton.onClick.AddListener(Close);
        }

        public void Show(Action onClose)
        {
            _onClose = onClose;
            gameObject.SetActive(true);
            OnShow();
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        private void Close()
        {
            BattleSounds.Play(BattleSound.Cursor);
            OnHide();
            gameObject.SetActive(false);
            _onClose?.Invoke();
        }
    }
}
