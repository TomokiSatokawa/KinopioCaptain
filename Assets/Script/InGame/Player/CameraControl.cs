using Common;
using DG.Tweening;
using Input;
using R3;
using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// カメラ制御
    /// </summary>
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Transform _cameraOrigin;
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _playerPosition;
        [Header("Rotation")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private FloatRange _verticalRotationRange;
        [SerializeField] private float _quickRotationSpeed;
        [SerializeField, Range(0, 180)] private float _quickRotationAngle;
        [Header("Position")]
        [SerializeField] private FloatRange _originPositionRangeZ;
        [SerializeField] private float _followSmoothTime;

        [Header("Zoom")]
        [SerializeField] private float[] _zoomAmount;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private Ease _zoomEase;

        [Header("Shake")]
        [SerializeField] private float _shakeStrength = 0.3f;   // 横揺れの強さ
        [SerializeField] private float _shakeDuration = 0.4f;
        [SerializeField] private int _shakeVibrato = 15;        // 揺れる回数の目安
        [SerializeField, Range(0, 1)] private float _shakeElasticity = 0.5f;

        private Tween _shakeTween;

        private int _currentZoomIndex;
        private Tween _zoomTween;
        private Tween _quickRotationTween;
        private Vector3 _currentEuler = Vector3.zero;

        private void Start()
        {
            InputManager.Zoom.Subscribe(_ => SwitchZoom());
            InputManager.CameraRotationLeft.Where(x => x).Subscribe(_ => QuickRotation(1));
            InputManager.CameraRotationRight.Where(x => x).Subscribe(_ => QuickRotation(-1));
        }

        private void Update()
        {
            CameraRotation();
            CameraPosition();
        }

        private void CameraRotation()
        {
            var input = InputManager.Look;

            input.x *= _horizontalSpeed * Time.deltaTime;
            input.y *= _verticalSpeed * Time.deltaTime;

            var euler = _currentEuler;

            euler.x = _verticalRotationRange.Clamp(_currentEuler.x + input.y);

            euler.y = _currentEuler.y + input.x;

            _cameraOrigin.rotation = Quaternion.Euler(euler);

            _currentEuler = euler;
        }

        private void CameraPosition()
        {
            var pos = _cameraOrigin.position;
            var clampedPlayerPos = _originPositionRangeZ.Clamp(_playerPosition.position.z);
            pos.z = Mathf.Lerp(pos.z, clampedPlayerPos, Time.deltaTime * _followSmoothTime);
            _cameraOrigin.position = pos;
        }

        private void SwitchZoom()
        {
            //ズーム中はズームしない
            if (_zoomTween?.active ?? false) return;

            //次のIndexにする
            _currentZoomIndex = (_currentZoomIndex + 1) % _zoomAmount.Length;

            _zoomTween = _camera.DOLocalMoveZ(_zoomAmount[_currentZoomIndex], _zoomDuration).SetEase(_zoomEase);
        }


        private void QuickRotation(float amount)
        {
            //回転中は回転させない
            if (_quickRotationTween?.active ?? false) return;

            float startAngle = _currentEuler.y;
            float endAngle = startAngle + amount * _quickRotationAngle;

            _quickRotationTween = DOVirtual.Float(
                startAngle,
                endAngle,
                _quickRotationSpeed,
                angle =>
                {
                    _currentEuler.y = angle;
                    _cameraOrigin.rotation = Quaternion.Euler(_currentEuler);
                });
        }

        /// <summary>横揺れのカメラシェイク(Inspectorの値を使用)</summary>
        public void Shake(float duration = 0.3f)
        {
            // 前回のシェイクが残っていれば元の位置に戻してから再開する
            _shakeTween?.Complete();

            // カメラのローカルX方向(画面の横方向)に揺らす
            _shakeTween = _camera.DOPunchPosition(Vector3.right * _shakeStrength, duration,Mathf.RoundToInt(_shakeVibrato * duration), _shakeElasticity);
        }

        private void OnDestroy()
        {
            _shakeTween?.Kill();
        }
    }
}