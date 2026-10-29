using InGame.Player;
using UnityEngine;

namespace InGame.Gimmick
{
    public class TurnipPlant : PlantBase, IInteractable
    {
        [SerializeField] private TurnipController _turnipPrefab;
        [SerializeField] private PlayerEquipment _playerEquipment;
        [SerializeField] private PlayerInteractor _interactor;
        private bool _isUsed = false;

        private void Start()
        {
            if(_playerEquipment == null)
                _playerEquipment = FindAnyObjectByType<PlayerEquipment>();

            if(_interactor == null)
                _interactor = FindAnyObjectByType<PlayerInteractor>();
        }

        public bool CanInteract()
        {
            return !_isUsed;
        }

        public void Interact()
        {
            var turnip = Instantiate(_turnipPrefab);
            turnip.Initialize(ThrowAction);
            _playerEquipment.Equip(EquipmentPosition.Overhead, turnip.gameObject);
            _interactor.IgnoreInteract = true;
            SetState(false);
            _isUsed = true;
        }

        private void ThrowAction()
        {
            _playerEquipment.Remove(EquipmentPosition.Overhead);
            _interactor.IgnoreInteract = false;
        }
    }
}
