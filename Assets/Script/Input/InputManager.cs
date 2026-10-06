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
        private static Subject<Unit> _headLight = new();
        private static Subject<Unit> _interact = new();

        public static Vector2 Move;
        public static Vector2 Look;
        public static Observable<Unit> Zoom => _zoom;
        public static Observable<Unit> HeadLight => _headLight;
        public static Observable<Unit> Interact => _interact;

        private static GameInputs _gameInputs;

        public void Awake()
        {
            _gameInputs = new();

            SubscribeEvent(_gameInputs.Player.Move, OnMove);
            SubscribeEvent(_gameInputs.Player.Look, OnLook);
            SubscribeEvent(_gameInputs.Player.Zoom, OnZoom);
            SubscribeEvent(_gameInputs.Player.HeadLight, OnHeadLight);
            SubscribeEvent(_gameInputs.Player.Interact, OnInteract);

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

        public void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        public void OnHeadLight(InputAction.CallbackContext context)
        {
            if(context.ReadValueAsButton())
                _headLight.OnNext(Unit.Default);
        }

        public void OnZoom(InputAction.CallbackContext context) 
        {
            if (context.ReadValueAsButton())
                _zoom.OnNext(Unit.Default);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
                _interact.OnNext(Unit.Default);
        }

    }
}
