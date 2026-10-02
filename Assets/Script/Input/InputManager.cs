using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// <summary>
    /// ì¸óÕä«óù
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public static Vector2 Look;

        private static GameInputs _gameInputs;

        public void Awake()
        {
            _gameInputs = new();

            _gameInputs.Player.Look.performed += OnLook;
            _gameInputs.Player.Look.canceled += OnLook;

            _gameInputs.Enable();
        }

        private void OnDestroy()
        {
            _gameInputs.Disable();
            _gameInputs.Dispose();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }
    }
}
