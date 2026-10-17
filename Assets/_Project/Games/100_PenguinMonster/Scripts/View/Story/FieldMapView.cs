using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// マップを Tilemap に並べる。マップの位置は上の行から数えるが、Unity は上ほど y が大きいので、ここで上下を入れ替える。
    /// 1マス＝1ワールド単位で、マップの左下のマスの左下が原点
    /// </summary>
    public class FieldMapView : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;

        private FieldMap _map;

        public void Draw(FieldMap map)
        {
            _map = map;
            _tilemap.ClearAllTiles();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var pos = new GridPos(x, y);
                    _tilemap.SetTile(ToCell(pos), FieldTileArt.Get(map.TileAt(pos)));
                }
            }
        }

        /// <summary>そのマスの下端の中央（主人公の足元）のワールド座標</summary>
        public Vector3 FeetPosition(GridPos pos)
        {
            var cell = ToCell(pos);
            return new Vector3(cell.x + 0.5f, cell.y, 0f);
        }

        private Vector3Int ToCell(GridPos pos) => new Vector3Int(pos.X, _map.Height - 1 - pos.Y, 0);
    }
}
