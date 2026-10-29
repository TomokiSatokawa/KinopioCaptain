using InGame.Common;
using InGame.Gimmick;
using Input;
using R3;
using UnityEngine;

namespace InGame.Player
{

    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private ObjectDetector _detector;

        public bool IgnoreInteract = false;

        private ObjectFilter<IInteractable> _filter;

        private void Start()
        {
            _filter = new(_detector);
            InputManager.Interact.Subscribe(_ => Interact()).RegisterTo(destroyCancellationToken);
        }

        public void Interact()
        {
            if (IgnoreInteract) return;

            //インタラクトできるものを探す
            foreach (var interactable in _filter.FilteredObjects)
            {
                if (!interactable.CanInteract()) continue;

                interactable.Interact();
                break;
            }
        }

        private void OnDestroy()
        {
            _filter?.Dispose();
        }
    }
}