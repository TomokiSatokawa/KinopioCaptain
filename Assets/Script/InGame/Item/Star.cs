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
        [SerializeField] private CutInController _cutInPlayer;

        private bool _isRotating  = true;  
        public string Name => "Star";

        private void Update()
        {
            if (!_isRotating ) return;
            _mesh.rotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
        }

        public void OnPlayerTouch()
        {
            _isRotating  = false;
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
