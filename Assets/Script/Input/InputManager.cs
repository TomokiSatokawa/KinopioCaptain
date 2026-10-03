using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// <summary>
    /// ì¸óÕä«óù
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        private static Subject<Unit> _zoom = new();

        public static Vector2 Look;
        public static Observable<Unit> Zoom => _zoom;

        private static GameInputs _gameInputs;

        public void Awake()
        {
            _gameInputs = new();

            SubscribeEvent(_gameInputs.Player.Look, OnLook);
            SubscribeEvent(_gameInputs.Player.Zoom, OnZoom);

            _gameInputs.Enable();
        }

        private void SubscribeEvent(InputAction action , Action<InputAction.CallbackContext> callback)
        {
            action.performed += callback;
            action.canceled += callback;
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

        public void OnZoom(InputAction.CallbackContext context) 
        {
            if (context.ReadValueAsButton())
                _zoom.OnNext(Unit.Default);
        }


    }
}
