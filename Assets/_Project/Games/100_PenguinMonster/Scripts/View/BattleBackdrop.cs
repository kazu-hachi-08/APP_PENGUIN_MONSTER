using PenguinMonster.View.Art;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 背景の Image に、実行時に作った空・地面・台座の絵をはめる。絵をアセットにしないので、シーン上では白い四角のまま
    /// </summary>
    public class BattleBackdrop : MonoBehaviour
    {
        [SerializeField] private Image _sky;
        [SerializeField] private Image _ground;
        [SerializeField] private Image[] _platforms;

        private void Awake()
        {
            _sky.sprite = BackdropArt.CreateSky();
            _ground.sprite = BackdropArt.CreateGround();
            var platform = BackdropArt.CreatePlatform();
            foreach (var image in _platforms)
            {
                image.sprite = platform;
            }
        }
    }
}
