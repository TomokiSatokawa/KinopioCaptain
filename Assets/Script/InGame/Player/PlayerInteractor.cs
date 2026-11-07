using System;
using Cysharp.Threading.Tasks;
using InGame.Common;
using InGame.Gimmick;
using UnityEngine;

namespace InGame.Player
{

    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private ObjectDetector _detector;
        [SerializeField] private PlayerControl _playerControl;

        public bool IgnoreInteract = false;
        public bool IsInteractStop = false;

        private ObjectFilter<IInteractable> _filter;

        private void Start()
        {
            _filter = new(_detector);

            //押された時にインタラクト関数を呼ぶ
            InputService.Player.Interact.OnPressed(Interact);
        }

        public void Interact()
        {
            if (IgnoreInteract) return;

            //インタラクトできるものを探す
            foreach (var interactable in _filter.FilteredObjects)
            {
                if (!interactable.CanInteract()) continue;

                interactable.Interact();
                Stillness(interactable).Forget();
                break;
            }
        }

        private async UniTask Stillness(IInteractable interactable)
        {
            //移動を無効化
            var token = _playerControl.IgnoreMoveInput.RegisterFlag();
            IsInteractStop = true;


            try
            {
                await UniTask.WaitForSeconds(interactable.InteractDuration, cancellationToken: destroyCancellationToken);
            }
            catch (OperationCanceledException)
            {
            }

            //移動を有効化
            _playerControl.IgnoreMoveInput.UnregisterFlag(token);
            IsInteractStop = false;
        }

        private void OnDestroy()
        {
            _filter?.Dispose();
        }
    }
}