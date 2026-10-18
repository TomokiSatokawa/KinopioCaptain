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

        private ObjectFilter<IInteractable> _filter;
        private void Start()
        {
            InputManager.Interact.Subscribe(_ => Interact()).RegisterTo(destroyCancellationToken);
            _filter = new(_detector);
        }

        public void Interact()
        {
            Debug.Log("インタラクト");
            foreach(var a in _filter.Fillters)
            {
                Debug.Log(a);
            }
        }
    }
}