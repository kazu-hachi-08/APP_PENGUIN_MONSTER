using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// フィールドのカメラ。主人公を画面の中央に追い、マップの端では止める（マップの外の何も無い所を映さない）。
    /// マップが画面より小さい向き（家の中など）は、マップを中央に置く
    /// </summary>
    public class FieldCamera : MonoBehaviour
    {
        // 主人公はマスの下端が基準なので、マスの中央を追うよう半マス上げる
        private const float TargetLift = 0.5f;
        // カメラはマップ（z=0）より手前に置かないと何も映らない
        private const float CameraZ = -10f;

        [SerializeField] private Camera _camera;
        [Tooltip("画面の縦に入るマスの数（の半分）。ダイパに近い近さにする")]
        [SerializeField] private float _halfTilesTall = 4.5f;
        [Tooltip("マップの外（家の中のまわりなど）の色。ダイパと同じく黒にする")]
        [SerializeField] private Color _outsideColor = Color.black;

        private Transform _target;
        private Vector2 _mapSize;

        public void Follow(Transform target, int mapWidth, int mapHeight)
        {
            _target = target;
            _mapSize = new Vector2(mapWidth, mapHeight);
            _camera.orthographicSize = _halfTilesTall;
            _camera.backgroundColor = _outsideColor;
            enabled = true;
            LateUpdate();
        }

        public void Stop()
        {
            enabled = false;
            _target = null;
        }

        private void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }
            float halfHeight = _camera.orthographicSize;
            float halfWidth = halfHeight * _camera.aspect;
            var target = _target.position;
            float x = Clamp(target.x, halfWidth, _mapSize.x);
            float y = Clamp(target.y + TargetLift, halfHeight, _mapSize.y);
            transform.position = new Vector3(x, y, CameraZ);
        }

        private static float Clamp(float value, float halfView, float mapLength)
        {
            if (mapLength <= halfView * 2f)
            {
                return mapLength / 2f;
            }
            return Mathf.Clamp(value, halfView, mapLength - halfView);
        }
    }
}
