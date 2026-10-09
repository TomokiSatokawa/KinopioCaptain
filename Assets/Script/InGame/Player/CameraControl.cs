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
        [Header("Rotation")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _maxVerticalRotation;
        [SerializeField] private float _minVerticalRotation;
        [SerializeField] private float _quickRotationSpeed;
        [SerializeField,Range(0,180)] private float _quickRotationAngle;

        [Header("Zoom")]
        [SerializeField] private float[] _zoomAmount;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private Ease _zoomEase;

        private int _currentZoomIndex;
        private Tween _zoomTween;

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
        }

        private void CameraRotation()
        {
            var input = InputManager.Look;

            input.x *= _horizontalSpeed * Time.deltaTime;
            input.y *= _verticalSpeed * Time.deltaTime;

            var euler = _currentEuler;

            euler.x = Mathf.Clamp(_currentEuler.x + input.y, _minVerticalRotation, _maxVerticalRotation);

            euler.y = _currentEuler.y + input.x;

            _cameraOrigin.rotation = Quaternion.Euler(euler);

            _currentEuler = euler;
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
            _cameraOrigin.DORotate(Vector3.up * amount * _quickRotationAngle, _quickRotationSpeed);
        }
    }
}