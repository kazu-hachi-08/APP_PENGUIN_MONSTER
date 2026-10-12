using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 画面の背景の Image に夜空の絵をはめる。タイトル以外の画面もタイトルと同じ世界に見せるため。
    /// 絵をアセットにしないので、シーン上では白い四角のまま
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class NightSky : MonoBehaviour
    {
        // 画面ごとに作ると同じテクスチャが何枚もできるので、1枚を使い回す
        private static Sprite _sprite;

        public static Sprite Sprite => _sprite != null ? _sprite : _sprite = BackdropArt.CreateNightSky();

        private void Awake()
        {
            GetComponent<Image>().sprite = Sprite;
        }
    }
}
