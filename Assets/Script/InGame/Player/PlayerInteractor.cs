using Cysharp.Threading.Tasks;
using InGame.Common;
using InGame.Gimmick;
using Input;
using R3;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Player
{

    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private ObjectDetector _detector;
        [SerializeField] private PlayerControl _playerControl;

        [Header("インタラクト案内UI")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _promptImage;

        [Header("吹き出しの大きさ（距離で拡大縮小）")]
        [SerializeField] private float _referenceDistance = 5f;
        [SerializeField] private float _minScale = 0.3f;
        [SerializeField] private float _maxScale = 2f;

        public bool IgnoreInteract = false;
        public bool IsInteractStop = false;

        private ObjectFilter<IInteractable> _filter;
        private IInteractable _currentTarget;
        private Camera _worldCamera;
        private Camera _uiCamera;
        private RectTransform _canvasRect;


        private void Start()
        {
            _filter = new(_detector);
            InputManager.Interact.Subscribe(_ => Interact()).RegisterTo(destroyCancellationToken);

            _worldCamera = Camera.main;
            if (_canvas != null)
            {
                _canvasRect = _canvas.transform as RectTransform;
                _uiCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null
                    : _canvas.worldCamera;
            }
            if (_promptImage != null)
            {
                _promptImage.raycastTarget = false;
                _promptImage.rectTransform.pivot = new Vector2(0.5f, 0f);
                _promptImage.enabled = false;
            }
        }

        private void Update()
        {
            UpdatePrompt();
        }

        public void Interact()
        {
            if (IgnoreInteract) return;

            foreach (var interactable in _filter.FilteredObjects)
            {
                if (!interactable.CanInteract()) continue;

                interactable.Interact();
                Stillness().Forget();
                break;
            }
        }

        private IInteractable FindInteractable()
        {
            IInteractable best = null;
            float bestSqr = float.MaxValue;

            foreach (var interactable in _filter.FilteredObjects)
            {
                if (!interactable.CanInteract()) continue;

                float sqr = interactable is SwitchInterface.IInteractPromptProvider p
                    ? (p.PromptWorldPosition - transform.position).sqrMagnitude
                    : float.MaxValue;

                if (best == null || sqr < bestSqr)
                {
                    best = interactable;
                    bestSqr = sqr;
                }
            }
            return best;
        }

        private void UpdatePrompt()
        {
            if (_promptImage == null || _canvas == null) return;

            var target = (IgnoreInteract || IsInteractStop) ? null : FindInteractable();

            if (!ReferenceEquals(target, _currentTarget))
            {
                _currentTarget = target;

                if (target is SwitchInterface.IInteractPromptProvider provider && provider.PromptSprite != null)
                {
                    _promptImage.sprite = provider.PromptSprite;
                    _promptImage.enabled = true;
                }
                else
                {
                    _promptImage.enabled = false;
                }
            }

            if (_promptImage.enabled && _currentTarget is SwitchInterface.IInteractPromptProvider p)
            {
                UpdatePromptPosition(p.PromptWorldPosition);
            }
        }

        private void UpdatePromptPosition(Vector3 worldPos)
        {
            if (_worldCamera == null) _worldCamera = Camera.main;
            if (_worldCamera == null) return;

            Vector3 screenPos = _worldCamera.WorldToScreenPoint(worldPos);

            if (screenPos.z <= 0f)
            {
                _promptImage.enabled = false;
                return;
            }
            _promptImage.enabled = true;

            float scale = 1f;
            if (!_worldCamera.orthographic)
            {
                scale = Mathf.Clamp(_referenceDistance / screenPos.z, _minScale, _maxScale);
            }
            _promptImage.rectTransform.localScale = Vector3.one * scale;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvasRect, screenPos, _uiCamera, out Vector2 localPoint))
            {
                _promptImage.rectTransform.localPosition = localPoint;
            }
        }

        private async UniTask Stillness()
        {
            _playerControl.IsMovingInteract = false;
            IsInteractStop = true;


            try
            {
                await UniTask.WaitForSeconds(0.1f, cancellationToken: destroyCancellationToken);
            }
            catch (OperationCanceledException)
            {
            }

            _playerControl.IsMovingInteract = true;
            IsInteractStop = false;
        }

        private void OnDestroy()
        {
            _filter?.Dispose();
        }
    }
}