using InGame.Common;
using UnityEngine;
using UnityEngine.Playables;

namespace InGame.Item
{
    public class Star : MonoBehaviour, ICollectible
    {
        [SerializeField] private Transform _mesh;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private PlayableAsset _asset;
        [SerializeField] private Transform _cutInCameraPosition;
        [SerializeField] private PlayableDirector _playableDirector;
        [SerializeField] private CutInPlayer _cutInPlayer;

        private bool _isRotation = true;  
        public string Name => "Star";

        public void Update()
        {
            if (!_isRotation) return;
            _mesh.rotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
        }

        public void OnPlayerTouch()
        {
            _isRotation = false;
            _playableDirector.Play(_asset);
            _collider.enabled = false;
        }

        public void StartCutIn()
        {
            _cutInPlayer.PlayCutIn(_asset);
        }

        public void StartFollow()
        {
            _cutInPlayer.StartFollowCamera(_cutInCameraPosition);
            _cutInPlayer.LookDirection(-this.transform.forward);
            _cutInPlayer.SetPosition(this.transform.position);
        }
    }
}
