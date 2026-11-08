using Cysharp.Threading.Tasks;
using InGame.Common;
using InGame.Gimmick;
using Input;
using R3;
using System;
using UnityEngine;
using UnityEngine.UI; // ★

namespace InGame.Player
{

    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private ObjectDetector _detector;
        [SerializeField] private PlayerControl _playerControl;

        [Header("インタラクト案内UI")] // ★
        [SerializeField] private Canvas _canvas;          // ★ 表示先のCanvas
        [SerializeField] private Image _promptImage;      // ★ Canvasの直下に置いたImage
        [SerializeField] private Vector2 _screenOffset = new(0f, 0f); // ★ 位置の微調整（ピクセル）

        public bool IgnoreInteract = false;
        public bool IsInteractStop = false;

        private ObjectFilter<IInteractable> _filter;
        private IInteractable _currentTarget; // ← 追加
        private Camera _worldCamera;      // ★ ワールドを映すカメラ
        private Camera _uiCamera;         // ★ Canvas用カメラ（Overlayならnull）
        private RectTransform _canvasRect; // ★


        private void Start()
        {
            _filter = new(_detector);
            InputManager.Interact.Subscribe(_ => Interact()).RegisterTo(destroyCancellationToken);

            // ★ UIの準備
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
                _promptImage.raycastTarget = false; // クリックを邪魔しない
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

            //インタラクトできるものを探す
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
            foreach (var interactable in _filter.FilteredObjects)
            {
                if (!interactable.CanInteract()) continue;
                return interactable;
            }
            return null;
        }

        private void UpdatePrompt()
        {
            if (_promptImage == null || _canvas == null) return;

            var target = (IgnoreInteract || IsInteractStop) ? null : FindInteractable();

            // 対象が変わったときだけ画像を差し替える
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

            // 表示中は毎フレーム位置を更新
            if (_promptImage.enabled && _currentTarget is SwitchInterface.IInteractPromptProvider p)
            {
                UpdatePromptPosition(p.PromptWorldPosition);
            }
        }

        // ★ ワールド座標 → Canvas上の座標
        private void UpdatePromptPosition(Vector3 worldPos)
        {
            if (_worldCamera == null) _worldCamera = Camera.main;
            if (_worldCamera == null) return;

            Vector3 screenPos = _worldCamera.WorldToScreenPoint(worldPos);

            // カメラの後ろにある場合は隠す
            if (screenPos.z < 0f)
            {
                _promptImage.enabled = false;
                return;
            }
            _promptImage.enabled = true;

            Vector2 screenPoint = (Vector2)screenPos + _screenOffset;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvasRect, screenPoint, _uiCamera, out Vector2 localPoint))
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