using Input;
using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// ÉJÉÅÉâêßå‰
    /// </summary>
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Transform _cameraOrigin;
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _maxVerticalRotation;
        [SerializeField] private float _minVerticalRotation;

        private Vector3 _currentEuler = Vector3.zero;

        private void Update()
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
    }
}