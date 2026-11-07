using Cysharp.Threading.Tasks;
using InGame.Common;
using InGame.Gimmick;
using Input;
using R3;
using System;
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
                Stillness().Forget();
                break;
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